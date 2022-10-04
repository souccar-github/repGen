#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Conviction : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string Number { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual ConvictionType Type { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime ReleaseDate { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual DateTime ExpirationDate { get; set; }

        //[UserInterfaceParameter(Order = 55,IsHidden=true)]
        //public virtual DateTime ConvictionDate { get; set; }

        [UserInterfaceParameter(Order = 56)]
        public virtual bool IsConvicted { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual string Reason { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
