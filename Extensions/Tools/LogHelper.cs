using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;

namespace Extensions.Tools
{
    /// <summary>
    /// Статический класс, содержащий методы для генерации заголовка логгера.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Вспомогательный метод, отвечающий за получение имени метода для логирования.
        /// </summary>
        /// <param name="className">Имя класса.</param>
        /// <param name="methodInfo">Информация о методе.</param>
        /// <returns>Заголовок лога, сформированный из переданных параметров.</returns>
        public static string GetLogHeader(string className, MethodBase? methodInfo)
        {
            string methodName;

            if (methodInfo == null)
                methodName = "неизвестный метод";
            else
                methodName = methodInfo.Name;

            return $"{className}: {methodName}";
        }

        /// <summary>
        /// Вспомогательный метод, отвечающий за получение имени метода для логирования.
        /// </summary>
        /// <param name="className">Имя класса.</param>
        /// <param name="methodInfo">Информация о методе.</param>
        /// <param name="transactionId">Номер транзакции.</param>
        /// <returns>Заголовок лога, сформированный из переданных параметров.</returns>
        public static string GetLogHeader(string className, MethodBase? methodInfo, Guid transactionId)
        {
            string methodName;

            if (methodInfo == null)
                methodName = "неизвестный метод";
            else
                methodName = methodInfo.Name;

            return $"{className}: {methodName}; номер транзакции: {transactionId}";
        }

        /// <summary>
        /// Метод для генерации заголовка логгера для асинхронных методов.
        /// </summary>
        /// <param name="className">Имя класса.</param>
        /// <returns>Заголов логгера для асинхронного метода.</returns>
        public static string GetLogHeaderForAsyncMethods(string className)
        {
            var currentMethodName = MethodBase.GetCurrentMethod()?.Name;

            //Чтобы извлечь имя асинхронного метода, необходимо создать текущий стек вызовов.
            var stackTrace = new StackTrace();

            //Фрейм - это запись о вызове метода. Берем всю пачку.
            var stackFrames = stackTrace.GetFrames();

            /* Это интересно: выполнение асинхронных методов.
             * Компилятор преобразует асинхронный метод в конечный автомат, чтобы управлять асинхронным выполнением.
             * Это преобразование включает в себя создание класса-обертки, который реализует интерфейс IAsyncStateMachine.
             * Этот класс управляет состоянием выполнения и содержит методы MoveNext и SetStateMachine. Метод MoveNext вызывается для продвижения 
             * выполнения асинхронного метода к следующему ожидаемому результату. Он управляет тем, когда и как продолжить выполнение после await.
             * В некоторых контекстах (при использовании async методов в Task.Run), компилятор может создать дополнительные методы (Start)
             * для управления выполнением задачи.*/

            //Выбираем первый фрейм, в котором имя метода не равно Start, MoveNext или имени текущего метода.
            var stackFrame = stackFrames.Where(stackFrame => stackFrame.GetMethod()?.Name != "Start" || stackFrame.GetMethod()?.Name != "MoveNext" ||
                stackFrame.GetMethod()?.Name != currentMethodName).FirstOrDefault();

            //Выгружаем информацию о методе.
            var methodInfo = stackFrame?.GetMethod();

            string methodName;

            if (methodInfo == null)
                methodName = "неизвестный метод";
            else
                methodName = methodInfo.Name;

            return $"{className}: {methodName}";
        }

        /// <summary>
        /// Получение сообщения исключения.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <returns>Строка с описанием исключения.</returns>
        public static string GetExceptionMessage(Exception? exception)
        {
            if (exception == null)
                return string.Empty;

            var errorMessage = $"Исключение:\n{exception.Message}\nЖурнал вызовов:\n{exception.StackTrace}";

            if (exception.InnerException != null)
                errorMessage += $"\nВнутреннее исключение: {exception.InnerException.Message}\nЖурнал вызовов внутреннего сообщения: {exception.StackTrace}";

            return errorMessage;
        }

        /// <summary>
        /// Составление сообщения об ошибке с исключением.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="exception">Исключение</param>
        /// <returns>Строка с сообщением об ошибки и описанием исключения.</returns>
        public static string GetExceptionMessage(string message, Exception? exception)
        {
            if (exception == null)
                return message;

            string errorMessage = message;

            switch (exception)
            {
                case SocketException:
                    errorMessage += $"Код ошибки: {((SocketException)exception).ErrorCode}. ";
                    break;
                case ArgumentNullException:
                    errorMessage += $"Имя параметра, который оказался null: {((ArgumentNullException)exception).ParamName}. ";
                    break;
            }

            errorMessage += GetExceptionMessage(exception);

            return errorMessage;
        }
    }
}
