using APBD_Cw1.Configuration;
using APBD_Cw1.Domain;

namespace APBD_Cw1.Services;

public class RentalService
{
    private readonly IdGenerator _idGenerator;
    private readonly BusinessRules _businessRules;
    private readonly List<Equipment> _equipment = [];
    private readonly List<User> _users = [];
    private readonly List<Rental> _rentals = [];

    public RentalService(IdGenerator idGenerator, BusinessRules businessRules)
    {
        _idGenerator = idGenerator;
        _businessRules = businessRules;
    }

    public void AddEquipment(Equipment equipment)
    {
        _equipment.Add(equipment);
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public IReadOnlyCollection<Equipment> GetAllEquipment() => _equipment.AsReadOnly();

    public IReadOnlyCollection<Equipment> GetAvailableEquipment()
        => _equipment.Where(e => e.Status == EquipmentStatus.Available).ToList().AsReadOnly();

    public IReadOnlyCollection<User> GetAllUsers() => _users.AsReadOnly();

    public IReadOnlyCollection<Rental> GetAllRentals() => _rentals.AsReadOnly();

    public IReadOnlyCollection<Rental> GetActiveRentalsForUser(string userId)
        => _rentals.Where(r => r.UserId == userId && r.IsActive).ToList().AsReadOnly();

    public IReadOnlyCollection<Rental> GetOverdueRentals(DateTime referenceDate)
        => _rentals.Where(r => r.IsOverdue(referenceDate)).ToList().AsReadOnly();

    public Result RentEquipment(string userId, string equipmentId, DateTime rentedAt, int rentalDays)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            return Result.Failure($"Nie znaleziono użytkownika o ID {userId}.");
        }

        var equipment = _equipment.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment is null)
        {
            return Result.Failure($"Nie znaleziono sprzętu o ID {equipmentId}.");
        }

        if (equipment.Status != EquipmentStatus.Available)
        {
            return Result.Failure($"Sprzęt {equipment.Name} nie jest dostępny do wypożyczenia.");
        }

        var activeRentalsCount = _rentals.Count(r => r.UserId == userId && r.IsActive);
        var userLimit = _businessRules.GetLimitFor(user);

        if (activeRentalsCount >= userLimit)
        {
            return Result.Failure(
                $"Użytkownik {user.FullName} przekroczył limit aktywnych wypożyczeń ({userLimit}).");
        }

        var rental = new Rental(
            _idGenerator.NextRentalId(),
            userId,
            equipmentId,
            rentedAt,
            rentalDays);

        _rentals.Add(rental);
        equipment.MarkAsRented();

        return Result.Success(
            $"Wypożyczono sprzęt {equipment.Name} użytkownikowi {user.FullName} do dnia {rental.DueDate:yyyy-MM-dd}.");
    }

    public Result ReturnEquipment(string rentalId, DateTime returnedAt)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental is null)
        {
            return Result.Failure($"Nie znaleziono wypożyczenia o ID {rentalId}.");
        }

        if (!rental.IsActive)
        {
            return Result.Failure($"Wypożyczenie {rentalId} zostało już wcześniej zamknięte.");
        }

        var equipment = _equipment.First(e => e.Id == rental.EquipmentId);

        var penalty = _businessRules.CalculatePenalty(rental.DueDate, returnedAt);
        rental.CompleteReturn(returnedAt, penalty);
        equipment.MarkAsAvailable();

        if (penalty > 0)
        {
            return Result.Success(
                $"Zwrot zrealizowany. Naliczona kara za opóźnienie: {penalty:C}.");
        }

        return Result.Success("Zwrot zrealizowany w terminie. Kara: 0,00 zł.");
    }

    public Result MarkEquipmentAsUnavailable(string equipmentId, string reason)
    {
        var equipment = _equipment.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment is null)
        {
            return Result.Failure($"Nie znaleziono sprzętu o ID {equipmentId}.");
        }

        if (equipment.Status == EquipmentStatus.Rented)
        {
            return Result.Failure("Nie można oznaczyć wypożyczonego sprzętu jako niedostępnego.");
        }

        equipment.MarkAsUnavailable(reason);
        return Result.Success($"Sprzęt {equipment.Name} oznaczono jako niedostępny. Powód: {reason}.");
    }
}