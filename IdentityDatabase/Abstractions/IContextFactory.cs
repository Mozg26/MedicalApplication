namespace IdentityDatabase.Abstractions
{
    public interface IContextFactory
    {
        /// <summary>
        /// Создание конктекста базы данных.
        /// </summary>
        /// <returns>Конекст базы</returns>
        public MainContext CreateContext();
    }
}
