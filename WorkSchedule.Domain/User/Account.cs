using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.User;

public class Account
{
    public Account(Guid id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }

    public Guid Id { get; set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpTime { get; private set; }

    public void SetRefreshToken(string token)
    {
        RefreshToken = token;
    }
    public void SetRefreshTokenExpTime(DateTime dateTime)
    {
        RefreshTokenExpTime = dateTime;
    }
}
