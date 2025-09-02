namespace TreeApi.Middleware
{
    public class DotRouteMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Processes the HTTP request and converts dot notation in paths to slash notation
        /// </summary>
        /// <param name="context">The HTTP context for the request</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            
            if (path != null && path.Contains('.'))
            {
                var lastDotIndex = path.LastIndexOf('.');
                if (lastDotIndex > 0)
                {
                    var newPath = $"{path[..lastDotIndex]}/{path[(lastDotIndex + 1)..]}";
                    context.Request.Path = newPath;
                }
            }

            await _next(context);
        }
    }

    public static class DotRouteMiddlewareExtensions
    {
        /// <summary>
        /// Adds the dot route middleware to the application pipeline
        /// </summary>
        /// <param name="builder">The application builder</param>
        /// <returns>The application builder for method chaining</returns>
        public static IApplicationBuilder UseDotRoute(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DotRouteMiddleware>();
        }
    }
}
