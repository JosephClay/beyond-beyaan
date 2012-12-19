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
		public void SetPlanet(Empire owner, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, ResourceManager resourceManager, StartingPlanet planet) //Set this planet to the specified information
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
			foreach (var startingResource in planet.Resources)
			{
				Resource resource = resourceManager.GetResource(startingResource.Key);
				if (resource != null)
				{
					Resources[resource] = startingResource.Value;
				}
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
			foreach (var race in Population)
			{
				foreach (var consumption in race.Key.Consumptions)
				{
					float value = consumption.Value * race.Value;
					if (Consumptions.ContainsKey(consumption.Key))
					{
						Consumptions[consumption.Key] += value;
					}
					else
					{
						Consumptions[consumption.Key] = value;
					}
				}
			}

			// TODO: Tally building maintenance

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

		public void TallyProduction(Dictionary<Resource, float> productions)
		{
			Productions.Clear();
			foreach (Region region in regions)
			{
				float shortage = 1.0f;
				//First, find out shortages
				foreach (var consumption in region.RegionType.Consumptions)
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
				foreach (var production in region.RegionType.Productions)
				{
					float totalProduction = 0;
					foreach (var race in Population)
					{
						float value = (race.Value / regions.Count) * production.Value;
						if (race.Key.ProductionBonuses.ContainsKey(region.RegionType.RegionTypeName))
						{
							value *= race.Key.ProductionBonuses[region.RegionType.RegionTypeName];
						}
						value *= shortage;
						totalProduction += value;
					}
					if (Productions.ContainsKey(production.Key))
					{
						Productions[production.Key] += totalProduction;
					}
					else
					{
						Productions[production.Key] = totalProduction;
					}
				}
			}
			//Now add races' passive productions
			foreach (var race in Population)
			{
				foreach (var production in race.Key.Productions)
				{
					if (Productions.ContainsKey(production.Key))
					{
						Productions[production.Key] += production.Value * race.Value;
					}
					else
					{
						Productions[production.Key] = production.Value * race.Value;
					}
				}
			}
			foreach (KeyValuePair<Resource, float> production in Productions)
			{
				if (production.Key.LimitTo != LimitTo.PLANET)
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

		//Resources that's donated by other planets are added here
		public void SetSharedResources(Dictionary<Resource, float> empireSharedAvailable, Dictionary<Resource, float> empireConsumpedShared, Dictionary<Resource, float> systemSharedAvailable, Dictionary<Resource, float> systemConsumpedShared)
		{
			ResourcesShared.Clear();
			ResourcesSupplied.Clear();
			List<Resource> availableToRemove = new List<Resource>();
			List<Resource> shortageToRemove = new List<Resource>();
			foreach (var resource in AvailableResources)
			{
				if (empireSharedAvailable.ContainsKey(resource.Key))
				{
					ResourcesShared[resource.Key] = resource.Value * empireSharedAvailable[resource.Key];
					AvailableResources[resource.Key] -= ResourcesShared[resource.Key];
					if (AvailableResources[resource.Key] <= 0)
					{
						availableToRemove.Add(resource.Key);
					}
				}
				else if (systemSharedAvailable.ContainsKey(resource.Key))
				{
					ResourcesShared[resource.Key] = resource.Value * systemSharedAvailable[resource.Key];
					AvailableResources[resource.Key] -= ResourcesShared[resource.Key];
					if (AvailableResources[resource.Key] <= 0)
					{
						availableToRemove.Add(resource.Key);
					}
				}
			}
			foreach (var shortage in Shortages)
			{
				if (empireConsumpedShared.ContainsKey(shortage.Key))
				{
					ResourcesSupplied[shortage.Key] = shortage.Value * empireConsumpedShared[shortage.Key];
					Shortages[shortage.Key] -= ResourcesSupplied[shortage.Key];
					if (Shortages[shortage.Key] <= 0)
					{
						shortageToRemove.Add(shortage.Key);
					}
				}
				else if (systemConsumpedShared.ContainsKey(shortage.Key))
				{
					ResourcesSupplied[shortage.Key] = shortage.Value * systemConsumpedShared[shortage.Key];
					Shortages[shortage.Key] -= ResourcesSupplied[shortage.Key];
					if (Shortages[shortage.Key] <= 0)
					{
						shortageToRemove.Add(shortage.Key);
					}
				}
			}
			foreach (var shortage in shortageToRemove)
			{
				Shortages.Remove(shortage);
			}
			foreach (var resource in availableToRemove)
			{
				AvailableResources.Remove(resource);
			}
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
