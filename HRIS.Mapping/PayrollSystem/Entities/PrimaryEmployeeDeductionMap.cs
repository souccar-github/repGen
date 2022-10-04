﻿using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class PrimaryEmployeeDeductionMap : ClassMap<PrimaryEmployeeDeduction>
    {

        public PrimaryEmployeeDeductionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.HasExpiryDate);
            Map(x => x.HasStartDate);
            Map(x => x.ExpiryDate).Nullable(); ;
            Map(x => x.StartDate).Nullable();
            Map(x => x.Value);
            Map(x => x.Formula);
            Map(x => x.ExtraValue);
            Map(x => x.ExtraValueFormula);
            Map(x => x.Note);
            //Map(x => x.AuditState);

            References(x => x.EmployeeCard);
            References(x => x.DeductionCard);
        }
    }
}
