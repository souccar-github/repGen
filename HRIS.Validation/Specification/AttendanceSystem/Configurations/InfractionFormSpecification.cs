using HRIS.Domain.AttendanceSystem.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class InfractionFormSpecification : Validates<InfractionForm>
    {
        public InfractionFormSpecification()
        {
            IsDefaultForType();
            Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
