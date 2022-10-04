using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Infrastructure.Core;
using HRIS.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Web.Mvc4.Extensions.Domain;

namespace HRIS.Web.Mvc4.Areas.PayrollSystem.Services
{

    public static class PayrollSystemIntegrationService
    {

        #region // Employee Relation Services //

        public static List<PayrollSystemIntegrationDTO> GetAdministrativeLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·«Ã«“«  «·«œ«—Ì…

            List<AdministrativeLeaveRequest> administrativeLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                administrativeLeaveRequests = employee.AdministrativeLeaveRequests.
                    Where(x => x.IsDeducted && !x.IsForced && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                administrativeLeaveRequests = employee.AdministrativeLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.EndDate == toDate && x.IsDeducted && !x.IsForced && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                administrativeLeaveRequests = employee.AdministrativeLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.IsDeducted && !x.IsForced && x.IsTransferToPayroll == isAccepted).ToList();
            else
                administrativeLeaveRequests = employee.AdministrativeLeaveRequests.
                    Where(x => x.EndDate == toDate && x.IsDeducted && !x.IsForced && x.IsTransferToPayroll == isAccepted).ToList();

            List<HourlyLeaveRequest> hourlyLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                hourlyLeaveRequests = employee.HourlyLeaveRequests.Where(x => x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                hourlyLeaveRequests = employee.HourlyLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                hourlyLeaveRequests = employee.HourlyLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                hourlyLeaveRequests = employee.HourlyLeaveRequests.
                    Where(x => x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();

            var result = new List<PayrollSystemIntegrationDTO>();

            result.AddRange(administrativeLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RequiredDays,
                    Formula = Formula.DaysOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList());

            result.AddRange(hourlyLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RequiredDays,
                    Formula = Formula.HoursOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList());

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetHealthyLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·«Ã«“«  «·„—÷Ì…

            List<HealthyLeaveRequest> healthyLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                healthyLeaveRequests = employee.HealthyLeaveRequests.Where(x => x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                healthyLeaveRequests = employee.HealthyLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                healthyLeaveRequests = employee.HealthyLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                healthyLeaveRequests = employee.HealthyLeaveRequests.
                    Where(x => x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();

            var result = healthyLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RequiredDays,
                    Formula = Formula.DaysOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetUnpaidLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·«Ã«“«  »·« √Ã—

            List<UnpaidLeaveRequest> unpaidLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                unpaidLeaveRequests = employee.UnpaidLeaveRequests.
                    Where(x => x.IsDeducted && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                unpaidLeaveRequests = employee.UnpaidLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.EndDate == toDate && x.IsDeducted && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                unpaidLeaveRequests = employee.UnpaidLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.IsDeducted && x.IsTransferToPayroll == isAccepted).ToList();
            else
                unpaidLeaveRequests = employee.UnpaidLeaveRequests.
                    Where(x => x.EndDate == toDate && x.IsDeducted && x.IsTransferToPayroll == isAccepted).ToList();

            var result = unpaidLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RequiredDays,
                    Formula = Formula.DaysOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetMaternityLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·«Ã«“«  «·√„Ê„…

            List<MaternityLeaveRequest> maternityLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                maternityLeaveRequests = employee.MaternityLeaveRequests.Where(x => x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                maternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                maternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.StartDate == fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                maternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.EndDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();

            var result = maternityLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RequiredDays,
                    Formula = Formula.DaysOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetAdditionalMaternityLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·«Ã«“«  «·√„Ê„… «·«÷«›Ì…

            List<MaternityLeaveRequest> additionalMaternityLeaveRequests;

            if (!fromDate.HasValue && !toDate.HasValue)
                additionalMaternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.IsExistAdditionalMaternity && x.IsAdditionalMaternityTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                additionalMaternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.IsExistAdditionalMaternity && x.StartDate == fromDate && x.EndDate == toDate && x.IsAdditionalMaternityTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                additionalMaternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.IsExistAdditionalMaternity && x.StartDate == fromDate && x.IsAdditionalMaternityTransferToPayroll == isAccepted).ToList();
            else
                additionalMaternityLeaveRequests = employee.MaternityLeaveRequests.
                    Where(x => x.IsExistAdditionalMaternity && x.EndDate == toDate && x.IsAdditionalMaternityTransferToPayroll == isAccepted).ToList();

            var result = additionalMaternityLeaveRequests
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = (x.EndDate - x.StartDate).Days,
                    Formula = Formula.DaysOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetPenalties(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·⁄ﬁÊ»« 

            List<IncidenceDefinition> penalties;

            if (!fromDate.HasValue && !toDate.HasValue)
                penalties = employee.IncidenceDefinitions.Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.ImpositionPenalty && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                penalties = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.ImpositionPenalty && x.FirstDate == fromDate && x.SecondDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                penalties = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.ImpositionPenalty && x.FirstDate == fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                penalties = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.ImpositionPenalty && x.SecondDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();

            var result = penalties
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.IsSalary ? x.FirstDouble : x.FirstInt,
                    Formula = x.IsSalary ? Formula.FixedValue : Formula.PercentageOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetRewards(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // «·„ﬂ«›¬ 

            List<IncidenceDefinition> rewards;

            if (!fromDate.HasValue && !toDate.HasValue)
                rewards = employee.IncidenceDefinitions.Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.GrantingBonus && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                rewards = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.GrantingBonus && x.FirstDate == fromDate && x.SecondDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                rewards = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.GrantingBonus && x.FirstDate == fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                rewards = employee.IncidenceDefinitions.
                    Where(x => x.Type == Domain.EmployeeRelationServices.Enums.IncidenceType.GrantingBonus && x.SecondDate == toDate && x.IsTransferToPayroll == isAccepted).ToList();

            var result = rewards
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.IsSalary ? x.FirstDouble : x.FirstInt,
                    Formula = x.IsSalary ? Formula.FixedValue : Formula.PercentageOfSalary,
                    IncidenceDate = x.StartDate,
                    Repetition = 1,
                    SourceId = x.Id
                }).ToList();

            return result;
        }
        public static void AcceptAdministrativeLeaves(Employee employee, int sourceId)
        {
            var administrativeLeaveRequest =
                employee.AdministrativeLeaveRequests.FirstOrDefault(x => x.Id == sourceId);
            if (administrativeLeaveRequest == null)
                return;
            administrativeLeaveRequest.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptHealthyLeaves(Employee employee, int sourceId)
        {
            var healthyLeaveRequest =
                   employee.HealthyLeaveRequests.FirstOrDefault(x => x.Id == sourceId);
            if (healthyLeaveRequest == null)
                return;
            healthyLeaveRequest.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptUnpaidLeaves(Employee employee, int sourceId)
        {
            var unpaidLeaveRequest =
                      employee.UnpaidLeaveRequests.FirstOrDefault(x => x.Id == sourceId);
            if (unpaidLeaveRequest == null)
                return;
            unpaidLeaveRequest.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptMaternityLeaves(Employee employee, int sourceId)
        {
            var maternityLeaveRequest =
                         employee.MaternityLeaveRequests.FirstOrDefault(x => x.Id == sourceId);
            if (maternityLeaveRequest == null)
                return;
            maternityLeaveRequest.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptAdditionalMaternityLeaves(Employee employee, int sourceId)
        {
            var additionalMaternityLeaveRequest =
                         employee.MaternityLeaveRequests.FirstOrDefault(x => x.Id == sourceId && x.IsExistAdditionalMaternity);
            if (additionalMaternityLeaveRequest == null)
                return;
            additionalMaternityLeaveRequest.IsAdditionalMaternityTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptPenalties(Employee employee, int sourceId)
        {
            var penalty =
                            employee.IncidenceDefinitions.FirstOrDefault(x => x.Type == IncidenceType.ImpositionPenalty && x.Id == sourceId);
            if (penalty == null)
                return;
            penalty.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptRewards(Employee employee, int sourceId)
        {
            var reward =
                               employee.IncidenceDefinitions.FirstOrDefault(x => x.Type == IncidenceType.GrantingBonus && x.Id == sourceId);
            if (reward == null)
                return;
            reward.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }

        #endregion

        #region // Attendance //
        public enum AttendanceType
        {
            Overtime = 0,
            Absence = 1,
            NonAttendance = 2,
            Lateness = 3
        }
        public static List<PayrollSystemIntegrationDTO> ImportFromAttendance(AttendanceType attendanceType, Employee employee, DateTime date, bool isAccepted)
        {
            // «·œÊ«„ «·≈÷«›Ì

            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.Date.Month == date.Month);
            var payrollSystemIntegrationDtOs = new List<PayrollSystemIntegrationDTO>();

            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment = GetAttendanceWithoutAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceMonthlyAdjustment = GetAttendanceMonthlyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceDailyAdjustment = GetAttendanceDailyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);

                if (attendanceWithoutAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceWithoutAdjustment(attendanceWithoutAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceMonthlyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceMonthlyAdjustment(attendanceMonthlyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceDailyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceDailyAdjustment(attendanceDailyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }
            }

            return payrollSystemIntegrationDtOs;
        }
        private static AttendanceWithoutAdjustment GetAttendanceWithoutAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceMonthlyAdjustment GetAttendanceMonthlyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceDailyAdjustment GetAttendanceDailyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceWithoutAdjustment(AttendanceWithoutAdjustment attendanceWithoutAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                IncidenceDate = attendanceWithoutAdjustment.AttendanceRecord.Date,
                Repetition = 1,
                SourceId = attendanceWithoutAdjustment.Id
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalTotalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.TotalAbsenceDaysValue;
                        payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalNonAttendanceTotalValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalLatenessTotalValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                IncidenceDate = attendanceMonthlyAdjustment.AttendanceRecord.Date,
                Repetition = 1,
                SourceId = attendanceMonthlyAdjustment.Id
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalNonAttendanceValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceDailyAdjustment(AttendanceDailyAdjustment attendanceDailyAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                IncidenceDate = attendanceDailyAdjustment.AttendanceRecord.Date,
                Repetition = 1,
                SourceId = attendanceDailyAdjustment.Id
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.TotalAbsenceDays;
                        payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalNonAttendanceValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        public static void AcceptAttendance(AttendanceType attendanceType, DateTime date, int sourceId)
        {
            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.Date.Month == date.Month);

            if (attendanceRecord != null)
            {
                AcceptAttendanceWithoutAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceMonthlyAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceDailyAdjustment(attendanceType, attendanceRecord, sourceId);
            }
        }
        private static void AcceptAttendanceWithoutAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment =
                    attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceWithoutAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceWithoutAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceWithoutAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceWithoutAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceWithoutAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
            }



        }
        private static void AcceptAttendanceMonthlyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceMonthlyAdjustment =
                    attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceMonthlyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceMonthlyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceMonthlyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceMonthlyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceMonthlyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
            }



        }
        private static void AcceptAttendanceDailyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceDailyAdjustment =
                    attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceDailyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceDailyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceDailyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceDailyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceDailyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
            }



        }

        #endregion

    }


}