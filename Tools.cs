using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;

namespace RocketTools
{
    public class RocketTools : RocketPlugin<NobStartConfiguration>
    {
        public static RocketTools Instance;
        public PermissionsHelper URPerm;
        const string Build = "3";
        
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    { "error_usage_rperm", "Invalid command syntax." },
                    { "error_duplicate_group", "That group already exists!" },
                    { "error_duplicate_permission", "The group {0} already has that permission!" },
                    { "error_notfound_group", "That group does not exist!" },
                    { "error_notfound_permission", "The group does not have the permission {0}."},
                    { "error_same_color", "The group {0} already has that color!" },
                    { "error_same_prefix", "The group {0} already has that prefix!" },
                    { "error_same_suffix", "The group {0} already has that suffix!" },
                    { "error_same_parentgroup", "The group {0} already has that parentgroup!" },
                    { "error_same_id", "The group {0} already has that ID or another group already exists with that name!" },
                    { "error_same_displayname", "The group {0} already has that display name!" },
                    { "error_invalid_color", "The color {0} is invalid." },
                    { "error_unknown", "An unknown error has happened! O.O" },
                    { "error_command_disabled", "That command has been disabled by the server." },
                    { "notification_success_group_create", "Permission group {0} was created successfully!" },
                    { "notification_success_group_delete", "Permission group {0} was deleted successfully!" },
                    { "notification_permission_added", "Permission {0} was added to the group {1}." },
                    { "notification_permission_removed", "Permission {0} was removed from the group {1}." },
                    { "notification_color_change", "Chat color was changed for the group {0}." },
                    { "notification_prefix_change", "Prefix was changed for the group {0}." },
                    { "notification_suffix_change", "Suffix was changed for the group {0}." },
                    { "notification_parentgroup_change", "Parent group was changed for the group {0}." },
                    { "notification_id_change", "ID was changed for the group {0}." },
                    { "notification_displayname_change", "Display name was changed for the group {0}." },
                    { "notification_details_group", "ID: {0}, Display Name: {1}, Prefix: {2}, Suffix: {3}, Color: {4}, Parent Group: {5}, {6} Members and {7} Permissions." },
                    { "notification_list_start_players", "Players in {0}:" },
                    { "notification_list_start_perms", "Permissions in {0}:" },
                    { "notification_list_perms", "Permission: {0}, Cooldown: {1}." },
                    { "notification_list_players", "Player: {0}" },
                    { "notification_list_no_players", "There are no players in that group." },
                    { "notification_list_no_perms", "There are no permissions in that group." }
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            if (!Configuration.Instance.EnablePlugin)
            {
                UnloadPlugin(PluginState.Cancelled);
                return;
            }
            URPerm = new PermissionsHelper();

            Rocket.Core.Logging.Logger.LogWarning("RocketTools by persiafighter, build #" + Build + " has been loaded!");
        }

        protected override void Unload()
        {
            Instance = null;
            if (Configuration.Instance.EnablePlugin)
            {
                URPerm = null;
            }

            Rocket.Core.Logging.Logger.LogWarning("RocketTools has been unloaded!");
        }
    }
}
