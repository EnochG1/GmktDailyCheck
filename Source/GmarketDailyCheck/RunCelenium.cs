using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GmarketDailyCheck
{
	class RunCelenium
	{
		static void Main(string[] args)
		{
			var id = Convert.ToString(ConfigurationManager.AppSettings["ID"]);
			var pw = Convert.ToString(ConfigurationManager.AppSettings["PW"]);
			var chromePath = Convert.ToString(ConfigurationManager.AppSettings["ChromePath"]);

			Console.WriteLine("ID : " + id);

			if (string.IsNullOrEmpty(id))
			{
				Console.WriteLine("ID value is Empty. Check GmarketDailyCheck.exe.config");
				Console.ReadLine();
				Environment.Exit(0);
			}

			IWebDriver driver;
			if (string.IsNullOrEmpty(chromePath))
				driver = new ChromeDriver();
			else
			{
				var options = new ChromeOptions
				{
					BinaryLocation = chromePath
				};
				driver = new ChromeDriver(options);
			}

			try
			{
				driver.Url = "https://signinssl.gmarket.co.kr/LogOut/LogOut";

				IWebElement element;
				//element = driver.FindElement(By.Id("css_login_box"));
				//element.Click();

				WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
				waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("id")));

				element = driver.FindElement(By.Id("id"));
				element.SendKeys(id);
				element = driver.FindElement(By.Id("pwd"));
				element.SendKeys(pw);
				element = driver.FindElement(By.CssSelector("button[title='login']"));
				element.Click();

				driver.Navigate().GoToUrl("http://promotion.gmarket.co.kr/Event/pluszone.asp");

				waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
				waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("AttendRulletFrame")));

				driver.SwitchTo().Frame(driver.FindElement(By.Id("AttendRulletFrame")));
				element = driver.FindElement(By.CssSelector("a.button_start"));
				element.Click();

				AdditionalAttendBonus(driver);
			}
			catch (UnhandledAlertException e)
			{
				IAlert alert = ExpectedConditions.AlertIsPresent().Invoke(driver);
				driver.SwitchTo().Alert().Accept();

				AdditionalAttendBonus(driver);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		private static void AdditionalAttendBonus(IWebDriver driver)
		{
			driver.SwitchTo().DefaultContent();
			driver.SwitchTo().Frame(driver.FindElement(By.Id("AttendcalendarFrame")));

			string attendDays = driver.FindElement(By.CssSelector("span.date")).Text;

			driver.SwitchTo().DefaultContent();

			if (int.Parse(attendDays) >= 10)
			{
				IWebElement element = driver.FindElement(By.CssSelector("img[alt='10회이상 출석시 100 Smile Point']"));
				element.Click();

				Thread.Sleep(3000);
				if (int.Parse(attendDays) >= 15)
				{
					element = driver.FindElement(By.CssSelector("img[alt='15회이상 출석시 200 Smile Point']"));
					element.Click();
				}
			}
		}
	}
}
