using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobnRaid.Commands
{
    public class RaidCommand : IRocketCommand
    {
        public static RobnRaid Instance;
        public Color color;

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Name
        {
            get { return "raid"; }
        }

        public string Help
        {
            get { return "This is a raiding command."; }
        }

        public string Syntax
        {
            get { return "<location>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (!(caller is Player))
            {
                UnturnedChat.Say("raid_translation", color); return;
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "raid"
                };
            }
        }
    }
}