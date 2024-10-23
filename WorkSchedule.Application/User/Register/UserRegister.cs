using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Application.User.Register;

public class UserRegister
{
    public string Name { get; private set; }
    public string Lastname { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string PasswordConfirmation { get; private set; }
    public AppRole Rola { get; private set; } 
}
