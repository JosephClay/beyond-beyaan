using System.Collections.Generic;

namespace Beyond_Beyaan
{
	public class FleetManager
	{
		#region Variables
		private List<Ship> currentShipDesigns;
		private List<Ship> obsoleteShipDesigns;
		private List<Fleet> fleets;
		private Empire empire;
		#endregion

		#region Properties
		public Ship LastShipDesign { get; set; }
		
		public List<Ship> CurrentDesigns 
		{ 
			get { return currentShipDesigns; } 
		}
		
		public List<Ship> ObsoleteDesigns
		{
			get { return obsoleteShipDesigns; }
		}
		#endregion

		public FleetManager(Empire empire)
		{
			this.empire = empire;
			fleets = new List<Fleet>();
			currentShipDesigns = new List<Ship>();
			obsoleteShipDesigns = new List<Ship>();

			Technology retroEngine = null;
			Technology titaniumArmor = null;
			foreach (var tech in empire.TechnologyManager.ResearchedPropulsionTechs)
			{
				if (tech.Speed == 1)
				{
					retroEngine = tech;
					break;
				}
			}
			foreach (var tech in empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.Armor == Technology.TITANIUM_ARMOR)
				{
					titaniumArmor = tech;
					break;
				}
			}

			Ship scout = new Ship();
			scout.Name = "Scout";
			scout.Owner = empire;
			scout.Size = Ship.SMALL;
			scout.WhichStyle = 0;
			scout.Engine = retroEngine;
			scout.Armor = titaniumArmor;
			foreach (var tech in empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.ReserveFuelTanks)
				{
					scout.Specials.Add(tech);
					break;
				}
			}
			currentShipDesigns.Add(scout);

			Ship colonyShip = new Ship();
			colonyShip.Name = "Colony Ship";
			colonyShip.Owner = empire;
			colonyShip.Size = Ship.LARGE;
			colonyShip.WhichStyle = 0;
			colonyShip.Engine = retroEngine;
			colonyShip.Armor = titaniumArmor;
			foreach (var tech in empire.TechnologyManager.ResearchedPlanetologyTechs)
			{
				if (tech.Colony == Technology.STANDARD_COLONY)
				{
					colonyShip.Specials.Add(tech);
					break;
				}
			}
			currentShipDesigns.Add(colonyShip);

			LastShipDesign = new Ship(scout); //Make a copy so we don't accidentally modify the original ship
		}

		public void SetupStarterFleet(StarSystem homeSystem)
		{
			Fleet starterFleet = new Fleet();
			starterFleet.GalaxyX = homeSystem.X;
			starterFleet.GalaxyY = homeSystem.Y;
			starterFleet.AdjacentSystem = homeSystem;
			starterFleet.Empire = empire;
			starterFleet.AddShips(currentShipDesigns[0], 2);
			starterFleet.AddShips(currentShipDesigns[1], 1);
			fleets.Add(starterFleet);
		}

		public List<Fleet> GetFleets()
		{
			return fleets;
		}

		public Fleet[] ReturnFleetAtPoint(int x, int y)
		{
			List<Fleet> listOfFleets = new List<Fleet>();
			foreach (Fleet fleet in fleets)
			{
				if (fleet.AdjacentSystem != null)
				{
					if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
					{
						if (x >= fleet.AdjacentSystem.X - 48 && x < fleet.AdjacentSystem.X - 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
					else
					{
						if (x >= fleet.AdjacentSystem.X + 16 && x < fleet.AdjacentSystem.X + 48 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
				}
				else
				{
					if (x >= fleet.GalaxyX - 16 && x < fleet.GalaxyX + 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
					{
						listOfFleets.Add(fleet);
					}
				}
			}
			return listOfFleets.ToArray();
		}

		public void AddFleet(Fleet fleet)
		{
			fleets.Add(fleet);
		}

		public void RemoveFleet(Fleet fleet)
		{
			fleets.Remove(fleet);
		}

		public void AddShipDesign(Ship newShipDesign)
		{
			Ship ship = new Ship(newShipDesign); //Make a copy so it don't get modified by accident
			currentShipDesigns.Add(ship);
		}

		public void ObsoleteShipDesign(Ship shipToObsolete)
		{
			currentShipDesigns.Remove(shipToObsolete);
			obsoleteShipDesigns.Add(shipToObsolete);
		}

		public bool MoveFleets(float frameDeltaTime)
		{
			bool stillHaveMovement = false;
			//This is called during end of turn processing
			foreach (Fleet fleet in fleets)
			{
				if (fleet.Move(frameDeltaTime))
				{
					stillHaveMovement = true;
				}
			}
			return stillHaveMovement;
		}

		public void ResetFleetMovements()
		{
			foreach (Fleet fleet in fleets)
			{
				fleet.ResetMove();
			}
		}

		public void MergeIdleFleets()
		{
			for (int i = 0; i < fleets.Count; i++)
			{
				if (fleets[i].TravelNodes == null)
				{
					List<Fleet> fleetsToRemove = new List<Fleet>();
					for (int j = i + 1; j < fleets.Count; j++)
					{
						if (fleets[j].TravelNodes == null && fleets[j].AdjacentSystem == fleets[i].AdjacentSystem)
						{
							//Merge only fleets of the same type (i.e. ships with ships, transports with transports
							if (fleets[j].Ships.Count > 0 && fleets[i].Ships.Count > 0)
							{
								foreach (KeyValuePair<Ship, int> ship in fleets[j].Ships)
								{
									fleets[i].AddShips(ship.Key, ship.Value);
								}
								fleetsToRemove.Add(fleets[j]);
							}
							else if (fleets[j].TransportShips.Count > 0 && fleets[i].TransportShips.Count > 0)
							{
								foreach (TransportShip ship in fleets[j].TransportShips)
								{
									fleets[i].AddTransport(ship.raceOnShip, ship.amount);
								}
								fleetsToRemove.Add(fleets[j]);
							}
						}
					}
					foreach (Fleet fleet in fleetsToRemove)
					{
						fleets.Remove(fleet);
					}
				}
			}
		}
		public void ClearEmptyFleets()
		{
			//Clear out any empty fleets left after colonizing, space combat, etc
			List<Fleet> fleetsToRemove = new List<Fleet>();
			for (int i = 0; i < fleets.Count; i++)
			{
				if (fleets[i].Ships.Count == 0 && fleets[i].TransportShips.Count == 0)
				{
					fleetsToRemove.Add(fleets[i]);
				}
			}
			foreach (var fleet in fleetsToRemove)
			{
				fleets.Remove(fleet);
			}
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (Fleet fleet in fleets)
			{
				amount += fleet.GetExpenses();
			}
			return amount;
		}
	}
}
