#region

 //using Model.Objectives.ValueObjects;

#endregion

namespace Service.PMSComprehensive
{
    public class ObjectiveStepToEvaluatedObjectiveStep
    {
        //todo refactor or delete this code
        public static void Clone(int objectiveId, int evaluationId)
        {
            //var objectiveService = new EntityService<BasicObjective>();

            //var objective = objectiveService.LoadById(objectiveId);
            //if (objective != null)
            //{
            //    var evaluation = objective.Evaluations.SingleOrDefault(e => e.Id == evaluationId);
            //    if (evaluation != null)
            //    {
            //        foreach (ObjectiveStep objectiveStep in objective.ObjectiveSteps)
            //        {
            //            if (objectiveStep.ActualStartingDate != new DateTime(1800, 1, 1))
            //                // && objectiveStep.ActualClosingDate == new DateTime(1800, 1, 1))
            //            {
            //                evaluation.AddObjectiveStep(new EvaluatedObjectiveStep
            //                                                {
            //                                                    ActualStartingDate = objectiveStep.ActualStartingDate,
            //                                                    ActualClosingDate = objectiveStep.ActualClosingDate,
            //                                                    PlannedStartingDate = objectiveStep.PlannedStartingDate,
            //                                                    PlannedClosingDate = objectiveStep.PlannedClosingDate,
            //                                                    Description = objectiveStep.Description,
            //                                                    Number = objectiveStep.Number,
            //                                                    Owner = objectiveStep.Owner,
            //                                                    Status = objectiveStep.Status
            //                                                });
            //            }
            //        }
            //    }

            //    objectiveService.Update(objective);
            //}
        }
    }
}