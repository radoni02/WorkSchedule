using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Application.Interfaces;

public interface IUserRepository
{
    Task<WorkSchedule.Domain.User.User?> GetUserByEmail(string email);
    Task<WorkSchedule.Domain.User.User?> GetUserById(Guid id);
    Task<IList<Domain.User.User>> GetAllUsers();
    Task SaveChangesAsync();
    Task AddUser(WorkSchedule.Domain.User.User user);
    Task DeleteUser(WorkSchedule.Domain.User.User user);
    Task ModifyUser(WorkSchedule.Domain.User.User user);
}
