using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reporting.RDL.Query
{
    public class RdlQueryTable
    {
        public string Name { get; set; }
        public RdlTableType Type { get; set; }
        public string Parent { get; set; }
    }
}
