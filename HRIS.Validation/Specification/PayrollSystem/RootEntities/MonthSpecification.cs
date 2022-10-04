using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class MonthSpecification : Validates<Month>
    {
        public MonthSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name, y => typeof(Month).GetProperty("Name").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MonthNumber, y => typeof(Month).GetProperty("MonthNumber").GetTitle()).Required();
            Check(x => x.Date, y => typeof(Month).GetProperty("Date").GetTitle()).Required();
            Check(x => x.MonthType, y => typeof(Month).GetProperty("MonthType").GetTitle()).Required();
        }
    }
}
