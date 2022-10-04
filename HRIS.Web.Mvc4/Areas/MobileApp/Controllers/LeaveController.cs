﻿using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Areas.MobileApp.Dtos;
using Project.Web.Mvc4.Areas.MobileApp.Helpers;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Project.Web.Mvc4.Areas.MobileApp.Controllers
{
    public class LeaveController : BaseApiController
    {
        [Route("~/api/leave/getInit")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getInitBalances(System.Net.Http.HttpRequestMessage request,string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var empCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == emp);
                var leaveRequests = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.EmployeeCard == empCard)
                    .Select(x => x.LeaveSetting).GroupBy(i => i.Id)
                    .OrderByDescending(grp => grp.Count())
                    .Select(grp => grp.Key).ToList();
                var result = new List<LeaveInfoDto>();
                var startDate = DateTime.Now;
                var firstLeave = LeaveHelper.GetInformationForLeaveRequest(empCard.Id, leaveRequests[0], startDate, int.Parse(locale));
                var secLeave = LeaveHelper.GetInformationForLeaveRequest(empCard.Id, leaveRequests[1], startDate, int.Parse(locale));
                if (firstLeave == null || secLeave == null)
                {
                    return BadRequest();
                }
                result.Add(firstLeave);
                result.Add(secLeave);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("~/api/leave/getLeaveSettings")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getLeaveSettings(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);

                if (employeeCard.LeaveTemplateMaster != null)
                {
                    var result = employeeCard.LeaveTemplateMaster.LeaveTemplateDetails.Select(x => new { Title = x.LeaveSetting.Name, Id = x.LeaveSetting.Id }).ToList();
                    return Ok(result); ;
                }

                var position = employeeCard.Employee.PrimaryPosition();
                if (position != null)
                {
                    if (position.JobDescription.JobTitle.Grade.LeaveTemplateMaster != null)
                    {
                        var result = position.JobDescription.JobTitle.Grade.LeaveTemplateMaster.LeaveTemplateDetails.
                            Select(x => new { Title = x.LeaveSetting.Name, Id = x.LeaveSetting.Id }).ToList();
                        return Ok(result);
                    }
                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/getLeaveReasons")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getLeaveReasons(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var empCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == emp);
                var leaveSettings = ServiceFactory.ORMService.All<LeaveReason>().Select(x => new { Id = x.Id, Title = x.Name });
                return Ok(leaveSettings);
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("~/api/leave/getLeaveSettingInfo/{id}/{day}/{month}/{year}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getLeaveSettingInfo(System.Net.Http.HttpRequestMessage request, int id, int day, int month, int year, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var date = new DateTime(year, month, day);
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var empCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == emp);
                var result = LeaveHelper.GetInformationForLeaveRequest(empCard.Id, id, date, int.Parse(locale));
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/postRequest")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult postLeaveRequest(System.Net.Http.HttpRequestMessage request, LeaveRequestViewModel leave, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                try
                {
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    leave.EmployeeId = emp.Id;
                    leave.FullName = emp.FullName;
                    leave.PositionId = posistion.Id;
                    leave.PositionName = posistion.Position.NameForDropdown;
                    LeaveHelper.saveLeaveRequest(emp, leave, int.Parse(locale));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/getPendingLeaveRequests")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getPendingLeaveRequests(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var result = new List<LeaveRequestViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }

                var employeeLeaveRequests =
                    ServiceFactory.ORMService.All<LeaveRequest>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

                foreach (var leave in employeeLeaveRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(leave.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = leave.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new LeaveRequestViewModel()
                        {
                            EmployeeId = leave.EmployeeCard.Employee.Id,
                            FullName = leave.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            LeaveId = leave.Id,
                            //LeaveSettingId = leave.Id,
                            LeaveSettingName = leave.LeaveSetting.Name,
                            StartDate = DateTime.Parse(leave.StartDate.ToShortDateString()),
                            EndDate = DateTime.Parse(leave.EndDate.ToShortDateString()),
                            IsHourlyLeave = leave.IsHourlyLeave,
                            FromTime = (leave.IsHourlyLeave != true) ? null : leave.FromTime,//FromTime = leave.FromTime.GetValueOrDefault(),//DateTime.Parse(leave.FromTime.ToShortDateString()),
                            ToTime = (leave.IsHourlyLeave != true) ? null : leave.ToTime,//ToTime = leave.ToTime.GetValueOrDefault(),//DateTime.Parse(leave.ToTime.ToShortDateString()),
                            SpentDays = leave.SpentDays,
                            LeaveReason = leave.LeaveReason.Name ?? string.Empty,
                            RequestDate = DateTime.Parse(leave.RequestDate.ToShortDateString()),
                            Description = leave.Description ?? string.Empty,
                            WorkflowItemId = leave.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                    }
                }

                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/accept/{wfId}/{leaveId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult accept(System.Net.Http.HttpRequestMessage request, int wfId, int leaveId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LeaveHelper.SavePSLeaveWorkflow(wfId, leaveId, WorkflowStepStatus.Accept, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/reject/{wfId}/{leaveId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult reject(System.Net.Http.HttpRequestMessage request, int wfId, int leaveId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LeaveHelper.SavePSLeaveWorkflow(wfId, leaveId, WorkflowStepStatus.Reject, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/pending/{wfId}/{leaveId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult pending(System.Net.Http.HttpRequestMessage request, int wfId, int leaveId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LeaveHelper.SavePSLeaveWorkflow(wfId, leaveId, WorkflowStepStatus.Pending, note == "null"?"":note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/getSpentDays")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getSpentDays(System.Net.Http.HttpRequestMessage request, LeaveRequestViewModel leaveItem, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLeaveRequest", identity);
            if (auth)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var startDate = new DateTime(leaveItem.StartDate.Year, leaveItem.StartDate.Month, leaveItem.StartDate.Day);
                var endDate = new DateTime(leaveItem.EndDate.Year, leaveItem.EndDate.Month, leaveItem.EndDate.Day);
                double spentDays = 0.0;
                var result = new Dictionary<string, object>();
                var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(leaveItem.LeaveSettingId);

                if (leaveSetting != null)
                    if (leaveSetting.IsIndivisible)
                    {
                        var balance = LeaveService.GetBalance(leaveSetting, employee, false, DateTime.Today);
                        var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, DateTime.Today.Year - 1);
                        balance += recycledBalance;
                        spentDays = balance;

                    }
                    else
                    {
                        if (leaveItem.IsHourlyLeave)
                        {
                            if (leaveItem.FromTime == null || leaveItem.ToTime == null)
                            {
                                spentDays = 0;
                            }
                            else
                            {
                                var minutes = 0.00;
                                if (leaveItem.FromTime > leaveItem.ToTime)
                                {
                                    var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                                    var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                                    var minutesbefore = (maxDay.TimeOfDay - leaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                                    var minutesafter = (leaveItem.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                                    minutes = minutesafter + minutesbefore;

                                }
                                else
                                {
                                    minutes = (leaveItem.ToTime.GetValueOrDefault().TimeOfDay - leaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                                }
                                //var minutes =
                                //    (leaveItem.ToTime.GetValueOrDefault() - leaveItem.FromTime.GetValueOrDefault()).TotalMinutes;
                                spentDays =
                                    Math.Round(
                                        1 /
                                        ((leaveSetting.HoursEquivalentToOneLeaveDay *
                                          EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                            }
                        }
                        else
                        {
                            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                            {
                                spentDays = 0;
                            }
                            else
                            {
                                spentDays = LeaveService.GetSpentDays(startDate, endDate,
                                    leaveSetting.IsContinuous, employee);
                            }
                        }
                    }
                result["SpentDays"] = spentDays;
                return Json(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/leave/getLeaveByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getLeaveByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var leave =
                    ServiceFactory.ORMService.All<LeaveRequest>()
                    .FirstOrDefault(x => x.WorkflowItem.Id == id);
            var result = new LeaveRequestViewModel()
            {
                EmployeeId = leave.EmployeeCard.Employee.Id,
                FullName = leave.EmployeeCard.Employee.FullName,
                LeaveId = leave.Id,
                //LeaveSettingId = leave.Id,
                LeaveSettingName = leave.LeaveSetting.Name,
                StartDate = DateTime.Parse(leave.StartDate.ToShortDateString()),
                EndDate = DateTime.Parse(leave.EndDate.ToShortDateString()),
                IsHourlyLeave = leave.IsHourlyLeave,
                FromTime = (leave.IsHourlyLeave != true) ? null : leave.FromTime,//FromTime = leave.FromTime.GetValueOrDefault(),//DateTime.Parse(leave.FromTime.ToShortDateString()),
                ToTime = (leave.IsHourlyLeave != true) ? null : leave.ToTime,//ToTime = leave.ToTime.GetValueOrDefault(),//DateTime.Parse(leave.ToTime.ToShortDateString()),
                SpentDays = leave.SpentDays,
                LeaveReason = leave.LeaveReason.Name ?? string.Empty,
                RequestDate = DateTime.Parse(leave.RequestDate.ToShortDateString()),
                Description = leave.Description ?? string.Empty,
                WorkflowItemId = leave.WorkflowItem.Id,
            };
            return Ok(result);
        }

        [Route("~/api/leave/getMyPending/{loc}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getMyPending(System.Net.Http.HttpRequestMessage request, int loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
            var result = Helpers.WorkflowHelper.getPendingItems(user.Id, (int) WorkflowType.LeaveRequest,locale);
            return Ok(result);
        }
    }
}