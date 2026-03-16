namespace APBD_Cw1.Domain;

public abstract class Equipment
{
    protected Equipment(string id, string name)
    {
        Id = id;
        Name = name;
        Status = EquipmentStatus.Available;
    }

    public string Id { get; }
    public string Name { get; }
    public EquipmentStatus Status { get; private set; }
    public string? UnavailabilityReason { get; private set; }

    public bool IsAvailable => Status == EquipmentStatus.Available;

    public void MarkAsRented()
    {
        Status = EquipmentStatus.Rented;
        UnavailabilityReason = null;
    }

    public void MarkAsAvailable()
    {
        Status = EquipmentStatus.Available;
        UnavailabilityReason = null;
    }

    public void MarkAsUnavailable(string reason)
    {
        Status = EquipmentStatus.Unavailable;
        UnavailabilityReason = reason;
    }

    public override string ToString()
    {
        var extra = string.IsNullOrWhiteSpace(UnavailabilityReason)
            ? string.Empty
            : $" | Powód niedostępności: {UnavailabilityReason}";

        return $"{GetType().Name} [{Id}] {Name} | Status: {Status}{extra}";
    }
}