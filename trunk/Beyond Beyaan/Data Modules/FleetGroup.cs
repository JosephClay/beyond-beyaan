using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	public class FleetGroup
	{
		#region Member Variables
		private List<Fleet> fleets;
		private List<Ship> ships; //Used for UI component, since Fleet stores a dictionary of ships, which means no iterator

		private Fleet selectedFleet;
		private Fleet fleetToSplit;

		private int fleetIndex;
		private int shipIndex;
		#endregion

		#region Properties
		public Fleet SelectedFleet
		{
			get { return selectedFleet; }
		}
		public Fleet FleetToSplit
		{
			get { return fleetToSplit; }
		}

		public List<Fleet> Fleets
		{
			get { return fleets; }
		}

		public int FleetIndex
		{
			get { return fleetIndex; }
			set { fleetIndex = value; }
		}

		public int ShipIndex
		{
			get { return shipIndex; }
			set 
			{ 
				shipIndex = value;
				if (fleetToSplit.Ships.Count > 8)
				{
					if (shipIndex > fleetToSplit.Ships.Count - 8)
					{
						shipIndex = fleetToSplit.Ships.Count - 8;
					}
				}
				else
				{
					shipIndex = 0;
				}
				int maxShip = fleetToSplit.Ships.Count > 8 ? 8 : fleetToSplit.Ships.Count;

				ships = new List<Ship>();
				int i = 0;
				foreach (KeyValuePair<Ship, int> ship in fleetToSplit.Ships)
				{
					if (i >= shipIndex && i < shipIndex + maxShip)
					{
						ships.Add(ship.Key);
					}
					i++;
					if (i >= shipIndex + maxShip)
					{
						break;
					}
				}
			}
		}
		#endregion

		#region Constructors
		public FleetGroup(List<Fleet> fleets)
		{
			this.fleets = fleets;
			if (fleets != null && fleets.Count > 0)
			{
				SelectFleet(0);
			}
		}
		#endregion

		#region Functions
		public void SelectFleet(int whichFleet)
		{
			if (whichFleet < fleets.Count)
			{
				selectedFleet = fleets[whichFleet];

				fleetToSplit = new Fleet();
				fleetToSplit.Empire = selectedFleet.Empire;
				fleetToSplit.TravelNodes = selectedFleet.TravelNodes;
				fleetToSplit.TentativeNodes = selectedFleet.TentativeNodes;
				fleetToSplit.AdjacentSystem = selectedFleet.AdjacentSystem;
				fleetToSplit.GalaxyX = selectedFleet.GalaxyX;
				fleetToSplit.GalaxyY = selectedFleet.GalaxyY;
				foreach (KeyValuePair<Ship, int> ship in selectedFleet.Ships)
				{
					fleetToSplit.AddShips(ship.Key, ship.Value);
				}
				ShipIndex = 0;
			}
		}

		public void SplitFleet(Empire empire)
		{
			Fleet fleet = new Fleet();
			fleet.Empire = fleetToSplit.Empire;
			fleet.GalaxyX = fleetToSplit.GalaxyX;
			fleet.GalaxyY = fleetToSplit.GalaxyY;
			fleet.TravelNodes = fleetToSplit.TravelNodes;
			fleet.AdjacentSystem = fleetToSplit.AdjacentSystem;

			foreach (KeyValuePair<Ship, int> ship in fleetToSplit.Ships)
			{
				if (ship.Value > 0)
				{
					selectedFleet.SubtractShips(ship.Key, ship.Value);
					fleet.AddShips(ship.Key, ship.Value);
				}
			}
			selectedFleet.ClearEmptyShips();
			fleet.ClearEmptyShips();
			if (selectedFleet.Ships.Count == 0)
			{
				fleets.Remove(selectedFleet);
				empire.FleetManager.RemoveFleet(selectedFleet);
			}
			if (fleet.Ships.Count > 0)
			{
				fleets.Add(fleet);
				empire.FleetManager.AddFleet(fleet);
			}
		}

		public List<Ship> GetShipsForDisplay()
		{
			return ships;
		}
		#endregion
	}
}
