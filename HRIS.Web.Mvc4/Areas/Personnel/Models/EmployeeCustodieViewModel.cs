using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class EmployeeCustodieViewModel: ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeCustodieViewModel).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
       
        {
            var empcus = (EmployeeCustodie)entity;
            var empcard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (empcus.CustodyStartDate.CompareTo(empcard.StartWorkingDate.GetValueOrDefault()) <= 0)
            {
                var prop = typeof(EmployeeCustodie).GetProperty("CustodyStartDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.CustodyStartDateMustBeGreaterThanStartWorkingDate),
                    Property = prop
                });
            }

            if (empcus.CustodyStartDate.CompareTo(empcus.CustodyName.PurchaseDate) <= 0)
            {
                var prop = typeof(EmployeeCustodie).GetProperty("CustodyStartDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.CustodyStartDateMustBeGreaterThanPurchaseDate),
                    Property = prop
                });
            }
        }
    }
}