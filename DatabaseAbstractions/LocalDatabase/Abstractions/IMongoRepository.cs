using DatabaseAbstractions.Models.Communication;

namespace DatabaseAbstractions.LocalDatabase.Abstractions
{
    /// <summary>
    /// Интерфейс репозитория коллекций Mongo.
    /// </summary>
    public interface IMongoRepository
    {
        /// <summary>
        /// Метод сохранения данных в коллекцию Mongo.
        /// </summary>
        /// <param name="databaseChangeModel">Модель изменения в базе данных.</param>
        void Save(CacheChangeModel databaseChangeModel);

        /// <summary>
        /// Метод удаления данных из коллекции Mongo.
        /// </summary>
        /// <param name="databaseChangeModel">Модель изменения в базе данных.</param>
        void Delete(CacheChangeModel databaseChangeModel);

        /// <summary>
        /// Метод получения всех сущностей Mongo.
        /// </summary>
        /// <returns>Список сущностей Mongo.</returns>
        List<CacheChangeModel> GetAllEntities();
    }
}
