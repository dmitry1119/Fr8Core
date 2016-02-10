﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Segment;
using Segment.Model;
using StructureMap;
using Data.Infrastructure.StructureMap;
using Data.Interfaces;
using Hub.Interfaces;
using Hub.Services;

namespace HubWeb.Controllers
{
    public class TokenAuthController : Controller
    {
	    private readonly ITime _time;

	    public TokenAuthController()
	    {
		    _time = ObjectFactory.GetInstance<ITime>();
	    }

        public ActionResult Index(String token)
        {
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                var validToken = uow.AuthorizationTokenRepository.FindTokenById(token);
                
				if (validToken == null)
                    throw new HttpException(404, "Authorization token not found.");

	            DateTime currentTime = _time.CurrentDateTime();

				if (validToken.ExpiresAt < currentTime)
                    throw new HttpException(404, "Authorization token expired.");

                // Auth token are cached, so navigational properties will not work
                var user = uow.UserRepository.GetByKey(validToken.UserID);

                ObjectFactory.GetInstance<ISecurityServices>().Login(uow, user);

                if (!String.IsNullOrEmpty(validToken.SegmentTrackingEventName))
                {
                    Properties segmentProps = null;
                    if (!String.IsNullOrEmpty(validToken.SegmentTrackingProperties))
                    {
                        segmentProps = new Properties();
                        var trackingProperties = JsonConvert.DeserializeObject<Dictionary<String, Object>>(validToken.SegmentTrackingProperties);
                        foreach (var prop in trackingProperties)
                            segmentProps.Add(prop.Key, prop.Value);
                    }

                    ObjectFactory.GetInstance<ITracker>().Track(user, validToken.SegmentTrackingEventName, segmentProps);
                }

                return Redirect(validToken.RedirectURL);
            }
        }
	}
}