using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Domain.User;

public class User
{
    public User()
    {
        Account = new(Guid.NewGuid(), string.Empty, string.Empty);
        Name = string.Empty;
        Lastname = string.Empty;
    }
    public User(Guid id, string name, string lastname, AppRole role,Guid accountId ,Account account)
    {
        Id = id;
        Name = name;
        Lastname = lastname;
        Role = role;
        Account = account;
        AccountId = accountId;
    }

    public Guid Id { get;private set; }
    public Guid AccountId { get; set; }
    public Account Account { get; private set; }
    public string Name { get; private set; }
    public string Lastname { get; private set; }
    public AppRole Role { get; private set; }
}
