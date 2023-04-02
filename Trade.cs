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
            //initialize plugin here
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose used resources here
            }
            //dipose unused resources here
            base.Dispose(disposing);
        }
        public Trade(Main game)
            : base(game)
        {
            Order = 50;
            TradeConfig = new TradeConfigFile();

        }
        //main plugin logic

        public void OnInitialize()
        {
            SetupConfig();
            Commands.ChatCommands.Add(new Command("trade.cfg", TradeReload, "Tradereload")
            {
                AllowServer = true,
                HelpText = "Reloads from config file"
            });

    
            Commands.ChatCommands.Add(new Command("trade.list", TradeList, "Tradelist")
            {
                AllowServer = true,
                HelpText = "Lists all possible trades"
                
            });

            Commands.ChatCommands.Add(new Command("trade.trade", TradeTrade, "Tradetrade")
            {
                AllowServer = false,
                HelpText = "Exchanges items in your inventory for your desired item!"
 
            });
    
        }
        //startup logic
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

        //main command logic
        private void TradeReload(CommandArgs args)
        {
            SetupConfig();
            TShock.Log.ConsoleInfo("Trade Plugin Reload Intiated");
            args.Player.SendSuccessMessage("Trade Plugin Reload Initiated");
        }

        private void TradeList(CommandArgs args)
        {
            //implement logic for reading json file of all possible trades
        }
        private void TradeTrade(CommandArgs args)
        {
            //implement logic for trading
        }
    }
}