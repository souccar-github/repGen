using System.Collections.Generic;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class EntityClassChild : IAggregateRoot
    {
        public EntityClassParent Parent { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public List<EntityClass1> Entites { get; set; }
    }
}