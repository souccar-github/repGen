using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Training.Entities;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.AttendanceSystem });
            return View();
        }

        [HttpPost]
        public ActionResult FilterEmployeeToActiveEmployee(Employee Employee, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand).ToList().Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        
    }
}
