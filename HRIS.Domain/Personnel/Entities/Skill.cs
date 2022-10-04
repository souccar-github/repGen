#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Skill : Entity
    {
        [UserInterfaceParameter(Order = 130)]
        public virtual SkillType Name { get; set; }

        [UserInterfaceParameter(Order = 135)]
        public virtual Level Level { get; set; }
        [UserInterfaceParameter(Order = 140)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 160)]
        public virtual string Comments { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
