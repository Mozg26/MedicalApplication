using DatabaseAbstractions.Models.CacheModels;
using DatabaseShared.Enums;

namespace DatabaseShared.HelpModels.ForCache
{
    public class PersonInfo : CacheEntity
    {
        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string Pathronymic { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string Photo { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public Sex Sex { get; set; }
    }
}
