using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class AttendanceRecordEventHandlers : ViewModel
    {


        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1, 
            //    Order = 1,
            //    HandlerName = "GenerateAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.GenerateTitle)),
            //    ShowCommand = true
            //});
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1,
            //    Order = 2,
            //    HandlerName = "CalculateAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.CalculateTitle)),
            //    ShowCommand = true
            //});
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1,
            //    Order = 2,
            //    HandlerName = "LockAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.LockTitle)),
            //    ShowCommand = true
            //});

            model.SchemaFields.SingleOrDefault(x => x.Name ==
                                                    typeof (AttendanceRecord).GetPropertyNameAsString<AttendanceRecord>(
                                                        y => y.AttendanceMonthStatus)).Editable = false;
            model.ViewModelTypeFullName = typeof (AttendanceRecordEventHandlers).FullName;
            //model.IsEditable = false;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
            string customInformation = null)
        {
            var attendanceRecord = (AttendanceRecord) entity;
            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Created;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var attendanceRecord = (AttendanceRecord) entity;
            attendanceRecord.Date = new DateTime(attendanceRecord.Date.Year, attendanceRecord.Date.Month, 1);


            var RecordNumberExist = typeof(AttendanceRecord).GetAll<AttendanceRecord>().Count(x => x.Id != attendanceRecord.Id && x.Number == attendanceRecord.Number);
            if (RecordNumberExist > 0)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource( CustomMessageKeysAttendanceSystemModule.GetFullKey(
                                CustomMessageKeysAttendanceSystemModule.AttendanceRecordNumberMustBeUnique)),
                    Property = null
                });
            }

            var recordCount =
                    typeof (AttendanceRecord).GetAll<AttendanceRecord>() .Count(x => x.Id != attendanceRecord.Id && x.Name == attendanceRecord.Name);
                if (recordCount > 0)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(
                                    CustomMessageKeysAttendanceSystemModule.AttendanceRecordNameMustBeUnique)),
                        Property = null
                    });
                }

                recordCount = typeof (AttendanceRecord).GetAll<AttendanceRecord>().Count( x => x.Date < attendanceRecord.Date && x.AttendanceMonthStatus != AttendanceMonthStatus.Locked);
                if (recordCount > 0)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource( CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule
                                        .CannotCreateAttendanceRecordWhileAllPreviousAttendanceRecordNotLocked)),
                        Property = null
                    });
                }
            
        }

    }
}