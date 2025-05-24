using BookingSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Member configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.DateJoined).IsRequired();
        });

        // InventoryItem configuration
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.ExpirationDate).IsRequired();
            entity.Property(e => e.RemainingCount).IsRequired();
            entity.HasIndex(e => new { e.Name, e.Description }).IsUnique();
        });

        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ReferenceNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ReferenceNumber).IsUnique();
            entity.Property(e => e.BookingDate).IsRequired();
            entity.Property(e => e.IsCancelled).IsRequired().HasDefaultValue(false);

            // Relationships
            entity.HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.InventoryItem)
                .WithMany(i => i.Bookings)
                .HasForeignKey(b => b.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
