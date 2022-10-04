#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class SkillMap : ClassMap<Skill>
    {
        public SkillMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Name);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            References(x => x.Level);
            Map(x => x.Comments).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Employee);
        }
    }
}