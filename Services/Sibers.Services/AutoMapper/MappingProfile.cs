using AutoMapper;
using Sibers.Data.Models.Entities;
using Sibers.Services.EmployeeService.Models;
using Sibers.Services.ProjectService.Models;

namespace Sibers.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Employee, EmployeeDto>();
            this.CreateMap<EmployeeDto, Employee>()
                .ForMember(x => x.Projects, desc => desc.Ignore());
            this.CreateMap<Employee, EmployeeForView>();

            this.CreateMap<Project, ProjectDto>();
            this.CreateMap<ProjectDto, Project>();
        }
    }
}
