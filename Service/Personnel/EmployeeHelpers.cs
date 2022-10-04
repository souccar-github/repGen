#region

using System;
using System.Linq;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Repository.UnitOfWork;

#endregion

namespace Service.Personnel
{
    public class EmployeeHelpers
    {
        public static Employee GetByLoginName(string loginName)
        {
            var employeeService = new EntityService<Employee>();
            if (loginName.Trim().Length > 0)
            {
                Employee employee = employeeService.GetAll().SingleOrDefault(w => w.LoginName == loginName);

                return employee;
            }

            return null;
        }

        public static IQueryable<PositionFulfillment> GetPositions(int employeeId)
        {
            var positionService = new EntityService<Position>();
            IQueryable<PositionFulfillment> positionFulfillments = from position in positionService.GetAll()
                                                                   from positionFulfillment in
                                                                       position.PositionFulfillments
                                                                   where positionFulfillment.Position.Id == position.Id
                                                                         &&
                                                                         positionFulfillment.Employee.Id == employeeId
                                                                         && positionFulfillment.ExpireDate == null
                                                                         &&
                                                                         positionFulfillment.Type !=
                                                                         PositionFulfillmentType.Delegated
                                                                   select positionFulfillment;

            return positionFulfillments;
        }

        public static IQueryable<PositionFulfillment> GetPositions(UnitOfWork unitOfWork, int employeeId)
        {
            var positionService = new EntityService<Position>(unitOfWork);
            IQueryable<PositionFulfillment> positionFulfillments = from position in positionService.GetAll()
                                                                   from positionFulfillment in
                                                                       position.PositionFulfillments
                                                                   where positionFulfillment.Position.Id == position.Id
                                                                         &&
                                                                         positionFulfillment.Employee.Id == employeeId
                                                                         && positionFulfillment.ExpireDate == null
                                                                         &&
                                                                         positionFulfillment.Type !=
                                                                         PositionFulfillmentType.Delegated
                                                                   select positionFulfillment;

            return positionFulfillments;
        }

        public static IQueryable<PositionFulfillment> GetDelegatedPositions(int employeeId, DateTime fromDate)
        {
            //todo check Delegation is appraisable


            var positionService = new EntityService<Position>();

            IQueryable<PositionFulfillment> positionFulfillments = from position in positionService.GetAll()
                                                                   from delegation in position.Delegations
                                                                   from positionFulfillment in
                                                                       position.PositionFulfillments
                                                                   where delegation.Appraisable
                                                                         &&
                                                                         positionFulfillment.Position.Id == position.Id
                                                                         &&
                                                                         positionFulfillment.Employee.Id == employeeId
                                                                         &&
                                                                         positionFulfillment.Type ==
                                                                         PositionFulfillmentType.Delegated
                                                                         && positionFulfillment.FromDate >= fromDate
                                                                   //  && positionFulfillment.IsPrimary 
                                                                   select positionFulfillment;

            return positionFulfillments;
        }

        public static Position GetPrimaryPosition(int employeeId)
        {
            PositionFulfillment positionFulfillment =
                GetPositions(employeeId).Where(pf => pf.Type == PositionFulfillmentType.Primary).FirstOrDefault();
            if (positionFulfillment == null)
            {
                throw new ArgumentException("Primary Position");
            }
            return positionFulfillment.Position;
        }

        //public static Position GetPrimapryPosition(int employeeId)
        //{

        //    return GetPositions(employeeId).Where(p=>p.is)
        //}


        public static Appraisal GetSelfAssesmentAppraisal(int employeeId)
        {
            //comment after delete employee from appraisal
            //var appraisalService = new EntityService<Appraisal>();
            //return appraisalService.GetAll().SingleOrDefault(x => x.Employee.Id == employeeId);
            return null;
        }

        public static TeamMember GetProjectMembership(int employeeId, int projectId)
        {
            var projectService = new EntityService<Project>();
            IQueryable<TeamMember> query = from project in projectService.GetAll()
                                           from projectTeam in project.ProjectTeams
                                           from projectTeamRole in projectTeam.ProjectTeamRoles
                                           from teamMember in projectTeamRole.TeamMembers
                                           where project.Id == projectId
                                                 && projectTeam.Project.Id == project.Id
                                                 && projectTeamRole.ProjectTeam.Id == projectTeam.Id
                                                 && teamMember.ProjectTeamRole.Id == projectTeamRole.Id
                                           select teamMember;
            return (query.FirstOrDefault());
        }

        public static IQueryable<PhaseTask> GetEmployeeTasks(int employeeId)
        {
            var projectService = new EntityService<Project>();
            IQueryable<PhaseTask> result = from project in projectService.GetAll()
                                           from phase in project.ProjectPhases
                                           from task in phase.Tasks
                                           where task.TeamMember.Employee.Id == employeeId
                                           select task;


            return result;
        }

        #region // Position Fulfillment //

        public static bool IsExistEmployeePrimaryPositionFulfillment(UnitOfWork unitOfWork, int employeeId)
        {
            return
                GetPositions(unitOfWork, employeeId).Where(
                    positionFulfillment => positionFulfillment.Type == PositionFulfillmentType.Primary).Any();
        }

        public static bool IsExistPrimaryPositionFulfillment(Position position)
        {
            return position.PositionFulfillments.Where(positionFulfillment => positionFulfillment.ExpireDate == null &&
                                                                              positionFulfillment.Type ==
                                                                              PositionFulfillmentType.Primary).Any();
        }

        public static decimal GetEmployeePositionFulfillmentToatalWeights(UnitOfWork unitOfWork, int employeeId,
                                                                          PositionFulfillmentType
                                                                              positionFulfillmentType)
        {
            if (
                GetPositions(unitOfWork, employeeId).Where(
                    positionFulfillment => positionFulfillment.Type == positionFulfillmentType).Any())
                return GetPositions(unitOfWork, employeeId).Where(
                    positionFulfillment => positionFulfillment.Type == positionFulfillmentType)
                    .Sum(positionFulfillment => positionFulfillment.Weight);


            return 0;
        }

        public static Position UpdatePrimaryPositionFulfillmentWeight(UnitOfWork unitOfWork, int employeeId,
                                                                      decimal totalSecondaryWeights)
        {
            PositionFulfillment primaryPositionFulfillment =
                GetPositions(unitOfWork, employeeId).Where(x => x.Type == PositionFulfillmentType.Primary).
                    SingleOrDefault();

            if (primaryPositionFulfillment != null)
            {
                decimal newPrimaryPositionFulfillmentWeight = 100 - totalSecondaryWeights;
                primaryPositionFulfillment.Weight = newPrimaryPositionFulfillmentWeight;
                return primaryPositionFulfillment.Position;
            }

            return null;
        }

        #endregion
    }
}