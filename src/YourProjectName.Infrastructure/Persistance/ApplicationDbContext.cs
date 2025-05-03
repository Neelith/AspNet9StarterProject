using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Application.Infrastructure.Persistance;

namespace YourProjectName.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //Set your DB sets here, put them in the IApplicationDbContext interface first
        //public DbSet<Movement> Movements => Set<Movement>();
    }
}
