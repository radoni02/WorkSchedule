using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.Preference;

namespace WorkSchedule.Infrastructure.Data.Repository;
public class PreferenceRepository : IPreferenceRepository
{
    private readonly WorkScheduleContext _context;

    public PreferenceRepository(WorkScheduleContext context)
    {
        _context = context;
    }
    public async Task<IList<Preference>> GetPreferences(DateOnly day, IList<Domain.User.User> targetedUsers)
    {
        DateTime localStart = day.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = day.ToDateTime(TimeOnly.MaxValue);

        IQueryable<Preference> filteredPreferences = _context.Preferences.AsQueryable();
        if(targetedUsers.Count > 0)
        {
            List<Guid> userIds = targetedUsers.Select(user => user.Id).ToList();
            filteredPreferences = filteredPreferences.Where(x => userIds.Contains(x.UserId));
        }
        filteredPreferences = filteredPreferences.Where(x => localStart.CompareTo(x.Start) >= 0 && localEnd.CompareTo(x.End) <= 0);

        return await filteredPreferences.ToListAsync();
    }
    public async Task<IList<Preference>> GetPreferencesByRange(DateOnly start, DateOnly end, IList<Domain.User.User> targetedUsers)
    {
        DateTime localStart = start.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = end.ToDateTime(TimeOnly.MaxValue);

        IQueryable<Preference> filteredPreferences = _context.Preferences.AsQueryable();
        if (targetedUsers.Count > 0)
        {
            List<Guid> userIds = targetedUsers.Select(user => user.Id).ToList();
            filteredPreferences = filteredPreferences.Where(x => userIds.Contains(x.UserId));
        }
        filteredPreferences = filteredPreferences.Where(x => localStart.CompareTo(x.Start) >= 0 && localEnd.CompareTo(x.End) <= 0);

        return await filteredPreferences.ToListAsync();
    }
    public async Task SetUserPreference(DateTime start, DateTime end, Domain.User.User targetedUser)
    {
        if(start.Day != end.Day)
        {
            throw new Exception("Preferences cannot be set across two days!");
        }

        List<Preference> presentPreferences = await _context.Preferences
            .Where(y => y.UserId.Equals(targetedUser.Id) && start.CompareTo(y.End) <= 0 && end.CompareTo(y.Start) >= 0)
            .ToListAsync();

        DateTime finalStart = start;
        DateTime finalEnd = end;
        if(presentPreferences.Count > 0)
        {
            finalStart = presentPreferences.Min(x => x.Start);
            finalEnd = presentPreferences.Max(x => x.End);
            _context.Preferences.RemoveRange(presentPreferences);
        }
        await _context.Preferences.AddAsync(new Preference(targetedUser.Id, finalStart, finalEnd));
        await _context.SaveChangesAsync();
    }
    public async Task ClearUserPreference(DateOnly day, Domain.User.User targetedUser)
    {
        DateTime localStart = day.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = day.ToDateTime(TimeOnly.MaxValue);

        List<Preference> foundPreferences = await _context.Preferences
            .Where(x => x.UserId.Equals(targetedUser.Id) && localStart.CompareTo(x.Start) >= 0 && localEnd.CompareTo(x.End) <= 0)
            .ToListAsync();

        _context.Preferences.RemoveRange(foundPreferences);
        await _context.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
