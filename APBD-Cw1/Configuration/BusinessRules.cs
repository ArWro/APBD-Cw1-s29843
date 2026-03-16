using APBD_Cw1.Domain;

namespace APBD_Cw1.Configuration;

public class BusinessRules
{
    public BusinessRules(
        int maxStudentActiveRentals,
        int maxEmployeeActiveRentals,
        decimal penaltyPerDayLate)
    {
        MaxStudentActiveRentals = maxStudentActiveRentals;
        MaxEmployeeActiveRentals = maxEmployeeActiveRentals;
        PenaltyPerDayLate = penaltyPerDayLate;
    }

    public int MaxStudentActiveRentals { get; }
    public int MaxEmployeeActiveRentals { get; }
    public decimal PenaltyPerDayLate { get; }

    public int GetLimitFor(User user)
        => user.UserType switch
        {
            UserType.Student => MaxStudentActiveRentals,
            UserType.Employee => MaxEmployeeActiveRentals,
            _ => throw new InvalidOperationException("Nieobsługiwany typ użytkownika.")
        };

    public decimal CalculatePenalty(DateTime dueDate, DateTime returnedAt)
    {
        var lateDays = (returnedAt.Date - dueDate.Date).Days;
        if (lateDays <= 0)
        {
            return 0m;
        }

        return lateDays * PenaltyPerDayLate;
    }

    public static BusinessRules Default => new(
        maxStudentActiveRentals: 2,
        maxEmployeeActiveRentals: 5,
        penaltyPerDayLate: 15m);
}