using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.RootEntities;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class EmployeeLoanViewModel : ViewModel
    {
        //
        // GET: /PayrollSystem/EmployeeLoan/
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeLoanViewModel).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation,
            Souccar.Domain.DomainModel.Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var employeeLoan = (EmployeeLoan)entity;
            var sameLoanNumbers = employee.EmployeeCard.EmployeeLoans.Where(x => x.LoanNumber == employeeLoan.LoanNumber && x.Id != employeeLoan.Id).ToList();

            if (sameLoanNumbers.Count > 0)
            {
                var loanNumberProperty = typeof(EmployeeLoan).GetProperty("LoanNumber");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", PayrollSystemLocalizationHelper.GetResource(
                                    PayrollSystemLocalizationHelper.YouCannotDuplicateLoanNumber)),
                    Property = loanNumberProperty
                });
            }

            //var loanInSameDuration = employee.EmployeeCard.EmployeeLoans.Where(x=>x.RemainingAmountOfLoan>0).Sum(x => x.MonthlyInstalmentValue);
            //if (employeeLoan.MonthlyInstalmentValue > employee.EmployeeCard.Salary)
            //{
            //    var monthlyInstalmentValue = typeof(EmployeeLoan).GetProperty("MonthlyInstalmentValue");
            //    validationResults.Add(new ValidationResult()
            //    {
            //        Message = string.Format("{0} {1}", "", PayrollSystemLocalizationHelper.GetResource(
            //                        PayrollSystemLocalizationHelper.YourSalaryLessThanMonthlyInstalmentValue)),
            //        Property = monthlyInstalmentValue
            //    });
            
            
            //}

        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeLoan = (EmployeeLoan)entity;
            var employeeLoans = ServiceFactory.ORMService.All<LoanPayment>()
               .Where(x => x.MonthlyCard.Month.MonthStatus == MonthStatus.Locked && x.EmployeeLoan == employeeLoan).ToList();

            if (employeeLoans.Count() != 0)
            {
               PreventDefault = true;
            }
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            var primaryCard = (EmployeeCard)ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            foreach (var loan in primaryCard.EmployeeLoans)
            {
                loan.RemainingAmountOfLoan = (loan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).HasValue ? (loan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).Value : 0;
                ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { loan }, Helpers.DomainExtensions.UserExtensions.CurrentUser);
            }
        }

    }
}