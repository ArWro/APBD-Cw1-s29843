using System.Text;
using APBD_Cw1.Domain;

namespace APBD_Cw1.Services;

public class ReportService
{
    public string BuildSummary(RentalService rentalService, DateTime referenceDate)
    {
        var allEquipment = rentalService.GetAllEquipment();
        var allRentals = rentalService.GetAllRentals();
        var overdueRentals = rentalService.GetOverdueRentals(referenceDate);

        var availableCount = allEquipment.Count(e => e.Status == EquipmentStatus.Available);
        var rentedCount = allEquipment.Count(e => e.Status == EquipmentStatus.Rented);
        var unavailableCount = allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable);

        var totalPenalty = allRentals.Sum(r => r.PenaltyFee);

        var sb = new StringBuilder();
        sb.AppendLine("Raport stanu wypożyczalni");
        sb.AppendLine("-------------------------");
        sb.AppendLine($"Liczba sprzętów ogółem: {allEquipment.Count}");
        sb.AppendLine($"Dostępne: {availableCount}");
        sb.AppendLine($"Wypożyczone: {rentedCount}");
        sb.AppendLine($"Niedostępne: {unavailableCount}");
        sb.AppendLine($"Aktywne wypożyczenia: {allRentals.Count(r => r.IsActive)}");
        sb.AppendLine($"Przeterminowane wypożyczenia: {overdueRentals.Count}");
        sb.AppendLine($"Suma naliczonych kar: {totalPenalty:C}");

        return sb.ToString();
    }
}