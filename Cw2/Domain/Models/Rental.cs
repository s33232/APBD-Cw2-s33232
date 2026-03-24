using System;

namespace Cw2.Domain.Models;

public class Rental
{
    public Guid Id { get; } = Guid.NewGuid();
    public User RentedBy { get; set; }
    public Equipment RentedEquipment { get; set; }
    public DateTime RentDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal PenaltyFee { get; set; }

    public bool IsActive => ActualReturnDate == null;
    public bool IsOverdue => IsActive && DateTime.Now > ExpectedReturnDate;
}