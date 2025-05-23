namespace DatabaseAbstractions.Models.CacheModels
{
    /// <summary>
    /// Абстрактный класс сущности отпечатка базы данных.
    /// </summary>
    public abstract class CacheEntity
    {
        /// <summary>
        /// Номер сущности в базе данных.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Проверка на нулевое значение поля Id.
        /// </summary>
        /// <returns>true — если Id имеет значение 0; false — если имеет значение, отличное от нуля. </returns>
        public bool IsIdNull()
        {
            return Id == 0;
        }
    }
}
