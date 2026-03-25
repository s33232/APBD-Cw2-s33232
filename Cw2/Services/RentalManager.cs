using System;
using System.Collections.Generic;
using System.Linq;
using Cw2.Domain.Models;

namespace Cw2.Services;

public class RentalManager
{
    private readonly List<Equipment> _equipmentList = new();
    private readonly List<User> _users = new();
    private readonly List<Rental> _rentals = new();
    
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalManager(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }
    
    public void AddEquipment(Equipment equipment) => _equipmentList.Add(equipment);
    public void AddUser(User user) => _users.Add(user);
    
    public IEnumerable<Equipment> GetAllEquipment() => _equipmentList;
    public IEnumerable<Equipment> GetAvailableEquipment() => _equipmentList.Where(e => e.IsAvailable);
    
    public Rental RentEquipment(User user, Equipment equipment, int days)
    {
        if (!equipment.IsAvailable) 
            throw new InvalidOperationException($"Equipment {equipment.Name} is not available for rent.");

        var activeRentalsCount = _rentals.Count(r => r.RentedBy.Id == user.Id && r.IsActive);
        if (activeRentalsCount >= user.MaxActiveRentals)
            throw new InvalidOperationException($"User exceeded the rental limit (max: {user.MaxActiveRentals}).");

        var rental = new Rental
        {
            RentedBy = user,
            RentedEquipment = equipment,
            RentDate = DateTime.Now,
            ExpectedReturnDate = DateTime.Now.AddDays(days)
        };

        equipment.IsAvailable = false;
        _rentals.Add(rental);
        return rental;
    }
    
    public void ReturnEquipment(Rental rental)
    {
        if (!rental.IsActive) throw new InvalidOperationException("This equipment has already been returned.");

        rental.ActualReturnDate = DateTime.Now;
        rental.RentedEquipment.IsAvailable = true;
        rental.PenaltyFee = _penaltyCalculator.CalculatePenalty(rental.ExpectedReturnDate, rental.ActualReturnDate.Value);
    }
    
    public void MarkAsUnavailable(Equipment equipment) => equipment.IsAvailable = false;
    public IEnumerable<Rental> GetActiveRentalsForUser(User user) => _rentals.Where(r => r.RentedBy.Id == user.Id && r.IsActive);
    public IEnumerable<Rental> GetOverdueRentals() => _rentals.Where(r => r.IsOverdue);
}