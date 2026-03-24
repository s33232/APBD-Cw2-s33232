using System;

namespace Cw2.Domain.Models;

public abstract class User
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class Student : User
{
    public override int MaxActiveRentals => 2;
    public Student(string first, string last) : base(first, last) { }
}

public class Employee : User
{
    public override int MaxActiveRentals => 5;
    public Employee(string first, string last) : base(first, last) { }
}