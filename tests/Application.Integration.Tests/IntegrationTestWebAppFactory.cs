﻿using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Application.Integration.Tests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        //private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        //.WithImage("mcr.microsoft.com/mssql/server")
        //.WithPassword("Rakib1523@@")
        //.Build();

        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:alpine3.20")
            .WithDatabase("IventoryDB")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if(descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
                    .UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}