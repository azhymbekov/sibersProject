using Sibers.Services.EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sibers.Services.EmployeeService.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployees();

        EmployeeDto Get(Guid id);

        IEnumerable<EmployeeDto> GetSupervisors();

        Task<OperationResult> Remove(Guid id);

        Task<OperationResult> Create(EmployeeDto model);

        Task<OperationResult> Edit(EmployeeEditModel model);

        EmployeeForView EmployeeInfo(Guid id);

        Task<EmployeeProjectsView> PrepeareEmployeeProjectView();

        Task<EmployeeForEditModel> PrepeareEmployeeForEditAsync(Guid employeeId);        
    }
}
