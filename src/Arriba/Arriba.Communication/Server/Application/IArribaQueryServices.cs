using Arriba.Model.Query;
using System.Collections.Specialized;
using System.Security.Principal;

namespace Arriba.Communication.Server.Application
{
    public interface IArribaQueryServices
    {
        SelectResult QueryTableForUser(string tableName, NameValueCollection parameters, ITelemetry telemetry, IPrincipal user);

        DistinctResult DistinctQueryTableForUser(string tableName, NameValueCollection parameters, ITelemetry telemetry, IPrincipal user);

        AggregationResult AggregateQueryTableForUser(string tableName, NameValueCollection parameters, ITelemetry telemetry, IPrincipal user);

        IntelliSenseResult IntelliSenseTableForUser(NameValueCollection parameters, ITelemetry telemetry, IPrincipal user);

        AllCountResult AllCountForUser(ITelemetry telemetry, NameValueCollection parameters, IPrincipal user);
    }
}
