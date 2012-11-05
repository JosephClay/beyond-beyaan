using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Beyond_Beyaan.Data_Managers
{
	//This stores all of the available items, such as ship items, buildings, regions, etc
	public class MasterItemManager
	{
		public Dictionary<string, BaseItem> Items { get; private set; }

		public MasterItemManager(DirectoryInfo directoryToLoad)
		{
			Items = new Dictionary<string, BaseItem>();

			XDocument doc = XDocument.Load(Path.Combine(directoryToLoad.FullName, "shipItems.xml"));
			LoadShipItems(doc.Root);
		}

		private void LoadShipItems(XElement root)
		{
			foreach (XElement element in root.Element("ShipMainItems").Elements())
			{
				ShipMainItem mainItem = new ShipMainItem(element);
				Items.Add(mainItem.Code, mainItem);
			}
			foreach (XElement element in root.Element("ShipModifierItems").Elements())
			{
				ShipModifierItem modifierItem = new ShipModifierItem(element);
				Items.Add(modifierItem.Code, modifierItem);
			}
			foreach (XElement element in root.Element("ShipEnhancerItems").Elements())
			{
				ShipEnhancerItem enhancerItem = new ShipEnhancerItem(element);
				Items.Add(enhancerItem.Code, enhancerItem);
			}
		}

		public BaseItem GetItem(string code)
		{
			if (Items.ContainsKey(code))
			{
				return Items[code];
			}
			return null;
		}
	}
}
