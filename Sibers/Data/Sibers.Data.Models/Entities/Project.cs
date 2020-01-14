using System;
using System.Collections.Generic;
using System.Text;

namespace Sibers.Data.Models.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CustomerCompany { get; set; }

        public string ExecutorCompany { get; set; }

        public Guid SupervisorId { get; set; }

        public Employee Supervisor { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public ICollection<ProjectEmployee> Employees { get; set; }
    }
}
