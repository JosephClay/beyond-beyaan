using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan
{
	/*public enum PLANET_TYPE { TERRAN = 0, JUNGLE, OCEAN, BADLAND, STEPPE, DESERT, ARCTIC, BARREN, TUNDRA, DEAD, VOLCANIC, TOXIC, RADIATED, ASTEROIDS, GAS_GIANT }
	public enum OUTPUT_TYPE { RESEARCH, COMMERCE, AGRICULTURE, ENVIRONMENT, CONSTRUCTION }
	public enum PLANET_CONSTRUCTION_BONUS { DEARTH, POOR, AVERAGE, COPIOUS, RICH }
	public enum PLANET_ENVIRONMENT_BONUS { DESOLATE, INFERTILE, AVERAGE, FERTILE, LUSH }
	public enum PLANET_ENTERTAINMENT_BONUS { INSIPID, DULL, AVERAGE, SENSATIONAL, EXCITING }*/

	public class Planet
	{
		#region Member Variables
		private StarSystem whichSystem;
		private PlanetType planetType;
		private Empire owner;
		string name;
		string numericName;
		private List<Region> regions;
		private float spaceUsage;
		private Dictionary<Resource, float> _productions;
		private Dictionary<Resource, float> _consumptions;
		private List<Race> races;
		#endregion

		#region Properties
		public StarSystem System
		{
			get { return whichSystem; }
		}
		public PlanetType PlanetType
		{
			get { return planetType; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		public string NumericName
		{
			get { return numericName; }
			set { numericName = value; }
		}
		public Empire Owner
		{
			get { return owner; }
			set { owner = value; }
		}
		public float TotalPopulation
		{
			get 
			{
				float totalPopulation = 0.0f;
				foreach (Race race in races)
				{
					totalPopulation += Population[race];
				}
				return totalPopulation;
			}
		}
		public float SpaceUsage
		{
			get
			{
				return spaceUsage;
			}
		}
		public List<Race> Races
		{
			get	{ return races;	}
		}
		public List<Region> Regions
		{
			get { return regions; }
		}
		public Dictionary<Resource, float> Productions { get; private set; }
		public Dictionary<Resource, float> Consumptions { get; private set; }
		public Dictionary<Resource, float> AvailableResources { get; private set; }
		public Dictionary<Resource, float> ResourcesSupplied { get; private set; }
		public Dictionary<Resource, float> ResourcesShared { get; private set; }
		public Dictionary<Resource, float> Resources { get; private set; }
		public Dictionary<Resource, float> Shortages { get; private set; }
		public Dictionary<Resource, float> ProjectConsumptions { get; private set; }
		public Dictionary<Resource, float> ProjectInvestments { get; private set; }

		public Dictionary<Race, float> PopGrowth { get; private set; }
		public Dictionary<Race, float> Population { get; private set; }
		#endregion

		#region Constructor
		public Planet(string name, string numericName, PlanetType planetType, Random r, StarSystem system, int numOfRegions, RegionTypeManager regionTypeManager)
		{
			whichSystem = system;
			this.name = name;
			this.numericName = numericName;
			races = new List<Race>();
			Population = new Dictionary<Race, float>();
			PopGrowth = new Dictionary<Race, float>();

			this.planetType = planetType;

			int[] probabilityRange = new int[16];

			string defaultRegionForAll = string.Empty;
			foreach (string regionDefault in planetType.DefaultRegions)
			{
				string[] values = regionDefault.Split(new[] { ',' });
				if (values[1] == "-1")
				{
					if (!string.IsNullOrEmpty(defaultRegionForAll))
					{
						throw new Exception("Planet Type " + planetType.InternalName + " has at least two default general regions (value of -1), max is 1");
					}
					defaultRegionForAll = values[0];
				}
			}
			if (string.IsNullOrEmpty(defaultRegionForAll))
			{
				throw new Exception("Planet Type " + planetType.InternalName + " has no default general region type (value of -1), must have 1");
			}

			regions = new List<Region>();
			for (int i = 0; i < numOfRegions; i++)
			{
				Region newRegion = new Region();
				newRegion.RegionType = regionTypeManager.GetRegionType(defaultRegionForAll);
				regions.Add(newRegion);
			}
			//outputPercentages = new Dictionary<string, int>();

			Productions = new Dictionary<Resource,float>();
			Consumptions = new Dictionary<Resource,float>();
			AvailableResources = new Dictionary<Resource,float>();
			ResourcesSupplied = new Dictionary<Resource,float>();
			ResourcesShared = new Dictionary<Resource,float>();
			Resources = new Dictionary<Resource,float>();
			Shortages = new Dictionary<Resource,float>();
			ProjectConsumptions = new Dictionary<Resource, float>();
			ProjectInvestments = new Dictionary<Resource, float>();
		}
		#endregion

		public void SetName(string name, string numericName)
		{
			this.name = name;
			this.numericName = numericName;
		}
		public void SetPlanet(Empire owner, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, StartingPlanet planet) //Set this planet to the specified information
		{
			planetType = planetTypeManager.GetPlanet(planet.PlanetType);
			regions = new List<Region>();
			foreach (string region in planet.Regions)
			{
				Region newRegion = new Region();
				newRegion.RegionType = regionTypeManager.GetRegionType(region);
				regions.Add(newRegion);
			}
			
			if (planet.Owned)
			{
				this.owner = owner;
				races.Add(owner.EmpireRace);
				Population.Add(owner.EmpireRace, planet.Population);
				//outputPercentages = new Dictionary<string, int>(planet.OutputSliderValues);
			}
			CalculateSpaceUsage();
		}

		/*public void SetPlanetType(PlanetType planet, Random r)
		{
			planetType = planet;
			populationMax = planetType.GetRandomMaxPop(r);
		}*/

		//Used for end of turn processing
		public void UpdatePlanet(Dictionary<Resource, float> foodShortages)
		{
			//Update ship construction
			/*if (ConstructionOutput > 0)
			{
				constructionTotal += ConstructionOutput;
			}*/

			//Clean up last turn's pollution
			//accumulatedWaste = CalculatePollutionCleanup();
			//Add current turn's pollution
			//accumulatedWaste += CalculatePollution();
			//CheckPollutionLevel(planetTypeManager, r);

			//Calculate normal population growth using formula (rate of growth * population) * (1 - (population / planet's capacity)) with foodModifier in place of 1
			/*foreach (Race race in races)
			{
				Population[race] += CalculateRaceGrowth(race, foodShortages);
			}*/

			races.Sort((Race a, Race b) => { return (Population[a].CompareTo(Population[b])); });

			//CalculateOutputs();

			//SetMinimumFoodAndWaste();
			//UpdateOutput();
			CalculateSpaceUsage();
		}

		public void CalculatePopGrowth()
		{
			foreach (Race race in races)
			{
				//First, find the lowest shortage (a race may consume more than one resource)
				float shortage = 1.0f;
				foreach (var consumption in race.Consumptions)
				{
					if (Shortages.ContainsKey(consumption.Key))
					{
						float amount = 1.0f - (Shortages[consumption.Key] / Consumptions[consumption.Key]);
						if (shortage > amount)
						{
							shortage = amount;
						}
					}
				}

				//Last, calculate the growth formula.  If there's a shortage that exceeds the percentage of space used, there's starvation
				//If space usage exceeds the planet's capacity, there's overcrowding and will always exceed the max 1.0f value for shortage
				PopGrowth[race] = Population[race] * 0.05f * (shortage - (spaceUsage / (regions.Count * 10)));
			}
		}

		public void CalculateFoodConsumption(Dictionary<Resource, float> foodConsumption)
		{
			foreach (var race in Population)
			{
				foreach (var consumption in race.Key.Consumptions)
				{
					if (foodConsumption.ContainsKey(consumption.Key))
					{
						foodConsumption[consumption.Key] += race.Value * consumption.Value;
					}
					else
					{
						foodConsumption[consumption.Key] = race.Value * consumption.Value;
					}
				}
			}
		}

		public void ConsumeFood(Dictionary<Resource, float> resources, Dictionary<Resource, float> shortages)
		{
			foreach (var race in Population)
			{
				foreach (var consumption in race.Key.Consumptions)
				{
					if (resources.ContainsKey(consumption.Key))
					{
						resources[consumption.Key] -= race.Value * consumption.Value * shortages[consumption.Key];
					}
				}
			}
		}

		public void CalculateOptimalConsumption(Dictionary<Resource, float> currentConsumption)
		{
			float inefficiency = CalculateIneffectivity();

			//To-do: include maintenance for buildings

			

			//Calculate the initial region consumption (add up all the regional bonuses then average them)
			Dictionary<string, Dictionary<Resource, float>> regionConsumptions = new Dictionary<string, Dictionary<Resource, float>>();
			foreach (Region region in regions)
			{
				if (region.RegionType.Consumptions.Count == 0)
				{
					//Skip those that don't consume anything, i.e. farming or mining
					continue;
				}
				foreach (var consumption in region.RegionType.Consumptions)
				{
					float rate = consumption.Value * inefficiency;
					//To-do - include specials and buildings
					float totalConsumption = 0;

					foreach (var race in Population)
					{
						float value = (race.Value / regions.Count) * rate;
						if (race.Key.ConsumptionBonuses.ContainsKey(region.RegionType.RegionTypeName))
						{
							value *= race.Key.ConsumptionBonuses[region.RegionType.RegionTypeName];
						}
						totalConsumption += value;
					}
					if (currentConsumption.ContainsKey(consumption.Key))
					{
						currentConsumption[consumption.Key] += totalConsumption;
					}
					else
					{
						currentConsumption[consumption.Key] = totalConsumption;
					}
				}
			}
		}

		public void TallyConsumptions(Dictionary<Resource, float> consumptions)
		{
			Consumptions.Clear();
			foreach (Region region in regions)
			{
				foreach (var consumption in region.RegionType.Consumptions)
				{
					float totalConsumption = 0;

					foreach (var race in Population)
					{
						float value = (race.Value / regions.Count) * consumption.Value;
						if (race.Key.ConsumptionBonuses.ContainsKey(region.RegionType.RegionTypeName))
						{
							value *= race.Key.ConsumptionBonuses[region.RegionType.RegionTypeName];
						}
						totalConsumption += value;
					}
					if (Consumptions.ContainsKey(consumption.Key))
					{
						Consumptions[consumption.Key] += totalConsumption;
					}
					else
					{
						Consumptions[consumption.Key] = totalConsumption;
					}
				}
			}
			foreach (KeyValuePair<Resource, float> consumption in Consumptions)
			{
				if (consumption.Key.LimitTo != LimitTo.PLANET)
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

		//We build each system and the empire's total resources from each of the planet (we clear all systems and empire's resources before running this)
		public void TallyResources(Dictionary<Resource, float> resources)
		{
			foreach (KeyValuePair<Resource, float> resource in Resources)
			{
				if (resource.Key.LimitTo != LimitTo.PLANET)
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

		public void TallyProjectConsumptions(Dictionary<Resource, float> projectConsumptions)
		{
			ProjectConsumptions.Clear();
			// TODO: Add project
		}

		//This grabs the resouces, then deducts what it don't need to consume as it is passed to system and empire
		public void TallyAvailableResourcesAndShortages(Dictionary<Resource, float> availableResources, Dictionary<Resource, float> shortages)
		{
			//To-do: verify that this copies the dictionary, and not just the reference.  Don't want float changes to affect the original
			AvailableResources = new Dictionary<Resource, float>(Resources);
			Shortages.Clear();
			ProjectInvestments.Clear();

			//Calculate what's left after consumpting
			foreach (KeyValuePair<Resource, float> consumption in Consumptions)
			{
				if (AvailableResources.ContainsKey(consumption.Key))
				{
					if (AvailableResources[consumption.Key] >= consumption.Value)
					{
						AvailableResources[consumption.Key] -= consumption.Value;
					}
					else
					{
						//We have a shortage here
						Shortages.Add(consumption.Key, consumption.Value - AvailableResources[consumption.Key]);
						//Nothing to pass on
						AvailableResources[consumption.Key] = 0;
					}
				}
				else
				{
					//We have a shortage here
					Shortages.Add(consumption.Key, consumption.Value);
				}
			}
			foreach (KeyValuePair<Resource, float> projectConsumption in ProjectConsumptions)
			{
				if (AvailableResources.ContainsKey(projectConsumption.Key))
				{
					/* TODO: if (project allows multiple items produced at once)
					{
						ProjectInvestments[projectConsumption.Key] = AvailableResources[projectConsumption.Key];
						AvailableResources[projectConsumption.Key] = 0;
					}
					else*/
					{
						if (AvailableResources[projectConsumption.Key] >= projectConsumption.Value)
						{
							//Sufficient to finish this portion of the project's cost
							// TODO: add project here
							AvailableResources[projectConsumption.Key] -= projectConsumption.Value;
						}
						else
						{
							//Not sufficient, but still make progress, so put in all the available resources
							// TODO: add project here
							AvailableResources[projectConsumption.Key] = 0;
						}
					}
				}
			}

			foreach (KeyValuePair<Resource, float> resource in AvailableResources)
			{
				if (resource.Key.LimitTo != LimitTo.PLANET)
				{
					//Can pass this on
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
		}

		//Resources that's donated by other planets are added here
		public void AddSuppliedResources(Resource resourceSupplied, float amount)
		{
			if (ResourcesSupplied.ContainsKey(resourceSupplied))
			{
				ResourcesSupplied[resourceSupplied] += amount;
			}
			else
			{
				ResourcesSupplied[resourceSupplied] = amount;
			}
			Shortages[resourceSupplied] -= amount;
			if (Shortages[resourceSupplied] <= 0)
			{
				Shortages.Remove(resourceSupplied);
			}
		}

		//Resources that's sent to other planets
		public void AddSharedResources(Resource resourceShared, float amount)
		{
			if (ResourcesShared.ContainsKey(resourceShared))
			{
				ResourcesShared[resourceShared] += amount;
			}
			else
			{
				ResourcesShared[resourceShared] = amount;
			}
			AvailableResources[resourceShared] -= amount;
			if (AvailableResources[resourceShared] <= 0)
			{
				AvailableResources.Remove(resourceShared);
			}
		}

		public void CalculateConsumptionAndProduction(Dictionary<Resource, float> shortages, Dictionary<Resource, float> consumptions, Dictionary<Resource, float> productions)
		{
			float inefficiency = CalculateIneffectivity();

			//Calculate the initial region consumption and production (add up all the regional bonuses then average them)
			Dictionary<string, Dictionary<Resource, float>> regionConsumptions = new Dictionary<string, Dictionary<Resource, float>>();
			Dictionary<string, Dictionary<Resource, float>> regionProductions = new Dictionary<string, Dictionary<Resource, float>>();

			_productions = new Dictionary<Resource, float>();
			_consumptions = new Dictionary<Resource, float>();

			foreach (Region region in regions)
			{
				//Find the lowest shortage for the region
				float lowestShortage = 1.0f;
				foreach (var consumption in region.RegionType.Consumptions)
				{
					if (shortages[consumption.Key] < lowestShortage)
					{
						lowestShortage = shortages[consumption.Key];
					}
				}
				region.CurrentConsumptions = new Dictionary<Resource, float>();
				region.CurrentProductions = new Dictionary<Resource, float>();
				foreach (var consumption in region.RegionType.Consumptions)
				{
					// Get the regular consumption,
					float rate = consumption.Value * inefficiency * lowestShortage;
					//To-do - include specials and buildings in rate equation
					float totalConsumption = 0;

					foreach (var race in Population)
					{
						float value = (race.Value / regions.Count) * rate;
						if (race.Key.ConsumptionBonuses.ContainsKey(region.RegionType.RegionTypeName))
						{
							value *= race.Key.ConsumptionBonuses[region.RegionType.RegionTypeName];
						}
						totalConsumption += value;
					}
					//Region Consumptions
					region.CurrentConsumptions[consumption.Key] = totalConsumption;

					//Planet Consumptions
					if (_consumptions.ContainsKey(consumption.Key))
					{
						_consumptions[consumption.Key] += totalConsumption;
					}
					else
					{
						_consumptions[consumption.Key] = totalConsumption;
					}

					//Empire Consumptions
					if (consumptions.ContainsKey(consumption.Key))
					{
						consumptions[consumption.Key] += totalConsumption;
					}
					else
					{
						consumptions[consumption.Key] = totalConsumption;
					}
				}
				foreach (var production in region.RegionType.Productions)
				{
					// Get the regular production,
					float rate = production.Value * inefficiency * lowestShortage;
					//To-do - include specials and buildings in rate equation
					float totalProduction = 0;

					foreach (var race in Population)
					{
						float value = (race.Value / regions.Count) * rate;
						if (race.Key.ProductionBonuses.ContainsKey(region.RegionType.RegionTypeName))
						{
							value *= race.Key.ProductionBonuses[region.RegionType.RegionTypeName];
						}
						totalProduction += value;
					}
					//Region Productions
					region.CurrentProductions[production.Key] = totalProduction;

					//Planet Productions
					if (_productions.ContainsKey(production.Key))
					{
						_productions[production.Key] += totalProduction;
					}
					else
					{
						_productions[production.Key] = totalProduction;
					}

					//Empire Productions
					if (productions.ContainsKey(production.Key))
					{
						productions[production.Key] += totalProduction;
					}
					else
					{
						productions[production.Key] = totalProduction;
					}
				}
			}
		}

		private float CalculateIneffectivity()
		{
			float spaceUsage = 0;
			foreach (var race in Population)
			{
				spaceUsage += race.Value * 1.0f; //race.Key.SpaceUsage
			}

			Dictionary<string, float> ineffectivities = new Dictionary<string, float>();
			//Calculate ineffectivity - If people assigned are greater than planet capacity, diminish the optimal output 

			if (spaceUsage > regions.Count * 10)
			{
				return (float)(Math.Sqrt((regions.Count * 10) / spaceUsage));
			}
			return 1.0f;
		}

		public float GetRacePopulation(Race whichRace)
		{
			return Population[whichRace];
		}
		public KeyValuePair<Race, int> GetDominantRacePop()
		{
			int pop = 0;
			Race whichRace = null;
			foreach (Race race in races)
			{
				if (Population[race] > pop)
				{
					pop = (int)Population[race];
					whichRace = race;
				}
			}
			return new KeyValuePair<Race,int>(whichRace, pop);
		}

		public void AddRacePopulation(Race whichRace, float amount)
		{
			if (!Population.ContainsKey(whichRace))
			{
				Population.Add(whichRace, amount);
				races.Add(whichRace);
			}
			else
			{
				Population[whichRace] += amount;
			}
			CalculateSpaceUsage();
		}

		public void RemoveRacePopulation(Race whichRace, float amount)
		{
			Population[whichRace] -= amount;
			if (Population[whichRace] < 0.1f)
			{
				//Must have at least 1.0 to have self-sustaining population
				Population.Remove(whichRace);
				races.Remove(whichRace);
			}
		}

		public void RemoveRace(Race whichRace)
		{
			Population.Remove(whichRace);
			races.Remove(whichRace);
			CalculateSpaceUsage();
		}

		public void RemoveAllRaces()
		{
			Population.Clear();
			races.Clear();
			CalculateSpaceUsage();
		}

		private void CalculateSpaceUsage()
		{
			spaceUsage = 0;
			foreach (var race in Population)
			{
				spaceUsage += race.Value * 1.0f; // race.Key.SpaceUsage[planetType.InternalName];
			}
		}
	}
}
