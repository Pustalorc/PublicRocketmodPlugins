using Rocket.API;

namespace EFG.Duty
{
    public class DutyConfiguration : IRocketPluginConfiguration
    {
        public bool EnableServerAnnouncer;
        public bool RemoveAdminOnLogout;
        public bool AllowDutyCheck;
        public string MessageColor;
        public string HelperGroupName;
        public string ModeratorGroupName;
        public string AdminGroupName;


        public void LoadDefaults()
        {
            EnableServerAnnouncer = true;
            RemoveAdminOnLogout = true;
            AllowDutyCheck = true;
            MessageColor = "red";
            HelperGroupName = "Helper";
            ModeratorGroupName = "Moderator";
            AdminGroupName = "Administrator";
        }
    }
}
