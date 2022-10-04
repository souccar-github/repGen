using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class MonthEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(MonthEventHandlers).FullName;
            model.Views[0].EditHandler = "Month_EditHandler";
            model.SchemaFields.SingleOrDefault(x => x.Name == "MonthStatus").Editable = false;
            model.IsEditable = false;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var Month = (Month)entity;
            var MonthNameExist= ServiceFactory.ORMService.All<Month>().Where(a => a.Name.Equals(Month.Name) &&  a.Id != Month.Id).FirstOrDefault();
            if (MonthNameExist != null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(CustomMessageKeysAttendanceSystemModule.TheMonthNameIsAlreadyExists)),
                    Property = null
                });
                return;
            }          
            

            //todo Mhd Alsadi: منع تعديل نوع الشهر بعد اضافته
            if (entity.IsTransient())
            {
                var month = (Month)entity;
                var recordCount = typeof(Month).GetAll<Month>().Count(x => month.MonthType == MonthType.SalaryAndBenefit && x.MonthType == MonthType.SalaryAndBenefit && x.MonthStatus != MonthStatus.Locked);
                if (recordCount > 0)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotCreateNewMonthWhileNotAllPreviousMonthsNotLocked)),
                        Property = null
                    });
                }
            }
        }

        
    }
}