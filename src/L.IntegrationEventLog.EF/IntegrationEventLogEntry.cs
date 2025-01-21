using L.EventBus.Core.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace L.IntegrationEventLog.EF;

public class IntegrationEventLogEntry
{
    public Guid EventId { get; private set; }
    [Required]
    public string EventTypeName { get; private set; } = null!;
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    public EventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreatedOn { get; private set; }
    [Required]
    public string Content { get; private set; } = null!;
    public Guid TransactionId { get; private set; }

    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreatedOn = @event.CreatedOn;
        EventTypeName = @event.GetType().FullName!;
        Content = JsonSerializer.Serialize(@event, @event.GetType());
        State = EventState.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }

    public IntegrationEventLogEntry DeserializeJsonContent(Type typeToDeserialize)
    {
        IntegrationEvent = (JsonSerializer.Deserialize(Content, typeToDeserialize) as IntegrationEvent)!;
        return this;
    }
}