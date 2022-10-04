using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.AttendanceSystem.Configurations;
using System.Windows.Forms;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{//todo : Mhd Update changeset no.1
    public class AttendanceRecordController : Controller
    {
        [HttpPost]
        public ActionResult AttendanceRecordOperation(int attendanceRecordId, string operation)
        {
            var message = String.Empty;
            var messageInfo = String.Empty;
            var isSuccess = false;
            var error = false;
            var attendanceRecord = (AttendanceRecord)typeof(AttendanceRecord).GetById(attendanceRecordId);
            List<EntranceExitRecord> entranceExitRecordNotAccepted = new List<EntranceExitRecord>();
            List<EntranceExitRecord> entranceExitRecordNotPairOfRecords = new List<EntranceExitRecord>();

            try
            {
                switch (operation)
                {
                    case "Calculate":
                        {
                            if (attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked ||
                                attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Created)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotCalculateLockedOrCreatedAttendanceRecords));
                                error = true;
                                break;
                            }

                            //entranceExitRecordNotAccepted = AttendanceService.CheckEntranceExitStatus(attendanceRecord);
                            //if (entranceExitRecordNotAccepted.Count > 0)
                            //{                               
                            //    message = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordStatusForThisMonth));
                            //    break;
                            //}
                            //entranceExitRecordNotPairOfRecords = AttendanceService.CheckAttendanceRecordPairOfRecords(attendanceRecord);
                            //if (entranceExitRecordNotPairOfRecords.Count > 0)
                            //{
                            //    foreach (var record in entranceExitRecordNotPairOfRecords)
                            //    {
                            //        record.ErrorMessage = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.MustBePairOfRecords));
                            //        record.Save();

                            //    }
                            //    message = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordsMustBePairOfRecords));
                            //    break;
                            //}
                            if (!AttendanceService.CheckEntranceExitRecordsConsistency(attendanceRecord))
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordsConsistencyFailed));
                                error = true;
                                break;
                            }

                            AttendanceService.CalculateAttendanceRecord(attendanceRecord);
                            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Calculated;
                            attendanceRecord.Save();                   
                            break;
                        }
                    case "Lock":
                        {
                            if (attendanceRecord.AttendanceMonthStatus != AttendanceMonthStatus.Calculated)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.OnlyCalculatedAttendanceRecordCanBeLocked));
                                break;
                            }
                            AttendanceService.LockAttendanceRecord(attendanceRecord);
                            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Locked;
                            attendanceRecord.Save();
                            break;
                        }
                }

                attendanceRecord.Save();
                if (String.IsNullOrEmpty(message) && !error)
                {
                    isSuccess = true;
                    message = Helpers.GlobalResource.DoneMessage;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        [HttpPost]
        public ActionResult GetEmployeeAttendanceCardGridModel()
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmployeeCard), null);
            gridModel.Views[0].ReadUrl = "AttendanceSystem/AttendanceRecord/ReadEmployeeAttendanceCardData";
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    ClassName = "grid-action-button EmployeeAttendanceCardGenerator",
                    Handler = "GenerateFilteredEmployeeAttendanceCards",
                    ImageClass = "",
                    Text =ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.GenerateTitle)),
                    Name = "GenerateFilteredEmployeeAttendanceCards"
                }
            };

            gridModel.ActionList.Commands.RemoveAt(0);
            gridModel.ActionList.Commands.RemoveAt(1);
            //gridModel.ToolbarCommands.RemoveAt(0);


            var displayColumnsList = new List<string> { "Employee", "StartWorkingDate", "ContractType", "EmployeeType", "EmployeeMachineCode", "AttendanceForm", 
                "LatenessForm", "AbsenceForm", "OvertimeForm", "LeaveTemplateMaster" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckGeneralSettings()
        {
            var message = String.Empty;
            var isSuccess = false;

            if (!ServiceFactory.ORMService.All<GeneralSettings>().Any())
            {
                message = ServiceFactory.LocalizationService
                                 .GetResource(CustomMessageKeysAttendanceSystemModule
                                 .GetFullKey(CustomMessageKeysAttendanceSystemModule.MustAddGeneralSettings));
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        [HttpPost]
        public ActionResult ReadEmployeeAttendanceCardData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var entityType = typeof(EmployeeCard);
            CrudController.UpdateFilter(filter, entityType);

            if (filter == null)
            {
                filter = new GridFilter();
                filter.Logic = "and";
            }
            if (filter.Filters == null)
            {
                filter.Filters = new List<GridFilter>().AsEnumerable();
                filter.Logic = "and";
            }
            var temp = filter.Filters.ToList();
            if (temp.Count == 0)
            {
                temp.Add(new GridFilter()
                {
                    Field = "IsVertualDeleted",
                    Operator = "eq",
                    Value = false
                });
            }
          
            temp.Add(new GridFilter()
            {
                Field = "CardStatus",
                Operator = "eq",
                Value = EmployeeCardStatus.OnHeadOfHisWork

            });
            temp.Add(new GridFilter()
            {
                Field = "AttendanceDemand",
                Operator = "eq",
                Value = true

            });
            filter.Filters = temp.AsEnumerable();
            IQueryable<IEntity> EmployeeCards = CrudController.GetAllWithVertualDeleted(entityType);


            var dataSourse = DataSourceResult.GetDataSourceResult(EmployeeCards , entityType, pageSize, skip, serverPaging, sort,filter);
            dataSourse.Data = (IQueryable<EmployeeCard>)dataSourse.Data;

            var data = entityType.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GenerateFilteredEmployeeAttendanceCards(int attendanceRecordId, GridFilter filter = null)
        {
            string message;
            var isSuccess = false;
            var attendanceRecord = (AttendanceRecord)typeof(AttendanceRecord).GetById(attendanceRecordId);
            AttendanceService.ResetNonAttendanceFormLastReset(attendanceRecord);//تصفير تاريخ أخر تصفير لنماذج نقص الدوام والتأخر
            if (attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotGenerateLockedAttendanceRecord));
            }
            else
            {
                GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
                var entityType = typeof(EmployeeCard);
                CrudController.UpdateFilter(filter, entityType);
                IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
                var filteredEmployeeAttendanceCards = DataSourceResult.GetDataSourceResult(queryable, entityType, 10, 0, false, null, filter);
                filteredEmployeeAttendanceCards.Data = ((IQueryable<EmployeeCard>)filteredEmployeeAttendanceCards.Data)
                    .Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand);
                var totalGeneratedCards = AttendanceService.GenerateAttendanceRecord(filteredEmployeeAttendanceCards.Data, attendanceRecord, generalSetting);
                attendanceRecord.Save();
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.AttendanceCardGenerated)) + "{" + totalGeneratedCards + "}";
                isSuccess = true;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }

    }
}
