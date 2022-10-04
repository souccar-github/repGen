using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.OrgChart.Indexes;

namespace Service.JobDesc
{
    public class JobDescHelper
    {
        public static JobDescription GetJobDescription(int jobTitleId)
        {
            var jobDescriptionService = new EntityService<JobDescription>();
            var jobDescription =
                jobDescriptionService.GetAll().SingleOrDefault(jobDesc => jobDesc.JobTitle.Id == jobTitleId);
            return jobDescription;
        }

        public static JobDescription GetJobDescription(JobTitle jobTitle)
        {
            return GetJobDescription(jobTitle.Id);
        }
    }
}