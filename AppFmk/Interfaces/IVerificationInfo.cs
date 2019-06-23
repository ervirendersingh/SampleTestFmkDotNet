

using Selenium.AutomationToolInterfaces;
using System.Collections.Generic;

namespace AppFmk.Interfaces
{
    public interface IVerificationInfo
    {
        bool Result { get; set; }
        string Actual { get; set; }
        string Expected { get; set; }
        string Message { get; set; }
        Dictionary<string, string> Data { get; set; }
    }
}
