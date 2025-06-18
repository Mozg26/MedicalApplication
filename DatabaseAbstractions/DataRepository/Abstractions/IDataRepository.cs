using DatabaseAbstractions.Models.CacheModels;
using System.Linq.Expressions;

namespace DatabaseAbstractions.DataRepository.Abstractions
{
    /// <summary>
    /// Интерфейс репозитория отпечатка базы данных.
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Метод заполнения отпечатка базы данных актуальными данными из БД.
        /// </summary>
        /// <returns>true — если отпечаток заполнен успешно, false — если возникла ошибка.</returns>
        void Fill();

        /// <summary>
        /// Метод очистки отпечатка базы данных.
        /// </summary>
        void Clear();

        void SaveEntity(CacheEntity newEntity);

        void RemoveEntity(CacheEntity newEntity);

        TEntity? FindOrDefaultEntity<TEntity>(int id) where TEntity : CacheEntity;

        TEntity? FindOrDefaultEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : CacheEntity;

        IEnumerable<TEntity?>? FindOrDefaultEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : CacheEntity;

        IEnumerable<TEntity?>? FindOrDefaultEntities<TEntity>() where TEntity : CacheEntity;
    }
}
