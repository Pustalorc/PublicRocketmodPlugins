using Rocket.Unturned.Player;
using Rocket.API;
using System.Collections.Generic;

namespace EFG.Duty
{
    public class CommandDuty : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller == null) return;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (caller.HasPermission("duty.superadmin")) Duty.Instance.DoDuty(player);
            else if (caller.HasPermission("duty.admin")) Duty.Instance.Admin(caller);
            else if (caller.HasPermission("duty.moderator")) Duty.Instance.Moderator(caller);
            else if (caller.HasPermission("duty.helper")) Duty.Instance.Helper(caller);
            else if (caller.HasPermission("duty")) Rocket.Unturned.Chat.UnturnedChat.Say(caller, Duty.Instance.Translate("not_enough_permissions"));
        }

        public string Help
        {
            get { return "Gives admin powers to the player without the need of the console."; }
        }

        public string Name
        {
            get { return "duty"; }
        }

        public string Syntax
        {
            get { return ""; }
        }

        public bool AllowFromConsole
        {
            get { return false; }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "d" }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "duty.superadmin", "duty.admin", "duty.moderator", "duty.helper" };
            }
        }
    }
}
