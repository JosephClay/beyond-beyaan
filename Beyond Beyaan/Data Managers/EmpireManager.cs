using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	class EmpireManager
	{
		#region Variables
		private List<Empire> empires;
		//private List<CombatToProcess> combatsToProcess;
		private List<SettlerToProcess> colonizersToProcess;
		private List<SettlerToProcess> invadersToProcess;
		private Empire currentEmpire;
		private int empireIter;
		#endregion

		#region Properties
		public Empire CurrentEmpire
		{
			//Current human empire
			get { return currentEmpire; }
		}
		/*public List<CombatToProcess> CombatsToProcess
		{
			get { return combatsToProcess; }
		}
		public bool HasCombat
		{
			get { return combatsToProcess.Count > 0; }
		}*/
		public List<SettlerToProcess> ColonizersToProcess
		{
			get { return colonizersToProcess; }
		}
		public bool HasColonizers
		{
			get { return ColonizersToProcess.Count > 0; }
		}
		public List<SettlerToProcess> InvadersToProcess
		{
			get { return invadersToProcess; }
		}
		public bool HasInvaders
		{
			get { return InvadersToProcess.Count > 0; }
		}
		#endregion

		#region Constructors
		public EmpireManager()
		{
			empires = new List<Empire>();
			empireIter = -1;
			//combatsToProcess = new List<CombatToProcess>();
			colonizersToProcess = new List<SettlerToProcess>();
			invadersToProcess = new List<SettlerToProcess>();
		}
		#endregion

		#region Functions
		public void AddEmpire(Empire empire)
		{
			empires.Add(empire);
		}

		public void RemoveEmpire(Empire empire)
		{
			empires.Remove(empire);
		}

		public void SetupContacts()
		{
			foreach (Empire empire in empires)
			{
				empire.SetUpContacts(empires);
			}
		}

		public void SetInitialEmpireTurn()
		{
			empireIter = 0;
			//If first player isn't human, find the next one
			for (int i = 0; i < empires.Count; i++)
			{
				if (empires[i].Type != PlayerType.HUMAN)
				{
					empires[i].HandleAIEmpire();
				}
				else
				{
					currentEmpire = empires[i];
					empireIter = i;
					break;
				}
			}
		}

		public bool ProcessNextEmpire()
		{
			if (empireIter + 1 == empires.Count)
			{
				//It've reached teh end
				return true;
			}
			//This will update each empire if they're AI.  If an empire is human-controlled, it stops and waits for the player to press end of turn
			for (int i = empireIter + 1; i < empires.Count; i++)
			{
				empireIter = i;
				if (empires[i].Type != PlayerType.HUMAN)
				{
					empires[i].HandleAIEmpire();
					if (i + 1 == empires.Count)
					{
						//reached end of list with CPU player as last player
						return true;
					}
				}
				else
				{
					currentEmpire = empires[i];
					break;
				}
			}
			return false;
		}

		public List<Squadron> GetFleetsWithinArea(int x, int y, int width, int height)
		{
			List<Squadron> fleets = new List<Squadron>();
			foreach (Empire empire in empires)
			{
				foreach (Squadron fleet in empire.FleetManager.GetFleets())
				{
					if (fleet.FleetLocation.X >= x * 32 && fleet.FleetLocation.Y >= y * 32 && fleet.FleetLocation.X < (x + width) * 32 && fleet.FleetLocation.Y < (y + height) * 32)
					{
						fleets.Add(fleet);
					}
				}
			}
			return fleets;
		}

		public List<Squadron> GetFleetsNextToSystem(StarSystem system)
		{
			List<Squadron> fleets = new List<Squadron>();
			foreach (Empire empire in empires)
			{
				foreach (Squadron fleet in empire.FleetManager.GetFleets())
				{
					if (fleet.System == system)
					{
						fleets.Add(fleet);
					}
				}
			}
			return fleets;
		}

		public SquadronGroup GetSquadronsAtPoint(StarSystem system, int x, int y)
		{
			List<Squadron> fleets = new List<Squadron>();
			foreach (Empire empire in empires)
			{
				foreach (Squadron fleet in empire.FleetManager.ReturnFleetAtPoint(x, y))
				{
					fleets.Add(fleet);
				}
			}
			/*if (system != null && system.Y == y)
			{
				foreach (Squadron fleet in system.SystemFleets)
				{
					fleets.Add(fleet);
				}
			}*/
			return (fleets.Count > 0 ? new SquadronGroup(fleets, CurrentEmpire, system) : null);
		}

		public bool UpdateFleetMovement()
		{
			bool stillHaveMovement = false;
			foreach (Empire empire in empires)
			{
				if (empire.FleetManager.MoveFleets())
				{
					stillHaveMovement = true;
				}
			}
			return stillHaveMovement;
		}

		public void UpdateEmpires(Galaxy galaxy, PlanetTypeManager planetTypeManager, Random r)
		{
			foreach (Empire empire in empires)
			{
				empire.SitRepManager.ClearItems();
				empire.FleetManager.ResetFleetMovements();
				empire.ProcessTurn();
				empire.UpdateProjects(planetTypeManager, r);
				empire.TechnologyManager.ProcessResearchTurn(empire.ResearchPoints, empire.SitRepManager);
				empire.CheckExploredSystems(galaxy);
				empire.UpdateResearchPoints();
				empire.ContactManager.UpdateContacts(empire.SitRepManager);
			}
		}

		public void LookForCombat()
		{
			/*List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in empires)
			{
				foreach (Fleet fleet in empire.FleetManager.GetFleets())
				{
					fleets.Add(fleet);
				}
			}
			combatsToProcess = new List<CombatToProcess>();
			CombatToProcess combatToProcess = new CombatToProcess(0, 0, fleets);
			combatsToProcess.Add(combatToProcess);*/
		}

		public void CheckForColonizers(Galaxy galaxy)
		{
			colonizersToProcess.Clear();
			foreach (Empire empire in empires)
			{
				foreach (Squadron fleet in empire.FleetManager.GetFleets())
				{
					//Fleet have stopped, and has colony ships
					if (fleet.ColonizablePlanets.Count > 0 && fleet.TravelNodes == null)
					{
						StarSystem system = fleet.System;

						bool breakOut = false;
						foreach (Planet planet in system.Planets)
						{
							if (planet.Owner != null)
							{
								//Already owned, skip
								continue;
							}
							foreach (string colonizablePlanet in fleet.ColonizablePlanets)
							{
								if (string.Compare(colonizablePlanet, planet.PlanetType.InternalName) == 0)
								{
									//At least one planet can be colonized by at least one ship, add this to processing
									SettlerToProcess settler = new SettlerToProcess();
									settler.whichFleet = fleet;
									settler.whichSystem = system;
									colonizersToProcess.Add(settler);
									breakOut = true;
									break;
								}
							}
							if (breakOut)
							{
								break;
							}
						}
					}
				}
			}
		}

		public void CheckForInvaders(Galaxy galaxy)
		{
			invadersToProcess.Clear();
			foreach (Empire empire in empires)
			{
				foreach (Squadron fleet in empire.FleetManager.GetFleets())
				{
					//Fleet have stopped, and has colony ships
					if (fleet.TotalPeopleInTransit > 0 && fleet.TravelNodes == null)
					{
						StarSystem system = fleet.System;

						foreach (Planet planet in system.Planets)
						{
							if (planet.Owner == null)
							{
								//Can't land on empty planets
								continue;
							}
							else
							{
								//At least one planet can be colonized by at least one ship, add this to processing
								SettlerToProcess settler = new SettlerToProcess();
								settler.whichFleet = fleet;
								settler.whichSystem = system;
								invadersToProcess.Add(settler);
								break;
							}
						}
					}
				}
			}
		}

		public void ClearEmptyFleets()
		{
			//Done after colonization/invasion, it's possible that a fleet consisted only a colony ship, and it've colonized
			foreach (Empire empire in empires)
			{
				empire.FleetManager.ClearEmptyFleets();
			}
		}

		/*public void UpdateInfluenceMaps(Galaxy galaxy)
		{
			GridCell[][] gridCells = galaxy.GetGridCells();
			Dictionary<Empire, int>[][] cells = new Dictionary<Empire,int>[gridCells.Length][];
			for (int i = 0; i < gridCells.Length; i++)
			{
				cells[i] = new Dictionary<Empire, int>[gridCells.Length];
				for (int j = 0; j < cells[i].Length; j++)
				{
					cells[i][j] = new Dictionary<Empire, int>();
				}
			}

			foreach (StarSystem system in galaxy.GetAllStars())
			{
				if (system.DominantEmpire != null)
				{
					//Get the total influence from this system
					float totalPopulation = 0.0f;
					foreach (Planet planet in system.Planets)
					{
						if (planet.Owner == system.DominantEmpire)
						{
							totalPopulation += planet.TotalPopulation;
						}
					}

					int roundedPopulation = (int)totalPopulation;
					if (totalPopulation - roundedPopulation >= 0.5f)
					{
						//round it up
						roundedPopulation++;
					}

					for (int i = 0; i < system.Size; i++)
					{
						for (int j = 0; j < system.Size; j++)
						{
							Dictionary<Empire, int> cell = cells[system.X + i][system.Y + j];
							//Mark this system's gridcells as owned by the current empire
							if (!cell.ContainsKey(system.DominantEmpire))
							{
								cell.Add(system.DominantEmpire, -1);
							}
							else
							{
								cell[system.DominantEmpire] = -1;
							}
						}
					}

					int distance = 1;
					while (roundedPopulation > 0)
					{
						bool[][] disc = Utility.CalculateDisc(distance, system.Size);
						int x = system.X - distance;
						int y = system.Y - distance;
						for (int i = 0; i < disc.Length; i++)
						{
							for (int j = 0; j < disc[i].Length; j++)
							{
								if (disc[i][j] && x + i >= 0 && x + i < gridCells.Length && y + j >= 0 && y + j < gridCells.Length)
								{
									Dictionary<Empire, int> cell = cells[x + i][y + j];
									if (cell.ContainsKey(system.DominantEmpire))
									{
										if (cell[system.DominantEmpire] >= 0)
										{
											cell[system.DominantEmpire] += 1;
										}
									}
									else
									{
										cell.Add(system.DominantEmpire, 1);
									}
								}
							}
						}
						distance++;
						roundedPopulation -= 20;
					}
				}
			}

			//Now that the influence map is calculated, figure out the dominant and secondary influence per grid cell
			for (int i = 0; i < cells.Length; i++)
			{
				for (int j = 0; j < cells.Length; j++)
				{
					int dominantValue = 0;
					int secondaryValue = 0;
					foreach (KeyValuePair<Empire, int> influence in cells[i][j])
					{
						if ((influence.Value > dominantValue && dominantValue >= 0) || influence.Value == -1)
						{
							gridCells[i][j].secondaryEmpire = gridCells[i][j].dominantEmpire;
							gridCells[i][j].dominantEmpire = influence.Key;
							secondaryValue = dominantValue;
							dominantValue = influence.Value;
						}
						else if (influence.Value > secondaryValue)
						{
							gridCells[i][j].secondaryEmpire = influence.Key;
							secondaryValue = influence.Value;
						}
					}

					if (gridCells[i][j].secondaryEmpire == null || gridCells[i][j].dominantEmpire == null)
					{
						continue;
					}
					gridCells[i][j].dominantEmpire.ContactManager.EstablishContact(gridCells[i][j].secondaryEmpire, gridCells[i][j].dominantEmpire.SitRepManager);
					gridCells[i][j].secondaryEmpire.ContactManager.EstablishContact(gridCells[i][j].dominantEmpire, gridCells[i][j].secondaryEmpire.SitRepManager);
				}
			}
			List<Squadron> allFleets = GetFleetsWithinArea(-1, -1, galaxy.GalaxySize + 2, galaxy.GalaxySize + 2);
			foreach (Empire empire in empires)
			{
				//empire.CreateInfluenceMapSprite(gridCells);

				List<Squadron> visibleFleets = new List<Squadron>();
				foreach (Squadron fleet in allFleets)
				{
					if (fleet.Empire == empire)
					{
						continue;
					}
					if (gridCells[fleet.GalaxyX][fleet.GalaxyY].dominantEmpire == empire || gridCells[fleet.GalaxyX][fleet.GalaxyY].secondaryEmpire == empire)
					{
						visibleFleets.Add(fleet);
					}
				}

				visibleFleets.Sort((Squadron a, Squadron b) => { return string.Compare(a.Empire.EmpireName, b.Empire.EmpireName); });
				empire.SetVisibleFleets(visibleFleets);
			}
		}*/

		public void UpdateMigration(Galaxy galaxy)
		{
			//GridCell[][] gridCells = galaxy.GetGridCells();
			foreach (Empire empire in empires)
			{
				empire.SystemsUnderInfluence = new List<StarSystem>();
			}
			List<StarSystem> systems = galaxy.GetAllStars();
			foreach (StarSystem system in systems)
			{
				foreach (Empire empire in system.EmpiresWithPlanetsInThisSystem)
				{
					empire.SystemsUnderInfluence.Add(system);
				}
				system.EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
				foreach (Empire empire in system.EmpiresWithFleetAdjacentThisTurn)
				{
					system.EmpiresWithFleetAdjacentLastTurn.Add(empire);
				}
				system.EmpiresWithFleetAdjacentThisTurn = new List<Empire>();

				List<Squadron> fleetsAdjacentThisTurn = GetFleetsNextToSystem(system);
				foreach (Squadron fleet in fleetsAdjacentThisTurn)
				{
					if (!system.EmpiresWithFleetAdjacentThisTurn.Contains(fleet.Empire))
					{
						system.EmpiresWithFleetAdjacentThisTurn.Add(fleet.Empire);
					}
				}

				foreach (Empire empire in system.EmpiresWithFleetAdjacentThisTurn)
				{
					if (system.EmpiresWithFleetAdjacentLastTurn.Contains(empire) && !empire.SystemsUnderInfluence.Contains(system))
					{
						empire.SystemsUnderInfluence.Add(system);
					}
				}
			}
			/*foreach (Empire empire in empires)
			{
				//Now that we know which systems are claimed by which empires, time to process the actual migration
				empire.UpdateMigration(galaxy);
			}*/
			foreach (StarSystem system in systems)
			{
				system.UpdateOwners();
			}
		}

		public void CheckForDefeatedEmpires()
		{
			List<Empire> empiresToRemove = new List<Empire>();
			foreach (Empire empire in empires)
			{
				if (empire.PlanetManager.Planets.Count == 0 && empire.FleetManager.GetFleets().Count == 0)
				{
					empiresToRemove.Add(empire);
				}
			}
			foreach (Empire empire in empiresToRemove)
			{
				empires.Remove(empire);

				//Add news event for empire downfall
			}
		}
		#endregion
	}
}
