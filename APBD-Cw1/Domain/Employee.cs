namespace APBD_Cw1.Domain;

public sealed class Employee : User
{
    public Employee(string id, string firstName, string lastName)
        : base(id, firstName, lastName, UserType.Employee)
    {
    }
}