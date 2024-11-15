using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Infrastructure.Data.Repository;
using WorkSchedule.Infrastructure.Token;

namespace WorkSchedule.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IRoles, RolesRepository>();
        services.AddTransient<LoginUserService>();
        return services;
    }
}
