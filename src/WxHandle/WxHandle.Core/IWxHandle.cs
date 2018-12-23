using System.Threading.Tasks;

namespace WxHandle.Core
{
    public interface IWxHandle
    {
        Task<string> PayCallback(WxResult<PayCallbackData> wxResult);
    }
}
