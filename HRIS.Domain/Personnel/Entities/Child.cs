#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Enums;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Child : ChildBase
    {
        [UserInterfaceParameter(Order = 25, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string FatherName
        {
            get
            {
                if (Employee == null)
                    return "";
                if (Employee.Gender == Enums.Gender.Male)
                    return Employee.FullName;
                return Spouse == null ? "" : string.Format("{0} {1}", Spouse.FirstName, Spouse.LastName);
            }
        }
        [UserInterfaceParameter(Order = 27, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string MatherName
        {
            get
            {
                if (Employee == null)
                    return "";
                if (Employee.Gender == Enums.Gender.Female)
                    return Employee.FullName;
                return Spouse == null ? "" : string.Format("{0} {1}", Spouse.FirstName, Spouse.LastName);
            }
        }
        public virtual Employee Employee { get; set; }  
    }
}