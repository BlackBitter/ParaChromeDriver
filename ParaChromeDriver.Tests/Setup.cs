using OpenQA.Selenium.Chrome;
[assembly: LevelOfParallelism(3)]

namespace ParaChromeDriver
{


    [SetUpFixture]
    internal class Setup
    {
        [OneTimeSetUp]
        public void SetupF()
        {

        }
    }
}
