using Project.Web.Mvc4.APIAttribute;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class NotifyHelper
    {
        public static List<Notify> CheckNotifications(BasicAuthenticationIdentity identity)
        {
            var user = ServiceFactory.ORMService.All<User>()
                    .FirstOrDefault(x => x.Username == identity.Name);
            var unreadedNotifications = ServiceFactory.ORMService.All<NotifyReceiver>()
                .Where(x => x.Receiver == user 
                && !x.IsRead 
                &&( x.Notify.DestinationEntityId == "EmployeeLeaveRequest"||x.Notify.DestinationEntityId == "EntranceExitRecordRequest"))
                .Select(x => x.Notify).ToList();
            return unreadedNotifications;
        }
    }
}