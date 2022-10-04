using System.Collections.Generic;
using Infrastructure.Localization;
using ReportGenerator.Test.TestClasses;
using Resources.Areas.Personnel.Entities.Employee;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class EntityClassLevel1
    {
        public string Name { get; set; }

        [LocalizationDisplayName("Children", typeof (EmployeeModel))]
        public IList<ClassWithIndexes> ClassWithIndexeses { get; set; }

        public IList<EntityClassLevel2> EntityClassLevel2S { get; set; }
    }
}