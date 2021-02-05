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
            string url = "https://localhost:5001";
            try
            {
                IWebDriver driver = new ChromeDriver("C:/WebDriver/bin/");
                driver.Url = url;
                driver.Navigate().GoToUrl(url);
                driver.Quit();
            }
            catch
            {
                var options = new ChromeOptions();
                options.AddArguments("--headless");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                IWebDriver driver = new ChromeDriver("/usr/local/bin/",options);
                //driver.Url = url;
                int count = 0;
                while (count <= 1)
                {
                    try
                    {
                        driver.Url = url;
                        driver.Navigate().GoToUrl(driver.Url);
                        string item = driver.FindElement(By.XPath("/html/body/div/main/div/p")).Text;
                        //if (item.Equals("Welcome")) {
                        //    throw new Exception("Success");
                        //}
                        //else Console.WriteLine("Failed");
                        Assert.Equals(item, ("Welcome"));
                        break;
                    }
                    catch
                    {
                        count+=1;
                    }    
                }
                
                //driver.Navigate().GoToUrl("https://google.com");
                //driver.Quit();
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
