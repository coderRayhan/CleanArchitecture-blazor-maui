using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions;
public interface IApplicationDbContext
{
	#region Common Setup
	DbSet<Lookup> Lookups { get; }
	DbSet<LookupDetails> LookupDetails { get; }
	#endregion

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
