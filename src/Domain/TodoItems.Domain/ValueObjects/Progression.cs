using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Domain.ValueObjects;

public sealed class Progression
{
    public DateTime Date { get; }
    public decimal Percent { get; }

    public Progression(DateTime date, decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Percent must be between 0 and 100");

        Date = date;
        Percent = percent;
    }
}
