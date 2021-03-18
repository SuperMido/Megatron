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
        string url = "https://megatron.gcd-gw.com/";
        IWebDriver driver;
        IDictionary<string, object> vars { get; set; }


        [SetUp]
        public void start_Browser()
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArguments("--window-size=1440x800");
                options.AddArguments("--headless");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                driver = new ChromeDriver("/usr/local/bin/chromedriver", options);
                vars = new Dictionary<string, object>();
            }
            catch
            {
                driver = new ChromeDriver();
            }
        }

        [Test]
        public void test_GoToIndex()
        {
            driver.Url = url;
            string homeTitle = driver.FindElement(By.XPath("/html/body/header/div[1]/div/h1")).Text;
            Assert.AreEqual("Megatron", homeTitle);
            driver.Quit();
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

        [Test]
        public void test_Logout()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.Id("Input_Password")).SendKeys(Keys.Enter);
            driver.FindElement(By.CssSelector(".d-md-inline-block")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            driver.Quit();
        }


        [Test]
        public void test_CreateArticle()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".flex-column > .nav-item:nth-child(2) > .nav-link")).Click();
            driver.FindElement(By.LinkText("Create New Article")).Click();
            driver.FindElement(By.Id("Article_Title")).SendKeys("Test create article automation");
            driver.FindElement(By.Id("Article_FacultyId")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Article_FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            driver.FindElement(By.Id("termAndCondition")).Click();
            driver.FindElement(By.Id("submitButton")).Click();
            Thread.Sleep(1000);
            string createArticle =
                driver.FindElement(By.XPath("//h3[contains(text(),'Article Overview')]")).Text;
            Assert.AreEqual("Article Overview", createArticle);
            driver.Quit();
        }

        [Test]
        public void test_ArticleReview()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-item:nth-child(1) > .nav-link > span")).Click();
            driver.FindElement(By.LinkText("View Article")).Click();
            driver.FindElement(By.CssSelector(".page-title")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("#\\31 5 > td:nth-child(1)")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[contains(text(),'Take review')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("status")).Click();
            {
                var dropdown = driver.FindElement(By.Id("status"));
                dropdown.FindElement(By.XPath("//option[. = 'Decline']")).Click();
            }
            Thread.Sleep(1000);
            driver.FindElement(By.Id("statusMessage")).Click();
            driver.FindElement(By.Id("statusMessage")).SendKeys("test decline");
            driver.FindElement(By.Id("UpdateStatusButton")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[contains(text(),'Take review')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//select[@id='status']")).Click();
            {
                var dropdown = driver.FindElement(By.Id("status"));
                dropdown.FindElement(By.XPath("//option[. = 'Approve']")).Click();
            }
            Thread.Sleep(1000);
            driver.FindElement(By.Id("statusMessage")).Click();
            driver.FindElement(By.Id("statusMessage")).SendKeys("test approve");
            driver.FindElement(By.Id("UpdateStatusButton")).Click();
            driver.Quit();
        }

        [Test]
        public void test_CreateFaculty()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".flex-column > .nav-item:nth-child(4) > .nav-link")).Click();
            driver.FindElement(By.LinkText("Create New Faculty")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("FacultyName")).Click();
            driver.FindElement(By.Id("FacultyName")).SendKeys("Test Automation");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).SendKeys("testing only");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn")).Click();

            Thread.Sleep(1000);
            string createFaculty =
                driver.FindElement(By.XPath("//td[contains(text(),'Test Automation')]")).Text;
            Assert.AreEqual("Test Automation", createFaculty);
            driver.Quit();
        }

        [Test]
        public void test_CreateSemester()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-item:nth-child(5) span")).Click();
            driver.FindElement(By.LinkText("Create New Semester")).Click();
            driver.FindElement(By.Id("Semester_SemesterName")).Click();
            driver.FindElement(By.Id("Semester_SemesterName")).SendKeys("Test semester automation");
            Actions actions = new Actions(driver);
            actions.Click(driver.FindElement(By.Id("Semester_SemesterStartDate")))
                .SendKeys("31-03-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();

            Actions actions2 = new Actions(driver);
            actions2.Click(driver.FindElement(By.Id("Semester_SemesterClosureDate")))
                .SendKeys("24-04-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();

            Actions actions3 = new Actions(driver);
            actions3.Click(driver.FindElement(By.Id("Semester_SemesterEndDate")))
                .SendKeys("30-04-2021" + Keys.Tab).SendKeys("22:38").Build().Perform();

            driver.FindElement(By.CssSelector(".btn")).Click();
            Thread.Sleep(1000);
            string createSemester =
                driver.FindElement(By.XPath("//td[contains(text(),'Test semester automation')]")).Text;
            Assert.AreEqual("Test semester automation", createSemester);
            driver.Quit();
        }

        [Test]
        public void test_CreateUser()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-item:nth-child(6) > .nav-link")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Create New User")).Click();
            driver.FindElement(By.Id("Input_FullName")).Click();
            driver.FindElement(By.Id("Input_FullName")).SendKeys("Selenium");
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("automationtesting@gmail.com");
            driver.FindElement(By.Id("Input_ConfirmEmail")).Click();
            driver.FindElement(By.Id("Input_ConfirmEmail")).SendKeys("automationtesting@gmail.com");
            driver.FindElement(By.Id("Input_PhoneNumber")).Click();
            driver.FindElement(By.Id("Input_PhoneNumber")).SendKeys("0944444444");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.Id("Input_ConfirmPassword")).Click();
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector("input:nth-child(4)")).Click();
            driver.FindElement(By.CssSelector(".btn")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".nav-item:nth-child(6) > .nav-link")).Click();
            Thread.Sleep(1000);
            string createUser =
                driver.FindElement(By.XPath("//td[contains(text(),'Selenium')]")).Text;
            Assert.AreEqual("Selenium", createUser);
            driver.Quit();
        }

        [Test]
        public void test_AssignMC()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-item:nth-child(7) > .nav-link")).Click();
            driver.FindElement(By.LinkText("Assign Marketing Coordinator")).Click();
            driver.FindElement(By.Id("UserId")).Click();
            Thread.Sleep(1000);
            {
                var dropdown = driver.FindElement(By.Id("UserId"));
                dropdown.FindElement(By.XPath("//option[. = 'Megatron Marketing Coordinator']")).Click();
            }
            driver.FindElement(By.Id("FacultyId")).Click();
            Thread.Sleep(1000);
            {
                var dropdown = driver.FindElement(By.Id("FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn-info")).Click();
            string assignMc =
                driver.FindElement(By.XPath("//h3[contains(text(),'Assign user into Faculty')]")).Text;
            Assert.AreEqual("Assign user into Faculty", assignMc);
            driver.Quit();
        }

        [Test]
        public void test_AssignGuest()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-item:nth-child(7) span")).Click();
            driver.FindElement(By.LinkText("Assign Guest")).Click();
            driver.FindElement(By.Id("FacultyId")).Click();
            Thread.Sleep(1000);
            {
                var dropdown = driver.FindElement(By.Id("FacultyId"));
                dropdown.FindElement(By.XPath("//option[. = 'IT']")).Click();
            }
            driver.FindElement(By.CssSelector(".btn-info")).Click();
            Thread.Sleep(1000);
            string assignMc =
                driver.FindElement(By.XPath("//h3[contains(text(),'Assign user into Faculty')]")).Text;
            Assert.AreEqual("Assign user into Faculty", assignMc);
            driver.Quit();
        }

        [Test]
        public void test_loginEachRoles()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(1440, 800);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronadmin@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'megatronadmin@gmail.com\')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronmc@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'megatronmc@gmail.com\')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronmm@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.LinkText("megatronmm@gmail.com")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronstudent@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'megatronstudent@gmail.com\')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".form-inline > .dropdown-item")).Click();
            driver.Quit();
        }

        [Test]
        public void test_PhoneResponsive()
        {
            driver.Navigate().GoToUrl("https://megatron.gcd-gw.com/");
            driver.Manage().Window.Size = new System.Drawing.Size(375, 812);
            driver.FindElement(By.CssSelector(".navbar-toggler-icon")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("megatronstudent@gmail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Password@123");
            driver.FindElement(By.CssSelector(".login100-form-btn")).Click();
            driver.FindElement(By.CssSelector(".nav-link-icon > .material-icons")).Click();
            driver.Quit();
        }
    }
}