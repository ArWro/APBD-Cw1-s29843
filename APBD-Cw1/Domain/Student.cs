namespace APBD_Cw1.Domain;

public sealed class Student : User
{
    public Student(string id, string firstName, string lastName)
        : base(id, firstName, lastName, UserType.Student)
    {
    }
}