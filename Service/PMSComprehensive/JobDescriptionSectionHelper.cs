using System.Collections.Generic;
using System.Linq;
using Model.PMS.ValueObjects.Implementation.JobDescription;

namespace Service.PMSComprehensive
{
    public static class JobDescriptionSectionHelper
    {
        //todo remove all service witch not implement IAggregateRoot interface
        //private static readonly EntityService<JobDescriptionSectionItem> _service = new EntityService<JobDescriptionSectionItem>();

        public static IList<JobDescriptionSectionItemKpi> GetKPI(int sectionItemId, int taskId)
        {
            //var descriptionSectionItem = _service.LoadById(sectionItemId);
            //return descriptionSectionItem.JobDescriptionSectionTasks.SingleOrDefault(x => x.Id == taskId).JobDescriptionSectionItemKpis;
            return null;

        }

        public static void CalulateJobDescriptionRate(JobDescriptionSection jobDescriptionSection)
        {
            jobDescriptionSection.TotalRate = 0;
            foreach (var section in jobDescriptionSection.JobDescriptionSectionItems)
            {
                foreach (var task in section.JobDescriptionSectionTasks)
                {
                    jobDescriptionSection.TotalRate += task.Weight * (task.Rate / 100);
                }
                
            }
        }
    }
}