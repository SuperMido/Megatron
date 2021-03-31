using System.Collections.Generic;
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
        private const string Url = "https://megatron.gcd-gw.com/";
        private IWebDriver _driver;
        private IDictionary<string, object> Vars { get; set; }


        [SetUp]
        public void start_Browser()
        {
            var options = new ChromeOptions();
            options.AddArguments("start-maximized");
            options.AddArguments("--disable-gpu");
            options.AddArguments("--headless");
            _driver = new ChromeDriver(options);
            Vars = new Dictionary<string, object>();
        }

        [Test]
        public void test01_GoToLoginPageFromIndexPageOnWebView()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            var loginTitle = _driver.FindElement(By.XPath("//*[@id=\"account\"]/span")).Text;
            Assert.AreEqual("Login", loginTitle);
            _driver.Quit();
        }

        [Test]
        public void test02_LoginPageOnWebView()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.XPath("//*[@id=\"navbarResponsive\"]/ul/a")).Click();
            var actions = new Actions(_driver);
            actions.Click(_driver.FindElement(By.XPath("//*[@id=\"Input_Email\"]")))
                .SendKeys("megatronadmin@gmail.com" + Keys.Tab).SendKeys("Password@123").Build().Perform();

            _driver.FindElement(By.XPath("//*[@id=\"account\"]/div[3]/div/button")).Click();

            Thread.Sleep(500);
            var loggedInHeaderTitle =
                _driver.FindElement(By.XPath("/html/body/div/div/aside/div[1]/nav/a[1]/div/span")).Text;
            Assert.AreEqual("Megatron", loggedInHeaderTitle);
            _driver.Quit();
        }

        [Test]
        public void test03_Logout()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).Click();
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            _driver.FindElement(By.CssSelector(".d-md-inline-block")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            _driver.Quit();
        }

        [Test]
        public void test04_CreateFaculty()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".flex-column > .nav-item:nth-child(4) > .nav-link")).Click();
            _driver.FindElement(By.LinkText("Create New Faculty")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.Id("FacultyName")).Click();
            _driver.FindElement(By.Id("FacultyName")).SendKeys("Test Automation");
            Thread.Sleep(500);
            _driver.FindElement(By.Id("Description")).Click();
            _driver.FindElement(By.Id("Description")).SendKeys("testing only");
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector(".btn")).Click();
            _driver.Quit();
        }

        [Test]
        public void test05_CreateArticle()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".flex-column > .nav-item:nth-child(2) > .nav-link")).Click();
            _driver.FindElement(By.LinkText("Create New Article")).Click();
            _driver.FindElement(By.Id("Article_Title")).SendKeys("Test create article automation");
            _driver.FindElement(By.Id("Article_FacultyId")).Click();
            {
                var dropdown = _driver.FindElement(By.Id("Article_FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            _driver.FindElement(By.Id("termAndCondition")).Click();
            _driver.Quit();
        }

        [Test]
        public void test06_ArticleReview()
        {
            _driver.Navigate().GoToUrl(Url);

            Thread.Sleep(500);
            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).Click();
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-item:nth-child(1) > .nav-link > span")).Click();
            _driver.FindElement(By.LinkText("View Article")).Click();
            _driver.Quit();
        }

        [Test]
        public void test07_CreateSemester()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-item:nth-child(5) span")).Click();
            _driver.FindElement(By.LinkText("Create New Semester")).Click();
            _driver.FindElement(By.Id("Semester_SemesterName")).Click();
            _driver.FindElement(By.Id("Semester_SemesterName")).SendKeys("Test semester automation");
            Actions actions = new Actions(_driver);
            actions.Click(_driver.FindElement(By.Id("Semester_SemesterStartDate")))
                .SendKeys("19-04-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();

            Actions actions2 = new Actions(_driver);
            actions2.Click(_driver.FindElement(By.Id("Semester_SemesterClosureDate")))
                .SendKeys("24-04-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();

            Actions actions3 = new Actions(_driver);
            actions3.Click(_driver.FindElement(By.Id("Semester_SemesterEndDate")))
                .SendKeys("30-04-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();
            
            _driver.Quit();
        }
        
        [Test]
        public void test08_CreateUser()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-item:nth-child(6) > .nav-link")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.LinkText("Create New User")).Click();
            _driver.FindElement(By.Id("Input_FullName")).Click();
            _driver.FindElement(By.Id("Input_FullName")).SendKeys("Selenium");
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("automationtesting@gmail.com");
            _driver.FindElement(By.Id("Input_ConfirmEmail")).Click();
            _driver.FindElement(By.Id("Input_ConfirmEmail")).SendKeys("automationtesting@gmail.com");
            _driver.FindElement(By.Id("Input_PhoneNumber")).Click();
            _driver.FindElement(By.Id("Input_PhoneNumber")).SendKeys("0944444444");
            _driver.FindElement(By.Id("Input_Password")).Click();
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.Id("Input_ConfirmPassword")).Click();
            _driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector("input:nth-child(4)")).Click();
            _driver.Quit();
        }

        [Test]
        public void test09_AssignMC()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-item:nth-child(7) > .nav-link")).Click();
            _driver.FindElement(By.LinkText("Assign Marketing Coordinator")).Click();
            _driver.FindElement(By.Id("UserId")).Click();
            Thread.Sleep(500);
            {
                var dropdown = _driver.FindElement(By.Id("UserId"));
                dropdown.FindElement(By.XPath("//option[. = 'Megatron Marketing Coordinator']")).Click();
            }
            _driver.FindElement(By.Id("FacultyId")).Click();
            Thread.Sleep(500);
            {
                var dropdown = _driver.FindElement(By.Id("FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            _driver.Quit();
        }

        [Test]
        public void test10_AssignGuest()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-item:nth-child(7) span")).Click();
            _driver.FindElement(By.LinkText("Assign Guest")).Click();
            _driver.FindElement(By.Id("FacultyId")).Click();
            Thread.Sleep(500);
            {
                var dropdown = _driver.FindElement(By.Id("FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            _driver.Quit();
        }

        [Test]
        public void test11_loginEachRoles()
        {
            _driver.Navigate().GoToUrl(Url);

            _driver.FindElement(By.LinkText("Login")).Click();
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.XPath("//span[contains(.,\'megatronadmin@gmail.com\')]")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronmc@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.XPath("//span[contains(.,\'megatronmc@gmail.com\')]")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.Id("Input_Email")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronstudent@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.XPath("//span[contains(.,\'megatronstudent@gmail.com\')]")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            _driver.Quit();
        }

        [Test]
        public void test12_PhoneResponsive()
        {
            _driver.Manage().Window.Size = new System.Drawing.Size(375, 812);

            _driver.Navigate().GoToUrl(Url);
            
            _driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(500);
            _driver.FindElement(By.Id("Input_Email")).Click();
            _driver.FindElement(By.Id("Input_Email")).SendKeys("megatronstudent@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            _driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            _driver.FindElement(By.CssSelector(".nav-link-icon > .material-icons")).Click();
            _driver.Quit();
        }
    }
}