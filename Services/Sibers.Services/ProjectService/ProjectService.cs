using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sibers.Data;
using Sibers.Data.Models.Entities;
using Sibers.Services.EmployeeService.Interfaces;
using Sibers.Services.ProjectService.Interfaces;
using Sibers.Services.ProjectService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext context;

        private readonly IEmployeeService employeeService;

        private readonly IMapper mapper;

        public ProjectService(ApplicationContext context, IEmployeeService employeeService, IMapper mapper)
        {
            this.context = context;
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        public async Task<OperationResult> AppointEmployeesToProject(ProjectForDisplay model)
        {
            var result = new OperationResult();
            try
            {
                var projEmployees = context.ProjectEmployees.Where(x => x.ProjectId == model.Id);
                context.ProjectEmployees.RemoveRange(projEmployees);
                if (model.Employees != null)
                {
                    foreach (var empId in model.Employees)
                    {
                        context.ProjectEmployees.Add(new ProjectEmployee
                        {
                            ProjectId = model.Id,
                            EmployeeId = empId
                        });
                    }
                }

                await context.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<OperationResult> Create(ProjectDto model)
        {
            var project = await context.Projects.FirstOrDefaultAsync(x => x.Name == model.Name);
            var result = new OperationResult();
            if (project == null)
            {
                project = mapper.Map<Project>(model);
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                result.Succeeded = true;                
            }
            else
            {
                result.Message = "Такой проект уже существует";
            }
            return result;
        }

        public ProjectDto Details(Guid id)
        {
            var project = (from proj in context.Projects
                           join emp in context.Employees on proj.Supervisor.Id equals emp.Id
                           where proj.Id == id
                           select  new ProjectDto
                           {
                               Id = proj.Id,
                               Name = proj.Name,
                               CustomerCompany = proj.CustomerCompany,
                               ExecutorCompany = proj.ExecutorCompany,
                               BeginDate = proj.BeginDate,
                               EndDate = proj.EndDate,
                               Priority = proj.Priority,
                               Supervisor = $"{emp.LastName} {emp.FirstName} {emp.Patronymic}"
                           }).FirstOrDefault();
            return project;
        }

        public async Task<OperationResult> EditAsync(ProjectDto model)
        {
            var result = new OperationResult();
            var project = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (project != null)
            {
                project.Name = model.Name;
                project.CustomerCompany = model.CustomerCompany;
                project.ExecutorCompany = model.ExecutorCompany;
                project.BeginDate = model.BeginDate;
                project.EndDate = model.EndDate;
                project.SupervisorId = model.SupervisorId;
                project.Priority = model.Priority;

                await context.SaveChangesAsync();
                result.Succeeded = true;
            }
            else
            {
                result.Message = "Данного проекта не существует";
            }
            return result;
        }

        public ProjectDto Get(Guid id)
        {
            var project = (from proj in context.Projects
                           join emp in context.Employees on proj.Supervisor.Id equals emp.Id
                           where proj.Id == id
                           select new ProjectDto
                           {
                               Id = proj.Id,
                               Name = proj.Name,
                               CustomerCompany = proj.CustomerCompany,
                               ExecutorCompany = proj.ExecutorCompany,
                               BeginDate = proj.BeginDate,
                               EndDate = proj.EndDate,
                               Priority = proj.Priority,
                               Supervisor = $"{emp.LastName} {emp.FirstName} {emp.Patronymic}"
                           }).FirstOrDefault();
            return project;
        }

        public async Task<ProjectEmployees> GetEmployeesToProjectAsync(Guid projectId)
        {
            var project = Get(projectId);
            var model = new ProjectEmployees
            {
                Project = new ProjectForDisplay { Id = project.Id, Name = project.Name },
                CurrentEmployeesIds = await context.ProjectEmployees.Where(x => x.ProjectId == projectId).Select(x => x.EmployeeId).ToArrayAsync(),
                Employees = employeeService.GetEmployees().ToDictionary(x => x.Id, x => $"{x.LastName} {x.FirstName} {x.Patronymic}")
            };

            return model;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjects(ProjectFilter filter)
        {
            filter = filter ?? new ProjectFilter();
            var projects = await(from proj in context.Projects
                                 join pe in context.ProjectEmployees on proj.Id equals pe.ProjectId
                                 into temp from pe in temp.DefaultIfEmpty()
                                 where (filter.Name == null || proj.Name == filter.Name) &&
                                       (filter.BeginDate == null || proj.BeginDate >= filter.BeginDate) &&
                                       (filter.EndDate == null || proj.EndDate <= filter.EndDate) &&
                                       (filter.Supervisor == null || proj.Supervisor.Id == filter.Supervisor) &&
                                       (filter.Executor == null || pe.EmployeeId == filter.Executor)
                                 select new ProjectDto
                                 {
                                     Id = proj.Id,
                                     Name = proj.Name,
                                     CustomerCompany = proj.CustomerCompany,
                                     ExecutorCompany = proj.ExecutorCompany,
                                     BeginDate = proj.BeginDate,
                                     EndDate = proj.EndDate,
                                     Supervisor = $"{proj.Supervisor.LastName} {proj.Supervisor.FirstName} {proj.Supervisor.Patronymic}",
                                     Priority = proj.Priority,
                                     Executors = string.Join(", ", (from emp in context.Employees
                                                                    join projEmp in context.ProjectEmployees on emp.Id equals projEmp.EmployeeId
                                                                    where projEmp.ProjectId == proj.Id
                                                                    select $"{emp.LastName} {emp.FirstName} {emp.Patronymic}").ToArray())
                                 }).Distinct().ToListAsync();
            return projects;
        }

        public async Task Remove(Guid id)
        {
            var project = await context.Projects.FindAsync(id);
            if (project != null)
            {
                context.Remove(project);
                await context.SaveChangesAsync();
            }
        }
    }
}
