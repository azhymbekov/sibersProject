using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sibers.Services.EmployeeService.Interfaces;
using Sibers.Services.EmployeeService.Models;

namespace Sibers.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult Index()
        {
            var user = employeeService.GetEmployees() ?? new List<EmployeeDto>();
            return View(user);
        }

        public IActionResult Details(Guid id)
        {
            var employee = employeeService.EmployeeInfo(id);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var employeeEditModel = await employeeService.PrepeareEmployeeForEditAsync(id);
            ViewBag.Projects = employeeEditModel.Projects.Select(x => new SelectListItem(x.Value, x.Key.ToString(), employeeEditModel.CurrentProjects.Contains(x.Key)));

            return View(employeeEditModel.EmployeeEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditModel model)
        {
            EmployeeForEditModel employeeEditModel;
            if (!ModelState.IsValid)
            {
                employeeEditModel = await employeeService.PrepeareEmployeeForEditAsync(model.Id);
                ViewBag.Projects = employeeEditModel.Projects.Select(x => new SelectListItem(x.Value, x.Key.ToString(), employeeEditModel.CurrentProjects.Contains(x.Key)));
                return View(model);
            }

            var result = await employeeService.Edit(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            employeeEditModel = await employeeService.PrepeareEmployeeForEditAsync(model.Id);
            ViewBag.Projects = employeeEditModel.Projects.Select(x => new SelectListItem(x.Value, x.Key.ToString(), employeeEditModel.CurrentProjects.Contains(x.Key)));
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employeeCreationModel = await employeeService.PrepeareEmployeeProjectView();
            ViewBag.Projects = employeeCreationModel.Projects.Select(x => new SelectListItem(x.Value, x.Key.ToString()));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto model)
        {
            EmployeeProjectsView employeeProjectsView;            

            var result = await employeeService.Create(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            employeeProjectsView = await employeeService.PrepeareEmployeeProjectView();
            ViewBag.Projects = employeeProjectsView.Projects.Select(x => new SelectListItem(x.Value, x.Key.ToString()));
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await employeeService.Remove(id);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index), new { Error = result.Message });
            }
        }
       

    }
}