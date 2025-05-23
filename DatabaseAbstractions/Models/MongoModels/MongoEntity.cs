using Extensions.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.Communication;

namespace DatabaseAbstractions.Models.MongoModels
{
    /// <summary>
    /// Модель сущности базы данных MongoDB.
    /// </summary>
    public class MongoEntity
    {
        /// <summary>
        /// Уникальный номер сущности в базе данных Mongo.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Уникальный номер транзакции.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Сущность отпечатка базы данных для сохранения.
        /// </summary>
        public CacheEntity FingerprintEntity { get; set; }

        /// <summary>
        /// Тип изменения в базе данных.
        /// </summary>
        public DatabaseChangeType TypeChange { get; set; }

        /// <summary>
        /// Время добавления в базу данных MongoDB.
        /// </summary>
        public DateTime AddingTime { get; set; }

        /// <summary>
        /// Конструктор сущности Mongo по номеру транзакции, сущности отпечатка базы данных и типу изменения в базе данных.
        /// </summary>
        /// <param name="transactionId">Уникальный номер транзакции.</param>
        /// <param name="fingerprintEntity">Сущность отпечатка базы данных для сохранения.</param>
        /// <param name="typeChange">Тип изменения в базе данных.</param>
        public MongoEntity(Guid transactionId, CacheEntity fingerprintEntity, DatabaseChangeType typeChange)
        {
            TransactionId = transactionId;
            FingerprintEntity = fingerprintEntity;
            TypeChange = typeChange;
            AddingTime = DateTime.Now;
        }

        /// <summary>
        /// Конструктор сущности Mongo по модели изменения в базе данных.
        /// </summary>
        /// <param name="changeModel">Модель изменения в базе данных.</param>
        public MongoEntity(CacheChangeModel changeModel)
        {
            TransactionId = changeModel.TransactionId;
            FingerprintEntity = changeModel.CacheEntity;
            TypeChange = changeModel.DatabaseChangeType;
            AddingTime = DateTime.Now;
        }

        /// <summary>
        /// Конвертация сущности Mongo в модель изменения в базе данных.
        /// </summary>
        /// <returns>Модель изменения в базе данных.</returns>
        public CacheChangeModel ToDatabaseChangeModelEntity()
        {
            return new CacheChangeModel(TransactionId, FingerprintEntity, TypeChange);
        }
    }
}
