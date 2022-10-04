using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.PayrollSystem.Indexes
{
    public class DonorLoanMap : ClassMap<DonorLoan>
    {
        public DonorLoanMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
