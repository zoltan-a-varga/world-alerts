using Microsoft.EntityFrameworkCore;
using WorldAlerts.Core.Alerts;
using WorldAlerts.Core.Common;

namespace WorldAlerts.Infrastructure.Persistence;

public sealed class AlertsDbContext : DbContext
{
    public AlertsDbContext(DbContextOptions<AlertsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var alert = modelBuilder.Entity<Alert>();

        alert.HasKey(x => x.Id);

        alert.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(100);

        alert.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        alert.Property(x => x.EventCategory)
            .IsRequired();

        alert.Property(x => x.MinimumSeverity)
            .IsRequired();

        alert.Property(x => x.NotificationChannels)
            .HasConversion(
                channels => string.Join(",", channels.Select(x => (int)x)),
                value => value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => (NotificationChannelType)int.Parse(x))
                    .ToArray());
    }
}