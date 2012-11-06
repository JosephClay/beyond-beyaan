using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Beyond_Beyaan.Scripts;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan
{
	public class TechnologyItem
	{
		public string Name { get; private set; }
		public int TechLevel { get; private set; }
		public int FudgeFactor { get; private set; }
		public bool IsRequired { get; private set; }
		public Dictionary<Resource, int> Cost { get; private set; }
		public string Category { get; private set; }

		#region Items that this technology unlock
		public List<ShipMainItem> ShipMainItems { get; private set; }
		public List<ShipModifierItem> ShipModifierItems { get; private set; }
		public List<ShipEnhancerItem> ShipEnhancerItems { get; private set; }
		#endregion

		#region Items that this technology removes (such as obsolete items)
		public List<ShipMainItem> ShipMainItemsRemoved { get; private set; }
		public List<ShipModifierItem> ShipModifierItemsRemoved { get; private set; }
		public List<ShipEnhancerItem> ShipEnhancerItemsRemoved { get; private set; }
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="technologyToLoad">The base technology</param>
		public TechnologyItem(XElement technologyToLoad, ResourceManager resourceManager, MasterItemManager masterItemManager)
		{
			InitializeLists();

			//Load the attributes
			foreach (XAttribute attribute in technologyToLoad.Attributes())
			{
				switch (attribute.Name.LocalName.ToLower())
				{
					case "name": Name = attribute.Value;
						break;
					case "techlevel": TechLevel = int.Parse(attribute.Value);
						break;
					case "fudge": FudgeFactor = int.Parse(attribute.Value);
						break;
					case "category": Category = attribute.Value;
						break;
					case "isrequired": IsRequired = bool.Parse(attribute.Value);
						break;
					case "cost":
						{
							string[] parts = attribute.Value.Split(new[] { '|' });
							foreach (string part in parts)
							{
								string[] cost = part.Split(new[] { ',' });
								Cost.Add(resourceManager.GetResource(cost[0]), int.Parse(cost[1]));
							}
						} break;
				}
			}
			foreach (XElement element in technologyToLoad.Elements())
			{
				var item = masterItemManager.GetItem(element.Attribute("code").Value);
				switch (element.Name.ToString().ToLower())
				{
					case "shipmainitem": ShipMainItems.Add((ShipMainItem)item);
						break;
					case "shipmodifieritem": ShipModifierItems.Add((ShipModifierItem)item);
						break;
					case "shipenhanceritem": ShipEnhancerItems.Add((ShipEnhancerItem)item);
						break;
					case "removeshipmainitem": ShipMainItemsRemoved.Add((ShipMainItem)item);
						break;
					case "removeshipmodifieritem": ShipModifierItemsRemoved.Add((ShipModifierItem)item);
						break;
					case "removeshipenhanceritem": ShipEnhancerItemsRemoved.Add((ShipEnhancerItem)item);
						break;
				}
			}
		}

		private void InitializeLists()
		{
			ShipMainItems = new List<ShipMainItem>();
			ShipModifierItems = new List<ShipModifierItem>();
			ShipEnhancerItems = new List<ShipEnhancerItem>();

			ShipMainItemsRemoved = new List<ShipMainItem>();
			ShipModifierItemsRemoved = new List<ShipModifierItem>();
			ShipEnhancerItemsRemoved = new List<ShipEnhancerItem>();

			Cost = new Dictionary<Resource, int>();
		}

		public Dictionary<string, BaseItem> ItemsToAdd()
		{
			Dictionary<string, BaseItem> itemsToAdd = new Dictionary<string, BaseItem>();
			foreach (ShipMainItem item in ShipMainItems)
			{
				itemsToAdd.Add(item.Code, item);
			}
			foreach (ShipModifierItem item in ShipModifierItems)
			{
				itemsToAdd.Add(item.Code, item);
			}
			foreach (ShipEnhancerItem item in ShipEnhancerItems)
			{
				itemsToAdd.Add(item.Code, item);
			}
			return itemsToAdd;
		}
		public Dictionary<string, BaseItem> ItemsToRemove()
		{
			Dictionary<string, BaseItem> itemsToRemove = new Dictionary<string, BaseItem>();
			foreach (ShipMainItem item in ShipMainItemsRemoved)
			{
				itemsToRemove.Add(item.Code, item);
			}
			foreach (ShipModifierItem item in ShipModifierItemsRemoved)
			{
				itemsToRemove.Add(item.Code, item);
			}
			foreach (ShipEnhancerItem item in ShipEnhancerItemsRemoved)
			{
				itemsToRemove.Add(item.Code, item);
			}
			return itemsToRemove;
		}
	}

	public class TechnologyProject
	{
		public TechnologyItem TechBeingResearched { get; private set; }
		public bool Locked { get; set; }
		public int Percentage { get; set; }
		public Dictionary<Resource, float> Invested { get; private set; }

		public TechnologyProject(TechnologyItem item)
		{
			TechBeingResearched = item;
			Invested = new Dictionary<Resource, float>();
			foreach (KeyValuePair<Resource, int> resource in TechBeingResearched.Cost)
			{
				Invested.Add(resource.Key, 0);
			}
			Percentage = 0;
			Locked = false;
		}

		//Returns true if the project's cost is paid.
		public bool InvestProject(Resource resource, float amount)
		{
			Invested[resource] += amount;
			foreach (KeyValuePair<Resource, float> item in Invested)
			{
				if (TechBeingResearched.Cost[item.Key] > item.Value)
				{
					return false;
				}
			}
			return true;
		}
	}

	//This is an base interface that specifies the standard layout of different components in the game, such as ship items, buildings, etc
	public class BaseItem
	{
		public string Code { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public Dictionary<string, object> Values { get; private set; }

		public BaseItem(XElement itemToLoad)
		{
			Values = new Dictionary<string, object>();
			foreach (XAttribute attribute in itemToLoad.Attributes())
			{
				switch (attribute.Name.LocalName.ToLower())
				{
					case "code": Code = attribute.Value;
						break;
					case "name": Name = attribute.Value;
						break;
					case "description": Description = attribute.Value;
						break;
					default: Values.Add(attribute.Name.LocalName.ToLower(), attribute.Value);
						break;
				}
			}
		}

		protected void AddControls(List<BaseControl> controls, string item)
		{
			string[] items = item.Split(new[] { '|' });

			foreach (string part in items)
			{
				string[] parts = part.Split(new[] { ',' });
				switch (parts[0].ToLower())
				{
					case "numupdown":
						{
							int min = int.Parse(parts[3]);
							int max = int.Parse(parts[4]);
							int incr = int.Parse(parts[5]);
							int origVal = int.Parse(parts[6]);
							NumUpDown control = new NumUpDown(min, max, incr, origVal, parts[1], parts[2]);
							controls.Add(control);
						} break;
				}
			}
		}
	}

	public class ShipMainItem : BaseItem
	{
		public string Type { get; private set; }
		public List<string> Modifiers { get; private set; }
		public List<string> Enhancers { get; private set; }

		public List<BaseControl> Controls { get; private set; }

		public ShipItemScript Script { get; private set; }

		public ShipMainItem(XElement itemToLoad) : base(itemToLoad)
		{
			Type = (string)Values["type"];
			Modifiers = new List<string>();
			Enhancers = new List<string>();
			Controls = new List<BaseControl>();

			foreach (string part in ((string)Values["modifiers"]).Split(new[] { '|' }))
			{
				Modifiers.Add(part);
			}
			foreach (string part in ((string)Values["enhancers"]).Split(new[] { '|' }))
			{
				Enhancers.Add(part);
			}

			if (Values.ContainsKey("controls"))
			{
				AddControls(Controls, (string)Values["controls"]);
			}
		}
	}

	public class ShipModifierItem : BaseItem
	{
		public List<string> Modifiers { get; private set; }

		public ShipModifierItem(XElement itemToLoad)
			: base(itemToLoad)
		{
			Modifiers = new List<string>();
			foreach (string part in ((string)Values["modifiers"]).Split(new[] { '|' }))
			{
				Modifiers.Add(part);
			}
		}
	}

	public class ShipEnhancerItem : BaseItem
	{
		public List<string> Enhancers { get; private set; }

		public ShipEnhancerItem(XElement itemToLoad)
			: base(itemToLoad)
		{
			Enhancers = new List<string>();
			foreach (string part in ((string)Values["enhancers"]).Split(new[] { '|' }))
			{
				Enhancers.Add(part);
			}
		}
	}

	public class EquipmentInstance : Equipment
	{
		//This is for storing the equipment data invidiually
		public Dictionary<string, object> Values { get; set; }

		public EquipmentInstance(Equipment equipmentToCopy, int shipSize) :
			base (equipmentToCopy)
		{
			Values = new Dictionary<string, object>();
			//Values.Add("Range", Range());
			//Values.Add("Space", GetSpaceUsage(shipSize));
			//Values.Add("Passive", IsPassive());
			//Values.Add("Cost", GetCost(shipSize));
			//Values.Add("Power", GetPower(shipSize));
			//Values.Add("Time", GetTime(shipSize));
			//Values.Add("Name", GetName());
			//Values.Add("GalaxySpeed", GetSpeed());
			//Values.Add("SelectionSize", GetSelectionSize(shipSize));
		}
	}

	public class Equipment
	{
		public ShipMainItem ShipMainItem { get; set; }
		public List<ShipModifierItem> ShipModifierItems { get; private set; }
		public List<ShipEnhancerItem> ShipEnhancerItems { get; private set; }

		public List<Icon> CombatIcons { get; private set; } //For combat view and other views
		public List<Icon> DesignIcons { get; private set; } //For ship design view
		public List<Icon> DesignControls { get; private set; } //For ship design view that requires user to input values (such as number of mounts, amount of ammo, etc)

		public List<BaseControl> Controls { get; private set; } //For storing and handling controls for main item

		public Equipment()
		{
			ShipMainItem = null;
			ShipModifierItems = new List<ShipModifierItem>();
			ShipEnhancerItems = new List<ShipEnhancerItem>();

			CombatIcons = new List<Icon>();
			DesignIcons = new List<Icon>();
			DesignControls = new List<Icon>();

			Controls = new List<BaseControl>();
		}
		public Equipment(ShipMainItem mainItem, List<ShipModifierItem> modifierItems, List<ShipEnhancerItem> enhancerItems, IconManager iconManager)
		{
			ShipMainItem = mainItem;
			ShipModifierItems = new List<ShipModifierItem>(modifierItems);
			ShipEnhancerItems = new List<ShipEnhancerItem>(enhancerItems);

			Controls = new List<BaseControl>();
			CopyControls(mainItem.Controls);
		}

		public Equipment(Equipment equipmentToCopy)
		{
			ShipMainItem = equipmentToCopy.ShipMainItem;
			ShipModifierItems = new List<ShipModifierItem>(equipmentToCopy.ShipModifierItems);
			ShipEnhancerItems = new List<ShipEnhancerItem>(equipmentToCopy.ShipEnhancerItems);

			CombatIcons = new List<Icon>(equipmentToCopy.CombatIcons);
			DesignIcons = new List<Icon>(equipmentToCopy.DesignIcons);
			DesignControls = new List<Icon>(equipmentToCopy.DesignControls);

			Controls = new List<BaseControl>(equipmentToCopy.Controls);
		}

		public bool IsThisEquipmentTheSame(Equipment equipmentToCompare)
		{
			if (ShipMainItem != equipmentToCompare.ShipMainItem)
			{
				return false;
			}
			if (ShipModifierItems.Count != equipmentToCompare.ShipModifierItems.Count)
			{
				return false;
			}
			for (int i = 0; i < ShipModifierItems.Count; i++)
			{
				if (ShipModifierItems[i] != equipmentToCompare.ShipModifierItems[i])
				{
					return false;
				}
			}
			if (ShipEnhancerItems.Count != equipmentToCompare.ShipEnhancerItems.Count)
			{
				return false;
			}
			for (int i = 0; i < ShipEnhancerItems.Count; i++)
			{
				if (ShipEnhancerItems[i] != equipmentToCompare.ShipEnhancerItems[i])
				{
					return false;
				}
			}
			return true;
		}

		private void CopyControls(List<BaseControl> controls)
		{
			foreach (BaseControl control in controls)
			{
				if (control is NumUpDown)
				{
					var convertedControl = (NumUpDown)control;
					NumUpDown newControl = new NumUpDown(convertedControl.MinValue, convertedControl.MaxValue, convertedControl.IncrAmount, convertedControl.Value, convertedControl.Name, convertedControl.Code);
					Controls.Add(newControl);
				}
			}
		}

		/*public bool IsPassive()
		{
			if (!ItemType.IsPassive)
			{
				return false;
			}
			if (EquipmentType == Beyond_Beyaan.EquipmentType.SPECIAL)
			{
				return true;
			}
			if (!MountType.IsPassive)
			{
				return false;
			}
			foreach (TechnologyItem item in ModifierItems)
			{
				if (!item.IsPassive)
				{
					return false;
				}
			}
			return true;
		}*/

		#region Name
		public string GetName()
		{
			if (ShipMainItem.Script != null)
			{
				Dictionary<string, object> values = GetEquipmentInfo(null);
				if (values.ContainsKey("_name"))
				{
					return (string)values["_name"];
				}
			}
			
			string name = ShipMainItem.Name;
			foreach (var modifier in ShipModifierItems)
			{
				name = string.Format(modifier.Name, name);
			}
			return name;
		}
		#endregion

		#region Script Functions
		public Dictionary<string, object> GetEquipmentInfo(Dictionary<string, object> shipValues)
		{
			if (ShipMainItem.Script != null)
			{
				Dictionary<string, object>[] modifierValues = new Dictionary<string, object>[ShipModifierItems.Count];
				Dictionary<string, object>[] enhancerValues = new Dictionary<string, object>[ShipEnhancerItems.Count];
				Dictionary<string, object> controlValues = new Dictionary<string, object>();
				for (int i = 0; i < modifierValues.Length; i++)
				{
					modifierValues[i] = new Dictionary<string, object>(ShipModifierItems[i].Values);
				}
				for (int i = 0; i < enhancerValues.Length; i++)
				{
					enhancerValues[i] = new Dictionary<string, object>(ShipEnhancerItems[i].Values);
				}
				foreach (BaseControl control in ShipMainItem.Controls)
				{
					controlValues.Add(control.Code, control.GetValue());
				}
				return ShipMainItem.Script.GetEquipmentInfo(ShipMainItem.Values, modifierValues, enhancerValues, shipValues, controlValues);
			}
			return null;
		}
		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues)
		{
			Dictionary<string, object> values = GetEquipmentInfo(shipValues);
			if (values != null)
			{
				return ShipMainItem.Script.UpdateShipInfo(shipValues, values);
			}
			return shipValues;
		}
		
		public int GetSelectionSize(int shipSize)
		{
			if (ShipMainItem.Script != null)
			{
				return ShipMainItem.Script.GetTargetReticle(shipSize);
			}
			return -1;
		}
		public Dictionary<string, object>[] Activate(Point gridcell, Point shipPos, Point boundary, Dictionary<string, object> equipmentValues)
		{
			if (ShipMainItem.Script != null)
			{
				return ShipMainItem.Script.Activate(shipPos.X, shipPos.Y, gridcell.X, gridcell.Y, boundary.X, boundary.Y, equipmentValues);
			}
			return null;
		}
		public Dictionary<string, object>[] OnHit(Point impact, Point shipPos, int shipSize, Dictionary<string, object> equipmentValues, Dictionary<string, object> particleValues)
		{
			if (ShipMainItem.Script != null)
			{
				return ShipMainItem.Script.OnHit(impact.X, impact.Y, shipPos.X, shipPos.Y, shipSize, equipmentValues, particleValues);
			}
			return null;
		}
		#endregion
	}
}
