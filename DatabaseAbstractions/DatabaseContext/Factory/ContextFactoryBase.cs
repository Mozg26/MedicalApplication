using DatabaseAbstractions.DatabaseContext.Abstractions;
using DatabaseAbstractions.Models.Configuration.Abstractions;
using Extensions.Enums;
using Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAbstractions.DatabaseContext.Factory
{
    public class ContextFactoryBase<T> : IContextFactory<T> where T : DatabaseContext
    {
        private readonly DbContextOptions<T> _options;
        private readonly IContextBuilder _contextBuilder;

        public ContextFactoryBase(IDatabaseConfiguration databaseConfiguration, IContextBuilder contextBuilder)
        {
            DatabaseType dbType = databaseConfiguration.GetDatabaseType();
            string connString = databaseConfiguration.GetConnectionString();
            string migrationsAssemblyName = databaseConfiguration.GetMigrationAssemblyName(dbType);

            _contextBuilder = contextBuilder;

            if (dbType == DatabaseType.MSSQL)
            {
                _options = new DbContextOptionsBuilder<T>()
                    .UseSqlServer(connString, opts => opts
                        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                        .MigrationsAssembly($"{migrationsAssemblyName}"))
                        .EnableSensitiveDataLogging(true)
                        .Options;
            }
            //else if (dbType == DatabaseType.PostgreSQL)
            //{
            //    _options = new DbContextOptionsBuilder<T>()
            //        .UseNpgsql(connString, opts => opts
            //            .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
            //            .MigrationsAssembly($"{migrationsAssemblyName}"))
            //            .Options;
            //}
            else
                throw new CreationDatabaseContextException($"[ContextFactoryBase: конструктор] Указанный тип базы данных — [{dbType}] — не поддерживаеся.");

            using var context = CreateContext();

            context.Database.Migrate();
        }


        public T CreateContext()
        {
            if (Activator.CreateInstance(typeof(T), [_contextBuilder, _options]) is not T context)
                throw new CreationDatabaseContextException("[ContextFactoryBase: CreateContext] Не удалось создать контекст базы данных.");

            return context;
        }
    }
}
