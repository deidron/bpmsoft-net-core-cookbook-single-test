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
                        return await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.Error($"Server returned an error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is OperationCanceledException)
            {
                _logger.Error(ex);
                return null;
            }
        }
    }
}
