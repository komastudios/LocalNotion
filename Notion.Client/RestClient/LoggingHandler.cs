using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Notion.Client
{
    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Log.Trace("Request: {request}", request);

            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                Log.Trace("Response: {response}", response);

                return response;
            }
            catch (Exception ex)
            {
                // Trace only: RestClient retries transient failures and the caller reports
                // the final one, so an error here would flag requests that later succeed.
                Log.Trace("Failed to get response: {exception}", ex);

                throw;
            }
        }
    }
}
