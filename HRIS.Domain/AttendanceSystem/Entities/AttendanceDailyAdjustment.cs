﻿using System.Collections.Generic;
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
    public class AttendanceDailyAdjustment : Entity, IAggregateRoot
    {
        public AttendanceDailyAdjustment()
        {
            AttendanceDailyAdjustmentDetails = new List<AttendanceDailyAdjustmentDetail>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceRecord AttendanceRecord { get; set; }

        //[UserInterfaceParameter(Order = 1, IsReference = true)]
        //public virtual EmployeeAttendanceCard EmployeeAttendanceCard { get; set; }

        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual EmployeeCard EmployeeAttendanceCard { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual double TotalAbsenceDays
        {
            get
            {
                return AttendanceDailyAdjustmentDetails == null
                    ? 0
                    : AttendanceDailyAdjustmentDetails.Count(x => x.DailyAdjustmentAttendanceStatus == DailyAdjustmentAttendanceStatus.Absence);
            }
        } // عدد الايام التي حالة السجل فيها غياب

        [UserInterfaceParameter(Order = 4, IsHidden = true)]
        public virtual double TotalNormalOvertimeValue
        {
            get
            {
                return AttendanceDailyAdjustmentDetails == null
                    ? 0
                    : AttendanceDailyAdjustmentDetails.Sum(x => x.NormalOvertimeValue);
            }
        } // الاضافي الاجمالي المحتسب العادي

        [UserInterfaceParameter(Order = 4)]
        public virtual string TotalNormalOvertimeValueFormatedValue
        {
            get
            {
                var result = AttendanceDailyAdjustmentDetails.Sum(x => x.NormalOvertimeValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الاجمالي المحتسب العادي

        [UserInterfaceParameter(Order = 5, IsHidden = true)]
        public virtual double TotalHolidayOvertimeValue
        {
            get
            {
                return AttendanceDailyAdjustmentDetails == null
                    ? 0
                    : AttendanceDailyAdjustmentDetails.Sum(x => x.HolidayOvertimeValue);
            }

        } // الاضافي الاجمالي المحتسب العطل

        [UserInterfaceParameter(Order = 5)]
        public virtual string TotalHolidayOvertimeValueFormatedValue
        {
            get
            {
                var result = AttendanceDailyAdjustmentDetails.Sum(x => x.HolidayOvertimeValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }

        } // الاضافي الاجمالي المحتسب العطل

        [UserInterfaceParameter(Order = 6, IsHidden = true)]
        public virtual double FinalOvertimeValue { get; set; } // الاضافي المحتسب بعد معامل الضرب

        [UserInterfaceParameter(Order = 6)]
        public virtual string FinalOvertimeValueFormatedValue { get; set; } // الاضافي المحتسب بعد معامل الضرب

        [UserInterfaceParameter(Order = 7, IsHidden = true)]
        public virtual double InitialNonAttendanceValue
        {
            get
            {
                return AttendanceDailyAdjustmentDetails == null
                    ? 0
                    : AttendanceDailyAdjustmentDetails.Sum(x => x.NonAttendanceHoursValue);
            }
        } //  عدم التواجد الشهري قبل معامل الضرب

        [UserInterfaceParameter(Order = 7)]
        public virtual string InitialNonAttendanceValueFormatedValue
        {
            get
            {
                var result = AttendanceDailyAdjustmentDetails.Sum(x => x.NonAttendanceHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } //  عدم التواجد الشهري قبل معامل الضرب

        [UserInterfaceParameter(Order = 8, IsHidden = true)]
        public virtual double FinalNonAttendanceValue { get; set; } //  عدم التواجد الشهري بعد معامل الضرب

        [UserInterfaceParameter(Order = 8)]
        public virtual string FinalNonAttendanceValueFormatedValue { get; set; } //  عدم التواجد الشهري بعد معامل الضرب

        [UserInterfaceParameter(Order = 9, IsReference = true)]
        public virtual DisciplinarySetting Penalty { get; set; } // العقوبة المترتبة على المخالفة المتعلقة بالدوام وذلك حسب نموذج التأخير والمخالفة المرتبطة بها

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsOvertimeTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsAbsenceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsNonAttendanceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsLatenessTransferToPayroll { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual IList<AttendanceDailyAdjustmentDetail> AttendanceDailyAdjustmentDetails { get; set; }
        public virtual void AddAttendanceDailyAdjustmentDetail(AttendanceDailyAdjustmentDetail attendanceDailyAdjustmentDetail)
        {
            AttendanceDailyAdjustmentDetails.Add(attendanceDailyAdjustmentDetail);
            attendanceDailyAdjustmentDetail.AttendanceDailyAdjustment = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return EmployeeAttendanceCard.Employee.NameForDropdown; } }
    }
}
