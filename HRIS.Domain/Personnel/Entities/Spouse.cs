#region


using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Helpers;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Spouse:SpouseBase
    {
        public virtual Employee Employee { get; set; }

    }
}