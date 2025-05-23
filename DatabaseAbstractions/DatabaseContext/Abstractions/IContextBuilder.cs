using DatabaseAbstractions.Models.DatabaseModels;

namespace DatabaseAbstractions.DatabaseContext.Abstractions
{
    /// <summary>
    /// Интрефейс сборщика сущностей базы данных.
    /// </summary>
    public interface IContextBuilder
    {
        /// <summary>
        /// Метод построения таблицы для базы данных.
        /// </summary>
        /// <param name="query">Запрос на построение таблицы.</param>
        /// <param name="type">Тип сущности.</param>
        /// <returns>Результат запроса на построение таблицы заданного типа.</returns>
        IQueryable<BaseEntity> BuildTable(IQueryable<BaseEntity>? query, Type type);
    }
}
