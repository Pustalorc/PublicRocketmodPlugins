using Rocket.Core.Plugins;

namespace HideConfig
{
    public class Class1 : RocketPlugin
    {
        protected override void Load() => Rocket.Core.R.Plugins.OnPluginsLoaded += () =>
        {
            Steamworks.SteamGameServer.SetKeyValue("Browser_Config_Count", "0");
        };
    }
}
