using System;
using System.Web.UI;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;

namespace Micajah.Common.Pages
{
    /// <summary>
    /// Provides the functionality to persist the view and control state in the SQL database.
    /// </summary>
    public sealed class PageStatePersister : System.Web.UI.PageStatePersister
    {
        #region Members

        private const string ViewStateKeyName = "__VIEWSTATE_ID";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Micajah.Common.Pages.PageStatePersister class.
        /// </summary>
        /// <param name="page">The System.Web.UI.Page that the view state persistence mechanism is created for.</param>
        public PageStatePersister(Page page) : base(page) { }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value indicating this persister is in use.
        /// </summary>
        internal static bool IsInUse { get; set; }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Loads the view and control states from SQL database.
        /// </summary>
        public override void Load()
        {
            if (!this.Page.EnableViewState) return;

            IsInUse = true;

            Guid viewStateId = Guid.Empty;
            if (Page.Request[ViewStateKeyName] != null)
            {
                object obj = Support.ConvertStringToType(Page.Request[ViewStateKeyName], typeof(Guid));
                if (obj != null) viewStateId = (Guid)obj;
            }

            if (viewStateId != Guid.Empty)
            {
                Pair statePair = ViewStateProvider.GetViewState(viewStateId, this.StateFormatter);
                if (statePair != null)
                {
                    ViewState = statePair.First;
                    ControlState = statePair.Second;
                }
            }
        }

        /// <summary>
        /// Persists the view and control states in SQL database.
        /// </summary>
        public override void Save()
        {
            if (!this.Page.EnableViewState) return;
            if ((ViewState == null) && (ControlState == null)) return;

            IsInUse = true;

            Guid viewStateId = Guid.NewGuid();

            ViewStateProvider.InsertViewState(viewStateId, this.StateFormatter, new Pair(ViewState, ControlState));

            Page.ClientScript.RegisterHiddenField(ViewStateKeyName, viewStateId.ToString("N"));
        }

        #endregion
    }
}
