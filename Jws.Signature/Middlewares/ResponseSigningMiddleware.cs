using Jws.Signature.Jws;
using Microsoft.AspNetCore.Http;

namespace Jws.Signature.Middlewares;

public sealed class ResponseSigningMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMakeJwsService _makeJwsService;

    public ResponseSigningMiddleware(
        RequestDelegate next,
        IMakeJwsService makeJwsService)
    {
        _next = next;
        _makeJwsService = makeJwsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var responseBodyStream = new MemoryStream();
        var bodyStream = context.Response.Body;

        try
        {
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

            responseBody = _makeJwsService.MakeJws(responseBody);

            using var newStream = new MemoryStream();

            var sw = new StreamWriter(newStream);
            sw.Write(responseBody);
            sw.Flush();

            newStream.Seek(0, SeekOrigin.Begin);

            await newStream.CopyToAsync(bodyStream);
        }
        finally
        {
            context.Response.Body = bodyStream;
        }
    }
}
