﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Fr8.Infrastructure.Data.DataTransferObjects;
using Fr8.Infrastructure.Data.Managers;
using Moq;
using terminalDocuSign;
using terminalDocuSign.Services.New_Api;
using terminalDocuSign.Activities;

namespace terminalDocuSignTests.Fixtures
{
    public class BaseDocusignActivityMock : BaseDocuSignActivity
    {
        public static ActivityTemplateDTO ActivityTemplateDTO = new ActivityTemplateDTO
        {
            Version = "1",
            Name = "BaseDocusignActivityMock",
            Label = "BaseDocusignActivityMock",
            NeedsAuthentication = true,
            MinPaneWidth = 330,
            WebService = TerminalData.WebServiceDTO,
            Terminal = TerminalData.TerminalDTO
        };
        protected override ActivityTemplateDTO MyTemplate => ActivityTemplateDTO;
        protected override string ActivityUserFriendlyName { get; }


        public BaseDocusignActivityMock(ICrateManager crateManager, IDocuSignManager docuSignManager) 
            : base(crateManager, docuSignManager)
        {
        }

        protected override Task RunDS()
        {
            return Task.FromResult(0);
        }

        protected override Task InitializeDS()
        {
            return Task.FromResult(0);
        }

        protected override Task FollowUpDS()
        {
            return Task.FromResult(0);
        }
    }

    public static partial class DocuSignActivityFixtureData
    {
       
        public static IDocuSignManager DocuSignManagerWithTemplates()
        {
            var result = new Mock<IDocuSignManager>();
            result.Setup(x => x.GetTemplatesList(It.IsAny<DocuSignApiConfiguration>()))
                  .Returns(new List<FieldDTO> { new FieldDTO("1", "First") });
            return result.Object;
        }

        public static IDocuSignManager DocuSignManagerWithoutTemplates()
        {
            var result = new Mock<IDocuSignManager>();
            result.Setup(x => x.GetTemplatesList(It.IsAny<DocuSignApiConfiguration>()))
                  .Returns(new List<FieldDTO>());
            return result.Object;
        }
    }
}