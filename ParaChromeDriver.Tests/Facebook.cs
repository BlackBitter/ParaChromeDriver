using OpenQA.Selenium;

namespace ParaChromeDriver.Tests
{
    internal class Facebook
    {
        private IWebDriver webDriver;

        public Facebook(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public static By UserNameTextBox => By.XPath("//input[@id='email']");

        public static By PasswordTextBox => By.XPath("//input[@id='pass']");

        public static By Next => By.XPath("//button[@type='submit']");

        public void SignIn()
        {
            int retry = 3;
            while (retry-- > 0)
            {
                try
                {
                    webDriver.Navigate().GoToUrl("https://www.facebook.com");
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            webDriver.FindElement(UserNameTextBox).SendKeys("patelsurajofficial@gmail.com");
            webDriver.FindElement(PasswordTextBox).SendKeys("");
            webDriver.FindElement(Next).Click();

            Thread.Sleep(10000);
        }
    }
}
