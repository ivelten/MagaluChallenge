using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;

namespace Magalu.Challenge.Infrastructure.Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            var requestInfo = await FormatRequest(context.Request);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                var response = await FormatResponse(context.Response);

                var log = new RequestResponseLog
                {
                    RequestUrl = requestInfo.Url,
                    RequestBody = requestInfo.Body,
                    RemoteAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ResponseBody = response
                };

                var logRepository = unitOfWork.GetRepository<RequestResponseLog>();

                await logRepository.InsertAsync(log);
                await unitOfWork.SaveChangesAsync();

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<RequestInfo> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = body;

            return new RequestInfo
            {
                Url = $"{request.Method}: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Body = string.IsNullOrWhiteSpace(bodyAsText) ? null : bodyAsText
            };
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return string.IsNullOrWhiteSpace(text) ? null : $"{response.StatusCode}: {text}";
        }
    }
}
