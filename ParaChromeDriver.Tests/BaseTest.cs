﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaChromeDriver.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using ParaDriver;
    public class BaseTest
    {
        protected ThreadLocal<ParaChromeDriver> chromeDriver = new ThreadLocal<ParaChromeDriver>();

        [SetUp]
        public void Setup()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            
            chromeDriver.Value = new ParaChromeDriver(chromeOptions);
            chromeDriver.Value.Navigate().GoToUrl("https://www.facebook.com");
        }

        [TearDown]
        public void TearDown()
        {
            chromeDriver.Value.Quit();
        }

        protected void TakeScreenshot()
        {
            chromeDriver.Value.FindElement(By.XPath("//div[@aria-label='Messenger']")).Click();
            Screenshot ss = ((ITakesScreenshot)chromeDriver.Value).GetScreenshot();
            ss.SaveAsFile(@$"C:\Test\Image{TestContext.CurrentContext.Test.FullName}.png",
            ScreenshotImageFormat.Png);
        }
    }
}