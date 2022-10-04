using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class EmployeeCustodieMap : ClassMap<EmployeeCustodie>
    {
        public EmployeeCustodieMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Quantity);
            Map(x => x.CustodyStartDate);
            Map(x => x.CustodyEndDate);
            References(x => x.CustodyName);
            References(x => x.EmployeeCard);

        }
    }
}
