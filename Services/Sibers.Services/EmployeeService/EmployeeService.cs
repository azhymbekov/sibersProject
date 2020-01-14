using AutoMapper;
using Sibers.Data;
using System.Linq;
using Sibers.Services.EmployeeService.Models;
using Sibers.Services.EmployeeService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sibers.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sibers.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationContext context;

        private readonly IMapper mapper;

        public EmployeeService(ApplicationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OperationResult> Create(EmployeeDto model)
        {
            var result = new OperationResult();
            try
            {
                var employee = this.mapper.Map<Employee>(model);
                employee.Id = Guid.NewGuid();

                if (model.Projects != null)
                {
                    foreach (var projectIds in model.Projects)
                    {
                        context.ProjectEmployees.Add(new ProjectEmployee
                        {
                            EmployeeId = employee.Id,
                            ProjectId = projectIds
                        });
                    }
                }

                context.Employees.Add(employee);
                await context.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception)
            {
                result.Message = "Oшибка при добавлении сотрудника";
            }

            return result;
        }

        public async Task<OperationResult> Edit(EmployeeEditModel model)
        {
            var entity = await context.Employees.FindAsync(model.Id);
            var result = new OperationResult();
            if (entity != null)
            {
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Patronymic = model.Patronymic;
                entity.Email = model.Email;

                var employeeProjects = context.ProjectEmployees.Where(x => x.EmployeeId == model.Id);

                context.ProjectEmployees.RemoveRange(employeeProjects);

                if (model.Projects != null)
                {
                    foreach (var projectId in model.Projects)
                    {
                        context.ProjectEmployees.Add(new ProjectEmployee()
                        {
                            EmployeeId = model.Id,
                            ProjectId = projectId
                        });
                    }
                }

                await context.SaveChangesAsync();
                result.Succeeded = true;
            }
            else
            {
                result.Message = "Данного пользователя не существует";
            }
            return result;
        }

        public EmployeeForView EmployeeInfo(Guid id)
        {

            var employee = Get(id);
            var projects = from proj in context.Projects
                           join ord in context.ProjectEmployees on proj.Id equals ord.ProjectId
                           where ord.EmployeeId == id
                           select proj.Name;
            var employeeForView = this.mapper.Map<EmployeeForView>(employee);
            employeeForView.Projects = projects;
            return employeeForView;
        }

        public EmployeeDto Get(Guid id)
        {
            var entity = context.Employees.Find(id);
            return this.mapper.Map<EmployeeDto>(entity);
        }

        public IEnumerable<EmployeeDto> GetEmployees()
        {
            return from emp in context.Employees
                   select this.mapper.Map<EmployeeDto>(emp);
        }

        public IEnumerable<EmployeeDto> GetSupervisors()
        {
            return from emp in context.Employees
                   select this.mapper.Map<EmployeeDto>(emp);
        }

        public async Task<EmployeeForEditModel> PrepeareEmployeeForEditAsync(Guid employeeId)
        {
            var entity = context.Employees.Find(employeeId);
            var employee = this.mapper.Map<EmployeeEditModel>(entity);

            var result = new EmployeeForEditModel
            {
                EmployeeEditModel = employee,
                CurrentProjects = await context.ProjectEmployees.Where(x => x.EmployeeId == employee.Id).Select(x => x.ProjectId).ToArrayAsync(),
                Projects = await context.Projects.ToDictionaryAsync(x => x.Id, x => x.Name),
            };

            return result;
        }

        public async Task<EmployeeProjectsView> PrepeareEmployeeProjectView()
        {
            var result = new EmployeeProjectsView
            {
                Projects = await context.Projects.ToDictionaryAsync(x => x.Id, x => x.Name),
            };

            return result;
        }

      
        public async Task<OperationResult> Remove(Guid id)
        {
            var result = new OperationResult();
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                await context.Entry(employee).Collection(x => x.SupervisorProjects).LoadAsync();
                if (employee.SupervisorProjects.Any())
                {
                    result.Message = "Нельзя удалить сотрудника, так как он руководитель проектов";
                }
                else
                {
                    context.Employees.Remove(employee);
                    await context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
    
}
