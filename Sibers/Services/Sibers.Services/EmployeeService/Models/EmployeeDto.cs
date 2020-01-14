using System;
using System.ComponentModel.DataAnnotations;

namespace Sibers.Services.EmployeeService.Models
{
    public class EmployeeDto
    {        
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Введите почту")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }               

        [Display(Name = "Проекты")]
        public Guid[] Projects { get; set; }        
    
    }
}
