using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YourProjectName.Domain.Commons;

public abstract class Entity<T> where T : notnull
{
    public T? Id { get; init; }

    private List<IDomainEvent> _domainEvents = [];

    [JsonIgnore]
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
        _domainEvents = [];
    }
}
