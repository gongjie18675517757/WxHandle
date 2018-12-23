namespace WxHandle.Core.Models
{
    public interface IWxResult
    {
        string return_code { get; }

        string return_msg { get; }
    }
}
