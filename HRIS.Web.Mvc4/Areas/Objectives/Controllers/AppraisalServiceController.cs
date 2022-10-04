
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Enums;
using  Project.Web.Mvc4.Areas.Objectives.Helper;
using  Project.Web.Mvc4.Areas.Objectives.Models;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class AppraisalServiceController : Controller
    {

        // GET: /Objective/ApprovalService/
        public ActionResult GetObjectiveForAppraisal()
        {
            return Json(new { Objectives = ObjectiveHelper.GetEmployeeObjectiveAppraisalViewModel() }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AcceptAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Accept, objective, note);
            return Json(true);
        }

        public ActionResult PendingAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Pending, objective, note);
            return Json(true);
        }
        public ActionResult RejectAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Reject, objective, note);
            return Json(true);
        }

        public void SaveAppraisalWorkflow(int workflowId, WorkflowStepStatus status, ObjectiveDataViewModel objectiveDataViewModel, string note)
        {
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalBody);
            var title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalTitle);
            var objective = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(objectiveDataViewModel.ObjectiveId);
            ObjectiveHelper.SaveAppraisal(objectiveDataViewModel, objective);
            var entities = new List<IAggregateRoot>() { objective, workflow };
            WorkflowStatus workflowStatus;
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.Objective;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.Objective);
            var destinationControllerName = "Objectives/Home";
            var destinationActionName = "AppraisalService";
            var destinationEntityId = "AppraisalService";
            var destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", objectiveDataViewModel.ObjectiveId);

            entities.Add(WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName,destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus));
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }


    }
}
