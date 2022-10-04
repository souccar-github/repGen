﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Conventions;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Core.Extensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using WebMatrix.WebData;
using HRIS.Domain.Workflow;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Notification;
using Souccar.Domain.DomainModel;


namespace Project.Web.Mvc4.Helpers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class WorkflowHelper
    {
        public static bool IsFinish(WorkflowItem workflow)
        {
            if (workflow.Status == WorkflowStatus.Completed || workflow.Status == WorkflowStatus.Canceled)
                return true;
            var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
                              workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
            if (acceptCount < workflow.StepCount)
                return false;
            var approvalCount = workflow.Approvals.Count(x => x.Status == WorkflowStepStatus.Pending);
            return approvalCount == 0;
        }

        public static WorkflowItem InitWithSetting(WorkflowSetting workflowSetting, User target, string notifyTitle, string notifyBody,
           string destinationTabName, string destinationModuleName, string destinationLocalizationModuleName, string destinationControllerName,
              string destinationActionName, string destinationEntityId, string destinationEntityTitle, OperationType destinationEntityOperationType,
            IDictionary<string, int> destinationData, Position position, Souccar.Domain.Workflow.Enums.WorkflowType workflowType, string description,out Notify notify)
        {
            return InitWithSetting(workflowSetting, target, UserExtensions.CurrentUser,  notifyTitle,  notifyBody,
           destinationTabName,  destinationModuleName,  destinationLocalizationModuleName,  destinationControllerName,
              destinationActionName,  destinationEntityId,  destinationEntityTitle,  destinationEntityOperationType,
            destinationData, position, workflowType, description, out notify);
        }

        public static WorkflowItem InitWithSetting(WorkflowSetting workflowSetting, User target, User creator, string notifyTitle, string notifyBody,
           string destinationTabName, string destinationModuleName, string destinationLocalizationModuleName, string destinationControllerName,
              string destinationActionName, string destinationEntityId, string destinationEntityTitle, OperationType destinationEntityOperationType,
            IDictionary<string, int> destinationData, Position position, Souccar.Domain.Workflow.Enums.WorkflowType workflowType, string description, out Notify Notify)
        {
            Notify = new Notify();
            Notify notify;
            var result = new WorkflowItem();
            result.TargetUser = target;
            result.Date = DateTime.Today;
            result.Creator = creator;
            result.Description = description;
            result.Type = workflowType;
            foreach (var approvalPosition in workflowSetting.SettingApprovals)
            {
                result.AddApproval(new WorkflowApproval()
                {
                    Date = DateTime.Today,
                    Order = approvalPosition.Order,
                    Status = WorkflowStepStatus.Pending,
                    User = approvalPosition.Position.User(),
                });
            }
            var CountOfManagers = target.getCountOfManagers();
            var settingPosition = workflowSetting.SettingPositions.SingleOrDefault(x => x.Position == position);
            if (settingPosition == null)
            {
                result.StepCount = workflowSetting.InitStepCount;
            }
            else
            {
                result.StepCount = settingPosition.Count;
            }
            if (result.StepCount > CountOfManagers)
                result.StepCount = CountOfManagers;
            if (result.StepCount > 0)
                result.FirstUser = (target != null && position != null && position.Manager != null) ? position.Manager.User() : null;
            //========================================
            if (result.FirstUser != null || result.Approvals.Count() > 0)
            {
                notify = new Notify()
                {
                    Sender = null,
                    Body = string.Format(notifyBody),
                    Subject = string.Format(notifyTitle),
                    Type = NotificationType.Request,
                    DestinationTabName = destinationTabName,
                    DestinationModuleName = destinationModuleName,
                    DestinationLocalizationModuleName = destinationLocalizationModuleName,
                    DestinationControllerName = destinationControllerName,
                    DestinationActionName = destinationActionName,
                    DestinationEntityId = destinationEntityId,
                    DestinationEntityTitle = destinationEntityTitle,
                    DestinationEntityOperationType = destinationEntityOperationType,
                    DestinationData = destinationData
                };
                //=========================================
                if (result.FirstUser != null)
                {

                    notify.AddNotifyReceiver(new NotifyReceiver()
                    {
                        Date = DateTime.Now,
                        Receiver = position.Manager.User()
                    });
                }
                else
                {
                    if (result.Approvals.Count() > 0)
                    {
                        var Approval = result.Approvals.ElementAt(0);
                        notify.AddNotifyReceiver(new NotifyReceiver()
                        {
                            Date = DateTime.Now,
                            Receiver = Approval.User
                        });


                    }
                }

                result.CurrentUser = notify?.Receivers.FirstOrDefault()?.Receiver;
                Notify = notify;

            }

            result.Status = IsFinish(result) ? WorkflowStatus.Completed : WorkflowStatus.Pending;
            
            return result;
        }

        public static Position GetNextAppraiser(WorkflowItem workflow, out WorkflowPendingType pendingType)
        {
            switch (workflow.Status)
            {
                case WorkflowStatus.Completed:
                case WorkflowStatus.Canceled:
                    pendingType = WorkflowPendingType.NotPending;
                    return null;
                    break;
            }
            var user = workflow.FirstUser;
            var managers = getManagers(user, workflow.StepCount);
            if (workflow.StepCount > 0 && user != null)
            {
                if (workflow.Steps.Count == 0)
                {
                    pendingType = WorkflowPendingType.PendingStep;
                    return user.Employee().PrimaryPosition();
                }
                var lastStep = workflow.Steps.Last();
                switch (lastStep.Status)
                {
                    case WorkflowStepStatus.Pending:
                        pendingType = WorkflowPendingType.PendingStep;
                        return lastStep.User.Employee().PrimaryPosition();
                        break;
                    case WorkflowStepStatus.Reject:
                        pendingType = WorkflowPendingType.PendingStep;

                        var previousAppraisal = getPreviousManager(lastStep.User, managers);
                        if (previousAppraisal != null)
                        {
                            return previousAppraisal.Employee().PrimaryPosition();
                        }
                        return null;
                        break;
                    case WorkflowStepStatus.Accept:
                        var nextAppraisal = getNextManager(lastStep.User, managers);
                        if (nextAppraisal != null)
                        {
                            var nextPosition = nextAppraisal.Employee().PrimaryPosition();
                            if (nextPosition != null)
                            {
                                pendingType = WorkflowPendingType.PendingStep;
                                return nextPosition;
                            }

                        }
                        break;
                }
            }
            var nextApproval = getNextApproval(workflow);
            if (nextApproval != null)
            {
                pendingType = WorkflowPendingType.PendingApproval;
                return nextApproval.Employee().PrimaryPosition();
            }
            pendingType = WorkflowPendingType.NotPending;
            return null;
        }

        public static WorkflowPendingType GetPendingType(WorkflowItem workflow)
        {
            switch (workflow.Status)
            {
                case WorkflowStatus.Completed:
                case WorkflowStatus.Canceled:
                    return WorkflowPendingType.NotPending;
            }
            var user = workflow.FirstUser;
            var managers = getManagers(user, workflow.StepCount);
            if (workflow.StepCount <= 0)
                return workflow.Approvals.IsEmpty()
                    ? WorkflowPendingType.NotPending : WorkflowPendingType.PendingApproval;
            if (workflow.Steps.Count == 0)
                return WorkflowPendingType.NewStep;
            var lastStep = workflow.Steps.Last();
            switch (lastStep.Status)
            {
                case WorkflowStepStatus.Pending:
                    return WorkflowPendingType.PendingStep;
                    break;
                case WorkflowStepStatus.Reject:
                    return WorkflowPendingType.NewStep;
                    break;
                case WorkflowStepStatus.Accept:
                    var nextAppraisal = getNextManager(lastStep.User, managers);
                    if (nextAppraisal != null)
                    {
                        var nextPosition = nextAppraisal.Employee().PrimaryPosition();
                        if (nextPosition != null)
                            return WorkflowPendingType.NewStep;
                    }
                    break;
            }
            return workflow.Approvals.Where(x => x.Status == WorkflowStepStatus.Pending).IsEmpty() ? WorkflowPendingType.NotPending : WorkflowPendingType.PendingApproval;
        }

        public static Notify UpdateDefaultWorkflow(WorkflowItem workflow, string note, WorkflowStepStatus status, User user, string notifyTitle, string notifyBody,
           string destinationTabName, string destinationModuleName, string destinationLocalizationModuleName, string destinationControllerName,
              string destinationActionName, string destinationEntityId, string destinationEntityTitle, OperationType destinationEntityOperationType, 
            IDictionary<string, int> destinationData, out WorkflowStatus workflowStatus)
        {
            workflowStatus = workflow.Status;
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            switch (pendingType)
            {
                case WorkflowPendingType.NewStep:
                    var step = new WorkflowStep()
                    {
                        Description = note,
                        Date = DateTime.Now,
                        Status = status,
                        User = user
                    };
                    if (workflow.Steps.Count == 0)
                         step.Order = 1;
                    else
                        step.Order = workflow.Steps.LastOrDefault().Order + 1;
                    workflow.AddStep(step);

                    break;
                case WorkflowPendingType.PendingStep:
                    step = workflow.Steps.Where(x => x.User == user && x.Status == WorkflowStepStatus.Pending)
                        .OrderBy(x => x.Date)
                        .LastOrDefault();
                    step.Status = status;
                    step.Description = note;
                    step.Date = DateTime.Now;

                    break;
                case WorkflowPendingType.PendingApproval:
                    var approval = workflow.Approvals.SingleOrDefault(x => x.User == user);
                    approval.Status = status;
                    approval.IsSeen = true;
                    approval.Description = note;
                    approval.Date = DateTime.Now;
                    break;
            }
            Notify notify = null;
            var _notification = ServiceFactory.ORMService.All<Notify>().OrderByDescending(x => x.Date)
                .FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflow.Id);
            if (_notification != null){
               foreach (var receiver in _notification.Receivers)
                   receiver.IsRead = true;
               ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { _notification }, UserExtensions.CurrentUser);
            }
            if (status != WorkflowStepStatus.Pending)
            {
                var nextAppraisal = WorkflowHelper.GetNextAppraiser(workflow, out pendingType);
                if (status == WorkflowStepStatus.Accept && WorkflowHelper.IsFinish(workflow))
                {
                    workflowStatus = WorkflowStatus.Completed;
                    workflow.Status = workflowStatus;
                }
                if (status == WorkflowStepStatus.Reject && (nextAppraisal == null || user == nextAppraisal.User()))
                {
                    workflowStatus = WorkflowStatus.Canceled;
                    workflow.Status = workflowStatus;
                }
                if (nextAppraisal != null)
                {
                    workflow.CurrentUser = nextAppraisal.Employee?.User();
                    notify = new Notify()
                    {
                        Sender = user,
                        Body = string.Format(notifyBody, workflow.TargetUser.FullName),
                        Subject = string.Format(notifyTitle, workflow.TargetUser.FullName),
                        Type = NotificationType.Request,
                        DestinationTabName = destinationTabName,
                        DestinationModuleName = destinationModuleName,
                        DestinationLocalizationModuleName = destinationLocalizationModuleName,
                        DestinationControllerName = destinationControllerName,
                        DestinationActionName = destinationActionName,
                        DestinationEntityId = destinationEntityId,
                        DestinationEntityTitle = destinationEntityTitle,
                        DestinationEntityOperationType = destinationEntityOperationType,
                        DestinationData = destinationData
                    };
                    notify.AddNotifyReceiver(new NotifyReceiver()
                    {
                        Date = DateTime.Now,
                        Receiver = nextAppraisal.Employee.User()
                    });
                }
            }
            return notify;
        }


        private static User getNextApproval(WorkflowItem workflow)
        {
            if (workflow.Approvals.IsEmpty())
                return null;
            if (workflow.Approvals.Any(x => x.Status == WorkflowStepStatus.Reject))
                return null;
            var temp = workflow.Approvals.Where(x => x.Status == WorkflowStepStatus.Pending).OrderBy(x => x.Order).FirstOrDefault();
            return temp != null ? temp.User : null;
        }
        private static User getPreviousManager(User user, List<User> managers)
        {
            if (managers == null || managers.Count == 0)
                return null;
            return user == managers.First() ? null : managers[managers.IndexOf(user) - 1];
        }
        private static User getNextManager(User user, List<User> managers)
        {
            if (managers == null || managers.Count == 0)
                return null;
            return user == managers.Last() ? null : managers[managers.IndexOf(user) + 1];
        }
        private static List<User> getManagers(User user, int count)
        {
            var result = new List<User>();
            for (var i = 0; i < count; i++)
            {
                if (user != null)
                {
                    result.Add(user);
                    var manager = user.Employee().Manager();
                    user = manager != null ? manager.User() : null;
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        public static int getCountOfManagers(this User user)
        {
            var result = 0;
            if (user != null)
            {
                var manager = user.Employee().Manager();
                if (manager != null)
                {

                    var user_ = manager.User();
                    if (user_ != null)
                    {
                        result += user_.getCountOfManagers();
                    }
                    result++;
                    return result;
                }
                return result;
            }
            return result;
        }
        public static bool AuthorizedAddStatus(WorkflowItem workflow)
        {
            var emp = EmployeeExtensions.CurrentEmployee;
            if (workflow.Status == WorkflowStatus.Completed ||
                workflow.Status == WorkflowStatus.Canceled ||
                emp == null ||
                !WebSecurity.IsAuthenticated)
                return false;
            WorkflowPendingType pendingType;
            return emp.PrimaryPosition() == GetNextAppraiser(workflow, out pendingType);
        }

    }

    public enum WorkflowPendingType
    {
        NewStep,
        PendingStep,
        PendingApproval,
        NotPending
    }
}