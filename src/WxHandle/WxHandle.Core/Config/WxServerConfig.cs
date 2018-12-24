namespace WxHandle.Core.Options
{
    /// <summary>
    /// 微信服务器配置
    /// </summary>
    public class WxServerConfig
    {
        /// <summary>
        /// 支付 统一下单地址
        /// </summary>
        public string SendPayAddress { get; set; } = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 查询 订单支付情况地址
        /// </summary>
        public string QueryPayAddress { get; set; } = "https://api.mch.weixin.qq.com/pay/orderquery";
    }
}
