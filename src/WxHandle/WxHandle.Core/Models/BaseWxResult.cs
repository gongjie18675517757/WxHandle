namespace WxHandle.Core.Models
{
    /// <summary>
    /// 微信服务端返回
    /// </summary>
    public abstract class BaseWxResult : IWxResult,IHasSignModel
    {
        /// <summary>
        /// 返回状态码	
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述	
        /// </summary>
        public string err_code_des { get; set; }
    }
}
