using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Micajah.Common.Bll;

public class CustomLoginProvider : Micajah.Common.Bll.Providers.LoginProvider
{
    public override OrganizationCollection GetOrganizationsByLoginId(Guid loginId)
    {
        OrganizationCollection orgs = base.GetOrganizationsByLoginId(loginId);

        //string loginName = GetLoginName(loginId);

        //if (!(loginName.EndsWith("@micajah.com", StringComparison.OrdinalIgnoreCase) || loginName.EndsWith("@bigwebapps.com", StringComparison.OrdinalIgnoreCase)))
        //{
        //    foreach (Organization org in orgs)
        //    {
        //        if (string.Compare(org.Name, "Micajah", StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            org.Visible = false;
        //            break;
        //        }
        //    }
        //}

        return orgs;
    }
}