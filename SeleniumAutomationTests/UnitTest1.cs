using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomationTests
{
    [TestClass]
    public class TestLogin
    {
        [TestMethod]
        public void TestLoginMethod()
        {
            // IWebDriver driver = new ChromeDriver("C:/WebDriver/bin/");
            // string url = "https://localhost:5001";
            // driver.Url = url;1
            // driver.Navigate().GoToUrl(url);
            // driver.Quit();
        }
    }

    [TestClass]
    public class TestSubmitArticle
    {
        [TestMethod]
        public void TestSubmitArticleMethod()
        {
            IWebDriver driver = new ChromeDriver("C:/WebDriver/bin/");
            driver.Navigate().GoToUrl("https://localhost:5001/student/submitarticle");
            SubmitArticlePage submitArticlePage = new SubmitArticlePage(driver);
            HomePage homePage = submitArticlePage.SubmitArticle("Title so 1", "<h1>Content ne</h1>", "1");
            Assert.Equals(homePage.GetMessageText(), ("Article submitted"));
        }
    }
}
