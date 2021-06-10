using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IO;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Architecture
{
    public class RedisMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _connection;

        public RedisMiddleware(RequestDelegate next, IConnectionMultiplexer connection)
        {
            this._next = next;
            this._connection = connection;
        }

        public async Task Invoke(HttpContext context)
        {
            await CheckRedisDatabaseAsync(context);
        }

        private async Task CheckRedisDatabaseAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            if (context.Request.Method == "GET")
            {
                var redisDatabase = _connection.GetDatabase();
                var key = string.Format("{0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(context.Request.Path.Value)));
                var value = await redisDatabase.StringGetAsync(key);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    var response = context.Response.WriteAsync(value);
                    await _next.Invoke(context);
                }
                else
                {
                    var originalBodyStream = context.Response.Body;

                    try
                    {
                        var memoryBodyStream = new MemoryStream();
                        context.Response.Body = memoryBodyStream;

                        await _next.Invoke(context);

                        memoryBodyStream.Seek(0, SeekOrigin.Begin);
                        string body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
                        await redisDatabase.StringSetAsync(key, body, expiry: TimeSpan.FromMinutes(10));
                        memoryBodyStream.Seek(0, SeekOrigin.Begin);

                        await memoryBodyStream.CopyToAsync(originalBodyStream);
                    }
                    finally
                    {
                        context.Response.Body = originalBodyStream;
                        await _next.Invoke(context);
                    }
                }
                return;
            }
            await _next.Invoke(context);
        }
    }
}
