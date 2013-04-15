using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.Graphics;

namespace Beyond_Beyaan
{
	public class StarSystem : IComparable<StarSystem>
	{
		#region Member Variables

		private int x;
		private int y;

		private List<Empire> exploredBy;
		private Dictionary<Empire, int> sectorSelected;

		internal List<Squadron> OrbitingSquadrons;
		#endregion

		#region Properties
		public int X 
		{ 
			get { return x; } 
			set { x = value; } 
		}
		public int Y
		{
			get { return y; }
			set { y = value; }
		}

		public string Name { get; private set; }

		public List<SectorObject> SectorObjects { get; private set; }

		public StarType Type { get; private set; }
		public BBSprite Sprite { get; private set; }

		public Label StarName { get; set; }
		public Empire DominantEmpire { get; private set; }

		public List<Empire> EmpiresWithFleetAdjacentLastTurn { get; set; }
		public List<Empire> EmpiresWithFleetAdjacentThisTurn { get; set; }
		public List<Empire> EmpiresWithSectorsInThisSystem { get; private set; }

		public Dictionary<Empire, float> OwnerPercentage;

		#region Dijkstra's Algorithm
		public double Distance { get; set; }
		public StarSystem PreviousSystem { get; set; }
		public Starlane PreviousStarlane { get; set; }
		#endregion
		#endregion

		#region Constructor
		public StarSystem(string name, int x, int y, StarType starType, SectorTypeManager sectorTypeManager, SpriteManager spriteManager, Random r, Font font)
		{
			this.Name = name;
			this.x = x;
			this.y = y;

			exploredBy = new List<Empire>();
			Dictionary<string, int> generateNameIter = new Dictionary<string, int>();

			this.Type = starType;
			var objects = starType.GenerateSectorObjects(r);
			SectorObjects = new List<SectorObject>();
			foreach (var sectorObject in objects)
			{
				SectorObject newObject = new SectorObject(sectorObject);
				int iter;
				if (generateNameIter.ContainsKey(sectorObject.GenerateName))
				{
					iter = generateNameIter[sectorObject.GenerateName];
					generateNameIter[sectorObject.GenerateName]++;
				}
				else
				{
					iter = 1;
					generateNameIter[sectorObject.GenerateName] = 2;
				}
				string sectorName = string.IsNullOrEmpty(sectorObject.GenerateName) ? name : sectorObject.GenerateName;
				sectorName = sectorName + " " + Utility.ConvertNumberToRomanNumberical(iter);
				newObject.Name = sectorName;
				SectorObjects.Add(newObject);
			}

			Sprite = spriteManager.GetSprite(Type.SpriteName, r);

			StarName = new Label(name, 0, 0, font);
			EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
			EmpiresWithFleetAdjacentThisTurn = new List<Empire>();
			EmpiresWithSectorsInThisSystem = new List<Empire>();
			OwnerPercentage = new Dictionary<Empire, float>();
			OrbitingSquadrons = new List<Squadron>();
			sectorSelected = new Dictionary<Empire, int>();
		}
		#endregion

		#region Public Functions
		#region Overrides
		public int CompareTo(StarSystem starSystem)
		{
			/*int value = Distance.CompareTo(starSystem.Distance);
			if (value == 0)
			{
				return Name.CompareTo(starSystem.Name);
			}
			return value;*/
			return Distance.CompareTo(starSystem.Distance);
		}
		#endregion

		/*public void SetSystem(Empire empire, StartingSystem system, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, ResourceManager resourceManager, Random r, out List<Sector> ownedSectors)
		{
			if (system.OverrideSystem) //Replace everything in this system with the specified system's info
			{
				sectors.Clear();
			}
			List<int> replacedSectors = new List<int>();
			ownedSectors = new List<Sector>();
			for (int i = 0; i < system.Planets.Count; i++)
			{
				PlanetType planetType = planetTypeManager.GetPlanet(system.Planets[i].PlanetType);
				if (system.ReplacePlanets)
				{
					if (system.InOrder)
					{
						if (i >= sectors.Count)
						{
							Planet newPlanet = new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager);
							newPlanet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
							sectors.Add(new Sector(SECTORTYPE.PLANET, newPlanet, name, empire));
						}
						else
						{
							sectors[i].Planet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
							sectors[i].Owner = empire;
						}
						if (system.Planets[i].Owned)
						{
							ownedSectors.Add(sectors[i]);
						}
					}
					else
					{
						int attempts = 0;
						while (attempts < 3)
						{
							if (sectors.Count == 0)
							{
								attempts = 3;
								break;
							}
							int planetToReplace = r.Next(sectors.Count);
							if (!replacedSectors.Contains(planetToReplace))
							{
								sectors[planetToReplace].Planet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
								sectors[planetToReplace].Owner = empire;
								if (system.Planets[i].Owned)
								{
									ownedSectors.Add(sectors[planetToReplace]);
								}
								replacedSectors.Add(planetToReplace);
								break;
							}
							attempts++;
						}
						if (attempts == 3)
						{
							//tried replacing planets, but no luck, just randomly insert the planet
							int position = r.Next(sectors.Count + 1);
							if (position == sectors.Count)
							{
								//add at end
								Planet newPlanet = new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager);
								newPlanet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
								sectors.Add(new Sector(SECTORTYPE.PLANET, newPlanet, name, empire));
							}
							else
							{
								Planet newPlanet = new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager);
								newPlanet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
								sectors.Insert(position, new Sector(SECTORTYPE.PLANET, newPlanet, name, empire));
								//Correct the list
								for (int p = 0; p < replacedSectors.Count; p++)
								{
									if (replacedSectors[p] >= position)
									{
										replacedSectors[p]++;
									}
								}
							}
							if (system.Planets[position].Owned)
							{
								ownedSectors.Add(sectors[position]);
							}
							replacedSectors.Add(position); //So we don't lose this planet
						}
					}
				}
				else
				{
					int position = r.Next(sectors.Count + 1);
					if (position == sectors.Count)
					{
						//add at end
						Planet newPlanet = new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager);
						newPlanet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
						sectors.Add(new Sector(SECTORTYPE.PLANET, newPlanet, name, empire));
					}
					else
					{
						Planet newPlanet = new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager);
						newPlanet.SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
						sectors.Insert(position, new Sector(SECTORTYPE.PLANET, newPlanet, name, empire));
					}
					if (system.Planets[position].Owned)
					{
						ownedSectors.Add(sectors[position]);
					}
				}
			}

			int k = 1;
			for (int i = 0; i < sectors.Count; i++)
			{
				string numericValue = Utility.ConvertNumberToRomanNumberical(k);
				sectors[i].Name = Name + " " + numericValue;
				k++;
			}
			exploredBy.Add(empire);
			//stargates.Add(empire, new Stargate() { Size = 3 });
			UpdateOwners();
		}

		public void AddStarlane(Starlane starlane)
		{
			Starlanes.Add(starlane);
		}

		public void AddInvisibleStarlane(Starlane starlane)
		{
			InvisibleStarlanes.Add(starlane);
		}*/

		private void UpdateDominantEmpire()
		{
			Dictionary<Empire, int> count = new Dictionary<Empire, int>();
			foreach (SectorObject sector in SectorObjects)
			{
				if (sector.ClaimedBy != null)
				{
					if (count.ContainsKey(sector.ClaimedBy))
					{
						count[sector.ClaimedBy] = count[sector.ClaimedBy] + 1;
					}
					else
					{
						count.Add(sector.ClaimedBy, 1);
					}
				}
			}
			int biggestCount = 0;
			Empire biggestOwner = null;
			foreach (KeyValuePair<Empire, int> empire in count)
			{
				if (biggestCount < empire.Value)
				{
					biggestCount = empire.Value;
					biggestOwner = empire.Key;
				}
			}
			DominantEmpire = biggestOwner;
		}
		/*public Dictionary<Empire, Stargate> GetStargates()
		{
			return stargates;
		}*/
		public bool IsThisSystemExploredByEmpire(Empire empire)
		{
			return exploredBy.Contains(empire);
		}
		public bool SystemHavePlanetOwnedByEmpire(Empire empire)
		{
			return EmpiresWithSectorsInThisSystem.Contains(empire);
		}
		public bool SystemHaveInhabitableSectors()
		{
			foreach (SectorObject sector in SectorObjects)
			{
				//Look for inhabitable sectors that are unclaimed
				if (sector.Type.IsInhabitable && sector.ClaimedBy == null)
				{
					return true;
				}
			}
			return false;
		}
		public void AddEmpireExplored(Empire empire)
		{
			exploredBy.Add(empire);
		}
		public void UpdateOwners()
		{
			EmpiresWithSectorsInThisSystem = new List<Empire>();
			int amountOfSectorsOwned = 0;
			Dictionary<Empire, int> sectorsOwned = new Dictionary<Empire,int>();
			OwnerPercentage = new Dictionary<Empire, float>();
			foreach (SectorObject sector in SectorObjects)
			{
				if (sector.ClaimedBy != null)
				{
					if (!EmpiresWithSectorsInThisSystem.Contains(sector.ClaimedBy))
					{
						EmpiresWithSectorsInThisSystem.Add(sector.ClaimedBy);
						sectorsOwned.Add(sector.ClaimedBy, 1);
					}
					else
					{
						sectorsOwned[sector.ClaimedBy] += 1;
					}
					amountOfSectorsOwned++;
				}
			}

			//Update the color percentage here
			foreach (KeyValuePair<Empire, int> keyValuePair in sectorsOwned)
			{
				OwnerPercentage[keyValuePair.Key] = (keyValuePair.Value / (float)amountOfSectorsOwned);
			}

			UpdateDominantEmpire();
		}
		public int GetPlanetSelected(Empire whichEmpire)
		{
			if (sectorSelected.ContainsKey(whichEmpire))
			{
				return sectorSelected[whichEmpire];
			}
			if (IsThisSystemExploredByEmpire(whichEmpire))
			{
				sectorSelected.Add(whichEmpire, -1);
			}
			return -1;
		}
		public void SetSectorSelected(Empire empire, int whichSector)
		{
			//Should already exist, because UI calls GetPlanetSelected first before this can be ever called
			sectorSelected[empire] = whichSector;
		}
		public List<Project> GetAvailableProjects(Empire whichEmpire)
		{
			/*int totalAmount = GetProductionCapacity(whichEmpire);
			List<Project> availableProjects = new List<Project>();
			if (totalAmount <= 0)
			{
				return availableProjects; //No remaining production capacity, so return empty list
			}
			//int biggestSize = 0;
			foreach (Planet planet in planets)
			{
				if (!string.IsNullOrEmpty(planet.PlanetType.PlanetTerraformsTo) && planet.PlanetType.ProductionCapacityRequiredForTerraforming <= totalAmount)
				{
					availableProjects.Add(new Project(this, planet));
				}
			}
			foreach (ShipDesign ship in whichEmpire.FleetManager.ShipDesigns)
			{
				if (ship.ShipClass.Size <= totalAmount)
				{
					availableProjects.Add(new Project(this, ship));
				}
			}
			return availableProjects;*/
			return null;
		}
		public int GetProductionCapacity(Empire whichEmpire)
		{
			/*int amount = 0;
			foreach (Planet planet in planets)
			{
				if (planet.Owner == whichEmpire)
				{
					amount += (int)(planet.TotalPopulation / 15); //replace 50 with empire's production data
				}
			}
			/*foreach (Ship ship in dockedShips)
			 * {
			 *	amount += ship.capacity;
			 * }
			 **/
			//Now subtract the existing projects
			/*foreach (Project project in whichEmpire.ProjectManager.Projects)
			{
				if (project.Location == this)
				{
					amount -= project.ProductionCapacityRequired;
				}
			}
			return amount;*/
			return 0;
		}
		public float GetPopulation(Empire whichEmpire)
		{
			float pop = 0;
			/*foreach (Sector sector in sectors)
			{
				if (sector.SectorType == SECTORTYPE.PLANET && sector.Owner == whichEmpire)
				{
					pop += sector.Planet.TotalPopulation;
				}
			}*/
			return pop;
		}
		/*public void TallyConsumptions(Empire whichEmpire, Dictionary<Resource, float> consumptions)
		{
			foreach (Sector sector in sectors)
			{
				if (sector.SectorType == SECTORTYPE.PLANET && sector.Owner == whichEmpire)
				{
					sector.Planet.TallyConsumptions(consumptions);
				}
			}
		}
		public void TallyResources(Empire whichEmpire, Dictionary<Resource, float> resources)
		{
			foreach (Sector sector in sectors)
			{
				if (sector.SectorType == SECTORTYPE.PLANET && sector.Owner == whichEmpire)
				{
					sector.Planet.TallyResources(resources);
				}
			}
		}*/

		//This grabs the resouces, then deducts what it don't need to consume as it is passed to system and empire
		/*public void TallyAvailableResourcesAndShortages(Empire whichEmpire, Dictionary<Resource, float> availableResources, Dictionary<Resource, float> shortages)
		{
			foreach (Sector sector in sectors)
			{
				if (sector.SectorType == SECTORTYPE.PLANET && sector.Owner == whichEmpire)
				{
					sector.Planet.TallyAvailableResourcesAndShortages(availableResources, shortages);
					/*foreach (KeyValuePair<Resource, float> projectConsumption in ProjectConsumptions[whichEmpire])
					{
						if (tempAvailableResources.ContainsKey(projectConsumption.Key))
						{
							/* TODO: if (project allows multiple items produced at once)
							{
								ProjectInvestments[projectConsumption.Key] = AvailableResources[projectConsumption.Key];
								AvailableResources[projectConsumption.Key] = 0;
							}
							else*/
							/*{
								if (tempAvailableResources[projectConsumption.Key] >= projectConsumption.Value)
								{
									//Sufficient to finish this portion of the project's cost
									// TODO: add project here
									planet.AddSharedResources(projectConsumption.Key, projectConsumption.Value);
									tempAvailableResources[projectConsumption.Key] -= projectConsumption.Value;
								}
								else
								{
									//Not sufficient, but still make progress, so put in all the available resources
									// TODO: add project here
									planet.AddSharedResources(projectConsumption.Key, tempAvailableResources[projectConsumption.Key]);
									tempAvailableResources[projectConsumption.Key] = 0;
								}
							}
							if (tempAvailableResources[projectConsumption.Key] <= 0)
							{
								tempAvailableResources.Remove(projectConsumption.Key);
							}
						}
					}*/
				/*}
			}
		}

		public void SetSharedResources(Empire empire, Dictionary<Resource, float> sharedAvailable, Dictionary<Resource, float> sharedConsumped)
		{
			Dictionary<Resource, float> percentageAvailableShared = new Dictionary<Resource, float>();
			Dictionary<Resource, float> percentageSharedConsumed = new Dictionary<Resource, float>();

			/*foreach (var shortage in Shortages[empire])
			{
				if (shortage.Key.LimitTo == LimitTo.SYSTEM && AvailableResources[empire].ContainsKey(shortage.Key))
				{
					percentageAvailableShared[shortage.Key] = shortage.Value / AvailableResources[empire][shortage.Key];
					if (percentageAvailableShared[shortage.Key] > 1)
					{
						percentageAvailableShared[shortage.Key] = 1;
					}
					percentageSharedConsumed[shortage.Key] = AvailableResources[empire][shortage.Key] / shortage.Value;
					if (percentageSharedConsumed[shortage.Key] > 1)
					{
						percentageSharedConsumed[shortage.Key] = 1;
					}
				}
			}*/
			/*foreach (Sector sector in sectors)
			{
				if (sector.Owner == empire)
				{
					sector.Planet.SetSharedResources(sharedAvailable, sharedConsumped, percentageAvailableShared, percentageSharedConsumed);
				}
			}
		}

		public void CalculatePopGrowth(Empire empire)
		{
			foreach (Sector sector in sectors)
			{
				if (sector.Owner == empire)
				{
					sector.Planet.CalculatePopGrowth();
				}
			}
		}

		public void TallyProductions(Empire whichEmpire, Dictionary<Resource, float> productions)
		{
			foreach (Sector sector in sectors)
			{
				if (sector.SectorType == SECTORTYPE.PLANET && sector.Owner == whichEmpire)
				{
					sector.Planet.TallyProduction(productions);
				}
			}
		}*/
		#endregion
	}
}
