using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HRIS.Domain.Personnel.RootEntities;

namespace Service.Reporting.OrgChart
{
    public interface IOrgChartReporting
    {
        IList<Employee> GetEmployeesWithoutAssignedGradesBenefits();

        IQueryable<dynamic> GetEmployeesWithoutAssignedPositions(Expression<Func<dynamic, bool>> query);
    }
}