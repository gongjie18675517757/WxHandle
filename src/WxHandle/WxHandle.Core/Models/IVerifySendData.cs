using System.Collections.Generic;

namespace WxHandle.Core.Models
{
    public interface IVerifySendData
    {
        bool Verify(out List<string> errMsgs);
    }
}
