﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Core.CustomAttribute;

namespace Souccar.Domain.Workflow.Entities
{
    
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class WorkflowApproval : Entity
    {
        #region basic info
        public virtual DateTime Date { get; set; }
        public virtual int Order { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsSeen{ get; set; }
        public virtual WorkflowStepStatus Status { get; set; }
        [UserInterfaceParameter(IsReference = true)]
        public virtual User User { get; set; }
        #endregion

        public virtual WorkflowItem Workflow { get; set; }
    }
}
