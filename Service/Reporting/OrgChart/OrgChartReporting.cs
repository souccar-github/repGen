using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HRIS.Domain.Personnel.Entities;

namespace Service.Reporting.OrgChart
{
    public class OrgChartReporting : IOrgChartReporting
    {
        #region IOrgChartReporting Members

        public IList<Employee> GetEmployeesWithoutAssignedGradesBenefits()
        {
            var empService = new EntityService<Employee>();
            var employees = (from employee in empService.GetAll()
                             select employee);
            return employees.ToList();
        }

        public IQueryable<dynamic> GetEmployeesWithoutAssignedPositions(Expression<Func<dynamic, bool>> query)
        {
            throw new Exception("");
        }

        #endregion
    }
}