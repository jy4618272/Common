using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    [ToolboxData("<{0}:AdvancedGridView runat=server></{0}:AdvancedGridView>")]
    public class AdvancedGridView : RadGrid
    {
        #region Public Properties

        [Browsable(false)]
        public override string Skin { get { return "Default"; } }

        #endregion
    }
}
