using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Extensions;
using Souccar.Core.Extensions;
using System.Web;
using WebMatrix.WebData;
using System.Web.Security;
using Souccar.Domain.Security;
using Castle.Core.Internal;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Audit;
using Souccar.Domain.Notification;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.Security.Helpers
{
    public class UserHelper
    {
        public const string DefaultPassword = "123456";
        public const string DefaultUser = "admin";
        public const int MaxNumberOfUser=1000;
        public static int NumberOfUser{ get{return ServiceFactory.ORMService.All<User>().Count(x=>x.IsEnabled);}}
        public static CreateUserResult ActiveUserForEmployee(Employee employee, string username, string password)
        {
           
            if (NumberOfUser >=UserHelper.MaxNumberOfUser)
                return CreateUserResult.LimitNumber;
            var user = employee.User();
            if (user != null)
            {
                user.IsEnabled = true;
                user.Save();
                return CreateUserResult.Success;
            }
            CreateUser(employee, employee.Username, DefaultPassword, true, true);
            return CreateUserResult.Success;
        }
        public static void DeactiveUserForEmployee(Employee employee, string username, string password)
        {
            var user = employee.User();
            if (user != null)
            {
                user.IsEnabled = false;
                user.Save();
                return;
            }
            CreateUser(employee, employee.Username, DefaultPassword, false, false);
            return;
        }
        public static User GetUserByUsername(string username)
        {
            return ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(username));
        }
        public static void CreateUser(Employee newEmp, string username, string password, bool isApproved, bool isEnabled)
        {
            var employeeRole = "Employee";

            var notExist = true;
            while (notExist)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(username, password);
                    notExist = !WebSecurity.UserExists(username);
                }
                catch (Exception)
                {
                    notExist = !WebSecurity.UserExists(username);
                }
            }

            if (!Roles.GetAllRoles().Any(x => x.Equals(employeeRole)))
                Roles.CreateRole(employeeRole);
            if (!Roles.IsUserInRole(username, employeeRole))
                Roles.AddUserToRole(username, employeeRole);

            var newUser = ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(username));
            newUser.MobilePhone = newEmp.Mobile;
            newUser.FullName = newEmp.FullName;
            newUser.Comment = "";
            newUser.Email = newEmp.Email;
            newUser.IsEnabled = isEnabled;
            newUser.IsLockedOut = true;
            newUser.ThemingType = ThemingHelper.DefaultTheme;
            newUser.SaveWithoutValidation();
        }
        public static void UpdateUser(Employee newEmp, string username, string password)
        {
            var employeeRole = "Employee";

            var notExist = !WebSecurity.UserExists(username);
            while (notExist)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(username, password);
                    notExist = !WebSecurity.UserExists(username);
                }
                catch (Exception)
                {
                    notExist = !WebSecurity.UserExists(username);
                }
            }

            if (!Roles.GetAllRoles().Any(x => x.Equals(employeeRole)))
                Roles.CreateRole(employeeRole);
            if (!Roles.IsUserInRole(username, employeeRole))
                Roles.AddUserToRole(username, employeeRole);

            var newUser = ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(username));
            newUser.MobilePhone = newEmp.Mobile;
            newUser.FullName = newEmp.FullName;

            newUser.Email = newEmp.Email;
            newUser.IsLockedOut = true;
            newUser.Username = username;
            newUser.SaveWithoutValidation();
        }
        public static void CreateUsersForAllEmployees()
        {
            var employees = ServiceFactory.ORMService.All<Employee>();
            foreach (var emp in employees)
            {
                UpdateUser(emp, emp.Username, DefaultPassword);
            }
        }
        public static void DeleteAllUser()
        {
            var users = ServiceFactory.ORMService.All<User>();
            foreach (var u in users)
            {
                u.DeleteWithoutValidation();
            }
        }


        public static void DeleteUser(Employee newEmp, string username)
        {
            User user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == username);

            if (user != null)
            {


                user.IsEnabled = false;
                user.IsVertualDeleted = true;
                user.Save();
            }
        }
        public static void CheckDefaultUser()
        {
            if (!ServiceFactory.ORMService.All<User>().Any(x => x.Username.Equals(DefaultUser)))
            {
                var role = ServiceFactory.ORMService.All<Role>().FirstOrDefault(x => x.Name.Equals(DefaultUser)) != null
                    ? ServiceFactory.ORMService.All<Role>().FirstOrDefault(x => x.Name.Equals(DefaultUser))
                    : new Role()
                    {
                        Description = DefaultUser,
                        Name = DefaultUser
                    };
                var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username.Equals(DefaultUser)) !=
                           null
                    ? ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username.Equals(DefaultUser))
                    : new User()
                    {
                        Username = DefaultUser,
                        FullName = DefaultUser,
                        IsEnabled = true
                    };
                user.AddRole(role);
                var element = new AuthorizableElementRole()
                {
                    Role = role,
                    AuthorizableElementType = AuthorizableElementType.Service,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = "Security",
                    AuthorizableElementId = "ManageRole"
                };
                var element2 = new AuthorizableElementRole()
                {
                    Role = role,
                    AuthorizableElementType = AuthorizableElementType.Module,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = "Security",
                    AuthorizableElementId = "Security"
                };

                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(DefaultUser);
                //ServiceFactory.ORMService.All<AuthorizableElementRole>().Where(x => x.Role.Name.Equals(DefaultUser))
                //    .ForEach(x => x.DeleteWithoutValidation());
                ServiceFactory.ORMService.All<UserRole>().Where(x => x.User.Username.Equals(DefaultUser))
                    .ForEach(x => x.DeleteWithoutValidation());
                //ServiceFactory.ORMService.All<Role>().Where(x => x.Name.Equals(DefaultUser))
                //    .ForEach(x => x.DeleteWithoutValidation());
                ServiceFactory.ORMService.AllWithVertualDeleted<Log>().Where(x => x.User.Username.Equals(DefaultUser))
                    .ForEach(x => x.DeleteWithoutValidation());
                ServiceFactory.ORMService.AllWithVertualDeleted<Notify>().Where(x => x.Sender.Username.Equals(DefaultUser))
                    .ForEach(x =>
                    {
                        x.Receivers.Clear();
                        x.Save();
                        x.DeleteWithoutValidation();
                    });
                ServiceFactory.ORMService.AllWithVertualDeleted<User>().Where(x => x.Username.Equals(DefaultUser))
                    .ForEach(x => x.DeleteWithoutValidation());
                try
                {
                    ServiceFactory.ORMService.SaveTransaction(
                        new List<IAggregateRoot> { element2, element, role, user },
                        null);
                    WebSecurity.CreateAccount(DefaultUser, DefaultPassword);
                }
                catch (System.Exception e)
                {
                    ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(DefaultUser);
                    //ServiceFactory.ORMService.All<AuthorizableElementRole>().Where(x => x.Role.Name.Equals(DefaultUser))
                    //    .ForEach(x => x.DeleteWithoutValidation());
                    ServiceFactory.ORMService.All<UserRole>().Where(x => x.User.Username.Equals(DefaultUser))
                        .ForEach(x => x.DeleteWithoutValidation());
                    //ServiceFactory.ORMService.All<Role>().Where(x => x.Name.Equals(DefaultUser))
                    //    .ForEach(x => x.DeleteWithoutValidation());
                    ServiceFactory.ORMService.AllWithVertualDeleted<Log>().Where(x => x.User.Username.Equals(DefaultUser))
                        .ForEach(x => x.DeleteWithoutValidation());
                    ServiceFactory.ORMService.AllWithVertualDeleted<Notify>().Where(x => x.Sender.Username.Equals(DefaultUser))
                        .ForEach(x =>
                        {
                            x.Receivers.Clear();
                            x.Save();
                            x.DeleteWithoutValidation();
                        });
                    ServiceFactory.ORMService.AllWithVertualDeleted<User>().Where(x => x.Username.Equals(DefaultUser))
                        .ForEach(x => x.DeleteWithoutValidation());
                }
            }
        }
    }
    public enum CreateUserResult
    {
        Success,
        UserExist,
        LimitNumber
    }
}