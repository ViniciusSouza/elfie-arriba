using Arriba.Model;
using Arriba.Model.Correctors;
using Arriba.ParametersCheckers;
using Arriba.Server.Authentication;

namespace Arriba.Communication.Server.Application
{
    public class ArribaManagementServiceFactory
    {
        private const string Table_People = "People";

        private readonly SecureDatabase secureDatabase;
        private readonly ClaimsAuthenticationService _claimsAuth;

        public ArribaManagementServiceFactory(SecureDatabase secureDatabase, ClaimsAuthenticationService claims)
        {
            ParamChecker.ThrowIfNull(secureDatabase, nameof(secureDatabase));
            ParamChecker.ThrowIfNull(claims, nameof(claims));

            this.secureDatabase = secureDatabase;
            _claimsAuth = claims;
        }

        private ComposedCorrector GetComposedCorrectors(ref string userAliasCorrectorTable)
        {
            if (string.IsNullOrWhiteSpace(userAliasCorrectorTable))
                userAliasCorrectorTable = Table_People;

            var correctors = new ComposedCorrector(new TodayCorrector(), new UserAliasCorrector(secureDatabase[userAliasCorrectorTable]));
            return correctors;
        }

        public IArribaManagementService CreateArribaManagementService(string userAliasCorrectorTable = "")
        {
            var correctors = GetComposedCorrectors(ref userAliasCorrectorTable);

            return new ArribaManagementService(secureDatabase, correctors, _claimsAuth);
        }

        public IArribaQueryServices CreateArribaQueryService(string userAliasCorrectorTable = "")
        {
            var correctors = GetComposedCorrectors(ref userAliasCorrectorTable);

            return new ArribaQueryServices(secureDatabase, correctors, _claimsAuth);
        }

    }
}