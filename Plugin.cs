using Persiafighter.Plugins.AntiAFK.Classes;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using Rocket.API.Collections;
using System.Linq;

namespace Persiafighter.Plugins.AntiAFK
{
    public sealed class Main : RocketPlugin<Configuration>
    {
        Dictionary<CSteamID, PlayerData> LastPosition = new Dictionary<CSteamID, PlayerData>();

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "afk_kick_reason", "You have been afk for too long!" }
        };

        protected override void Load()
        {
            Console.WriteLine("Anti-AFK by persiafighter has been loaded!");
        }

        protected override void Unload()
        {
            Console.WriteLine("Anti-AFK by persiafighter has been unloaded!");
        }

        public override string ToString()
        {
            return "Persiafighter's Anti-AFK";
        }

        private void Update()
        {
            foreach (var a in Provider.clients.ToList())
            {
                var ID = a.playerID.steamID;
                var UPlayer = UnturnedPlayer.FromSteamPlayer(a);

                if (!LastPosition.ContainsKey(ID))
                {
                    LastPosition.Add(ID, new PlayerData(UPlayer.Position, DateTime.Now));
                    return;
                }
                else if (LastPosition.ContainsKey(ID))
                {
                    if (LastPosition[ID].LastPosition == UPlayer.Position && DateTime.Now.Subtract(LastPosition[ID].LastPositionChange).TotalSeconds >= Configuration.Instance.AFKMaxTime)
                        Provider.kick(ID, Translate("afk_kick_reason"));
                    else if (LastPosition[ID].LastPosition != UPlayer.Position)
                        LastPosition[ID].Update(UPlayer.Position, DateTime.Now);
                }
            }
        }
    }

    public sealed class Configuration : IRocketPluginConfiguration
    {
        public uint AFKMaxTime;

        public void LoadDefaults()
        {
            AFKMaxTime = 300;
        }
    }
}
