using Microsoft.AspNetCore.Http;
using School_Administration.Data.Logs;
using System.IO;
using System;
using System.Threading.Tasks;

namespace School_Administration.Data.Logging
{
    public class Logger
    {
        RequestDelegate next;

        public Logger(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerRepo repo)
        {
            //Request handling comes here

            // create a new log object
            var log = new Log
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.ToString()
            };

            // check if the Request is a POST call 
            // since we need to read from the body
            if (context.Request.Method == "POST")
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body)
                                                    .ReadToEndAsync();
                context.Request.Body.Position = 0;
                log.Payload = body;
            }

            log.RequestedOn = DateTime.Now;
            //await next.Invoke(context);

            using (Stream originalRequest = context.Response.Body)
            {
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;

                        await next.Invoke(context);

                        memStream.Position = 0;
                        
                        var response = await new StreamReader(memStream)
                                                                .ReadToEndAsync();
                        log.Response = response;

                        log.ResponseCode = context.Response.StatusCode.ToString();

                        log.IsSuccessStatusCode = (
                              context.Response.StatusCode == 200 ||
                              context.Response.StatusCode == 201);

                        log.RespondedOn = DateTime.Now;

                        repo.AddToLogs(log);

                        memStream.Position = 0;

                        await memStream.CopyToAsync(originalRequest);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    // assign the response body to the actual context
                    context.Response.Body = originalRequest;
                }
            }
        }
    }
}
