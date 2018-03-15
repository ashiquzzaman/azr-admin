﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AzR.Core.Notifications
{
    [HubName("notification")]
    public class NotificationHub : Hub
    {
        [HubMethodName("notify")]
        public static void Notify()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.notify();
        }
    }
}