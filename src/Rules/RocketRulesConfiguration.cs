using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket_Rules
{
    public class RocketRulesConfiguration : IRocketPluginConfiguration
    {
        public bool DisplayOnConnect;
        [XmlArrayItem(ElementName = "Rule")]
        public List<IngameRules> Rules;

        public void LoadDefaults()
        {
            DisplayOnConnect = false;
            Rules = new List<IngameRules>
            {
                new IngameRules { configText = "Example Rule 1", configColor = "FFFF00" },
                new IngameRules { configText = "Example Rule 2", configColor = "00FF00" },
                new IngameRules { configText = "Example Rule 3", configColor = "FF0000" },
                new IngameRules { configText = "Example Rule 4", configColor = "0000FF" }
            };
        }
    }
}
