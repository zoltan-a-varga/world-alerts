using Microsoft.EntityFrameworkCore;
using WorldAlerts.Core.Alerts;

namespace WorldAlerts.Infrastructure.Persistence;

public sealed class AlertRepository : IAlertRepository
{
    private readonly AlertsDbContext _dbContext;

    public AlertRepository(AlertsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Alert>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Alerts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Alert?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return _dbContext.Alerts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Alert alert,
        CancellationToken cancellationToken)
    {
        _dbContext.Alerts.Add(alert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Alert alert,
        CancellationToken cancellationToken)
    {
        _dbContext.Alerts.Update(alert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Alert alert,
        CancellationToken cancellationToken)
    {
        _dbContext.Alerts.Remove(alert);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}