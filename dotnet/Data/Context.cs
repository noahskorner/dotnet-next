using Data.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public abstract class Context : DbContext
    {
        private readonly IDateService _dateService;

        public Context(
            DbContextOptions options,
            IDateService dateService) : base(options)
        {
            _dateService = dateService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException("Use SaveChangesAsync instead.");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Use SaveChangesAsync instead.");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Auditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Auditable)entityEntry.Entity).UpdatedAt = _dateService.UtcNow();

                if (entityEntry.State == EntityState.Added)
                {
                    ((Auditable)entityEntry.Entity).CreatedAt = _dateService.UtcNow();
                }
            }
        }
    }
}
