using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WxHandle.Core.Models;
using WxHandle.Core.Options;
using WxHandle.Core.Interfaces;

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

        public async Task Invoke(HttpContext context, IOptions<WxConfig> options, IWxHandle wxHandle)
        {
            var config = options.Value;
            if (context.Request.Path.HasValue)
            {
                if (context.Request.Method == "POST" && context.Request.Path.Value == config.Pay_Notify_Url)
                {
                    var xmlHelp = context.RequestServices.GetService<WxXmlHelp>();
                    var cbModel = await xmlHelp.ReadFromStream<PayCallbackData>(context.Request.Body);
                    var result = await wxHandle.PayCallback(cbModel);
                    var xml = xmlHelp.WriteToXml(result);
                    await context.Response.WriteAsync(xml);
                }
            }
            else
            {
                await this._next(context);
            }
        }
    }
}
