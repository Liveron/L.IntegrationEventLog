using L.Heritage.Shared.Types.Integration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace L.IntegrationEventLog.EF;

public class IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
{
    public Guid EventId { get; private set; } = @event.Id;
    [Required]
    public string EventTypeName { get; private set; } = @event.GetType().FullName!;
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    public EventState State { get; set; } = EventState.NotPublished;
    public int TimesSent { get; set; } = 0;
    public DateTime CreatedOn { get; private set; } = @event.CreatedAt;
    [Required]
    public string Content { get; private set; } = JsonSerializer.Serialize(@event, @event.GetType());
    public Guid TransactionId { get; private set; } = transactionId;

    public IntegrationEventLogEntry DeserializeJsonContent(Type typeToDeserialize)
    {
        IntegrationEvent = (JsonSerializer.Deserialize(Content, typeToDeserialize) as IntegrationEvent)!;
        return this;
    }
}