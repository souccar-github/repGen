using System.ComponentModel.DataAnnotations;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    [MetadataType(typeof (ClassWithPrivateSetterMetaData))]
    public class ClassWithPrivateSetter
    {
        public string FirstName { get; private set; }
    }
}