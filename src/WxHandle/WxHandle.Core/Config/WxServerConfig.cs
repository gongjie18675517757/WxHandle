namespace WxHandle.Core.Options
{
    /// <summary>
    /// 微信服务器配置
    /// </summary>
    public class WxPayServerConfig
    {
        /// <summary>
        /// 支付 统一下单地址
        /// </summary>
        public string SendPayUrl { get; set; } = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 查询 订单支付情况地址
        /// </summary>
        public string QueryPayUrl { get; set; } = "https://api.mch.weixin.qq.com/pay/orderquery";
    }

    public class WxServerConfig
    {
        /// <summary>
        /// 获取凭证
        /// </summary>
        public string AccessTokenUrl { get; set; } = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 创建自定义菜单 
        /// </summary>
        public string CreateCustMenuUrl { get; set; } = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
    }
}
