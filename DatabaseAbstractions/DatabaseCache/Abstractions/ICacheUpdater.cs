using DatabaseAbstractions.Models.Communication;

namespace DatabaseAbstractions.DatabaseCache.Abstractions
{
    public interface ICacheUpdater
    {
        /// <summary>
        /// Триггер на изменение
        /// </summary>
        public Action<DatabaseChangeModel>? OnChangeDetected { get; set; }

        /// <summary>
        /// Конфигурация относительно БД
        /// </summary>
        public void Configure();

        /// <summary>
        /// Запуск прослушивания
        /// </summary>
        public void Start();

        /// <summary>
        /// Остановка прослушивания
        /// </summary>
        public void Stop();

    }
}
