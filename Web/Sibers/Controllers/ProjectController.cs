using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sibers.Services.EmployeeService.Interfaces;
using Sibers.Services.EmployeeService.Models;
using Sibers.Services.ProjectService.Interfaces;
using Sibers.Services.ProjectService.Models;
using Sibers.ViewModels;

namespace Sibers.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly IEmployeeService employeeService;

        public ProjectController(
            IProjectService projectService,
            IEmployeeService employeeService)
        {
            this.projectService = projectService;
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await projectService.GetProjects(new ProjectFilter());
            if (projects == null)
            {
                projects = new List<ProjectDto>();
            }

            ViewBag.Employees = employeeService.GetEmployees().Select(x => new SelectListItem($"{x.LastName} {x.FirstName} {x.Patronymic}", x.Id.ToString()));

            ViewBag.FilterModel = new ProjectFilter();

            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<ItemViewModel> employees = new List<ItemViewModel>();
            employees = employeeService.GetSupervisors().Select(x =>
                new ItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.LastName} {x.FirstName} {x.Patronymic}"
                }).ToList();
            ViewBag.EmployeeList = employees;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto model)
        {
            var result = await projectService.Create(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Project");
            }
            else
            {
                ViewBag.Result = result.Message;
                return this.View();
            }
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var project = projectService.Details(id);
            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            await projectService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            List<ItemViewModel> employees = new List<ItemViewModel>();
            var project = projectService.Get(id);
            employees = employeeService.GetSupervisors().Select(x =>
                new ItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.LastName} {x.FirstName} {x.Patronymic}"
                }).ToList();
            ViewBag.EmployeeList = employees;
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectDto model)
        {
            List<ItemViewModel> employees = new List<ItemViewModel>();
            if (!ModelState.IsValid)
            {
                employees = employeeService.GetSupervisors().Select(x =>
                new ItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.LastName} {x.FirstName} {x.Patronymic}"
                }).ToList();
                ViewBag.EmployeeList = employees;
                return View(model);
            }

            var result = await projectService.EditAsync(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            employees = employeeService.GetSupervisors().Select(x =>
                new ItemViewModel
                {
                    Id = x.Id,
                    Name = $"{x.LastName} {x.FirstName} {x.Patronymic}"
                }).ToList();
            ViewBag.EmployeeList = employees;
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProjects(ProjectFilter model)
        {
            var projects = await projectService.GetProjects(model);
            ViewBag.Employees = employeeService.GetEmployees().Select(x => new SelectListItem($"{x.LastName} {x.FirstName} {x.Patronymic}", x.Id.ToString()));
            ViewBag.FilterModel = model;
            return View(nameof(Index), projects);
        }

        [HttpGet]
        public async Task<IActionResult> AppointEmployees(Guid id)
        {
            var projectEmployees = await projectService.GetEmployeesToProjectAsync(id);
            ViewBag.Employees = projectEmployees.Employees.Select(x => new SelectListItem(x.Value, x.Key.ToString(), projectEmployees.CurrentEmployeesIds.Contains(x.Key)));
            return View(projectEmployees.Project);
        }

        [HttpPost]
        public async Task<IActionResult> AppointEmployees(ProjectForDisplay model)
        {
            ProjectEmployees projectEmployees;
            if (!ModelState.IsValid)
            {
                projectEmployees = await projectService.GetEmployeesToProjectAsync(model.Id);
                ViewBag.Employees = projectEmployees.Employees.Select(x => new SelectListItem(x.Value, x.Key.ToString(), projectEmployees.CurrentEmployeesIds.Contains(x.Key)));
                return View(model);
            }

            var result = await projectService.AppointEmployeesToProject(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            projectEmployees = await projectService.GetEmployeesToProjectAsync(model.Id);
            ViewBag.Employees = projectEmployees.Employees.Select(x => new SelectListItem(x.Value, x.Key.ToString(), projectEmployees.CurrentEmployeesIds.Contains(x.Key)));
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
    }
}