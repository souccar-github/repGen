using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.PayrollSystem.Services;
using  Project.Web.Mvc4.Controllers;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class SalaryIncreaseOrdinanceController : Controller
    {
        public ActionResult GetPrimaryCardGridModel()
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmployeeCard), null);
            var displayColumnsList = new List<string> { "Employee", "SalaryDeservableType", "StartWorkingDate", "ContractType", "EmployeeType", "Salary", "BenefitSalary", "TempSalary1", "TempSalary2" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();

            gridModel.Views[0].ReadUrl = "PayrollSystem/SalaryIncreaseOrdinance/ReadPrimaryCardData";
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand 
                {
                    Additional = false,
                    Name = "GenerateSalaryIncreaseEmployees",
                    ClassName = "grid-action-button SalaryIncreaseEmployeesGenerator",
                    Handler = "GenerateFilteredPrimaryCardsSalaryIncrease",
                    ImageClass = "",
                    Text =
                        ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.GenerateTitle))
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPrimaryCardData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var entityType = typeof(EmployeeCard);
            CrudController.UpdateFilter(filter, entityType);
           // IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
            IQueryable<IEntity> queryable = typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x=>x.SalaryDeservableType!=SalaryDeservableType.Nothing).ToList().AsQueryable();
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter);
            var data = entityType.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GenerateFilteredPrimaryCards(int salaryIncreaseOrdinanceId, GridFilter filter = null)
        {
            string message;
            var isSuccess = false;
            var salaryIncreaseOrdinance = (SalaryIncreaseOrdinance)typeof(SalaryIncreaseOrdinance).GetById(salaryIncreaseOrdinanceId);

            if (salaryIncreaseOrdinance.Status == Status.Accepted)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotGenerateAcceptedSalaryIncreaseOrdinance));
            }
            else
            {
                try
                {
                    var entityType = typeof(EmployeeCard);
                    CrudController.UpdateFilter(filter, entityType);
                    //IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
                    IQueryable<IEntity> queryable = typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x=>x.SalaryDeservableType!=SalaryDeservableType.Nothing).ToList().AsQueryable();
                    var filteredPrimaryCards = DataSourceResult.GetDataSourceResult(queryable, entityType, 10, 0, false, null, filter);
                    var totalGeneratedCards = SalaryIncreaseOrdinanceService.GenerateEmployees(filteredPrimaryCards.Data, salaryIncreaseOrdinance);
                    message = ServiceFactory.LocalizationService
                        .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.SalaryIncreaseOrdinanceGenerated)) + CustomMessageKeysPayrollSystemModule.TotalGeneratedCards + "{" + totalGeneratedCards + "}";
                    isSuccess = true;
                }
                catch (Exception)
                {
                    message = Helpers.GlobalResource.FailMessage;
                    isSuccess = false;
                }
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }


        [HttpPost]
        public ActionResult CalculateSalaryIncreaseOrdinance(int salaryIncreaseOrdinanceId)
        {
            string message;
            var isSuccess = false;
            var salaryIncreaseOrdinance = (SalaryIncreaseOrdinance)typeof(SalaryIncreaseOrdinance).GetById(salaryIncreaseOrdinanceId);

            if (salaryIncreaseOrdinance.Status == Status.Accepted)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotCalculateAcceptedSalaryIncreaseOrdinance));
            }
            else
            {
                try
                {
                    SalaryIncreaseOrdinanceService.CalculateIncreasedValues(salaryIncreaseOrdinance);
                    message = Helpers.GlobalResource.DoneMessage;
                    isSuccess = true;
                }
                catch (Exception)
                {
                    message = Helpers.GlobalResource.FailMessage;
                    isSuccess = false;
                }
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }

        [HttpPost]
        public ActionResult AcceptSalaryIncreaseOrdinance(int salaryIncreaseOrdinanceId)
        {
            string message;
            var isSuccess = false;
            var salaryIncreaseOrdinance = (SalaryIncreaseOrdinance)typeof(SalaryIncreaseOrdinance).GetById(salaryIncreaseOrdinanceId);

            if (salaryIncreaseOrdinance.SalaryIncreaseOrdinanceEmployees.Any(x => x.SalaryAfterIncrease <= 0))
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.YouHaveToCalculateSalaryIncreaseBeforeAcceptTheRecord));
            }
            else if (salaryIncreaseOrdinance.Status == Status.Accepted)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.OnlyDraftSalaryIncreaseOrdinanceCanBeAccepted));
            }
            else
            {
                try
                {
                    SalaryIncreaseOrdinanceService.AcceptIncreasedValues(salaryIncreaseOrdinance);
                    message = Helpers.GlobalResource.DoneMessage;
                    isSuccess = true;
                }
                catch (Exception)
                {
                    message = Helpers.GlobalResource.FailMessage;
                    isSuccess = false;
                }
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }
    }
}
