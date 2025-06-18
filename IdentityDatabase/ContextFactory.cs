using Extensions.Enums;
using IdentityDatabase.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace IdentityDatabase
{
    public class ContextFactory : IContextFactory
    {
        private readonly DbContextOptions<MainContext> _options;

        public ContextFactory(string connString, DatabaseType dbType = DatabaseType.MSSQL, bool tryConnect = true)
        {
            if (dbType == DatabaseType.MSSQL)
            {
                _options = new DbContextOptionsBuilder<MainContext>()
                    .UseSqlServer(connString, opts => opts
                        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                        .MigrationsAssembly("IdentityMigrationsSqlServer"))
                        .Options;
            }
            else
            {
                throw new Exception($"Указанный в appsetings 'DBType' - {dbType} не поддерживаеся!");
            }

            if (!tryConnect)
                return;

            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public MainContext CreateContext()
        {
            return new MainContext(_options);
        }
    }
}
