using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade
{
    public class TradeConfigFile
    {
        public string howTo = "To add new trades, either use /tradeadd [item] [tradedItem] ingame, or add them manually here. Use of Item ID Names or item names are accepted. (/tradeadd and /traderemove are bugged, and only persist until a server restart.";
        public string howTo2 = "Currently, only 1:1 trades are possible. Example: Stone Block, Dirt Block creates a trade for  DirtBlock -> StoneBlock";
        public Dictionary<string, string> possibleTrades = new Dictionary<string, string>
        {
            { "Flower Boots", "Jungle Crate" },
            { "Anklet of the Wind", "Jungle Crate" },
            { "Feral Claws", "Jungle Crate" },
            { "Boomstick", "Jungle Crate" },
            { "Staff of Regrowth", "Jungle Crate" },
        };

        public static TradeConfigFile Read(string path)
        {
            if(!File.Exists(path))
            {
                return new TradeConfigFile();
            }
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(fs);
            }
        }
        public static TradeConfigFile Read(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var cf = JsonConvert.DeserializeObject<TradeConfigFile>(sr.ReadToEnd());
                if (ConfigRead != null)
                {
                    ConfigRead(cf);
                }
                return cf;
            }
        }
        public void Write(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                Write(fs);
            }
        }

        public void Write(Stream stream)
        {
            var str = JsonConvert.SerializeObject(this, Formatting.Indented);
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(str);
            }
        }
        public static Action<TradeConfigFile> ConfigRead;
    }
}
