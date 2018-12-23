namespace WxHandle.Core.Models
{
    /// <summary>
    /// 需要签名
    /// </summary>
    public interface IHasSignModel
    {
        string nonce_str { get; set; }

        string sign { get; set; }
    }
}
