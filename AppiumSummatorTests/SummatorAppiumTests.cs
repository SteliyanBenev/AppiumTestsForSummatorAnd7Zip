using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace AppiumSummatorTests
{
    public class SummatorAppiumTests
    {
        private WindowsDriver<WindowsElement> driver;
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private AppiumOptions options;

        [OneTimeSetUp]
        public void Setup()
        {
            this.options = new AppiumOptions() { PlatformName = "Windows" };
            this.options.AddAdditionalCapability(MobileCapabilityType.App, @"D:\QA Automation 2022\06.Appium-Desktop-Testing-Exercises-Resources\SummatorDesktopApp.exe");
            this.driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), this.options);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_Summator_With_Valid_Numbers()
        {
            //Arrange
            var firstNumberField = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var secondNumberField = driver.FindElementByAccessibilityId("textBoxSecondNum");
            var buttonCalc = driver.FindElementByAccessibilityId("buttonCalc");
            var fieldSum = driver.FindElementByAccessibilityId("textBoxSum");
            var expectedResult = "25";

            //Act
            firstNumberField.Clear();
            firstNumberField.Click();
            firstNumberField.SendKeys("7");

            secondNumberField.Clear();
            secondNumberField.Click();
            secondNumberField.SendKeys("18");

            buttonCalc.Click();

            //Assert
            Assert.AreEqual(expectedResult, fieldSum.Text);
        }

        [Test]
        public void Test_Summator_With_Empty_NumbersFields()
        {
            //Arrange
            var firstNumberField = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var secondNumberField = driver.FindElementByAccessibilityId("textBoxSecondNum");
            var buttonCalc = driver.FindElementByAccessibilityId("buttonCalc");
            var fieldSum = driver.FindElementByAccessibilityId("textBoxSum");
            var expectedResult = "error";

            //Act
            firstNumberField.Clear();
            secondNumberField.Clear();
            buttonCalc.Click();

            //Assert
            Assert.AreEqual(expectedResult, fieldSum.Text);
        }

        [Test]
        public void Test_Summator_With_Invalid_Data()
        {
            //Arrange
            var firstNumberField = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var secondNumberField = driver.FindElementByAccessibilityId("textBoxSecondNum");
            var buttonCalc = driver.FindElementByAccessibilityId("buttonCalc");
            var fieldSum = driver.FindElementByAccessibilityId("textBoxSum");
            var expectedResult = "error";

            //Act
            firstNumberField.Clear();
            firstNumberField.Click();
            firstNumberField.SendKeys("sdf");

            secondNumberField.Clear();
            secondNumberField.Click();
            secondNumberField.SendKeys("#$@");

            buttonCalc.Click();

            //Assert
            Assert.AreEqual(expectedResult, fieldSum.Text);
        }
    }
}