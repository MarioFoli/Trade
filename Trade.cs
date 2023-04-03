using System.Reflection;
using TerrariaApi.Server;
using TShockAPI;
using Terraria;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Trade
{
    [ApiVersion(2, 1)]
    public class Trade : TerrariaPlugin
    {
        public static TradeConfigFile TradeConfig { get; set; }
        internal static string TradeConfigPath { get { return Path.Combine(TShock.SavePath, "Tradeconfig.json"); } }

        public override string Name
        {
            get { return "Trade Plugin"; }
        }
        public override String Author
        {
            get { return "MarioFoli"; }
        }
        public override string Description
        {
            get { return "Allows for customizable trading of items using /trade"; }
        }
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public override void Initialize()
        {
            SetupConfig();
            ServerApi.Hooks.GameInitialize.Register(this, (args) => { OnInitialize(); });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, (args) => { OnInitialize(); });
            }
            base.Dispose(disposing);
        }
        public Trade(Main game)
            : base(game)
        {
            Order = 50;
            TradeConfig = new TradeConfigFile();

        }
        public void OnInitialize()
        {
            SetupConfig();
            Commands.ChatCommands.Add(new Command("trade.admin", TradeReload, "tradereload")
            {
                AllowServer = true,
                HelpText = "Reloads from config file"
            });

            Commands.ChatCommands.Add(new Command("trade.list", TradeList, "tradelist")
            {
                AllowServer = true,
                HelpText = "Lists all possible trades"
                
            });

            Commands.ChatCommands.Add(new Command("trade.trade", TradeTrade, "trade")
            {
                AllowServer = false,
                HelpText = "Exchanges items in your inventory for your desired item!"
 
            });
            Commands.ChatCommands.Add(new Command("trade.add", TradeAdd, "tradeadd")
            {
                AllowServer = true,
                HelpText = "Adds a new possible trade to the config file."

            });

        }
        public static void SetupConfig()
        {
            try
            {
                if(File.Exists(TradeConfigPath))
                {
                    TradeConfig = TradeConfigFile.Read(TradeConfigPath);
                }
                TradeConfig.Write(TradeConfigPath);
            } catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Trade] Error in config file");
                Console.ForegroundColor = ConsoleColor.Gray;
                TShock.Log.ConsoleError("[Trade] Config file error");
                TShock.Log.ConsoleError(ex.ToString());
            }
        }
        private void TradeReload(CommandArgs args)
        {
            SetupConfig();
            TShock.Log.ConsoleInfo("Trade Plugin Reload Intiated");
            args.Player.SendSuccessMessage("Trade Plugin Reload Initiated");
        }
        private void TradeAdd(CommandArgs args)
        {
            if (args.Parameters.Count < 2)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper synax: /tradeadd [item] [tradedItem]");
            }
            string item = args.Parameters[0].ToString();
            string tradedItem = args.Parameters[1].ToString();
            try
            {
                TradeConfig.possibleTrades.Add(tradedItem, item);
                args.Player.SendSuccessMessage("Added trade for " + item + "-> " + tradedItem + "!");
            } catch (Exception ex)
            {
                args.Player.SendErrorMessage("That item is invalid, or already has a trade associated with it.");
            } 
        }
        private void TradeList(CommandArgs args)
        {
            args.Player.SendMessage("Current Trades:", Microsoft.Xna.Framework.Color.White);
            args.Player.SendMessage("(Note: Some items are in Item ID Form)", Microsoft.Xna.Framework.Color.Gray);
            foreach (KeyValuePair<string, string> trade in TradeConfig.possibleTrades)
            {
                args.Player.SendMessage(trade.Value + " -> " + trade.Key, Microsoft.Xna.Framework.Color.White);
            }
        }
        private void TradeTrade(CommandArgs args)
        {
            if (args.Parameters[0] == "help")
            {
                args.Player.SendMessage("Commands:", Microsoft.Xna.Framework.Color.White);
                args.Player.SendMessage("/trade [item] [desireditem] || Turns item -> desired item", Microsoft.Xna.Framework.Color.White);
            } else
            {
                try
                { 
                    string tradedItem = args.Parameters[0];
                    string desiredItem = args.Parameters[1];
                    if (tradedItem != TradeConfig.possibleTrades.GetValueOrDefault(desiredItem))
                    {
                        args.Player.SendErrorMessage("That trade doesn't exist. Use /tradelist for a list of possible trades");
                    } else
                    {
                        TSPlayer player = args.Player;
                        Item[] inventory = player.TPlayer.inventory;
                        bool foundItem = false;
                        var list = TShock.Utils.GetItemByIdOrName(tradedItem)[0];
                        for (int i = 0; i < inventory.Length; i++)
                        {
                            if (inventory[i].IsTheSameAs(list))
                            {
                                args.Player.SendSuccessMessage("Success!");
                                player.TPlayer.inventory[i] = TShock.Utils.GetItemByIdOrName(desiredItem)[0];
                                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[i].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, i, player.TPlayer.inventory[i].prefix);
                                NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[i].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, i, player.TPlayer.inventory[i].prefix);
                                foundItem = true;
                                break;
                            }
                        }
                        if (foundItem == false)
                        {
                            args.Player.SendErrorMessage("You don't have the required items for this trade! View current trades with /tradelist!");
                        }
                    }
                } catch (Exception ex)
                {
                    args.Player.SendErrorMessage("That trade doesn't exist. Use /tradelist for a list of possible trades.");
                }
            }
        }
    }
}