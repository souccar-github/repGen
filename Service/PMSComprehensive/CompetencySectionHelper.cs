using System.Collections.Generic;
using System.Linq;
using Model.JobDesc.Indexes;
using Model.PMS.ValueObjects.Implementation.Competency;

namespace Service.PMSComprehensive
{
    public class CompetencySectionHelper
    {
        public static void CalulateCompetencySectionRate(CompetencySection competencySection)
        {
            competencySection.TotalRate = 0;
            foreach (var item in competencySection.CompetencySectionItems)
            {
                competencySection.TotalRate += item.Rate*(item.Weight/100);
            }

        }

        public static IList<string> GetDistinctCompetencyTypes(CompetencySection competencySection)
        {
            var distinctTypes =
                (from competencySectionItem in competencySection.CompetencySectionItems
                 select competencySectionItem.Type).Distinct();
            return distinctTypes.ToList();
        }

    }
}