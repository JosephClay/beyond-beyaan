using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class FleetManager
	{
		#region Variables

		private List<Squadron> fleets;
		private Empire empire;
		#endregion

		#region Properties
		public ShipDesign LastShipDesign { get; set; }

		public List<ShipDesign> ShipDesigns { get; private set; }

		#endregion

		#region Constructor
		public FleetManager(Empire empire)
		{
			this.empire = empire;
			fleets = new List<Squadron>();
			ShipDesigns = new List<ShipDesign>();
		}
		#endregion

		#region Functions
		public List<Squadron> GetFleets()
		{
			return fleets;
		}

		public Squadron[] ReturnFleetAtPoint(int x, int y)
		{
			List<Squadron> listOfFleets = new List<Squadron>();
			foreach (Squadron fleet in fleets)
			{
				if ((fleet.FleetLocation.X - 16 <= x&& fleet.FleetLocation.X + 16 > x) &&
					(fleet.FleetLocation.Y - 16 <= y && fleet.FleetLocation.Y + 16 > y))
				{
					listOfFleets.Add(fleet);
				}
			}
			return listOfFleets.ToArray();
		}

		public void AddFleet(Squadron fleet)
		{
			fleets.Add(fleet);
			//MergeIdleFleets();
		}

		/*public void AddTransportShips(List<TransportShip> transportShips, int x, int y, Empire owner)
		{
			Fleet fleet = new Fleet();
			fleet.GalaxyX = x;
			fleet.GalaxyY = y;
			fleet.Empire = owner;
			foreach (TransportShip ship in transportShips)
			{
				fleet.AddTransport(ship.raceOnShip, ship.amount);
			}
			fleets.Add(fleet);
			MergeIdleFleets();
		}*/

		public void RemoveFleet(Squadron fleet)
		{
			fleets.Remove(fleet);
		}

		public void AddShipDesign(ShipDesign newShipDesign)
		{
			ShipDesign ship = new ShipDesign(newShipDesign); //Make a copy so it don't get modified by accident
			ShipDesigns.Add(ship);
		}

		public bool MoveFleets()
		{
			bool stillHaveMovement = false;
			//This is called during end of turn processing
			foreach (Squadron fleet in fleets)
			{
				if (fleet.Move())
				{
					stillHaveMovement = true;
				}
			}
			return stillHaveMovement;
		}

		public void ResetFleetMovements()
		{
			foreach (Squadron fleet in fleets)
			{
				fleet.ResetMove();
			}
		}

		/*public void MergeIdleFleets()
		{
			for (int i = 0; i < fleets.Count; i++)
			{
				if (fleets[i].TravelNodes == null)
				{
					List<Squadron> fleetsToRemove = new List<Squadron>();
					for (int j = i + 1; j < fleets.Count; j++)
					{
						if (fleets[j].TravelNodes == null && fleets[j].GalaxyX == fleets[i].GalaxyX && fleets[j].GalaxyY == fleets[i].GalaxyY)
						{
							fleets[i].AddShipsThemselves(fleets[j].Ships);
							fleetsToRemove.Add(fleets[j]);
						}
					}
					foreach (Squadron fleet in fleetsToRemove)
					{
						fleets.Remove(fleet);
					}
				}
			}
		}*/

		public void ClearEmptyFleets()
		{
			List<Squadron> fleetsToRemove = new List<Squadron>();
			for (int i = 0; i < fleets.Count; i++)
			{
				if (fleets[i].Ships.Count == 0)
				{
					fleetsToRemove.Add(fleets[i]);
				}
			}
			foreach (Squadron fleet in fleetsToRemove)
			{
				fleets.Remove(fleet);
			}
		}

		public Dictionary<Resource, float> GetExpenses()
		{
			//TODO: Tally up expenses
			/*float amount = 0;
			foreach (Squadron fleet in fleets)
			{
				amount += fleet.GetExpenses();
			}
			return amount;*/
			return new Dictionary<Resource, float>();
		}

		public float ScrapShip(ShipDesign ship)
		{
			//float salvageAmount = ship.Cost * 0.25f;
			//return salvageAmount;
			return 0.0f;
		}

		public void ScrapDesign(ShipDesign ship)
		{
			ShipDesigns.Remove(ship);
			if (LastShipDesign.IsThisShipTheSame(ship) && ShipDesigns.Count > 0)
			{
				//Make a copy so we don't modify the original
				LastShipDesign = new ShipDesign(ShipDesigns[0]);
			}
		}
		#endregion
	}
}
