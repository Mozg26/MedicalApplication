using Extensions.Enums;
using System.Diagnostics;

namespace DatabaseAbstractions.Models.Communication
{
    /// <summary>
    /// Модель данных, отображающая изменения в базе данных извне.
    /// </summary>
    [DebuggerDisplay("Пользователь {User} запрашивает изменение {Content} типа {ChangeType} в таблице {TableName}")]
    public class DatabaseChangeModel
    {
        public string TableName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public CacheUpdateType ChangeType { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
