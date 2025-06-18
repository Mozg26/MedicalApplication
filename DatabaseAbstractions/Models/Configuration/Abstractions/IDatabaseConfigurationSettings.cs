using Extensions.Enums;

namespace DatabaseAbstractions.Models.Configuration.Abstractions
{
    public interface IDatabaseConfigurationSettings : IMigrationsConfigurationSettings, IDatabaseSettings
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
    }

    public interface IMigrationsConfigurationSettings
    {
        public Dictionary<DatabaseType, string> MigrationAssemblies { get; set; }
    }

    public interface IDatabaseSettings
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
    }
}
