﻿using System.Collections.Generic;
using System.Linq;
using StructureMap;
using Data.Control;
using Data.Crates;
using Data.Interfaces.DataTransferObjects;
using Data.Interfaces.Manifests;
using Hub.Managers;

namespace TerminalBase.Infrastructure.Behaviors
{
    public class TextSourceMappingBehavior : BaseControlMappingBehavior<TextSource>
    {
        private readonly bool _reqeustUpstream;

        public TextSourceMappingBehavior(ICrateStorage crateStorage, string behaviorName, bool requestUpstream)
            : base(crateStorage, behaviorName)
        {
            _reqeustUpstream = requestUpstream;

            //BehaviorPrefix = "TextSourceMappingBehavior-";
        }

        public void Append(IEnumerable<string> fieldIds, string upstreamSourceLabel)
        {
            var controlsCM = GetOrCreateStandardConfigurationControlsCM();

            foreach (var fieldId in fieldIds)
            {
                var name = string.Concat(BehaviorPrefix, fieldId);

                var textSource = new TextSource(fieldId, upstreamSourceLabel, name);

                if (_reqeustUpstream)
                {
                    textSource.Source = new FieldSourceDTO()
                    {
                        ManifestType = CrateManifestTypes.StandardDesignTimeFields,
                        RequestUpstream = true
                    };
                }

                controlsCM.Controls.Add(textSource);
            }
        }

        public IDictionary<string, string> GetValues(ICrateStorage payload = null)
        {
            var controlsCM = GetOrCreateStandardConfigurationControlsCM();
            var result = new Dictionary<string, string>();

            var textSources = controlsCM
                .Controls
                .Where(IsBehaviorControl)
                .OfType<TextSource>();

            foreach (var textSource in textSources)
            {
                var fieldId = GetFieldId(textSource);
                string value = null;
                try
                {
                    value = textSource.GetValue(payload ?? _crateStorage);
                }
                catch { }
                result.Add(fieldId, value);
            }

            return result;
        }
    }
}
