using Microsoft.EntityFrameworkCore;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Infrastructure.Persistence;

public sealed class NotificationAttemptRepository
    : INotificationAttemptRepository
{
    private readonly AlertsDbContext _dbContext;

    public NotificationAttemptRepository(
        AlertsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        NotificationAttempt attempt,
        CancellationToken cancellationToken)
    {
        _dbContext.NotificationAttempts.Add(attempt);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<NotificationAttempt>> GetAllAsync(
    CancellationToken cancellationToken)
    {
        var attempts = await _dbContext.NotificationAttempts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attempts
            .OrderByDescending(x => x.CreatedAt)
            .ToArray();
    }
}