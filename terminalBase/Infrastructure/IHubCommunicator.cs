﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Crates;
using Data.Entities;
using Data.Interfaces.DataTransferObjects;
using Data.States;
using System.IO;

namespace TerminalBase.Infrastructure
{
    public interface IHubCommunicator
    {
        Task<PayloadDTO> GetProcessPayload(ActionDO actionDO, Guid containerId);

        Task<List<Crate<TManifest>>> GetCratesByDirection<TManifest>(ActionDO actionDO, CrateDirection direction);

        Task<List<Crate>> GetCratesByDirection(ActionDO actionDO, CrateDirection direction);

        Task CreateAlarm(AlarmDTO alarmDTO);

        Task<FileDO> SaveFile(string name, Stream stream);

        Task<List<ActivityTemplateDTO>> GetActivityTemplates(ActionDO actionDO);

        Task<List<ActivityTemplateDTO>> GetActivityTemplates(
            ActionDO actionDO, ActivityCategory category);

        Task<List<ActivityTemplateDTO>> GetActivityTemplates(
            ActionDO actionDO, string tag);
    }
}
