using Microsoft.AspNetCore.Mvc;
using WorldAlerts.Core.Alerts;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Api.Controllers;

[ApiController]
[Route("api/admin")]
public sealed class AdminController : ControllerBase
{
    private readonly IAlertRepository _alertRepository;
    private readonly INotificationAttemptRepository
        _notificationAttemptRepository;

    public AdminController(
        IAlertRepository alertRepository,
        INotificationAttemptRepository notificationAttemptRepository)
    {
        _alertRepository = alertRepository;
        _notificationAttemptRepository =
            notificationAttemptRepository;
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetAlerts(
        CancellationToken cancellationToken)
    {
        var alerts =
            await _alertRepository.GetAllAsync(cancellationToken);

        return Ok(alerts);
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications(
        CancellationToken cancellationToken)
    {
        var notifications =
            await _notificationAttemptRepository.GetAllAsync(
                cancellationToken);

        return Ok(notifications);
    }
}