using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using Assert = NUnit.Framework.Assert;

namespace SeleniumAutomationTests
{
    class TestIndex
    {
        string url = "https://megatron.gcd-gw.com/";
        IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArguments("--window-size=1325x744");
                options.AddArguments("--headless");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                driver = new ChromeDriver("/usr/local/bin/", options);
            }
            catch
            {
                driver = new ChromeDriver();
            }
        }

        [Test]
        public void test_GoToLoginPageFromIndexPageOnWebView()
        {
            driver.Url = url;
            driver.Manage().Window.Maximize();
            driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            string loginTitle = driver.FindElement(By.XPath("//*[@id=\"account\"]/span")).Text;
            Assert.AreEqual("Login", loginTitle);
            driver.Quit();
        }

        [Test]
        public void test_LoginPageOnWebView()
        {
            driver.Url = url;
            driver.Manage().Window.Maximize();

            driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            Actions actions = new Actions(driver);
            actions.Click(driver.FindElement(By.XPath("//*[@id=\"Input_Email\"]")))
                .SendKeys("megatronadmin@gmail.com" + Keys.Tab).SendKeys("Password@123").Build().Perform();

            driver.FindElement(By.XPath("//*[@id=\"account\"]/div[3]/div/button")).Click();

            Thread.Sleep(1000);
            string loggedInHeaderTitle =
                driver.FindElement(By.XPath("/html/body/div/div/aside/div[1]/nav/a[1]/div/span")).Text;
            Assert.AreEqual("Megatron", loggedInHeaderTitle);
            driver.Quit();
        }
    }
}