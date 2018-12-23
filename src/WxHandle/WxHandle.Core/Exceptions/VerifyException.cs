using System;
using System.Collections.Generic;
using System.Text;

namespace WxHandle.Core.Exceptions
{
    public class VerifyException : Exception
    {
        private readonly List<string> errs;

        public VerifyException(string msg, List<string> errs) : base(msg)
        {
            this.errs = errs;
        }
    }
}
