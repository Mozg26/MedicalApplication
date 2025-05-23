using DatabaseAbstractions.Models.Communication;

namespace DatabaseAbstractions.LocalDatabase.Abstractions
{
    /// <summary>
    /// Интерфейс модуля локальной базы данных.
    /// </summary>
    public interface ILocalDatabaseModule
    {
        /// <summary>
        /// Метод сохранения изменений в локальную базу данных.
        /// </summary>
        /// <param name="entity">Модель изменения базы данных.</param>
        void Save(CacheChangeModel entity);

        /// <summary>
        /// Метод удаления изменений из локальной базы данных.
        /// </summary>
        /// <param name="entity">Модель изменения базы данных.</param>
        void Delete(CacheChangeModel entity);

        /// <summary>
        /// Метод получения всех сущностей из локальной базы данных.
        /// </summary>
        /// <returns>Список сущностей на изменение.</returns>
        List<CacheChangeModel>? GetAllEntities();
    }
}
