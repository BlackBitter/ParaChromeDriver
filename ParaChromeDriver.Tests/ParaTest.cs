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
            chromeDriver.Value = new ParaChromeDriver(chromeOptions);

            int retry = 30;
            while (retry --> 0)
            {
                try
                {
                    chromeDriver.Value.Navigate().GoToUrl("https://gmail.com");
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
                    //Gmail gmail = new Gmail(chromeDriver);
                    //gmail.SignIn();

                });
            ParaChromeDriver.Start(ParaConstants.Mode.ManualParasitic, @"C:\Users\patel\AppData\Local\Google\Chrome\Data");
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