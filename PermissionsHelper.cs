using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Collections.Generic;

namespace Bloodstone.Systems.RocketTools.Systems
{
    public sealed class PermissionsHelper
    {
        IRocketPermissionsProvider Permissions = R.Instance.GetComponent<IRocketPermissionsProvider>();

        public enum PermissionHelperResult { Success, UnspecifiedError, DuplicateEntry, GroupNotFound, PermissionNotFound, InvalidColor };

        public PermissionHelperResult SetPrefix(string prefix, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                if (Group.Prefix != prefix)
                {
                    Group.Prefix = prefix;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.Prefix == prefix)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult SetSuffix(string suffix, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                if (Group.Suffix != suffix)
                {
                    Group.Suffix = suffix;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.Suffix == suffix)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult SetColor(string color, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                if ((UnturnedChat.GetColorFromName(color, UnityEngine.Color.black) == UnityEngine.Color.black && color.ToLower() == "black" && color.ToLower() == "000000") || UnturnedChat.GetColorFromName(color, UnityEngine.Color.black) != UnityEngine.Color.black)
                {
                    Group.Color = color;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (UnturnedChat.GetColorFromName(color, UnityEngine.Color.black) == UnityEngine.Color.black)
                {
                    return PermissionHelperResult.InvalidColor;
                }
                else if (Group.Color == color)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult RemovePermission(string permission, string group) 
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                foreach (Permission a in Group.Permissions)
                {
                    if (a.Name == permission)
                    {
                        Group.Permissions.Remove(a);
                        Permissions.SaveGroup(Group);
                        return PermissionHelperResult.Success;
                    }
                }
                return PermissionHelperResult.PermissionNotFound;
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult AddPermission(string permission, string group)
        {
            Permission NewPerm = new Permission(permission);
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                foreach (Permission a in Group.Permissions)
                {
                    if (a.Name == permission)
                    {
                    return PermissionHelperResult.DuplicateEntry;
                    }
                }
                Group.Permissions.Add(NewPerm);
                Permissions.SaveGroup(Group);
                return PermissionHelperResult.Success;
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult AddPermission(string permission, uint cooldown, string group)
        {
            Permission NewPerm = new Permission(permission, cooldown);
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                foreach (Permission a in Group.Permissions)
                {
                    if (a.Name == permission)
                    {
                        return PermissionHelperResult.DuplicateEntry;
                    }
                }
                Group.Permissions.Add(NewPerm);
                Permissions.SaveGroup(Group);
                return PermissionHelperResult.Success;
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }
        
        public PermissionHelperResult SetDisplayName(string newname, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                if (Group.DisplayName != newname)
                {
                    Group.DisplayName = newname;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.DisplayName == newname)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult SetID(string newid, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            RocketPermissionsGroup TGroup = Permissions.GetGroup(newid);
            if (Group != null && TGroup == null)
            {
                if (Group.Id != newid)
                {
                    Group.Id = newid;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.Id == newid)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            else if (TGroup != null)
            {
                return PermissionHelperResult.DuplicateEntry;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult SetParentGroup(string parent, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            RocketPermissionsGroup TGroup = Permissions.GetGroup(parent);
            if (Group != null && TGroup != null)
            {
                if (Group.ParentGroup != parent)
                {
                    Group.ParentGroup = parent;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.ParentGroup == parent)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null || TGroup == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public PermissionHelperResult SetPriority(short priority, string group)
        {
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                if (Group.Priority != priority)
                {
                    Group.Priority = priority;
                    Permissions.SaveGroup(Group);
                    return PermissionHelperResult.Success;
                }
                else if (Group.Priority == priority)
                {
                    return PermissionHelperResult.DuplicateEntry;
                }
            }
            else if (Group == null)
            {
                return PermissionHelperResult.GroupNotFound;
            }
            return PermissionHelperResult.UnspecifiedError;
        }

        public List<string> GetDetails(string group)
        {
            List<string> Result = new List<string>();
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                Result.Add(Group.Id);
                Result.Add(Group.DisplayName);
                Result.Add(Group.Prefix);
                Result.Add(Group.Suffix);
                Result.Add(Group.Color);
                Result.Add(Group.ParentGroup);
                Result.Add(Group.Members.Count.ToString());
                Result.Add(Group.Permissions.Count.ToString());
                Result.Add(Group.Priority.ToString());
            }
            return Result;
        }

        public List<Permission> GetPermissions(string group)
        {
            List<Permission> Result = new List<Permission>();
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                foreach (Permission Permission in Group.Permissions)
                {
                    Result.Add(Permission);
                }
            }
            return Result;
        }

        public List<string> GetMembers(string group)
        {
            List<string> Result = new List<string>();
            RocketPermissionsGroup Group = Permissions.GetGroup(group);
            if (Group != null)
            {
                foreach (string ID in Group.Members)
                {
                    bool flag = false;
                    foreach (SteamPlayer Player in Provider.clients)
                    {
                        if (Player.playerID.steamID.ToString() == ID)
                        {
                            Result.Add(Player.playerID.characterName);
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue;
                    }
                    Result.Add(ID);
                }
            }
            return Result;
        }
    }
}
