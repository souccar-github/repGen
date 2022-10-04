#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Language : Entity
    {
        [UserInterfaceParameter(Order = 130)]
        public virtual LanguageName LanguageName { get; set; }

        [UserInterfaceParameter(Order = 140)]
        public virtual Level Writing { get; set; }

        [UserInterfaceParameter(Order = 160)]
        public virtual Level Reading { get; set; }

        [UserInterfaceParameter(Order = 176)]
        public virtual Level Speaking { get; set; }

        [UserInterfaceParameter(Order = 180)]
        public virtual Level Listening { get; set; }

        public virtual Employee Employee { get; set; }
    }
}