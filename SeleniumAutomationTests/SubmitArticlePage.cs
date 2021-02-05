using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomationTests
{
    public class SubmitArticlePage
    {
        protected IWebDriver Driver;
        
        private By _titleBy = By.Name("title");
        private By _contentBy = By.Name("content");
        private By _facultyBy = By.Name("faculty");
        private By _submitBy = By.Name("submit");

        public SubmitArticlePage(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public HomePage SubmitArticle(string title, string content, string faculty)
        {
            Driver.FindElement(_titleBy).SendKeys(title);
            Driver.FindElement(_contentBy).SendKeys(content);
            Driver.FindElement(_facultyBy).SendKeys(faculty);
            Driver.FindElement(_submitBy).Click();
            return new HomePage(Driver);
        }
    }
}
