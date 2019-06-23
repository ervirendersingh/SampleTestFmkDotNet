

using Selenium.Helpers;

namespace AppFmk.Interfaces
{
    public interface IReporter
    {
        string FilePath { get; set; }
        string TestName { get; set; }
        void ReportResult(IVerificationInfo verifyInfo);
        void ReportResult(bool Result);
    }
}
