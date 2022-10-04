﻿using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class EmployeeLoanMap : ClassMap<EmployeeLoan>
    {
        public EmployeeLoanMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.TotalAmountOfLoan);
            Map(x => x.PrePayed);
            Map(x => x.Date);
            Map(x => x.MonthlyInstalmentValue);
            Map(x => x.LoanNumber);
            Map(x => x.FirstRepresentative);
            Map(x => x.SecondRepresentative);
            Map(x => x.RemainingAmountOfLoan);
            //Map(x => x.AuditState);

            References(x => x.EmployeeCard);
            References(x => x.DonorLoan);

            HasMany(x => x.LoanPayments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}