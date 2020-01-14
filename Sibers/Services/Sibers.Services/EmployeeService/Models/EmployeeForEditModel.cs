using System;
using System.Collections.Generic;
using System.Text;

namespace Sibers.Services.EmployeeService.Models
{
    public class EmployeeForEditModel
    {
        public EmployeeEditModel EmployeeEditModel { get; set; }

        public Guid[] CurrentProjects { get; set; }

        public Dictionary<Guid, string> Projects { get; set; }
    }
}
