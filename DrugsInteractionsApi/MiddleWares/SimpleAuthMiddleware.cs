namespace DrugsInteractionsApi.MiddleWares
{
    public class SimpleAuthMiddleware
    {
        private readonly RequestDelegate next;

        int dummyRequestsCount = 0;

        DateTime lastDummyRequestTime = DateTime.Now;

        public SimpleAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var validKey = "[in production code there is valid key]";
                if (!context.Request.Headers.Keys.Contains("Authorization"))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Auth key must be provided");
                    return;
                }
                if(context.Request.Headers.Authorization.First() != validKey && context.Request.Headers.Authorization.First() != "dummy-key")
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Invalid auth key");
                    return;
                }
                if (DateTime.Now.Subtract(lastDummyRequestTime).TotalDays >= 1)
                {
                    dummyRequestsCount = 0;
                }
                if (context.Request.Headers.Authorization.First() == "dummy-key")
                {
                    dummyRequestsCount++;
                    lastDummyRequestTime = DateTime.Now;
                    if (dummyRequestsCount >= 100) 
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Dummy requests is out");
                        return;
                    } 
                }
            }

            await next.Invoke(context);
        }
    }
}
