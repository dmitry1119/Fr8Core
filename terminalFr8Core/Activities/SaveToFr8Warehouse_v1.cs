﻿using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Fr8Data.Control;
using Fr8Data.Crates;
using Fr8Data.DataTransferObjects;
using Fr8Data.Managers;
using Fr8Data.Manifests;
using Fr8Data.States;
using StructureMap;
using TerminalBase.BaseClasses;
using TerminalBase.Infrastructure;

namespace terminalFr8Core.Activities
{
    public class SaveToFr8Warehouse_v1 : BaseTerminalActivity
    {
        public static ActivityTemplateDTO ActivityTemplateDTO = new ActivityTemplateDTO
        {
            Name = "SaveToFr8Warehouse",
            Label = "Save To Fr8 Warehouse",
            Category = ActivityCategory.Processors,
            Version = "1",
            MinPaneWidth = 330,
            WebService = TerminalData.WebServiceDTO,
            Terminal = TerminalData.TerminalDTO
        };
        protected override ActivityTemplateDTO MyTemplate => ActivityTemplateDTO;
        public SaveToFr8Warehouse_v1(ICrateManager crateManager)
            : base(false, crateManager)
        {
        }

        public override Task Run()
        {
            // get the selected event from the drop down
            var crateChooser = GetControl<UpstreamCrateChooser>("UpstreamCrateChooser");
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                var manifestTypes = crateChooser.SelectedCrates.Select(c => c.ManifestType.Value);
                var curCrates = Payload.CratesOfType<Manifest>().Where(c => manifestTypes.Contains(c.ManifestType.Id.ToString(CultureInfo.InvariantCulture)));
                //get the process payload
                foreach (var curCrate in curCrates)
                {
                    var curManifest = curCrate.Content;
                    uow.MultiTenantObjectRepository.AddOrUpdate(CurrentUserId, curManifest);
                }

                uow.SaveChanges();
                Success();
            }

            return Task.FromResult(0);
        }

        public override async Task Initialize()
        {
            var mergedUpstreamRunTimeObjects = await MergeUpstreamFields("Available Run-Time Objects");
            FieldDTO[] upstreamLabels = mergedUpstreamRunTimeObjects.Content.
                Fields.Select(field => new FieldDTO { Key = field.Key, Value = field.Value }).ToArray();
            var configControls = new StandardConfigurationControlsCM();
            configControls.Controls.Add(ControlHelper.CreateUpstreamCrateChooser("UpstreamCrateChooser", "Store which crates?"));
            var curConfigurationControlsCrate = PackControls(configControls);
            //TODO let's leave this like that until Alex decides what to do
            var upstreamLabelsCrate = CrateManager.CreateDesignTimeFieldsCrate("AvailableUpstreamLabels", new FieldDTO[] { });
            //var upstreamLabelsCrate = Crate.CreateDesignTimeFieldsCrate("AvailableUpstreamLabels", upstreamLabels);
            var upstreamDescriptions = await HubCommunicator.GetCratesByDirection<ManifestDescriptionCM>(ActivityId, CrateDirection.Upstream);
            var upstreamRunTimeDescriptions = upstreamDescriptions.Where(c => c.Availability == AvailabilityType.RunTime);
            var fields = upstreamRunTimeDescriptions.Select(c => new FieldDTO(c.Content.Name, c.Content.Id));
            var upstreamManifestsCrate = CrateManager.CreateDesignTimeFieldsCrate("AvailableUpstreamManifests", fields.ToArray());
            Storage.Clear();
            Storage.Add(curConfigurationControlsCrate);
            Storage.Add(upstreamLabelsCrate);
            Storage.Add(upstreamManifestsCrate);
        }

        public override Task FollowUp()
        {
            return Task.FromResult(0);
        }
    }
}