using System;
using Cw2.Domain.Models;
using Cw2.Services;

namespace Cw2;

class Program
{
    static void Main()
    {
        Console.WriteLine("University Equipment Rental System");
        
        var penaltyCalculator = new StandardPenaltyCalculator();
        var manager = new RentalManager(penaltyCalculator);
        
        var laptop = new Laptop("Dell XPS 15", "Intel i7", 16);
        var projector = new Projector("Epson 1080p", "1920x1080", 3000);
        var camera = new Camera("Sony A7", 24, "50mm f/1.8");
        manager.AddEquipment(laptop);
        manager.AddEquipment(projector);
        manager.AddEquipment(camera);
        
        var student = new Student("John", "Doe");
        var employee = new Employee("Dr. Smith", "Brown");
        manager.AddUser(student);
        manager.AddUser(employee);
        
        Console.WriteLine("\n[Successful Rental]");
        var rental1 = manager.RentEquipment(student, laptop, 7);
        Console.WriteLine($"{student.FirstName} successfully rented {laptop.Name}");
        
        Console.WriteLine("\n[Attempting to rent unavailable equipment]");
        try
        {
            manager.RentEquipment(employee, laptop, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        Console.WriteLine("\n[Attempting to exceed rental limit]");
        var rental2 = manager.RentEquipment(student, projector, 2);
        Console.WriteLine($"{student.FirstName} successfully rented {projector.Name}");
        try
        {
            manager.RentEquipment(student, camera, 2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        Console.WriteLine("\n[On-time Return]");
        manager.ReturnEquipment(rental1);
        Console.WriteLine($"{rental1.RentedEquipment.Name} returned on time. Penalty: {rental1.PenaltyFee} PLN");
        
        Console.WriteLine("\n[Late Return with Penalty]");
        rental2.ExpectedReturnDate = DateTime.Now.AddDays(-5);
        manager.ReturnEquipment(rental2);
        Console.WriteLine($"{rental2.RentedEquipment.Name} returned late. Penalty: {rental2.PenaltyFee} PLN");
        
        Console.WriteLine("\n[Final System Report]");
        foreach (var eq in manager.GetAllEquipment())
        {
            Console.WriteLine($"- {eq.Name} | Available: {eq.IsAvailable}");
        }
    }
}