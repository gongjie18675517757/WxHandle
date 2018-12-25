namespace WxHandle.Core.Options
{
    public class WxConfig
    {
        /// <summary>
        /// 公众号通知路径
        /// </summary>
        public string Wx_Notify_Url { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 开发者密码
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }

        /// <summary>
        /// 商户ID
        /// </summary>
        public string Mch_Id { get; set; }

        /// <summary>
        /// 商户key
        /// </summary>
        public string PayKey { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public SignMode SignMode { get; set; } = SignMode.MD5;

        /// <summary>
        /// 支付通知路径
        /// </summary>
        public string Pay_Notify_Url { get; set; }

        /// <summary>
        /// 微信服务器配置
        /// </summary>
        public WxServerConfig Server { get; set; } = new WxServerConfig();
    }
}
