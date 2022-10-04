using System.Collections.Generic;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class EntityClassParent
    {
        public EntityClassParent()
        {
            Children = new List<EntityClassChild>();
        }

        public List<EntityClassChild> Children { get; set; }
    }
}