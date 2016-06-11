﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Fr8.Infrastructure.Data.DataTransferObjects;
using Fr8.Infrastructure.Interfaces;
using Fr8.TerminalBase.BaseClasses;
using terminalDropbox.Infrastructure;

namespace terminalDropbox.Controllers
{
    [RoutePrefix("authentication")]
    public class AuthenticationController : BaseTerminalController
    {
        private const string curTerminal = "terminalDropbox";
        private Authentication _authentication = new Authentication();

        public AuthenticationController(IRestfulServiceClient restfulServiceClient) 
            : base(restfulServiceClient)
        {
        }

        [HttpPost]
        [Route("request_url")]
        public ExternalAuthUrlDTO GenerateOAuthInitiationURL()
        {
            return _authentication.GetExternalAuthUrl();
        }

        [HttpPost]
        [Route("token")]
        public async Task<AuthorizationTokenDTO> GenerateOAuthToken(ExternalAuthenticationDTO externalAuthDTO)
        {
            try
            {
                return await _authentication.Authenticate(externalAuthDTO);
            }
            catch (Exception ex)
            {
                ReportTerminalError(curTerminal, ex,externalAuthDTO.Fr8UserId);
                return await Task.FromResult(
                    new AuthorizationTokenDTO()
                    {
                        Error = "An error occurred while trying to authorize, please try again later."
                    }
                );
            }
        }
    }
}