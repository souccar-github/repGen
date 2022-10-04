using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    [Order(70)]
    [Module(ModulesNames.PayrollSystem)]
    public class GeneralOption : Entity, IConfigurationRoot
    {

        [UserInterfaceParameter(Order = 5)]
        public virtual int TotalMonthDays { get; set; } // عدد ايام الدوام بالشهر

        [UserInterfaceParameter(Order = 10)]
        public virtual double TotalDayHours { get; set; } // عدد ساعات الدوام في اليوم

        [UserInterfaceParameter(Order = 15)]
        public virtual double TaxThreshold { get; set; } // الحد الادنى المعفى من الضريبة

        [UserInterfaceParameter(Order = 20)]
        public virtual bool StoppingTaxByReserveMilitaryService { get; set; } // حالة الخدمة العسكرية تؤثر بالضريبة بحيث يتم ايقاف الضريبة اذا كان الموظف مسحوب للاحتياط

        [UserInterfaceParameter(Order = 25, IsReference = true)]
        public virtual BenefitCard RewardBenefit { get; set; } // تعويض المكافأة 

        [UserInterfaceParameter(Order = 35, IsReference = true)]
        public virtual BenefitCard OvertimeBenefit { get; set; } // تعويض الدوام الإضافي

        [UserInterfaceParameter(Order = 40, IsReference = true)]
        public virtual BenefitCard RecycledLeaveBenefit { get; set; } // تعويض تدوير الإجازات المالي السنوي

        [UserInterfaceParameter(Order = 45, IsReference = true)]
        public virtual DeductionCard TaxDeduction { get; set; } // حسم الضريبة 

        [UserInterfaceParameter(Order = 50, IsReference = true)]
        public virtual DeductionCard LeaveDeduction { get; set; } // حسم الاجازة  

        //[UserInterfaceParameter(Order = 50, IsReference = true)]
        //public virtual DeductionCard HealthyLeaveDeduction { get; set; } // حسم حسم الاجازة المرضية 

        //[UserInterfaceParameter(Order = 55, IsReference = true)]
        //public virtual DeductionCard UnpaidMaternityLeaveDeduction { get; set; } // حسم إجازة الامومة الاضافية بلا أجر 

        //[UserInterfaceParameter(Order = 60, IsReference = true)]
        //public virtual DeductionCard MaternityLeaveDeduction { get; set; } // حسم إجازة الامومة 

        //[UserInterfaceParameter(Order = 65, IsReference = true)]
        //public virtual DeductionCard AdministrativeLeaveDeduction { get; set; } // حسم الاجازة الادراية 

        //[UserInterfaceParameter(Order = 70, IsReference = true)]
        //public virtual DeductionCard UnpaidLeaveDeduction { get; set; } // حسم الاجازة بلا أجر 

        [UserInterfaceParameter(Order = 55, IsReference = true)]
        public virtual DeductionCard PenaltyDeduction { get; set; } // حسم العقوبة 

        [UserInterfaceParameter(Order = 60, IsReference = true)]
        public virtual DeductionCard AbsenceDaysDeduction { get; set; } // حسم أيام الغياب 

        [UserInterfaceParameter(Order = 65, IsReference = true)]
        public virtual DeductionCard NonAttendanceDeduction { get; set; } // حسم نقص الدوام 

        [UserInterfaceParameter(Order = 70, IsReference = true)]
        public virtual DeductionCard LatenessDeduction { get; set; } // حسم التأخير الصباحي 

        //[UserInterfaceParameter(Order = 75)]
        //public virtual bool AllowAuditFeature { get; set; } // الخيار الذي يسمح بميزة التدقيق في النظام من عدمها

    }
}
