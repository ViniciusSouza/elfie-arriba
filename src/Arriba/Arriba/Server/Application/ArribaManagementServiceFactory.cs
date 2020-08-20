using Arriba.Model;
using Arriba.Model.Correctors;
using Arriba.ParametersCheckers;
using Arriba.Server.Authentication;
using Arriba.Server.Hosting;

namespace Arriba.Communication.Server.Application
{
    public class ArribaManagementServiceFactory
    {
        private const string Table_People = "People";

        private readonly SecureDatabase secureDatabase;
        private readonly DatabaseFactory factory;
        private readonly ClaimsAuthenticationService _claimsAuth;

        public ArribaManagementServiceFactory(DatabaseFactory factory, ClaimsAuthenticationService claims)
        {
            ParamChecker.ThrowIfNull(factory, nameof(factory));
            ParamChecker.ThrowIfNull(claims, nameof(claims));

            this.factory = factory;
            this.secureDatabase = factory.GetDatabase();
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

            return new ArribaQueryServices(factory, correctors, _claimsAuth);
        }

    }
}