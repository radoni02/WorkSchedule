using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.Preference;

namespace WorkSchedule.Infrastructure.Data.Repository;
public class WorkDayRepository : IWorkDayRepository
{
    private readonly WorkScheduleContext _context;

    public WorkDayRepository(WorkScheduleContext context)
    {
        _context = context;
    }
    public async Task<IList<WorkDay>> GetWorkday(DateOnly day, IList<Domain.User.User> targetedUsers)
    {
        DateTime localStart = day.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = day.ToDateTime(TimeOnly.MaxValue);

        IQueryable<WorkDay> filteredWorkdays = _context.WorkDays.AsQueryable();
        if (targetedUsers.Count > 0)
        {
            List<Guid> userIds = targetedUsers.Select(user => user.Id).ToList();
            filteredWorkdays = filteredWorkdays.Where(x => userIds.Contains(x.UserId));
        }
        filteredWorkdays = filteredWorkdays.Where(x => localStart.CompareTo(x.Start) <= 0 && localEnd.CompareTo(x.End) >= 0);

        return await filteredWorkdays.ToListAsync();
    }
    public async Task<IList<WorkDay>> GetWorkdaysByRange(DateOnly start, DateOnly end, IList<Domain.User.User> targetedUsers)
    {
        DateTime localStart = start.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = end.ToDateTime(TimeOnly.MaxValue);

        IQueryable<WorkDay> filteredWorkdays = _context.WorkDays.AsQueryable();
        if (targetedUsers.Count > 0)
        {
            List<Guid> userIds = targetedUsers.Select(user => user.Id).ToList();
            filteredWorkdays = filteredWorkdays.Where(x => userIds.Contains(x.UserId));
        }
        filteredWorkdays = filteredWorkdays.Where(x => localStart.CompareTo(x.Start) <= 0 && localEnd.CompareTo(x.End) >= 0);

        return await filteredWorkdays.ToListAsync();
    }
    public async Task SetUserWorkday(DateTime start, DateTime end, Domain.User.User targetedUser)
    {
        if (start.Day != end.Day)
        {
            throw new Exception("Workdays cannot be set across two days!");
        }

        List<WorkDay> presentWorkdays = await _context.WorkDays
            .Where(y => y.UserId.Equals(targetedUser.Id) && start.CompareTo(y.End) <= 0 && end.CompareTo(y.Start) >= 0)
            .ToListAsync();

        DateTime finalStart = start;
        DateTime finalEnd = end;
        if (presentWorkdays.Count > 0)
        {
            finalStart = presentWorkdays.Min(x => x.Start);
            finalEnd = presentWorkdays.Max(x => x.End);
            _context.WorkDays.RemoveRange(presentWorkdays);
        }
        await _context.WorkDays.AddAsync(new WorkDay(targetedUser.Id, finalStart, finalEnd));
    }
    public async Task TransferUserPreference(DateTime start, DateTime end, Domain.User.User targetedUser)
    {
        Preference? foundPreference = await _context.Preferences
            .FirstOrDefaultAsync(x => x.UserId.Equals(targetedUser.Id) && x.Start.Equals(start) && x.End.Equals(end));
        if(foundPreference == null)
        {
            throw new Exception(string.Concat("Preference with the given dates not found! (start=", start.ToString(), "|end=", end.ToString(), ")"));
        }

        _context.Preferences.Remove(foundPreference);

        List<WorkDay> presentWorkdays = await _context.WorkDays
            .Where(y => y.UserId.Equals(targetedUser.Id) && start.CompareTo(y.End) <= 0 && end.CompareTo(y.Start) >= 0)
            .ToListAsync();

        DateTime finalStart = start;
        DateTime finalEnd = end;
        if (presentWorkdays.Count > 0)
        {
            finalStart = presentWorkdays.Min(x => x.Start);
            finalEnd = presentWorkdays.Max(x => x.End);
            _context.WorkDays.RemoveRange(presentWorkdays);
        }
        await _context.WorkDays.AddAsync(new WorkDay(targetedUser.Id, finalStart, finalEnd));
    }
    public async Task ClearUserWorkday(DateOnly day, Domain.User.User targetedUser)
    {
        DateTime localStart = day.ToDateTime(TimeOnly.MinValue);
        DateTime localEnd = day.ToDateTime(TimeOnly.MaxValue);

        List<WorkDay> foundWorkdays = await _context.WorkDays
            .Where(x => x.UserId.Equals(targetedUser.Id) && localStart.CompareTo(x.Start) <= 0 && localEnd.CompareTo(x.End) >= 0)
            .ToListAsync();

        _context.WorkDays.RemoveRange(foundWorkdays);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
