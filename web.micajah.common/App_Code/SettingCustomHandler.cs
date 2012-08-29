/// <summary>
/// Summary description for SettingCustomHandler
/// </summary>
public class SettingCustomHandler : Micajah.Common.Bll.Handlers.SettingHandler
{
    public override string GetName(Micajah.Common.Bll.Setting setting)
    {
        if (setting.Name == "test1")
            return setting.Name + " custom name";
        return base.GetName(setting);
    }

    public override string GetDescription(Micajah.Common.Bll.Setting setting)
    {
        if (setting.Name == "test1")
            return setting.Description + ". It's the custom string that can be added to the description of the setting in the handler.";
        return base.GetDescription(setting);
    }
}