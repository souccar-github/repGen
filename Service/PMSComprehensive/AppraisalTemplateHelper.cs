using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.PMS.Entities;
using Model.OrgChart.ValueObjects;

namespace Service.PMSComprehensive
{
    public class AppraisalTemplateHelper
    {
        public static AppraisalTemplate GetAppraisalTemplate(Position position)
        {
            var gradeId = position.ActiveGrade.GradeId;
            var appraisalTemplateService = new EntityService<AppraisalTemplate>();
            var query =
                from appraisalTemplate in appraisalTemplateService.GetAll()
                from appraisalTemplateGrade in appraisalTemplate.AppraisalTemplateGrades
                where  appraisalTemplateGrade.Grade.Id == gradeId
                                                            && appraisalTemplateGrade.PositionLevel.Id == position.Level.Id
                select appraisalTemplate;
            return query.SingleOrDefault();
        }
    }
}
