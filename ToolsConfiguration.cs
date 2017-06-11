using Rocket.API;

namespace Bloodstone.Plugins.RocketTools
{
    public sealed class Configuration : IRocketPluginConfiguration
    {
        public bool EnableRPermCommand;

        public void LoadDefaults()
        {
            EnableRPermCommand = true;
        }
    }
}
