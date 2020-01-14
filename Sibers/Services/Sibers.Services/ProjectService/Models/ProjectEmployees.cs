using System;
using System.Collections.Generic;
using System.Text;

namespace Sibers.Services.ProjectService.Models
{
    public class ProjectEmployees
    {
        public ProjectForDisplay Project { get; set; }

        public Guid[] CurrentEmployeesIds { get; set; }

        public Dictionary<Guid, string> Employees { get; set; }
    }
}
