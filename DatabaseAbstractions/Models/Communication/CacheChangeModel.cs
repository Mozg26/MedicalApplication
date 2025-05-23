using DatabaseAbstractions.Models.CacheModels;
using Extensions.Enums;

namespace DatabaseAbstractions.Models.Communication
{
    /// <summary>
    /// Модель изменения в базе данных.
    /// </summary>
    public class CacheChangeModel
    {
        /// <summary>
        /// Уникальный идентификатор транзакции.
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Модель отпечатка базы данных.
        /// </summary>
        public CacheEntity CacheEntity { get; set; } = null!;

        /// <summary>
        /// Тип изменения в базе данных.
        /// </summary>
        public DatabaseChangeType DatabaseChangeType { get; set; }

        /// <summary>
        /// Конструктор по умолчанию модели изменения в базе данных.
        /// </summary>
        public CacheChangeModel()
        {
            TransactionId = Guid.NewGuid();

            DatabaseChangeType = DatabaseChangeType.None;
        }

        /// <summary>
        /// Конструктор модели изменения в базе данных.
        /// </summary>
        /// <param name="fingerprintEntity">Модель отпечатка базы данных.</param>
        /// <param name="databaseChangeType">Тип изменения в базе данных.</param>
        public CacheChangeModel(CacheEntity cacheEntity, DatabaseChangeType databaseChangeType)
        {
            TransactionId = Guid.NewGuid();
            CacheEntity = cacheEntity;
            DatabaseChangeType = databaseChangeType;
        }

        public CacheChangeModel(Guid transactionId, CacheEntity cacheEntity, DatabaseChangeType databaseChangeType)
        {
            TransactionId = transactionId;
            CacheEntity = cacheEntity;
            DatabaseChangeType = databaseChangeType;
        }
    }
}
