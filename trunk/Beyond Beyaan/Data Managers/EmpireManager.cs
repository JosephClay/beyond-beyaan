using System.Collections.Generic;

namespace Beyond_Beyaan
{
	class EmpireManager
	{
		#region Variables
		private List<Empire> empires;
		//private List<CombatToProcess> combatsToProcess;
		private Empire currentEmpire;
		private int empireIter;
		private GameMain _gameMain;
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
		#endregion

		#region Constructors
		public EmpireManager(GameMain gameMain)
		{
			_gameMain = gameMain;
			empires = new List<Empire>();
			empireIter = -1;
			//combatsToProcess = new List<CombatToProcess>();
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

		public void Reset()
		{
			empires.Clear();
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
			foreach (Empire empire in empires)
			{
				//Reset fleet movement and stuff
				empire.FleetManager.ResetFleetMovements();
			}
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

		public List<Fleet> GetFleetsWithinArea(float left, float top, float width, float height)
		{
			List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in empires)
			{
				foreach (Fleet fleet in empire.FleetManager.GetFleets())
				{
					if (fleet.GalaxyX + 16 < left || fleet.GalaxyY + 16 < top || fleet.GalaxyX - 16 > left + width || fleet.GalaxyY - 16 > top + height)
					{
						continue;
					}
					fleets.Add(fleet);
				}
			}
			return fleets;
		}

		public FleetGroup GetFleetsAtPoint(int x, int y)
		{
			List<Fleet> fleets = new List<Fleet>();
			foreach (Empire empire in empires)
			{
				foreach (Fleet fleet in empire.FleetManager.ReturnFleetAtPoint(x, y))
				{
					fleets.Add(fleet);
				}
			}
			return (fleets.Count > 0 ? new FleetGroup(fleets) : null);
		}

		public void ResetFleetMovement()
		{
			foreach (Empire empire in empires)
			{
				empire.FleetManager.ResetFleetMovements();
			}
		}

		public bool UpdateFleetMovement(float frameDeltaTime)
		{
			bool stillHaveMovement = false;
			foreach (Empire empire in empires)
			{
				if (empire.FleetManager.MoveFleets(frameDeltaTime))
				{
					stillHaveMovement = true;
				}
			}
			return stillHaveMovement;
		}

		public void MergeIdleFleets()
		{
			foreach (Empire empire in empires)
			{
				empire.FleetManager.MergeIdleFleets();
			}
		}

		public void ClearEmptyFleets()
		{
			foreach (Empire empire in empires)
			{
				empire.FleetManager.MergeIdleFleets();
			}
		}

		public void UpdateEmpires()
		{
			foreach (Empire empire in empires)
			{
				empire.SitRepManager.ClearItems();
				empire.CheckForBuiltShips();
				empire.UpdateResearchPoints();
				empire.TechnologyManager.ProcessResearchTurn(empire.ResearchPoints, _gameMain.Random, empire.SitRepManager);
				empire.ContactManager.UpdateContacts(empire.SitRepManager);
			}
		}

		public void UpdatePopulationGrowth()
		{
			foreach (Empire empire in empires)
			{
				empire.PlanetManager.UpdatePopGrowth();
			}
		}

		public Dictionary<Empire, List<StarSystem>> CheckExploredSystems(Galaxy galaxy)
		{
			Dictionary<Empire, List<StarSystem>> exploredSystems = new Dictionary<Empire, List<StarSystem>>();
			foreach (Empire empire in empires)
			{
				List<StarSystem> temp = empire.CheckExploredSystems(galaxy);
				if (empire.IsHumanPlayer && temp.Count > 0)
				{
					exploredSystems.Add(empire, temp);
				}
			}
			return exploredSystems;
		}

		public Dictionary<Empire, List<Fleet>> CheckColonizableSystems(Galaxy galaxy)
		{
			Dictionary<Empire, List<Fleet>> colonizableSystems = new Dictionary<Empire, List<Fleet>>();
			foreach (Empire empire in empires)
			{
				List<Fleet> temp = empire.CheckColonizableSystems(galaxy);
				if (empire.IsHumanPlayer && temp.Count > 0)
				{
					colonizableSystems.Add(empire, temp);
				}
			}
			return colonizableSystems;
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
			List<Fleet> allFleets = GetFleetsWithinArea(-1, -1, galaxy.GalaxySize + 2, galaxy.GalaxySize + 2);
			foreach (Empire empire in empires)
			{
				//empire.CreateInfluenceMapSprite(gridCells);

				List<Fleet> visibleFleets = new List<Fleet>();
				foreach (Fleet fleet in allFleets)
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

				visibleFleets.Sort((Fleet a, Fleet b) => { return string.Compare(a.Empire.EmpireName, b.Empire.EmpireName); });
				empire.SetVisibleFleets(visibleFleets);
			}
		}*/

		/*public void UpdateMigration(Galaxy galaxy)
		{
			GridCell[][] gridCells = galaxy.GetGridCells();
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

				List<Fleet> fleetsAdjacentThisTurn = GetFleetsWithinArea(system.X - 1, system.Y - 1, system.Size + 2, system.Size + 2);
				foreach (Fleet fleet in fleetsAdjacentThisTurn)
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
			foreach (Empire empire in empires)
			{
				//Now that we know which systems are claimed by which empires, time to process the actual migration
				empire.UpdateMigration(galaxy);
			}
			foreach (StarSystem system in systems)
			{
				system.UpdateOwners();
			}
		}*/
		#endregion
	}
}
