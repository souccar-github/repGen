﻿using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.BaseClasses
{
    public class EmployeeCardBase : Entity, IAggregateRoot
    {

        [UserInterfaceParameter(Order = 10)]
        public virtual float Salary { get; set; } // راتب الموظف المقطوع

        [UserInterfaceParameter(Order = 15)]
        public virtual float BenefitSalary { get; set; } // 1راتب الموظف الاحتياطي

        [UserInterfaceParameter(Order = 20)]
        public virtual float InsuranceSalary { get; set; } // راتب الموظف التأميني

        [UserInterfaceParameter(Order = 25)]
        public virtual float TempSalary1 { get; set; } // راتب الموظف الاحتياطي2

        [UserInterfaceParameter(Order = 30)]
        public virtual float TempSalary2 { get; set; } // راتب الموظف الاحتياطي3

        [UserInterfaceParameter(Order = 35)]
        public virtual float Threshold { get; set; } // عتبة الراتب


    }
}
