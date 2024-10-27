using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Data.Repository;

public class RolesRepository : IRoles
{
    private readonly WorkScheduleContext _context;

    public RolesRepository(WorkScheduleContext context)
    {
        _context = context;
    }
    public async Task<IList<string>> GetAllRoles() 
        => await Task.FromResult(Enum.GetValues(typeof(AppRole))
        .Cast<AppRole>()
        .Select(v => v.ToString())
        .ToList());
}
