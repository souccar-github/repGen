using System.Collections.Generic;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Configurations
{
    [Order(35)]
    [Module(ModulesNames.AttendanceSystem)]
    public class InfractionForm : Entity, IConfigurationRoot  // نموذج مخالفة
    {

        public InfractionForm()
        {
            InfractionSlices = new List<InfractionSlice>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual  int Number { get; set; } // رقم المخالفة

        [UserInterfaceParameter(Order = 1)]
        public virtual  string Description { get; set; } // وصف المخالفة

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Description; }
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual  IList<InfractionSlice> InfractionSlices { get; set; } // شراح المخالفة
        public virtual void AddInfractionSlice(InfractionSlice infractionSlice)
        {
            infractionSlice.InfractionForm = this;
            InfractionSlices.Add(infractionSlice);
        }
    }
}
