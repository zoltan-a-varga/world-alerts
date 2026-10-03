using Microsoft.AspNetCore.Mvc;
using WorldAlerts.Api.Contracts.Events;
using WorldAlerts.Core.Events;

namespace WorldAlerts.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly EventProcessingService _eventProcessingService;

    public EventsController(
        EventProcessingService eventProcessingService)
    {
        _eventProcessingService = eventProcessingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        var worldEvent = new WorldEvent
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Severity = request.Severity,
            OccurredAt = request.OccurredAt
                ?? DateTimeOffset.UtcNow
        };

        var notificationCount =
            await _eventProcessingService.ProcessAsync(
                worldEvent,
                cancellationToken);

        return Ok(new
        {
            eventId = worldEvent.Id,
            matchedNotifications = notificationCount
        });
    }
}