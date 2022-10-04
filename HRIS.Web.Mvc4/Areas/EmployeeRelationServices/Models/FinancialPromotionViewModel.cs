#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 18/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Enums;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.Resource;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class FinancialPromotionViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(FinancialPromotionViewModel).FullName;
            model.Views[0].EditHandler = "FinancialPromotionEditHandler";
            model.Views[0].ViewHandler = "FinancialPromotionViewHandler";
        }
       public override System.Web.Mvc.ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
       {
           var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
           if (employeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)

                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                    EmployeeRelationServicesLocalizationHelper.GetResource(
                        employeeCard.CardStatus == EmployeeCardStatus.New ?
                        EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                    EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)
                });
            else
               return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
       }
        

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {

            var financialPromotion = entity as FinancialPromotion;
            var currentUser = UserExtensions.CurrentUser;
            financialPromotion.Creator = currentUser;
            financialPromotion.CreationDate = DateTime.Now;
            financialPromotion.FinancialPromotionStatus=Status.Approved;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            //employeeCard.BasicSalary = (financialPromotion.IsPercentage != true)
            //    ? employeeCard.BasicSalary + financialPromotion.FixedValue
            //    : employeeCard.BasicSalary + ((employeeCard.BasicSalary*financialPromotion.Percentage)/100);
            financialPromotion.NewSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.Salary,
                financialPromotion.Percentage, financialPromotion.FixedValue);
            employeeCard.Salary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.Salary,
                financialPromotion.Percentage, financialPromotion.FixedValue);
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var financialPromotion = (FinancialPromotion)entity;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.Salary, (float)originalState["Percentage"], (float)originalState["FixedValue"]);

            employeeCard.Salary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                financialPromotion.Percentage, financialPromotion.FixedValue);

        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var financialPromotion = (FinancialPromotion)entity;
            employeeCard.Salary = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.Salary, financialPromotion.Percentage, financialPromotion.FixedValue);
        }


        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int FinancialPromotionId { get; set; }
        //public int PromotionSettingId { get; set; }
        //public string PromotionSetting { get; set; }
        public bool IsPercentage { get; set; }
        public float FixedValue { get; set; }
        public float Percentage { get; set; }
        public string FinancialPromotionReason { get; set; }
        public string Comment { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}