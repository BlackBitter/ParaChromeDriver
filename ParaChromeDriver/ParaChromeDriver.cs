using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using System.Collections.ObjectModel;
using static ParaChromeDriver.ParaConstants;

namespace ParaChromeDriver
{
    public class ParaChromeDriver : IWebDriver, IDisposable, IJavaScriptExecutor, ISupportsLogs, IDevTools, ISearchContext, IFindsElement, ITakesScreenshot, ISupportsPrint, IActionExecutor, IAllowsFileDetection, IHasCapabilities, IHasCommandExecutor, IHasSessionId, ICustomDriverCommandExecutor
    {
        private const string ParaPathContainer = "{0}\\AppData\\Local\\Google\\Chrome\\ParaData";
        public Guid ParaId;
        private string ParaPath;
        private static Mode Mode;

        public ChromeDriver CurrentDriver { get; private set; }

        public ParaChromeDriver(ChromeOptions chromeOptions)
        {
            if (Mode == Mode.Degugging && ParaSetup != null)
            {
                CurrentDriver = new ChromeDriver(chromeOptions);
                ParaSetup(CurrentDriver);
            }

            if (Mode == Mode.Parasitic || Mode == Mode.ManualParasitic) 
            { 
                ParaInitialize(chromeOptions); 
            }
        }

        public static void Stop()
        {
            if (Mode == Mode.Parasitic)
            {
                Helpers.DeleteFolder(MasterParaPath);
            }
        }

        public delegate void ParaSetupDelegate(ChromeDriver chromeDriver);
        public static bool IsParaMasterDataCreated { get; private set; } = false;
        private static string MasterParaPath { get; set; } = string.Empty;
        private static ParaSetupDelegate? ParaSetup { get; set; }

        public static void AttachParasite(ParaSetupDelegate prestate)
        {
            ParaSetup += prestate;
        }

        /// <summary>
        /// Function should be called one to create an state of chrome that can be used by other tests
        /// </summary>
        public static void Start(Mode paraMode = Mode.Parasitic, string? paraPath = null)
        {
            Mode = paraMode;
            switch (paraMode)
            {
                case Mode.Degugging:
                    Mode = Mode.Degugging;
                    break;
                case Mode.ManualParasitic:
                    Mode = Mode.ManualParasitic;
                    StartManualParasiticMode(paraPath);
                    break;
                case Mode.Parasitic:
                    Mode = Mode.Parasitic;
                    StartParasiticMode();
                    break;
            }
        }

        private static void StartParasiticMode()
        {
            if (ParaSetup != null)
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                MasterParaPath = string.Format(ParaPathContainer, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                if (!IsParaMasterDataCreated)
                {
                    if (Mode != Mode.Degugging)
                    {
                        CleanParaJunks();
                        chromeOptions.AddArguments($"--user-data-dir={MasterParaPath}");
                    }

                    var driver = new ChromeDriver(chromeOptions);

                    try
                    {
                        if (ParaSetup != null)
                        {
                            ParaSetup(driver);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (Mode != Mode.Degugging)
                        {
                            driver.Quit();
                            IsParaMasterDataCreated = true;
                        }
                    }
                }
            }
        }

        private static void StartManualParasiticMode(string? paraDataPath)
        {
            if (paraDataPath != null)
            {
                MasterParaPath = paraDataPath;
                IsParaMasterDataCreated = true;
            }
            else
            {
                throw new Exception("Manual Parasitic mode requires data directory path of pre set profile data!");
            }
        }

        public string Url
        {
            get
            {
                return CurrentDriver.Url;
            }
            set
            {
                CurrentDriver.Url = value;
            }
        }

        public string Title => CurrentDriver.Title;

        public string PageSource => CurrentDriver.PageSource;

        public string CurrentWindowHandle => CurrentDriver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => CurrentDriver.WindowHandles;

        public bool HasActiveDevToolsSession => CurrentDriver.HasActiveDevToolsSession;

        public bool IsActionExecutor => CurrentDriver.IsActionExecutor;

        public IFileDetector FileDetector
        {
            get => FileDetector;
            set
            {
                FileDetector = value;
            }
        }

        public ICapabilities Capabilities => Capabilities;

        public ICommandExecutor CommandExecutor => CurrentDriver.CommandExecutor;

        public SessionId SessionId => CurrentDriver.SessionId;

        public void Close()
        {
            CurrentDriver.Close();
        }

        public void Dispose()
        {
            CurrentDriver.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            return CurrentDriver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return CurrentDriver.FindElements(by);
        }

        public IOptions Manage()
        {
            return CurrentDriver.Manage();
        }

        public INavigation Navigate()
        {
            return CurrentDriver.Navigate();
        }

        public void Quit()
        {
            try
            {
                CurrentDriver.Quit();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Mode != Mode.Degugging)
                {
                    Thread.Sleep(1000);
                    Helpers.DeleteFolder(ParaPath);
                }
            }

        }

        public ITargetLocator SwitchTo()
        {
            return CurrentDriver.SwitchTo();
        }

        public static void CleanParaJunks()
        {
            var junkDirs = new DirectoryInfo(MasterParaPath)?.Parent?.GetDirectories("ParaData*");

            try
            {
                foreach (var junkDir in junkDirs)
                {
                    Helpers.DeleteFolder(junkDir.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Please close all the instance of Chrome created by ParaChromeDriver form pervious run and Rerun the tests : - {ex.Message}");
            }
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return CurrentDriver.ExecuteScript(script, args);
        }

        public object ExecuteScript(PinnedScript script, params object[] args)
        {
            return CurrentDriver.ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return CurrentDriver.ExecuteAsyncScript(script, args);
        }

        public DevToolsSession GetDevToolsSession()
        {
            return CurrentDriver.GetDevToolsSession();
        }

        public DevToolsSession GetDevToolsSession(int protocolVersion)
        {
            return CurrentDriver.GetDevToolsSession(protocolVersion);
        }

        public void CloseDevToolsSession()
        {
            CurrentDriver.CloseDevToolsSession();
        }

        public IWebElement FindElement(string mechanism, string value)
        {
            return CurrentDriver.FindElement(mechanism, value);
        }

        public ReadOnlyCollection<IWebElement> FindElements(string mechanism, string value)
        {
            return CurrentDriver.FindElements(mechanism, value);
        }

        public Screenshot GetScreenshot()
        {
            return CurrentDriver.GetScreenshot();
        }

        public PrintDocument Print(PrintOptions options)
        {
            return CurrentDriver.Print(options);
        }

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            CurrentDriver.PerformActions(actionSequenceList);
        }

        public void ResetInputState()
        {
            CurrentDriver.ResetInputState();
        }

        public object ExecuteCustomDriverCommand(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            return CurrentDriver.ExecuteCustomDriverCommand(driverCommandToExecute, parameters);
        }

        public void RegisterCustomDriverCommands(IReadOnlyDictionary<string, CommandInfo> commands)
        {
            CurrentDriver.RegisterCustomDriverCommands(commands);
        }

        public bool RegisterCustomDriverCommand(string commandName, CommandInfo commandInfo)
        {
            return CurrentDriver.RegisterCustomDriverCommand(commandName, commandInfo);
        }

        private void ParaInitialize(ChromeOptions chromeOptions)
        {
            if (IsParaMasterDataCreated && (Mode == Mode.Parasitic || Mode == Mode.ManualParasitic))
            {
                ParaId = Guid.NewGuid();
                ParaPath = $"{MasterParaPath}{ParaId}";
                Helpers.Copy(MasterParaPath, ParaPath);
                chromeOptions.AddArguments($"--user-data-dir={ParaPath}");
            }

            if (Mode == Mode.Degugging)
            {
                StartParasiticMode();
            }

            if (Mode != Mode.Degugging)
            {
                CurrentDriver = new ChromeDriver(chromeOptions);
            }
        }
    }
}