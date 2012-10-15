using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Threading;

namespace Task___Test_registration_form
{
    [TestFixture]
    public class Class1
    {
        IWebDriver driver;
        IWebElement webelement;
        string URL;

        [SetUp]
        public void SetupTest() //actions before tests
        {
            driver = new ChromeDriver();           
            URL = "http://prom.ua/join-now?path_id=txt.register"; //basic URL
        }

        [TearDown]
        public void TeardownTest() //actions after tests
        {
            try { driver.Quit(); } //quit from driver, close all windows which are related to it
            catch (Exception) { } //ignore errors if can't quit
        }

        //function for latin letters' generation (a-z)
        public string Generate_latin_letters(int str_length) // int str_length - string's length you'd like to get
        {            
            string result = "";
            char[] mas = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Random random = new Random();
            int next;

            for (int i = 0; i < str_length; i++)
            {
                next = random.Next(0, 26); //getting the random index
                result += mas[next]; //getting the symbol from massive by random generated index
            }
            return result;
        }

        //function for cyrillic letters' generation (а-я) 
        public string Generate_cyril_letters(int str_length) 
        {
            string result = "";
            char[] mas = new char[33] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };
            Random random = new Random();
            int next;

            for (int i = 0; i < str_length; i++)
            {
                next = random.Next(0, 33); //getting the random index
                result += mas[next]; //getting the symbol from massive by random generated index
            }
            return result;
        }

        //function for numbers' generation (0-9) 
        public string Generate_numbers(int str_length) 
        {
            string result = "";            
            Random random = new Random();

            for (int i = 0; i < str_length; i++)
            {
                result += random.Next(0, 10); //generating the random number
            }
            return result;
        }

        //function for special symbols' generation (!@#$%^)
        public string Generate_special_symb(int str_length)  
        {
            string result = "";
            char[] mas = new char[16] { ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/'};
            Random random = new Random();
            int next;

            for (int i = 0; i < str_length; i++)
            {
                next = random.Next(0, 16); //getting the random index
                result += mas[next]; //getting the symbol from massive by random generated index
            }
            return result;
        }
        
        #region Positive tests
        [Test]
        public void _1() //test #1 in Excel-file
        {
            driver.Navigate().GoToUrl(URL); //go to the set URL
            Thread.Sleep(5000);

            webelement = driver.FindElement(By.Id("company_name"));
            webelement.SendKeys(Generate_cyril_letters(7) + " " + Generate_cyril_letters(3)); //sending generated random symbols

            webelement = driver.FindElement(By.Id("first_name"));
            webelement.SendKeys(Generate_cyril_letters(10));

            webelement = driver.FindElement(By.Id("last_name"));
            webelement.SendKeys(Generate_cyril_letters(10));

            webelement = driver.FindElement(By.Id("email"));
            webelement.SendKeys(Generate_latin_letters(5) + "@" + Generate_latin_letters(3) + "." + Generate_latin_letters(3));

            webelement = driver.FindElement(By.Id("password"));
            webelement.SendKeys(Generate_cyril_letters(10));
                        
            webelement = driver.FindElement(By.Id("register_button"));
            webelement.Click();

            Thread.Sleep(5000);
            string title = driver.Title;
            try { Assert.True(title.Contains("Кабинет компании")); }//check that we are exactly on personal page
            catch { Assert.True(false, "1st test failed"); }
        }
        #endregion

        #region Negative tests
        [Test]
        public void _15() //test #15 in Excel-file
        {
            WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));

            driver.Navigate().GoToUrl(URL); //go to the set URL
            Thread.Sleep(5000);

            webelement = driver.FindElement(By.Id("company_name"));
            webelement.SendKeys(Generate_cyril_letters(1)); //sending generated random symbols

            webelement = driver.FindElement(By.Id("first_name"));
            webelement.SendKeys(Generate_cyril_letters(1));

            webelement = driver.FindElement(By.Id("last_name"));
            webelement.SendKeys(Generate_cyril_letters(1));

            webelement = driver.FindElement(By.Id("email"));
            webelement.SendKeys(Generate_latin_letters(5) + "@" + Generate_latin_letters(3) + "." + Generate_latin_letters(3));

            webelement = driver.FindElement(By.Id("password")); // password's length = 2 symbols
            webelement.SendKeys(Generate_cyril_letters(2));

            webelement = driver.FindElement(By.Id("register_button"));
            webelement.Click();

            Thread.Sleep(5000);
            try { wait.Until(ExpectedConditions.ElementExists(By.Id("password_error"))); Assert.True(true); } //check for error-message appearence (about password's length)
            catch { Assert.True(false, "8th test failed"); }
        }

        [Test]
        public void _31() //test #31 in Excel-file
        {
            driver.Navigate().GoToUrl(URL); //go to the set URL
            Thread.Sleep(5000);

            webelement = driver.FindElement(By.Id("company_name"));
            webelement.SendKeys(Generate_cyril_letters(3) + Generate_latin_letters(4) + " " + Generate_numbers(5) + " " + Generate_special_symb(5)); //sending generated random symbols

            webelement = driver.FindElement(By.Id("first_name"));
            webelement.SendKeys(Generate_cyril_letters(4) + Generate_latin_letters(4) + Generate_numbers(5) + " " + Generate_special_symb(5));

            webelement = driver.FindElement(By.Id("last_name"));
            webelement.SendKeys(Generate_cyril_letters(3) + Generate_latin_letters(4) + " " + Generate_numbers(5) + Generate_special_symb(5));

            webelement = driver.FindElement(By.Id("email"));
            webelement.SendKeys(Generate_latin_letters(5) + "@" + Generate_latin_letters(3) + "." + Generate_latin_letters(3));

            webelement = driver.FindElement(By.Id("password"));
            webelement.SendKeys(Generate_cyril_letters(5) + Generate_latin_letters(8) + " " + Generate_numbers(5) + " " + Generate_special_symb(5) + "   ");

            webelement = driver.FindElement(By.Id("register_button"));
            webelement.Click();

            Thread.Sleep(5000);
            string title = driver.Title;
            try { Assert.True(title.Contains("Кабинет компании")); }//check that we are exactly on personal page
            catch { Assert.True(false, "31st test failed"); }
        }

        [Test]
        public void _32() //test #32 in Excel-file
        {
            driver.Navigate().GoToUrl(URL); //go to the set URL
            Thread.Sleep(5000);

            string company1 = Generate_cyril_letters(3) + Generate_latin_letters(4) + " " + Generate_numbers(5) + " " + Generate_special_symb(5); //save the generated symbols to check equivalence leter
            webelement = driver.FindElement(By.Id("company_name"));
            webelement.SendKeys(company1); //sending generated random and then saved symbols

            string name1 = Generate_cyril_letters(4) + Generate_latin_letters(4) + Generate_numbers(5) + " " + Generate_special_symb(5); //save the generated symbols to check equivalence leter
            webelement = driver.FindElement(By.Id("first_name"));
            webelement.SendKeys(name1);//sending generated random and then saved symbols

            string surname = Generate_cyril_letters(3) + Generate_latin_letters(4) + " " + Generate_numbers(5) + Generate_special_symb(5); //save the generated symbols to check equivalence leter
            webelement = driver.FindElement(By.Id("last_name"));
            webelement.SendKeys(surname);//sending generated random and then saved symbols

            webelement = driver.FindElement(By.Id("email"));
            webelement.SendKeys(Generate_latin_letters(5) + "@" + Generate_latin_letters(3) + "." + Generate_latin_letters(3));

            webelement = driver.FindElement(By.Id("password"));
            webelement.SendKeys(Generate_cyril_letters(5) + Generate_latin_letters(8) + " " + Generate_numbers(5) + " " + Generate_special_symb(5) + "   ");

            webelement = driver.FindElement(By.Id("register_button"));
            webelement.Click();

            Thread.Sleep(5000);
            string company2 = driver.FindElement(By.Id("name")).GetAttribute("value"); //finding out the value in the input-field (the company name)
            string name2 = driver.FindElement(By.Id("contact_person")).GetAttribute("value"); //finding out the value in the input-field (the name + surname)

            Assert.IsTrue(company1 == company2 && (name1 + " " + surname) == name2); //checking the equivalence between strings which were inputed on Registr.page and strings which are on Pers.page in input-fields           
        }
        #endregion
    }
}
