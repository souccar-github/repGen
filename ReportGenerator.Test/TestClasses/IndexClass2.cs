using System;
using System.Collections.Generic;
using System.Reflection;
using Infrastructure.Localization;
using Resources.Areas.Personnel.Entities.Employee;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class IndexClass2 : IndexEntity
    {
        #region IIndex Members
        
        [LocalizationDisplayName("FirstName", typeof (EmployeeModel))]
        public new string Name { get; set; }

        #endregion
    }
}