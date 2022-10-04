#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using Model.JobDesc.Entities;
using Model.JobDesc.ValueObjects;
using Model.Objectives.Entities;
using Model.OrgChart.ValueObjects;
using Model.OrgChart.ValueObjects.AssignedGrade;
using Model.PMS.Entities;
using Model.PMS.ValueObjects;
using Model.PMS.ValueObjects.Implementation.Competency;
using Model.PMS.ValueObjects.Implementation.Customized;
using Model.PMS.ValueObjects.Implementation.JobDescription;
using Model.PMS.ValueObjects.Implementation.Objective;
using Model.PMS.ValueObjects.Implementation.Project;
using Model.Personnel.Entities;
using Model.ProjectManagment.Entities;
using Model.ProjectManagment.ValueObjects;
using Model.Services;
using Repository.NHibernate;
using Resources.Shared.Messages;
using Service.JobDesc;
using Service.OrgChart;
using Service.Personnel;
using Repository.UnitOfWork;

#endregion

namespace Service.PMSComprehensive
{
    public static class AppraisalHelper
    {
        //todo notimplemented yet for new case
        public static Appraisal CreateAppraisalForEmployee(Employee employee)
        {
            //comment after delete phase form appraisal
           /* if ((employee == null) || (employee.IsTransient()))
            {
                throw new ArgumentNullException("");
            }

            else
            {

                var apprisalPhase = AppraisalPhaseHelper.GetCurrent();
               var apprisal = apprisalPhase.Appraisals.Where(a => a.Employee.Id == employee.Id).FirstOrDefault();
                if (apprisal != null)
                {
                    return apprisal;
                }

                else
                {
                    #region Properties

                    var appraisalEntityService = new EntityService<Appraisal>();

                    #endregion

                    #region Prepare Properties

                    Position position = EmployeeHelpers.GetPrimaryPosition(employee.Id);

                    if (position == null)
                    {
                        throw new ArgumentNullException("Position Can't be Empty");
                    }

                    var positionGrade = position.ActiveGrade;

                    if (positionGrade == null)
                    {
                        throw new ArgumentNullException("Grade Can't be Empty");
                    }

                    AppraisalTemplate template = AppraisalTemplateHelper.GetAppraisalTemplate(position);
                    if (template == null)
                    {
                        throw new ArgumentNullException("Template Can't be Empty");
                    }

                    Employee appraiser = employee;
                    if (apprisalPhase.WithSelfAssessment)
                    {
                        appraiser = employee;
                    }
                    else
                    {
                        var positionManager =
                            position.PositionReportings.Where(p => p.IsActive && p.IsPrimary).FirstOrDefault();
                        if (positionManager != null)
                        {
                            appraiser = PositionHelpers.GetCurrentEmployee(positionManager.Position.Id);
                        }
                    }

                    var appraisal = new Appraisal
                    {
                        Appraiser = appraiser,
                        Employee = employee,
                        Position = position.Name,
                        AppraiserJobTitle = position.JobTitle.Name,
                        EmployeeOrganizationalLevel = position.Level.Name,
                        EmployeeGrade = positionGrade.Name,
                        Date = DateTime.Today
                    };

                    var jobDescription = JobDescHelper.GetJobDescription(position.JobTitle);


                    #endregion

                    #region Competency

                    if (template.CanAddCompetency)
                    {
                        var competencySection = new CompetencySection() { Weight = template.WeightCompetency };
                        ImportCompetency(jobDescription, competencySection);
                        appraisal.AddCompetencySection(competencySection);
                    }

                    #endregion

                    #region Job Description

                    if (template.CanAddJobDescription)
                    {
                        var jobDescriptionSection = new JobDescriptionSection();
                        ImportJobDescription(jobDescription, jobDescriptionSection);



                        var delegatedPositions = EmployeeHelpers.GetDelegatedPositions(employee.Id,
                                                                                        apprisalPhase.FromDate);

                        foreach (PositionFulfillment positionFulfillment in delegatedPositions)
                        {
                            ImportIndirectJobDescriptions(positionFulfillment, jobDescriptionSection);
                        }
                        appraisal.AddJobDescriptionSection(jobDescriptionSection);
                    }

                    #endregion

                    #region Objective

                    if (template.CanAddObjective)
                    {
                        var objectiveSection = new ObjectiveSection();
                        ImportObjectives(employee.Id, position, objectiveSection);
                        appraisal.AddObjectiveSection(objectiveSection);
                    }

                    #endregion

                    #region Project

                    if (template.CanAddProject)
                    {
                        var projectSection = new ProjectSection();
                        ImportProjects(employee.Id, projectSection);
                        appraisal.AddProjectSection(projectSection);
                    }

                    #endregion

                    #region Customized Sections

                    if (template.Sections.Count > 0)
                    {
                        ImportCustomizedSection(template, appraisal);
                    }

                    #endregion

                    #region Update Appraisal Entity

                    try
                    {
                        AppraisalPhaseHelper.AddAppraisal(appraisal);
                    }
                    catch (Exception)
                    {
                        throw new Exception(General.GeneralErrorOccurred);
                    }

                    #endregion

                    return appraisal;
                }
            }*/


            return null;

        }
        public static void DeleteAppraisalForEmployee(Employee employee)
        {
            //Comment after delete employee from appraisal
            //using (var unitOfWork = new UnitOfWork())
            //{
            //    var appraisalService = new EntityService<Appraisal>(unitOfWork);
            //    var appraisal = appraisalService.GetAll().Where(x => x.Employee.Id == employee.Id).FirstOrDefault();
            //    if (appraisal != null)
            //    {
            //        appraisalService.DeletEntity(appraisal);
            //        unitOfWork.Commit();
            //    }
            //}


        }
        public static Appraisal CreateAppraisalForEmployee(Employee employee, Position position, AppraisalPhase appraisalPhase, Employee appraiser)
        {
            if ((employee == null) || (employee.IsTransient()))
            {
                throw new ArgumentNullException("Employee can not be null or transient");
            }
            if ((position == null) || (position.IsTransient()))
            {
                throw new ArgumentNullException("Position can not be null or transient");
            }
            if ((appraisalPhase == null) || (appraisalPhase.IsTransient()))
            {
                throw new ArgumentNullException("Appraisal Phase can not be null or transient");
            }


            else
            {

                #region Prepare Properties
                var positionGrade = position.ActiveGrade;
                if (positionGrade == null)
                {
                    throw new ArgumentNullException("Grade Can't be Empty");
                }
                var template = AppraisalTemplateHelper.GetAppraisalTemplate(position);
                if (template == null)
                {
                    throw new ArgumentNullException("Template Can't be Empty");
                }


                var appraisal = new Appraisal
                {
                   // Appraiser = appraiser,
                  //  Employee = employee,
                    Position = position.Name,
                    AppraiserJobTitle = position.JobTitle.Name,
                    EmployeeOrganizationalLevel = position.Level.Name,
                    EmployeeGrade = positionGrade.Name,
                    Date = DateTime.Today
                };

                var jobDescription = JobDescHelper.GetJobDescription(position.JobTitle);


                #endregion

                #region Competency

                if (template.CanAddCompetency)
                {
                    var competencySection = new CompetencySection() { Weight = template.WeightCompetency };
                    ImportCompetency(jobDescription, competencySection);
                    appraisal.AddCompetencySection(competencySection);
                }

                #endregion

                #region Job Description

                if (template.CanAddJobDescription)
                {
                    var jobDescriptionSection = new JobDescriptionSection();
                    ImportJobDescription(jobDescription, jobDescriptionSection);



                    var delegatedPositions = EmployeeHelpers.GetDelegatedPositions(employee.Id,
                                                                                    appraisalPhase.FromDate);

                    foreach (var positionFulfillment in delegatedPositions)
                    {
                        ImportIndirectJobDescriptions(positionFulfillment, jobDescriptionSection);
                    }
                    appraisal.AddJobDescriptionSection(jobDescriptionSection);
                }

                #endregion

                #region Objective

                if (template.CanAddObjective)
                {
                    var objectiveSection = new ObjectiveSection();
                    ImportObjectives(employee.Id, position, objectiveSection);
                    appraisal.AddObjectiveSection(objectiveSection);
                }

                #endregion

                #region Project

                if (template.CanAddProject)
                {
                    var projectSection = new ProjectSection();
                    ImportProjects(employee.Id, projectSection);
                    appraisal.AddProjectSection(projectSection);
                }

                #endregion

                #region Customized Sections

                if (template.Sections.Count > 0)
                {
                    ImportCustomizedSection(template, appraisal);
                }

                #endregion

                //#region Update Appraisal Entity

                //try
                //{
                //    AppraisalPhaseHelper.AddAppraisal(appraisal);
                //}
                //catch (Exception)
                //{
                //    throw new Exception(General.GeneralErrorOccurred);
                //}

                //#endregion

                return appraisal;

            }




        }

        public static Appraisal CreateAppraisalForEmployee(Employee employee, Position position, Employee appraiser)
        {
            if ((employee == null) || (employee.IsTransient()))
            {
                throw new ArgumentNullException("Employee can not be null or transient");
            }
            if ((position == null) || (position.IsTransient()))
            {
                throw new ArgumentNullException("Position can not be null or transient");
            }
            //if ((appraisalProcess == null) || (appraisalProcess.IsTransient()))
            //{
            //    throw new ArgumentNullException("Appraisal Phase can not be null or transient");
            //}


            else
            {

                #region Prepare Properties
                var positionGrade = position.ActiveGrade;
                if (positionGrade == null)
                {
                    throw new ArgumentNullException("Grade Can't be Empty");
                }
                var template = AppraisalTemplateHelper.GetAppraisalTemplate(position);
                if (template == null)
                {
                    throw new ArgumentNullException("Template Can't be Empty");
                }


                var appraisal = new Appraisal
                {
                   // Appraiser = appraiser,
                    //Employee = employee,
                    Position = position.Name,
                    AppraiserJobTitle = position.JobTitle.Name,
                    EmployeeOrganizationalLevel = position.Level.Name,
                    EmployeeGrade = positionGrade.Name,
                    Date = DateTime.Today
                };

                var jobDescription = JobDescHelper.GetJobDescription(position.JobTitle);


                #endregion

                #region Competency

                if (template.CanAddCompetency)
                {
                    var competencySection = new CompetencySection() { Weight = template.WeightCompetency };
                    ImportCompetency(jobDescription, competencySection);
                    appraisal.AddCompetencySection(competencySection);
                }

                #endregion

                #region Job Description

                if (template.CanAddJobDescription)
                {
                    var jobDescriptionSection = new JobDescriptionSection();
                    ImportJobDescription(jobDescription, jobDescriptionSection);



                    //var delegatedPositions = EmployeeHelpers.GetDelegatedPositions(employee.Id,
                    //                                                                appraisalPhase.FromDate);

                    //foreach (var positionFulfillment in delegatedPositions)
                    //{
                    //    ImportIndirectJobDescriptions(positionFulfillment, jobDescriptionSection);
                    //}
                    appraisal.AddJobDescriptionSection(jobDescriptionSection);
                }

                #endregion

                #region Objective

                if (template.CanAddObjective)
                {
                    var objectiveSection = new ObjectiveSection();
                    ImportObjectives(employee.Id, position, objectiveSection);
                    appraisal.AddObjectiveSection(objectiveSection);
                }

                #endregion

                #region Project

                if (template.CanAddProject)
                {
                    var projectSection = new ProjectSection();
                    ImportProjects(employee.Id, projectSection);
                    appraisal.AddProjectSection(projectSection);
                }

                #endregion

                #region Customized Sections

                if (template.Sections.Count > 0)
                {
                    ImportCustomizedSection(template, appraisal);
                }

                #endregion



                return appraisal;

            }




        }

        #region Import Generic Sections

        private static void ImportCustomizedSection(AppraisalTemplate template, Appraisal apprisal)
        {
            foreach (var tempCustomizedSection in template.Sections)
            {
                var customizedSection = new CustomizedSection();
                customizedSection.Name = tempCustomizedSection.Name;
                customizedSection.Weight = tempCustomizedSection.Weight;

                foreach (var tempCustomizedSectionItem in tempCustomizedSection.SectionItems)
                {
                    var sectionItem = new SectionItem
                    {
                        //CustomizedSection = customizedSection,
                        Name = tempCustomizedSectionItem.Name,
                        Weight = tempCustomizedSectionItem.Weight,
                        Description = tempCustomizedSectionItem.Description
                    };

                    foreach (var tempCustomizedSectionItemKpi in tempCustomizedSectionItem.Kpis)
                    {
                        var itemKpi = new ItemKpi
                        {
                            //Item = sectionItem,
                            Value = tempCustomizedSectionItemKpi.Value,
                            Description = tempCustomizedSectionItemKpi.Description
                        };

                        sectionItem.AddKpi(itemKpi);
                    }

                    customizedSection.AddItems(sectionItem);
                }

                apprisal.AddGenericSection(customizedSection);
            }
        }

        #endregion

        #region Import Competencies

        private static void ImportCompetency(JobDescription jobDescription, CompetencySection competencySection)
        {
            var competenciesList = new List<Competency>();
            if (jobDescription != null)
            {
                competenciesList.AddRange(jobDescription.Specification.First().Competencies);
            }

            foreach (var competency in competenciesList)
            {
                var competencySectionItem = new CompetencySectionItem
                {
                    Name = competency.Name,
                    Description = competency.Description,
                    Weight = competency.Weight,
                    Type = competency.Type.Name,
                };
                competencySection.AddItems(competencySectionItem);
            }
        }

        #endregion

        #region Import Job Description

        private static void ImportJobDescription(JobDescription jobDescription, JobDescriptionSection jobDescriptionSection)
        {
            #region job Description Initialization

            if (jobDescription == null)
                return;

            #endregion

            var jobDescriptionSectionItem = new JobDescriptionSectionItem { JobTitle = jobDescription.JobTitle.Name, IsIndirect = false };

            #region Add Responsibilities

            foreach (Role role in jobDescription.Roles)
                ImportRoleResponsibilities(jobDescriptionSectionItem, role);

            #endregion

            jobDescriptionSection.AddItems(jobDescriptionSectionItem);
        }

        private static void ImportIndirectJobDescriptions(PositionFulfillment positionFulfillment,
                                                          JobDescriptionSection jobDescriptionSection)
        {

            var jobDescriptionSectionItem = new JobDescriptionSectionItem { JobTitle = positionFulfillment.Position.JobTitle.Name, IsIndirect = true };

            foreach (var pfRole in positionFulfillment.Roles)
            {
                ImportRoleResponsibilities(jobDescriptionSectionItem, pfRole.Role);
                jobDescriptionSection.AddItems(jobDescriptionSectionItem);
            }


        }

        private static void ImportRoleResponsibilities(JobDescriptionSectionItem jobDescriptionSectionItem, Role role)
        {
            foreach (var responsibility in role.Responsibilities)
            {
                var jobDescriptionSectionTask = new JobDescriptionSectionTask
                {
                    RoleName = role.Name,
                    JobTask = responsibility.Description,
                    Weight = responsibility.Weight
                };

                AddResponsibilityKPI(responsibility, jobDescriptionSectionTask);

                jobDescriptionSectionItem.AddItems(jobDescriptionSectionTask);
            }
        }

        private static void AddResponsibilityKPI(Responsibility responsibility,
                                                 JobDescriptionSectionTask jobDescriptionSectionTask)
        {
            foreach (var responsibilityKpi in responsibility.ResponsibilityKpis)
            {
                var jobDescriptionSectionItemKpi = new JobDescriptionSectionItemKpi
                {
                    Description = responsibilityKpi.Description,
                    Value = responsibilityKpi.Value
                };
                jobDescriptionSectionTask.AddKpi(jobDescriptionSectionItemKpi);
            }
        }

        #endregion

        #region Import Objectives

        private static void ImportObjectives(int employeeId, Position position, ObjectiveSection objectiveSection)
        {
            #region Owner

            foreach (
                var objective in
                    new EntityService<Objective>().GetAll().Where(x => x.Owner.Id == position.Id))
            {
                var objectiveSectionItem = new ObjectiveSectionItem
                {
                    Name = objective.Name,
                    Description = objective.Description,
                    IsShared = false,
                    SharedWithPercentage = 0,
                    Weight = objective.Weight
                };

                GetobjectiveKpisValues(objectiveSectionItem, objective);

                objectiveSection.AddItems(objectiveSectionItem);
            }

            #endregion

            #region Shared

            foreach (var objective in new EntityService<Objective>().GetAll().Where(
                x =>
                x.SharedWiths.Count(e =>  e.Objective.Owner.Id == position.Id) >
                0))
            {
                var objectiveSectionItem = new ObjectiveSectionItem
                {
                    Name = objective.Name,
                    Description = objective.Description,
                    IsShared = true,
                    SharedWithPercentage =
                        objective.SharedWiths.First(
                          //  x => x.Employee.Id == employeeId).Percentage,
                          ).Percentage,
                    Weight = objective.Weight
                };

                GetobjectiveKpisValues(objectiveSectionItem, objective);

                objectiveSection.AddItems(objectiveSectionItem);
            }

            #endregion
        }

        private static void GetobjectiveKpisValues(ObjectiveSectionItem objectiveSectionItem,
                                                   Objective objective)
        {
            foreach (
                var objectiveSectionItemKpi in
                    objective.Kpis.Select(objectiveKpi => new ObjectiveSectionItemKpi
                    {
                        Description =
                            objectiveKpi.Description,
                        Type = objectiveKpi.Type.Name,
                        Value = objectiveKpi.Value,
                        Weight = objectiveKpi.Weight
                    }))
            {
                objectiveSectionItem.AddKpi(objectiveSectionItemKpi);
            }
        }

        #endregion

        #region Import Projects

        private static void ImportProjects(int employeeId, ProjectSection projectSection)
        {
            var employeeTasks = EmployeeHelpers.GetEmployeeTasks(employeeId);

            foreach (var task in employeeTasks)
            {
                var projectSectionItem = new ProjectSectionItem
                {
                    //ProjectSection = projectSection,
                    Weight = task.Weight,
                    TaskKpi = task.TaskKpi,
                    TaskDescription = task.Description,
                    Phase = task.ProjectPhase.Name,
                    Role = task.TeamRole.Role.Name
                };

                projectSection.AddItems(projectSectionItem);
            }
        }

        #endregion

    }







}