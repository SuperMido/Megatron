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
        private const string Url = "https://localhost:5001";

        [TestMethod]
        public void TestLoginMethod()
        {

            IWebDriver driver = null;
            if (_osPlatform.isWindows())
            {
                driver = new ChromeDriver("C:/WebDriver/bin") {Url = Url};
                driver.Navigate().GoToUrl(Url);
                var item = driver.FindElement(By.XPath("/html/body/div/main/div/h1")).Text;
                //Console.WriteLine(item);
                Assert.AreEqual( ("Welcome"),item);
                driver.Quit();
            }
            else if (_osPlatform.isMacOs())
            {
                var options = new ChromeOptions();
                options.AddArguments("--headless");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                driver = new ChromeDriver("/usr/local/bin", options) {Url = Url};
                driver.Navigate().GoToUrl(driver.Url);
                var item = driver.FindElement(By.XPath("/html/body/div/main/div/h1")).Text;
                Assert.AreEqual( ("Welcome"),item);
            }
            else
            {
                var options = new ChromeOptions();
                options.AddArguments("--headless");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                driver = new ChromeDriver("/usr/local/bin/chromedriver", options) {Url = Url};
                driver.Navigate().GoToUrl(driver.Url);
                var item = driver.FindElement(By.XPath("/html/body/div/main/div/h1")).Text;
                Assert.AreEqual( ("Welcome"),item);
            }
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
