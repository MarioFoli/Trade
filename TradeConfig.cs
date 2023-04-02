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
        public string howTo = "To add new trades, either use /trade add [item] [tradedItem] ingame, or add them manually here. Use of item id names or names are accepted.";
        public string errorMessage = "You don't have the required materials to trade for this item!";
        public Dictionary<string, string> possibleTrades = new Dictionary<string, string>
        {
            { "3017", "3208" }

        };
        //TODO: Add new trades via CSC, ex: apple -> terra blade


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
