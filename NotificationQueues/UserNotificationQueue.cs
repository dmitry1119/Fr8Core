﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Infrastructure;

namespace Web.NotificationQueues
{
    public class UserNotificationQueue : SharedNotificationQueue<UserNotificationData>
    {
        public UserNotificationQueue()
        {
            EventManager.AlertUserNotification +=
                (id, message, expiresIn) => AppendUpdate(new UserNotificationData(id, message), expiresIn);
        }
    }
}