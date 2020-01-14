using Microsoft.EntityFrameworkCore;
using Sibers.Data.Configurations;
using Sibers.Data.Models.Entities;

namespace Sibers.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProjectEmployeeConfiguration());

            builder.Entity<Project>().HasOne(x => x.Supervisor).WithMany(x => x.SupervisorProjects).HasForeignKey(x => x.SupervisorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
