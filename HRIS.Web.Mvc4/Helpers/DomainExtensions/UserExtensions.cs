using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Core;
using HRIS.Domain.JobDescription.Entities;

namespace Project.Web.Mvc4.Helpers.DomainExtensions { 

    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
public static class UserExtensions
    {
        public static Employee Employee(this User user)
        {
            return EmployeeExtensions.GetEmployeeByUsername(user.Username);
        }



        public static string FullName()
        {
            return EmployeeExtensions.CurrentEmployee == null ? null : EmployeeExtensions.CurrentEmployee.FullName;

        }

        public static string PhotoId()
        {
            return EmployeeExtensions.CurrentEmployee == null ? null : EmployeeExtensions.CurrentEmployee.PhotoId;

        }

        public static Position Position(this User user)
        {
            return user.Employee().PrimaryPosition();
        }
        public static User GetUserByUsername(string username)
        {
            return ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(username));
        }

        public static User GetManager(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            var manager= emp.Manager();
            if (manager == null)
                return null;
            return manager.User();
        }

        public static Employee GetManagerAsEmployee(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            return emp.Manager();
        }

        public static Position GetManagerAsPosition(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            return emp.ManagerAsPosition();
        }

        /// <summary>
        /// Return  The used in this session
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current!=null && !HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;
                return ServiceFactory.ORMService.All<User>().SingleOrDefault(x=>x.Username== HttpContext.Current.User.Identity.Name);
            }

        }

        public static string CurrentUserTheming
        {
            get
            {
                if (CurrentUser!=null)
                    return CurrentUser.ThemingType.ToString().ToLower();
                var httpContext = new HttpContextWrapper(HttpContext.Current);

                var myCookie = httpContext.Request.Cookies["Theming"];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    return myCookie.Value;
                }
                return ThemingHelper.DefaultTheme.ToString().ToLower();
            }

        }
        public static ThemingType CurrentUserThemingType
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.ThemingType;
               
                return ThemingHelper.DefaultTheme;
            }

        }
        
    }
}