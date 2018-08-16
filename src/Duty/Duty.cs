using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Unturned;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using Rocket.API;
using Rocket.Core.Permissions;
using Rocket.Core;
using System.Collections.Generic;
using Rocket.API.Serialisation;
using Rocket.Core.Logging;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using System.Linq;

namespace EFG.Duty
{
    public class Duty : RocketPlugin<DutyConfiguration>
    {
        public static Duty Instance;
        public List<DutyGroups> ValidGroups;

        protected override void Load()
        {
            Instance = this;

            ValidGroups = Configuration.Instance.Groups;

            foreach (DutyGroups Group in ValidGroups.ToList())
            {
                RocketPermissionsGroup g = R.Permissions.GetGroup(Group.GroupID);
                if (g == null)
                {
                    Logger.LogWarning("Permission group " + Group.GroupID + " does not exist! No command related to that group will execute.");
                    ValidGroups.Remove(Group);
                }
            }


            Logger.LogWarning("Loading event \"Player Connected\"...");
            U.Events.OnPlayerConnected += PlayerConnected;
            Logger.LogWarning("Loading event \"Player Disconnected\"...");
            U.Events.OnPlayerDisconnected += PlayerDisconnected;

            Logger.LogWarning("");
            Logger.LogWarning("Duty has been successfully loaded!");
        }
        
        protected override void Unload()
        {
            Instance = null;

            Logger.LogWarning("Unloading on player connect event...");
            U.Events.OnPlayerConnected -= PlayerConnected;
            Logger.LogWarning("Unloading on player disconnect event...");
            U.Events.OnPlayerConnected -= PlayerDisconnected;

            Logger.LogWarning("");
            Logger.LogWarning("Duty has been unloaded!");
        }

        public void DoDuty(UnturnedPlayer caller)
        {
            if (caller.IsAdmin)
            {
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
            else
            {
                caller.Admin(true);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }

        public void DoDuty(UnturnedPlayer Player, DutyGroups Group)
        {
            RocketPermissionsGroup Target = R.Permissions.GetGroup(Group.GroupID);
            if (Target.Members.Contains(Player.CSteamID.ToString()))
            {
                R.Permissions.RemovePlayerFromGroup(Target.Id, Player);
                Player.Features.GodMode = false;
                Player.Features.VanishMode = false;
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", Player.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                return;
            }
            else
            {
                R.Permissions.AddPlayerToGroup(Group.GroupID, Player);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", Player.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }

        public void CDuty(UnturnedPlayer cplayer, IRocketPlayer caller)
        {
            if (!Configuration.Instance.AllowDutyCheck)
            {
                UnturnedChat.Say(caller, Translate("error_unable_checkduty"));
                return;
            }
            if (cplayer != null && cplayer.IsAdmin)
            {
                if (caller is ConsolePlayer)
                {
                    UnturnedChat.Say(Instance.Translate("check_on_duty_message", "Console", cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                }
                else if (caller is UnturnedPlayer)
                {
                    UnturnedChat.Say(Instance.Translate("check_on_duty_message", caller.DisplayName, cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                }
                return;
            }
            else if (cplayer != null)
            {
                foreach (DutyGroups Group in ValidGroups)
                {
                    RocketPermissionsGroup Target = R.Permissions.GetGroup(Group.GroupID);
                    if (Target.Members.Contains(cplayer.CSteamID.ToString()))
                    {
                        if (caller is ConsolePlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", "Console", cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        else if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", caller.DisplayName, cplayer.DisplayName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        return;
                    }
                }
            }
            else if (cplayer == null)
            {
                UnturnedChat.Say(caller, Translate("error_cplayer_null"));
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList {
                    {"admin_login_message", "{0} has logged on and is now on duty."},
                    {"admin_logoff_message", "{0} has logged off and is now off duty."},
                    {"on_duty_message", "{0} is now on duty."},
                    {"off_duty_message", "{0} is now off duty."},
                    {"check_on_duty_message", "{0} has confirmed that {1} is on duty."},
                    {"check_off_duty_message", "{0} has confirmed that {1} is not on duty."},
                    {"not_enough_permissions", "You do not have the correct permissions to use duty."},
                    {"error_unable_checkduty", "Unable To Check Duty. Configuration Is Set To Be Disabled."},
                    {"error_cplayer_null", "Player is not online or his name is invalid." },
                    {"error_dc_usage", "No argument was specified. Please use \"dc <playername>\" to check on a player." }
                };
                    
            }
        }

        void PlayerConnected(UnturnedPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                return;
            }

            foreach (DutyGroups Group in ValidGroups)
            {
                RocketPermissionsGroup Target = R.Permissions.GetGroup(Group.GroupID);
                if (Target.Members.Contains(player.CSteamID.ToString()))
                {
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
        }

        void PlayerDisconnected(UnturnedPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.Instance.RemoveDutyOnLogout)
                {
                    player.Admin(false);
                    player.Features.GodMode = false;
                    player.Features.VanishMode = false;
                }

                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                return;
            }

            foreach (DutyGroups Group in ValidGroups)
            {
                RocketPermissionsGroup Target = R.Permissions.GetGroup(Group.GroupID);
                if (Target.Members.Contains(player.CSteamID.ToString()))
                {
                    if (Configuration.Instance.RemoveDutyOnLogout)
                    {
                        player.Features.GodMode = false;
                        player.Features.VanishMode = false;
                        R.Permissions.RemovePlayerFromGroup(Target.Id, player);
                    }

                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logoff_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                    return;
                }
            }
        }
    }
}
