#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Training : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual CourseSpecialize Specialize { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string CourseName { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual int CourseDuration { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string TrainingCenter { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual string TrainingCenterLocation { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual DateTime CertificateIssuanceDate { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual Status Status { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
