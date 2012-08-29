using System;
using System.Web.UI;

public partial class TreeViewExtendedTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        EntityTreeView1.LoadTree();
    }
}
