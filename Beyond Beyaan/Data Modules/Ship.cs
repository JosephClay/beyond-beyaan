using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public Empire Owner { get; set; }
		public int Size { get; set; }
		public int WhichStyle { get; set; }
		public Technology Engine;
		public Technology Shield;
		public Technology Armor;
		public Technology Computer;
		public Technology ECM;
		public List<Technology> Weapons;
		public List<Technology> Specials;
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
				//Todo: Add 2% space for each level in construction tech
				//space *= (1.0f + (0.02 * ConstructionLevel));
				return space;
			} 
		}
		public int Cost
		{
			get
			{
				int cost = 6;
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
				/*if (engine != null)
				{
					cost += engine.GetCost(TotalSpace);
				}
				if (armor != null)
				{
					cost += armor.GetCost(TotalSpace);
				}
				if (shield != null)
				{
					cost += shield.GetCost(TotalSpace);
				}
				if (computer != null)
				{
					cost += computer.GetCost(TotalSpace);
				}
				foreach (var weapon in weapons)
				{
					cost += weapon.GetCost();
				}
				foreach (var special in specials)
				{
					cost += special.GetCost(TotalSpace);
				}*/
				return cost;
			}
		}
		#endregion

		#region Constructors
		public Ship()
		{
			Weapons = new List<Technology>();
			Specials = new List<Technology>();
		}
		public Ship(Ship shipToCopy)
		{
			Name = shipToCopy.Name;
			Owner = shipToCopy.Owner;
			Size = shipToCopy.Size;
			WhichStyle = shipToCopy.WhichStyle;
			Engine = shipToCopy.Engine;
			Shield = shipToCopy.Shield;
			Armor = shipToCopy.Armor;
			Computer = shipToCopy.Computer;
			ECM = shipToCopy.ECM;
			Weapons = new List<Technology>(shipToCopy.Weapons);
			Specials = new List<Technology>(shipToCopy.Specials);
		}
		#endregion
	}

	public class TransportShip
	{
		public Race raceOnShip;
		public int amount;
	}
}
