using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomationTests
{
    [TestClass]
    public class TestLogin
    {
        private readonly IOsPlatform _osPlatform = new OsPlatform();
        private const string Url = "https://localhost:5001/";

        [TestMethod]
        public void TestLoginMethod()
        {

            IWebDriver driver = null;
            if (_osPlatform.isWindows())
            {
                driver = new ChromeDriver("C:/WebDriver/bin");
                driver.Navigate().GoToUrl(Url);
                var item = driver.FindElement(By.XPath("//h1[@class='display-4']")).Text;
                //Console.WriteLine(item);
                Assert.AreEqual( ("Welcome"),item);
            }
            else
            {
                driver = new ChromeDriver("/usr/local/bin");
                driver.Navigate().GoToUrl(driver.Url);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
                IWebElement element = driver.FindElement(By.XPath("//h1[@class='display-4']"));
                Assert.AreEqual( ("Welcome"),element.Text);
            }
            driver.Quit();
        }
    }

    [TestClass]
    public class TestSubmitArticle
    {
        [TestMethod]
        public void TestSubmitArticleMethod()
        {
            // IWebDriver driver = new ChromeDriver("C:/WebDriver/bin/");
            // driver.Navigate().GoToUrl("https://localhost:5001/student/submitarticle");
            // SubmitArticlePage submitArticlePage = new SubmitArticlePage(driver);
            // HomePage homePage = submitArticlePage.SubmitArticle("Title so 1", "<h1>Content ne</h1>", "1");
            // Assert.Equals(homePage.GetMessageText(), ("Article submitted"));
        }
    }
}
