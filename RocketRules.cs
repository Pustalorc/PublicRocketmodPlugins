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
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket_Rules
{
    class RocketRules : RocketPlugin<RocketRulesConfiguration>
    {
        public static RocketRules Instance;
        public bool configDisplayOnConnect;
        public string[] StoredRulesText;
        public Color[] StoredRulesColor;
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
            this.configRules = RocketRules.Instance.Configuration.Instance.Rules;
            this.numRules = configRules.Count;
            this.Pages = (numRules / 3) + 1;
            this.extraRules = numRules % 3;
            StoredRulesColor = new Color[numRules];
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
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), StoredRulesColor[0]);
                }
                else if (numRules == 2)
                {
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), StoredRulesColor[0]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), StoredRulesColor[1]);
                }
                else if (numRules == 3)
                {
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), StoredRulesColor[0]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), StoredRulesColor[1]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[2]), StoredRulesColor[2]);
                }
                else if (numRules >= 4)
                {
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[0]), StoredRulesColor[0]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[1]), StoredRulesColor[1]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[2]), StoredRulesColor[2]);
                    UnturnedChat.Say(player, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[3]), StoredRulesColor[3]);
                }
            }
        }
    }
    public class IngameRules
    {
        public IngameRules() { }
        public string configText;
        public Color configColor;
    }
}
