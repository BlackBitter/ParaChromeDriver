## ParaChromeDriver
## _Parasite chrome driver_


Parasite chrome driver allows you to do pre-setup for authentication, utilize the same resources/cache files instead of downloading it again to execute other tests hence offers fast UI testing. We only need to login once and the same login will be used for the entire test run.

## Features

- Use one login to run entire test suite.
- Provides a way to bypass captcha and OTP.
- It allows you to run UI tests in parallel with a single login.

## Setup Guide
Parachrome comes with 3 modes 
1) Parasitic Mode
2) Manual Parasitic Mode
3) Debugging Mode

### 1. Parasitic mode
In parasitic mode it starts with pre-setup to create para data and run the test cases in sequence/parallel and at the end perform cleanup.

Create a test project on ```.NET5```or above (in the current example we have used Nunit you can use any unit testing framework which has a similar attribute for assembly setup which runs the part of code only once for the entire run.)

Create a class file with ```ParaSetup.cs``` in your test layer and add  ```[SetUpFixture]``` attribute on the class. Create one time setup function and one time teardown function as shown below with respective attributes.


```sh
namespace ProjectNamespace
{
    using OpenQA.Selenium.Chrome;
    using ParaDriver;

    [SetUpFixture]
    internal class ParaSetup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ParaChromeDriver.AttachParasite(FacebookSignIn);
            ParaChromeDriver.Start(ParaConstants.Mode.Parasitic);
        }

        public void FacebookSignIn(ChromeDriver chromeDriver)
        {
            Facebook facebook = new Facebook(chromeDriver);
            facebook.SignIn();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ParaChromeDriver.Stop();
        }
    }
}
```

> Note : If you are using any other unit testing framework like MS Test framework, it has the ```[AssemblyInitialize]``` attribute and doesn't require attributes like```[SetUpFixture]``` .

ParaChromeSetup starts in ```OneTimeSetup()``` where we can attach parasites, here you can see ```FacebookSignIn``` function is attached to parachrome, the only thing we have to check is the function which we are attaching should have below signature.
```sh
            void FunctionName(ChromeDriver chromeDriver)
```

We can also attach multiple parasites as show below, and the functions will be invoked sequentially.

```sh
            ParaChromeDriver.AttachParasite(ParasiteFunction1);
            ParaChromeDriver.AttachParasite(ParasiteFunction1);
```

Once the parasite is attach we can start our parachromedriver by calling ```ParaChromeDriver.Start()``` and set the mode to ```Mode.Parasitic``` this will start the parachromedriver in parasitic mode.

```sh
            ParaChromeDriver.Start(ParaConstants.Mode.Parasitic);
```

In ```OneTimeTeardown()``` we call ```ParaChromeDriver.Stop()``` to stop the parachromedriver and do memory cleaning.

Now we can create the instance of ParaChromeDrive similar to ChromeDriver and navigate to the web application url and you will see the you are into you app without login. Below is sample code for your test layer.

```sh
namespace TestLayer
{
    [TestFixture]
    [Parallelizable]
    public class ParaTest1
    {
        [OneTimeSetUp]
        public void OneTimeSetup() { }

        [OneTimeTearDown]
        public void OneTimeTearDown() { }

        [SetUp]
        public void Setup() { }

        [TearDown]
        public void Teardown() { }

        [Test]
        [Parallelizable]
        public void Test1()
        {
            var driver = new ParaChromeDriver();

            // Test Code here without login.
        }

        [Test]
        [Parallelizable]
        public void Test2()
        {
            var driver = new ParaChromeDriver();

            // Test Code here without login.
        }
    }
}
```
### 2. Manual Parasitic mode
Manual parasitic mode is generally used when we have a web application which has Captcha, and we don't want to bypass it.
In manual parasitic mode keep our para data in a custom path and run the test cases in sequence/parallel. using this paradata.


Create a class file with ```ParaSetup.cs``` in your test layer and add  ```[SetUpFixture]``` attribute on the class. Create one time setup function and one time teardown function as shown below with respective attributes.


```sh
namespace ProjectNamespace
{
    using OpenQA.Selenium.Chrome;
    using ParaDriver;

    [SetUpFixture]
    internal class ParaSetup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ParaChromeDriver.AttachParasite(FacebookSignIn);
            ParaChromeDriver.Start(ParaConstants.Mode.ManualParasitic, @"E:\WorkPlace\ParaData");
        }

        public void FacebookSignIn(ChromeDriver chromeDriver)
        {
            Facebook facebook = new Facebook(chromeDriver);
            facebook.SignIn();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ParaChromeDriver.Stop();
        }
    }
}
```
To start in manual mode, we only have to set the mode to ```Mode.ManualParasitic```  and provide the custom path where you want to keep all the para data. (Always name folder as ParaData) as shown below
```sh
             ParaChromeDriver.Start(ParaConstants.Mode.ManualParasitic, @"E:\WorkPlace\ParaData");
```
To create the paradata in this path you need to run the parasite function once and during this run you can manually enter the captcha and OTP to complete the login process, once the para data is created parasite functions will not be called for the further runs. It will only run the parasite function again if we delete the files inside the ParaData folder.

### 3. Debugging mode
In this mode we disable the parasitic behavior of parachrome and run it as simple chromedrive, which is helpful during debugging.
Simply change the mode to ```Mode.Debugging```

```sh
            ParaChromeDriver.Start(ParaConstants.Mode.Debugging);
```

