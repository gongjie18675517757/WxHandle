using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using WxHandle.Core.Models;
using WxHandle.Core.Options;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace WxHandle.Core.Interfaces
{
    public interface IWxPayHandle
    {
        Task PayCallback(PayCallbackData wxResult);

        Task<SendPayResult> SendPay(SendPayInput input);

        Task<QueryOrderOutput> QueryOrder(QueryOrderInput input);
    }

    public interface IWxHandle
    {

    }

    public class WxResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string errmsg { get; set; }
    }

    public class AccessTokenOutput: WxResult
    {
        /// <summary>
        /// 凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间[秒]
        /// </summary>
        public int expires_in { get; set; } 
    }

    public enum CustMenuType
    {
        /// <summary>
        /// 1、click：点击推事件用户点击click类型按钮后，
        /// 微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），
        /// 并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互；
        /// </summary>
        click,

        /// <summary>
        /// 2、view：跳转URL用户点击view类型按钮后，
        /// 微信客户端将会打开开发者在按钮中填写的网页URL，
        /// 可与网页授权获取用户基本信息接口结合，
        /// 获得用户基本信息。
        /// </summary>
        view,

        /// <summary>
        /// 3、scancode_push：扫码推事件用户点击按钮后，
        /// 微信客户端将调起扫一扫工具，
        /// 完成扫码操作后显示扫描结果（如果是URL，将进入URL），
        /// 且会将扫码的结果传给开发者，
        /// 开发者可以下发消息。
        /// </summary>
        scancode_push,

        /// <summary>
        /// 4、scancode_waitmsg：扫码推事件且弹出“消息接收中”
        /// 提示框用户点击按钮后，微信客户端将调起扫一扫工具，
        /// 完成扫码操作后，将扫码的结果传给开发者，
        /// 同时收起扫一扫工具，然后弹出“消息接收中”提示框，
        /// 随后可能会收到开发者下发的消息。
        /// </summary>
        scancode_waitmsg,

        /// <summary>
        /// 5、pic_sysphoto：弹出系统拍照发图用户点击按钮后，
        /// 微信客户端将调起系统相机，完成拍照操作后，
        /// 会将拍摄的相片发送给开发者，并推送事件给开发者，
        /// 同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </summary>
        pic_sysphoto,

        /// <summary>
        /// 6、pic_photo_or_album：弹出拍照或者相册发图用户点击按钮后，
        /// 微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// </summary>
        pic_photo_or_album,

        /// <summary>
        /// 7、pic_weixin：弹出微信相册发图器用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，
        /// 将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。
        /// </summary>
        pic_weixin,

        /// <summary>
        /// 8、location_select：弹出地理位置选择器用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，
        /// 将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </summary>
        location_select,

        /// <summary>
        /// 9、media_id：下发消息（除文本消息）用户点击media_id类型按钮后，
        /// 微信服务器会将开发者填写的永久素材id对应的素材下发给用户，
        /// 永久素材类型可以是图片、音频、视频、图文消息。
        /// 请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </summary>
        media_id,

        /// <summary>
        /// 10、view_limited：跳转图文消息URL用户点击view_limited类型按钮后，
        /// 微信客户端将打开开发者在按钮中填写的永久素材id对应的图文消息URL，
        /// 永久素材类型只支持图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </summary>
        view_limited,
        miniprogram,

    }
    public class CustMenuInput
    {
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过60个字节
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CustMenuType type { get; set; }

        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 网页 链接，用户点击菜单可打开链接，不超过1024字节。 
        /// type为miniprogram时，不支持小程序的老版本客户端将打开本url。
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 调用新增永久素材接口返回的合法media_id
        /// </summary>
        public string media_id { get; set; }

        /// <summary>
        /// 小程序的appid（仅认证公众号可配置）
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 小程序的页面路径
        /// </summary>
        public string pagepath { get; set; }

        /// <summary>
        /// 下级菜单
        /// </summary>
        public List<CustMenuInput> sub_button { get; set; }
    }

    public class WxHandle
    {
        private readonly IOptions<WxConfig> options;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;

        public WxHandle(IOptions<WxConfig> options, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取凭证
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            if (memoryCache.TryGetValue("AccessToken", out var value) && value is AccessTokenOutput accessTokenOutput)
                return accessTokenOutput.access_token;
            else
            {
                var httpResponse = await httpClientFactory.CreateClient()
                    .GetAsync(string.Format(options.Value.ServerConfig.AccessTokenUrl, options.Value.AppId, options.Value.AppSecret));
                var result = await httpResponse.Content.ReadAsStringAsync();
                var tokenOutput = JsonConvert.DeserializeObject<AccessTokenOutput>(result);

                if (httpResponse.IsSuccessStatusCode)
                {
                    memoryCache.Set("AccessToken", tokenOutput, DateTime.Now.AddSeconds(tokenOutput.expires_in - 60));
                    return tokenOutput.access_token;
                }
                else
                {

                    throw new Exception($"{tokenOutput.errmsg}:{tokenOutput.errcode}");
                }
            }
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateCustMenu()
        {

        }
    }
}
