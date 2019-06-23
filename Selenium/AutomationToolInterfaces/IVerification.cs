using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.AutomationToolInterfaces
{
    public interface IVerification
    {
        bool Result { get; set; }
        string Actual { get; set; }
        string Expected { get; set; }
        string Message { get; set; }
        Dictionary<string, string> Data { get; set; }
    }
}
