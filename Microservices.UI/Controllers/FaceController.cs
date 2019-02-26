using Microservices.UI.Contracts;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]   
    public class FaceController : Controller
    {
        private readonly IRequisicaoService _requisicaoService;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly Guid RequesterId = Guid.NewGuid();
        private readonly ILogger _logger;

        public FaceController(IRequisicaoService requisicao, ILogger<FaceController> logger,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _configurationService = configurationService;
            _requisicaoService = requisicao;
            _policyRegistry = policyRegistry;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/Face")]
        public async Task<HttpResponseMessage> Index(FaceToPost face)
        {

            var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.BasicRetry) ?? Policy.NoOpAsync<HttpResponseMessage>();
            var context = new Context($"GetSomeData-{Guid.NewGuid()}", new Dictionary<string, object>
            {
                { PolicyContextItems.Logger, _logger }, { "url", Request.GetUri() }
            });

            var response = await retryPolicy.ExecuteAsync((ctx) =>
            {
                return _requisicaoService.PostAsync(new UserToPostMoc(), Request.GetUri(), "User");
            },context);

            //return Ok(new FaceToProcessing());
            return response;
        }
    }
}