using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using  Project.Web.Mvc4.Areas.Objectives.Helper;
using  Project.Web.Mvc4.Areas.Objectives.Models;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Domain.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class ApprovalServiceController : Controller
    {
        #region Approval Objective


        public ActionResult GetObjectiveForApproval()
        {
            return Json(new { Objectives = ObjectiveHelper.GetEmployeeObjectiveApprovalViewModel() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AcceptApproval(int workflowId, int objectiveId, string note)
        {
            SaveWorkflow(workflowId, objectiveId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }

        public ActionResult RejectApproval(int workflowId, int objectiveId, string note)
        {
            SaveWorkflow(workflowId, objectiveId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }

        public ActionResult PendingApproval(int workflowId, int objectiveId, string note)
        {
            SaveWorkflow(workflowId, objectiveId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }

        public void SaveWorkflow(int workflowId, int objectiveId, WorkflowStepStatus status, string note)
        {
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalBody);
            var title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalTitle);
            WorkflowStatus workflowStatus;
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.Objective;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.Objective);
            var destinationControllerName = "Objectives/Home";
            var destinationActionName = "ApprovalService";
            var destinationEntityId = "ApprovalService";
            var destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.ApprovalService);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId",objectiveId);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData,
                out workflowStatus);
            var obj = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(objectiveId);

            if (workflowStatus == WorkflowStatus.Completed)
            {
                obj.Status = ObjectiveStatus.Approved;
            }
            if (workflowStatus == WorkflowStatus.Canceled)
            {
                obj.Status = ObjectiveStatus.Canceled;
            }
            var entities = new List<IAggregateRoot>() { workflow, obj, notify };
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);

        }
        #endregion


    }
}
