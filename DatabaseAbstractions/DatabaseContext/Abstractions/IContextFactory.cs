namespace DatabaseAbstractions.DatabaseContext.Abstractions
{
    /// <summary>
    /// Интерфейс фабрики контекста базы данных.
    /// </summary>
    public interface IContextFactory<T> where T : DatabaseContext
    {
        /// <summary>
        /// Создание контекста базы данных.
        /// </summary>
        /// <returns>Контекст базы данных.</returns>
        public T CreateContext();
    }
}
