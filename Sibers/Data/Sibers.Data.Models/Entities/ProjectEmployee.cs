using System;

namespace Sibers.Data.Models.Entities
{
    public class ProjectEmployee
    {
        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
