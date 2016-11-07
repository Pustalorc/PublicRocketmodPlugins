using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket_Rules
{
    public class RocketRulesConfiguration : IRocketPluginConfiguration
    {
        public string Rule1;
        public string Rule2;
        public string Rule3;
        public string Rule4;
        public string RuleColor1;
        public string RuleColor2;
        public string RuleColor3;
        public string RuleColor4;
        public bool DisplayOnConnect;

        public void LoadDefaults()
        {
            Rule1 = "Example Rule 1";
            Rule2 = "Example Rule 2";
            Rule3 = "Example Rule 3";
            Rule4 = "Example Rule 4";
            RuleColor1 = "Red";
            RuleColor2 = "Blue";
            RuleColor3 = "Green";
            RuleColor4 = "Yellow";
            DisplayOnConnect = false;
        }
    }
}
