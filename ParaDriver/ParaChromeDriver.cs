using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using static ParaDriver.ParaConstants;
using System.Linq;

namespace ParaDriver
{
    public class ParaChromeDriver : IWebDriver, IDisposable, IJavaScriptExecutor, ISupportsLogs, IDevTools, ISearchContext, IFindsElement, ITakesScreenshot, ISupportsPrint, IActionExecutor, IAllowsFileDetection, IHasCapabilities, IHasCommandExecutor, IHasSessionId, ICustomDriverCommandExecutor
    {
        private string paraPath;
        private static Mode mode;
        private static string masterParaPath = string.Empty;
        private static ParaSetupDelegate? paraSetup;

        public Guid ParaId { get; private set; }
        public ChromeDriver CurrentDriver { get; private set; }
        public static bool IsParaMasterDataCreated { get; private set; } = false;

        public delegate void ParaSetupDelegate(ChromeDriver chromeDriver);

        public ParaChromeDriver(ChromeOptions chromeOptions)
        {
            if (mode == Mode.Debugging && paraSetup != null)
            {
                CurrentDriver = GetChomeDriver(chromeOptions, Guid.Empty, string.Empty);
                paraSetup(CurrentDriver);
            }

            if (mode == Mode.Parasitic || mode == Mode.ManualParasitic) 
            { 
                ParaInitialize(chromeOptions); 
            }
        }

        public static void Stop()
        {
            if (mode == Mode.Parasitic)
            {
                Helpers.DeleteFolder(masterParaPath);
            }
        }

        public static void AttachParasite(ParaSetupDelegate prestate)
        {
            paraSetup += prestate;
        }

        /// <summary>
        /// Function should be called once to create an state of chrome that can be used by other tests
        /// </summary>
        public static void Start(Mode paraMode = Mode.Parasitic, string? paraPath = null)
        {
            mode = paraMode;
            masterParaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\\Local\\Google\\Chrome\\ParaChrome\\ParaData");
            
            if (!Directory.Exists(masterParaPath))
            {
                Directory.CreateDirectory(masterParaPath);
            }

            switch (paraMode)
            {
                case Mode.Debugging:
                    mode = Mode.Debugging;
                    break;
                case Mode.ManualParasitic:
                    mode = Mode.ManualParasitic;
                    StartManualParasiticMode(paraPath);
                    break;
                case Mode.Parasitic:
                    mode = Mode.Parasitic;
                    StartParasiticMode();
                    break;
            }
        }

        private static void StartParasiticMode()
        {
            if (paraSetup != null)
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                
                if (!IsParaMasterDataCreated)
                {
                    if (mode != Mode.Debugging)
                    {
                        CleanParaJunks();
                        chromeOptions.AddArguments($"--user-data-dir={masterParaPath}");
                    }

                    var driver = GetChomeDriver(chromeOptions, Guid.Empty, string.Empty);

                    try
                    {
                        if (paraSetup != null)
                        {
                            paraSetup(driver);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (mode != Mode.Debugging)
                        {
                            driver.Quit();
                            IsParaMasterDataCreated = true;
                        }
                    }
                }
            }
        }

        private static void StartManualParasiticMode(string paraDataPath)
        {
            if(paraDataPath == null)
            {
                paraDataPath = masterParaPath;
            }
            else
            {
                paraDataPath = Path.Combine(paraDataPath, "ParaData");

                if (!Directory.Exists(paraDataPath))
                {
                    Directory.CreateDirectory(paraDataPath);
                }
            }

            var folderCount = new DirectoryInfo(paraDataPath).GetDirectories().Count();

            if (folderCount == 0)
            {
                StartParasiticMode();
                Helpers.Copy(masterParaPath, paraDataPath);
                CleanParaJunks();
                Helpers.DeleteFolder(masterParaPath);
            }

            masterParaPath = paraDataPath;
            CleanParaJunks();
            IsParaMasterDataCreated = true;
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
            get => CurrentDriver.FileDetector;
            set
            {
                CurrentDriver.FileDetector = value;
            }
        }

        public ICapabilities Capabilities => CurrentDriver.Capabilities;

        public ICommandExecutor CommandExecutor => CurrentDriver.CommandExecutor;

        public SessionId SessionId => CurrentDriver.SessionId;

        public void Close()
        {
            CurrentDriver.Close();
        }

        public void Dispose()
        {
            Helpers.ForceCloseParaChrome(ParaId);
            CurrentDriver.Dispose();
            Helpers.DeleteFolder(paraPath);
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
            return new ParaNavigator(CurrentDriver);
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
                if (mode != Mode.Debugging)
                {
                    Helpers.ForceCloseParaChrome(ParaId);
                    Thread.Sleep(1000);
                    Helpers.DeleteFolder(paraPath);
                }
            }
        }

        public ITargetLocator SwitchTo()
        {
            return CurrentDriver.SwitchTo();
        }

        public static void CleanParaJunks()
        {
            var junkDirs = new DirectoryInfo(masterParaPath)?.Parent?.GetDirectories("ParaData*");

            try
            {
                foreach (var junkDir in junkDirs)
                {
                    Guid paraId = new Guid();
                    try
                    {
                        paraId = new Guid(junkDir.ToString().Split('\\').Last().Replace("ParaData", string.Empty));
                    }
                    catch
                    {

                    }

                    if (paraId != default(Guid))
                    {
                        Helpers.ForceCloseParaChrome(paraId);
                        Helpers.DeleteFolder(junkDir.ToString());
                    }
                    else if(mode != Mode.ManualParasitic)
                    {
                        Helpers.DeleteFolder(junkDir.ToString());
                    }
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
            if (IsParaMasterDataCreated && (mode == Mode.Parasitic || mode == Mode.ManualParasitic))
            {
                ParaId = Guid.NewGuid();
                paraPath = $"{masterParaPath}{ParaId}";
                Helpers.Copy(masterParaPath, paraPath);
                chromeOptions.AddArguments($"--user-data-dir={paraPath}");
            }

            if (mode == Mode.Debugging)
            {
                StartParasiticMode();
            }

            if (mode != Mode.Debugging)
            {
                CurrentDriver = GetChomeDriver(chromeOptions, ParaId, paraPath);
            }
        }

        private static ChromeDriver GetChomeDriver(ChromeOptions chromeOptions, Guid paraId, string ParaPath)
        {
            ChromeDriver chromeDriver = null;

            int retry = 3;
            while(retry-- > 0)
            {
                try
                {
                    chromeDriver = new ChromeDriver(chromeOptions);
                    return chromeDriver;
                }
                catch
                {
                    if (!(paraId == Guid.Empty && ParaPath == string.Empty))
                    {
                        if(retry == 2)
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            chromeOptions.AddUserProfilePreference("profile.exit_type", "Normal");
                        }

                        Helpers.ForceCloseParaChrome(paraId);
                    }
                }
            }

            if (chromeDriver == null)
            {
                Helpers.DeleteFolder(ParaPath);
                throw new Exception("Unable to start chrome and load chrome profile within given time, It seems you are running parachrome in parallel with a low proccesing power machine! To avoid this failure request you to reduce the number of parallel runs or increase the processing power of your machine.");
            }

            return chromeDriver;
        }
    }
}
