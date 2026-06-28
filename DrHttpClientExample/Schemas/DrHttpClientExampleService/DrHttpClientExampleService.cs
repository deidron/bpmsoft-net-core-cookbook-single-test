namespace BPMSoft.Configuration.DrAcula
{
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;
    using BPMSoft.Core.Factories;
    using BPMSoft.Web.Common;
    using BPMSoft.Web.Common.ServiceRouting;

    [ServiceContract]
    [DefaultServiceRoute, ServiceRoute("dr")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class DrHttpClientExampleService : BaseService
    {
        private IDrHttpClientExample DefaultHttpClient { get; } = ClassFactory.Get<IDrHttpClientExample>("Default");

        private IDrHttpClientExample AntiPatternHttpClient { get; } = ClassFactory.Get<IDrHttpClientExample>("AntiPattern");

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Task<string> TestAsync(string endpoint) =>
            DefaultHttpClient.ExecuteAsync(endpoint);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string Test(string endpoint) =>
            DefaultHttpClient.Execute(endpoint);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Task<string> TestAntiPatternAsync(string endpoint) =>
            AntiPatternHttpClient.ExecuteAsync(endpoint);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string TestAntiPattern(string endpoint) =>
            AntiPatternHttpClient.Execute(endpoint);
    }
}
