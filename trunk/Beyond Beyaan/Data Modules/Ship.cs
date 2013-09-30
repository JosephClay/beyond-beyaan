using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Ship
	{
		public const int SMALL = 0;
		public const int MEDIUM = 1;
		public const int LARGE = 2;
		public const int HUGE = 3;

		#region Properties
		public string Name { get; set; }
		public int DesignID { get; set; }
		public Empire Owner { get; set; }
		public int Size { get; set; }
		public int WhichStyle { get; set; }
		public KeyValuePair<Equipment, float> Engine;
		public int ManeuverSpeed = 1;
		public Equipment Shield;
		public Equipment Armor;
		public Equipment Computer;
		public Equipment ECM;
		public List<KeyValuePair<Equipment, int>> Weapons;
		public List<Equipment> Specials;
		public float Maintenance { get { return Cost * 0.02f; } }
		public int TotalSpace 
		{ 
			get 
			{
				int space = 40;
				switch (Size)
				{
					case MEDIUM: 
						space = 200;
						break;
					case LARGE:
						space = 1000;
						break;
					case HUGE:
						space = 5000;
						break;
				}
				space = (int)(space * (1.0 + (0.02 * Owner.TechnologyManager.ConstructionLevel)));
				return space;
			} 
		}
		public float Cost
		{
			get
			{
				float cost = 6;
				switch (Size)
				{
					case MEDIUM:
						cost = 36;
						break;
					case LARGE:
						cost = 200;
						break;
					case HUGE:
						cost = 1200;
						break;
				}
				var fieldLevels = Owner.TechnologyManager.GetFieldLevels();
				cost += Engine.Key.GetCost(fieldLevels, Size) * Engine.Value;
				cost += Armor.GetCost(fieldLevels, Size);
				if (Shield != null)
				{
					cost += Shield.GetCost(fieldLevels, Size);
				}
				if (Computer != null)
				{
					cost += Computer.GetCost(fieldLevels, Size);
				}
				if (ECM != null)
				{
					cost += ECM.GetCost(fieldLevels, Size);
				}
				foreach (var weapon in Weapons)
				{
					//Weapon times amount of mounts
					cost += weapon.Key.GetCost(fieldLevels, Size) * weapon.Value;
				}
				foreach (var special in Specials)
				{
					cost += special.GetCost(fieldLevels, Size);
				}
				return cost;
			}
		}
		public float SpaceUsed
		{
			get
			{
				float sizeUsed = 0;
				var fieldLevels = Owner.TechnologyManager.GetFieldLevels();
				sizeUsed += Engine.Key.GetSize(fieldLevels, Size) * Engine.Value;
				sizeUsed += Armor.GetSize(fieldLevels, Size);
				if (Shield != null)
				{
					sizeUsed += Shield.GetSize(fieldLevels, Size);
				}
				if (Computer != null)
				{
					sizeUsed += Computer.GetSize(fieldLevels, Size);
				}
				if (ECM != null)
				{
					sizeUsed += ECM.GetSize(fieldLevels, Size);
				}
				foreach (var weapon in Weapons)
				{
					//Weapon times amount of mounts
					sizeUsed += weapon.Key.GetSize(fieldLevels, Size) * weapon.Value;
				}
				foreach (var special in Specials)
				{
					sizeUsed += special.GetSize(fieldLevels, Size);
				}
				return sizeUsed;
			}
		}
		public float PowerUsed
		{
			get
			{
				float powerUsed = 0;
				//First, get the maneuver power requirement
				switch (Size)
				{
					case SMALL:
						powerUsed += ManeuverSpeed * 2;
						break;
					case MEDIUM:
						powerUsed += ManeuverSpeed * 15;
						break;
					case LARGE:
						powerUsed += ManeuverSpeed * 100;
						break;
					case HUGE:
						powerUsed += ManeuverSpeed * 700;
						break;
				}
				//since engines provide power, do NOT include engines in this total
				//Armor don't use up power, but perhaps in a mod?  For now, don't include armor as well
				if (Computer != null)
				{
					powerUsed += Computer.GetPower(Size);
				}
				if (ECM != null)
				{
					powerUsed += ECM.GetPower(Size);
				}
				if (Shield != null)
				{
					powerUsed += Shield.GetPower(Size);
				}
				foreach (var weapon in Weapons)
				{
					powerUsed += weapon.Key.GetPower(Size) * weapon.Value;
				}
				foreach (var special in Specials)
				{
					powerUsed += special.GetPower(Size);
				}
				return powerUsed;
			}
		}
		public int GalaxySpeed
		{
			get { return Engine.Key.Technology.Speed; }
		}
		public int DefenseRating
		{
			get { return (2 - Size) + (ManeuverSpeed - 1); }
		}
		#endregion

		#region Constructors
		public Ship()
		{
			Weapons = new List<KeyValuePair<Equipment, int>>();
			Specials = new List<Equipment>();
		}
		public Ship(Ship shipToCopy)
		{
			Name = shipToCopy.Name;
			DesignID = shipToCopy.DesignID;
			Owner = shipToCopy.Owner;
			Size = shipToCopy.Size;
			WhichStyle = shipToCopy.WhichStyle;
			Engine = new KeyValuePair<Equipment, float>(shipToCopy.Engine.Key, shipToCopy.Engine.Value);
			Shield = shipToCopy.Shield;
			Armor = shipToCopy.Armor;
			Computer = shipToCopy.Computer;
			ECM = shipToCopy.ECM;
			Weapons = new List<KeyValuePair<Equipment, int>>(shipToCopy.Weapons);
			Specials = new List<Equipment>(shipToCopy.Specials);
		}
		#endregion

		#region Save/Load
		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("ShipDesign");
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("DesignID", DesignID.ToString());
			writer.WriteAttributeString("Size", Size.ToString());
			writer.WriteAttributeString("WhichStyle", WhichStyle.ToString());
			writer.WriteAttributeString("Engine", Engine.Key.EquipmentName);
			writer.WriteAttributeString("NumOfEngines", Engine.Value.ToString());
			writer.WriteAttributeString("Armor", Armor.EquipmentName);
			writer.WriteAttributeString("Shield", Shield == null ? "" : Shield.EquipmentName);
			writer.WriteAttributeString("Computer", Computer == null ? "" : Computer.EquipmentName);
			writer.WriteAttributeString("ECM", ECM == null ? "" : ECM.EquipmentName);
			foreach (var weapon in Weapons)
			{
				writer.WriteStartElement("Weapon");
				writer.WriteAttributeString("Name", weapon.Key.EquipmentName);
				writer.WriteAttributeString("NumOfMounts", weapon.Value.ToString());
				writer.WriteEndElement();
			}
			foreach (var special in Specials)
			{
				writer.WriteStartElement("Special");
				writer.WriteAttributeString("Name", special.EquipmentName);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public void Load(XElement shipDesign, GameMain gameMain)
		{
			Name = shipDesign.Attribute("Name").Value;
			DesignID = int.Parse(shipDesign.Attribute("DesignID").Value);
			Size = int.Parse(shipDesign.Attribute("Size").Value);
			WhichStyle = int.Parse(shipDesign.Attribute("WhichStyle").Value);
			Engine = new KeyValuePair<Equipment, float>(LoadEquipment(shipDesign.Attribute("Engine").Value, gameMain), float.Parse(shipDesign.Attribute("NumOfEngines").Value));
			Armor = LoadEquipment(shipDesign.Attribute("Armor").Value, gameMain);
			Shield = LoadEquipment(shipDesign.Attribute("Shield").Value, gameMain);
			Computer = LoadEquipment(shipDesign.Attribute("Computer").Value, gameMain);
			ECM = LoadEquipment(shipDesign.Attribute("ECM").Value, gameMain);
			foreach (var weapon in shipDesign.Elements("Weapon"))
			{
				var weaponTech = LoadEquipment(weapon.Attribute("Name").Value, gameMain);
				Weapons.Add(new KeyValuePair<Equipment, int>(weaponTech, int.Parse(weapon.Attribute("NumOfMounts").Value)));
			}
			foreach (var special in shipDesign.Elements("Special"))
			{
				var specialTech = LoadEquipment(special.Attribute("Name").Value, gameMain);
				Specials.Add(specialTech);
			}
		}
		private static Equipment LoadEquipment(string equipmentName, GameMain gameMain)
		{
			bool useSecondary = equipmentName.EndsWith("|Sec");
			string actualTechName = useSecondary ? equipmentName.Substring(0, equipmentName.Length - 4) : equipmentName;
			return new Equipment(gameMain.MasterTechnologyManager.GetTechnologyWithName(actualTechName), useSecondary);
		}
		#endregion
	}

	public class TransportShip
	{
		public Race raceOnShip;
		public int amount;
	}
}
