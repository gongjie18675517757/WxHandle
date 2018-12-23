using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WxHandle.Core
{

    public class WxMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string path;

        public WxMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            this.path = path;
        } 

        public async Task Invoke(HttpContext context, IOptions<WxConfig> options, IWxHandle wxHandle,WxXmlHelp xmlHelp)
        {
            var config = options.Value;
            if (context.Request.Path.HasValue)
            {
                if (context.Request.Method == "POST" && context.Request.Path.Value == config.PayCallbackPath)
                {
                    var cbModel = await xmlHelp.ReadAsXmlFromBody<PayCallbackData>(context.Request.Body);
                    var result = await wxHandle.PayCallback(cbModel);
                    await context.Response.WriteAsync(result);                   
                }
            }
            else
            {
                await this._next(context);
            }
        }
    }
}
