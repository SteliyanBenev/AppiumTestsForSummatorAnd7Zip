using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;
using System.Threading;

namespace AppiumSummatorTests
{
    public class Appium7ZipTests
    {
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> desktopDriver;
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        private AppiumOptions options;
        private AppiumOptions appiumOptionsDesktop;
        private string workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            this.options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability(MobileCapabilityType.App, @"C:\Program Files\7-Zip\7zFM.exe");
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options);

            appiumOptionsDesktop = new AppiumOptions() { PlatformName = "Windows"};
            appiumOptionsDesktop.AddAdditionalCapability("app", "Root");
            desktopDriver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), appiumOptionsDesktop);

            workDir = Directory.GetCurrentDirectory() + @"\workDir";
            if (Directory.Exists(workDir))
                Directory.Delete(workDir, true);
            Directory.CreateDirectory(workDir);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test7ZipApp()
        {
            var textBoxLocationFolder = driver.FindElementByXPath
                ("/Window/Pane/Pane/ComboBox/Edit");
            textBoxLocationFolder.SendKeys(@"C:\Program Files\7-Zip\" + Keys.Enter);

            var listBoxFiles = driver.FindElementByXPath("/Window/Pane/List");
            listBoxFiles.SendKeys(Keys.Control + "a");

            var buttonAdd = driver.FindElementByName("Add"); //"Add"
            buttonAdd.Click();

            Thread.Sleep(500);
            var windowAddToArchive = desktopDriver.FindElementByName
                ("Add to Archive"); // "Add to Archive"

            var textBoxArchiveName = windowAddToArchive.FindElementByXPath
                ("/Window/ComboBox/Edit[@Name='Archive:']"); // 'Archive:'
            string archiveFileName = workDir + "\\" + DateTime.Now.Ticks + ".7z";
            textBoxArchiveName.SendKeys(archiveFileName);

            var comboArchiveFormat = windowAddToArchive.FindElementByXPath
                ("/Window/ComboBox[@Name='Archive format:']"); //'Archive format:'
            comboArchiveFormat.SendKeys("7z");

            var comboCompressionLevel = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']"); // 'Compression level:'
            comboCompressionLevel.SendKeys("Ultra"); // "Ultra"

            var comboDictionarySize = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']"); // 'Dictionary size:'
            comboDictionarySize.SendKeys(Keys.End);

            var comboWordSize = windowAddToArchive.FindElementByXPath("/Window/ComboBox[@Name='Word size:']"); // 'Word size:'
            comboWordSize.SendKeys(Keys.End);

            var buttonAddToArchiveOK = windowAddToArchive.FindElementByXPath("/Window/Button[@Name='OK']"); // 'OK'
            buttonAddToArchiveOK.Click();

            Thread.Sleep(1000);

            textBoxLocationFolder.SendKeys(archiveFileName + Keys.Enter);

            var buttonExtract = driver.FindElementByName("Extract"); // "Extract"
            buttonExtract.Click();

            var buttonExtractOK = driver.FindElementByName("OK"); // "OK"
            buttonExtractOK.Click();

            Thread.Sleep(1000);

            string executable7ZipOriginal = @"C:\Program Files\7-Zip\7zFM.exe";
            string executable7ZipExtracted = workDir + @"\7zFM.exe";
            FileAssert.AreEqual(executable7ZipOriginal, executable7ZipExtracted);
        }
    }
}
