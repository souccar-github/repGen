using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Command(CommandsNames.GenerateAttendanceRecord, Order = 1)]
    [Command(CommandsNames.CalculateAttendanceRecord, Order = 2)]
    [Command(CommandsNames.LockAttendanceRecord, Order = 3)]


    //  يشبه الشهر في الرواتب والاجور ويعبر عن المعلومات الاساسية للشهر الذس سنقوم بحساب الدوام للموظفين فيه
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class AttendanceRecord : Entity, IAggregateRoot // سجل دوام الموظفين مع تقاص شهري
    {
        public AttendanceRecord()
        {
            AttendanceDailyAdjustments = new List<AttendanceDailyAdjustment>();
            AttendanceMonthlyAdjustments = new List<AttendanceMonthlyAdjustment>();
            AttendanceWithoutAdjustments = new List<AttendanceWithoutAdjustment>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // 

        [UserInterfaceParameter(Order = 2)]
        public virtual string Name { get; set; } // اسم الشهر

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual AttendanceMonthStatus AttendanceMonthStatus { get; set; } // حالة الشهر 

        [UserInterfaceParameter(Order = 5)]
        public virtual IList<AttendanceWithoutAdjustment> AttendanceWithoutAdjustments { get; set; }
        public virtual void AddAttendanceWithoutAdjustment(AttendanceWithoutAdjustment attendanceWithoutAdjustment)
        {
            AttendanceWithoutAdjustments.Add(attendanceWithoutAdjustment);
            attendanceWithoutAdjustment.AttendanceRecord = this;
        }

        [UserInterfaceParameter(Order = 6)]
        public virtual IList<AttendanceDailyAdjustment> AttendanceDailyAdjustments { get; set; }
        public virtual void AddAttendanceDailyAdjustment(AttendanceDailyAdjustment attendanceDailyAdjustment)
        {
            AttendanceDailyAdjustments.Add(attendanceDailyAdjustment);
            attendanceDailyAdjustment.AttendanceRecord = this;
        }

        [UserInterfaceParameter(Order = 7)]
        public virtual IList<AttendanceMonthlyAdjustment> AttendanceMonthlyAdjustments { get; set; }
        public virtual void AddAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment)
        {
            AttendanceMonthlyAdjustments.Add(attendanceMonthlyAdjustment);
            attendanceMonthlyAdjustment.AttendanceRecord = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }

}
