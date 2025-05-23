using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseAbstractions.Models.Enums;
using Extensions.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DatabaseAbstractions.DatabaseContext
{
    /// <summary>
    /// Базовый контекст базы данных.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Интерфейс сборщика сущностей базы данных.
        /// </summary>
        protected readonly IContextBuilder _contextBuilder = null!;

        /// <summary>
        /// Логгер.
        /// </summary>
        protected readonly ILogger _logger = null!;

        /// <summary>
        /// Имя класса.
        /// </summary>
        private readonly string _className;

        /// <summary>
        /// Конструктор базового контекста базы данных с опциями.
        /// </summary>
        /// <param name="options">Опции, которые необходимо применить при создании контекста.</param>
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            _className = GetType().Name;
        }

        /// <summary>
        /// Конструктор базового контекста базы данных с инструкциями и опциями.
        /// </summary>
        /// <param name="contextBuilder">Интерфейс сборщика сущностей базы данных.</param>
        /// <param name="options">Опции, которые необходимо применить при создании контекста.</param>
        /// <param name="logger">Логгер.</param>
        public DatabaseContext(IContextBuilder contextBuilder, DbContextOptions options, ILogger logger) : base(options)
        {
            _className = GetType().Name;
            _contextBuilder = contextBuilder;
            _logger = logger;
        }

        /// <summary>
        /// Метод получения списка типов контекста базы данных.
        /// </summary>
        /// <returns>Список типов контекста базы данных.</returns>
        public List<Type> GetTypes()
        {
            var types = Model.GetEntityTypes().Select(t => t.ClrType).ToList();
            return types;
        }

        /// <summary>
        /// Метод получения всех таблиц контекста.
        /// </summary>
        /// <returns>Ответ контекста базы данных со списком всех таблиц контекса.</returns>
        public virtual DatabaseResponse<List<IQueryable<BaseEntity>>> GetAllTables()
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            var dbSetProps = GetType().GetProperties().Where(prorertyInfo => prorertyInfo.PropertyType.IsGenericType
            && typeof(DbSet<>).IsAssignableFrom(prorertyInfo.PropertyType.GetGenericTypeDefinition()));

            List<object?> dbSets = dbSetProps.Select(x => x.GetValue(this, null)).ToList();

            var resultList = new List<IQueryable<BaseEntity>>();

            foreach (var dbSet in dbSets)
            {
                if (dbSet != null)
                {
                    var type = dbSet.GetType().GenericTypeArguments[0];
                    var query = DatabaseSafeGetter.GetValue(() => _contextBuilder.BuildTable(dbSet as IQueryable<BaseEntity>, type));

                    if (!query.IsCorrect)
                    {
                        _logger.LogInformation($"[{logHeader}] Запрос таблицы {type} некорректный. Ошибка: {query.GetErrorString()}");
                        return new DatabaseResponse<List<IQueryable<BaseEntity>>>
                            (default, logHeader, query.GetErrorString(), query.ResponseType, false, query.Ex);
                    }

                    var table = query.Content;

                    if (table == null)
                    {
                        _logger.LogInformation($"[{logHeader}] Таблица {type} пустая.");
                        return new DatabaseResponse<List<IQueryable<BaseEntity>>>
                            (default, logHeader, query.GetErrorString(), ResponseType.EmptyTable, false, query.Ex);
                    }

                    resultList.Add(table);
                    _logger.LogInformation($"[{logHeader}] Таблица {type} успешно выгружена.");
                }
            }

            _logger.LogInformation($"[{logHeader}] Выгрузка таблиц завершена.");
            return DatabaseResponse<List<IQueryable<BaseEntity>>>.CorrectResponse(resultList, logHeader);
        }

        /// <summary>
        /// Метод получения таблицы контекста определенного типа.
        /// </summary>
        /// <param name="type">Тип требуемой таблицы.</param>
        /// <returns>Ответ контекста с требуемой таблицей.</returns>
        public virtual DatabaseResponse<IQueryable<BaseEntity>> GetTable(Type type)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            var dbSetProp = GetType()
                .GetProperties()
                .FirstOrDefault(prorertyInfo => prorertyInfo.PropertyType.IsGenericType
                && (typeof(DbSet<>).IsAssignableFrom(prorertyInfo.PropertyType.GetGenericTypeDefinition()))
                && (prorertyInfo.GetValue(this, null) is { } value).GetType().GenericTypeArguments.Contains(type));

            string errorString;

            if (dbSetProp == null)
            {
                errorString = $"[{logHeader}] Таблицы типа {type} не существует в контексте базы данных.";
                _logger.LogInformation(errorString);
                return new DatabaseResponse<IQueryable<BaseEntity>>
                    (default, logHeader, errorString, ResponseType.TableNotExist, false);
            }

            var dbSet = dbSetProp.GetValue(this, null);
            var query = DatabaseSafeGetter.GetValue(() => _contextBuilder.BuildTable(dbSet as IQueryable<BaseEntity>, type));

            return query;
        }

        /// <summary>
        /// Метод добавления сущности 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void AddEntity<T>(T entity) where T : BaseEntity
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            DbSet<T> set = Set<T>();
            set.Add(entity);
            _logger.LogInformation($"[{logHeader}] Добавлена сущность типа {typeof(T)} с id: {entity.Id}.");
        }

        public void UpdateEntity<T>(T entity) where T : BaseEntity
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            DbSet<T> set = Set<T>();
            set.Update(entity);
            _logger.LogInformation($"[{logHeader}] Обновлена сущность типа {typeof(T)} с id: {entity.Id}.");
        }

        public void DeleteEntity<T>(T entity) where T : BaseEntity
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            DbSet<T> set = Set<T>();
            set.Remove(entity);
            _logger.LogInformation($"[{logHeader}] Удалена сущность типа {typeof(T)} с id: {entity.Id}.");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
