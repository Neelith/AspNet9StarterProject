using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourProjectName.Application.Infrastructure.Persistance;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    //Add DbSets here
    //DbSet<Movement> Movements { get; }
}
