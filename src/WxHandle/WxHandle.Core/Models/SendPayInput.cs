using System;
using System.Collections.Generic;

namespace WxHandle.Core.Models
{
    public class SendPayInput : IHasSignModel, IVerifySendData
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; } = "WEB";

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; } = "MD5";

        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        public string fee_type { get; set; } = "CNY";

        /// <summary>
        /// 总金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 交易起始时间
        /// </summary>
        public string time_start { get; set; }

        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string time_expire { get; set; }

        /// <summary>
        /// 订单优惠标记
        /// </summary>
        public string goods_tag { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; } = "APP";

        /// <summary>
        /// 指定支付方式
        /// </summary>
        public string limit_pay { get; set; }

        /// <summary>
        /// 开发票入口开放标识
        /// </summary>
        public string receipt { get; set; }

        public bool Verify(out List<string> errMsgs)
        {
            var msgs = new List<string>();

            void AddErr<T>(T val, Func<T, bool> verify, string name, string errMsg)
            {
                if (verify(val))
                    msgs.Add($"{name}异常:{errMsg}!");
            }

            AddErr(appid, string.IsNullOrWhiteSpace, "应用ID", "不可为空");
            AddErr(mch_id, string.IsNullOrWhiteSpace, "商户号", "不可为空");
            AddErr(body, string.IsNullOrWhiteSpace, "商品描述", "不可为空");
            AddErr(total_fee, v => v <= 0, "总金额", "不可小于等于0");
            AddErr(spbill_create_ip, string.IsNullOrWhiteSpace, "终端IP", "不可为空");
            AddErr(notify_url, string.IsNullOrWhiteSpace, "通知地址", "不可为空");
            AddErr(trade_type, string.IsNullOrWhiteSpace, "交易类型", "不可为空"); 

            errMsgs = msgs;
            return msgs.Count == 0;
        }
    }
}
