#region Using Statements

using System;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using Infrastructure.Entities;
using Service.DTO.JobDesc;
using Service.JobDesc;
using Service.OrgChart;

#endregion

namespace Service.Reporting.JobDesc
{
    public class JobDescriptionReporting : IJobDescriptionReporting
    {
        private readonly IEntityServiceBase<JobDescription> _jobDescriptionService;
        private readonly IEntityServiceBase<Position> _positionService;

        private JobDescriptionReporting(IEntityServiceBase<JobDescription> entityServiceBase)
        {
            _jobDescriptionService = entityServiceBase;
        }

        public JobDescriptionReporting(IEntityServiceBase<JobDescription> jobDescriptionService,
                                       IEntityServiceBase<Position> positionService) : this(jobDescriptionService)
        {
            _positionService = positionService;
        }

        #region IJobDescriptionReporting Members

        public JobDescription GetJobDescriptionTemplate(int jobTitleID)
        {
            var jobDescription =
                _jobDescriptionService.GetAll().SingleOrDefault(jobDesc => jobDesc.JobTitle.Id == jobTitleID);
            return jobDescription;
        }

        public JobDescriptionTemplate GetJobDescriptionTemplateByPositionID(int positionID)
        {
            var position = _positionService.GetById(positionID);
            var jobDescription =
                JobDescHelper.GetJobDescription(position.JobTitle);
            Employee reportingToEmployee = PositionHelpers.GetReportingToEmployee(position);
            var template = new JobDescriptionTemplate
                               {
                                   Authorities = jobDescription.Authorities.ToList(),
                                   Comptencies =
                                       jobDescription.Specification[0].Competencies.ToList(),
                                   Educations = jobDescription.Specification[0].Educations.ToList(),
                                   Experiences = jobDescription.Specification[0].Experiences.ToList(),
                                   ExternalRelationship =
                                       GetExternalRelationship(jobDescription),
                                   InternalRelationship =
                                       GetInternalRelationship(jobDescription),
                                   JobSummary = jobDescription.Summary,
                                   JobTitle = jobDescription.JobTitle.Name,
                                   Languages = jobDescription.Specification[0].Languages.ToList(),
                                   NodeName = position.Node.Name,
                                   NodeType = position.Node.Type.Name,
                                   OperatingBudget = position.Budget,
                                   PCSkills = jobDescription.Specification[0].ComputerSkills.ToList(),
                                   OtherSkills = jobDescription.Specification[0].Skills.ToList(),
                                   ReportingTo =
                                       reportingToEmployee != null
                                           ? String.Format("{0} {1}", reportingToEmployee.FirstName,
                                                           reportingToEmployee.LastName)
                                           : ""
                               };
            template.AddRoles(jobDescription.Roles);
            return template;
        }

        #endregion

        private static string GetInternalRelationship(JobDescription jobDescription)
        {
            var result =
                jobDescription.Specification[0].WorkingConditions.Select(work => work.InternalRelationships).ToArray();
            return String.Join(", ", result);
        }

        private static string GetExternalRelationship(JobDescription jobDescription)
        {
            var result =
                jobDescription.Specification[0].WorkingConditions.Select(work => work.ExternalRelationships).ToArray();
            return String.Join(", ", result);
        }
    }
}