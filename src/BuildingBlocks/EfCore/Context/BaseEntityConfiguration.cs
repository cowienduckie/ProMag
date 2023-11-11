using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.Context;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    protected abstract void ConfigureProperties(EntityTypeBuilder<TEntity> builder);

    protected abstract void ConfigureRelationships(EntityTypeBuilder<TEntity> builder);
}