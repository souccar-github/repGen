#region

using System.Linq;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;

#endregion

namespace Service.PMSComprehensive
{
    public class ProjectPhaseToEvaluatedPhase
    {
        public static void Clone(int projectId, int projectEvaluationId)
        {
            var service = new EntityService<Project>();

            Project project = service.LoadById(projectId);

            if (project != null)
            {
                ProjectEvaluation evaluation =
                    project.ProjectEvaluations.SingleOrDefault(e => e.Id == projectEvaluationId);

                if (evaluation != null)
                {
                    foreach (ProjectPhase projectPhase in project.ProjectPhases)
                    {
                        var evaluatedPhase = new EvaluatedPhase
                                                 {
                                                     CompletionPercentage = projectPhase.CompletionPercentage,
                                                     Description = projectPhase.Description,
                                                     EndDate = projectPhase.EndDate,
                                                     Name = projectPhase.Name,
                                                     StartDate = projectPhase.StartDate,
                                                     Status = projectPhase.Status,
                                                     Team = projectPhase.Team,
                                                     TeamRole = projectPhase.TeamRole,
                                                     TeamMember = projectPhase.TeamMember
                                                 };

                        foreach (PhaseTask phaseTask in projectPhase.Tasks)
                        {
                            evaluatedPhase.AddTask(new EvaluatedTask
                                                       {
                                                           ActualClosingDate = phaseTask.ActualClosingDate,
                                                           Constraints = phaseTask.Constraints,
                                                           Status = phaseTask.Status,
                                                           Description = phaseTask.Description,
                                                           DeadLine = phaseTask.DeadLine,
                                                           Remarks = phaseTask.Remarks,
                                                           TaskKpi = phaseTask.TaskKpi,
                                                           Team = phaseTask.Team,
                                                           TeamRole = phaseTask.TeamRole,
                                                           TeamMember = phaseTask.TeamMember,
                                                           Weight = phaseTask.Weight
                                                       });
                        }

                        evaluation.AddEvaluatedPhase(evaluatedPhase);
                    }

                    service.Update(project);
                }
            }
        }
    }
}