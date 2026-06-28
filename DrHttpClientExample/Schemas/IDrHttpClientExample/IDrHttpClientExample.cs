namespace BPMSoft.Configuration.DrAcula
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDrHttpClientExample
    {
        string Execute(string endpoint);

        Task<string> ExecuteAsync(string endpoint);

        Task<string> ExecuteAsync(string endpoint, CancellationToken cancellationToken);

    }
}
