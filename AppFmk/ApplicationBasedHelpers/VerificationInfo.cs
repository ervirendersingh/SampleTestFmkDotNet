using AppFmk.Interfaces;
using Selenium.AutomationToolInterfaces;
using System.Collections.Generic;

namespace AppFmk.ApplicationBasedHelpers
{
    public class VerificationInfo : IVerificationInfo
    {
        public bool Result { get; set; }
        public string Actual { get; set; }
        public string Expected { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public VerificationInfo(IVerification toVerification)
        {
            Result = toVerification.Result;
            Actual = toVerification.Actual;
            Expected = toVerification.Expected;
            Message = toVerification.Message;
            Data = toVerification.Data;
        }
    }
}
