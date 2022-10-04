﻿using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core;

namespace HRIS.Mapping.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    public class MonthlyEmployeeBenefitMap : ClassMap<MonthlyEmployeeBenefit>
    {
        public MonthlyEmployeeBenefitMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.FinalValue);
            Map(x => x.InitialValue);
            Map(x => x.Value);
            Map(x => x.Formula);
            Map(x => x.ExtraValue);
            Map(x => x.ExtraValueFormula);
            Map(x => x.CeilValue);
            Map(x => x.CeilFormula);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.SourceId);
            //Map(x => x.AuditState);
            Map(x => x.CrossDependencyInitialValue);

            References(x => x.MonthlyCard);
            References(x => x.BenefitCard);
        }
    }
}