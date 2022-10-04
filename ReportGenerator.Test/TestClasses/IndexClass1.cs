using System.ComponentModel.DataAnnotations;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    [MetadataType(typeof (IndexClass1Metadata))]
    public class IndexClass1 : IndexEntity, IAggregateRoot
    {
    }
}