using System.Reflection;
using TerrariaApi.Server;
using TShockAPI;
using Terraria;

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
            Commands.ChatCommands.Add(new Command("trade.admin", TradeReload, "trade reload")
            {
                AllowServer = true,
                HelpText = "Reloads from config file"
            });

    
            Commands.ChatCommands.Add(new Command("trade.list", TradeList, "trade list")
            {
                AllowServer = true,
                HelpText = "Lists all possible trades"
                
            });

            Commands.ChatCommands.Add(new Command("trade.trade", TradeTrade, "trade")
            {
                AllowServer = false,
                HelpText = "Exchanges items in your inventory for your desired item!"
 
            });
            Commands.ChatCommands.Add(new Command("trade.add", TradeAdd, "trade add")
            {
                AllowServer = true,
                HelpText = "Adds a new possible trade to the config file."

            });

        }
        //Startup and Reload Logic:
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


        //Main Command Logic:
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
                args.Player.SendErrorMessage("Invalid syntax! Proper synax: //trade add [item] [tradedItem]");
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
            //TODO: List possible trades and their required items from config
        }
        private void TradeTrade(CommandArgs args)
        {
            //TODO: Command args need to pass item desired,
            //match what item player needs from configs
            //loop through inventory to see if player has item
            //if they do, replace item in inventory with item from config
            TSPlayer player = args.Player;
            Item[] inventory = player.TPlayer.inventory;
            foreach (Item item in inventory)
            {
 
            }
        }
    }
}