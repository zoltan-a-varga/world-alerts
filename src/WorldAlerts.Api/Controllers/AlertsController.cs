using Microsoft.AspNetCore.Mvc;
using WorldAlerts.Api.Contracts.Alerts;
using WorldAlerts.Core.Alerts;

namespace WorldAlerts.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public sealed class AlertsController : ControllerBase
{
    private readonly IAlertRepository _repository;

    public AlertsController(IAlertRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Alert>>> GetAll(
        CancellationToken cancellationToken)
    {
        var alerts = await _repository.GetAllAsync(cancellationToken);
        return Ok(alerts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Alert>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var alert = await _repository.GetByIdAsync(id, cancellationToken);

        return alert is null
            ? NotFound()
            : Ok(alert);
    }

    [HttpPost]
    public async Task<ActionResult<Alert>> Create(
        CreateAlertRequest request,
        CancellationToken cancellationToken)
    {
        var alert = new Alert
        {
            UserId = request.UserId,
            Name = request.Name,
            EventCategory = request.EventCategory,
            MinimumSeverity = request.MinimumSeverity,
            NotificationChannels = request.NotificationChannels
        };

        await _repository.AddAsync(alert, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = alert.Id },
            alert);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateAlertRequest request,
        CancellationToken cancellationToken)
    {
        var alert = await _repository.GetByIdAsync(id, cancellationToken);

        if (alert is null)
        {
            return NotFound();
        }

        alert.Name = request.Name;
        alert.EventCategory = request.EventCategory;
        alert.MinimumSeverity = request.MinimumSeverity;
        alert.Enabled = request.Enabled;
        alert.NotificationChannels = request.NotificationChannels;

        await _repository.UpdateAsync(alert, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var alert = await _repository.GetByIdAsync(id, cancellationToken);

        if (alert is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(alert, cancellationToken);

        return NoContent();
    }
}