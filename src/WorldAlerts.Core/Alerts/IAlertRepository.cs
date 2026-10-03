namespace WorldAlerts.Core.Alerts;

public interface IAlertRepository
{
    Task<IReadOnlyCollection<Alert>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Alert?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Alert alert,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Alert alert,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Alert alert,
        CancellationToken cancellationToken);
}