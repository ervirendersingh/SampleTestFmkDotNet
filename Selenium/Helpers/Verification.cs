using Selenium.AutomationToolInterfaces;
using System.Collections.Generic;

namespace Selenium.Helpers
{
    public class Verification : IVerification
    {
        public bool Result { get; set; }
        public string Actual { get; set; }
        public string Expected { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public Verification()
        {
            Actual = "";
            Expected = "";
            Message = "";
            Data = new Dictionary<string, string>();
        }
    }
}
