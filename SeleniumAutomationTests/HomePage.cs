using System;
using OpenQA.Selenium;

namespace SeleniumAutomationTests
{
    public class HomePage
    {
        protected IWebDriver Driver;
        
        private By _messageBy = By.TagName("h1");

        public HomePage(IWebDriver driver){
            this.Driver = driver;
            if (!driver.Title.Equals("Home Page of logged in user")) {
                throw new Exception("This is not Home Page of logged in user," +
                                                " current page is: " + driver.PageSource);
            }
        }

        /**
    * Get message (h1 tag)
    *
    * @return String message text
    */
        public string GetMessageText() {
            return Driver.FindElement(_messageBy).Text;
        }

        public HomePage ManageProfile() {
            // Page encapsulation to manage profile functionality
            return new HomePage(Driver);
        }
        /* More methods offering the services represented by Home Page
        of Logged User. These methods in turn might return more Page Objects
        for example click on Compose mail button could return ComposeMail class object */
    }
}
