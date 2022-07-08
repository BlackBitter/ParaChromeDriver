namespace ParaDriver
{
    using OpenQA.Selenium;
    using System;
    using System.Threading;

    internal class ParaNavigator : INavigation
    {
        IWebDriver driver;
        public ParaNavigator(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void Back()
        {
            driver.Navigate().Back();
        }

        public void Forward()
        {
            driver.Navigate().Forward();
        }

        public void GoToUrl(string url)
        {
            int retry = 3;
            while (retry-- > 0)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                    break;
                }
                catch
                {
                    Thread.Sleep(2000 * (3 - retry));
                }
            }
        }

        public void GoToUrl(Uri url)
        {
            int retry = 3;
            while (retry-- > 0)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                    break;
                }
                catch
                {
                    Thread.Sleep(2000 * (3 - retry));
                }
            }
        }

        public void Refresh()
        {
            int retry = 3;
            while (retry-- > 0)
            {
                try
                {
                    driver.Navigate().Refresh();
                    break;
                }
                catch
                {
                    Thread.Sleep(2000 * (3 - retry));
                }
            }
        }
    }
}
