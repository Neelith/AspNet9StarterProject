namespace YourProjectName.Application.Infrastructure.Persistance;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    //Add DbSets here
    //DbSet<Movement> Movements { get; }
}
