﻿using HRIS.Domain.Recruitment.Indexes;
using FluentNHibernate.Mapping;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Indexes
{
    public class InterviewTypeMap : ClassMap<InterviewType>
    {
        public InterviewTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
