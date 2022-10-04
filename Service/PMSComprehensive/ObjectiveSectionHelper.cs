using System.Collections.Generic;
using Model.PMS.ValueObjects.Implementation.Objective;

namespace Service.PMSComprehensive
{
    public class ObjectiveSectionHelper
    {
        //todo remove all service witch not implement IAggregateRoot interface
        //private  static EntityService<ObjectiveSectionItem> _service = new EntityService<ObjectiveSectionItem>();


        public static IList<ObjectiveSectionItemKpi> GetKPI(int sectionID)
        {
            //var objectiveSection = _service.LoadById(sectionID);
            //return objectiveSection.ObjectiveSectionItemKpis;
            return null;

        }

        public static void CalulateObjectiveSectionRate (ObjectiveSection objectiveSection)
        {
            objectiveSection.TotalRate = 0;
            foreach (var item in objectiveSection.ObjectiveSectionItems)
            {
                objectiveSection.TotalRate += item.Weight*(item.Rate/100);
            }

        }
    }
}