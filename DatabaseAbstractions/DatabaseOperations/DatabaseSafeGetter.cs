using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DatabaseAbstractions.DatabaseOperations
{
    public static class DatabaseSafeGetter
    {
        private static ILogger Logger { get; set; } = null!;

        public static void SetLogger(ILogger logger)
        {
            Logger = logger;
        }

        public static DatabaseResponse<T> GetValue<T>(this Func<T> func)
        {
            string valueType = $"[{typeof(T).Name}]";
            if (typeof(T).GetGenericArguments().Length != 0)
                valueType += $" of [{typeof(T).GetGenericArguments()[0]}]";

            string method = func.Method.Name;
            string type = func.GetMethodInfo().ReturnType.ToString();

            try
            {
                T value = func.Invoke();
                var logString = $"[DatabaseSafeGetter: GetValue] Получение {valueType} в методе {type} {method} ЗАВЕРШЕНО";
                Logger.LogInformation(logString);
                DatabaseResponse<T> response = new(value, logString);

                return response;
            }
            catch (SqlException ex)
            {
                var logString = $"[DatabaseSafeGetter: GetValue] Получение {valueType} в методе {type} {method} вызвало ошибку базы данных: {ex.Message}";
                Logger.LogCritical(logString);
                DatabaseResponse<T> response = new(default, "DatabaseSafeGetter", logString, ResponseType.SqlException, false, ex);
                return response;
            }

            catch (Exception ex)
            {
                var logString = $"[DatabaseSafeGetter: GetValue] Получение {valueType} в методе {type} {method} вызвало ошибку: {ex.Message}";
                Logger.LogCritical(logString);
                DatabaseResponse<T> response = new(default, "DatabaseSafeGetter", logString, ResponseType.LogicError, false, ex);
                return response;
            }
        }
    }
}
