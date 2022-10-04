﻿using HRIS.Domain.Personnel.Entities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class AttachmentSpecification : Validates<Attachment>
    {
        public AttachmentSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Title).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);


            #endregion




        }
    }
}
