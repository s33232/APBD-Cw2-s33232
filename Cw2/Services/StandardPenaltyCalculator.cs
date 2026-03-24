using System;

namespace Cw2.Services;

public class StandardPenaltyCalculator : IPenaltyCalculator
{
    private const decimal DailyPenaltyRate = 10.50m;

    public decimal CalculatePenalty(DateTime expectedReturn, DateTime actualReturn)
    {
        if (actualReturn <= expectedReturn) return 0;
        
        var daysLate = (actualReturn - expectedReturn).Days;
        return daysLate * DailyPenaltyRate;
    }
}