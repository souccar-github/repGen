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
    public static class AppraisaProccesslHelper
    {
       
        public static void DeleteAppraisaProccessForEmployee(Employee employee)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var appraisalProccesService = new EntityService<AppraisalProcess>(unitOfWork);
                var appraisalProcces = appraisalProccesService.GetAll().Where(x => x.Employee.Id == employee.Id).FirstOrDefault();
                if (appraisalProcces != null)
                {
                    appraisalProccesService.DeletEntity(appraisalProcces);
                    unitOfWork.Commit();
                }
            }
        }
        public static AppraisalProcess GetAppraisalProcess(int id)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var appraisalProccesService = new EntityService<AppraisalProcess>(unitOfWork);
                return appraisalProccesService.GetById(id);
                
            }
        }
 
    }


}