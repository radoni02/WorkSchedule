using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.Preference;

namespace WorkSchedule.Application.Interfaces;
public interface IPreferenceRepository
{
    Task<IList<Preference>> GetPreferences(DateOnly day, IList<Domain.User.User> targetedUsers);
    Task<IList<Preference>> GetPreferencesByRange(DateOnly start, DateOnly end, IList<Domain.User.User> targetedUsers);
    Task SetUserPreference(DateTime start, DateTime end, Domain.User.User targetedUser);
    Task ClearUserPreference(DateOnly day, Domain.User.User targetedUser);
    Task SaveChangesAsync();
}
