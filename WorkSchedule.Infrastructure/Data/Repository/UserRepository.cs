using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly WorkScheduleContext _context;

    public UserRepository(WorkScheduleContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Account.Email == email);

    public async Task AddUser(User user)
        => await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
