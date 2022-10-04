using HRIS.Domain.AttendanceSystem.RootEntities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class AttendanceRecordSpecification : Validates<AttendanceRecord>
    {
        public AttendanceRecordSpecification()
        {
            IsDefaultForType();

            Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.Name).Required();
            Check(x => x.Date).Required();
            Check(x => x.Note).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
