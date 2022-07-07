using OpenQA.Selenium;
using OtpNet;

namespace ParaChromeDriver.Tests
{
    internal class Gmail
    {
        private IWebDriver webDriver;

        public Gmail(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public static By UserNameTextBox => By.XPath("//input[@type='email']");

        public static By PasswordTextBox => By.XPath("//input[@type='password']");

        public static By OTPTextBox => By.XPath("//input[@type='tel']");

        public static By Next => By.XPath("//button[contains(normalize-space(),'Next')]");

        public static By ErrorMessage => By.CssSelector("p[class= 'error-message']");

        public static By GoogleAuthenticator => By.XPath("//li[contains(normalize-space(),'Google Authenticator')]");

        public void SignIn()
        {
            int retry = 30;
            while (retry-- > 0)
            {
                try
                {
                    webDriver.Navigate().GoToUrl("https://accounts.google.com/signin");
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            webDriver.FindElement(UserNameTextBox).SendKeys("");
            Thread.Sleep(1000);
            webDriver.FindElement(Next).Click();
            Thread.Sleep(3000);
            webDriver.FindElement(PasswordTextBox).SendKeys("");
            Thread.Sleep(2000);
            webDriver.FindElement(Next).Click();
            Thread.Sleep(5000);
            webDriver.FindElement(OTPTextBox).SendKeys(GenerateOTP(""));
            Thread.Sleep(2000);
            webDriver.FindElement(Next).Click();

            Thread.Sleep(10000);
        }
        private string GenerateOTP(string otpKey)
        {
            Totp otpGenerator = new Totp(Base32Encoding.ToBytes(otpKey));
            if (otpGenerator.RemainingSeconds() > 5)
            {
                return otpGenerator.ComputeTotp();
            }
            else
            {
                Thread.Sleep(TimeSpan.FromSeconds(otpGenerator.RemainingSeconds()));
                return otpGenerator.ComputeTotp();
            }
        }
    }
}
