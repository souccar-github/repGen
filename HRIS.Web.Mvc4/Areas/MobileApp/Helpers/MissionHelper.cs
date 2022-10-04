using DevExpress.XtraRichEdit.Model;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class MissionHelper
    {
        public static void saveMissionRequest(Employee emp, MissionRequestViewModel employeeMissionItem,User user,int locale)
        {
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            employeeMissionItem.EmployeeId = emp.Id;
            employeeMissionItem.FullName = emp.FullName;
            employeeMissionItem.PositionId = posistion.Id;
            employeeMissionItem.PositionName = posistion.Position.NameForDropdown;
            employeeMissionItem.Description = employeeMissionItem.Description ?? "";

            var result = SaveMissionRequestItem(emp.Id, posistion.Id, employeeMissionItem,user);
        }
        public static string SaveMissionRequestItem(int employeeId, int positionId, MissionRequestViewModel missionRequestItem,User user)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.MissionRequestWorkflowName == null)
                return EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist;
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist;
            if (missionRequestItem.IsHourlyMission)
            {
                var request = new HourlyMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
                    Date = new DateTime(missionRequestItem.StartDate.Year, missionRequestItem.StartDate.Month, missionRequestItem.StartDate.Day),
                    StartDateTime = missionRequestItem.StartDate,
                    EndDateTime = missionRequestItem.StartDate,
                    StartTime = missionRequestItem.FromTime ?? new DateTime(),
                    EndTime = missionRequestItem.ToTime ?? new DateTime(),
                };
                var defaultDate = new DateTime(2000, 1, 1);
                request.StartTime =defaultDate.Add(request.StartTime.TimeOfDay);
                request.EndTime = defaultDate.Add(request.EndTime.TimeOfDay);
                request.StartDateTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.StartTime.Hour, request.StartTime.Minute, request.StartTime.Second);
                request.EndDateTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.EndTime.Hour, request.EndTime.Minute, request.EndTime.Second);

                //اختبار تكرار الطلب
                var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == employee);
                var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == employee);
                //اختبار تكرار الطلب
                if (travelMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && request.Date.Year == x.FromDate.Year
                    && request.Date.Month == x.FromDate.Month && request.Date.Day == x.FromDate.Day))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && request.Date.Year == x.Date.Year
                    && request.Date.Month == x.Date.Month && request.Date.Day == x.Date.Day &&
                   (((missionRequestItem.FromTime >= x.StartTime && missionRequestItem.FromTime <= x.EndTime) ||
                    (missionRequestItem.ToTime >= x.StartTime && missionRequestItem.ToTime <= x.EndTime)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
                }
                var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody);
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject);
                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "HourlyMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.HourlyMission + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, user);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
                return string.Empty;
            }
            else
            {
                var request = new TravelMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
                    FromDate = new DateTime(missionRequestItem.StartDate.Year, missionRequestItem.StartDate.Month, missionRequestItem.StartDate.Day),
                    ToDate = new DateTime(missionRequestItem.EndDate.Year, missionRequestItem.EndDate.Month, missionRequestItem.EndDate.Day),
                };
                var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == employee);
                var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == employee);
                //اختبار تكرار الطلب
                if (travelMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
                   (((missionRequestItem.StartDate >= x.FromDate && missionRequestItem.StartDate <= x.ToDate) ||
                    (missionRequestItem.EndDate >= x.FromDate && missionRequestItem.EndDate <= x.ToDate)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
                   (((missionRequestItem.EndDate >= x.Date && missionRequestItem.StartDate <= x.Date)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
                }
                var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody);
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject);
                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "TravelMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.TravelMission + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request },user);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify },user);
                return string.Empty;
            }



        }

        public static void SaveMissionRequestWorkflow(int workflowId, HourlyMission mission, WorkflowStepStatus status, string note,User user)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "MissionRequest";
            var destinationEntityId = "MissionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HourlyMission);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var notify = Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                mission.Status = Status.Approved;
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                mission.Status = Status.Rejected;
            }

            entities.Add(mission);

            ServiceFactory.ORMService.SaveTransaction(entities,user);
        }

        public static void SaveMissionRequestWorkflow(int workflowId, TravelMission mission, WorkflowStepStatus status, string note,User user)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "MissionRequest";
            var destinationEntityId = "MissionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TravelMission);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var notify = Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                mission.Status = Status.Approved;
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                mission.Status = Status.Rejected;
            }

            entities.Add(mission);

            ServiceFactory.ORMService.SaveTransaction(entities, user);
        }

        public static void SavePSMissionWorkflow(int workflowId, int missionId, WorkflowStepStatus status, string note, User user, bool hourly, int locale)
        {
            if (hourly)
            {
                var mission = ServiceFactory.ORMService.GetById<HourlyMission>(missionId);
                SaveMissionRequestWorkflow(workflowId, mission, status, note,user);
            }
            else
            {
                var mission = ServiceFactory.ORMService.GetById<TravelMission>(missionId);
                SaveMissionRequestWorkflow(workflowId, mission, status, note,user);
            }
        }
    }

}