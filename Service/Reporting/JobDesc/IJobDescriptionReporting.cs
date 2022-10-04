using HRIS.Domain.JobDesc.Entities;
using Service.DTO.JobDesc;

namespace Service.Reporting.JobDesc
{
    public interface IJobDescriptionReporting
    {
        JobDescription GetJobDescriptionTemplate(int jobTitleID);
        JobDescriptionTemplate GetJobDescriptionTemplateByPositionID(int positionID);
    }
}