using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class MonthMap : ClassMap<Month>
    {
        public MonthMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.MonthNumber);
            Map(x => x.MonthStatus);
            Map(x => x.Date);
            Map(x => x.Name);
            Map(x => x.MonthType);
            Map(x => x.ImportPrimaryBenefits);
            Map(x => x.ImportBenefitDistribution);
            Map(x => x.ImportFromEmployeeRelation);
            Map(x => x.ImportFromAttendance);

            HasMany(x => x.MonthlyCards).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}