using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.MongoModels;
using Extensions.Tools;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;

namespace DatabaseAbstractions.LocalDatabase.Abstractions
{
    /// <summary>
    /// Класс репозитория базы данных MongoDB.
    /// </summary>
    public class MongoRepository : IMongoRepository
    {
        /// <summary>
        /// Коллекция сохранения для базы данных Mongo.
        /// </summary>
        private readonly IMongoCollection<MongoEntity> SaveCollection;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Имя класса.
        /// </summary>
        private readonly string _className;

        /// <summary>
        /// Конструктор репозитория Mongo.
        /// </summary>
        /// <param name="mongoDatabase">Интерфейс базы данных Mongo.</param>
        /// <param name="logger">Логгер.</param>
        public MongoRepository(IMongoDatabase mongoDatabase, ILogger logger)
        {

            _className = GetType().Name;

            SaveCollection = mongoDatabase.GetCollection<MongoEntity>(MongoCollectionsNames.SaveCollectionName);

            _logger = logger;
        }

        public void Save(CacheChangeModel databaseChangeModel)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), databaseChangeModel.TransactionId);

            var mongoEntity = new MongoEntity(databaseChangeModel);

            _logger.LogInformation($"[{logHeader}] Сохранение сущности типа {databaseChangeModel.CacheEntity.GetType().Name} в локальную базу данных MongoDB.");

            SaveCollection?.InsertOne(mongoEntity);
        }

        public void Delete(CacheChangeModel databaseChangeModel)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod(), databaseChangeModel.TransactionId);

            _logger.LogInformation($"[{logHeader}] Удаление сущности типа {databaseChangeModel.CacheEntity.GetType().Name} из локальной базы данных MongoDB.");

            SaveCollection?.DeleteOne(MongoEntity => MongoEntity.TransactionId == databaseChangeModel.TransactionId);
            return;
        }

        public List<CacheChangeModel> GetAllEntities()
        {
            List<CacheChangeModel> mongoEntities = [];

            var entities = SaveCollection
                .Find(mongoEntity => true)
                .ToList()
                .Select(mongoEntity => new CacheChangeModel(mongoEntity.FingerprintEntity, mongoEntity.TypeChange));

            mongoEntities.AddRange(entities);

            return mongoEntities;
        }
    }
}
