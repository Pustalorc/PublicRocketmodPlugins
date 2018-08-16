using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdminOverride
{
    public class Class1 : RocketPlugin
    {
    }

    public class OverrideAdminCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "admin";
        public string Help => "Give a player admin privileges";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "rocket.admin" };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = command.GetUnturnedPlayerParameter(0);
            if (player == null)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            if (!player.IsAdmin)
            {
                UnturnedChat.Say(caller, "Successfully admined " + player.CharacterName);
                player.Admin(true);
            }
        }
    }
    public class OverrideUnadminCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "unadmin";
        public string Help => "Revoke a players admin privileges";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "rocket.unadmin" };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = command.GetUnturnedPlayerParameter(0);
            if (player == null)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            if (player.IsAdmin)
            {
                UnturnedChat.Say(caller, "Successfully unadmined " + player.CharacterName);
                player.Admin(false);
            }
        }
    }
}
