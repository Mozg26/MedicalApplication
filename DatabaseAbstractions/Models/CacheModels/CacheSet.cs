namespace DatabaseAbstractions.Models.CacheModels
{
    /// <summary>
    /// Модель набора отпечатка базы данных.
    /// </summary>
    public class CacheSet
    {

    }

    /// <summary>
    /// Модель набора отпечатка базы данных со списком сущностей.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public class CacheSet<T> : CacheSet where T : CacheEntity
    {
        /// <summary>
        /// Список сущностей.
        /// </summary>
        public List<T> Values { get; set; }

        /// <summary>
        /// Конструктор модели набора отпечатка базы данных со списком сущностей по умолчанию.
        /// </summary>
        public CacheSet()
        {
            Values = [];
        }

        /// <summary>
        /// Конструктор модели набора отпечатка базы данных со списком сущностей.
        /// </summary>
        /// <param name="values">Список сущностей.</param>
        public CacheSet(IEnumerable<T> values)
        {
            Values = [];
            Values.AddRange(values);
        }

        /// <summary>
        /// Поиск сущности по номеру id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности.</param>
        /// <returns>Найденная сущность или default.</returns>
        /// <remarks>Допустимо значение default.</remarks>
        public T? FindEntity(int id)
        {
            return Values.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Поиск сущности по заданному предикату.
        /// </summary>
        /// <param name="predicate">Заданный предикат, согласно которому осуществляется поиск сущности.</param>
        /// <returns>Найденная сущность или default.</returns>
        /// <remarks>Допустимо значение default.</remarks>
        public T? FindEntity(Predicate<T> predicate)
        {
            return Values.FirstOrDefault(e => predicate(e));
        }

        /// <summary>
        /// Поиск сущностей по предикату.
        /// </summary>
        /// <param name="predicate">Заданный предикат, согласно которому осуществляется поиск сущностей.</param>
        /// <returns>Найденные сущности или default.</returns>
        /// <remarks>Допустимо значение default.</remarks>
        public IEnumerable<T>? FindEntities(Predicate<T> predicate)
        {
            return Values.Where(e => predicate(e) == true);
        }

        /// <summary>
        /// Добавление сущности в набор.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо добавить.</param>
        public void AddEntity(T entity)
        {
            Values.Add(entity);
        }

        /// <summary>
        /// Обновление сущности в наборе.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо обновить.</param>
        public void UpdateEntity(T entity)
        {
            var updateEntity = Values.FirstOrDefault(e => e.Id == entity.Id);

            if (updateEntity != null)
                updateEntity = entity;
        }

        /// <summary>
        /// Удаление сущности из набора по ссылке.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо удалить.</param>
        public void RemoveEntity(T entity)
        {
            Values.Remove(entity);
        }

        /// <summary>
        /// Удаление сущности из набора по номеру id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности.</param>
        public void RemoveEntity(int id)
        {
            var removeEntity = Values.FirstOrDefault(e => e.Id == id);

            if (removeEntity != null)
                Values.Remove(removeEntity);
        }

        /// <summary>
        /// Очистка модели набора отпечатка базы.
        /// </summary>
        public void Clear()
        {
            Values.Clear();
        }
    }
}
