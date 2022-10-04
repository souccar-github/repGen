
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Details(IsDetailHidden = false)]
    public class LeaveTemplateDetail : Entity
    {
        
        #region Basic Info

        [UserInterfaceParameter(IsReference = true, Order = 1)]
        public virtual LeaveSetting LeaveSetting { get; set; }

        public virtual LeaveTemplateMaster LeaveTemplateMaster { get; set; }

        #endregion

    }
}
