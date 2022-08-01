namespace ParaChromeDriver.Tests
{
    using OpenQA.Selenium.Chrome;
    using ParaDriver;

    [SetUpFixture]
    internal class ParaSetup
    {
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
            ParaChromeDriver.Start(ParaConstants.Mode.Parasitic, @"E:\WorkPlace\DataPara\ParaData");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ParaChromeDriver.Stop();
        }
    }
}
