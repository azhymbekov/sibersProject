using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sibers.Services.ProjectService.Models
{
    public class ProjectDto : IEquatable<ProjectDto>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Введите название проекта")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите компанию заказчика")]
        [Display(Name = "Компания заказчика")]
        public string CustomerCompany { get; set; }

        [Required(ErrorMessage = "Введите исполняющую компанию")]
        [Display(Name = "Компания исполнителя")]
        public string ExecutorCompany { get; set; }

        [Required(ErrorMessage = "Выберите руководителя проекта")]
        [Display(Name = "Руководитель")]
        public Guid SupervisorId { get; set; }

        [Display(Name = "Руководитель")]
        public string Supervisor { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage ="Укажите приоритет задачи")]
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }

        [Display(Name = "Исполнители")]
        public string Executors { get; set; }

        public bool Equals(ProjectDto other)
        {
            //Check whether the compared object is null. 
            if (object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (object.ReferenceEquals(this, other)) return true;

            //Check whether properties are equal. 
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Id field if it is not null. 
            return Id == null ? 0 : Id.GetHashCode();
        }
    }
}
