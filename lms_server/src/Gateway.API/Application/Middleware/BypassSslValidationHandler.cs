namespace Gateway.API.Application.Middleware;
public class BypassSslValidationHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request?.RequestUri?.Scheme == Uri.UriSchemeHttps)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);

            // Re-send request with HttpClient with bypass SSL
            var newRequest = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content,
                Version = request.Version
            };
            foreach (var header in request.Headers)
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await httpClient.SendAsync(newRequest, cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}