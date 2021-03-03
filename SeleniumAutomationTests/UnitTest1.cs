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
        IWebDriver _driver;

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
                _driver = new ChromeDriver("/usr/local/bin/", options);
            }
            catch
            {
                _driver = new ChromeDriver();
            }
        }

        [Test]
        public void test_GoToIndex()
        {
            _driver.Url = url;
            string homeTitle = _driver.FindElement(By.XPath("/html/body/header/div[1]/div/h1")).Text;
            Assert.AreEqual("One Page Wonder", homeTitle);
            _driver.Quit();
        }

        [Test]
        public void test_GoToLoginOnWebView()
        {
            _driver.Url = url;
            _driver.Manage().Window.Maximize();
            _driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            string loginTitle = _driver.FindElement(By.XPath("//*[@id=\"account\"]/span")).Text;
            Assert.AreEqual("Login", loginTitle);
            _driver.Quit();
        }

        [Test]
        public void test_LoginOnWebView()
        {
            _driver.Url = url;
            _driver.Manage().Window.Maximize();

            _driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            Actions actions = new Actions(_driver);
            actions.Click(_driver.FindElement(By.XPath("//*[@id=\"Input_Email\"]")))
                .SendKeys("megatronadmin@gmail.com" + Keys.Tab).SendKeys("Password@123").Build().Perform();

            _driver.FindElement(By.XPath("//*[@id=\"account\"]/div[3]/div/button")).Click();

            Thread.Sleep(1000);
            string loggedInHeaderTitle =
                _driver.FindElement(By.XPath("/html/body/div/div/aside/div[1]/nav/a[1]/div/span")).Text;
            Assert.AreEqual("Megatron", loggedInHeaderTitle);
            _driver.Quit();
        }
    }
}