﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.Preference;

public class Preference : WorkDayBase
{
    public Preference(Guid userId, DateTime start, DateTime end) : base(userId, start, end) { }
}
