using DatabaseAbstractions.Models.Enums;

namespace DatabaseAbstractions.Models.Communication.Abstractions
{
    /// <summary>
    /// Интерфейс ответа хранилища данных на различные запросы к нему.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Развернутое описание ответа.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Корректность ответа на запрос.
        /// </summary>
        bool IsCorrect { get; set; }

        /// <summary>
        /// Тип ответа.
        /// </summary>
        ResponseType ResponseType { get; set; }
    }

    /// <summary>
    /// Интерфейс ответа (с содержимым) хранилища данных на различные запросы к нему.
    /// </summary>
    /// <typeparam name="T">Тип содержимого ответа.</typeparam>
    public interface IResponse<T> : IResponse
    {
        /// <summary>
        /// Содержимое ответа.
        /// </summary>
        T? Content { get; set; }
    }
}
