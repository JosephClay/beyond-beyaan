using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public class ShipDesign
	{
		#region Properties
		public string Name { get; set; }
		public Ship ShipClass { get; set; }
		public int WhichStyle { get; set; }
		public List<Equipment> Equipments { get; private set; }
		public Race Race { get; set; }

		public Dictionary<Resource, float> DevelopmentCost { get; set; }
		public Dictionary<Resource, float> Maintenance { get; set; }

		#endregion

		#region Constructors
		public ShipDesign()
		{
			Equipments = new List<Equipment>();
		}
		public ShipDesign(ShipDesign shipToCopy)
		{
			Race = shipToCopy.Race;
			Name = shipToCopy.Name;
			ShipClass = shipToCopy.ShipClass;
			WhichStyle = shipToCopy.WhichStyle;
			Equipments = new List<Equipment>();
			foreach (Equipment equipment in shipToCopy.Equipments)
			{
				Equipments.Add(new Equipment(equipment));
			}
		}
		public bool IsThisShipTheSame(ShipDesign shipToCompare)
		{
			if (Name != shipToCompare.Name)
			{
				return false;
			}
			if (ShipClass != shipToCompare.ShipClass)
			{
				return false;
			}
			if (WhichStyle != shipToCompare.WhichStyle)
			{
				return false;
			}
			if (Race != shipToCompare.Race)
			{
				return false;
			}
			if (Equipments.Count != shipToCompare.Equipments.Count)
			{
				return false;
			}
			for (int i = 0; i < Equipments.Count; i++)
			{
				if (!Equipments[i].IsThisEquipmentTheSame(shipToCompare.Equipments[i]))
				{
					return false;
				}
			}
			return true;
		}
		public Dictionary<string, object> GetBasicValues()
		{
			Dictionary<string, object> shipValues = ShipClass.ShipScript.Initialize(ShipClass.Values);

			foreach (Equipment equipment in Equipments)
			{
				shipValues = equipment.UpdateShipInfo(shipValues);
			}

			return ShipClass.ShipScript.GetInformation(shipValues);
		}
		#endregion
	}
}
