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
using System.Security.Cryptography;

namespace WxHandle.Core
{
    public class WxMiddleware
    {
        private readonly RequestDelegate _next;


        public WxMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public string Sha1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public async Task Invoke(HttpContext context, IOptions<WxConfig> options, IWxPayHandle wxHandle)
        {
            var config = options.Value;
            if (context.Request.Path.HasValue && context.Request.Method == "POST" && context.Request.Path.Value == config.Pay_Notify_Url)
            {
                var xmlHelp = context.RequestServices.GetService<WxXmlHelp>();
                var cbModel = await xmlHelp.ReadFromStream<PayCallbackData>(context.Request.Body);
                await wxHandle.PayCallback(cbModel);
                await context.Response.WriteAsync(@"<xml>
  <return_code><![CDATA[SUCCESS]]></return_code>
  <return_msg><![CDATA[OK]]></return_msg>
</xml>");
            }
            else if (context.Request.Path.HasValue && context.Request.Path.Value == config.Wx_Notify_Url)
            {
                if (context.Request.Method == "GET")
                {
                    string signature = string.Empty, timestamp = string.Empty, nonce = string.Empty,
                        echostr = string.Empty, token = options.Value.Token;

                    if (context.Request.Query.TryGetValue("signature", out var signatures) && signatures.Count > 0)
                        signature = signatures[0];

                    if (context.Request.Query.TryGetValue("timestamp", out var timestamps) && timestamps.Count > 0)
                        timestamp = timestamps[0];

                    if (context.Request.Query.TryGetValue("nonce", out var nonces) && nonces.Count > 0)
                        nonce = nonces[0];

                    if (context.Request.Query.TryGetValue("echostr", out var echostrs) && echostrs.Count > 0)
                        echostr = echostrs[0];

                    var list = new SortedSet<string>() { token, timestamp, nonce };
                    var signStr = string.Join("", list);
                    signStr = Sha1(signStr);

                    if (signStr.ToLower() == signature)
                        await context.Response.WriteAsync(echostr);
                }
            }
            else
            {
                await this._next(context);
            }
        }
    }
}
