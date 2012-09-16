using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan
{
	public class TechnologyItem
	{
		#region Attributes
		public const string POWER = "basePower";
		public const string POWER_PERCENTAGE = "basePowerPercentage";
		public const string TIME = "baseTime";
		public const string TIME_PERCENTAGE = "baseTimePercentage";
		public const string COST = "baseCost";
		public const string COST_PERCENTAGE = "baseCostPercentage";
		public const string SPACE = "baseSpace";
		public const string SPACE_PERCENTAGE = "baseSpacePercentage";
		public const string DESCRIPTION = "description";
		#endregion

		private TechnologyField techField;
		private string name;
		private Dictionary<string, object> values;

		public int ResearchPoints { get; private set; }
		public int TechLevel { get; private set; }
		public int FudgeFactor { get; private set; }
		public string Name { get { return name; } }
		public TechType TechType { get; private set; }
		public bool IsRequired { get; private set; }
		public bool IsPassive { get; private set; }
		public TechnologyScript Script { get; private set; }
		public Dictionary<string, object> AttributeValues { get { return values; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="technologyToLoad">The base technology</param>
		/// <param name="technologyType">Data on a particular type of technology, i.e. weapon or armor</param>
		public TechnologyItem(XElement technologyToLoad, TechnologyField techField, string techScriptPath)
		{
			values = new Dictionary<string, object>();
			//Load the attributes
			foreach (XAttribute attribute in technologyToLoad.Attributes())
			{
				KeyValuePair<string, object> newValue = ConvertElementToObject(attribute.Name.LocalName, attribute.Value);
				values.Add(newValue.Key, newValue.Value);
			}
			name = (string)values["name"];
			this.techField = techField;
			TechLevel = (int)values["techLevel"];
			FudgeFactor = (int)values["fudge"];
			ResearchPoints = (int)values["researchPoints"];
			IsRequired = values.ContainsKey("isRequired") ? (bool)values["isRequired"] : false;
			IsPassive = values.ContainsKey("isPassive") ? (bool)values["isPassive"] : false;

			if (values.ContainsKey("script"))
			{
				Script = new TechnologyScript(new FileInfo(Path.Combine(techScriptPath, (string)values["script"] + ".cs")));
			}
			
			switch((string)values["techType"])
			{
				case "Infrastructure": TechType = TechType.INFRASTRUCTURE;
					break;
				case "Computer": TechType = TechType.COMPUTER;
					break;
				case "ComputerModifier": TechType = TechType.COMPUTER_MODIFICATION;
					break;
				case "ComputerMount": TechType = TechType.COMPUTER_MOUNT;
					break;
				case "StellarEngine": TechType = TechType.STELLAR_ENGINE;
					break;
				case "StellarEngineModifier": TechType = TechType.STELLAR_MODIFICATION;
					break;
				case "StellarEngineMount": TechType = TechType.STELLAR_ENGINE_MOUNT;
					break;
				case "SystemEngine": TechType = TechType.SYSTEM_ENGINE;
					break;
				case "SystemEngineModifier": TechType = TechType.SYSTEM_MODIFICATION;
					break;
				case "SystemEngineMount": TechType = TechType.SYSTEM_ENGINE_MOUNT;
					break;
				case "Shield": TechType = TechType.SHIELD;
					break;
				case "ShieldModifier": TechType = TechType.SHIELD_MODIFICATION;
					break;
				case "ShieldMount": TechType = TechType.SHIELD_MOUNT;
					break;
				case "Armor": TechType = TechType.ARMOR;
					break;
				case "ArmorModifier": TechType = TechType.ARMOR_MODIFICATION;
					break;
				case "ArmorPlating": TechType = TechType.ARMOR_PLATING;
					break;
				case "Beam": TechType = TechType.BEAM;
					break;
				case "BeamMount": TechType = TechType.BEAM_MOUNT;
					break;
				case "BeamModifier": TechType = TechType.BEAM_MODIFICATION;
					break;
				case "Projectile": TechType = TechType.PROJECTILE;
					break;
				case "ProjectileMount": TechType = TechType.PROJECTILE_MOUNT;
					break;
				case "ProjectileModifier": TechType = TechType.PROJECTILE_MODIFICATION;
					break;
				case "MissileWarhead": TechType = TechType.MISSILE_WARHEAD;
					break;
				case "MissileBody": TechType = TechType.MISSILE_BODY;
					break;
				case "MissileModifier": TechType = TechType.MISSILE_MODIFICATION;
					break;
				case "Torpedo": TechType = TechType.TORPEDO;
					break;
				case "TorpedoLauncher": TechType = TechType.TORPEDO_LAUNCHER;
					break;
				case "TorpedoModifier": TechType = TechType.TORPEDO_MODIFICATION;
					break;
				case "Bomb": TechType = TechType.BOMB;
					break;
				case "BombBody": TechType = TechType.BOMB_BODY;
					break;
				case "BombMod": TechType = TechType.BOMB_MODIFICATION;
					break;
				case "Shockwave": TechType = TechType.SHOCKWAVE;
					break;
				case "ShockwaveEmitter": TechType = TechType.SHOCKWAVE_EMITTER;
					break;
				case "ShockwaveModifier": TechType = TechType.SHOCKWAVE_MODIFICATION;
					break;
				case "Reactor": TechType = TechType.REACTOR;
					break;
				case "ReactorModifier": TechType = TechType.REACTOR_MODIFICATION;
					break;
				case "ReactorMount": TechType = TechType.REACTOR_MOUNT;
					break;
				case "Special": TechType = TechType.SPECIAL;
					break;
				default: throw new Exception("Invalid Technology Type: " + values["TechType"]);
			}
		}
		private KeyValuePair<string, object> ConvertElementToObject(string name, string value)
		{
			switch (name)
			{
				case "techType":
				case "description":
				case "colonizes":
				case "particle":
				case "name": return new KeyValuePair<string, object>(name, value);
				/*case "baseSpeed":
				case "baseSpeedMultipler":
				case "baseSpacePercentage":
				case "baseCostPercentage":
				case "basePowerPercentage":
				case "baseHPPercentage":
				case "baseAccuracyMultiplier":
				case "baseSpaceMultiplier":
				case "baseCostMultiplier":
				case "basePowerMultiplier":
				case "baseTimeMultipler":
				case "baseDamageMultiplier":
				case "range":
				case "rangeMultiplier": return new KeyValuePair<string, object>(name, float.Parse(value));*/
				case "transferCapacity":
				case "researchPoints":
				case "fudge":
				case "techLevel": return new KeyValuePair<string, object>(name, int.Parse(value));
				case "isPassive":
				case "isRequired": return new KeyValuePair<string, object>(name, bool.Parse(value));
				/*case "baseSpace":
				case "baseCost":
				case "basePower":
				case "baseTime": return new KeyValuePair<string,object>(name, long.Parse(value));*/
				default: return new KeyValuePair<string, object>(name, value);
			}
		}
		/*public string GetAttributeValue(string attribute)
		{
			if (values.ContainsKey(attribute))
			{
				return values[attribute];
			}
			return null;
		}
		public bool AttributeExists(string attribute)
		{
			return values.ContainsKey(attribute);
		}*/
		/*public long GetSpace(int size)
		{
			if (values.ContainsKey(SPACE))
			{
				return (long)values[SPACE];
			}
			else
			{
				double value = (double)values[SPACE_PERCENTAGE] * 0.01 * size;
				return (long)(value + (value - ((long)value) > 0 ? 1 : 0));
			}
		}
		public long GetPower(int size)
		{
			if (values.ContainsKey(POWER))
			{
				return (long)values[POWER];
			}
			else
			{
				double value = (double)values[POWER_PERCENTAGE] * 0.01 * size;
				return (long)(value + (value - ((long)value) > 0 ? 1 : 0));
			}
		}
		public long GetCost(int size)
		{
			if (values.ContainsKey(COST))
			{
				return (long)values[COST];
			}
			else
			{
				double value = (double)values[COST_PERCENTAGE] * 0.01 * size;
				return (long)(value + (value - ((long)value) > 0 ? 1 : 0));
			}
		}
		public int GetTime(int size)
		{
			if (values.ContainsKey(TIME))
			{
				return (int)values[TIME];
			}
			else if (values.ContainsKey(TIME_PERCENTAGE))
			{
				double value = (double)values[TIME_PERCENTAGE] * 0.01 * size;
				return (int)(value + (value - (int)value) > 0 ? 1 : 0);
			}
			return 0; //No time defined, default to 0
		}*/
		#region Description
		public string GetDescription()
		{
			return (string)values[DESCRIPTION];
		}
		#endregion
	}

	public enum EquipmentType { BEAM, PROJECTILE, MISSILE, TORPEDO, BOMB, SHOCKWAVE, COMPUTER, STELLAR_ENGINE, SYSTEM_ENGINE, SHIELD, ARMOR, REACTOR, SPECIAL }
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
			Values.Add("Passive", IsPassive());
			//Values.Add("Cost", GetCost(shipSize));
			//Values.Add("Power", GetPower(shipSize));
			//Values.Add("Time", GetTime(shipSize));
			Values.Add("Name", GetName());
			//Values.Add("GalaxySpeed", GetSpeed());
			Values.Add("SelectionSize", GetSelectionSize(shipSize));
		}
	}

	public class Equipment
	{
		public TechnologyItem ItemType { get; set; }
		public TechnologyItem MountType { get; set; }
		public List<TechnologyItem> ModifierItems { get; set; }

		public List<Icon> CombatIcons { get; private set; } //For combat view and other views
		public List<Icon> DesignIcons { get; private set; } //For ship design view
		public List<Icon> DesignControls { get; private set; } //For ship design view that requires user to input values (such as number of mounts, amount of ammo, etc)

		public Dictionary<string, object> ModifiableValues { get; set; } //This is primarily for icons, so icons can apply changes

		public EquipmentType EquipmentType { get; private set; }

		public Equipment(EquipmentType type)
		{
			ItemType = null;
			MountType = null;
			ModifierItems = new List<TechnologyItem>();
			ModifiableValues = new Dictionary<string, object>();
			CombatIcons = new List<Icon>();
			DesignIcons = new List<Icon>();
			DesignControls = new List<Icon>();
		}
		public Equipment(EquipmentType type, TechnologyItem mainItem, TechnologyItem secondaryItem, List<TechnologyItem> modifierItems, Dictionary<string, object> modifiableValues, IconManager iconManager)
		{
			this.EquipmentType = type;
			ItemType = mainItem;
			MountType = secondaryItem;
			ModifierItems = new List<TechnologyItem>();
			ModifiableValues = new Dictionary<string, object>();
			foreach (TechnologyItem item in modifierItems)
			{
				ModifierItems.Add(item);
			}
			foreach (KeyValuePair<string, object> keyValuePair in modifiableValues)
			{
				ModifiableValues.Add(keyValuePair.Key, keyValuePair.Value);
			}
			CombatIcons = new List<Icon>();
			if (mainItem.AttributeValues.ContainsKey("combatIcons"))
			{
				string[] icons = ((string)mainItem.AttributeValues["combatIcons"]).Split(new[] { '|' });
				foreach (string icon in icons)
				{
					CombatIcons.Add(iconManager.GetIcon(icon));
				}
			}
			DesignIcons = new List<Icon>();
			if (mainItem.AttributeValues.ContainsKey("designIcons"))
			{
				string[] icons = ((string)mainItem.AttributeValues["designIcons"]).Split(new[] { '|' });
				foreach (string icon in icons)
				{
					DesignIcons.Add(iconManager.GetIcon(icon));
				}
			}
			DesignControls = new List<Icon>();
			if (mainItem.AttributeValues.ContainsKey("designControls"))
			{
				string[] icons = ((string)mainItem.AttributeValues["designControls"]).Split(new[] { '|' });
				foreach (string icon in icons)
				{
					DesignControls.Add(iconManager.GetIcon(icon));
				}
			}
		}

		public Equipment(Equipment equipmentToCopy)
		{
			EquipmentType = equipmentToCopy.EquipmentType;
			ItemType = equipmentToCopy.ItemType;
			MountType = equipmentToCopy.MountType;
			ModifiableValues = new Dictionary<string, object>(equipmentToCopy.ModifiableValues);
			ModifierItems = new List<TechnologyItem>();
			foreach (TechnologyItem item in equipmentToCopy.ModifierItems)
			{
				ModifierItems.Add(item);
			}
			CombatIcons = new List<Icon>(equipmentToCopy.CombatIcons);
			DesignIcons = new List<Icon>(equipmentToCopy.DesignIcons);
			DesignControls = new List<Icon>(equipmentToCopy.DesignControls);
		}

		public bool IsThisEquipmentTheSame(Equipment equipmentToCompare)
		{
			if (ItemType != equipmentToCompare.ItemType)
			{
				return false;
			}
			if (MountType != equipmentToCompare.MountType)
			{
				return false;
			}
			if (ModifierItems.Count != equipmentToCompare.ModifierItems.Count)
			{
				return false;
			}
			for (int i = 0; i < ModifierItems.Count; i++)
			{
				if (ModifierItems[i] != equipmentToCompare.ModifierItems[i])
				{
					return false;
				}
			}
			return true;
		}

		public bool IsPassive()
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
		}

		#region Name
		public string GetName()
		{
			if (ItemType.Script != null)
			{
				Dictionary<string, object> values = GetEquipmentInfo(null);
				if (values.ContainsKey("_name"))
				{
					return (string)values["_name"];
				}
			}
			if (EquipmentType == Beyond_Beyaan.EquipmentType.SPECIAL)
			{
				return ItemType.GetDescription();
			}
			return string.Format(MountType.GetDescription(), ItemType.GetDescription());// + (Count > 1 ? " x " + Count : ""));
		}
		#endregion

		#region Script Functions
		public Dictionary<string, object> GetEquipmentInfo(Dictionary<string, object> shipValues)
		{
			if (ItemType.Script != null)
			{
				Dictionary<string, object>[] modifierValues = new Dictionary<string, object>[ModifierItems.Count];
				for (int i = 0; i < modifierValues.Length; i++)
				{
					modifierValues[i] = new Dictionary<string, object>(ModifierItems[i].AttributeValues);
				}
				if (EquipmentType != Beyond_Beyaan.EquipmentType.SPECIAL)
				{
					return ItemType.Script.GetEquipmentInfo(ItemType.AttributeValues, MountType.AttributeValues, modifierValues, shipValues, ModifiableValues);
				}
				else
				{
					return ItemType.Script.GetEquipmentInfo(ItemType.AttributeValues, null, null, shipValues, ModifiableValues);
				}
			}
			return null;
		}
		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues)
		{
			Dictionary<string, object> values = GetEquipmentInfo(shipValues);
			if (values != null)
			{
				return ItemType.Script.UpdateShipInfo(shipValues, values);
			}
			return shipValues;
		}
		
		public int GetSelectionSize(int shipSize)
		{
			if (ItemType.Script != null)
			{
				return ItemType.Script.GetTargetReticle(shipSize);
			}
			return -1;
		}
		public Dictionary<string, object>[] Activate(Point gridcell, Point shipPos, Point boundary, Dictionary<string, object> equipmentValues)
		{
			if (ItemType.Script != null)
			{
				return ItemType.Script.Activate(shipPos.X, shipPos.Y, gridcell.X, gridcell.Y, boundary.X, boundary.Y, equipmentValues);
			}
			return null;
		}
		public Dictionary<string, object>[] OnHit(Point impact, Point shipPos, int shipSize, Dictionary<string, object> equipmentValues, Dictionary<string, object> particleValues)
		{
			if (ItemType.Script != null)
			{
				return ItemType.Script.OnHit(impact.X, impact.Y, shipPos.X, shipPos.Y, shipSize, equipmentValues, particleValues);
			}
			return null;
		}
		#endregion
	}
}
