using Microsoft.EntityFrameworkCore;
using booksy.API.Models.Entities;

namespace booksy.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Hardware> Hardwares { get; set; } = null!;
    public DbSet<RentalRecord> RentalRecords { get; set; } = null!;

    // implement auto timestamping and soft delete
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.DateModified = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.DateModified = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DateDeleted = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(ct);
    }
}
