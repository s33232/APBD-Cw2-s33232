using System;

namespace Cw2.Services;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(DateTime expectedReturn, DateTime actualReturn);
}