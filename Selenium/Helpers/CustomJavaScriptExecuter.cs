
using OpenQA.Selenium;
using System;
using System.Threading;

namespace Selenium.Helpers
{
    public class CustomJavaScriptExecuter
    {
        IJavaScriptExecutor _jsExecuter;
        IWebDriver _driver;
        string _script;
        object[] _parameters;

        public CustomJavaScriptExecuter(IWebDriver driver)
        {
            this._driver = driver;
            _jsExecuter = (IJavaScriptExecutor)_driver;
        }

        public string Execute(string scrpt, object[] prmts)
        {
            _script = scrpt;
            _parameters = prmts;
            return ExecuteWithRetry();
        }

        private string ExecuteWithRetry()
        {
            RetryHelper Helper = new RetryHelper();
            return Helper.RetryWithExceptions(3, () => Run()).Result;
        }

        private string Run()
        {
            return (string)_jsExecuter.ExecuteScript(_script, _parameters);
        }
    }

    internal class RetryHelper
    {
        internal string Retry(int numberOfRetries, Func<string> action)
        {
            var currentNumberOfRetries = numberOfRetries;
            string Result;
            do
            {
                if (currentNumberOfRetries < numberOfRetries)
                {
                    Thread.Sleep(1000);
                }

                Result = action.Invoke();
            } while (currentNumberOfRetries-- > 0 && Result == "customException");

            return Result;
        }


        public RetryResult RetryWithExceptions(int numberOfRetries, Func<string> action)
        {
            return RetryWithExceptions<Exception>(numberOfRetries, action);
        }

        public RetryResult RetryWithExceptions<T>(int numberOfRetries, Func<string> action) where T : Exception
        {
            Exception lastException = null;
            var result = Retry(numberOfRetries, () =>
            {
                try
                {
                    return action();
                }
                catch (T ex)
                {
                    lastException = ex;
                    System.Console.WriteLine("Exception while JScript Execution - " + ex.Message);
                    return "customException";
                }
            });
            return new RetryResult
            {
                Result = result,
                LastException = lastException
            };
        }
    }

    internal class RetryResult
    {
        public string Result { get; set; }
        public Exception LastException { get; set; }
    }
}
