#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 31/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Global.Constant;
using Status = HRIS.Domain.Global.Enums.Status;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Infrastructure.Extenstions;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Workflow;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper
{
    public static class ServiceHelper
    {
        #region تدفق عمل منح المكافئة
        public static void SaveRewardWorkflow(int workflowId, EmployeeReward reward, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalBody);
           
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "RewardRequest";
            var destinationEntityId = "RewardRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", reward.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
            destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData,
             out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeRewardInfo(reward, reward.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                reward.RewardStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل فرض عقوبة
        public static void SaveDisciplinaryWorkflow(int workflowId, EmployeeDisciplinary disciplinary, WorkflowStepStatus status, string note)
        {
            //var currentUser = UserExtensions.CurrentUser;
            //disciplinary.Creator = currentUser;
            //disciplinary.CreationDate = DateTime.Now;

            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "DisciplinaryRequest";
            var destinationEntityId = "DisciplinaryRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", disciplinary.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                var terminationDecisionNotify = AddTerminationDecisionNotify(disciplinary, disciplinary.EmployeeCard);
                if (terminationDecisionNotify != null)
                {
                    entities.Add(terminationDecisionNotify);
                }
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                disciplinary.DisciplinaryStatus = Status.Rejected;
            }
            entities.Add(disciplinary);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل انهاء الخدمة
        public static void SaveTerminationWorkflow(int workflowId, EmployeeTermination termination, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalBody);
          
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "TerminationRequest";
            var destinationEntityId = "TerminationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", termination.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeTerminationInfo(termination, termination.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                termination.TerminationStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل منح مكافئة
        public static void SavePromotionWorkflow(int workflowId, EmployeePromotion promotion, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "PromotionRequest";
            var destinationEntityId = "PromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", promotion.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeePromotionInfo(promotion));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                promotion.PromotionStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion
        
        #region تدفق عمل منح مكافئة مالية
        public static void SaveFinancialPromotionWorkflow(int workflowId, FinancialPromotion financialPromotion, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalBody) + " " + workflow.TargetUser.FullName;

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "FinancialPromotionRequest";
            var destinationEntityId = "FinancialPromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", financialPromotion.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeFinancialPromotionInfo(financialPromotion));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                financialPromotion.FinancialPromotionStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل الاستقالة
        public static void SaveResignationWorkflow(int workflowId, EmployeeResignation resignation, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalBody);
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalSubjectFor), workflow.TargetUser.FullName);
            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "ResignationRequest";
            var destinationEntityId = "ResignationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", resignation.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeResignationInfo(resignation, resignation.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                resignation.ResignationStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل الاجازات
        public static void SaveLeaveWorkflow(int workflowId, LeaveRequest leave, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveApprovalBody) + " " + workflow.TargetUser.FullName;
           
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveApprovalSupject), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            if(leave!=null)
            destinationData.Add("ServiceId", leave.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, body, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                leave.LeaveStatus = Status.Approved;
                //entities.AddRange(ChangeEmployeeResignationInfo(resignation));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                leave.LeaveStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        #endregion

        #region تدفق عمل طلب مهمة
        public static void SaveMissionRequestWorkflow(int workflowId, HourlyMission mission, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
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
            var destinationEntityId = "HourlyMission";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HourlyMission);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
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

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        public static void SaveMissionRequestWorkflow(int workflowId, TravelMission mission, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
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
            var destinationEntityId = "TravelMission";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TravelMission);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
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

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        public static string SaveMissionRequestItem(int employeeId, int positionId, MissionRequestViewModel missionRequestItem)
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
                    StartTime =  missionRequestItem.FromTime??new DateTime(),
                    EndTime = missionRequestItem.ToTime ?? new DateTime(),
                };
                var defaultDate = new DateTime(2000, 1, 1);
                request.StartTime = defaultDate.Add(request.StartTime.TimeOfDay);
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
                var user = UserExtensions.CurrentUser;
                var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody) + " "
                            + employee.FullName;
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject) + " "
                            + employee.FullName;
                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "HourlyMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.HourlyMission + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
                return string.Empty;
            }
            else
            {
                var request = new TravelMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
                    FromDate =new DateTime(missionRequestItem.StartDate.Year, missionRequestItem.StartDate.Month, missionRequestItem.StartDate.Day) ,
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
                var user = UserExtensions.CurrentUser;
                var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalBody) + " "
                            + employee.FullName; ;
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequestApprovalSupject) + " "
                            + employee.FullName; ;
                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "TravelMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.TravelMission+" - " +missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
                return string.Empty;
            }
            
            
            
        }
        #endregion

        #region تدفق عمل سجل الدخول والخروج
        public static void SaveEntranceExitRecordRequestWorkflow(int workflowId, EntranceExitRecordRequest recordRequest, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequestApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequestApprovalSupject), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRequest";
            var destinationEntityId = "EntranceExitRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", recordRequest.Id);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                recordRequest.RecordStatus = Status.Approved;
                var entraceExitRecord = new EntranceExitRecord()
                {
                    Employee = recordRequest.Employee,
                    InsertSource = InsertSource.ByEmployee,
                    LogDate = recordRequest.RecordDate,
                    LogTime = recordRequest.RecordTime,
                    LogDateTime = recordRequest.LogDateTime,
                    LogType = recordRequest.LogType,
                    Note = recordRequest.Note
                };
                entities.Add(entraceExitRecord);
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                recordRequest.RecordStatus = Status.Rejected;
            }

            entities.Add(recordRequest);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
        public static string SaveEntranceExitRecordRequestItem(int employeeId, int positionId, EntranceExitRequestViewModel recordRequestItem)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EntranceExitRequestWorkflowName == null)
                return EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist;
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist;

            var request = new EntranceExitRecordRequest()
            {
                Creator = UserExtensions.CurrentUser,
                Employee = employee,
                LogType = recordRequestItem.LogType,
                Note = recordRequestItem.Note,
                RecordDate = recordRequestItem.RecordDate,
                RecordStatus = Status.Draft,
                RecordTime = recordRequestItem.RecordTime
            };
            DateTime logDateTime = new DateTime(recordRequestItem.RecordDate.Year, recordRequestItem.RecordDate.Month, recordRequestItem.RecordDate.Day, recordRequestItem.RecordTime.Hour, recordRequestItem.RecordTime.Minute, recordRequestItem.RecordTime.Second);
            request.LogDateTime = logDateTime;
            //اختبار تكرار الطلب
            if (ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                .Where(x => x.Employee.Id == request.Employee.Id && x.LogType == request.LogType
                && (x.LogDateTime.Year == request.LogDateTime.Year
                && x.LogDateTime.Month == request.LogDateTime.Month
                && x.LogDateTime.Day == request.LogDateTime.Day
                && x.LogDateTime.Hour == request.LogDateTime.Hour
                && x.LogDateTime.Minute == request.LogDateTime.Minute) && x.RecordStatus != Status.Rejected).Any())
            {
                return EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist;
            }
            //اختبار تكرار السجل
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            if (AttendanceSystem.Services.AttendanceService.CheckEntranceExitRecordDuplicate(employeeAttendanceCard.Employee, request.LogDateTime, InsertSource.ByEmployee, request.LogType, 0))
            {
                return EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist;
            }
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
            }
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequestApprovalBody) + " "
                            + employee.FullName;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequestApprovalSupject) + " "
                            + employee.FullName;
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRequest";
            var destinationEntityId = "EntranceExitRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.EntranceExitRequestWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                employee.User().Position(), WorkflowType.EmployeeEntranceExitRecordRequest, (recordRequestItem.LogType == LogType.Entrance ? 
                    EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Entrance) 
                    : EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Exit)) + " - " + recordRequestItem.Note, out notify);
            request.WorkflowItem = workflowItem;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return string.Empty;
        }
        public static List<EntranceExitRequestViewModel> GetEmployeeEntranceExitRequestApproval()
        {
            var result = new List<EntranceExitRequestViewModel>();
            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeEntranceExitRequests =
                ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var record in employeeEntranceExitRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(record.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                    result.Add(new EntranceExitRequestViewModel()
                    {
                        EmployeeId = record.Employee.Id,
                        FullName = record.Employee.FullName,
                        PositionId = record.Employee.PrimaryPosition().Id,
                        PositionName = record.Employee.PrimaryPosition().NameForDropdown,
                        RecordId = record.Id,
                        LogType = record.LogType,
                        Note = record.Note,
                        RecordDate = record.RecordDate,
                        RecordTime = record.RecordTime,
                        WorkflowItemId = record.WorkflowItem.Id,
                        PendingType = pendingType
                    });
            }

            return result;
        }
        #endregion

        public static Notify AddTerminationDecisionNotify(EmployeeDisciplinary employeeDisciplinary, EmployeeCard employeeCard)
        {
            employeeDisciplinary.DisciplinaryStatus = Status.Approved;
            if ((employeeDisciplinary.DisciplinarySetting.DisciplinaryNumber != 0) && (employeeCard.EmployeeDisciplinarys.Count(x => x.WorkflowItem == null || x.WorkflowItem.Status == WorkflowStatus.Completed) > employeeDisciplinary.DisciplinarySetting.DisciplinaryNumber))
            {
                var notify = new Notify()
                {
                    Sender = UserExtensions.CurrentUser,
                    Body = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.DisciplinaryNotifyBody, employeeCard.Employee.FullName),
                    Subject = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor, employeeCard.Employee.FullName),
                    Type = NotificationType.Information
                };
                notify.AddNotifyReceiver(new NotifyReceiver()
                {
                    Date = DateTime.Now,
                    Receiver = employeeCard.Employee.Manager().User()
                });
                return notify;
            }
            else
            {
                var newSalary = GetNewSalary(employeeDisciplinary.DisciplinarySetting.IsPercentage, employeeCard.Salary, employeeDisciplinary.DisciplinarySetting.Percentage, employeeDisciplinary.DisciplinarySetting.FixedValue);//الراتب الذي يجب خصمه من البطاقة الشهرية todo
            }
            return null;
        }
        public static List<IAggregateRoot> ChangeEmployeeTerminationInfo(EmployeeTermination termination, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            employeeCard.CardStatus = EmployeeCardStatus.Terminated;
            employeeCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.EndWorkingDate = termination.LastWorkingDate;
            termination.TerminationStatus = Status.Approved;
            foreach (var item in employeeCard.EmployeeCustodies)
            {
                item.CustodyEndDate = DateTime.Now;
            }

            var aeps = employeeCard.Employee.Positions;
            foreach (var aep in aeps.ToList())
            {
                aep.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                aep.Position.JobDescription.JobTitle.Vacancies++;

                var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == aep.Position.AssigningEmployeeToPosition);
                aep.Position.AssigningEmployeeToPosition = null;

                if (assignment != null)
                    assignment.AssigningEmployeeToPosition = null;

                aep.Position.Save();
                aep.Position = null;
                aeps.Remove(aep);
                aep.Save();
            }
            //while (termination.EmployeeCard.Employee.Positions.Any())
            //{
            //    termination.EmployeeCard.Employee.Positions.First().Position.AssigningEmployeeToPosition = null;
            //    entities.Add(termination.EmployeeCard.Employee.Positions.First().Position);
            //    termination.EmployeeCard.Employee.Positions.Remove(termination.EmployeeCard.Employee.Positions.First());
            //}

            entities.Add(employeeCard.Employee);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeePromotionInfo(EmployeePromotion promotion)
        {
            var entities = new List<IAggregateRoot>();

            var primaryPosition = promotion.EmployeeCard.Employee.Positions.SingleOrDefault(x => x.IsPrimary);

            promotion.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            promotion.Position.JobDescription.JobTitle.Vacancies--;

            if (primaryPosition != null)
            {
                primaryPosition.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                primaryPosition.Position.JobDescription.JobTitle.Vacancies++;
            }

            var ep = primaryPosition.Position.AssigningEmployeeToPosition;
            primaryPosition.Position.AssigningEmployeeToPosition = null;
            ep.Position = promotion.Position;
            promotion.Position.AssigningEmployeeToPosition = ep;
            primaryPosition.CreationDate = DateTime.Now;
            promotion.PromotionStatus = Status.Approved;

            entities.Add(ep);
            entities.Add(promotion.EmployeeCard.Employee);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeResignationInfo(EmployeeResignation resignation, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();

            resignation.ResignationStatus = Status.Approved;

            foreach (var item in employeeCard.EmployeeCustodies)
            {
                item.CustodyEndDate = DateTime.Now;
            }

            employeeCard.CardStatus = EmployeeCardStatus.Resigned;
            employeeCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.EndWorkingDate = resignation.LastWorkingDate;

            var aeps = resignation.EmployeeCard.Employee.Positions;
            foreach (var aep in aeps.ToList())
            {
                aep.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                aep.Position.JobDescription.JobTitle.Vacancies++;

                var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == aep.Position.AssigningEmployeeToPosition);
                aep.Position.AssigningEmployeeToPosition = null;

                if (assignment != null)
                    assignment.AssigningEmployeeToPosition = null;
                aep.Position.Save();
                aep.Position = null;
                aeps.Remove(aep);
                aep.Save();
            }

            entities.Add(employeeCard.Employee);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeFinancialPromotionInfo(FinancialPromotion financialPromotion)
        {
            var entities = new List<IAggregateRoot>();

            financialPromotion.FinancialPromotionStatus = Status.Approved;
            financialPromotion.EmployeeCard.Salary = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.Salary,
                financialPromotion.Percentage, financialPromotion.FixedValue);

            entities.Add(financialPromotion.EmployeeCard);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeRewardInfo(EmployeeReward employeeReward, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            var setting = employeeReward.RewardSetting;
            employeeReward.RewardStatus = Status.Approved;
            var newSalary = GetNewSalary(setting.IsPercentage, employeeCard.Salary, setting.Percentage, setting.FixedValue);//الراتب الذي يجب اضافته إلى البطاقة الشهرية todo
            entities.Add(employeeReward.EmployeeCard);

            return entities;
        }
        public static float GetNewSalary(bool isPercentage, float salary, float percentageValue, float fixedValue)
        {
            return isPercentage ? salary + ((salary * percentageValue) / 100) : salary + fixedValue;
        }
        public static float GetOldSalary(bool isPercentage, float salary, float percentageValue, float fixedValue)
        {
            if (isPercentage)
            {
                return (salary * 100) / (100 + percentageValue);
            }
            else
            {
                return salary - fixedValue;
            }
        }

        public static void GetLeaveInfo(LeaveRequest leaveRequest, LeaveSetting leaveSetting, Employee employee)
        {

            if (leaveSetting.IsIndivisible)
            {
                var balance = LeaveService.GetBalance(leaveSetting, employee, false, DateTime.Today);
                var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, DateTime.Today.Year - 1);
                balance += recycledBalance;
                leaveRequest.SpentDays = balance;
                leaveRequest.EndDate = LeaveService.GetEndDate(leaveRequest.StartDate, balance, leaveSetting.IsContinuous, employee);
                leaveRequest.FromTime = null;
                leaveRequest.ToTime = null;
            }
            else
            {
                if (leaveRequest.IsHourlyLeave)
                {
                    var minutes = 0.00;
                    if (leaveRequest.FromTime > leaveRequest.ToTime)
                    {
                        var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                        var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                        var minutesbefore = (maxDay.TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                        var minutesafter = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                        minutes =Math.Round( minutesafter + minutesbefore, 0);

                    }
                    else
                    {
                        minutes = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                  
                    }
                    var spentDays =
                        Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                    leaveRequest.SpentDays = spentDays;
                }
                else
                {
                    leaveRequest.SpentDays = LeaveService.GetSpentDays(leaveRequest.StartDate, leaveRequest.EndDate,
                    leaveSetting.IsContinuous, employee);
                    leaveRequest.FromTime = null;
                    leaveRequest.ToTime = null;
                }
            }

            employee.EmployeeCard.AddLeaveRequest(leaveRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { leaveRequest.WorkflowItem, employee.EmployeeCard }, UserExtensions.CurrentUser);
        }




    }
}