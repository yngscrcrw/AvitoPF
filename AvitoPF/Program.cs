using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SeleniumExtras.WaitHelpers;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading;
using System.Reflection;

class User
{
    string login;
    string password;
    public User(string login, string password)
    {
        this.login = login;
        this.password = password;

    }
    public void Autentification(IWebDriver driver)
    {
        try
        {
            Console.WriteLine("Нажатие на кнопку логина");
            IWebElement loginButton = driver.FindElement(By.ClassName("index-module-login-K8jzD"));
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.WriteLine("Ожидание поля логина");
            IWebElement loginInputElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='login']")));
            loginInputElement.Clear();
            loginInputElement.SendKeys(this.login);

            Console.WriteLine("Ожидание поля пароля");
            IWebElement passwordInputElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='password']")));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys(this.password);

            Console.WriteLine("Нажатие на кнопку отправки формы");
            IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[type='submit']")));
            submitButton.Click();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            throw;
        }
    }
}
class Driver
{
    IWebDriver driver;
    public Driver(string url)
    {
        List<string> userAgents = new List<string>
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Safari/605.1.15",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0"
        };
        Random rand = new Random();
        string randomUserAgent = userAgents[rand.Next(userAgents.Count)];

        string profilePath = @"C:\Users\Anton\AppData\Local\Google\Chrome\User Data\Profile 2";

        var options = new ChromeOptions();
        //options.AddArgument("--headless");
        options.AddArgument($"user-data-dir={profilePath}");
        options.AddArgument("--start-maximized");
        options.AddArguments("--use-fake-ui-for-media-stream");
        options.AddArgument($"user-agent={randomUserAgent}");
        options.AddHttpProxy("46.138.250.108", 64041, "ezuhUB", "syQ5YKhYZ5Ra");

        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl(url);
    }
    public void DeleteDriver()
    {
        driver.Quit();
        driver.Dispose();
    }
    public IWebDriver getDriver()
    { return driver; }
}

class PFAction
{
    private protected Random rand = new Random();
    private protected IWebElement PFElement;
    private protected IWebDriver driver;
    public PFAction(IWebDriver driver)
    {
        this.driver = driver;
    }

    public virtual void Perform()
    {

    }
}

class Favorites : PFAction
{

    public Favorites(IWebDriver driver) : base(driver)
    {

    }

    public override void Perform()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            IWebElement favElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-marker='item-view/add-note-button']")));

            if (favElement.Text == "Добавить в избранное")
            {
                favElement.Click();
                Console.WriteLine("Кнопка нажата.");
            }
            else
            {
                Console.WriteLine("Кнопка уже нажата.");
            }
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Кнопка 'Добавить в избранное' не найдена.");
        }
    }
}

class Notes : PFAction
{
    string note;
    public Notes(IWebDriver driver) : base(driver)
        {
        }
    public override void Perform()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            IWebElement noteElement = driver.FindElement(By.ClassName("style-item-notes-container-hNkRg"));
            note = driver.FindElement(By.XPath("//div[@itemscope and @itemtype='http://schema.org/BreadcrumbList']//span[@itemprop='itemListElement'][last()]//a")).Text;
            note = note + " " + driver.FindElement(By.ClassName("style-item-address__string-wt61A")).Text;
            noteElement.Click();

            int delay = rand.Next(1000, 10000);
            Thread.Sleep(delay);

            IWebElement inputContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[data-marker='item-note/text-input/input']")));
            inputContainer.SendKeys(note);

            Thread.Sleep(delay);

            IWebElement submitButton = driver.FindElement(By.CssSelector("div.styles-module-root-hbGhb button[data-marker='item-note/save-note-button']"));
            submitButton.Click();
            Console.WriteLine("Заметка записана.");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Кнопка 'Добавить в заметки' не найдена.");
        }    
    }
}

class Call : PFAction
{
    public Call(IWebDriver driver) : base(driver)
    {

    }
    public override void Perform()
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement callElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-marker='item-phone-button/card']")));
            callElement.Click();
        }
        catch(WebDriverTimeoutException)
        {
            Console.WriteLine("Кнопка показать телефон не найдена");
        }
            //IWebElement eCallElement = driver.FindElement(By.ClassName("styles-module-text_size_m-toJYF"));
            //eCallElement.Click();
        }
}

class Text : PFAction
{
    IWebElement submitTextElement;
    List<IWebElement> textElements;
    public Text(IWebDriver driver) : base(driver)
    { 
    }
    public override void Perform()
    {
        IWebElement textElement = driver.FindElement(By.ClassName("extended-input-extended-input-container-NGyjU"));
        submitTextElement = driver.FindElement(By.ClassName("extended-input-inputIcon-v0B9s"));
        ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.ClassName("desktop-19ozr6b"));
        textElements = new List<IWebElement>(elements);
        int delay = rand.Next(1000, 10000);

        //Console.WriteLine("Введите текст для сообщения: (если оставить пустую строку отправится сообщение по умолчанию)");
        //string text = Console.ReadLine();
        string text = "";

        if (text != "")
        {
            textElement.SendKeys(text);
            Thread.Sleep(delay);
            submitTextElement.Click();
        }
        else
        {
            int num = rand.Next(1, 4);
            switch (num)
            {
                case 1:
                    textElements[0].Click();
                    Thread.Sleep(delay);
                    submitTextElement.Click();
                    break;
                case 2:
                    textElements[1].Click();
                    Thread.Sleep(delay);
                    submitTextElement.Click();
                    break;
                case 3:
                    textElements[2].Click();
                    Thread.Sleep(delay);
                    submitTextElement.Click();
                    break;
                case 4:
                    textElements[3].Click();
                    Thread.Sleep(delay);
                    submitTextElement.Click();
                    break;
            }
        }
    }
}

class Subscribe : PFAction 
{

    public Subscribe(IWebDriver driver) : base(driver)
    {
    }
    public override void Perform()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            IWebElement SubscribeElement = driver.FindElement(By.CssSelector("button[data-marker='favorite-seller-subscription-button']"));
            if (SubscribeElement.Text == "Подписаться на продавца")
            {
                SubscribeElement.Click();
                Console.WriteLine("Подписка оформлена.");
            }
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Кнопка 'Подписаться на продавца' не найдена.");
        }

    }
}

class Photo : PFAction
{
    List<IWebElement> photoElements;
    public Photo(IWebDriver driver) :base(driver)
    {
    }
    public override void  Perform()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        int delay = rand.Next(1000, 10000);

        try
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.ClassName("images-preview-previewImageWrapper-RfThd"));
            photoElements = new List<IWebElement>(elements);
            foreach (IWebElement photoElement in photoElements)
            {
                photoElement.Click();
                Thread.Sleep(delay);
            }
            Console.WriteLine("Фото просмотрены.");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Фото не найдены.");
        }
    }
}
class AdFinder
{
    public AdFinder(string id, IWebDriver driver)
    {
        Thread.Sleep(10000);
        string itemId = "";
        int pagesNum = Convert.ToInt32(driver.FindElement(By.ClassName("styles-module-item_last-iHvU3")).GetAttribute("data-value"));
        
        bool found = false;

        for (int i = 0; i < pagesNum && !found; i++)
        {
            var adElements = driver.FindElements(By.ClassName("iva-item-list-rfgcH"));
            foreach (var adElement in adElements)
            {
                itemId = adElement.GetAttribute("id").Substring(1);
                Console.WriteLine(itemId);

                if (itemId == id)
                {
                    adElement.Click();
                    found = true;
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    break;
                }
            }

            if (!found)
            {
                IWebElement nextPageElement = driver.FindElement(By.ClassName("styles-module-listItem_arrow_next-aB2Kl"));
                nextPageElement.Click();
            }
        }

        if (!found)
        {
            Console.WriteLine("Объявление не найдено");
        }

    }
}
class Query
{
    private string query;
    private IWebDriver driver;
    public Query(string query, IWebDriver driver)
    {
        this.query = query;
        this.driver = driver;
    }
    public void Perform()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

        IWebElement inputElement = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("index-form-ENoC5")));
        inputElement.Click();
        IWebElement queryInput = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[data-marker='search-form/suggest']")));
        queryInput.SendKeys(query);
        Thread.Sleep(10000);
        IWebElement queryButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[data-marker='suggest/list/dropdown'] button[data-marker='suggest/list/custom-option']:first-child")));
        queryButton.Click();
    }
}

public class Program
{
    public static void Main()
    {
        string area = "";
        Console.WriteLine("Ваш регион: \n 1)Москва \n 2)Москва и область \n 3)Санкт-Петербург");
        Console.WriteLine("Ввдете число: ");
        string areaNum = Console.ReadLine();
        switch (areaNum)
        {
            case "1":
                area = "/moskva?forceLocation=1";
                break;
            case "2":
                area = "/moskva_i_mo?forceLocation=1";
                break;
            case "3":
                area = "/sankt-peterburg?forceLocation=1";
                break;
            default:
                Console.WriteLine("поиск выполняется по всем регионам");
                break;
        }

        Console.WriteLine("Введите id нужного объявления: ");
        string id = Console.ReadLine();
        Console.WriteLine("Введите запрос: ");
        string adQuery = Console.ReadLine();
        string url = "https://www.avito.ru" + area;
        Driver driver = new Driver(url);
        Query query = new Query(adQuery, driver.getDriver());
        query.Perform();
        List<PFAction> actionList = new List<PFAction>();
        Subscribe sub = new Subscribe(driver.getDriver());
        actionList.Add(sub);
        Favorites fav = new Favorites(driver.getDriver());
        actionList.Add(fav);
        Notes note = new Notes(driver.getDriver());
        actionList.Add(note);
        Photo photo = new Photo(driver.getDriver());
        actionList.Add(photo);
        Text text = new Text(driver.getDriver());
        actionList.Add(text);
        Call call = new Call(driver.getDriver());
        actionList.Add(call);
        AdFinder finder = new AdFinder(id, driver.getDriver());
        List<PFAction> randomActions = GetRandomActions(actionList, 3);
        Console.WriteLine("Выбранные действия:");
        foreach (PFAction action in randomActions)
        {
            Console.WriteLine(action);
            if (action == text)
            {
                action.Perform();
            }
            action.Perform();
        }
        driver.getDriver().Quit();
    }
    static List<PFAction> GetRandomActions<PFAction>(List<PFAction> list, int numberOfActions)
    {
        Random random = new Random();
        return list.OrderBy(x => random.Next()).Take(numberOfActions).ToList();
    }
}
