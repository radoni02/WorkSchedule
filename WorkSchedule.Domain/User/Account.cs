using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.User;

public class Account
{
    public Account(Guid id, string email, string password, string refreshToken, DateTime refreshTokenExpTime)
    {
        Id = id;
        Email = email;
        Password = password;
        RefreshToken = refreshToken;
        RefreshTokenExpTime = refreshTokenExpTime;
    }

    public Guid Id { get; set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime RefreshTokenExpTime { get; private set; }
}
