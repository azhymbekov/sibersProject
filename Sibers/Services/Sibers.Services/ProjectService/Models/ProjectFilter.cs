using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sibers.Services.ProjectService.Models
{
    public class ProjectFilter
    {
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Display(Name = "Руководитель")]
        public Guid? Supervisor { get; set; }

        [Display(Name = "Исполнитель")]
        public Guid? Executor { get; set; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime? BeginDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}
