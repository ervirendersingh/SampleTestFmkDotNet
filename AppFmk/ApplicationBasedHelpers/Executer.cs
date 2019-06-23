using System;
using AppFmk.Pages;
using AppFmk.Interfaces;

namespace AppFmk.ApplicationBasedHelpers
{
    public class Executer : IFunctionExecuter
    {
        IBasePage _basePage;

        public Executer(IBasePage basePage)
        {
            _basePage = basePage;
        }

        public void Execute(Action action)
        {
            try
            {
                action();
            }

            catch (Exception ex)
            {
                _basePage.SaveScreenshot();
                System.Console.WriteLine("Exception Message For " + _basePage.TestName + "=>" + ex.Message);
                throw ex;
            }
        }

        public T Execute<T>(Func<T> action)
        {
            try
            {
                var returnValue = action();
                return returnValue;
            }

            catch (Exception ex)
            {
                _basePage.SaveScreenshot();
                throw ex;
            }
        }
    }
}
