using DatabaseAbstractions.LocalDatabase.Abstractions;
using DatabaseAbstractions.Models.Communication;
using Extensions.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DatabaseAbstractions.LocalDatabase
{
    /// <summary>
    /// Абстрактный класс базы данных Mongo.
    /// </summary>
    public abstract class BaseMongoModule : ILocalDatabaseModule
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        protected readonly string ConnectionString = "mongodb://127.0.0.1:27017";

        /// <summary>
        /// Имя локальной базы данных в Mongo.
        /// </summary>
        protected readonly string DatabaseName = "LocalDatabase";

        /// <summary>
        /// Репозиторий коллекций Mongo.
        /// </summary>
        protected IMongoRepository MongoRepository = null!;

        /// <summary>
        /// База данных Mongo.
        /// </summary>
        protected IMongoDatabase MongoDatabase = null!;

        /// <summary>
        /// Логгер.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Конструктор для базового модуля локальной базы данных Mongo.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public BaseMongoModule(ILogger logger)
        {
            _logger = logger;

            ConnectToDatabase();

            MongoRepository = new MongoRepository(MongoDatabase, _logger);
        }

        /// <summary>
        /// Конструктор для базового модуля локальной базы данных Mongo со строкой подключения и именем базы данных.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        /// <param name="databaseName">Имя базы данных.</param>
        /// <param name="logger">Логгер.</param>
        public BaseMongoModule(string connectionString, string databaseName, ILogger logger)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            _logger = logger;

            ConnectToDatabase();

            MongoRepository = new MongoRepository(MongoDatabase, _logger);
        }

        public virtual void Save(CacheChangeModel databaseChangeModel)
        {
            _logger.LogInformation("[BaseMongoModule: SaveEntity] Сработал базовый метод сохранения сущности в локальную базу Mongo.");

            MongoRepository.Save(databaseChangeModel);
        }

        public virtual void Delete(CacheChangeModel databaseChangeModel)
        {
            _logger.LogInformation("[BaseMongoModule: DeleteEntity] Сработал базовый метод удаления сущности из локальной базы Mongo.");
        }

        public virtual List<CacheChangeModel>? GetAllEntities()
        {
            _logger.LogInformation("[BaseMongoModule: GetAllEntities] Сработал базовый метод получения всех сущностей из локальной базы Mongo.");
            return default;
        }

        /// <summary>
        /// Подключение к локальной базе Mongo.
        /// </summary>
        /// <exception cref="ConnectionErrorException">Если возникли ошибки при попытке подключения к локальной базе Mongo.</exception>
        private void ConnectToDatabase()
        {
            _logger.LogInformation($"[BaseMongoModule: ConnectToDatabase] Подключение к локальному серверу Mongo по адресу: {ConnectionString}.");
            var client = new MongoClient(ConnectionString);

            _logger.LogInformation($"[BaseMongoModule: ConnectToDatabase] Подключение к локальной базе Mongo по имени: {DatabaseName}.");
            var mongoDatabase = client?.GetDatabase(DatabaseName);

            if (client == null || mongoDatabase == null)
            {
                var errorMessage = $"[BaseMongoModule: ConnectToDatabase] Не удалось подключиться к локальной базе данных Mongo " +
                    $"по адресу {ConnectionString} и названию {DatabaseName}.";
                _logger.LogCritical(errorMessage);
                throw new ConnectionErrorException(errorMessage);
            }

            MongoDatabase = mongoDatabase;
        }
    }
}
