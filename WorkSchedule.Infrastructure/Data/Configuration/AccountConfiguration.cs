using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Infrastructure.Data.Configuration;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(a => a.Email)
            .IsRequired();

        builder.Property(a => a.Password)
            .IsRequired();
        
    }
}
