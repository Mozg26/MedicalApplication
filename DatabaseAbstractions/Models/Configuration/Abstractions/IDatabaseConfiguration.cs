using Extensions.Enums;

namespace DatabaseAbstractions.Models.Configuration.Abstractions
{
    public interface IDatabaseConfiguration : IMigrationsConfigurations, IDatabaseConnectionConfiguration
    {

    }

    public interface IMigrationsConfigurations : IConfigurationService
    {
        public string GetMigrationAssemblyName(DatabaseType type);
    }

    public interface IDatabaseConnectionConfiguration : IConfigurationService
    {
        public DatabaseType GetDatabaseType();
        public string GetConnectionString();
    }
}
