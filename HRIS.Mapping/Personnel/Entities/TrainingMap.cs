#region

using FluentNHibernate.Mapping;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class TrainingMap : ClassMap<Domain.Personnel.Entities.Training>
    {
        public TrainingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.CourseName).Length(GlobalConstant.SimpleStringMaxLength);
            References(x => x.Specialize);
            Map(x => x.CourseDuration);
            Map(x => x.TrainingCenter).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.TrainingCenterLocation).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CertificateIssuanceDate);
            References(x => x.Status);
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);
            
            References(x => x.Employee);
        }
    }
}