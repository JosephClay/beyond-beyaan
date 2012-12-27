using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class StarSystem : IComparable<StarSystem>
	{
		#region Member Variables
		StarType starType;
		private int x;
		private int y;
		private string name;

		private List<Empire> exploredBy;
		private Dictionary<Empire, int> planetSelected;
		internal List<Starlane> Starlanes;
		internal List<Starlane> InvisibleStarlanes;
		//private Dictionary<Empire, Stargate> stargates;

		private List<Planet> planets;
		internal List<Squadron> OrbitingSquadrons;
		internal Dictionary<Race, List<ShipInstance>> ShipReserves;
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
		public string Name
		{
			get { return name; }
		}
		public List<Planet> Planets
		{
			get { return planets; }
		}
		public StarType Type
		{
			get { return starType; }
		}
		public Label StarName { get; set; }
		public Empire DominantEmpire { get; private set; }

		public List<Empire> EmpiresWithFleetAdjacentLastTurn { get; set; }
		public List<Empire> EmpiresWithFleetAdjacentThisTurn { get; set; }
		public List<Empire> EmpiresWithPlanetsInThisSystem { get; private set; }

		public Dictionary<Empire, float> OwnerPercentage;
		public Dictionary<Empire, Dictionary<Resource, float>> Productions;
		public Dictionary<Empire, Dictionary<Resource, float>> Consumptions;
		public Dictionary<Empire, Dictionary<Resource, float>> AvailableResources;
		public Dictionary<Empire, Dictionary<Resource, float>> ResourcesSupplied;
		public Dictionary<Empire, Dictionary<Resource, float>> ResourcesShared;
		public Dictionary<Empire, Dictionary<Resource, float>> Resources;
		public Dictionary<Empire, Dictionary<Resource, float>> Shortages;
		public Dictionary<Empire, Dictionary<Resource, float>> ProjectResources;

		#region Dijkstra's Algorithm
		public double Distance { get; set; }
		public StarSystem PreviousSystem { get; set; }
		public Starlane PreviousStarlane { get; set; }
		#endregion
		#endregion

		#region Constructor
		public StarSystem(string name, int x, int y, StarType starType, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, Random r)
		{
			this.name = name;
			this.x = x;
			this.y = y;

			exploredBy = new List<Empire>();

			this.starType = starType;

			int amountOfPlanets = r.Next(starType.MinPlanets, starType.MaxPlanets);
			planets = new List<Planet>();
			for (int i = 0; i < amountOfPlanets; i++)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(this.name);
				sb.Append(" ");
				string numericName = Utility.ConvertNumberToRomanNumberical(i + 1);
				sb.Append(numericName);
				planets.Add(planetTypeManager.GenerateRandomPlanet(sb.ToString(), numericName, starType.GetRandomPlanetType(r), r, this, regionTypeManager));
			}
			StarName = new Label(name, 0, 0);
			EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
			EmpiresWithFleetAdjacentThisTurn = new List<Empire>();
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			OwnerPercentage = new Dictionary<Empire, float>();
			OrbitingSquadrons = new List<Squadron>();
			ShipReserves = new Dictionary<Race, List<ShipInstance>>();
			planetSelected = new Dictionary<Empire, int>();
			Starlanes = new List<Starlane>();
			InvisibleStarlanes = new List<Starlane>();
			//stargates = new Dictionary<Empire, Stargate>();

			Productions = new Dictionary<Empire, Dictionary<Resource, float>>();
			Consumptions = new Dictionary<Empire, Dictionary<Resource, float>>();
			AvailableResources = new Dictionary<Empire, Dictionary<Resource, float>>();
			ResourcesSupplied = new Dictionary<Empire, Dictionary<Resource, float>>();
			ResourcesShared = new Dictionary<Empire, Dictionary<Resource, float>>();
			Resources = new Dictionary<Empire, Dictionary<Resource, float>>();
			Shortages = new Dictionary<Empire, Dictionary<Resource, float>>();
			ProjectResources = new Dictionary<Empire, Dictionary<Resource, float>>();
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

		public void SetSystem(Empire empire, StartingSystem system, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, ResourceManager resourceManager, Random r, out List<Planet> ownedPlanets)
		{
			if (system.OverrideSystem) //Replace everything in this system with the specified system's info
			{
				planets.Clear();
			}
			List<int> replacedPlanets = new List<int>();
			ownedPlanets = new List<Planet>();
			for (int i = 0; i < system.Planets.Count; i++)
			{
				PlanetType planetType = planetTypeManager.GetPlanet(system.Planets[i].PlanetType);
				if (system.ReplacePlanets)
				{
					if (system.InOrder)
					{
						if (i >= planets.Count)
						{
							planets.Add(new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
							planets[i].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
						}
						else
						{
							planets[i].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
						}
						if (system.Planets[i].Owned)
						{
							ownedPlanets.Add(planets[i]);
						}
					}
					else
					{
						int attempts = 0;
						while (attempts < 3)
						{
							if (planets.Count == 0)
							{
								attempts = 3;
								break;
							}
							int planetToReplace = r.Next(planets.Count);
							if (!replacedPlanets.Contains(planetToReplace))
							{
								planets[planetToReplace].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
								if (system.Planets[i].Owned)
								{
									ownedPlanets.Add(planets[planetToReplace]);
								}
								replacedPlanets.Add(planetToReplace);
								break;
							}
							attempts++;
						}
						if (attempts == 3)
						{
							//tried replacing planets, but no luck, just randomly insert the planet
							int position = r.Next(planets.Count + 1);
							if (position == planets.Count)
							{
								//add at end
								planets.Add(new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
								planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
							}
							else
							{
								planets.Insert(position, new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
								planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
								//Correct the list
								for (int p = 0; p < replacedPlanets.Count; p++)
								{
									if (replacedPlanets[p] >= position)
									{
										replacedPlanets[p]++;
									}
								}
							}
							if (system.Planets[position].Owned)
							{
								ownedPlanets.Add(planets[position]);
							}
							replacedPlanets.Add(position); //So we don't lose this planet
						}
					}
				}
				else
				{
					int position = r.Next(planets.Count + 1);
					if (position == planets.Count)
					{
						//add at end
						planets.Add(new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
						planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
					}
					else
					{
						planets.Insert(position, new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
						planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, resourceManager, system.Planets[i]);
					}
					if (system.Planets[position].Owned)
					{
						ownedPlanets.Add(planets[position]);
					}
				}
			}

			int k = 1;
			for (int i = 0; i < planets.Count; i++)
			{
				string numericValue = Utility.ConvertNumberToRomanNumberical(k);
				planets[i].SetName(Name + " " + numericValue, numericValue);
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
		}

		private void UpdateDominantEmpire()
		{
			Dictionary<Empire, int> count = new Dictionary<Empire, int>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (count.ContainsKey(planet.Owner))
					{
						count[planet.Owner] = count[planet.Owner] + 1;
					}
					else
					{
						count.Add(planet.Owner, 1);
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
			return EmpiresWithPlanetsInThisSystem.Contains(empire);
		}
		public bool SystemHaveInhabitablePlanets()
		{
			foreach (Planet planet in planets)
			{
				if (planet.PlanetType.Inhabitable)
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
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			int amountOfPlanetsOwned = 0;
			Dictionary<Empire, int> planetsOwned = new Dictionary<Empire,int>();
			OwnerPercentage = new Dictionary<Empire, float>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (!EmpiresWithPlanetsInThisSystem.Contains(planet.Owner))
					{
						EmpiresWithPlanetsInThisSystem.Add(planet.Owner);
						planetsOwned.Add(planet.Owner, 1);
					}
					else
					{
						planetsOwned[planet.Owner] += 1;
					}
					amountOfPlanetsOwned++;
				}
			}

			//Update the color percentage here
			foreach (KeyValuePair<Empire, int> keyValuePair in planetsOwned)
			{
				OwnerPercentage[keyValuePair.Key] = ((float)keyValuePair.Value / (float)amountOfPlanetsOwned);
			}

			UpdateDominantEmpire();
		}
		public int GetPlanetSelected(Empire whichEmpire)
		{
			if (planetSelected.ContainsKey(whichEmpire))
			{
				return planetSelected[whichEmpire];
			}
			else if (IsThisSystemExploredByEmpire(whichEmpire))
			{
				planetSelected.Add(whichEmpire, -1);
			}
			return -1;
		}
		public void SetPlanetSelected(Empire empire, int whichPlanet)
		{
			//Should already exist, because UI calls GetPlanetSelected first before this can be ever called
			planetSelected[empire] = whichPlanet;
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
			int amount = 0;
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
			foreach (Planet planet in Planets)
			{
				if (planet.Owner == whichEmpire)
				{
					pop += planet.TotalPopulation;
				}
			}
			return pop;
		}
		public void TallyConsumptions(Empire whichEmpire, Dictionary<Resource, float> consumptions)
		{
			if (Consumptions.ContainsKey(whichEmpire))
			{
				Consumptions[whichEmpire].Clear();
			}
			else
			{
				Consumptions.Add(whichEmpire, new Dictionary<Resource, float>());
			}

			foreach (Planet planet in Planets)
			{
				if (planet.Owner == whichEmpire)
				{
					planet.TallyConsumptions(Consumptions[whichEmpire]);
				}
			}

			foreach (KeyValuePair<Resource, float> consumption in Consumptions[whichEmpire])
			{
				if (consumption.Key.LimitTo == LimitTo.EMPIRE)
				{
					if (consumptions.ContainsKey(consumption.Key))
					{
						consumptions[consumption.Key] += consumption.Value;
					}
					else
					{
						consumptions[consumption.Key] = consumption.Value;
					}
				}
			}
		}
		public void TallyResources(Empire whichEmpire, Dictionary<Resource, float> resources)
		{
			if (Resources.ContainsKey(whichEmpire))
			{
				Resources[whichEmpire].Clear();
			}
			else
			{
				Resources.Add(whichEmpire, new Dictionary<Resource, float>());
			}
			if (ProjectResources.ContainsKey(whichEmpire))
			{
				ProjectResources[whichEmpire].Clear();
			}
			else
			{
				ProjectResources.Add(whichEmpire, new Dictionary<Resource, float>());
			}

			Dictionary<Resource, float> tempResources = new Dictionary<Resource, float>();
			foreach (Planet planet in Planets)
			{
				if (planet.Owner == whichEmpire)
				{
					planet.TallyResources(tempResources);
				}
			}

			foreach (KeyValuePair<Resource, float> resource in tempResources)
			{
				if (resource.Key.LimitTo == LimitTo.SYSTEM)
				{
					Resources[whichEmpire][resource.Key] = resource.Value;
				}
				else if (resource.Key.LimitTo == LimitTo.SYSTEM_DEVELOPMENT)
				{
					ProjectResources[whichEmpire][resource.Key] = resource.Value;
				}
				//At this point, the remaining resources are at empire-wide level.
				else
				{
					if (resources.ContainsKey(resource.Key))
					{
						resources[resource.Key] += resource.Value;
					}
					else
					{
						resources[resource.Key] = resource.Value;
					}
				}
			}
		}

		//This grabs the resouces, then deducts what it don't need to consume as it is passed to system and empire
		public void TallyAvailableResourcesAndShortages(Empire whichEmpire, Dictionary<Resource, float> availableResources, Dictionary<Resource, float> shortages)
		{
			if (AvailableResources.ContainsKey(whichEmpire))
			{
				AvailableResources[whichEmpire].Clear();
			}
			else
			{
				AvailableResources.Add(whichEmpire, new Dictionary<Resource, float>());
			}
			if (Shortages.ContainsKey(whichEmpire))
			{
				Shortages[whichEmpire].Clear();
			}
			else
			{
				Shortages.Add(whichEmpire, new Dictionary<Resource, float>());
			}

			foreach (Planet planet in Planets)
			{
				if (planet.Owner == whichEmpire)
				{
					Dictionary<Resource, float> tempAvailableResources = new Dictionary<Resource, float>();
					planet.TallyAvailableResourcesAndShortages(tempAvailableResources, Shortages[whichEmpire]);
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
				}
			}

			foreach (KeyValuePair<Resource, float> resource in AvailableResources[whichEmpire])
			{
				if (resource.Key.LimitTo == LimitTo.EMPIRE)
				{
					if (availableResources.ContainsKey(resource.Key))
					{
						availableResources[resource.Key] += resource.Value;
					}
					else
					{
						availableResources[resource.Key] = resource.Value;
					}
				}
			}

			foreach (KeyValuePair<Resource, float> resource in Shortages[whichEmpire])
			{
				if (resource.Key.LimitTo == LimitTo.EMPIRE)
				{
					if (shortages.ContainsKey(resource.Key))
					{
						shortages[resource.Key] += resource.Value;
					}
					else
					{
						shortages[resource.Key] = resource.Value;
					}
				}
			}
		}

		public void SetSharedResources(Empire empire, Dictionary<Resource, float> sharedAvailable, Dictionary<Resource, float> sharedConsumped)
		{
			Dictionary<Resource, float> percentageAvailableShared = new Dictionary<Resource, float>();
			Dictionary<Resource, float> percentageSharedConsumed = new Dictionary<Resource, float>();

			foreach (var shortage in Shortages[empire])
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
			}
			foreach (Planet planet in planets)
			{
				if (planet.Owner == empire)
				{
					planet.SetSharedResources(sharedAvailable, sharedConsumped, percentageAvailableShared, percentageSharedConsumed);
				}
			}
		}

		public void CalculatePopGrowth(Empire empire)
		{
			foreach (Planet planet in planets)
			{
				if (planet.Owner == empire)
				{
					planet.CalculatePopGrowth();
				}
			}
		}

		public void TallyProductions(Empire whichEmpire, Dictionary<Resource, float> productions)
		{
			if (Productions.ContainsKey(whichEmpire))
			{
				Productions[whichEmpire].Clear();
			}
			else
			{
				Productions.Add(whichEmpire, new Dictionary<Resource, float>());
			}

			foreach (Planet planet in Planets)
			{
				if (planet.Owner == whichEmpire)
				{
					planet.TallyProduction(Productions[whichEmpire]);
				}
			}

			foreach (KeyValuePair<Resource, float> production in Productions[whichEmpire])
			{
				if (production.Key.LimitTo == LimitTo.EMPIRE)
				{
					if (productions.ContainsKey(production.Key))
					{
						productions[production.Key] += production.Value;
					}
					else
					{
						productions[production.Key] = production.Value;
					}
				}
			}
		}
		#endregion
	}
}
