using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

using Souccar.Domain.Security;
using WebMatrix.WebData;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.DomainExtensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class EmployeeExtensions
    {
        /// <summary>
        /// Return  The used in this session
        /// </summary>
        public static Employee CurrentEmployee 
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;
                return GetEmployeeByUsername(HttpContext.Current.User.Identity.Name);
            }

        }


        public static  Position PrimaryPosition(this Employee employee)
        {
            if (employee == null)
                return null;
            var assigningEmployeeToPosition = employee.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    return assigningEmployeeToPosition.Position;
                return null;
        }

        public static Employee Manager(this Employee employee)
        {

            var position = employee.PrimaryPosition();
            if (position != null)
            {
                var manager = position.Manager;
                if (manager != null)
                    return manager.Employee;
            }
            return null;

        }
        public static User ManagerAsUser(this Employee employee)
        {
            var emp = employee.Manager();
            if (emp == null)
                return null;
            return emp.User();
        }
        public static Position ManagerAsPosition(this Employee employee)
        {
            var position = employee.PrimaryPosition();
            if (position == null)
                return null;
            return position.Manager;
        }


        public static Employee GetEmployeeByCode(string code)
        {
            return typeof(Employee).GetAll<Employee>().SingleOrDefault(x => x.Code.Equals(code));
        }
        public static Employee GetEmployeeByUsername(string id)
        {
            var newId = 0;
            if(int.TryParse(id,out newId))
                return ServiceFactory.ORMService.GetById<Employee>(newId);
            else
                return null;
        }
        public static Employee GetEmployeeByIdentificationNo(string identificationNo)
        {
            return typeof(Employee).GetAll<Employee>().SingleOrDefault(x => x.IdentificationNo.Equals(identificationNo));
        }

        public static User User(this Employee employee)
        {
            return ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(employee.Username));
        }


    }
}