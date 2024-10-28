using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.Preference;

public class WorkDay : WorkDayBase
{
    public WorkDay(Guid userId, DateTime start, DateTime end) : base(userId, start, end) { }
}
