namespace ParaChromeDriver.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using ParaDriver;
    public class ParaTest
    {
        ThreadLocal<ParaChromeDriver> chromeDriver = new ThreadLocal<ParaChromeDriver>();

        [SetUp]
        public void Setup()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("start-maximized");
            chromeDriver.Value = new ParaChromeDriver(chromeOptions);

            int retry = 30;
            while (retry --> 0)
            {
                try
                {
                    chromeDriver.Value.Navigate().GoToUrl("https://www.facebook.com");
                    break;
                }
                catch (WebDriverException ex)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ParaChromeDriver.AttachParasite(
                (ChromeDriver chromeDriver)
                =>
                {
                    Facebook facebook = new Facebook(chromeDriver);
                    facebook.SignIn();

                });
            ParaChromeDriver.Start(ParaConstants.Mode.Parasitic, @"C:\Users\patel\AppData\Local\Google\Chrome\Test\ParaData");
        }

        [Test]
        [Parallelizable]
        public void Test1()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [Test]
        [Parallelizable]
        public void Test2()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [Test]
        [Parallelizable]
        public void Test3()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [Test]
        [Parallelizable]
        public void Test4()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [Test]
        [Parallelizable]
        public void Test5()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [Test]
        [Parallelizable]
        public void Test6()
        {
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.Name}.png",
            ScreenshotImageFormat.Png);
        }

        [TearDown]
        public void Tear()
        {
            chromeDriver.Value.Quit();
        }

        [OneTimeTearDown]
        public void OneTear()
        {
            ParaChromeDriver.Stop();
        }
    }
}