﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Souccar.Web.Mvc;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Localization;

namespace Project.Web.Mvc4.Controllers
{
    public class SysController : Controller
    {
        //
        // GET: /Sys/

        public ActionResult Index()
        {
            return View();
        }

        public object DeserializeStringToJSON(Type type, string obj)
        {
            var serializer = new JavaScriptSerializer();
            return (RequestInformation)serializer.Deserialize(obj, type);
        }

        private void ChangeLanguage(int newLang)
        {
            // CurrentLocale.Language = (Locale) Enum.Parse(typeof (Locale), newLang, true);
            //var temp = newLang.Replace('_', '-');
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(temp);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(temp);
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(temp);
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(temp);
            Project.Web.Mvc4.Helpers.LocalizationHelper.SetActiveLanguage(newLang);
            if (Project.Web.Mvc4.Helpers.LocalizationHelper.IsRtl)
            {
                Session["LangDir"] = true.ToString();
                CurrentLocale.Language = Locale.Rtl;
            }
            else
            {
                Session["LangDir"] = false.ToString();
                CurrentLocale.Language = Locale.Ltr;
            }
            var language = ServiceFactory.ORMService.GetById<Language>(newLang);

            var temp = language.LanguageCulture.ToString();
            temp = temp.Replace("_", "-");
            GE.SetLanguageCookie(HttpContext, "userLanguage", temp);
            GE.GetCurrentCulture(HttpContext);
        }

        public ActionResult HrisLogin()
        {
            ViewBag.Login = true;
            return View();
        }

        public ActionResult ChangeLan(int lan, string requestInformation)
        {
            ChangeLanguage(lan);
            TempData["ChangeLanguage"] = true;
            TempData["requestInformation"] = requestInformation;
            return Redirect(Request.UrlReferrer.ToString());
        }

    }

}
