using DatabaseShared.HelpModels.ForCache;

namespace DatabaseShared.CacheModels
{
    public class Staff : PersonInfo
    {
        public string AccessLevel { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public int WorkExperince { get; set; }
    }
}
