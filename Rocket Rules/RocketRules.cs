using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.API.Collections;

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

            Rocket.Core.Logging.Logger.Log("Rules has been loaded!");
        }

        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("Rules has been unloaded!");
        }
    }
}
