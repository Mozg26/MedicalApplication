using AutoMapper;
using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseAbstractions.DatabaseOperations.Abstractions;
using DatabaseAbstractions.LocalDatabase.Abstractions;
using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseAbstractions.Models.Enums;
using Extensions.Enums;
using Extensions.Models;
using Extensions.Tools;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;

namespace DatabaseAbstractions.DatabaseOperations
{
    /// <summary>
    /// Базовый класс операции сохранения изменений в базу данных.
    /// </summary>
    /// <typeparam name="ContextType">Интерфейс контекста базы данных.</typeparam>
    public class DatabaseSaverBase<ContextType> : IDatabaseSaver
        where ContextType : DatabaseContext.DatabaseContext
    {
        /// <summary>
        /// Потокобезопасная очередь для сохранения изменений в базу.
        /// </summary>
        protected readonly ConcurrentQueue<CacheChangeModel> SavingQueue = new();

        /// <summary>
        /// Потокобезопасная очередь для процессов, завершившихся с ошибкой.
        /// </summary>
        protected readonly List<CacheChangeModel> ProblemProcesses = [];

        protected readonly CacheConverter fingerprintConverter;

        protected readonly IContextFactory<ContextType> ContextFactory;
        protected readonly ILocalDatabaseModule LocalDatabaseModule;
        protected readonly ILogger _logger;

        /// <summary>
        /// Задача сохранения в базу.
        /// </summary>
        private Task SavingTask { get; set; } = null!;

        private bool queueIsWorking = false;

        private readonly string _className;

        public DatabaseSaverBase(ILocalDatabaseModule localDatabase, IContextFactory<ContextType> contextFactory, IMapper mapper, ILogger logger)
        {
            _className = GetType().Name;

            _logger = logger;

            ProblemProcesses = [];
            SavingQueue = new();

            fingerprintConverter = new(mapper);

            ContextFactory = contextFactory;
            LocalDatabaseModule = localDatabase;
        }

        /// <summary>
        /// Метод сохранения сущности в базу данных.
        /// </summary>
        /// <param name="databaseChangeModel">Сущность, которую необходимо сохранить.</param>
        /// <returns>Ответ хранилища данных об успешном или неуспешном сохранении в базу данных.</returns>
        protected DatabaseResponse SaveToDatabase(CacheChangeModel databaseChangeModel)
        {
            using var context = ContextFactory.CreateContext();

            var transactionId = Guid.NewGuid();

            switch (databaseChangeModel.DatabaseChangeType)
            {
                case DatabaseChangeType.Add:

                    DatabaseResponse createResponse = CreateEntity(databaseChangeModel.CacheEntity, context, transactionId);

                    if (createResponse.IsCorrect)
                        createResponse = DatabaseSaverHelper.SaveValues(() => context.SaveChanges());

                    return createResponse;

                case DatabaseChangeType.Save:

                    DatabaseResponse saveResponse = SaveEntity(databaseChangeModel.CacheEntity, context, transactionId);

                    if (saveResponse.IsCorrect)
                        saveResponse = DatabaseSaverHelper.SaveValues(() => context.SaveChanges());

                    return saveResponse;

                case DatabaseChangeType.Update:

                    DatabaseResponse updateResponse = UpdateEntity(databaseChangeModel.CacheEntity, context, transactionId);

                    if (updateResponse.IsCorrect)
                        updateResponse = DatabaseSaverHelper.SaveValues(() => context.SaveChanges());

                    return updateResponse;

                case DatabaseChangeType.Delete:

                    DatabaseResponse deleteResponse = DeleteEntity(databaseChangeModel.CacheEntity, context, transactionId);
                    if (deleteResponse.IsCorrect)
                        deleteResponse = DatabaseSaverHelper.SaveValues(() => context.SaveChanges());

                    return deleteResponse;

                default:
                    string errorString = "Неизвестный тип изменения базы данных.";
                    return new DatabaseResponse(_className, errorString, ResponseType.LogicError, false, new UnprocessedTypeException(errorString));
            }
        }

        #region IDatabaseSaver

        public void FillLocalDatabaseData()
        {
            var localDatabaseEntities = LocalDatabaseModule.GetAllEntities();

            if (localDatabaseEntities?.Count > 0)
                foreach (var entity in localDatabaseEntities)
                    AddToQueue(entity);
        }

        public List<CacheChangeModel> GetProblemProcesses()
        {
            return ProblemProcesses;
        }

        public void TryRepeateProblemProcesses()
        {
            AddToQueue(ProblemProcesses);
        }

        public DatabaseResponse SaveModel(CacheChangeModel model)
        {
            LocalDatabaseModule.Save(model);
            _logger.LogDebug($"[SafeSaver : SaveEntity] Сущность {model}, guid: {model.TransactionId} добавлена в локальную базу");
            return AddToQueue(model);
        }

        #endregion

        #region WorkWithSavingQueue

        /// <summary>
        /// Добавление сущности в очередь сохранения.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо сохранить.</param>
        /// <returns>Ответ об успешном или неуспешном добавлении в очередь сохранения.</returns>
        protected DatabaseResponse AddToQueue(CacheChangeModel entity)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), entity.TransactionId);

            _logger.LogDebug($"[{logHeader}] В очередь сохранения добавляется сущность типа {entity.GetType().Name} для изменения типа {entity.DatabaseChangeType}.");

            lock (SavingQueue)
            {
                SavingQueue.Enqueue(entity);
                _logger.LogDebug($"[{logHeader}] В очередь сохранения добавлена сущность типа {entity.GetType().Name} для изменения типа {entity.DatabaseChangeType}.");

                if (!queueIsWorking)
                {
                    _logger.LogDebug($"[{logHeader}] Очередь сохранения не была в работе. Включение очереди. " +
                        $"Начало сохранения сущности типа {entity.GetType().Name} для изменения типа {entity.DatabaseChangeType}.");
                    queueIsWorking = true;
                    HandleQueue();
                }
                else
                    _logger.LogDebug($"[{logHeader}] Очередь сохранения в работе.");

                return new DatabaseResponse(_className, "Успешное добавление в очередь сохранения.");
            }
        }

        /// <summary>
        /// Добавление сущности в очередь сохранения.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо сохранить.</param>
        /// <returns>Ответ об успешном или неуспешном добавлении в очередь сохранения.</returns>
        protected DatabaseResponse AddToQueue(List<CacheChangeModel> entities)
        {
            var logHeaderBase = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            _logger.LogDebug($"[{logHeaderBase}] В очередь сохранения добавляются сущности для сохранения в количестве {entities.Count} штук.");

            lock (SavingQueue)
            {
                foreach (CacheChangeModel entity in entities)
                {
                    SavingQueue.Enqueue(entity);
                    var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), entity.TransactionId);
                    _logger.LogDebug($"[{logHeader}] В очередь сохранения добавлена сущность типа {entity.GetType().Name} для изменения типа {entity.DatabaseChangeType}.");
                    entities.Remove(entity);
                }

                entities.Clear();

                if (!queueIsWorking)
                {
                    _logger.LogDebug($"[{logHeaderBase}] Очередь сохранения не была в работе. Включение очереди. Начало сохранения сущностей в количестве {entities.Count} штук.");
                    queueIsWorking = true;
                    HandleQueue();
                }
                else
                    _logger.LogDebug($"[{logHeaderBase}] Очередь сохранения в работе.");

                return new DatabaseResponse(_className, "Успешное добавление в очередь сохранения.");
            }
        }

        /// <summary>
        /// Обработка очереди сохранения.
        /// </summary>
        /// <exception cref="InvalidOperationException">Если невозможно обработать очередь.</exception>
        protected void HandleQueue()
        {
            _logger.LogInformation($"[SafeSaver: HandleQueue] Начало разбора очереди сохранения в БД");

            SavingTask = new Task(() =>
            {
                try
                {
                    while (SavingQueue.Count != 0)
                    {
                        if (SavingQueue.TryDequeue(out CacheChangeModel? entity))
                        {
                            var response = SaveToDatabase(entity);
                            //Цикл переподключений к базе в случае неудачи сохранения
                            while (!response.IsCorrect && response.ResponseType == ResponseType.DatabaseAccessError)
                            {
                                _logger.LogWarning($"[SafeSaver: HandleQueue] Ошибка доступа к бд, повторная попытка сохранения...");
                                _logger.LogCritical($"[SafeSaver: HandleQueue] {response.GetErrorString()}");
                                Thread.Sleep(5000);
                                response = SaveToDatabase(entity);
                            }

                            if (!response.IsCorrect && response.ResponseType == ResponseType.LogicError)
                            {
                                _logger.LogWarning($"[SafeSaver: HandleQueue LogicError] Сущность {JsonConvert.SerializeObject(entity)} Возникла неизвестная ошибка {response.Description}");
                                ProblemProcesses.Add(entity);
                            }
                            else
                            {
                                LocalDatabaseModule.Delete(entity);
                                _logger.LogDebug($"[SafeSaver: HandleQueue] Сущность {entity}, guid: {entity.TransactionId} удалена из локальной базы");
                            }
                        }
                        else
                            throw new InvalidOperationException("[SafeSaver: HandleQueue] Невозможно извлечь элемент из очереди!");
                    }

                    queueIsWorking = false;

                    _logger.LogDebug($"[SafeSaver: HandleQueue] Элемент успешно обработан!");
                }
                catch (Exception exception) when (exception is InvalidOperationException || exception is SqlException)
                {
                    queueIsWorking = false;
                    _logger.LogCritical($"[SafeSaver: HandleQueue] {exception.Message} {exception.StackTrace}");
                    return;
                }
            });

            SavingTask.Start();
        }

        #endregion

        #region WorkWithEntity

        private DatabaseResponse CreateEntity(CacheEntity fingerprintEntity, ContextType context, Guid transactionId)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), transactionId);

            if (IsFingerprintEntityExist(fingerprintEntity, logHeader, out DatabaseResponse databaseResponse))
                return databaseResponse;

            _logger.LogInformation($"[{logHeader}] Создание сущности {fingerprintEntity.GetType()}:\n{JsonConvert.SerializeObject(fingerprintEntity)}");

            var entity = fingerprintConverter.ConvertToEntity(fingerprintEntity);

            var saveResponse = DatabaseSaverHelper.SaveValues(() =>
            {
                var type = entity.GetType();

                var methodInfo = context.GetType().GetMethod("AddEntity");

                if (methodInfo == null)
                {
                    _logger.LogError($"[{logHeader}] Искомый метод AddEntity для типа {type.Name} не найден.");
                    return;
                }

                var method = methodInfo.MakeGenericMethod(type);

                method?.Invoke(context, [entity]);
            });

            return saveResponse;
        }

        private DatabaseResponse SaveEntity(CacheEntity fingerprintEntity, ContextType context, Guid transactionId)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), transactionId);

            if (IsFingerprintEntityExist(fingerprintEntity, logHeader, out DatabaseResponse databaseResponse))
                return databaseResponse;

            _logger.LogInformation($"[{logHeader}] Сохранение сущности {fingerprintEntity.GetType()}:\n{JsonConvert.SerializeObject(fingerprintEntity)}");

            var saveModel = fingerprintConverter.ConvertToEntity(fingerprintEntity);

            var response = GetEntityById(context, saveModel);

            if (!response.IsCorrect)
                return response;

            if (response.Content != default)
                return UpdateEntity(fingerprintEntity, context, transactionId);

            return CreateEntity(fingerprintEntity, context, transactionId);
        }

        private DatabaseResponse UpdateEntity(CacheEntity fingerprintEntity, ContextType context, Guid transactionId)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), transactionId);

            if (IsFingerprintEntityExist(fingerprintEntity, logHeader, out DatabaseResponse databaseResponse))
                return databaseResponse;

            _logger.LogInformation($"[{logHeader}] Обновление сущности {fingerprintEntity.GetType()}:\n{JsonConvert.SerializeObject(fingerprintEntity)}");

            var updateModel = fingerprintConverter.ConvertToEntity(fingerprintEntity);

            var response = GetEntityById(context, updateModel);

            if (!response.IsCorrect)
                return response;

            var entity = response.Content;

            if (entity == null)
            {
                string message = $"Обновление сущности {fingerprintEntity.GetType()} завершено с ошибкой: не найдена соответствующая запись в БД!";
                _logger.LogInformation($"[{logHeader}] {message}");
                return new DatabaseResponse($"{_className}", message, ResponseType.EntityNotExist, false, new ObjectNotFoundException(message));
            }

            entity.Update(updateModel);

            var saveResponse = DatabaseSaverHelper.SaveValues(() =>
            {
                var type = entity.GetType();
                var methodInfo = context.GetType().GetMethod("UpdateEntity");

                if (methodInfo == null)
                {
                    _logger.LogError($"[{logHeader}] Искомый метод UpdateEntity для типа {type.Name} не найден.");
                    return;
                }

                var method = methodInfo.MakeGenericMethod(type);

                method?.Invoke(context, [entity]);
            });

            return saveResponse;
        }

        private DatabaseResponse DeleteEntity(CacheEntity fingerprintEntity, ContextType context, Guid transactionId)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), transactionId);

            if (IsFingerprintEntityExist(fingerprintEntity, logHeader, out DatabaseResponse databaseResponse))
                return databaseResponse;

            _logger.LogInformation($"[{logHeader}] Удаление сущности {fingerprintEntity.GetType()}:\n{JsonConvert.SerializeObject(fingerprintEntity)}");

            var deleteModel = fingerprintConverter.ConvertToEntity(fingerprintEntity);
            var response = GetEntityById(context, deleteModel);

            if (!response.IsCorrect)
                return response;

            var entity = response.Content;

            if (entity == null)
            {
                string message = $"Обновление сущности {fingerprintEntity.GetType()} завершено с ошибкой: не найдена соответствующая запись в БД!";
                _logger.LogInformation($"[{logHeader}] {message}");
                return new DatabaseResponse($"{_className}", message, ResponseType.EntityNotExist, false, new ObjectNotFoundException(message));
            }

            _logger.LogInformation($"[{logHeader}] Удаление сущности {entity.GetType()}:\n{JsonConvert.SerializeObject(entity)}");

            var saveResponse = DatabaseSaverHelper.SaveValues(() =>
            {
                var type = entity.GetType();
                var methodInfo = context.GetType().GetMethod("DeleteEntity");

                if (methodInfo == null)
                {
                    _logger.LogError($"[{logHeader}] Искомый метод DeleteEntity для типа {type.Name} не найден.");
                    return;
                }

                var method = methodInfo.MakeGenericMethod(type);

                method?.Invoke(context, [entity]);
            });

            return saveResponse;
        }

        #endregion

        #region HelpMethods
        protected DatabaseResponse<BaseEntity> GetEntityById(ContextType context, BaseEntity reference)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            var response = context.GetTable(reference.GetType());

            if (!response.IsCorrect)
                return DatabaseResponse<BaseEntity>.IncorrectResponse(logHeader, response.Description, response.ResponseType, response.Ex);

            var dbSet = response.Content;

            if (dbSet == default)
            {
                string errorMessage = $"Таблица типа {reference.GetType().Name} в контексте {typeof(ContextType).Name} не найдена.";
                return DatabaseResponse<BaseEntity>.IncorrectResponse(logHeader, errorMessage, ResponseType.TableNotExist);
            }

            var entity = dbSet.FirstOrDefault(e => e.Id == reference.Id);

            if (entity == null)
            {
                string errorMessage = $"Сущность типа {reference.GetType().Name} не найдена.";
                return DatabaseResponse<BaseEntity>.IncorrectResponse(logHeader, errorMessage, ResponseType.EntityNotExist);
            }

            return DatabaseResponse<BaseEntity>.CorrectResponse(entity, logHeader);
        }

        protected bool IsFingerprintEntityExist(CacheEntity? fingerprintEntity, string logHeader, out DatabaseResponse databaseResponse)
        {
            if (fingerprintEntity == default || fingerprintEntity.IsIdNull())
            {
                string message = $"Создание сущности {fingerprintEntity?.GetType()} со значением NULL проигнорировано!";
                _logger.LogInformation($"[{logHeader}] {message}");
                databaseResponse = new DatabaseResponse($"{_className}", message, ResponseType.LogicError, false, new ObjectNotFoundException(message));
                return true;
            }

            databaseResponse = DatabaseResponse.CorrectResponse(_className);
            return false;
        }

        #endregion
    }
}
