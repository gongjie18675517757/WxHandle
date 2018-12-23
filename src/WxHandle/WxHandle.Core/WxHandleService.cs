using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WxHandle.Core.Exceptions;
using WxHandle.Core.Interfaces;
using WxHandle.Core.Models;
using WxHandle.Core.Options;

namespace WxHandle.Core
{
    public class WxHandleService : IWxHandle
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<WxConfig> options;
        private readonly WxXmlHelp xmlHelp;

        public WxHandleService(IHttpClientFactory httpClientFactory, IOptions<WxConfig> options, WxXmlHelp xmlHelp)
        {
            this.httpClientFactory = httpClientFactory;
            this.options = options;
            this.xmlHelp = xmlHelp;
        }
        Task<IWxResult> IWxHandle.PayCallback(PayCallbackData wxResult)
        {
            throw new System.NotImplementedException();
        }

        async Task<SendPayResult> IWxHandle.SendPay(SendPayInput input)
        {
            input.appid = input.appid ?? options.Value.AppId;
            input.mch_id = input.mch_id ?? options.Value.Mch_Id;
            input.notify_url = input.notify_url ?? options.Value.Pay_Notify_Url;

            /*验证参数有效性*/
            if (!input.Verify(out var errs))
                throw new VerifyException("发起支付错误:参数不正确", errs);

            /*签名*/
            input.sign = xmlHelp.CreateSign(input);

            /*转成xml*/
            var xml = xmlHelp.WriteToXml(input);

            /*发送请求*/
            var httpResponseMessage = await httpClientFactory.CreateClient().PostAsync(options.Value.Server.SendPayAddress, new StringContent(xml, Encoding.UTF8, "text/xml"));

            /*响应*/
            var result = xmlHelp.ReadFromXml<SendPayResult>(await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }
    }
}
