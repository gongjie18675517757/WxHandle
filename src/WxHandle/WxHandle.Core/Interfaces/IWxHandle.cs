using System.Threading.Tasks;
using WxHandle.Core.Models;

namespace WxHandle.Core.Interfaces
{
    public interface IWxHandle
    {
        Task<IWxResult> PayCallback(PayCallbackData wxResult);

        Task<SendPayResult> SendPay(SendPayInput input);
    }
}
