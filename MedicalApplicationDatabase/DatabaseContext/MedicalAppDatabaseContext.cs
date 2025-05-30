using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseShared.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicalApplicationDatabase.DatabaseContext
{
    public class MedicalAppDatabaseContext : DatabaseAbstractions.DatabaseContext.DatabaseContext
    {
        public DbSet<LoginSystemEntity> LoginSystem { get; set; }

        public DbSet<StaffEntity> Staff { get; set; }

        public DbSet<PatientEntity> Patients { get; set; }

        public DbSet<FamilyEntity> Families { get; set; }

        public DbSet<AllergyEntity> Allergies { get; set; }

        public DbSet<VaccinationEntity> Vaccination { get; set; }

        public DbSet<ContraindicationEntity> Contraindications { get; set; }

        public DbSet<ConstantSupervisionEntity> ConstantSupervision { get; set; }

        public DbSet<AppointmentEntity> Appointments { get; set; }

        public DbSet<ReferralEntity> Referrals { get; set; }

        public DbSet<MedicalTestEntity> MedicalTests { get; set; }

        #region TimeTable

        #endregion

        public MedicalAppDatabaseContext(DbContextOptions<MedicalAppDatabaseContext> options) : base(options: options)
        {

        }

        public MedicalAppDatabaseContext(IContextBuilder contextBuilder, DbContextOptions<MedicalAppDatabaseContext> options, ILogger logger) : base(contextBuilder, options, logger)
        {

        }
    }
}
