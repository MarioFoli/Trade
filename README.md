# Trade for TShock


Trade is a plugin for TShock Terraria that enables customizable trading between player and the server.
New trades can be added in game, or via the config. Item IDs or names are acceptable.



Commands:
/trade [item] [desiredItem] || Replaces an item in your inventory into the desired item.  
/tradelist || Lists all possible trades.  
/tradeadd [item] [desiredItem] || Adds a new possible trade to the config. 
/traderemove [item] [desiredItem] || Removes a trade from the config.
/tradereload || Reloads the plugin.  

Permissions:
- trade.admin || Allows use of /tradeadd and /tradereload
- trade.trade || Allows use of /tradelist and /trade


Known bugs:  
- Added trades via /tradeadd do not persist after a server restart. For now, use the config.      
- Trades do not take into account number of items. Eg, trading 1 dirt for 1 stone while having 999 dirt will result in only one stone.  
