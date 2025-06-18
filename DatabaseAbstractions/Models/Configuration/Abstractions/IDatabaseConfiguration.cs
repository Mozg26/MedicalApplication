using Extensions.Enums;

namespace DatabaseAbstractions.Models.Configuration.Abstractions
{
    public interface IDatabaseConfiguration : IMigrationsConfigurations, IDatabaseConnectionConfiguration
    {

    }

    public interface IMigrationsConfigurations
    {
        public string GetMigrationAssemblyName(DatabaseType type);
    }

    public interface IDatabaseConnectionConfiguration
    {
        public DatabaseType GetDatabaseType();
        public string GetConnectionString();
    }
}
