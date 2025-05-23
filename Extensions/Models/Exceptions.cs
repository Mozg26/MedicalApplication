namespace Extensions.Models
{
    /// <summary>
    /// Исключение, которое сообщает о том, что запрос к отпечатку или базе данных был неуспешный.
    /// </summary>
    [Serializable]
    public class IncorrectResponseException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что запрос к отпечатку или базе данных был неуспешный.
        /// </summary>
        public IncorrectResponseException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что запрос к отпечатку или базе данных был неуспешный.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public IncorrectResponseException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что запрос к отпечатку или базе данных был неуспешный.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public IncorrectResponseException(string message, Exception? inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что во время парсинга произошла ошибка.
    /// </summary>
    [Serializable]
    public class ParsingException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что во время парсинга произошла ошибка.
        /// </summary>
        public ParsingException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что во время парсинга произошла ошибка.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public ParsingException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что во время парсинга произошла ошибка.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public ParsingException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что указанный тип данных не обрабатывается.
    /// </summary>
    [Serializable]
    public class UnprocessedTypeException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что указанный тип данных не обрабатывается.
        /// </summary>
        public UnprocessedTypeException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что указанный тип данных не обрабатывается.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public UnprocessedTypeException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что указанный тип данных не обрабатывается.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public UnprocessedTypeException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что объект, который не должен был быть найден, уже существует.
    /// </summary>
    [Serializable]
    public class ObjectAlreadyExistsException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что объект, который не должен был быть найден, уже существует.
        /// </summary>
        public ObjectAlreadyExistsException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что объект, который не должен был быть найден, уже существует.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public ObjectAlreadyExistsException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что объект, который не должен был быть найден, уже существует.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public ObjectAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что объект, который должен был существовать, не найден.
    /// </summary>
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что объект, который должен был существовать, не найден.
        /// </summary>
        public ObjectNotFoundException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что объект, который должен был существовать, не найден.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public ObjectNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что объект, который должен был существовать, не найден.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что подключение не удалось осуществить.
    /// </summary>
    [Serializable]
    public class ConnectionErrorException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что подключение не удалось осуществить.
        /// </summary>
        public ConnectionErrorException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что подключение не удалось осуществить.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public ConnectionErrorException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что подключение не удалось осуществить.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public ConnectionErrorException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что не удалось создать контекст базы данных.
    /// </summary>
    [Serializable]
    public class CreationDatabaseContextException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что не удалось создать контекст базы данных.
        /// </summary>
        public CreationDatabaseContextException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что не удалось создать контекст базы данных.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public CreationDatabaseContextException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что не удалось создать контекст базы данных.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public CreationDatabaseContextException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что не удалось успешно конвертировать один тип данных в другой.
    /// </summary>
    [Serializable]
    public class ConvertationException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что не удалось успешно конвертировать один тип данных в другой.
        /// </summary>
        public ConvertationException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что не удалось успешно конвертировать один тип данных в другой.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public ConvertationException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что не удалось успешно конвертировать один тип данных в другой.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public ConvertationException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Исключение, которое сообщает о том, что данные, которые не должны быть пустыми, оказались пустыми.
    /// </summary>
    [Serializable]
    public class NullDataException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию исключения, которое сообщает о том, что данные, которые не должны быть пустыми, оказались пустыми.
        /// </summary>
        public NullDataException() : base() { }

        /// <summary>
        /// Конструктор с сообщением исключения, которое сообщает о том, что данные, которые не должны быть пустыми, оказались пустыми.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public NullDataException(string message) : base(message) { }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением текущего исключения, 
        /// которое сообщает о том, что данные, которые не должны быть пустыми, оказались пустыми.
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        /// <param name="inner">Внутреннее исключение текущего исключения.</param>
        public NullDataException(string message, Exception inner) : base(message, inner) { }
    }
}
