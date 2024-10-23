using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.Preference;

public abstract class WorkDayBase
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }
}
