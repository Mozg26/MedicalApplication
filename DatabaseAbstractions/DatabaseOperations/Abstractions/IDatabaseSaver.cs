using DatabaseAbstractions.Models.Communication;

namespace DatabaseAbstractions.DatabaseOperations.Abstractions
{
    public interface IDatabaseSaver
    {
        /// <summary>
        /// Метод выгрузки данных из локального хранилища для повторной попытки сохранения.
        /// </summary>
        public void FillLocalDatabaseData();

        /// <summary>
        /// Метод получения списка операций сохранения в БД, завершившихся неудачей.
        /// </summary>
        /// <returns>Список проблемных операций.</returns>
        public List<CacheChangeModel> GetProblemProcesses();

        /// <summary>
        /// Повторная выгрузка списка проблемных операций в очередь обработки (повторная попытка сохранения).
        /// </summary>
        public void TryRepeateProblemProcesses();

        /// <summary>
        /// Метод сохранения сущности.
        /// </summary>
        /// <param name="entity">Сущность, которую необходимо сохранить в базу.</param>
        /// <returns>Ответ об успешном или неуспешном добавлении в очередь сохранения.</returns>
        public DatabaseResponse SaveModel(CacheChangeModel entity);
    }
}
