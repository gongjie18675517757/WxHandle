namespace WxHandle.Core.Models
{
    /// <summary>
    /// 查询支付订单返回
    /// </summary>
    public class QueryOrderOutput: BaseWxResult
    {
        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 交易状态
        /// SUCCESS—支付成功
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中
        /// PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        public string cash_fee { get; set; }

        /// <summary>
        /// 现金支付货币类型
        /// </summary>
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 代金券金额
        /// </summary>
        public string coupon_fee { get; set; }

        /// <summary>
        /// 代金券使用数量
        /// </summary>
        public string coupon_count { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string time_end { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }
    }
}
