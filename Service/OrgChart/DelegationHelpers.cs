#region Using Statements

using System;
using System.Collections.Generic;
using HRIS.Domain.Services;

#endregion

namespace Service.OrgChart
{
    public class DelegationHelpers
    {
        /// <summary>
        /// Get the delegations for an employee in the appraisal period.
        /// </summary>
        /// <param name="employeeID"> The employee id to get appraisals for.</param>
        /// <param name="appraisalFrom">The start date of the appraisal period.</param>
        /// <param name="appraisalTo">The end date of the appraisal period.</param>
        /// <returns>returns the list of employee delegations</returns>
        public static IList<Delegation> GetAppraislableDelegations(int employeeID, DateTime appraisalFrom,
                                                                   DateTime appraisalTo)
        {
            //todo review this code
            //return (from delegation in Service.GetAll()
            //        where
            //            delegation.From >= appraisalFrom && delegation.To <= appraisalTo && delegation.Appraisable &&
            //            delegation.Employee.Id == employeeID
            //        select delegation).ToList();
            return null;
        }
    }
}