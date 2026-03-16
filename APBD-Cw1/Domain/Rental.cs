namespace APBD_Cw1.Domain;

public class Rental
{
    public Rental(string id, string userId, string equipmentId, DateTime rentedAt, int rentalDays)
    {
        Id = id;
        UserId = userId;
        EquipmentId = equipmentId;
        RentedAt = rentedAt;
        RentalDays = rentalDays;
        DueDate = rentedAt.AddDays(rentalDays);
    }

    public string Id { get; }
    public string UserId { get; }
    public string EquipmentId { get; }
    public DateTime RentedAt { get; }
    public int RentalDays { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal PenaltyFee { get; private set; }

    public bool IsActive => ReturnedAt is null;
    public bool WasReturnedOnTime => ReturnedAt is not null && ReturnedAt.Value.Date <= DueDate.Date;

    public bool IsOverdue(DateTime referenceDate)
        => IsActive && referenceDate.Date > DueDate.Date;

    public void CompleteReturn(DateTime returnedAt, decimal penaltyFee)
    {
        ReturnedAt = returnedAt;
        PenaltyFee = penaltyFee;
    }

    public override string ToString()
    {
        var status = IsActive
            ? "Aktywne"
            : $"Zwrócone: {ReturnedAt:yyyy-MM-dd}, kara: {PenaltyFee:C}";

        return $"Wypożyczenie [{Id}] | Użytkownik: {UserId} | Sprzęt: {EquipmentId} | " +
               $"Od: {RentedAt:yyyy-MM-dd} | Termin: {DueDate:yyyy-MM-dd} | Status: {status}";
    }
}