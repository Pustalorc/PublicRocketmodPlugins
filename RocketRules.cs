using Rocket;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket_Rules
{
    class RocketRules : RocketPlugin<RocketRulesConfiguration>
    {
        public static RocketRules Instance;
        public string configRule1;
        public string configRule2;
        public string configRule3;
        public string configRule4;
        public Color configRuleColor1;
        public Color configRuleColor2;
        public Color configRuleColor3;
        public Color configRuleColor4;
        public bool configDisplayOnConnect;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"rule1", "{0}"},
                    {"rule2", "{0}"},
                    {"rule3", "{0}"},
                    {"rule4", "{0}"},
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            configRule1 = RocketRules.Instance.Configuration.Instance.Rule1;
            configRule2 = RocketRules.Instance.Configuration.Instance.Rule2;
            configRule3 = RocketRules.Instance.Configuration.Instance.Rule3;
            configRule4 = RocketRules.Instance.Configuration.Instance.Rule4;
            configRuleColor1 = UnturnedChat.GetColorFromName(RocketRules.Instance.Configuration.Instance.RuleColor1, Color.red);
            configRuleColor2 = UnturnedChat.GetColorFromName(RocketRules.Instance.Configuration.Instance.RuleColor2, Color.blue);
            configRuleColor3 = UnturnedChat.GetColorFromName(RocketRules.Instance.Configuration.Instance.RuleColor3, Color.green);
            configRuleColor4 = UnturnedChat.GetColorFromName(RocketRules.Instance.Configuration.Instance.RuleColor4, Color.yellow);
            configDisplayOnConnect = RocketRules.Instance.Configuration.Instance.DisplayOnConnect;

            U.Events.OnPlayerConnected += Events_OnPlayerConnected;

            Rocket.Core.Logging.Logger.Log("Rules has been loaded!", ConsoleColor.DarkGreen);
            Rocket.Core.Logging.Logger.Log("---- Rocket Rules Config START ----", ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Rule #1: " + configRule1, ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Rule #2: " + configRule2, ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Rule #3: " + configRule3, ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Rule #4: " + configRule4, ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Color of Rule #1: " + Convert.ToString(configRuleColor1), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Color of Rule #2: " + Convert.ToString(configRuleColor2), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Color of Rule #3: " + Convert.ToString(configRuleColor3), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Color of Rule #4: " + Convert.ToString(configRuleColor4), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Display Rules On Player Connect: " + Convert.ToString(configDisplayOnConnect), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("---- Rocket Rules Config END ----", ConsoleColor.Magenta);
        }

        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("Rules has been unloaded!", ConsoleColor.DarkGreen);
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (configDisplayOnConnect == true)
            {
                UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule1", configRule1), configRuleColor1);
                UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule2", configRule2), configRuleColor2);
                UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule3", configRule3), configRuleColor3);
                UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule4", configRule4), configRuleColor4);
            }
        }
    }
}
