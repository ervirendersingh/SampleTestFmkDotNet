using Ninject.Activation;
using OpenQA.Selenium.Remote;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;
using Selenium.IOC;
using System;
using System.Collections.Generic;

namespace Selenium.Helpers
{
    public class ToolsDependencyResolverProvider : Provider<IToolDependencyResolver>
    {
        private BrowserName _browserName;

        public ToolsDependencyResolverProvider(BrowserName browserName)
        {
            this._browserName = browserName;
        }
        protected override IToolDependencyResolver CreateInstance(IContext context)
        {
            return new ToolDependencyResolver(_browserName);
        }
    }
}
