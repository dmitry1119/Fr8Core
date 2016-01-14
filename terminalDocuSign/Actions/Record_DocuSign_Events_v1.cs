﻿using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.Control;
using Data.Crates;
using Data.Interfaces;
using Data.Interfaces.DataTransferObjects;
using Data.Interfaces.Manifests;
using Hub.Managers;
using StructureMap;
using TerminalBase.Infrastructure;
using terminalDocuSign.Infrastructure;
using TerminalBase.BaseClasses;
using Data.Entities;
using Utilities.Configuration.Azure;

namespace terminalDocuSign.Actions
{
    public class Record_DocuSign_Events_v1 : BaseTerminalAction
    {
        /// <summary>
        /// //For this action, both Initial and Followup configuration requests are same. Hence it returns Initial config request type always.
        /// </summary>
        /// <param name="curActionDO"></param>
        /// <returns></returns>
        public override async Task<ActionDO> Configure(ActionDO curActionDO, AuthorizationTokenDO authTokenDO)
        {
            CheckAuthentication(authTokenDO);

            return await ProcessConfigurationRequest(curActionDO, x => ConfigurationRequestType.Initial,authTokenDO);
        }

        protected override async Task<ActionDO> InitialConfigurationResponse(ActionDO curActionDO, AuthorizationTokenDO authTokenDO)
        {
            /*
             * Discussed with Alexei and it is required to have empty Standard UI Control in the crate.
             * So we create a text block which informs the user that this particular aciton does not require any configuration.
             */
            var textBlock = GenerateTextBlock("Monitor All DocuSign events",
                "This Action doesn't require any configuration.", "well well-lg");
            var curControlsCrate = PackControlsCrate(textBlock);

            //create a Standard Event Subscription crate
            var curEventSubscriptionsCrate = Crate.CreateStandardEventSubscriptionsCrate("Standard Event Subscription", DocuSignEventNames.GetAllEventNames());

            //create Standard Design Time Fields for Available Run-Time Objects
            var curAvailableRunTimeObjectsDesignTimeCrate =
                Crate.CreateDesignTimeFieldsCrate("Available Run-Time Objects", new FieldDTO[]
                {
                    new FieldDTO {Key = "DocuSign Envelope", Value = string.Empty},
                    new FieldDTO {Key = "DocuSign Event", Value = string.Empty}
                });

            using (var updater = Crate.UpdateStorage(curActionDO))
            {
                updater.CrateStorage = new CrateStorage(curControlsCrate, curEventSubscriptionsCrate, curAvailableRunTimeObjectsDesignTimeCrate);
            }

            /*
             * Note: We should not call Activate at the time of Configuration. For this action, it may be valid use case.
             * Because this particular action will be used internally, it would be easy to execute the Process directly.
             */
            await Activate(curActionDO, null);

            return await Task.FromResult(curActionDO);
        }

        public override Task<ActionDO> Activate(ActionDO curActionDO, AuthorizationTokenDO authTokenDO)
        {
            DocuSignAccount curDocuSignAccount = new DocuSignAccount();
            var curConnectProfile = curDocuSignAccount.GetDocuSignConnectProfiles();
            try {
                if (curConnectProfile.configurations != null &&
                    !curConnectProfile.configurations.Any(config => !string.IsNullOrEmpty(config.name) && config.name.Equals("MonitorAllDocuSignEvents")))
                {
                    var monitorConnectConfiguration = new DocuSign.Integrations.Client.Configuration
                    {
                        allowEnvelopePublish = "true",
                        allUsers = "true",
                        enableLog = "true",
                        requiresAcknowledgement = "true",
                        envelopeEvents = string.Join(",", DocuSignEventNames.GetEventsFor("Envelope")),
                        recipientEvents = string.Join(",", DocuSignEventNames.GetEventsFor("Recipient")),
                        name = "MonitorAllDocuSignEvents",
                        urlToPublishTo =
                            Regex.Match(CloudConfigurationManager.GetSetting("TerminalEndpoint"), @"(\w+://\w+:\d+)").Value +
                            "/terminals/terminalDocuSign/events"
                    };

                    curDocuSignAccount.CreateDocuSignConnectProfile(monitorConnectConfiguration);
                }
            }
            catch(Exception ex)
            {

            }
            return Task.FromResult<ActionDO>(curActionDO);
        }

        public override Task<ActionDO> Deactivate(ActionDO curActionDO)
        {
            DocuSignAccount curDocuSignAccount = new DocuSignAccount();
            var curConnectProfile = curDocuSignAccount.GetDocuSignConnectProfiles();

            if (Int32.Parse(curConnectProfile.totalRecords) > 0 && curConnectProfile.configurations.Any(config => !string.IsNullOrEmpty(config.name) && config.name.Equals("MonitorAllDocuSignEvents")))
            {
                var monitorAllDocuSignEventsId = curConnectProfile.configurations.Where(config => !string.IsNullOrEmpty(config.name) && config.name.Equals("MonitorAllDocuSignEvents")).Select(s => s.connectId);
                foreach (var connectId in monitorAllDocuSignEventsId)
                {
                    curDocuSignAccount.DeleteDocuSignConnectProfile(connectId);
                }
            }

            return Task.FromResult<ActionDO>(curActionDO);
        }

        public async Task<PayloadDTO> Run(ActionDO actionDO, Guid containerId, AuthorizationTokenDO authTokenDO)
        {
            var curProcessPayload = await GetPayload(actionDO, containerId);

            if (NeedsAuthentication(authTokenDO))
            {
                return NeedsAuthenticationError(curProcessPayload);
            }

            var curEventReport = Crate.GetStorage(curProcessPayload).CrateContentsOfType<EventReportCM>().First();

            if (curEventReport.EventNames.Contains("Envelope"))
            {
                var docuSignFields = curEventReport.EventPayload.CrateContentsOfType<StandardPayloadDataCM>().First();

                var envelope = new DocuSignEnvelopeCM
                {
                    CompletedDate = docuSignFields.GetValueOrDefault("CompletedDate"), //.First(field => field.Key.Equals("CompletedDate")).Value,
                    CreateDate = docuSignFields.GetValueOrDefault("CreateDate"),//.First(field => field.Key.Equals("CreateDate")).Value,
                    DeliveredDate = docuSignFields.GetValueOrDefault("DeliveredDate"),//First(field => field.Key.Equals("DeliveredDate")).Value,
                    EnvelopeId = docuSignFields.GetValueOrDefault("EnvelopeId"),//First(field => field.Key.Equals("EnvelopeId")).Value,
                    ExternalAccountId = docuSignFields.GetValueOrDefault("HolderEmail"),//First(field => field.Key.Equals("Email")).Value,
                    SentDate = docuSignFields.GetValueOrDefault("SentDate"),//First(field => field.Key.Equals("SentDate")).Value,
                    Status = docuSignFields.GetValueOrDefault("Status"),//First(field => field.Key.Equals("Status")).Value
                };

                var events = new DocuSignEventCM
                {
                    EnvelopeId = docuSignFields.GetValueOrDefault("EnvelopeId"),//First(field => field.Key.Equals("EnvelopeId")).Value,
                    EventId = docuSignFields.GetValueOrDefault("EventId"),//First(field => field.Key.Equals("EventId")).Value,
                    Object = docuSignFields.GetValueOrDefault("Object"),//First(field => field.Key.Equals("Object")).Value,
                    RecepientId = docuSignFields.GetValueOrDefault("RecipientId"),//First(field => field.Key.Equals("RecipientId")).Value,
                    Status = docuSignFields.GetValueOrDefault("Status"),//First(field => field.Key.Equals("Status")).Value,
                    ExternalAccountId = docuSignFields.GetValueOrDefault("HolderEmail"),//First(field => field.Key.Equals("Email")).Value
                };

                using (var updater = Crate.UpdateStorage(curProcessPayload))
                {
                    updater.CrateStorage.Add(Data.Crates.Crate.FromContent("DocuSign Envelope Manifest", envelope));
                    updater.CrateStorage.Add(Data.Crates.Crate.FromContent("DocuSign Event Manifest", events));
                }
            }

            return Success(curProcessPayload);
        }
    }
}