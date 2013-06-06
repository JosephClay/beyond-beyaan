using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	class FleetManager
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

			Ship scout = new Ship();
			scout.Name = "Scout";
			scout.Size = 1;
			scout.WhichStyle = 0;
			scout.engine = empire.TechnologyManager.VisibleEngines[0];
			scout.armor = empire.TechnologyManager.VisibleArmors[0];
			scout.computer = empire.TechnologyManager.VisibleComputers[0];
			scout.shield = empire.TechnologyManager.VisibleShields[0];
			scout.weapons.Add(new Weapon(empire.TechnologyManager.VisibleBeams[0]));
			scout.Cost = empire.TechnologyManager.VisibleEngines[0].GetCost(40) + empire.TechnologyManager.VisibleArmors[0].GetCost(40) + empire.TechnologyManager.VisibleComputers[0].GetCost(40) +
				empire.TechnologyManager.VisibleShields[0].GetCost(40) + empire.TechnologyManager.VisibleBeams[0].GetCost();
			currentShipDesigns.Add(scout);

			Ship scout2 = new Ship();
			scout2.Name = "Scout2";
			scout2.Size = 2;
			scout2.WhichStyle = 3;
			scout2.engine = empire.TechnologyManager.VisibleEngines[0];
			scout2.armor = empire.TechnologyManager.VisibleArmors[0];
			scout2.computer = empire.TechnologyManager.VisibleComputers[0];
			scout2.shield = empire.TechnologyManager.VisibleShields[0];
			scout2.weapons.Add(new Weapon(empire.TechnologyManager.VisibleBeams[0]));
			scout2.Cost = empire.TechnologyManager.VisibleEngines[0].GetCost(40) + empire.TechnologyManager.VisibleArmors[0].GetCost(40) + empire.TechnologyManager.VisibleComputers[0].GetCost(40) +
				empire.TechnologyManager.VisibleShields[0].GetCost(40) + empire.TechnologyManager.VisibleBeams[0].GetCost();
			currentShipDesigns.Add(scout2);

			Ship bomber = new Ship();
			bomber.Name = "Bomber";
			bomber.Size = 7;
			bomber.WhichStyle = 1;
			bomber.engine = empire.TechnologyManager.VisibleEngines[0];
			bomber.armor = empire.TechnologyManager.VisibleArmors[0];
			bomber.computer = empire.TechnologyManager.VisibleComputers[0];
			bomber.shield = empire.TechnologyManager.VisibleShields[0];
			bomber.weapons.Add(new Weapon(empire.TechnologyManager.VisibleBeams[0]));
			bomber.Cost = empire.TechnologyManager.VisibleEngines[0].GetCost(40) + empire.TechnologyManager.VisibleArmors[0].GetCost(40) + empire.TechnologyManager.VisibleComputers[0].GetCost(40) +
				empire.TechnologyManager.VisibleShields[0].GetCost(40) + empire.TechnologyManager.VisibleBeams[0].GetCost();

			currentShipDesigns.Add(bomber);

			Ship levi = new Ship();
			levi.Name = "Leviathian";
			levi.Size = 10;
			levi.WhichStyle = 4;
			levi.engine = empire.TechnologyManager.VisibleEngines[0];
			levi.armor = empire.TechnologyManager.VisibleArmors[0];
			levi.computer = empire.TechnologyManager.VisibleComputers[0];
			levi.shield = empire.TechnologyManager.VisibleShields[0];
			Weapon pewPew = new Weapon(empire.TechnologyManager.VisibleBeams[1]);
			pewPew.Mounts = 10;
			levi.weapons.Add(new Weapon(empire.TechnologyManager.VisibleBeams[0]));
			levi.weapons.Add(pewPew);
			levi.Cost = empire.TechnologyManager.VisibleEngines[0].GetCost(40) + empire.TechnologyManager.VisibleArmors[0].GetCost(40) + empire.TechnologyManager.VisibleComputers[0].GetCost(40) +
				empire.TechnologyManager.VisibleShields[0].GetCost(40) + empire.TechnologyManager.VisibleBeams[0].GetCost();

			currentShipDesigns.Add(levi);

			LastShipDesign = new Ship(scout); //Make a copy so we don't accidentally modify the original ship
		}

		public void SetupStarterFleet(int galaxyX, int galaxyY)
		{
			Fleet starterFleet = new Fleet();
			starterFleet.GalaxyX = 0;//galaxyX;
			starterFleet.GalaxyY = 0;// galaxyY;
			starterFleet.Empire = empire;
			starterFleet.AddShips(currentShipDesigns[0], 2);
			starterFleet.AddShips(currentShipDesigns[1], 5);
			starterFleet.AddShips(currentShipDesigns[2], 15);
			starterFleet.AddShips(currentShipDesigns[3], 5);
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
				if (fleet.GalaxyX == x && fleet.GalaxyY == y)
				{
					listOfFleets.Add(fleet);
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

		public bool MoveFleets(GridCell[][] gridCells)
		{
			bool stillHaveMovement = false;
			//This is called during end of turn processing
			foreach (Fleet fleet in fleets)
			{
				if (fleet.Move(gridCells))
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
						if (fleets[j].TravelNodes == null && fleets[j].GalaxyX == fleets[i].GalaxyX && fleets[j].GalaxyY == fleets[i].GalaxyY)
						{
							foreach (KeyValuePair<Ship, int> ship in fleets[j].Ships)
							{
								fleets[i].AddShips(ship.Key, ship.Value);
							}
							foreach (TransportShip ship in fleets[j].TransportShips)
							{
								fleets[i].AddTransport(ship.raceOnShip, ship.amount);
							}
							fleetsToRemove.Add(fleets[j]);
						}
					}
					foreach (Fleet fleet in fleetsToRemove)
					{
						fleets.Remove(fleet);
					}
				}
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
