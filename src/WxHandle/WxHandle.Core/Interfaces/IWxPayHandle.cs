using System.Threading.Tasks;
using WxHandle.Core.Models;

namespace WxHandle.Core.Interfaces
{
    public interface IWxPayHandle
    {
        Task PayCallback(PayCallbackData wxResult);

        Task<SendPayResult> SendPay(SendPayInput input);

        Task<QueryOrderOutput> QueryOrder(QueryOrderInput input);
    }

    public interface IWxHandle
    {

    }
}
