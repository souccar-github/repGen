﻿#region

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.EmployeeRelationServices.Indexes
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.Training)]
    
    public class DocumentType : IndexEntity, IAggregateRoot
    {
    }
}