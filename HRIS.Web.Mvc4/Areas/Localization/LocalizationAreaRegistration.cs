﻿using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Localization
{
    public class LocalizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Localization";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Localization_default",
                "Localization/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
