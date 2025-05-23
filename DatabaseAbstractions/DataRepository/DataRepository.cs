using DatabaseAbstractions.DatabaseCache.Abstractions;
using DatabaseAbstractions.DatabaseOperations.Abstractions;
using DatabaseAbstractions.DataRepository.Abstractions;
using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.Communication;
using Extensions.Enums;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace DatabaseAbstractions.DataRepository
{
    /// <summary>
    /// Репозиторий отпечатка базы данных.
    /// </summary>
    public class DataRepository : IDataRepository, IDisposable
    {
        /// <summary>
        /// Интерфейс отпечатка базы данных.
        /// </summary>
        protected IDatabaseCache _databaseFingerprint = null!;

        /// <summary>
        /// Интерфейс сохранения изменений данных в базы данных.
        /// </summary>
        protected IDatabaseSaver _databaseSaver = null!;

        protected bool _inWork;

        /// <summary>
        /// Имя класса.
        /// </summary>
        private readonly string _className;

        /// <summary>
        /// Логгер.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Конструктор репозитория отпечатка базы данных.
        /// </summary>
        /// <param name="databaseSaver">Интерфейс сохранения изменений даннных в базы данных.</param>
        /// <param name="databaseFingerprint">Интерфейс отпечатка базы данных.</param>
        /// <param name="logger">Логгер.</param>
        public DataRepository(IDatabaseSaver databaseSaver, IDatabaseCache databaseFingerprint, ILogger logger)
        {
            _className = GetType().Name;

            _logger = logger;

            _databaseFingerprint = databaseFingerprint;

            _databaseSaver = databaseSaver;
        }

        #region FillClearFingerprint

        public async void Fill()
        {
            var methodName = GetCurrentMethodName(MethodBase.GetCurrentMethod());

            _logger.LogInformation($"[{_className}: {methodName}] Начало заполнения кэша.");

            var response = await _databaseFingerprint.Fill();

            if (!response.IsCorrect)
            {
                _logger.LogCritical($"[{_className}: {methodName}] Заполнение отпечатка базы завершено с ошибкой: {response.Description}");
            }

            _logger.LogInformation($"[{_className}: {methodName}] Отпечаток базы заполнен.");
        }

        public void Clear()
        {
            var methodName = GetCurrentMethodName(MethodBase.GetCurrentMethod());

            _logger.LogInformation($"[{_className}: {methodName}] Начало очистки отпечатка базы данных.");

            // Вероятно, длительный процесс
            _databaseFingerprint.Clear();

            _logger.LogInformation($"[{_className}: {methodName}] Отпечаток базы данных очищен и пуст.");
        }

        #endregion

        #region FindMethods

        public TEntity? FindOrDefaultEntity<TEntity>(int id) where TEntity : CacheEntity
        {
            var set = _databaseFingerprint.FindOrDefaultValues<TEntity>();
            return set?.FirstOrDefault(entity => entity.Id == id);
        }

        public TEntity? FindOrDefaultEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : CacheEntity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity?>? FindOrDefaultEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : CacheEntity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity?>? FindOrDefaultEntities<TEntity>() where TEntity : CacheEntity
        {
            return _databaseFingerprint.FindOrDefaultValues<TEntity>();
        }

        #endregion

        #region InteractionsWithDatabaseFingerprint

        /// <summary>
        /// Метод добавления сущности в отпечаток базы данных.
        /// </summary>
        /// <param name="newEntity">Сущность для добавления.</param>
        protected void AddEntity(CacheEntity newEntity)
        {
            _databaseFingerprint.AddEntity(newEntity);
            var model = new CacheChangeModel(newEntity, DatabaseChangeType.Add);

            _databaseSaver.SaveModel(model);
        }

        /// <summary>
        /// Метод сохранения сущности в отпечаток базы данных.
        /// </summary>
        /// <param name="entity">Сущность для сохранения.</param>
        public void SaveEntity(CacheEntity newEntity)
        {
            var type = newEntity.GetType();

            if (_databaseFingerprint.IsExistTypeInFingerprint(type))
            {
                _databaseFingerprint.SaveEntity(newEntity);
            }

            var model = new CacheChangeModel(newEntity, DatabaseChangeType.Save);

            _databaseSaver.SaveModel(model);
        }

        /// <summary>
        /// Метод удаления сущности из отпечатка базы данных.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        protected void RemoveEntity(CacheEntity newEntity)
        {
            _databaseFingerprint.RemoveEntity(newEntity);
            var model = new CacheChangeModel(newEntity, DatabaseChangeType.Delete);

            _databaseSaver.SaveModel(model);
        }

        #endregion

        /// <summary>
        /// Вспомогательный метод, отвечающий за получение имени метода для логирования.
        /// </summary>
        /// <param name="methodInfo">Информация о методе.</param>
        /// <returns>Имя метода, которое будет вписано в логи.</returns>
        private string GetCurrentMethodName(MethodBase? methodInfo)
        {
            if (methodInfo == null)
            {
                var errorString = "Невозможно получить информацию о методе.";
                _logger.LogError($"[{_className}: GetCurrentMethodName] {errorString}");
                return "неизвестный метод";
            }

            return methodInfo.Name;
        }

        public void Dispose()
        {

        }
    }
}
