using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransitExample.Saga
{
    public class SagaInstanceMap : IEntityTypeConfiguration<SagaInstance>
    {
        public void Configure(EntityTypeBuilder<SagaInstance> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("EfCoreSimpleSagas");
            entityTypeBuilder.Property(x => x.CorrelationId);
            entityTypeBuilder.Property(x => x.CurrentState);
            entityTypeBuilder.Property(x => x.Text);
        }
    }
}