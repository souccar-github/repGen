using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.Indexes;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;
using Souccar.Core.Utilities;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    [Command(CommandsNames.AcceptPenalty, Order = 1)]
    public class AttendanceMonthlyAdjustment : Entity, IAggregateRoot
    {
        public AttendanceMonthlyAdjustment()
        {
            AttendanceMonthlyAdjustmentDetails = new List<AttendanceMonthlyAdjustmentDetail>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceRecord AttendanceRecord { get; set; }

        //[UserInterfaceParameter(Order = 1, IsReference = true)]
        //public virtual EmployeeAttendanceCard EmployeeAttendanceCard { get; set; }

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual EmployeeCard EmployeeAttendanceCard { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual MonthlyAdjustmentAttendanceStatus AttendanceStatus
        {
            get
            {
                return TotalActualWorkHoursValue > TotalRequiredWorkHoursValue
                    ? MonthlyAdjustmentAttendanceStatus.Overtime
                    : TotalActualWorkHoursValue < TotalRequiredWorkHoursValue
                        ? MonthlyAdjustmentAttendanceStatus.NonAttendance
                        : MonthlyAdjustmentAttendanceStatus.Ok;
            }
        }

        [UserInterfaceParameter(Order = 3, IsHidden = true)]
        public virtual double TotalWorkHoursValue
        {
            get
            {
                return AttendanceMonthlyAdjustmentDetails == null ? 0 : AttendanceMonthlyAdjustmentDetails.Sum(x => x.WorkHoursValue);
            }
        } // عدد ساعات العمل المطلوبة بالشهر حسب الوردية

        [UserInterfaceParameter(Order = 3)]
        public virtual string TotalWorkHoursValueFormatedValue
        {
            get
            {
                var result = AttendanceMonthlyAdjustmentDetails.Sum(x => x.WorkHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات العمل المطلوبة بالشهر حسب الوردية

        [UserInterfaceParameter(Order = 4, IsHidden = true)]
        public virtual double TotalActualWorkHoursValue
        {
            get
            {
                return AttendanceMonthlyAdjustmentDetails == null ? 0 : AttendanceMonthlyAdjustmentDetails.Sum(x => x.ActualWorkHoursValue);
            }
        } // عدد ساعات العمل المحققة بالشهر

        [UserInterfaceParameter(Order = 4)]
        public virtual string TotalActualWorkHoursValueFormatedValue
        {
            get
            {
                var result = AttendanceMonthlyAdjustmentDetails.Sum(x => x.ActualWorkHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات العمل المحققة بالشهر

        [UserInterfaceParameter(Order = 5, IsHidden = true)]
        public virtual double TotalRequiredWorkHoursValue
        {
            get
            {
                return AttendanceMonthlyAdjustmentDetails == null ? 0 : AttendanceMonthlyAdjustmentDetails.Sum(x => x.RequiredWorkHoursValue);
            }
        } // عدد ساعات العمل المطلوبة بالشهر

        [UserInterfaceParameter(Order = 5)]
        public virtual string TotalRequiredWorkHoursValueFormatedValue
        {
            get
            {
                var result = AttendanceMonthlyAdjustmentDetails.Sum(x => x.RequiredWorkHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات العمل المطلوبة بالشهر

        [UserInterfaceParameter(Order = 6, IsHidden = true)]
        public virtual double ExpectedOvertimeValue
        {
            get
            {
                return TotalActualWorkHoursValue > TotalRequiredWorkHoursValue
                    ? TotalActualWorkHoursValue - TotalRequiredWorkHoursValue
                    : 0;
            }
        } // الاضافي الشهري المحتمل

        [UserInterfaceParameter(Order = 6)]
        public virtual string ExpectedOvertimeValueFormatedValue
        {
            get
            {
                var result = TotalActualWorkHoursValue > TotalRequiredWorkHoursValue ? TotalActualWorkHoursValue - TotalRequiredWorkHoursValue : 0;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الشهري المحتمل

        [UserInterfaceParameter(Order = 7, IsHidden = true)]
        public virtual double TotalOvertimeOrderValue
        {
            get
            {
                return AttendanceMonthlyAdjustmentDetails.Sum(x => x.OvertimeOrderValue);
            }
        } // عدد ساعات التكليف

        [UserInterfaceParameter(Order = 7)]
        public virtual string TotalOvertimeOrderValueFormatedValue
        {
            get
            {
                var result = AttendanceMonthlyAdjustmentDetails.Sum(x => x.OvertimeOrderValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات التكليف

        [UserInterfaceParameter(Order = 8, IsHidden = true)]//  يمكن جعله للقراءة فقط لكن يفضل تخزينه لاحتمال التغيير بالاعدادات بالتالي اختلاف النتيجة مع الوقت
        public virtual double InitialOvertimeValue { get; set; } // الاضافي الشهري المحتسب قبل معامل الضرب

        [UserInterfaceParameter(Order = 8)]//  يمكن جعله للقراءة فقط لكن يفضل تخزينه لاحتمال التغيير بالاعدادات بالتالي اختلاف النتيجة مع الوقت
        public virtual string InitialOvertimeValueFormatedValue { get; set; } // الاضافي الشهري المحتسب قبل معامل الضرب

        [UserInterfaceParameter(Order = 9, IsHidden = true)]//  يمكن جعله للقراءة فقط لكن يفضل تخزينه لاحتمال التغيير بالاعدادات بالتالي اختلاف النتيجة مع الوقت
        public virtual double FinalOvertimeValue { get; set; } // الاضافي الشهري المحتسب بعد معامل الضرب

        [UserInterfaceParameter(Order = 9)]//  يمكن جعله للقراءة فقط لكن يفضل تخزينه لاحتمال التغيير بالاعدادات بالتالي اختلاف النتيجة مع الوقت
        public virtual string FinalOvertimeValueFormatedValue { get; set; } // الاضافي الشهري المحتسب بعد معامل الضرب

        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual double InitialNonAttendanceValue
        {
            get
            {
                return TotalRequiredWorkHoursValue > TotalActualWorkHoursValue
                    ? TotalRequiredWorkHoursValue - TotalActualWorkHoursValue
                    : 0;
            }
        } //  عدم التواجد الشهري قبل معامل الضرب

        [UserInterfaceParameter(Order = 10)]
        public virtual string InitialNonAttendanceValueFormatedValue
        {
            get
            {
                var result = TotalRequiredWorkHoursValue > TotalActualWorkHoursValue
                    ? TotalRequiredWorkHoursValue - TotalActualWorkHoursValue
                    : 0;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } //  عدم التواجد الشهري قبل معامل الضرب

        [UserInterfaceParameter(Order = 11, IsHidden = true)]
        public virtual double FinalNonAttendanceValue { get; set; } //  عدم التواجد الشهري بعد معامل الضرب

        [UserInterfaceParameter(Order = 11)]
        public virtual string FinalNonAttendanceValueFormatedValue { get; set; } //  عدم التواجد الشهري بعد معامل الضرب

        [UserInterfaceParameter(Order = 12, IsReference = true)]
        public virtual DisciplinarySetting Penalty { get; set; } // العقوبة المترتبة على المخالفة المتعلقة بالدوام وذلك حسب نموذج التأخير والمخالفة المرتبطة بها

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsOvertimeTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsAbsenceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsNonAttendanceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsLatenessTransferToPayroll { get; set; }

        [UserInterfaceParameter(Order = 13)]
        public virtual IList<AttendanceMonthlyAdjustmentDetail> AttendanceMonthlyAdjustmentDetails { get; set; }
        public virtual void AddAttendanceMonthlyAdjustmentDetail(AttendanceMonthlyAdjustmentDetail attendanceMonthlyAdjustmentDetail)
        {
            AttendanceMonthlyAdjustmentDetails.Add(attendanceMonthlyAdjustmentDetail);
            attendanceMonthlyAdjustmentDetail.AttendanceMonthlyAdjustment = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return EmployeeAttendanceCard.Employee.NameForDropdown; } }
    }

}
