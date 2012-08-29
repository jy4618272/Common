using System.Globalization;

public class ActionCustomHandler : Micajah.Common.Bll.Handlers.ActionHandler
{
    #region Public Methods

    public override string GetName(Micajah.Common.Bll.Action action)
    {
        string name = null;
        if (action != null)
        {
            switch (action.Name.ToLower(CultureInfo.CurrentCulture))
            {
                case "my open tickets":
                    name = action.Name + " (8)";
                    break;
                case "my closed tickets":
                    name = action.Name + " (141)";
                    break;
                default:
                    name = base.GetName(action);
                    break;
            }
        }
        return name;
    }

    //public override bool AccessDenied(Micajah.Common.Bll.Action action)
    //{
    //    return (action.Name == "BWD Time");
    //}

    public override string GetDescription(Micajah.Common.Bll.Action action)
    {
        string description = null;
        if (action != null)
        {
            switch (action.Name.ToLower(CultureInfo.CurrentCulture))
            {
                case "my open tickets":
                    description = action.Description + ". It's the custom string that can be added to the description of the action.";
                    break;
                default:
                    description = base.GetDescription(action);
                    break;
            }

        }
        return description;
    }

    //public override string GetNavigateUrl(Micajah.Common.Bll.Action action)
    //{
    //    string url = null;
    //    if (action != null)
    //    {
    //        switch (action.Name.ToLower(CultureInfo.CurrentCulture))
    //        {
    //            case "my open tickets":
    //                url = "http://google.com";
    //                break;
    //            default:
    //                url = base.GetNavigateUrl(action);
    //                break;
    //        }

    //    }
    //    return url;
    //}

    #endregion
}
