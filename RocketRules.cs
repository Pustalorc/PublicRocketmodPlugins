using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket_Rules
{
    class RocketRules : RocketPlugin<RocketRulesConfiguration>
    {
        public static RocketRules Instance;
        public bool configDisplayOnConnect;
        public string[] StoredRulesText;
        public string[] StoredRulesColor;
        public List<IngameRules> configRules;
        public int numRules;
        public int extraRules;
        public int Pages;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"rule", "{0}"},
                    {"pages", "Next page: \"/rules {0}\"."},
                    {"endofrules", "You have reached the end of the rules."}
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            configRules = Instance.Configuration.Instance.Rules;
            numRules = configRules.Count;
            Pages = (numRules / 3) + 1;
            extraRules = numRules % 3;
            StoredRulesColor = new string[numRules];
            StoredRulesText = new string[numRules];
            int b = 0;
            foreach (IngameRules color in configRules)
            {
                StoredRulesColor[b] = color.configColor;
                b = b + 1;
            }
            b = 0;
            foreach (IngameRules text in configRules)
            {
                StoredRulesText[b] = text.configText;
                b = b + 1;
            }

            configDisplayOnConnect = RocketRules.Instance.Configuration.Instance.DisplayOnConnect;

            U.Events.OnPlayerConnected += Events_OnPlayerConnected;

            Rocket.Core.Logging.Logger.Log("", ConsoleColor.Black);
            Rocket.Core.Logging.Logger.Log("---- Rocket Rules Config START ----", ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Display Rules On Player Connect: " + Convert.ToString(configDisplayOnConnect), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Ammount of rules: " + Convert.ToString(numRules), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Ammount of pages: " + Convert.ToString(Pages), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("Ammount of rules in last page: " + Convert.ToString(extraRules), ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("---- Rocket Rules Config END ----", ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.Log("", ConsoleColor.Black);
            Rocket.Core.Logging.Logger.Log("Rules has been loaded!", ConsoleColor.DarkGreen);
        }

        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("Rules has been unloaded!", ConsoleColor.DarkGreen);
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (configDisplayOnConnect == true)
            {
                if (numRules == 1)
                {
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), UnturnedChat.GetColorFromHex(StoredRulesColor[0]).Value);
                }
                else if (numRules == 2)
                {
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), UnturnedChat.GetColorFromHex(StoredRulesColor[0]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), UnturnedChat.GetColorFromHex(StoredRulesColor[1]).Value);
                }
                else if (numRules == 3)
                {
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), UnturnedChat.GetColorFromHex(StoredRulesColor[0]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), UnturnedChat.GetColorFromHex(StoredRulesColor[1]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[2]), UnturnedChat.GetColorFromHex(StoredRulesColor[2]).Value);
                }
                else if (numRules >= 4)
                {
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), UnturnedChat.GetColorFromHex(StoredRulesColor[0]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), UnturnedChat.GetColorFromHex(StoredRulesColor[1]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[2]), UnturnedChat.GetColorFromHex(StoredRulesColor[2]).Value);
                    UnturnedChat.Say(player, Instance.Translations.Instance.Translate("rule", StoredRulesText[3]), UnturnedChat.GetColorFromHex(StoredRulesColor[3]).Value);
                }
            }
        }
    }
    public class IngameRules
    {
        public IngameRules() { }
        public string configText;
        public string configColor;
    }
}
