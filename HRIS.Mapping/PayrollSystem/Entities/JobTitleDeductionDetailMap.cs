﻿//using FluentNHibernate.Mapping;
//using HRIS.Domain.OrganizationChart.Entities;
//using HRIS.Domain.PayrollSystem.Entities;

//namespace HRIS.Mapping.PayrollSystem.Entities
//{
//    public class JobTitleDeductionDetailMap : ClassMap<JobTitleDeductionDetail>
//    {
//        public JobTitleDeductionDetailMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.Value);
//            Map(x => x.Formula);
//            Map(x => x.ExtraValue);
//            Map(x => x.ExtraValueFormula);
//            Map(x => x.Note);
//            Map(x => x.AuditState);

//            References(x => x.DeductionCard);
//            References(x => x.JobTitle);
//        }
//    }
//}
