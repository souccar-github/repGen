using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using SpecExpress;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Validation.Specification.PMS.Configurations
{
    public class AppraisalPhaseSettingSpecification : Validates<AppraisalPhaseSetting>
    {
        public AppraisalPhaseSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.WorkflowSetting).Required();
            Check(x => x.FromMark).Optional().GreaterThanEqualTo(0);
            Check(x => x.ToMark).Required().GreaterThan(x => x.FromMark);

            Check(x => x.FullMark).Required().Between(x => x.FromMark, x => x.ToMark);
            Check(x => x.MarkStep).Required().Between(x => x.FromMark, x => x.ToMark);

            Check(x => x.Title).Required();
            Check(x => x.GapThreshold).Required().Between(x => x.FromMark, x => x.ToMark);
            Check(x => x.SkillThreshold).Required().GreaterThan(x => x.GapThreshold).And.Between(x => x.FromMark, x => x.ToMark);

            Check(x => x.FromMarkBelowExpected).Optional().Between(x => x.FromMark, x => x.ToMark);
            Check(x => x.ToMarkBelowExpected).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.FromMarkBelowExpected);
            
            Check(x => x.FromMarkNeedTraining).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.ToMarkBelowExpected);
            Check(x => x.ToMarkNeedTraining).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.FromMarkNeedTraining);

            Check(x => x.FromMarkExpected).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.ToMarkNeedTraining);
            Check(x => x.ToMarkExpected).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.FromMarkExpected);

            Check(x => x.FromMarkUpExpected).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.ToMarkExpected);
            Check(x => x.ToMarkUpExpected).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.FromMarkUpExpected);

            Check(x => x.FromMarkDistinct).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.ToMarkUpExpected);
            Check(x => x.ToMarkDistinct).Required().Between(x => x.FromMark, x => x.ToMark).And.GreaterThan(x => x.FromMarkDistinct);

            #endregion Primitive Types
            
            #region Indexes

            #endregion Indexes

        }
    }
}
