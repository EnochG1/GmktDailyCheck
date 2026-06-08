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
				waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("typeMemberInputId")));

				element = driver.FindElement(By.Id("typeMemberInputId"));
				element.SendKeys(id);
				element = driver.FindElement(By.Id("typeMemberInputPassword"));
				element.SendKeys(pw);
				element = driver.FindElement(By.CssSelector("button[id='btn_memberLogin']"));
				element.Click();

				DailyCheck(driver);
			}
			catch (UnhandledAlertException e)
			{
				IAlert alert = ExpectedConditions.AlertIsPresent().Invoke(driver);
				driver.SwitchTo().Alert().Accept();

				DailyCheck(driver);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		private static void DailyCheck(IWebDriver driver)
		{
			driver.Navigate().GoToUrl("https://www.gmarket.co.kr/n/smilehome");

			WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			IWebElement element = waitForElement.Until(
				ExpectedConditions.ElementExists(By.CssSelector("button.button__daily-check")));

			// smilehome은 모바일 레이아웃 기반이라 데스크톱 너비에서 버튼이 숨겨질 수 있어
			// JavaScript로 스크롤 후 직접 클릭한다.
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
			js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
			js.ExecuteScript("arguments[0].click();", element);

			Thread.Sleep(3000);

			// 출석체크 결과 안내 알럿이 뜨면 확인 처리
			IAlert alert = ExpectedConditions.AlertIsPresent().Invoke(driver);
			if (alert != null)
				alert.Accept();
		}
	}
}