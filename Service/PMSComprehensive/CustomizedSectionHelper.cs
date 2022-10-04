using System.Collections.Generic;
using Model.PMS.ValueObjects.Implementation.Customized;
using Model.PMS.ValueObjects.Implementation.Objective;
using System.Linq.Expressions;
using System.Linq;
namespace Service.PMSComprehensive
{
    public class CustomizedSectionHelper
    {
        //todo remove all service witch not implement IAggregateRoot interface
        //private static EntityService<CustomizedSection> _service = new EntityService<CustomizedSection>();


        public static IList<ItemKpi> GetKPI(int sectionID, int sectionItemId)
        {
            //var customizedSection = _service.LoadById(sectionID);
            //return customizedSection.SectionItems.SingleOrDefault(x => x.Id == sectionItemId).Kpis;
            return null;

        }
        public  static  void CalulateCustomizedSectionRate(CustomizedSection customizedSection)
        {
            customizedSection.TotalRate = 0;
            foreach (var item in customizedSection.SectionItems)
            {
                customizedSection.TotalRate += item.Weight*(item.Rate/100);
            }
        }
    }
}