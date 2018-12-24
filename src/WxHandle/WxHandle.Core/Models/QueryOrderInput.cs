using System.Collections.Generic;

namespace WxHandle.Core.Models
{
    /// <summary>
    /// 查询支付订单参数
    /// </summary>
    public  class QueryOrderInput : IHasSignModel,IVerifySendData
    {
        /// <summary>
        /// 应用APPID	
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }


        public bool Verify(out List<string> errMsgs)
        {
            errMsgs = new List<string>();
            if (out_trade_no != null && transaction_id != null)
                errMsgs.Add("商户订单号与微信订单号只能2选1");

            return errMsgs.Count == 0;
        }
    }
}
