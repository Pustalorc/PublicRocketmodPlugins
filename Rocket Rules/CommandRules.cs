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

namespace Rocket_Rules
{
    public class CommandRules : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "Rules"; }
        }

        public string Help
        {
            get { return "Lists the rules set by the user."; }
        }

        public string Syntax
        {
            get { return "<player>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string configRule1 = RocketRules.Instance.configRule1;
            string configRule2 = RocketRules.Instance.configRule2;
            string configRule3 = RocketRules.Instance.configRule3;
            string configRule4 = RocketRules.Instance.configRule4;
            Color configRuleColor1 = RocketRules.Instance.configRuleColor1;
            Color configRuleColor2 = RocketRules.Instance.configRuleColor2;
            Color configRuleColor3 = RocketRules.Instance.configRuleColor3;
            Color configRuleColor4 = RocketRules.Instance.configRuleColor4;
            UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule1", configRule1), configRuleColor1);
            UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule2", configRule2), configRuleColor2);
            UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule3", configRule3), configRuleColor3);
            UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule4", configRule4), configRuleColor4);
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
              {
                  "help"
              };
            }
        }
    }
}
