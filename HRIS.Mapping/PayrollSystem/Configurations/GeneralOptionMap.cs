using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.Configurations
{
    public class GeneralOptionMap : ClassMap<GeneralOption>
    {
        public GeneralOptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.TaxThreshold);
            Map(x => x.TotalMonthDays);
            Map(x => x.TotalDayHours);
            //Map(x => x.AllowAuditFeature);
            Map(x => x.StoppingTaxByReserveMilitaryService);
            //Map(x => x.AuditState);

            References(x => x.OvertimeBenefit);
            References(x => x.RecycledLeaveBenefit);
            References(x => x.TaxDeduction);


            References(x => x.LeaveDeduction);
            References(x => x.PenaltyDeduction);
            References(x => x.AbsenceDaysDeduction);
            References(x => x.NonAttendanceDeduction);
            References(x => x.LatenessDeduction);
            References(x => x.RewardBenefit);
        }
    }
}