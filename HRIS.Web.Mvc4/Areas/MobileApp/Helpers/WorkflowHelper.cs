using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Project.Web.Mvc4.Areas.MobileApp.Dtos;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class WorkflowHelper
    {
        public static List<WorkflowInfoDto> getPendingItems(int userId, int workflowType,int locale)
        {
            var workflowItems = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Type == (WorkflowType)workflowType && x.TargetUser.Id == userId && x.Status == WorkflowStatus.Pending);
            var result = new List<WorkflowInfoDto>();
            foreach(var item in workflowItems)
            {
                string type = "";
                LogType logType = LogType.Entrance;
                DateTime? requestDate =null;
                switch (item.Type)
                {
                    case WorkflowType.LeaveRequest:
                        var leaveRequest = ServiceFactory.ORMService.All<LeaveRequest>().Where(x=>x.WorkflowItem == item).FirstOrDefault();
                        requestDate = leaveRequest.RequestDate;
                        break;
                    case WorkflowType.EmployeeEntranceExitRecordRequest:
                        var entranceExitRequest = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        requestDate = entranceExitRequest.RecordDate;
                        logType = entranceExitRequest.LogType;
                        break;
                    case WorkflowType.EmployeeMissionRequest:
                        var missionRequest = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        if (missionRequest == null)
                        {
                           var hourlyMissionRequest = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                            requestDate = hourlyMissionRequest.CreationDate;
                        }
                        else
                        {
                            requestDate = missionRequest.CreationDate;
                        }
                        break;
                }
                var pendingUser = item.CurrentUser.FullName;
                var waitingApprove = false;
                if (item.Steps.Count() == item.StepCount)
                {
                    waitingApprove = true;
                }
                var info = new WorkflowInfoDto()
                {
                    Type = workflowType.ToString(),
                    WaitingApprove = waitingApprove,
                    PendingStep = pendingUser,
                    Date = requestDate,
                    LogType = logType
                };
                result.Add(info);
            }
            return result;
        }
    }
}