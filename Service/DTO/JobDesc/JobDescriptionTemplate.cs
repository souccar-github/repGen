using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.ValueObjects;

namespace Service.DTO.JobDesc
{
    public class JobDescriptionTemplate
    {
        public JobDescriptionTemplate()
        {
            Authorities = new List<Authority>();
            Comptencies = new List<Competency>();
            Educations = new List<JEducation>();
            Experiences = new List<JExperience>();
            Languages = new List<JLanguage>();
            OtherSkills = new List<JSkill>();
            PCSkills = new List<ComputerSkill>();
            Roles = new List<JobDescriptionRoleDTO>();
        }

        public List<Competency> Comptencies { get; set; }
        public List<ComputerSkill> PCSkills { get; set; }
        public List<JLanguage> Languages { get; set; }
        public List<JExperience> Experiences { get; set; }
        public List<JEducation> Educations { get; set; }
        public List<JobDescriptionRoleDTO> Roles { get; set; }
        public List<Authority> Authorities { get; set; }
        public List<JSkill> OtherSkills { get; set; }

        public double OperatingBudget { get; set; }
        public string ExternalRelationship { get; set; }
        public string InternalRelationship { get; set; }
        public string JobSummary { get; set; }
        public string ReportingTo { get; set; }
        public string NodeName { get; set; }
        public string NodeType { get; set; }
        public string JobTitle { get; set; }

        public void AddRoles(IList<Role> roles)
        {
            Roles.AddRange(from role in roles
                           select new JobDescriptionRoleDTO(role));
        }
    }
}