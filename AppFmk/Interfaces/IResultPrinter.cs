

namespace AppFmk.Interfaces
{
    public interface IResultPrinter
    {
        string FilePath { get; set; }
        string TestName { get; set; }
        void PrepareTemplate();
        void UpdateResult(bool result, string actual, string expected, string message, string screenshotPath);    
    }
}
