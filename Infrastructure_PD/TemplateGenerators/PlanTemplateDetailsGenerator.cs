﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fr8.Infrastructure.Data.DataTransferObjects;
using Fr8.Infrastructure.Utilities.Configuration;
using Hub.Interfaces;
using HubWeb.Templates;

namespace HubWeb.Infrastructure_PD.TemplateGenerators
{
    public class PlanTemplateDetailsGenerator : IPlanTemplateDetailsGenerator
    {
        private readonly ITemplateGenerator _templateGenerator;

        public PlanTemplateDetailsGenerator(ITemplateGenerator templateGenerator)
        {
            if (templateGenerator == null)
                throw new ArgumentNullException(nameof(templateGenerator));

            _templateGenerator = templateGenerator;
        }

        public async Task Generate(PublishPlanTemplateDTO publishPlanTemplateDto)
        {
            var pageName = publishPlanTemplateDto.Name + "-" + publishPlanTemplateDto.ParentPlanId + ".html";
            if (publishPlanTemplateDto.Description == null)
                publishPlanTemplateDto.Description = "";
            await _templateGenerator.Generate(new PlanTemplateDetailsTemplate(), pageName, new Dictionary<string, object>
            {
                ["planTemplate"] = publishPlanTemplateDto,
                ["planCreateUrl"] = CloudConfigurationManager.GetSetting("HubApiUrl").Replace("/api/v1/", "")
                        + "/dashboard/plans/" + publishPlanTemplateDto.ParentPlanId + "/builder?viewMode=plan"
            });
        }
    }
}