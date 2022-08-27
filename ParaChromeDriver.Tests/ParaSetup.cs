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
                chromeDriver
                =>
                {
                    Facebook facebook = new Facebook(chromeDriver);
                    facebook.SignIn();

                });

            ParaChromeDriver.Start(ParaConstants.Mode.Parasitic);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ParaChromeDriver.Stop();
        }
    }
}
