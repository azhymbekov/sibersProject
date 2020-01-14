using Sibers.Services.ProjectService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Services.ProjectService.Interfaces
{
    public interface IProjectService
    {
        ProjectDto Details(Guid id);

        Task<IEnumerable<ProjectDto>> GetProjects(ProjectFilter filter);

        Task<OperationResult> Create(ProjectDto model);

        Task<OperationResult> EditAsync(ProjectDto model);

        Task Remove(Guid id);

        Task<ProjectEmployees> GetEmployeesToProjectAsync(Guid projectId);

        Task<OperationResult> AppointEmployeesToProject(ProjectForDisplay model);

        ProjectDto Get(Guid id);
    }
}
