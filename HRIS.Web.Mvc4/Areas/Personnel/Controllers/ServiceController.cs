using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Areas.Security.Helpers;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Personnel/Service/

        public ActionResult ActiveUserForEmployee(int id)
        {
            var emplyee = ServiceFactory.ORMService.GetById<Employee>(id);
            var result = UserHelper.ActiveUserForEmployee(emplyee, emplyee.Username, UserHelper.DefaultPassword);
            switch (result)
            {
                case CreateUserResult.Success:
                    return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
                    
                case CreateUserResult.UserExist:
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.AlreadyUserExist) }, JsonRequestBehavior.AllowGet);
                    
                case CreateUserResult.LimitNumber:
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.LimitNumberOfUser) }, JsonRequestBehavior.AllowGet);
                    
                default:
                    break;
            }
            return Json(new { Status = true, MessageTitle = GlobalResource.Fail, Message = GlobalResource.FailMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeactiveUserForEmployee(int id)
        {
            var emplyee = ServiceFactory.ORMService.GetById<Employee>(id);
            UserHelper.DeactiveUserForEmployee(emplyee, emplyee.Username, UserHelper.DefaultPassword);
            return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}
