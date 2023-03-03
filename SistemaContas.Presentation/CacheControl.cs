namespace SistemaContas.Presentation
{
    public class CacheControl
    {
        private readonly RequestDelegate? _requestDelegate;
      
        public CacheControl(RequestDelegate? requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
      
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.OnStarting((state) =>
            {
                httpContext.Response.Headers.Append("Cache-Control",
                "no-cache, no-store, must-revalidate");

                httpContext.Response.Headers.Append("Pragma", "no-cache");
                httpContext.Response.Headers.Append("Expires", "0");
                return Task.FromResult(0);
            }, null);
            await _requestDelegate.Invoke(httpContext);
        }
    }
}
