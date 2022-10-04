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

    public class Certification : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual CertificationType Type { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual Country PlaceOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual DateTime? DateOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime? ExpirationDate { get; set; }

        [UserInterfaceParameter(Order = 50)]
        //public virtual Status Status { get; set; }
        public virtual bool Status { get { return ExpirationDate >= DateTime.Today; } }

        [UserInterfaceParameter(Order = 60)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
