namespace ParaChromeDriver.Tests
{
    [TestFixture]
    [Parallelizable]
    public class ParaTest : BaseTest
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

    public class Person
    {
        public static string test;

        static Person()
        {
            test = "jack";
        }

        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Person() { }

        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}