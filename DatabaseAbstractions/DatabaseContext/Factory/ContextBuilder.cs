using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using Extensions.Models;
using Extensions.Tools;
using System.Reflection;

namespace DatabaseAbstractions.DatabaseContext.Factory
{
    /// <summary>
    /// Базовый сборщик сущностей для базы данных.
    /// </summary>
    public class ContextBuilder : IContextBuilder
    {
        /// <summary>
        /// Словарь методов сборки сущностей.
        /// </summary>
        private readonly Dictionary<Type, MethodInfo> Methods;

        private readonly string _className;

        /// <summary>
        /// Конструктор базового сборщика сущностей для базы данных.
        /// </summary>
        public ContextBuilder()
        {
            Methods = GetType().GetMethods()
                .Select(methodInfo => new
                {
                    Method = methodInfo,
                    Attribute = methodInfo.GetCustomAttribute<AssignedTypeAttribute>()
                })
                .Where(tempVariable => tempVariable.Attribute != null && tempVariable.Attribute != null)
                .ToDictionary(tempVariable => tempVariable.Attribute.Type, tempVariable => tempVariable.Method);
        }

        /// <summary>
        /// Метод построения таблицы заданного типа по определенному запросу.
        /// </summary>
        /// <param name="query">Запрос на построение таблицы.</param>
        /// <param name="type">Заданный тип таблицы.</param>
        /// <returns>Результат запроса на построение таблицы заданного типа.</returns>
        /// <exception cref="NullReferenceException">Если запрос оказался null.</exception>
        /// <exception cref="CreationDatabaseContextException">Если построение таблицы оказалось невозможным.</exception>
        public IQueryable<BaseEntity> BuildTable(IQueryable<BaseEntity>? query, Type type)
        {
            var logHeader = LogHelper.GetLogHeader(_className, MethodBase.GetCurrentMethod());

            if (query == null)
                throw new NullReferenceException($"[{logHeader}] Запрос на создание сущности типа {type} пустой.");

            var result = Methods[type].Invoke(this, [query]) ??
                throw new CreationDatabaseContextException($"[{logHeader}] Создание сущности типа {type} запросом {query} невозможно.");

            return (IQueryable<BaseEntity>)result;
        }
    }
}
