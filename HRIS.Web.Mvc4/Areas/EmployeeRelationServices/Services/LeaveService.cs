using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using NHibernate.Criterion;
using Souccar.Infrastructure.Core;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class LeaveService
    {
        public static double GetBalance(LeaveSetting leaveSetting, Employee employee, bool IsForRecycle, DateTime endDate)
        {
            double balance = 0;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee, endDate);
            if (employeeServiceHistory.Years == 0 && employeeServiceHistory.Months == 0 && employeeServiceHistory.Days == 0)
                return 0.00;
             var yearsOfService=0;
             var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
             var monthsOfService = employeeServiceHistory.Months;
             
            var DiffBetweenMonthsOfNowAndMonthsOfServ = monthsOfService - (endDate.Month - 1);
            if (IsForRecycle)
            {
                ///here not important to to  diff so diff had assigned to -1
                DiffBetweenMonthsOfNowAndMonthsOfServ = -1;
                if (employeeCard.StartWorkingDate.Value.Year == (endDate.Year - 1) && employeeServiceHistory.Years > 0)
                     yearsOfService  = employeeServiceHistory.Years-1;
                else
                    yearsOfService = employeeServiceHistory.Years;
            }
            else
            {
                yearsOfService = employeeServiceHistory.Years;
            }
             
            
            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);
                    balance = leaveSettingBalanceSlice != null ? leaveSettingBalanceSlice.Balance : leaveSetting.Balance;
                }
                else
                    balance = leaveSetting.Balance;


                if (leaveSetting.IsAffectedByAssigningDate && yearsOfService == 0 && DiffBetweenMonthsOfNowAndMonthsOfServ < 0)
                {
                    
                    if (employeeCard != null)
                    {
                        var startWorkingDate = employeeCard.StartWorkingDate.Value;
                        if (startWorkingDate == null){
                                   startWorkingDate=DateTime.Today;
                        }
                 
                        balance =Math.Round( (balance / EmployeeRelationServicesConstants.NumberOfMonthsInYear) * (12 - startWorkingDate.Month + 1) ,2);
                        
                    }
                }
            }

            return balance;
        }
      
        public static double GetMonthlyBalance(LeaveSetting leaveSetting, Employee employee)
        {
            double monthlyBalance = 0;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee,DateTime.Today);
            var yearsOfService = employeeServiceHistory.Years;
            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);
                    if (leaveSettingBalanceSlice != null && leaveSettingBalanceSlice.HasMonthlyBalance)
                        monthlyBalance = leaveSettingBalanceSlice.MonthlyBalance;
                }
                else
                {
                    if (leaveSetting.HasMonthlyBalance)
                        monthlyBalance = leaveSetting.MonthlyBalance;
                }
            }

            return monthlyBalance;
        }

        public static bool HasMonthlyBalance(LeaveSetting leaveSetting, Employee employee)
        {
            bool hasMonthlyBalance = false;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee, DateTime.Today);
            var yearsOfService = employeeServiceHistory.Years;
            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);
                    if (leaveSettingBalanceSlice != null && leaveSettingBalanceSlice.HasMonthlyBalance)
                        hasMonthlyBalance = true;
                }
                else
                {
                    if (leaveSetting.HasMonthlyBalance)
                        hasMonthlyBalance = true;
                }
            }

            return hasMonthlyBalance;
        }

        public static double GetRecycledBalance(Employee employee, LeaveSetting leaveSetting, int year)
        {
            double recycledBalance = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.RecycledLeaves != null && employeeCard.RecycledLeaves.Count > 0)
            {
                var recycledLeave = employeeCard.RecycledLeaves.FirstOrDefault(x => x.LeaveSetting == leaveSetting && x.Year == year
                    && x.RecycleType == RecycleType.Balance);
                if (recycledLeave != null)
                    recycledBalance = recycledLeave.RoundedBalance;
            }

            return recycledBalance;
        }

        public static double GetGranted(Employee employee, LeaveSetting leaveSetting, int year)
        {
            double granted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequestsInYear(employeeCard, leaveSetting, year);
                granted += leaveRequests.Sum(leaveRequest => GetDaysCountInLeave(leaveRequest, year));
            }
            granted = Math.Round(granted,2);
            return granted;
        }
        public static double GetGranted(Employee employee, LeaveSetting leaveSetting)
        {
            double granted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequests(employeeCard, leaveSetting);
                granted += leaveRequests.Count * leaveSetting.Balance;
            }
            granted = Math.Round(granted, 2);
            return granted;
        }
        public static double GetMonthlyGranted(Employee employee, LeaveSetting leaveSetting, DateTime date)
        {
            double monthlyGranted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequestsInMonth(employeeCard, leaveSetting, date);
                monthlyGranted += leaveRequests.Sum(leaveRequest => GetMonthlyDaysCountInLeave(leaveRequest));
            }

            return monthlyGranted;
        }

        public static List<LeaveRequest> GetLeaveRequestsInYear(EmployeeCard employeeCard, LeaveSetting leaveSetting, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var lastDay = new DateTime(year, 12, 31);
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        (x.LeaveSetting == leaveSetting) && ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                        (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                        (x.StartDate <= firstDay && x.EndDate >= lastDay)) && ((x.LeaveStatus==Status.Approved)||(x.LeaveStatus==Status.Draft))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequests(EmployeeCard employeeCard, LeaveSetting leaveSetting)
        {
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        (x.LeaveSetting == leaveSetting) && ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequestsInMonth(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                        x.LeaveSetting == leaveSetting && ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                        (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                        (x.StartDate <= firstDay && x.EndDate >= lastDay))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequestsInMonth(EmployeeCard employeeCard, DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                        ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                         (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                         (x.StartDate <= firstDay && x.EndDate >= lastDay))).ToList();
        }

        public static double GetDaysCountInLeave(LeaveRequest leaveRequest, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var lastDay = new DateTime(year, 12, 31);
            if (leaveRequest.StartDate.Year == leaveRequest.EndDate.Year)
                return leaveRequest.SpentDays;
            else if (leaveRequest.StartDate.Year == year)
            {
                return (lastDay - leaveRequest.StartDate).TotalDays;
            }
            else if (leaveRequest.EndDate.Year == year)
            {
                return (leaveRequest.EndDate - firstDay).TotalDays;
            }
            else
            {
                return firstDay.DayOfYear;
            }
        }
        public static double GetMonthlyDaysCountInLeave(LeaveRequest leaveRequest)
        {
            if (leaveRequest.StartDate.Month == leaveRequest.EndDate.Month)
                return leaveRequest.SpentDays;

            return 0;
        }

        public static DateTime GetEndDate(DateTime startDate, double days, bool isContinues, Employee employee)
        {
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var attendanceForm = Project.Web.Mvc4.Areas.AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            IList<WorkshopRecurrence> recurrences = attendanceForm != null ? attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList() : new List<WorkshopRecurrence>();
            var endDate = startDate;

            if (isContinues)
                endDate = startDate.AddDays(days - 1);
            else
            {
                var date = startDate;
                var i = 0;
                var holidayDays = 0;

                while (i < days)
                {
                    if ((HolidayService.IsPublicHoliday(date) || HolidayService.IsChangeableHoliday(date) ||
                        HolidayService.IsFixedHoliday(date)) && (attendanceForm != null ? attendanceForm.RelyHolidaies : true))
                        holidayDays++;
                    else
                    {
                        if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) && recurrences.Count == 7)
                            if (recurrences[(int) date.DayOfWeek].IsOff)
                                holidayDays++;
                        else
                            i++;
                    }

                    date = date.AddDays(1);
                }

                endDate = startDate.AddDays(days + holidayDays - 1);
            }

            return endDate;
        }

        public static double GetSpentDays(DateTime startDate, DateTime endDate, bool isContinues, Employee employee)
        {
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var attendanceForm = Project.Web.Mvc4.Areas.AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            IList<WorkshopRecurrence> recurrences = attendanceForm != null ? attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList() : new List<WorkshopRecurrence>();
            double spentDays = 0;

            if (isContinues)
            {
                while (DateTime.Parse(startDate.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
                {
                    spentDays++;
                    startDate = startDate.AddDays(1);
                }
            }
            else
            {
                while (DateTime.Parse(startDate.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
                {
                    if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) ||
                        (!HolidayService.IsPublicHoliday(startDate) && 
                         !HolidayService.IsChangeableHoliday(startDate) &&
                         !HolidayService.IsFixedHoliday(startDate)))
                    {
                        if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) && recurrences.Count == 7)
                        {
                            if (!recurrences[(int)startDate.DayOfWeek].IsOff)
                            {
                                spentDays++;
                            }
                        }
                        else
                            spentDays++;
                    }

                    startDate = startDate.AddDays(1);
                }
            }

            return spentDays;
        }

        public static int GetCountInYears(Employee employee, LeaveSetting leaveSetting)
        {
            var countInYears = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
                countInYears = employeeCard.LeaveRequests.Count(x => x.LeaveStatus != Status.Rejected && x.LeaveSetting == leaveSetting);

            return countInYears;
        }

        public static bool IsValidIntervalDays(DateTime requestDate, DateTime startDate, int intervalDays)
        {
            if (intervalDays > 0)
            {
                if ((DateTime.Parse(startDate.ToLongDateString()) - DateTime.Parse(requestDate.ToLongDateString())).Days < intervalDays)
                    return false;
            }

            return true;
        }

        public static bool IsValidLeaveDate(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime stratDate, DateTime endDate,
            LeaveRequest leaveRequestBeforeUpdate = null)
        {
            if (leaveRequestBeforeUpdate != null)
            {
                return !employeeCard.LeaveRequests.Where(x => x != leaveRequestBeforeUpdate).Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                   ( (stratDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && stratDate <= DateTime.Parse(x.EndDate.ToShortDateString())) ||
                    (endDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && endDate <= DateTime.Parse(x.EndDate.ToShortDateString()))));
            }
            else
            {
                return !employeeCard.LeaveRequests.Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                   ( ((stratDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && stratDate <= DateTime.Parse(x.EndDate.ToShortDateString())) ||
                    (endDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && endDate <= DateTime.Parse(x.EndDate.ToShortDateString())))));
            }
        }

        public static bool IsHourlyLeaveValidLeave(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime stratDate,
            TimeSpan stratTime, TimeSpan endTime, LeaveRequest leaveRequestBeforeUpdate = null)
        {
            var s =stratTime;
            var e = endTime;

            if (leaveRequestBeforeUpdate != null)
            {
                return !employeeCard.LeaveRequests.Where(x => x != leaveRequestBeforeUpdate && x.StartDate == stratDate).Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&((x.IsHourlyLeave==false)||
                    ((stratTime >= x.FromTime.Value.TimeOfDay &&
                     stratTime <= x.ToTime.Value.TimeOfDay) ||
                    (endTime >= x.FromTime.Value.TimeOfDay &&
                     endTime <= x.ToTime.Value.TimeOfDay))));
            }
            else
            {
                return !employeeCard.LeaveRequests.Where(x => x.StartDate == stratDate).Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&((x.IsHourlyLeave==false)||
                    ((stratTime >= x.FromTime.Value.TimeOfDay &&
                     stratTime <= x.ToTime.Value.TimeOfDay) ||
                    (endTime >= x.FromTime.Value.TimeOfDay &&
                     endTime <= x.ToTime.Value.TimeOfDay) ||
                    (stratTime <= x.FromTime.Value.TimeOfDay &&
                     endTime >= x.ToTime.Value.TimeOfDay))));
            }
        }

    }
}