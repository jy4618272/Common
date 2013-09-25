using System;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Security;

public partial class Pages1_Schedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // gets a rule engine
        RulesEngine rulesEngine = RuleEngineProvider.RulesEngines["DataCollectorAssignment"];
        // gets a ticket entity and fill his fields
        Entity ent = EntityNodeProvider.GetEntityNodeType(new Guid("00000000-0000-0000-0000-000000000003"));
        ent.Fields["Square"].Value = 40000;
        ent.Fields["Status"].Value = 3;
        //ent.Fields["Address"].Value = 3;
        //EntityNodeProvider.GetEntityNode(
        Entity orgStruct = EntityFieldProvider.Entities["4cda22f3-4f01-4768-8608-938dc6a06825"];
        if (orgStruct != null)
        {
            ent.Fields["Address"].Value = orgStruct;
        }
        // add entity as input parameter
        rulesEngine.AddInputEntity(ent);
        // gets a output RuleId
        Label1.Text = rulesEngine.Execute().ToString();


    }
}
