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
    public class Passport : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual string Number { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string FirstName { get; set; }

        //[UserInterfaceParameter(Order = 25)]
        //public virtual string SecondName { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual string LastName { get; set; }

        [UserInterfaceParameter(Order = 24)]
        public virtual string FirstNameL2 { get; set; }

        [UserInterfaceParameter(Order = 37)]
        public virtual string LastNameL2 { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string FatherName { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual string MotherName { get; set; }


        [UserInterfaceParameter(Order = 60)]
        public virtual Country PlaceOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual DateTime IssuanceDate { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual DateTime ExpiryDate { get; set; }


        public virtual Employee Employee { get; set; }
    }
}
