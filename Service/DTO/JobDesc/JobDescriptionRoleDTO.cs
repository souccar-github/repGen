using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.JobDesc.ValueObjects;

namespace Service.DTO.JobDesc
{
    public class JobDescriptionRoleDTO
    {
        public JobDescriptionRoleDTO(Role role)
        {
            Name = role.Name;
            Priority = role.Priority;
            Weight = role.Weight;
            Summary = role.Summary;
            Responsibilities = (from responsibility in role.Responsibilities
                                select new ResponsibilityDTO(responsibility)).ToList();
        }

        public string Name { get; set; }
        public Priority Priority { get; set; }
        public float Weight { get; set; }
        public string Summary { get; set; }
        public List<ResponsibilityDTO> Responsibilities { get; set; }
    }
}