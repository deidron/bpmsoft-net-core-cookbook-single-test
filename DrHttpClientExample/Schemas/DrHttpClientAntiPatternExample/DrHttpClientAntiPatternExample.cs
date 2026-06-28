namespace BPMSoft.Configuration.DrAcula
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using BPMSoft.Common.Threading;
    using BPMSoft.Core.Factories;
    using global::Common.Logging;

    [Obsolete("Anti-pattern! Leads to socket exhaustion in TIME_WAIT. Use DrHttpClientExample instead.")]
    [DefaultBinding(typeof(IDrHttpClientExample), Name = "AntiPattern")]
    public class DrHttpClientAntiPatternExample : IDrHttpClientExample
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(DrHttpClientAntiPatternExample));

        public Task<string> ExecuteAsync(string endpoint) =>
            ExecuteAsync(endpoint, CancellationToken.None);

        public string Execute(string endpoint) =>
            AsyncPump.Run(() => ExecuteAsync(endpoint));

        public async Task<string> ExecuteAsync(string endpoint, CancellationToken cancellationToken)
        {
            try
            {
                // Error: HttpClient is recreated and disposed on every call
                using (var client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
#if NET5_0_OR_GREATER
                        return await response.Content.ReadAsStringAsync(cancellationToken);
#else
                        return await response.Content.ReadAsStringAsync();
#endif
                    _logger.Error($"Server returned an error {response.StatusCode} for {endpoint}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.Error($"Request to {endpoint} failed", ex);
                return null;
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.Error($"Request to {endpoint} timed out");
                return null;
            }
        }
    }
}
