#region

using System.Linq;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.OrgChart.ValueObjects.AssignedGrade;
using HRIS.Domain.Personnel.Entities;

#endregion

namespace Service.OrgChart
{
    public class PositionHelpers //: EmployeeVsPositionHelpers
    {
        public static IQueryable<Employee> GetEmployees(int positionId)
        {
            var positionService = new EntityService<Position>();
            IQueryable<Employee> employees = from position in positionService.GetAll()
                                             from positionFulfillment in position.PositionFulfillments
                                             where positionFulfillment.Position.Id == position.Id
                                                   && position.Id == positionId
                                                   && positionFulfillment.ExpireDate == null
                                             //  && positionFulfillment.IsPrimary 
                                             select positionFulfillment.Employee;

            return employees;
        }

        public static Employee GetCurrentEmployee(int positionId)
        {
            return GetEmployees(positionId).FirstOrDefault();
        }

        public static Employee GetReportingToEmployee(Position position)
        {
            if (position.ParentPosition != null)
                return GetCurrentEmployee(position.ParentPosition.Id);
            return null;
        }

        public static Position ClonePosition(Position position, string code)
        {
            var newPosition = new Position
                                  {
                                      //General
                                      Code = code,
                                      Status = position.Status,
                                      Type = position.Type,
                                      Level = position.Level,
                                      DisabilityStatus = position.DisabilityStatus,
                                      //Salary
                                      WorkingHours = position.WorkingHours,
                                      Per = position.Per,
                                      Budget = position.Budget,
                                      CostCenter = position.CostCenter,
                                      //Job
                                      JobTitle = position.JobTitle,
                                      JobDescription = position.JobDescription
                                  };

            PositionGrade positionGrade = position.ActiveGrade;

            if (positionGrade != null)
                newPosition.AddGrade(positionGrade);

            if (position.Node != null)
                newPosition.AddPositionNode(position.Node);

            //foreach (var positionObjective in position.Objectives)
            //{
            //    newPosition.AddObjective(positionObjective);
            //}

            //foreach (var positionSharedWith in position.SharedWiths)
            //{
            //    newPosition.AddSharedWith(positionSharedWith);
            //}

            var positionService = new EntityService<Position>();
            positionService.Add(newPosition);

            return newPosition;
        }

        public static PositionGrade GetCurrentGrade(Position position)
        {
            return position.ActiveGrade;
        }
    }
}