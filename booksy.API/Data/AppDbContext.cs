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

    public DbSet<Hardware> Hardware => Set<Hardware>();
    public DbSet<RentalRecord> RentalRecords => Set<RentalRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Hardware>().HasQueryFilter(h => h.DateDeleted == null);
        builder.Entity<RentalRecord>().HasQueryFilter(r => r.DateDeleted == null);

        // seed roles
        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "1" },
            new IdentityRole<int> { Id = 2, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "2" }
        );

        builder.Entity<RentalRecord>()
               .HasOne(r => r.Hardware)
               .WithOne(h => h.RentalRecord)
               .HasForeignKey<RentalRecord>(r => r.HardwareId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<User>()
               .HasMany(u => u.Rentals)
               .WithOne(r => r.User)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        // only one active rental per hardware at a time
        builder.Entity<RentalRecord>()
               .HasIndex(r => new { r.HardwareId, r.ReturnedAt })
               .HasFilter("\"ReturnedAt\" IS NULL")
               .IsUnique();
    }

    // auto timestamping
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var baseEntityEntries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in baseEntityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.DateModified = DateTime.UtcNow;
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
                    break;
                case EntityState.Modified:
                    entry.Entity.DateModified = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(ct);
    }
}
