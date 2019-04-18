using System.Reflection;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MassTransitExample.Saga
{
    public class ContextFactory : IDesignTimeDbContextFactory<SagaDbContext<SagaInstance, SagaInstanceMap>>
    {
        public SagaDbContext<SagaInstance, SagaInstanceMap> CreateDbContext(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SagaDbContext<SagaInstance, SagaInstanceMap>>();

            dbContextOptionsBuilder.UseSqlServer(EnvironmentSettings.DbConnectionString,
                m =>
                {
                    var executingAssembly = typeof(ContextFactory).GetTypeInfo().Assembly;
                    m.MigrationsAssembly(executingAssembly.GetName().Name);
                });

            return new SagaDbContext<SagaInstance, SagaInstanceMap>(dbContextOptionsBuilder.Options);
        }
    }
}