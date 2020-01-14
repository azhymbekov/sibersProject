using System;
using System.Collections.Generic;
using System.Text;

namespace Sibers.Services.ProjectService.Models
{
    public class ProjectForDisplay
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid[] Employees { get; set; }
    }
}
