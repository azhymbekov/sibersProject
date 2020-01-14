using System;
using System.Collections.Generic;
using System.Text;

namespace Sibers.Data.Models.Entities
{
    public class Employee
    {       
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Email { get; set; }

        public ICollection<Project> SupervisorProjects { get; set; }

        public ICollection<ProjectEmployee> Projects { get; set; }        
    }
}
