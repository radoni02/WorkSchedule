using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.Preference;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Data;

public class WorkScheduleContext : DbContext
{
    public WorkScheduleContext(DbContextOptions options) : base(options)
    {
        
    }
    DbSet<User> Users { get; set; }
    DbSet<Preference> Preferences { get; set; }
    DbSet<WorkDay> WorkDays { get; set; }
    DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
