﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Data.Entities;
using Web.ViewModels;
using Web.ViewModels.JsonConverters;
using Utilities.AutoMapper;
using Data.Interfaces.DataTransferObjects;

namespace Web.App_Start
{
    public class AutoMapperBootStrapper
    {

        public static void ConfigureAutoMapper()
        {

            Mapper.CreateMap<DockyardAccountDO, ManageUserVM>()
                .ForMember(mu => mu.HasLocalPassword, opts => opts.ResolveUsing(account => !string.IsNullOrEmpty(account.PasswordHash)))
                .ForMember(mu => mu.HasDocusignToken, opts => opts.Ignore())
                .ForMember(mu => mu.HasGoogleToken, opts => opts.Ignore())
                .ForMember(mu => mu.GoogleSpreadsheets, opts => opts.Ignore());

            Mapper.CreateMap<ActionNameDTO, ActivityTemplateDO>()
                  .ForMember(activityTemplateDO => activityTemplateDO.Name, opts => opts.ResolveUsing(e => e.Name))
                  .ForMember(activityTemplateDO => activityTemplateDO.Version, opts => opts.ResolveUsing(e => e.Version));
             
            Mapper.CreateMap<ProcessTemplateDTO, ProcessTemplateDO>();
            Mapper.CreateMap<ProcessTemplateDO, ProcessTemplateDTO>();
            Mapper.CreateMap<UserVM, EmailAddressDO>()
                .ForMember(userDO => userDO.Address, opts => opts.ResolveUsing(e => e.EmailAddress));
            Mapper.CreateMap<UserVM, DockyardAccountDO>()
                .ForMember(userDO => userDO.Id, opts => opts.ResolveUsing(e => e.Id))
                .ForMember(userDO => userDO.FirstName, opts => opts.ResolveUsing(e => e.FirstName))
                .ForMember(userDO => userDO.LastName, opts => opts.ResolveUsing(e => e.LastName))
                .ForMember(userDO => userDO.UserName, opts => opts.ResolveUsing(e => e.UserName))
                .ForMember(userDO => userDO.EmailAddress, opts => opts.ResolveUsing(e => new EmailAddressDO {Address = e.EmailAddress}))
                .ForMember(userDO => userDO.Roles, opts => opts.Ignore());

            Mapper.CreateMap<ActionDO, ActionDTO>();
          
        }
    }
}
//Missing type map configuration or unsupported mapping.

//Mapping types:
//UserVM -> EmailAddressDO
//Web.ViewModels.UserVM -> Data.Entities.EmailAddressDO

//Destination path:
//DockyardAccountDO

//Source value:
//Web.ViewModels.UserVM