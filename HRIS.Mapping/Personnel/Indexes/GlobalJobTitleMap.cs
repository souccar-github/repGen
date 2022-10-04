﻿using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Personnel.Indexes
{
    public sealed class GlobalJobTitleMap : ClassMap<GlobalJobTitle>
    {
        public GlobalJobTitleMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
