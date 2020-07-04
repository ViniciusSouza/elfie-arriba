using System;
using System.Collections.Generic;
using System.Text;

namespace Arriba.Configuration
{
    public class OAuthConfig : IOAuthConfig
    {
        public OAuthConfig()
        {
            this.RedirectUrl = "http://localhost:42784/api/oauth/auth-code";
            this.TenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";
            this.AudienceId = "ae4ffb67-b66b-4b8f-907b-9e64e5249199";
            this.Prompt = "login";
            this.Scopes = new[] { "openid", $"{this.AudienceId}/.default" };
        }

        public string TenantId { get; }

        public string AudienceId { get; }

        public string RedirectUrl { get; }

        public IList<string> Scopes { get; }

        public string Prompt { get; }

        public string AppSecret { get; }
    }
}
