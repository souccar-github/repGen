using HRIS.Domain.PMS.RootEntities;

namespace Service.PMSComprehensive
{
    public class TemplateTasksToAppraisal
    {
        private static Appraisal _appraisal;
        //private static AppraisalTemplate _appraisalTemplate;
        private static EntityService<Appraisal> _appraisalEntityService;


        //public static void Clone(int appraisalId)
        //{
        //    _appraisalEntityService = new EntityService<Appraisal>();
        //    _appraisal = _appraisalEntityService.LoadById(appraisalId);
        //    //_appraisalTemplate = new EntityService<AppraisalTemplate>().GetList().First();

        //    if (_appraisalTemplate != null && _appraisal != null)
        //    {
        //        var jobDescriptionSection = new JobDescriptionSection();

        //        foreach (TemplateCustomizedSection tempCustomSec in new EntityService<AppraisalTemplate>().GetAll().Where(
        //                x => x.Employee.Id == employeeId && x.Position.Id == position.Id))
        //        {
        //            jobDescriptionSection.Name = tempCustomSec.Name;
        //            jobDescriptionSection.Weight = tempCustomSec.Weight;
        //        }

        //    }
        //}
    }
}