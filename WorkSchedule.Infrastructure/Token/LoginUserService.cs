using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Token;

public class LoginUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    public LoginUserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepo = userRepository;
        _passwordHasher = passwordHasher;
    }

    public record Request(string Email, string Password);

    public async Task<User> Handle(Request request)
    {
        var user = await _userRepo.GetUserByEmail(request.Email);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        bool verified = _passwordHasher.Verify(request.Password, user.Account.Password);
        if (!verified)
        {
            throw new Exception("Password is incorrect");
        }
        return user;
    }
}
