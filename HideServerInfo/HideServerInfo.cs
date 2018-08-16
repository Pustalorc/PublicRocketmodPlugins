using Rocket.Core.Plugins;
using Steamworks;

namespace persiafighter.Plugins.HideServerInfo
{
    public class HideServerInfo : RocketPlugin
    {
        protected override void Load() => Rocket.Core.R.Plugins.OnPluginsLoaded += () =>
        {
            SteamGameServer.SetKeyValue("Browser_Workshop_Count", "0");
            SteamGameServer.SetKeyValue("rocketplugins", "");
            SteamGameServer.SetKeyValue("Browser_Config_Count", "0");
        };
    }
}
