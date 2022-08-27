namespace ParaChromeDriver.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using ParaDriver;

    [TestFixture]
    [Parallelizable]
    public class ParaTest1 : BaseTest
    {
        [Test]
        [Parallelizable]
        public void Test1()
        {
            OpenMessangerAndTakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test2()
        {
            OpenMessangerAndTakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test3()
        {
            OpenMessangerAndTakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test4()
        {
            OpenMessangerAndTakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test5()
        {
            OpenMessangerAndTakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test6()
        {
            OpenMessangerAndTakeScreenshot();
        }
    }
}