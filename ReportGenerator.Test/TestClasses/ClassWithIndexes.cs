using System.ComponentModel.DataAnnotations;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    [MetadataType(typeof (ClassWithIndexesMetadata))]
    public class ClassWithIndexes
    {
        public IndexClass1 Index1 { get; set; }
        public IndexClass2 Index2 { get; set; }
    }
}