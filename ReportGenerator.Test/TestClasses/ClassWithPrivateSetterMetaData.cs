using Infrastructure.Localization;
using Resources.Areas.Personnel.Entities.Employee;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class ClassWithPrivateSetterMetaData
    {
        [LocalizationDisplayName("FirstName", typeof (EmployeeModel))]
        public string FirstName { get; private set; }
    }
}