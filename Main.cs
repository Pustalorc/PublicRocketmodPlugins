using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace RobnRaid
{
    public class RobnRaid : RocketPlugin<Configuration>
    {
        public static RobnRaid Instance;
        public Color color;

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    { "rob_translation", "{0} is robbing {1} at {2}" },
                    { "raid_translation", "{0} is raiding a base at {2}" },
                };
            }
        }

        protected override void Load()
        {
            Instance = this;

            color = UnturnedChat.GetColorFromName(Configuration.Instance.Color, Color.green);
            Rocket.Core.Logging.Logger.LogWarning("RobnRaid by Bullet_Tide has been loaded!");
        }

        protected override void Unload()
        {
            Instance = null;

            Rocket.Core.Logging.Logger.LogWarning("RobnRaid has been unloaded!");
        }
    }
}
