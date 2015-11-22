﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using Data.Infrastructure;
using Hub.Interfaces;
using Utilities;

namespace Hub.Services
{
    public class Notification : INotification
    {
        private readonly IConfigRepository _configRepository;
	    private readonly ITime _time;

        public Notification()
        {
            _configRepository = ObjectFactory.GetInstance<IConfigRepository>();
	        _time = ObjectFactory.GetInstance<ITime>();
        }

        public bool IsInNotificationWindow(string startTimeConfigName, string endTimeConfigName)
        {
            var startTimeStr = _configRepository.Get<string>(startTimeConfigName);
            var endTimeStr = _configRepository.Get<string>(endTimeConfigName);
            var startTime = DateTimeOffset.Parse(startTimeStr);
            var endTime = DateTimeOffset.Parse(endTimeStr).AddDays(1);    //We need to add days - since the end time is in the morning (For example 8pm -> 4am).
                                                                    //Not adding the AddDays() would mean we're never in the time frame (after 8pm and before 4am on the same dame).
			var currentTime = _time.CurrentDateTimeOffset();
            return (currentTime > startTime && currentTime < endTime);
        }

        public void Generate(string userId, string message, TimeSpan expiresIn = default(TimeSpan))
        {
            EventManager.UserNotification(userId, message, expiresIn);
        }
    }
}
