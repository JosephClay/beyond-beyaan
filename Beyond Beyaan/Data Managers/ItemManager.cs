using System.Collections.Generic;

namespace Beyond_Beyaan.Data_Managers
{
	public class ItemManager
	{
		public List<ShipMainItem> ShipMainItems { get; private set; }
		public List<ShipModifierItem> ShipModifierItems { get; private set; }
		public List<ShipEnhancerItem> ShipEnhancerItems { get; private set; }

		public ItemManager()
		{
			ShipMainItems = new List<ShipMainItem>();
			ShipModifierItems = new List<ShipModifierItem>();
			ShipEnhancerItems = new List<ShipEnhancerItem>();
		}

		public void UpdateItems(Dictionary<string, BaseItem> itemsToAdd, Dictionary<string, BaseItem> itemsToRemove)
		{
			foreach (KeyValuePair<string, BaseItem> item in itemsToAdd)
			{
				if (item.Value is ShipMainItem && !ShipMainItems.Contains((ShipMainItem)item.Value))
				{
					ShipMainItems.Add((ShipMainItem)item.Value);
				}
				else if (item.Value is ShipModifierItem && !ShipModifierItems.Contains((ShipModifierItem)item.Value))
				{
					ShipModifierItems.Add((ShipModifierItem)item.Value);
				}
				else if (item.Value is ShipEnhancerItem && !ShipEnhancerItems.Contains((ShipEnhancerItem)item.Value))
				{
					ShipEnhancerItems.Add((ShipEnhancerItem)item.Value);
				}
			}
			foreach (KeyValuePair<string, BaseItem> item in itemsToRemove)
			{
				if (item.Value is ShipMainItem && ShipMainItems.Contains((ShipMainItem)item.Value))
				{
					ShipMainItems.Remove((ShipMainItem)item.Value);
				}
				else if (item.Value is ShipModifierItem && ShipModifierItems.Contains((ShipModifierItem)item.Value))
				{
					ShipModifierItems.Remove((ShipModifierItem)item.Value);
				}
				else if (item.Value is ShipEnhancerItem && ShipEnhancerItems.Contains((ShipEnhancerItem)item.Value))
				{
					ShipEnhancerItems.Remove((ShipEnhancerItem)item.Value);
				}
			}
		}
	}
}
