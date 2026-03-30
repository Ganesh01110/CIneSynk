using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace TicketBooking.Api.Middleware
{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public IdempotencyMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only apply idempotency to POST requests for bookings/reservations
            if (context.Request.Method != HttpMethods.Post || !context.Request.Path.Value!.Contains("api/bookings"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-Idempotency-Key", out var idempotencyKey))
            {
                await _next(context);
                return;
            }

            var cacheKey = $"Idempotency_{idempotencyKey}";

            if (_cache.TryGetValue(cacheKey, out string? cachedResponse))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse!);
                return;
            }

            // Capture the response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Cache successful responses for 1 hour
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                _cache.Set(cacheKey, responseBodyText, TimeSpan.FromHours(1));
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
