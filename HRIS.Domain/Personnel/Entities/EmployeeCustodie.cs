﻿#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 03/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Configurations;
#endregion
namespace HRIS.Domain.Personnel.Entities
{
    public class EmployeeCustodie : Entity
    {
        public EmployeeCustodie()
        {
            CustodyStartDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 1, IsReference=true)]
        public virtual CustodieDetails CustodyName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int Quantity { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime CustodyStartDate { get; set; }
        [UserInterfaceParameter(Order = 5,IsNonEditable = true)]
        public virtual DateTime? CustodyEndDate { get; set; }
    }
}
