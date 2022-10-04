using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using  Project.Web.Mvc4.Controllers;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{//todo : Mhd Update changeset no.1
    public class EntranceExitRecordController : Controller
    {
        //[HttpPost]
        //public ActionResult AcceptEntranceExitRecords(GridFilter filter = null)
        //{
        //    const int skip = 0;
        //    const bool serverPaging = true;
        //    IEnumerable<GridSort> sort = null;

        //    var entityType = typeof(EntranceExitRecord);
        //    CrudController.UpdateFilter(filter, entityType);
        //    IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
        //    var pageSize = queryable.Count();
        //    var filteredEntranceExitRecords = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter);

        //    var entranceExitRecordsUpdated = AttendanceService.AcceptFilteredEntranceExitRecords(filteredEntranceExitRecords.Data);

        //    string message = "( " + entranceExitRecordsUpdated +" ) "+ ServiceFactory.LocalizationService
        //        .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.EntranceExitRecordsAccepted));

        //    return Json(new
        //    {
        //        Success = true,
        //        Msg = message,
        //    });
        //}

        [HttpPost]
        public ActionResult DeleteEntranceExitRecords(GridFilter filter = null)
        {
            string message;
            var isSuccess = false;
            int pageSize = 10;
            int skip = 0;
            bool serverPaging = true;
            IEnumerable<GridSort> sort = null;

            var entityType = typeof(EntranceExitRecord);
            CrudController.UpdateFilter(filter, entityType);
            IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
            pageSize = queryable.Count();
            var filteredEntranceExitRecords = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter);
            var withoutFilters = filter.Filters.Count() > 1 ? false : true;
            var entranceExitRecordsDeleted = AttendanceService.DeleteFilteredEntranceExitRecords(filteredEntranceExitRecords.Data, withoutFilters);

            message = "( " + entranceExitRecordsDeleted + " ) " + ServiceFactory.LocalizationService
                .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.EntranceExitRecordsDeleted));
            isSuccess = true;

            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }


        //[HttpPost]
        //public ActionResult GenerateEntranceExitRecordErrors(GridFilter filter = null)
        //{
        //    string message;
        //    var isSuccess = false;
        //    int pageSize = 10;
        //    int skip = 0;
        //    bool serverPaging = true;
        //    IEnumerable<GridSort> sort = null;

        //    var entityType = typeof(EntranceExitRecord);
        //    CrudController.UpdateFilter(filter, entityType);
        //    IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
        //    pageSize = queryable.Count();
        //    var filteredEntranceExitRecords = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter);

        //    var entranceExitRecordErrorsGenerated = AttendanceService.GetEntranceExitRecordErrors(filteredEntranceExitRecords.Data);

        //    message = "( " + entranceExitRecordErrorsGenerated + " ) " + ServiceFactory.LocalizationService
        //        .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.EntranceExitRecordErrorsGenerated));
        //    isSuccess = true;

        //    return Json(new
        //    {
        //        Success = isSuccess,
        //        Msg = message,
        //    });
        //}
    }
}
