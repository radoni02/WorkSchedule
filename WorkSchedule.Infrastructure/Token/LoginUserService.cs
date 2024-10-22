using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Token;

public class LoginUserService(IUserRepository userRepo, IPasswordHasher passwordHasher)
{
    public record Request(string Email, string Password);

    public async Task<User> Handle(Request request)
    {
        var user = userRepo.GetUserByEmail(request.Email);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        bool verified = passwordHasher.Verify(request.Password, user.Result.Account.Password);
        if (!verified)
        {
            throw new Exception("Password is incorrect");
        }
        return user.Result;
    }
}
