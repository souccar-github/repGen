#region

using System.Collections.Generic;
using System.Linq;
using Domain.Seedwork;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Repository.UnitOfWork;

#endregion

namespace Service.ProjectManagement
{
    public class ProjectManagementHelper //: EmployeeVsPositionHelpers
    {
        private static readonly EntityService<Project> projectService;
        private static readonly IUnitOfWork unitOfWork;

        static ProjectManagementHelper()
        {
            unitOfWork = new UnitOfWork();
            projectService = new EntityService<Project>(unitOfWork);
        }

        public static IList<Project> GetAllProject()
        {
            return projectService.GetAll().ToList();
        }

        public static Project GetProject(int projectId)
        {
            return projectService.GetById(projectId);
        }

        private static IQueryable<ProjectTeam> GetProjectTeamQuery(int? projectId)
        {
            IQueryable<ProjectTeam> query;
            if (projectId.HasValue)
            {
                query = from project in projectService.GetAll()
                        from projectTeam in project.ProjectTeams
                        where project.Id == projectId.Value
                        select projectTeam;
            }
            else
            {
                query = from project in projectService.GetAll()
                        from projectTeam in project.ProjectTeams
                        select projectTeam;
            }
            return query;
            //return projectService.GetById(projectId);
        }

        public static List<ProjectTeam> GetProjectTeams(int projectId)
        {
            return GetProjectTeamQuery(projectId).ToList();
        }

        public static IList<ProjectTeamRole> GetProjectTeamRoles(int projectId)
        {
            return GetProjectTeamQuery(projectId).SelectMany(pt => pt.ProjectTeamRoles).ToList();
        }

        public static IList<ProjectTeamRole> GetProjectTeamRoles(int projectTeamId, int? projectId = null)
        {
            return
                GetProjectTeamQuery(projectId).Where(pt => pt.Id == projectTeamId).SelectMany(pt => pt.ProjectTeamRoles)
                    .ToList();
        }

        public static IList<TeamMember> GetProjectRoleTeamMember(int projectTeamRoleId)
        {
            IQueryable<TeamMember> query = from projectTeam in GetProjectTeamQuery(null)
                                           from teamRole in projectTeam.ProjectTeamRoles
                                           from teamMember in teamRole.TeamMembers
                                           where teamRole.Id == projectTeamRoleId
                                           select teamMember;
            return query.ToList();
        }
    }
}