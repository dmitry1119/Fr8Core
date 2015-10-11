﻿using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.Web.Http;
using Data.Entities;
using Data.States;

namespace pluginSlack.Controllers
{
    [RoutePrefix("plugins")]
    public class PluginController : ApiController
    {
        /// <summary>
        /// Plugin discovery infrastructure.
        /// Action returns list of supported actions by plugin.
        /// </summary>
        [HttpGet]
        [Route("discover")]
        [ResponseType(typeof(List<ActivityTemplateDO>))]
        public IHttpActionResult DiscoverPlugins()
        {
            var plugin = new PluginDO
            {
                Endpoint = "localhost:39504",
                PluginStatus = PluginStatus.Active,
                Name = "pluginSlack",
                RequiresAuthentication = true,
                Version = "1"
            };

            var monitorChannelAction = new ActivityTemplateDO
            {
                Name = "Monitor_Channel",
                Category = ActivityCategory.fr8_Monitor,
                Plugin = plugin,
                Version = "1"
            };

            var publishToSlackAction = new ActivityTemplateDO
            {
                Name = "Publish_To_Slack",
                Category = ActivityCategory.fr8_Forwarder,
                Plugin = plugin,
                Version = "1"
            };

            var result = new List<ActivityTemplateDO>()
            {
                monitorChannelAction,
                publishToSlackAction
            };

            return Json(result);    
        }
    }
}