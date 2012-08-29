using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.Application
{
    /// <summary>
    /// The base class for applications.
    /// Defines the methods, properties, and events that are common to all application objects within an ASP.NET application.
    /// </summary>
    public class WebApplication : HttpApplication
    {
        #region Members

        private static CommonDataSetTableAdapters s_CommonDataSetTableAdapters;
        private static OrganizationDataSetTableAdapters s_OrganizationDataSetTableAdapters;
        private static SortedList s_OrganizationDataSetTableAdaptersList;
        private static LoginProvider s_LoginProvider;
        private static Guid? s_WebSiteId;

        // The objects which are used to synchronize access to the cached objects.
        private static object s_CommonDataSetSyncRoot = new object();
        private static object s_WebSiteIdSyncRoot = new object();
        private static object s_EntitiesSyncRoot = new object();
        private static object s_RulesEnginesSyncRoot = new object();
        private static object s_StartThreadsSyncRoot = new object();
        private static object s_OrganizationDataSetSyncRoot = new object();
        private static object s_OrganizationDataSetTableAdaptersListSyncRoot = new object();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an email is sending.
        /// </summary>
        public static event EventHandler<EmailSendingEventArgs> EmailSending;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the instance of the Micajah.Common.Dal.CommonDataSet class that contains common data of application.
        /// </summary>
        internal static CommonDataSet CommonDataSet
        {
            get
            {
                CommonDataSet ds = null;
                try
                {
                    ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                    if (ds == null)
                    {
                        lock (s_CommonDataSetSyncRoot)
                        {
                            ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                            if (ds == null)
                            {
                                ds = new CommonDataSet();
                                CommonDataSetTableAdapters.Fill(ds);

                                CacheManager.Current.Add("mc.CommonDataSet", ds);
                            }
                        }
                    }
                    return ds;
                }
                finally
                {
                    if (ds != null) ds.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the identifier of the web site, which the current web application is beign run.
        /// Zero, If the web site isn't found or is deleted.
        /// </summary>
        internal static Guid WebsiteId
        {
            get
            {
                if (!s_WebSiteId.HasValue)
                {
                    lock (s_WebSiteIdSyncRoot)
                    {
                        if (!s_WebSiteId.HasValue)
                        {
                            CommonDataSet.WebsiteRow row = WebsiteProvider.GetWebsiteRowByUrl(Hosts.ToArray());
                            s_WebSiteId = ((row != null) ? row.WebsiteId : Guid.Empty);
                        }
                    }
                }
                return s_WebSiteId.Value;
            }
        }

        /// <summary>
        /// Gets the hosts adresses of the current web application.
        /// </summary>
        internal static List<string> Hosts
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                List<string> list = new List<string>();
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress, ':', request.Url.Port));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, ':', request.Url.Port));
                return list;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the application's virtual application root path on the server.
        /// </summary>
        public static string RootPath
        {
            get
            {
                return ((HttpContext.Current.Request.ApplicationPath == "/")
                    ? string.Empty : HttpContext.Current.Request.ApplicationPath);
            }
        }

        /// <summary>
        /// Gets or sets the class that contains the tables adapters of the common data set.
        /// </summary>
        public static CommonDataSetTableAdapters CommonDataSetTableAdapters
        {
            get
            {
                if (s_CommonDataSetTableAdapters == null)
                    s_CommonDataSetTableAdapters = new CommonDataSetTableAdapters();
                return s_CommonDataSetTableAdapters;
            }
            set
            {
                s_CommonDataSetTableAdapters = value;
                RefreshCommonData();
            }
        }

        /// <summary>
        /// Gets or sets the class that contains the tables adapters of the organization data set.
        /// </summary>
        public static OrganizationDataSetTableAdapters OrganizationDataSetTableAdapters
        {
            get
            {
                if (s_OrganizationDataSetTableAdapters == null)
                    s_OrganizationDataSetTableAdapters = new OrganizationDataSetTableAdapters();
                return s_OrganizationDataSetTableAdapters;
            }
            set
            {
                s_OrganizationDataSetTableAdapters = value;
                s_OrganizationDataSetTableAdaptersList = null;
                RefreshOrganizationDataSets();
            }
        }

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public static LoginProvider LoginProvider
        {
            get
            {
                if (s_LoginProvider == null) s_LoginProvider = new LoginProvider();
                return s_LoginProvider;
            }
            set { s_LoginProvider = value; }
        }

        public static EntityCollection Entities
        {
            get
            {
                EntityCollection coll = CacheManager.Current.Get("mc.Entities") as EntityCollection;
                if (coll == null)
                {
                    lock (s_CommonDataSetSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.Entities") as EntityCollection;
                        if (coll == null)
                        {
                            coll = EntityCollection.Load();
                            CacheManager.Current.Add("mc.Entities", coll);
                        }
                    }
                }
                return coll;
            }
        }

        public static RulesEngineCollection RulesEngines
        {
            get
            {
                RulesEngineCollection coll = CacheManager.Current.Get("mc.RulesEngines") as RulesEngineCollection;
                if (coll == null)
                {
                    lock (s_CommonDataSetSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.RulesEngines") as RulesEngineCollection;
                        if (coll == null)
                        {
                            coll = RulesEngineCollection.Load();
                            CacheManager.Current.Add("mc.RulesEngines", coll);
                        }
                    }
                }
                return coll;
            }
        }

        public static StartThreadCollection StartThreads
        {
            get
            {
                StartThreadCollection coll = CacheManager.Current.Get("mc.StartThreads") as StartThreadCollection;
                if (coll == null)
                {
                    lock (s_CommonDataSetSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.StartThreads") as StartThreadCollection;
                        if (coll == null)
                        {
                            coll = StartThreadCollection.Load();
                            CacheManager.Current.Add("mc.StartThreads", coll);
                        }
                    }
                }
                return coll;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Raises the EmailSending event.
        /// </summary>
        /// <param name="e">An EmailSendingEventArgs that contains the event data.</param>
        internal static void RaiseEmailSending(EmailSendingEventArgs e)
        {
            if (EmailSending != null)
                EmailSending(null, e);
        }

        /// <summary>
        /// Refreshes all cached data.
        /// </summary>
        internal static void RefreshAllData()
        {
            RefreshCommonData();
            RefreshOrganizationDataSets();
            UserContext.RefreshCurrent();
        }

        /// <summary>
        /// Refreshes the cached data set of specified organization and the cached data of the current user.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to refresh.</param>
        internal static void RefreshOrganizationData(Guid organizationId)
        {
            RefreshOrganizationDataSetByOrganizationId(organizationId);
            UserContext.RefreshCurrent();
        }

        /// <summary>
        /// Refreshes the cached common data set and related data stored in collections.
        /// </summary>
        internal static void RefreshCommonData()
        {
            WebApplicationElement.CurrentDatabaseVersion = 0;
            RefreshCommonDataSet(false);
            RefreshWebsiteId();
            RefreshCollections();
        }

        /// <summary>
        /// Refreshes the cached common data set.
        /// </summary>
        internal static void RefreshCommonDataSet(bool organizationOnly)
        {
            if (organizationOnly)
            {
                CommonDataSet ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                if (ds != null)
                {
                    lock (s_CommonDataSetSyncRoot)
                    {
                        CommonDataSetTableAdapters.OrganizationTableAdapter.Fill(ds.Organization);

                        CacheManager.Current.Add("mc.CommonDataSet", ds);
                    }
                }
            }
            else
            {
                lock (s_CommonDataSetSyncRoot)
                {
                    CacheManager.Current.Remove("mc.CommonDataSet");
                }
            }
        }

        /// <summary>
        /// Refreshes the cached collections of the actions, global navigation links and settings.
        /// </summary>
        internal static void RefreshCollections()
        {
            ActionProvider.Refresh();
            SettingProvider.Refresh();
            RefreshEntities();
            RefreshRulesEngines();
            RefreshStartThreads();
        }

        /// <summary>
        /// Refreshes the measure units tables.
        /// </summary>
        internal static void RefreshMeasureUnits()
        {
            RefreshCommonDataSet(false);
        }

        internal static void RefreshEntities()
        {
            lock (s_EntitiesSyncRoot)
            {
                CacheManager.Current.Remove("mc.Entities");
            }
        }

        internal static void RefreshRulesEngines()
        {
            lock (s_RulesEnginesSyncRoot)
            {
                CacheManager.Current.Remove("mc.RulesEngines");
            }
        }

        internal static void RefreshStartThreads()
        {
            lock (s_StartThreadsSyncRoot)
            {
                CacheManager.Current.Remove("mc.StartThreads");
            }
        }

        /// <summary>
        /// Refreshes the cached organization data sets.
        /// </summary>
        internal static void RefreshOrganizationDataSets()
        {
            lock (s_OrganizationDataSetSyncRoot)
            {
                ArrayList list = new ArrayList();
                IDictionaryEnumerator cacheEnum = CacheManager.Current.GetEnumerator() as IDictionaryEnumerator;
                if (cacheEnum != null)
                {
                    while (cacheEnum.MoveNext())
                    {
                        string key = cacheEnum.Key.ToString();
                        if (key.StartsWith("mc.OrganizationDataSet.", StringComparison.Ordinal))
                            list.Add(key);
                    }
                }

                foreach (string key in list)
                {
                    CacheManager.Current.Remove(key);
                }
            }
        }

        internal static void RefreshCommonDataSetTableAdapters()
        {
            CommonDataSetTableAdapters = null;
        }

        internal static void RefreshOrganizationDataSetTableAdapters()
        {
            OrganizationDataSetTableAdapters = null;
        }

        /// <summary>
        /// Deletes the cached data set of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier.</param>
        internal static void RemoveOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            lock (s_OrganizationDataSetSyncRoot)
            {
                CacheManager.Current.Remove("mc.OrganizationDataSet." + organizationId.ToString("N"));
            }
        }

        /// <summary>
        /// Refreshes the cached data set of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to refresh.</param>
        internal static void RefreshOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            RemoveOrganizationDataSetByOrganizationId(organizationId);
        }

        /// <summary>
        /// Refreshes the identifier of the web site, which the current web application is beign run.
        /// </summary>
        internal static void RefreshWebsiteId()
        {
            s_WebSiteId = null;
        }

        /// <summary>
        /// Returns the instance of the OrganizationDataSet class that contains data of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to get data set.</param>
        /// <returns>The instance of the OrganizationDataSet class that contains data of specified organization.</returns>
        internal static OrganizationDataSet GetOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            OrganizationDataSet ds = null;
            try
            {
                string key = "mc.OrganizationDataSet." + organizationId.ToString("N");
                ds = CacheManager.Current.Get(key) as OrganizationDataSet;
                if (ds == null)
                {
                    lock (s_OrganizationDataSetSyncRoot)
                    {
                        ds = CacheManager.Current.Get(key) as OrganizationDataSet;
                        if (ds == null)
                        {
                            ds = new OrganizationDataSet();
                            OrganizationDataSetTableAdapters adapters = GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
                            adapters.Fill(ds, organizationId);

                            CacheManager.Current.Add(key, ds);
                        }
                    }
                }
                return ds;
            }
            finally
            {
                if (ds != null) ds.Dispose();
            }
        }

        /// <summary>
        /// Returns the instance of the OrganizationDataSetTableAdapters class that contains 
        /// tables adapters and connection to database of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier.</param>
        /// <returns>
        /// The instance of the OrganizationDataSetTableAdapters class that contains 
        /// tables adapters and connection to database of specified organization.
        /// </returns>
        internal static OrganizationDataSetTableAdapters GetOrganizationDataSetTableAdaptersByOrganizationId(Guid organizationId)
        {
            return GetOrganizationDataSetTableAdaptersByConnectionString(OrganizationProvider.GetConnectionString(organizationId));
        }

        /// <summary>
        /// Returns the instance of the OrganizationDataSetTableAdapters class that contains 
        /// tables adapters and specified connection to database.
        /// </summary>
        /// <param name="connectionString">The connection string to organization's database.</param>
        /// <returns>
        /// The instance of the OrganizationDataSetTableAdapters class that contains 
        /// tables adapters and specified connection to database.
        /// </returns>
        internal static OrganizationDataSetTableAdapters GetOrganizationDataSetTableAdaptersByConnectionString(string connectionString)
        {
            OrganizationDataSetTableAdapters adapters = null;

            if (s_OrganizationDataSetTableAdaptersList == null) s_OrganizationDataSetTableAdaptersList = new SortedList();

            if (!s_OrganizationDataSetTableAdaptersList.ContainsKey(connectionString))
            {
                lock (s_OrganizationDataSetTableAdaptersListSyncRoot)
                {
                    if (!s_OrganizationDataSetTableAdaptersList.ContainsKey(connectionString))
                    {
                        adapters = OrganizationDataSetTableAdapters.Clone();
                        adapters.ConnectionString = connectionString;
                        s_OrganizationDataSetTableAdaptersList.Add(connectionString, adapters);
                    }
                }
            }
            else adapters = s_OrganizationDataSetTableAdaptersList[connectionString] as OrganizationDataSetTableAdapters;

            return adapters;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a new ResourceVirtualPathProvider instance with the ASP.NET compilation system.
        /// </summary>
        private static void RegisterResourceVirtualPathProvider()
        {
            MethodInfo mi = typeof(HostingEnvironment).GetMethod("RegisterVirtualPathProviderInternal", (BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod));
            if (mi != null)
                mi.Invoke(null, new object[] { new ResourceVirtualPathProvider() });
            else
                HostingEnvironment.RegisterVirtualPathProvider(new ResourceVirtualPathProvider());
        }

        /// <summary>
        /// Starts all start thead and rerun it.
        /// </summary>
        private void MonitorStartThreadLoop()
        {
            StartThreadCollection startThreads = WebApplication.StartThreads;
            if (startThreads.Count > 0)
            {
                foreach (StartThread startThread in startThreads)
                {
                    DateTime now = DateTime.Now;
                    int startHour = 0;
                    int startMinute = 0;
                    DateTime startDate = DateTime.Now;
                    string[] startTime = startThread.StartTime.Split(':');

                    if (startTime.Length == 2)
                    {
                        startHour = startTime[0].StartsWith("0", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(startTime[0].Substring(1, startTime[0].Length - 1), CultureInfo.InvariantCulture) : Convert.ToInt32(startTime[0], CultureInfo.InvariantCulture);
                        startMinute = Convert.ToInt32(startTime[1], CultureInfo.InvariantCulture);

                        if ((now.Hour > startHour) || ((now.Hour == startHour) && (now.Minute >= startMinute)))
                            startDate = DateTime.Today.AddDays(1).AddHours(startHour);
                        else
                            startDate = DateTime.Today.AddHours(startHour);

                        startDate = startDate.AddMinutes(startMinute);

                        StartThreadTimer timer = new StartThreadTimer();
                        timer.StartThread = startThread;
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(StartThreadTimer_Elapsed);
                        timer.Interval = startDate.Subtract(now).TotalMilliseconds;
                        timer.AutoReset = true;
                        timer.Enabled = true;
                        timer.Start();
                    }
                }
            }
        }

        private void StartThreadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartThreadTimer timer = sender as StartThreadTimer;
            if (timer != null)
            {
                IThreadStateProvider threadProvider = timer.StartThread.GetStartThreadClassInstance();
                Thread workerThread = new Thread(threadProvider.Start);
                workerThread.CurrentCulture = CultureInfo.CurrentCulture;
                workerThread.CurrentUICulture = CultureInfo.CurrentUICulture;
                workerThread.Priority = ThreadPriority.Lowest;
                workerThread.Start();

                timer.Interval = 24 * 60 * 60 * 1000; //24 hours
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises when a security module has established the identity of the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string pageUrl = Request.AppRelativeCurrentExecutionFilePath;

            if (ResourceProvider.IsResourceUrl(pageUrl))
                Context.SkipAuthorization = true;

            if (Context.SkipAuthorization)
                return;

            if (ActionProvider.IsPublicPage(pageUrl))
            {
                if ((!FrameworkConfiguration.Current.WebApplication.Password.EnablePasswordRetrieval) &&
                    (string.Compare(pageUrl, ResourceProvider.PasswordRecoveryPageVirtualPath, StringComparison.OrdinalIgnoreCase) == 0))
                    throw new HttpException(404, Resources.Error_404);
                else
                {
                    Micajah.Common.Bll.Action action = ActionProvider.FindAction(CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                    if (action != null)
                        Context.SkipAuthorization = (!action.AuthenticationRequired);
                    else
                        Context.SkipAuthorization = true;

                    switch (FrameworkConfiguration.Current.WebApplication.AuthenticationMode)
                    {
                        case AuthenticationMode.Forms:
                            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                            if (authCookie == null)
                            {
                                FormsIdentity id = new FormsIdentity(new FormsAuthenticationTicket(string.Empty, false, FrameworkConfiguration.Current.WebApplication.Login.Timeout));
                                GenericPrincipal principal = new GenericPrincipal(id, null);
                                Context.User = principal;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Raises when an application is started.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            RegisterResourceVirtualPathProvider();
            Resources.ResourceManager.IgnoreCase = true;

            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
                ViewStateProvider.DeleteExpiredViewState();

            Thread MonitorThread = new Thread(MonitorStartThreadLoop);
            MonitorThread.CurrentCulture = CultureInfo.CurrentCulture;
            MonitorThread.CurrentUICulture = CultureInfo.CurrentUICulture;
            MonitorThread.Priority = ThreadPriority.Lowest;
            MonitorThread.Start();
        }

        /// <summary>
        /// Raises when an application is ended.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_End(object sender, EventArgs e)
        {
            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
                ViewStateProvider.DeleteExpiredViewState();
        }

        /// <summary>
        /// Raises when an error occured in an application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs as the first event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the request state (for example, session state) that is associated with the current request has been obtained.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx.Session != null)
            {
                UserContext user = null;

                if (ctx.Session.IsNewSession)
                {
                    user = UserContext.Current;
                    if (user != null)
                        LoginProvider.UpdateSession(user.UserId, ctx.Session.SessionID);
                }

                if (!ctx.SkipAuthorization)
                {
                    if (user == null)
                        user = UserContext.Current;

                    if (user == null)
                    {
                        Micajah.Common.Bll.Action action = ActionProvider.FindAction(CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                        if (action != null)
                        {
                            if (action.AuthenticationRequired)
                                LoginProvider.SignOut(true, Request.Url.PathAndQuery);
                        }
                    }
                    else if (!LoginProvider.ValidateSession(user.UserId, ctx.Session.SessionID))
                    {
                        if (!FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                            LoginProvider.SignOut(true, true, true);
                    }
                }

                if (ctx.Session.IsNewSession)
                    UserContext.InitializeOrganizationOrInstanceFromCustomUrl();
            }
        }

        /// <summary>
        /// Raises when a session is started.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Raises when a session is abandoned or expires.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Session_End(object sender, EventArgs e)
        {
            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
                ViewStateProvider.DeleteExpiredViewState();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the specified URL to the application relative URL if it is possible.
        /// </summary>
        /// <param name="url">The URL to convert.</param>
        /// <returns>The string that represents the application relative URL or original URL, if the conversion is not possible.</returns>
        public static string CreateApplicationRelativeUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = Regex.Replace(url, "default.aspx", string.Empty, RegexOptions.IgnoreCase);
                if (url.StartsWith("~", StringComparison.OrdinalIgnoreCase)) url = url.Remove(0, 1);
                if (!string.IsNullOrEmpty(RootPath)) url = Regex.Replace(url, RootPath, string.Empty, RegexOptions.IgnoreCase);
            }
            return url;
        }

        /// <summary>
        /// Converts the specified URL to the application absolute URL if it is possible.
        /// </summary>
        /// <param name="url">The URL to convert.</param>
        /// <returns>The string that represents the application absolute URL or original URL, if the conversion is not possible.</returns>
        public static string CreateApplicationAbsoluteUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!(url.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                    || url.StartsWith(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                    || url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase)))
                {
                    if (url.StartsWith("~", StringComparison.OrdinalIgnoreCase)) url = url.Remove(0, 1);
                    if (!url.StartsWith("/", StringComparison.OrdinalIgnoreCase)) url = "/" + url;
                    if (!string.IsNullOrEmpty(RootPath))
                    {
                        if (!url.ToUpperInvariant().Contains(RootPath.ToUpperInvariant() + "/"))
                            url = RootPath + url;
                    }
                    url = Regex.Replace(url, "default.aspx", string.Empty, RegexOptions.IgnoreCase);
                }
            }
            return url;
        }

        /// <summary>
        /// Converts the specified URL to the URI if it is possible.
        /// </summary>
        /// <param name="url">The URL to convert.</param>
        /// <returns>The string that represents the URI or original URL, if the conversion is not possible.</returns>
        public static string CreateApplicationUri(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!(url.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) || url.StartsWith(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
                {
                    if (url.StartsWith("~", StringComparison.OrdinalIgnoreCase)) url = url.Remove(0, 1);
                    if (!url.StartsWith("/", StringComparison.OrdinalIgnoreCase)) url = "/" + url;
                    if (FrameworkConfiguration.Current.WebApplication.Url.EndsWith("/", StringComparison.OrdinalIgnoreCase)) url = url.Remove(0, 1);
                    url = FrameworkConfiguration.Current.WebApplication.Url + url;
                    url = Regex.Replace(url, "default.aspx", string.Empty, RegexOptions.IgnoreCase);
                }
            }
            return url;
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the Micajah.Common.Application.WebApplication.EmailSending event.
    /// </summary>
    public class EmailSendingEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the e-mail message.
        /// </summary>
        public MailMessage MailMessage { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets whether the email should be sent and the calling thread will not blocked.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Gets or sets whether the sending should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets by what reason an email is sending.
        /// </summary>
        public EmailSendingReason Reason { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public Organization Organization { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents the different reasons for email sending.
    /// </summary>
    public enum EmailSendingReason
    {
        Undefined,
        SignupOrganization,
        ResetPassword,
        CreateNewLogin,
        ChangeLogin,
        ChangePassword,
        InviteUser
    }
}
