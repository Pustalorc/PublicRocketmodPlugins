using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Assets;
using Rocket.Core.Logging;
using Rocket.Core.Permissions;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;

namespace RocketTools
{
    public class CommandRPerm : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "rocketpermission"; }
        }

        public string Help
        {
            get { return "Adds or deletes a permission group or adds/removes a permission to/from a permission group."; }
        }

        public string Syntax
        {
            get { return "create <group> | delete <group> | add <permission> <group> | remove <permission> <group> | add <permission> <cooldown> <group> | color <color> <group> | prefix <prefix> <group> | suffix <suffix> <group> | displayname <name> <group> | id <id> <group> | parentgroup <parent group> <group> | list <permissions | members> <group> | details <group>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "rp", "rpermission", "rperm", "rocketp", "rocketperm" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (RocketTools.Instance.Configuration.Instance.EnableRPermCommand)
            {
                if (caller is ConsolePlayer)
                {
                    if (command.Length <= 1)
                    {
                        Logger.LogWarning(RocketTools.Instance.Translate("error_usage_rperm"));
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "create")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        RocketPermissionsGroup NewGroup = new RocketPermissionsGroup(command[1], command[1], "", new List<string>(), new List<Permission>(), "white");
                        NewGroup.Prefix = "";
                        NewGroup.Suffix = "";
                        RocketPermissionsGroup OldGroup = Permissions.GetGroup(command[1]);
                        if (OldGroup == null)
                        {
                            switch (Permissions.AddGroup(NewGroup))
                            {
                                case RocketPermissionsProviderResult.DuplicateEntry:
                                    Logger.LogWarning(RocketTools.Instance.Translate("error_duplicate_group", command[1]));
                                    break;
                                case RocketPermissionsProviderResult.Success:
                                    Logger.LogWarning(RocketTools.Instance.Translate("notification_success_group_create", command[1]));
                                    break;
                                default:
                                    Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                    break;
                            }
                        }
                        else if (OldGroup != null)
                        {
                            Logger.LogWarning(RocketTools.Instance.Translate("error_duplicate_group", command[1]));
                        }
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "delete")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (Permissions.DeleteGroup(command[1]))
                        {
                            case RocketPermissionsProviderResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[1]));
                                break;
                            case RocketPermissionsProviderResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_success_group_delete", command[1]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "details")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (Permissions.GetGroup(command[1]))
                        {
                            case null:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[1]));
                                break;
                            default:
                                List<string> details = RocketTools.Instance.URPerm.GetDetails(command[1]);
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_details_group", details[0], details[1], details[2], details[3], details[4], details[5], details[6], details[7]));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "add")
                    {
                        switch (RocketTools.Instance.URPerm.AddPermission(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_permission_added", command[1], command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_duplicate_permission", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "remove")
                    {
                        switch (RocketTools.Instance.URPerm.RemovePermission(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_permission_removed", command[1], command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.PermissionNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_permission", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "color")
                    {
                        switch (RocketTools.Instance.URPerm.SetColor(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_color_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_color", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.InvalidColor:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_invalid_color", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "prefix")
                    {
                        switch (RocketTools.Instance.URPerm.SetPrefix(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_prefix_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_prefix", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "suffix")
                    {
                        switch (RocketTools.Instance.URPerm.SetSuffix(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_suffix_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_suffix", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "displayname")
                    {
                        switch (RocketTools.Instance.URPerm.SetDisplayName(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_displayname_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_displayname", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "id")
                    {
                        switch (RocketTools.Instance.URPerm.SetID(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_id_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_id", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "parentgroup")
                    {
                        switch (RocketTools.Instance.URPerm.SetParentGroup(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_parentgroup_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_same_parentgroup", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "list")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (command[1].ToLower())
                        {
                            case "permissions":
                            case "perms":
                            case "perm":
                            case "p":
                            case "permission":
                            case "ps":
                                switch (Permissions.GetGroup(command[2]))
                                {
                                    case null:
                                        Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                        break;
                                    default:
                                        List<Permission> Perms = RocketTools.Instance.URPerm.GetPermissions(command[2]);
                                        if (Perms.Count == 0)
                                        {
                                            Logger.LogWarning(RocketTools.Instance.Translate("notification_list_no_perms"));
                                            return;
                                        }
                                        Logger.LogWarning(RocketTools.Instance.Translate("notification_list_start_perms", command[2]));
                                        foreach (Permission Perm in Perms)
                                        {
                                            Logger.LogWarning(RocketTools.Instance.Translate("notification_list_perms", Perm.Name, Perm.Cooldown));
                                        }
                                        break;
                                }
                                break;
                            case "m":
                            case "members":
                            case "mem":
                            case "membs":
                            case "memb":
                            case "mems":
                            case "ms":
                                switch (Permissions.GetGroup(command[2]))
                                {
                                    case null:
                                        Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                        break;
                                    default:
                                        List<string> Members = RocketTools.Instance.URPerm.GetMembers(command[2]);
                                        if (Members.Count == 0)
                                        {
                                            Logger.LogWarning(RocketTools.Instance.Translate("notification_list_no_players"));
                                            return;
                                        }
                                        Logger.LogWarning(RocketTools.Instance.Translate("notification_list_start_players", command[2]));
                                        foreach (string Player in Members)
                                        {
                                            Logger.LogWarning(RocketTools.Instance.Translate("notification_list_players", Player));
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    else if (command.Length == 4 && command[0].ToLower() == "add")
                    {
                        switch (RocketTools.Instance.URPerm.AddPermission(command[1], Convert.ToUInt32(command[2]), command[3]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                Logger.LogWarning(RocketTools.Instance.Translate("notification_permission_added", command[1], command[3]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_duplicate_permission", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_notfound_group", command[3]));
                                break;
                            default:
                                Logger.LogWarning(RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else
                    {
                        Logger.LogWarning(RocketTools.Instance.Translate("error_usage_rperm"));
                    }
                }
                else if (!(caller is ConsolePlayer))
                {
                    if (command.Length <= 1)
                    {
                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_usage_rperm"));
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "create")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        RocketPermissionsGroup NewGroup = new RocketPermissionsGroup(command[1], command[1], "", new List<string>(), new List<Permission>(), "white");
                        NewGroup.Prefix = "";
                        NewGroup.Suffix = "";
                        RocketPermissionsGroup OldGroup = Permissions.GetGroup(command[1]);
                        if (OldGroup == null)
                        {
                            switch (Permissions.AddGroup(NewGroup))
                            {
                                case RocketPermissionsProviderResult.DuplicateEntry:
                                    UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_duplicate_group", command[1]));
                                    break;
                                case RocketPermissionsProviderResult.Success:
                                    UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_success_group_create", command[1]));
                                    break;
                                default:
                                    UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                    break;
                            }
                        }
                        else if (OldGroup != null)
                        {
                            UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_duplicate_group", command[1]));
                        }
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "delete")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (Permissions.DeleteGroup(command[1]))
                        {
                            case RocketPermissionsProviderResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[1]));
                                break;
                            case RocketPermissionsProviderResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_success_group_delete", command[1]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 2 && command[0].ToLower() == "details")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (Permissions.GetGroup(command[1]))
                        {
                            case null:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[1]));
                                break;
                            default:
                                List<string> details = RocketTools.Instance.URPerm.GetDetails(command[1]);
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_details_group", details[0], details[1], details[2], details[3], details[4], details[5], details[6], details[7]));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "add")
                    {
                        switch (RocketTools.Instance.URPerm.AddPermission(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_permission_added", command[1], command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_duplicate_permission", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "remove")
                    {
                        switch (RocketTools.Instance.URPerm.RemovePermission(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_permission_removed", command[1], command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.PermissionNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_permission", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "color")
                    {
                        switch (RocketTools.Instance.URPerm.SetColor(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_color_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_color", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.InvalidColor:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_invalid_color", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "prefix")
                    {
                        switch (RocketTools.Instance.URPerm.SetPrefix(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_prefix_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_prefix", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "suffix")
                    {
                        switch (RocketTools.Instance.URPerm.SetSuffix(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_suffix_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_suffix", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "displayname")
                    {
                        switch (RocketTools.Instance.URPerm.SetDisplayName(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_displayname_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_displayname", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "id")
                    {
                        switch (RocketTools.Instance.URPerm.SetID(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_id_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_id", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "parentgroup")
                    {
                        switch (RocketTools.Instance.URPerm.SetParentGroup(command[1], command[2]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_parentgroup_change", command[2]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_same_parentgroup", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else if (command.Length == 3 && command[0].ToLower() == "list")
                    {
                        RocketPermissionsManager Permissions = R.Instance.GetComponent<RocketPermissionsManager>();
                        switch (command[1].ToLower())
                        {
                            case "permissions":
                            case "perms":
                            case "perm":
                            case "p":
                            case "permission":
                            case "ps":
                                switch (Permissions.GetGroup(command[2]))
                                {
                                    case null:
                                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                        break;
                                    default:
                                        List<Permission> Perms = RocketTools.Instance.URPerm.GetPermissions(command[2]);
                                        if (Perms.Count == 0)
                                        {
                                            UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_no_perms"));
                                            return;
                                        }
                                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_start_perms", command[2]));
                                        foreach (Permission Perm in Perms)
                                        {
                                            UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_perms", Perm.Name, Perm.Cooldown));
                                        }
                                        break;
                                }
                                break;
                            case "m":
                            case "members":
                            case "mem":
                            case "membs":
                            case "memb":
                            case "mems":
                            case "ms":
                                switch (Permissions.GetGroup(command[2]))
                                {
                                    case null:
                                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[2]));
                                        break;
                                    default:
                                        List<string> Members = RocketTools.Instance.URPerm.GetMembers(command[2]);
                                        if (Members.Count == 0)
                                        {
                                            UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_no_players"));
                                            return;
                                        }
                                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_start_players", command[2]));
                                        foreach (string Player in Members)
                                        {
                                            UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_list_players", Player));
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    else if (command.Length == 4 && command[0].ToLower() == "add")
                    {
                        switch (RocketTools.Instance.URPerm.AddPermission(command[1], Convert.ToUInt32(command[2]), command[3]))
                        {
                            case PermissionsHelper.PermissionHelperResult.Success:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("notification_permission_added", command[1], command[3]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.DuplicateEntry:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_duplicate_permission", command[1]));
                                break;
                            case PermissionsHelper.PermissionHelperResult.GroupNotFound:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_notfound_group", command[3]));
                                break;
                            default:
                                UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_unknown"));
                                break;
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_usage_rperm"));
                    }
                }
            }
            else if (!RocketTools.Instance.Configuration.Instance.EnableRPermCommand)
            {
                if (caller is ConsolePlayer)
                {
                    Logger.LogWarning("Command disabled by configuration setting.");
                }
                else if (!(caller is ConsolePlayer))
                {
                    UnturnedChat.Say(caller, RocketTools.Instance.Translate("error_command_disabled"));
                }
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "rocketpermission"
                };
            }
        }
    }
}
