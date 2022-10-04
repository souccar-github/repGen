using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Indexes;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class EmployeeLoan : Entity, IAggregateRoot
    {
        public EmployeeLoan()
        {
            LoanPayments = new List<LoanPayment>();
        }
        //[UserInterfaceParameter(Order = 5)]
        //public virtual PrimaryCard PrimaryCard { get; set; } // بطاقة الموظف الاساسية التي يتبع لها هذا القرض

        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual string LoanNumber { get; set; } // رقم القرض

        [UserInterfaceParameter(Order = 15)]
        public virtual DonorLoan DonorLoan { get; set; } // من اندكس اسماء القروض 

        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime Date { get; set; } // تاريخ اضافة القرض

        [UserInterfaceParameter(Order = 25)]
        public virtual double TotalAmountOfLoan { get; set; } // مبلغ القرض الكلي

        [UserInterfaceParameter(Order = 30)]
        public virtual double MonthlyInstalmentValue { get; set; } // قيمة القسط الشهري للقرض

        [UserInterfaceParameter(Order = 35)]
        public virtual double PrePayed { get; set; } // الدفعة الاولى

        private double remainingAmountOfLoan;

        [UserInterfaceParameter(Order = 40, IsNonEditable = true)]
        public virtual double RemainingAmountOfLoan
        {
            get
            {
                // الرصيد المتبقي
                return remainingAmountOfLoan;
            }
            set
            {
                var lastOrDefault = LoanPayments.LastOrDefault();
                remainingAmountOfLoan = !LoanPayments.Any()
                    ? TotalAmountOfLoan - PrePayed
                    : lastOrDefault.RemainingValueAfterPaymentValue;
            }
        }

        [UserInterfaceParameter(Order = 45)]
        public virtual string FirstRepresentative { get; set; } // الكفيل الاول

        [UserInterfaceParameter(Order = 50)]
        public virtual string SecondRepresentative { get; set; } // الكفيل الثاني

        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                var _donorName = DonorLoan != null ? DonorLoan.Name + ", " : "";
                return _donorName + LoanNumber;
            }
        }


        public virtual IList<LoanPayment> LoanPayments { get; set; } // الدفعات الشهرية التي تمت على القرض
        public virtual void AddLoanPayment(LoanPayment loanPayment)
        {
            LoanPayments.Add(loanPayment);
            loanPayment.EmployeeLoan = this;
        }
    }
}
