using HRIS.Domain.AttendanceSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Dtos
{
    public class WorkflowInfoDto
    {
        public string Type { get; set; }
        public string PendingStep { get; set; }
        public bool WaitingApprove { get; set; }
        public DateTime? Date { get; set; }
        public LogType LogType { get; set; }
    }
}