using Rocket.API;

namespace RocketTools
{
    public class NobStartConfiguration : IRocketPluginConfiguration
    {
        public bool EnablePlugin;
        public bool EnableRPermCommand;

        public void LoadDefaults()
        {
            EnablePlugin = true;
            EnableRPermCommand = true;
        }
    }
}
