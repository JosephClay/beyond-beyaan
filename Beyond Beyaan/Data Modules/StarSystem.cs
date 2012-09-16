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

		public void SetSystem(Empire empire, StartingSystem system, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, Random r, out List<Planet> ownedPlanets)
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
							planets[i].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
						}
						else
						{
							planets[i].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
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
								planets[planetToReplace].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
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
								planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
							}
							else
							{
								planets.Insert(position, new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
								planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
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
						planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
					}
					else
					{
						planets.Insert(position, new Planet(name, string.Empty, planetType, r, this, 1, regionTypeManager));
						planets[position].SetPlanet(empire, planetTypeManager, regionTypeManager, system.Planets[i]);
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
			int totalAmount = GetProductionCapacity(whichEmpire);
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
			return availableProjects;
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
			foreach (Project project in whichEmpire.ProjectManager.Projects)
			{
				if (project.Location == this)
				{
					amount -= project.ProductionCapacityRequired;
				}
			}
			return amount;
		}
		#endregion
	}
}
