using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.Preference;

public abstract class WorkDayBase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public WorkDayBase(Guid userId, DateTime start, DateTime end)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Start = start;
        End = end;
    }
}
