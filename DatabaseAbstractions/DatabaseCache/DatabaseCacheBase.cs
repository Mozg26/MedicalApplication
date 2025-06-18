using AutoMapper;
using DatabaseAbstractions.DatabaseCache.Abstractions;
using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseAbstractions.DatabaseOperations;
using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseAbstractions.Models.Enums;
using Extensions.Enums;
using Extensions.Models;
using Extensions.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Extensions.Converters;

namespace DatabaseAbstractions.DatabaseCache
{
    /// <summary>
    /// Абстрактный класс отпечатка базы данных.
    /// </summary>
    /// <typeparam name="ContextType">Тип контекста базы данных.</typeparam>
    public abstract class DatabaseCacheBase<ContextType> : IDatabaseCache
        where ContextType : DatabaseContext.DatabaseContext
    {
        /// <summary>
        /// Фабрика контекста базы данных.
        /// </summary>
        protected readonly IContextFactory<ContextType> _dbContextFactory;

        /// <summary>
        /// Потокобезопасная коллекция наборов отпечатка базы данных.
        /// </summary>
        protected readonly ConcurrentDictionary<Type, CacheSet<CacheEntity>> _fingerprintSets;

        /// <summary>
        /// Конвертер с методами трансформации объектов БД в кэш-сущности и обратно
        /// </summary>
        protected readonly CacheConverter fingerprintConverter;

        /// <summary>
        /// Имя класса.
        /// </summary>
        private readonly string _className;

        /// <summary>
        /// Логгер.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Конструктор абстрактного класса отпечатка базы данных.
        /// </summary>
        /// <param name="dbContextFactory">Интерфейс фабрики контекста базы данных.</param>
        /// <param name="logger">Логгер.</param>
        public DatabaseCacheBase(IContextFactory<ContextType> dbContextFactory, IMapper mapper, ILogger logger)
        {
            _className = GetType().Name;

            _logger = logger;

            _dbContextFactory = dbContextFactory;

            fingerprintConverter = new CacheConverter(mapper);

            _fingerprintSets = [];

        }

        public virtual async Task<DatabaseResponse> Fill()
        {
            var logHeader = LogHelper.GetLogHeaderForAsyncMethods(_className);

            _logger.LogInformation($"[{logHeader}] Попытка очистки отпечатка базы данных.");
            Clear();
            _logger.LogInformation($"[{logHeader}] Отпечаток базы данных очищен.");

            using var databaseContext = _dbContextFactory.CreateContext();

            _logger.LogInformation($"[{logHeader}] Заполнение отпечатка базы данных.");

            try
            {
                var dbTypes = databaseContext.GetTypes();

                _logger.LogInformation($"[{logHeader}] Обнаружено {dbTypes.Count} таблиц для заполнения: {string.Join(",", dbTypes.Select(t => t.Name))}.");

                var response = databaseContext.GetAllTables();

                if (!response.IsCorrect || response.Content == null)
                {
                    var errorMessage = LogHelper.GetExceptionMessage($"Ошибка получения всех таблиц из базы данных: {response.Description}", response.Ex);
                    _logger.LogError($"[{logHeader}] {errorMessage}");
                    return DatabaseResponse.IncorrectResponse(logHeader, response.Description, response.ResponseType, response.Ex);
                }

                var dbSets = response.Content;

                foreach (var set in dbSets)
                {
                    var fingerprintSet = GetTableFromDatabase(set);

                    if (fingerprintSet == null)
                    {
                        _logger.LogError($"[{logHeader}] Для набора типа [{set.GetType().GenericTypeArguments[0].Name}] не найдено таблицы в базе данных.");
                        continue;
                    }

                    _logger.LogInformation($"[{logHeader}] В набор типа [{set.GetType().GenericTypeArguments[0].Name}] получено [{fingerprintSet.Count()}] сущностей.");

                    var attribute = set.GetType().GetGenericArguments()[0].GetCustomAttribute<AssignedTypeAttribute>();

                    if (attribute == null)
                    {
                        _logger.LogError($"[{logHeader}] Набор типа [{set.GetType().GenericTypeArguments[0].Name}] не имеет атрибута AssignedTypeAttribute.");
                        continue;
                    }

                    var type = attribute.Type;

                    AddFingerprintSet(fingerprintSet, type);
                }

                return DatabaseResponse.CorrectResponse(logHeader);
            }
            catch (IncorrectResponseException ex)
            {
                _logger.LogCritical($"[{logHeader}] Ошибка подключения к базе данных: {ex.Message}, {ex.StackTrace}.");
                return DatabaseResponse.IncorrectResponse(logHeader, ex.Message, ResponseType.DatabaseAccessError, ex);
            }
            catch (ParsingException ex)
            {
                _logger.LogCritical($"[Cache: FillCache] Ошибка парсинга при заполнении отпечатка БД: {ex.Message}, {ex.StackTrace}.");
                return DatabaseResponse.IncorrectResponse("Cache: FillCache", ex.Message, ResponseType.LogicError, ex);

            }
        }

        public void Clear()
        {
            foreach (var cacheSet in _fingerprintSets)
            {
                cacheSet.Value.Clear();
            }
            _fingerprintSets.Clear();
        }

        #region Find/GetMethods

        /// <summary>
        /// Получение набора отпечатка базы данных из таблицы базы данных.
        /// </summary>
        /// <param name="dbSet">Коллекция сущностей определенного типа из базы данных.</param>
        /// <returns>Список сущностей отпечатка базы данных.</returns>
        /// <exception cref="IncorrectResponseException">Если база данных оказалась недоступна.</exception>
        protected IEnumerable<CacheEntity>? GetTableFromDatabase(IQueryable<BaseEntity> dbSet)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            var response = DatabaseSafeGetter.GetValue(() =>
                dbSet
                .AsNoTracking()
                .Select(baseEntity => fingerprintConverter.ConvertToFingerprintEntity(baseEntity)));

            if (!response.IsCorrect)
            {
                var errorMessage = $"[{logHeader}] Ошибка запроса к базе данных: {response.Description}.";
                _logger.LogCritical(errorMessage);
                throw new IncorrectResponseException(errorMessage, response.Ex);
            }

            return response.Content;
        }

        public IEnumerable<T>? FindOrDefaultValues<T>() where T : CacheEntity
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            _logger.LogDebug($"[{logHeader}] Поиск набора типа {typeof(T).Name}.");
            try
            {
                var fingerprintSet = _fingerprintSets[typeof(T)];
                _logger.LogDebug($"[{logHeader}] Набор типа {typeof(T).Name} НАЙДЕН.");
                var values = fingerprintSet.Values.Select(fingerprintEntity => (T)fingerprintEntity).ToList();
                return values;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{logHeader}] Набор типа {typeof(T).Name} НЕ НАЙДЕН. {ex.Message}");
                return default;
            }
        }

        public IEnumerable<T>? FindOrDefaultValues<T>(Predicate<T> predicate) where T : CacheEntity
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            _logger.LogDebug($"[{logHeader}] Поиск набора типа {typeof(T).Name}.");
            try
            {
                var cacheSet = _fingerprintSets[typeof(T)];
                _logger.LogDebug($"[{logHeader}] Набор типа {typeof(T).Name} НАЙДЕН.");
                var values = cacheSet.Values.Where(fingerprintEntity => predicate((T)fingerprintEntity)).Select(fingerprintEntity => (T)fingerprintEntity).ToList();
                return values;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{logHeader}] Набор типа {typeof(T).Name} НЕ НАЙДЕН. {ex.Message}");
                return default;
            }
        }

        #endregion

        #region InteractionsWithDatabaseFingerprint

        /// <summary>
        /// Добавление одного набора в отпечаток.
        /// </summary>
        /// <typeparam name="T">Тип сущностей, которые будут добавлены в отпечаток.</typeparam>
        /// <param name="values">Список сущностей.</param>
        protected void AddFingerprintSet(IEnumerable<CacheEntity> values, Type type)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            _logger.LogInformation($"[{logHeader}] Добавление в отпечаток набора типа {type.Name} из {values.Count()} сущностей.");

            CacheSet<CacheEntity> fingerprintSet = new(values);
            _fingerprintSets.TryAdd(type, fingerprintSet);
        }

        public void AddEntity(CacheEntity entity)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            Type type = entity.GetType();
            var cacheSet = _fingerprintSets[type];

            var res = cacheSet.FindEntity(entity.Id);
            if (res != null)
            {
                _logger.LogWarning($"[{logHeader}] Попытка добавления существующей сущности: {JsonConvert.SerializeObject(entity)}, найденный дубликат: {JsonConvert.SerializeObject(res)}. Добавление проигнорировано!");
                return;
            }

            cacheSet.AddEntity(entity);
        }

        public void SaveEntity(CacheEntity entity)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            Type type = entity.GetType();
            var cacheSet = _fingerprintSets[type];

            var res = cacheSet.FindEntity(entity.Id);
            if (res != null)
            {
                _logger.LogWarning($"[{logHeader}] Попытка добавления существующей сущности: {JsonConvert.SerializeObject(entity)}, найденный дубликат: {JsonConvert.SerializeObject(res)}, обновление.");
                cacheSet.UpdateEntity(entity);
            }

            cacheSet.AddEntity(entity);
        }

        public void RemoveEntity(CacheEntity entity)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            Type type = entity.GetType();
            var cacheSet = _fingerprintSets[type];

            var res = cacheSet.FindEntity(entity.Id);
            if (res == null)
            {
                _logger.LogWarning($"[{logHeader}] Попытка удаления несуществующей сущности: {JsonConvert.SerializeObject(entity)}.");
                return;
            }

            cacheSet.RemoveEntity(entity);
        }

        public bool IsExistTypeInFingerprint(Type type)
        {
            return _fingerprintSets.ContainsKey(type);
        }

        #endregion
    }
}
