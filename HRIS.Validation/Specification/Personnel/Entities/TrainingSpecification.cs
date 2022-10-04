
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class TrainingSpecification : Validates<HRIS.Domain.Personnel.Entities.Training>
    {
        public TrainingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.CourseName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.TrainingCenter).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.TrainingCenterLocation).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.CourseDuration).Required().GreaterThanEqualTo(0);
            Check(x => x.CertificateIssuanceDate).Required().LessThanEqualTo(DateTime.Now);

            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes
            Check(x => x.Specialize)
               .Required()
               .Expect((training, status) => status.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            

            //Check(x => x.Status)
            //    .Required()
            //    .Expect((training, status) => status.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion 
        }
    }
}
