using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using booksy.API.Models.Entities;

namespace booksy.API.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Hardware> Hardwares { get; set; } = null!;
    public DbSet<RentalRecord> RentalRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "1" },
            new IdentityRole<int> { Id = 2, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "2" }
        );
    }

    // auto timestamping and soft delete
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var baseEntityEntries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in baseEntityEntries)
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

        var userEntries = ChangeTracker.Entries<User>();
        foreach (var entry in userEntries)
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
