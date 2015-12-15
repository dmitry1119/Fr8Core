﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.Security;
using terminalQuickBooks.Interfaces;
using Utilities.Configuration.Azure;


namespace terminalQuickBooks.Services
{
    public class QuickBooksIntegration : IQuickBooksIntegration
    {
<<<<<<< HEAD
        private static readonly ConcurrentDictionary<string, string> TokenSecrets = new ConcurrentDictionary<string, string>();
        private const string TokenSeperator = ";;;;;;;";
        private static readonly string AccessToken = CloudConfigurationManager.GetSetting("QuickBooksRequestTokenUrl").ToString(CultureInfo.InvariantCulture);
        private const string AccessTokenSecret = "";
        private static readonly string ConsumerKey = CloudConfigurationManager.GetSetting("QuickBooksConsumerKey").ToString(CultureInfo.InvariantCulture);
        private static readonly string ConsumerSecret = CloudConfigurationManager.GetSetting("QuickBooksConsumerSecret").ToString(CultureInfo.InvariantCulture);
        private static readonly string AppToken = CloudConfigurationManager.GetSetting("QuickBooksAppToken").ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Build external QuickBooks OAuth url.
        /// </summary>
=======
>>>>>>> dev
        public string CreateAuthUrl()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetOAuthToken(string oauthToken, string oauthVerifier, string realmId)
        {
<<<<<<< HEAD
            var consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret,
                SignatureMethod = SignatureMethod.HmacSha1
            };
            return new OAuthSession(consumerContext,
                                   AccessToken,
                                    CloudConfigurationManager.GetSetting("QuickBooksOAuthAuthorizeUrl").ToString(CultureInfo.InvariantCulture),
                                    CloudConfigurationManager.GetSetting("QuickBooksOAuthAccessUrl").ToString(CultureInfo.InvariantCulture));
        }

        public async Task<string> GetOAuthToken(string oauthToken, string oauthVerifier, string realmId)
        {
            var oauthSession = CreateSession();
            string tokenSecret;
            TokenSecrets.TryRemove(oauthToken, out tokenSecret);

            IToken reqToken = new RequestToken
            {
                Token = oauthToken,
                TokenSecret = tokenSecret,
                ConsumerKey = CloudConfigurationManager.GetSetting("QuickBooksConsumerKey").ToString(CultureInfo.InvariantCulture)
            };
            var accToken = oauthSession.ExchangeRequestTokenForAccessToken(reqToken, oauthVerifier);

            return accToken.Token + TokenSeperator + accToken.TokenSecret + TokenSeperator + realmId;
        }

        public ServiceContext CreateServiceContext(string accessToken)
        {
            var tokens = accessToken.Split(new[] { TokenSeperator }, StringSplitOptions.None);
            var accToken = tokens[0];
            var accTokenSecret = tokens[1];
            var companyID = tokens[2];
            var oauthValidator = new OAuthRequestValidator(accToken, accTokenSecret, ConsumerKey, ConsumerSecret);
            return new ServiceContext(AppToken, companyID, IntuitServicesType.QBO, oauthValidator);
        }

        public OAuthRequestValidator getOAuthValidator()
        {
            return new OAuthRequestValidator(AccessToken,AccessTokenSecret,ConsumerKey,ConsumerSecret);
        }

=======
            throw new NotImplementedException();
        }
>>>>>>> dev
    }
}