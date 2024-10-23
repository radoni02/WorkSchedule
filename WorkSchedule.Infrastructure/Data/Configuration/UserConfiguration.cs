using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Data.Configuration
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);


            builder.Property(u => u.Name)
                .IsRequired();

            builder.Property(u => u.Lastname)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion(
                r => r.ToString(),
                r => (AppRole)Enum.Parse(typeof(AppRole),r));

            builder.HasOne(u => u.Account)
                .WithOne()
                .HasForeignKey<User>(u => u.AccountId);
        }
    }
}
