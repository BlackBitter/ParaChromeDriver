namespace ParaChromeDriver.Tests
{
    using OpenQA.Selenium;
    using ParaDriver;

    [TestFixture]
    [Parallelizable]
    public class ParaTest : BaseTest
    {

        [Test]
        [Parallelizable]
        public void Test1()
        {
            TakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test2()
        {
            TakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test3()
        {
            TakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test4()
        {
            TakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test5()
        {
            TakeScreenshot();
        }

        [Test]
        [Parallelizable]
        public void Test6()
        {
            TakeScreenshot();
        }
    }
}