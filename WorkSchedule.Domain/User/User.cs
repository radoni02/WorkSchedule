using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.User;

public class User
{
    public User(Guid id, string name, string lastname, AppRole role)
    {
        Id = id;
        Name = name;
        Lastname = lastname;
        Role = role;
    }

    public Guid Id { get;private set; }
    public Guid AccountId { get; set; }
    public Account Account { get; private set; }
    public string Name { get; private set; }
    public string Lastname { get; private set; }
    public AppRole Role { get; private set; }
}
