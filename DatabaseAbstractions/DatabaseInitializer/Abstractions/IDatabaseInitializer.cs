namespace DatabaseAbstractions.DatabaseInitializer.Abstractions
{
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Конфигурация относительно.
        /// </summary>
        public void Configure();

        /// <summary>
        /// Создание инфраструктуры БД.
        /// </summary>
        /// <returns></returns>
        public Task CreateDatabaseInfrastructureAsync();

        /// <summary>
        /// Очистить инфраструктуру БД. Выполнить при завершении работы приложения.
        /// </summary>
        /// <returns></returns>
        public Task ClearDatabaseInfrastructureAsync();
    }
}
