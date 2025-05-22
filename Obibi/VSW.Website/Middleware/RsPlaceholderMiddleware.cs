using VSW.Website.Interface;

namespace VSW.Website.Middleware
{
    public class RsPlaceholderMiddleware
    {
        private readonly RequestDelegate _next;

        public RsPlaceholderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IResourceServiceInterface parser)
        {
            var originalBody = context.Response.Body;

            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context); // Gọi controller & view

            memStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memStream);
            var html = await reader.ReadToEndAsync();

            context.Response.Body = originalBody;

            try
            {
                // Chỉ xử lý nếu là HTML
                if (context.Response.ContentType != null &&
                    context.Response.ContentType.Contains("text/html", StringComparison.OrdinalIgnoreCase))
                {
                    html = await parser.ParseAsync(html, context);
                }

                await context.Response.WriteAsync(html);
            }
            catch (Exception ex)
            {
                // Reset response để tránh lỗi conflict headers
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 302;
                    context.Response.Redirect("/Home/Error?msg=RSParser");
                }
                else
                {
                    // Nếu response đã bắt đầu, đành phải chèn lỗi nhẹ
                    await context.Response.WriteAsync("<div style='color:red;'>RS Error occurred.</div>");
                }
            }
        }
    }
}
