using DatabaseShared.CacheModels;

namespace DatabaseShared.CacheExtensions
{
    public static class StaffExtensions
    {
        public static IEnumerable<Staff?> BySpecialization(this IEnumerable<Staff?> staff, string specialization)
        {
            return staff
                    .Where(staff => staff?.AccessLevel == "doctor" && staff.Specialization == specialization)
                    .ToList();
        }
    }
}
