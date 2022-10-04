using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class EntityClass2 : IAggregateRoot
    {
        public int Property1 { get; set; }
        public string Property2 { get; set; }
        public DateTime Property3 { get; set; }
        public IndexClass1 Property4 { get; set; }
    }
}