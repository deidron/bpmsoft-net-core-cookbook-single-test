namespace BPMSoft.Configuration.DrAcula
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using BPMSoft.Common.Threading;
    using BPMSoft.Core.Factories;
    using global::Common.Logging;

    [DefaultBinding(typeof(IDrHttpClientExample), Name = "Default")]
    public class DrHttpClientExample(IHttpClientFactory httpClientFactory) : IDrHttpClientExample
    {

        readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        private readonly ILog _logger = LogManager.GetLogger(nameof(DrHttpClientExample));

        public string Execute(string endpoint) =>
            AsyncPump.Run(async () => await ExecuteAsync(endpoint));

        public Task<string> ExecuteAsync(string endpoint) =>
            ExecuteAsync(endpoint, CancellationToken.None);

        public async Task<string> ExecuteAsync(string endpoint, CancellationToken cancellationToken)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
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
