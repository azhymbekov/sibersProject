using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.Data.Models.Entities;

namespace Sibers.Data.Configurations
{
    public class ProjectEmployeeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
    {
        public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
        {
            builder.HasKey(x => new { x.EmployeeId, x.ProjectId });
            builder.HasOne(x => x.Employee).WithMany(x => x.Projects).HasForeignKey(x => x.EmployeeId).IsRequired();
            builder.HasOne(x => x.Project).WithMany(x => x.Employees).HasForeignKey(x => x.ProjectId).IsRequired();
        }
    }
}
