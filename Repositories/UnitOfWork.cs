using Microsoft.EntityFrameworkCore.ChangeTracking;

using FastDeliveryAPI.Data;
using Microsoft.EntityFrameworkCore;
using FastDeliveryAPI.Repositories.Interfaces;

namespace FastDeliveryAPI.Repositories;

internal sealed class UnitOfWork : IUnitOfWorks
{
    private readonly FastDeliveryDbContext _dbContext;

    public UnitOfWork(FastDeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntites();
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    private void UpdateAuditableEntites()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc)
                           .CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc)
                           .CurrentValue =DateTime.UtcNow;
            }
        }
    }
}