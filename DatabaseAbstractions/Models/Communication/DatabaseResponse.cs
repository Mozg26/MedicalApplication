using DatabaseAbstractions.Models.Communication.Abstractions;
using DatabaseAbstractions.Models.Enums;

namespace DatabaseAbstractions.Models.Communication
{
    /// <summary>
    /// Модель ответа на запрос к базе данных.
    /// </summary>
    /// <param name="source">Источник запроса.</param>
    /// <param name="description">Описание состояния запроса.</param>
    /// <param name="isCorrect">Корректность выполнения запроса.</param>
    /// <param name="ex">Исключение, если отловили.</param>
    public class DatabaseResponse
        (string source, string description = "Correct", ResponseType responseErrorType = ResponseType.Correct, bool isCorrect = true, Exception? ex = null) : IResponse
    {
        /// <summary>
        /// Источник запроса.
        /// </summary>
        public string Source { get; set; } = source;

        /// <summary>
        /// Исключение, если отловили.
        /// </summary>
        public Exception? Ex { get; set; } = ex;

        public string Description { get; set; } = description;

        public bool IsCorrect { get; set; } = isCorrect;

        public ResponseType ResponseType { get; set; } = responseErrorType;

        /// <summary>
        /// Преобразование в строку информации об ответе на запрос.
        /// </summary>
        /// <returns>Строка с информацией.</returns>
        public override string ToString()
        {
            string result;

            if (!IsCorrect)
                result = $"Ошибка базы данных: {Description}";
            else
                result = $"Источник: {Source}, Описание: {Description}";

            return result;
        }

        /// <summary>
        /// Получение строки ошибки, если поймано исключение, то с информацией по нему.
        /// </summary>
        /// <returns>Строка с информацией по ошибке.</returns>
        public string GetErrorString()
        {
            string result = $"[DatabaseResponse: GetErrorString] Источник: {Source}, Описание: {Description}";

            if (Ex != null)
                result += $", Исключение: {Ex.Message}";

            return result;
        }

        /// <summary>
        /// Ответ о корректном выполнении запроса.
        /// </summary>
        /// <param name="source">Источник запроса.</param>
        /// <returns>Корректный ответ.</returns>
        public static DatabaseResponse CorrectResponse(string source)
        {
            return new DatabaseResponse(source);
        }

        public static DatabaseResponse IncorrectResponse(string source, string decription = "Incorrect", ResponseType errorType = ResponseType.LogicError, Exception? ex = null)
        {
            return new DatabaseResponse(source, decription, errorType, false, ex);
        }
    }

    /// <summary>
    /// Модель ответа (с содержимым) на запрос к базе данных.
    /// </summary>
    /// <typeparam name="T">Тип содержимого.</typeparam>
    /// <param name="source">Источник запроса.</param>
    /// <param name="content">Содержимое ответа.</param>
    /// <param name="description">Описание состояния запроса.</param>
    /// <param name="isCorrect">Корректность выполнения запроса.</param>
    /// <param name="ex">Исключение, если отловили.</param>
    public class DatabaseResponse<T>
        (T? content, string source, string description = "Correct", ResponseType responseErrorType = ResponseType.Correct, bool isCorrect = true, Exception? ex = null)
        : DatabaseResponse(source, description, responseErrorType, isCorrect, ex), IResponse<T>
    {
        public T? Content { get; set; } = content;

        public static DatabaseResponse<T> CorrectResponse(T content, string source)
        {
            return new DatabaseResponse<T>(content, source);
        }

        public new static DatabaseResponse<T> IncorrectResponse(string source, string decription = "Incorrect", ResponseType errorType = ResponseType.LogicError, Exception? ex = null)
        {
            return new DatabaseResponse<T>(default, source, decription, errorType, false, ex);
        }
    }
}
