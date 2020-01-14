using System;
using System.Collections.Generic;

namespace Sibers.Services.EmployeeService.Models
{
    public class EmployeeProjectsView
    {
        public Dictionary<Guid, string> Projects { get; set; }
    }
}
