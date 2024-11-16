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

    public async Task<User?> GetUserByEmail(string email)
        => await _context.Users.Include(u => u.Account).FirstOrDefaultAsync(u => u.Account.Email.Equals(email));
    public async Task<User?> GetUserById(Guid id)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
    public async Task<IList<User>> GetAllUsers() 
        => await _context.Users.ToListAsync();

    public async Task AddUser(User user)
        => await _context.Users.AddAsync(user);
    public async Task DeleteUser(User user) 
        => await Task.FromResult(_context.Users.Remove(user));
    public async Task ModifyUser(User user)
        => await Task.FromResult(_context.Users.Update(user));

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
