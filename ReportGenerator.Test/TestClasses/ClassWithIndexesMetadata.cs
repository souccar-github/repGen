using Infrastructure.Localization;
using Resources.Areas.Personnel.Entities.Employee;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    internal class ClassWithIndexesMetadata
    {
        [LocalizationDisplayName("FirstName", typeof (EmployeeModel))]
        public IndexClass1 Index1 { get; set; }

        [LocalizationDisplayName("LastName", typeof (EmployeeModel))]
        public IndexClass1 Index2 { get; set; }
    }
}