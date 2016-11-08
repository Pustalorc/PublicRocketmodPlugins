using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Chat;

namespace Rocket_Rules
{
    public class CommandRules : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "Rules"; }
        }

        public string Help
        {
            get { return "Lists the rules set by the user."; }
        }

        public string Syntax
        {
            get { return "<page>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            bool error = false;
            string[] StoredRulesText = new string[RocketRules.Instance.StoredRulesText.Length];
            Color[] StoredRulesColor = new Color[RocketRules.Instance.StoredRulesColor.Length];
            StoredRulesText = RocketRules.Instance.StoredRulesText;
            StoredRulesColor = RocketRules.Instance.StoredRulesColor;
            int numRules = RocketRules.Instance.numRules;
            int extraRules = RocketRules.Instance.extraRules;
            int Pages = RocketRules.Instance.Pages;
            int CommandPage = 0;
            if (caller is ConsolePlayer)
            {
                // Dump all rules to console
                for (int i = 0; i <= StoredRulesText.Length - 1; i++)
                {
                    Rocket.Core.Logging.Logger.Log(RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[i]), ConsoleColor.Yellow);
                }
            }
            else
            {
                // Grab page syntax, if it wasn't set by player, set it to 1.

                if (command.Length != 1)
                {
                    CommandPage = 1;
                }
                else if (command.Length == 1)
                {
                    try
                    {
                        CommandPage = Convert.ToInt32(command[0]);
                    }
                    catch (FormatException)
                    {
                        UnturnedChat.Say(caller, string.Format("Failed to convert {0} to a number!", command[0]));
                        error = true;
                    }
                    catch (OverflowException)
                    {
                        UnturnedChat.Say(caller, string.Format("Failed to convert {0} to a number!", command[0]));
                        error = true;
                    }
                }

                // Start rules dumping to the player. Don't dump if there was an error with the syntax.

                if (error == false)
                {
                    if (CommandPage < Pages)
                    {
                        try
                        {
                            bool reminder0 = false;
                            bool reminder1 = false;
                            bool reminder2 = false;
                            for (int i = ((CommandPage * 4) - 4); i <= ((CommandPage * 4) - 1); i++)
                            {
                                if (i == (CommandPage * 4 - 1))
                                {
                                    UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("pages", Convert.ToString(CommandPage + 1)));
                                    reminder0 = true;
                                }
                                if (reminder0 == false && reminder1 == false && reminder2 == false)
                                {
                                    UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[i - (CommandPage - 1)]), StoredRulesColor[i - (CommandPage - 1)]);
                                }
                            }
                            reminder0 = false;
                            reminder1 = false;
                            reminder2 = false;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Rocket.Core.Logging.Logger.LogError("Unexpected error was encountered. If it's not caused because of your configuration, please report the error at https://github.com/persiafighter/Rocket-Rules/issues");
                        }
                    }
                    else if (CommandPage == Pages)
                    {
                        try
                        {
                            int counter = 0;
                            for (int i = ((CommandPage * 4) - 4); i <= ((CommandPage * 4) - 1); i++)
                            {
                                if (counter > extraRules - 1)
                                {
                                    UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("endofrules"), Color.green);
                                    i = 500000;
                                }
                                else if (counter <= extraRules)
                                {
                                    UnturnedChat.Say(caller, RocketRules.Instance.Translations.Instance.Translate("rule", StoredRulesText[i - (CommandPage - 1)]), StoredRulesColor[i - (CommandPage - 1)]);
                                }
                                counter = counter + 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Rocket.Core.Logging.Logger.LogError("Unexpected error was encountered. If it's not caused because of your configuration, please report the error at https://github.com/persiafighter/Rocket-Rules/issues");
                        }
                    }
                    else if (CommandPage > Pages)
                    {
                        UnturnedChat.Say(caller, "That page does not exist");
                    }
                }
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
              {
                  "help"
              };
            }
        }
    }
}
