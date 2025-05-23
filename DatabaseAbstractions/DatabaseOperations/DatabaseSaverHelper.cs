using DatabaseAbstractions.Models.Communication;
using DatabaseAbstractions.Models.Enums;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace DatabaseAbstractions.DatabaseOperations
{
    public static class DatabaseSaverHelper
    {
        public static DatabaseResponse SaveValues(this Action func)
        {
            string method = func.Method.Name;
            string type = func.GetMethodInfo().ReturnType.Name.ToString();

            try
            {
                func?.Invoke();
                DatabaseResponse response =
                    new($"DatabaseSaverHelper: SaveValues; {type}: {method}", $"[DatabaseSafeSaver: SaveValues] Сохранение из метода {type} {method} в локальную базу: Успешно!");
                return response;
            }
            catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException)
            {
                string message = $"[DatabaseSafeSaver: SaveValues; {type}: {method}] Сохранение вызвало ошибку базы данных: {ex.Message}, {ex.StackTrace}";

                DatabaseResponse response = new($"DatabaseSaverHelper: SaveValues; {type}: {method}", message, ResponseType.DatabaseAccessError, false, ex);
                return response;
            }
            catch (Exception ex)
            {
                string message = $"[DatabaseSafeSaver: SaveValues; {type}: {method}] Сохранение вызвало неизвестную ошибку: {ex.Message}, {ex.StackTrace}";

                DatabaseResponse response = new($"DatabaseSafeSaver: SaveValues; {type}: {method}", message, ResponseType.LogicError, false, ex);
                return response;
            }
        }
    }
}
