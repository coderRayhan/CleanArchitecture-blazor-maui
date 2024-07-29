using Application.Common.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Lookups> Lookups => Set<Lookups>();

    public DbSet<LookupDetails> LookupDetails => Set<LookupDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
