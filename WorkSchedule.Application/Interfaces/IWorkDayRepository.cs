using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.Preference;

namespace WorkSchedule.Application.Interfaces;
public interface IWorkDayRepository
{
    Task<IList<WorkDay>> GetWorkday(DateOnly day, IList<Domain.User.User> targetedUsers);
    Task<IList<WorkDay>> GetWorkdaysByRange(DateOnly start, DateOnly end, IList<Domain.User.User> targetedUsers);
    Task SetUserWorkday(DateTime start, DateTime end, Domain.User.User targetedUser);
    Task TransferUserPreference(DateTime start, DateTime end, Domain.User.User targetedUser);
    Task ClearUserWorkday(DateOnly day, Domain.User.User targetedUser);
    Task SaveChangesAsync();
}
