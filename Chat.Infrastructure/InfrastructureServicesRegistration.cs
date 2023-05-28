﻿using Chat.Application.Persistence.Contracts;
using Chat.Application.Services.Abstractions;
using Chat.Domain.DAOs;
using Chat.Infrastructure.DataContext;
using Chat.Infrastructure.Repositories;
using Chat.Infrastructure.Services.Implementions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("SqlServerConnection")));
            //Dependency
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWebRootPathProvider, WebRootPathProvider>();
        //Indentity configuration
        services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });
            var s = configuration[""];
            services.AddIdentity<UserApp, IdentityRole>()
                    .AddEntityFrameworkStores<ChatDbContext>()
                    .AddDefaultTokenProviders();
            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            return services;
        }
    }
}
