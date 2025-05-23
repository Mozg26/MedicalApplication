using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.Communication;


namespace DatabaseAbstractions.DatabaseCache.Abstractions
{
    /// <summary>
    /// Интерфейс отпечатка базы данных.
    /// </summary>
    public interface IDatabaseCache
    {
        /// <summary>
        /// Заполнение отпечатка данными из БД.
        /// </summary>
        /// <returns>Ответ БД об успешном или неуспешном заполнении отпечатка.</returns>
        Task<DatabaseResponse> Fill();

        #region FindMethods

        /// <summary>
        /// Поиск списка значений набора из отпечатка БД заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип сущностей.</typeparam>
        /// <returns>Список сущностей по заданному типу или default, если такие сущности не найдены.</returns>
        /// <remarks>Допустимо значение default.</remarks>
        IEnumerable<T>? FindOrDefaultValues<T>() where T : CacheEntity;

        /// <summary>
        /// Поиск списка значений набора из отпечатка БД заданного типа по предикату.
        /// </summary>
        /// <typeparam name="T">Тип сущностей.</typeparam>
        /// <param name="predicate">Предикат, согласно которому необходимо отобрать сущности из отпечатка БД.</param>
        /// <returns>Список сущностей по заданному типу и предикату или default, если такие сущности не найдены.</returns>
        /// <remarks>Допустимо значение default.</remarks>
        IEnumerable<T>? FindOrDefaultValues<T>(Predicate<T> predicate) where T : CacheEntity;

        #endregion

        #region InteractionsWithDatabaseFingerprint

        /// <summary>
        /// Метод добавления сущности в отпечаток базы данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void AddEntity(CacheEntity entity);

        /// <summary>
        /// Метод сохранения сущности в отпечаток базы данных.
        /// </summary>
        /// <param name="entity">Сущность для сохранения.</param>
        void SaveEntity(CacheEntity entity);

        /// <summary>
        /// Метод удаления сущности из отпечатка базы данных.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        void RemoveEntity(CacheEntity entity);

        bool IsExistTypeInFingerprint(Type type);

        /// <summary>
        /// Очистка отпечатка базы данных.
        /// </summary>
        void Clear();

        #endregion
    }
}
