using System;
using System.Collections.Generic;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class EntityClassWithTwoLevels
    {
        public DateTime Property1 { get; set; }
        public IList<EntityClassLevel1> EntityClassLevel1s { get; set; }
    }
}