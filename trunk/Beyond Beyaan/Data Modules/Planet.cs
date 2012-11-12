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
		/*private List<string> outputs;
		private Dictionary<string, int> outputPercentages;
		private Dictionary<string, bool> outputLocks;*/
		private Dictionary<Resource, float> _productions;
		private Dictionary<Resource, float> _consumptions;
		private Dictionary<Race, float> racePopulations;
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
					totalPopulation += racePopulations[race];
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

		/*public string AgricultureStringOutput
		{
			get 
			{
				float growthAmount = CalculateTotalPopGrowth();
				if (growthAmount < 0)
				{
					return String.Format("Starving ({0:0.00} Pop)", growthAmount);
				}
				if (growthAmount > 0.05f)
				{
					return String.Format("Content (+{0:0.00} Pop)", growthAmount);
				}
				return "Content"; 
			}
		}*/
		#endregion

		#region Constructor
		public Planet(string name, string numericName, PlanetType planetType, Random r, StarSystem system, int numOfRegions, RegionTypeManager regionTypeManager)
		{
			whichSystem = system;
			this.name = name;
			this.numericName = numericName;
			races = new List<Race>();
			racePopulations = new Dictionary<Race, float>();

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
				racePopulations.Add(owner.EmpireRace, planet.Population);
				//outputPercentages = new Dictionary<string, int>(planet.OutputSliderValues);
			}
			CalculateSpaceUsage();
		}

		/*public void SetOutputs(float[] percentages)
		{
			//Tally up available outputs that aren't locked
			bool[] oldLocks = new bool[5];
			oldLocks[0] = AgricultureLocked;
			if (percentages[0] == -1)
			{
				AgricultureLocked = true;
			}
			oldLocks[1] = EnvironmentLocked;
			if (percentages[1] == -1)
			{
				EnvironmentLocked = true;
			}
			oldLocks[2] = CommerceLocked;
			if (percentages[2] == -1)
			{
				CommerceLocked = true;
			}
			oldLocks[3] = ResearchLocked;
			if (percentages[3] == -1)
			{
				ResearchLocked = true;
			}
			oldLocks[4] = ConstructionLocked;
			if (percentages[4] == -1)
			{
				ConstructionLocked = true;
			}
			int outputAvailable = 0;
			if (!AgricultureLocked)
			{
				outputAvailable += AgricultureAmount;
			}
			if (!EnvironmentLocked)
			{
				outputAvailable += EnvironmentAmount;
			}
			if (!CommerceLocked)
			{
				outputAvailable += CommerceAmount;
			}
			if (!ResearchLocked)
			{
				outputAvailable += ResearchAmount;
			}
			if (!ConstructionLocked)
			{
				outputAvailable += ConstructionAmount;
			}
			//calculate the new outputs
			int[] outputs = new int[5];
			int remainingOutput = outputAvailable;
			for (int i = 0; i < 5; i++)
			{
				if (percentages[i] >= 0)
				{
					outputs[i] = (int)(outputAvailable * percentages[i]);
					remainingOutput -= outputs[i];
				}
			}
			if (remainingOutput > 0)
			{
				for (int i = 4; i >= 0; i++)
				{
					if (outputs[i] > 0)
					{
						outputs[i] += remainingOutput;
						break;
					}
				}
			}
			//Now assign the modified outputs
			if (!AgricultureLocked)
			{
				AgricultureAmount = outputs[0];
			}
			if (!EnvironmentLocked)
			{
				EnvironmentAmount = outputs[1];
			}
			if (!CommerceLocked)
			{
				CommerceAmount = outputs[2];
			}
			if (!ResearchLocked)
			{
				ResearchAmount = outputs[3];
			}
			if (!ConstructionLocked)
			{
				ConstructionAmount = outputs[4];
			}
			//Now set the locks back to normal
			AgricultureLocked = oldLocks[0];
			EnvironmentLocked = oldLocks[1];
			CommerceLocked = oldLocks[2];
			ResearchLocked = oldLocks[3];
			ConstructionLocked = oldLocks[4];
			UpdateOutput();
		}

		public void SetOutputAmount(OUTPUT_TYPE outputType, int amount)
		{
			int remainingPercentile = 100;
			if (AgricultureLocked)
			{
				remainingPercentile -= AgricultureAmount;
			}
			if (EnvironmentLocked)
			{
				remainingPercentile -= EnvironmentAmount;
			}
			if (CommerceLocked)
			{
				remainingPercentile -= CommerceAmount;
			}
			if (ConstructionLocked)
			{
				remainingPercentile -= ConstructionAmount;
			}
			if (ResearchLocked)
			{
				remainingPercentile -= ResearchAmount;
			}

			//if the player set the slider to or beyond the allowed percentile, all other sliders except food and waste are set to 0
			if (amount >= remainingPercentile)
			{
				//set all sliders to 0, and change amount to remainingPercentile
				if (!AgricultureLocked)
				{
					AgricultureAmount = 0;
				}
				if (!EnvironmentLocked)
				{
					EnvironmentAmount = 0;
				}
				if (!CommerceLocked)
				{
					CommerceAmount = 0;
				}
				if (!ResearchLocked)
				{
					ResearchAmount = 0;
				}
				if (!ConstructionLocked)
				{
					ConstructionAmount = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			switch (outputType)
			{
				case OUTPUT_TYPE.AGRICULTURE:
					{
						AgricultureAmount = amount;
						remainingPercentile -= AgricultureAmount;
					} break;
				case OUTPUT_TYPE.ENVIRONMENT:
					{
						EnvironmentAmount = amount;
						remainingPercentile -= EnvironmentAmount;
					} break;
				case OUTPUT_TYPE.COMMERCE:
					{
						CommerceAmount = amount;
						remainingPercentile -= CommerceAmount;
					} break;
				case OUTPUT_TYPE.CONSTRUCTION:
					{
						ConstructionAmount = amount;
						remainingPercentile -= ConstructionAmount;
					} break;
				case OUTPUT_TYPE.RESEARCH:
					{
						ResearchAmount = amount;
						remainingPercentile -= ResearchAmount;
					} break;
			}
			totalPointsExcludingSelectedType = GetTotalPercentageExcludingOutputAndLocked(outputType);

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if (!AgricultureLocked && outputType != OUTPUT_TYPE.AGRICULTURE)
				{
					prevValue = AgricultureAmount;
					AgricultureAmount -= (AgricultureAmount >= amountToDeduct ? amountToDeduct : AgricultureAmount);
					amountToDeduct -= (prevValue - AgricultureAmount);
				}
				if (amountToDeduct > 0)
				{
					if (!EnvironmentLocked && outputType != OUTPUT_TYPE.ENVIRONMENT)
					{
						prevValue = EnvironmentAmount;
						EnvironmentAmount -= (EnvironmentAmount >= amountToDeduct ? amountToDeduct : EnvironmentAmount);
						amountToDeduct -= (prevValue - EnvironmentAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!CommerceLocked && outputType != OUTPUT_TYPE.COMMERCE)
					{
						prevValue = CommerceAmount;
						CommerceAmount -= (CommerceAmount >= amountToDeduct ? amountToDeduct : CommerceAmount);
						amountToDeduct -= (prevValue - CommerceAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ResearchLocked && outputType != OUTPUT_TYPE.RESEARCH)
					{
						prevValue = ResearchAmount;
						ResearchAmount -= (ResearchAmount >= amountToDeduct ? amountToDeduct : ResearchAmount);
						amountToDeduct -= (prevValue - ResearchAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ConstructionLocked && outputType != OUTPUT_TYPE.CONSTRUCTION)
					{
						prevValue = ConstructionAmount;
						ConstructionAmount -= (ConstructionAmount >= amountToDeduct ? amountToDeduct : ConstructionAmount);
						amountToDeduct -= (prevValue - ConstructionAmount);
					}
				}
			}
			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!AgricultureLocked && outputType != OUTPUT_TYPE.AGRICULTURE)
				{
					AgricultureAmount += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!EnvironmentLocked && outputType != OUTPUT_TYPE.ENVIRONMENT)
					{
						EnvironmentAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!CommerceLocked && outputType != OUTPUT_TYPE.COMMERCE)
					{
						CommerceAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ResearchLocked && outputType != OUTPUT_TYPE.RESEARCH)
					{
						ResearchAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ConstructionLocked && outputType != OUTPUT_TYPE.CONSTRUCTION)
					{
						ConstructionAmount += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current output field
					switch (outputType)
					{
						case OUTPUT_TYPE.AGRICULTURE:
							AgricultureAmount += amountToAdd;
							break;
						case OUTPUT_TYPE.COMMERCE:
							CommerceAmount += amountToAdd;
							break;
						case OUTPUT_TYPE.CONSTRUCTION:
							ConstructionAmount += amountToAdd;
							break;
						case OUTPUT_TYPE.ENVIRONMENT:
							EnvironmentAmount += amountToAdd;
							break;
						case OUTPUT_TYPE.RESEARCH:
							ResearchAmount += amountToAdd;
							break;
					}
				}
			}
			UpdateOutput();
		}

		private int GetTotalPercentageExcludingOutputAndLocked(OUTPUT_TYPE whichOutput)
		{
			int total = 0;
			if (whichOutput != OUTPUT_TYPE.AGRICULTURE && !AgricultureLocked)
			{
				total += AgricultureAmount;
			}
			if (whichOutput != OUTPUT_TYPE.COMMERCE && !CommerceLocked)
			{
				total += CommerceAmount;
			}
			if (whichOutput != OUTPUT_TYPE.CONSTRUCTION && !ConstructionLocked)
			{
				total += ConstructionAmount;
			}
			if (whichOutput != OUTPUT_TYPE.ENVIRONMENT && !EnvironmentLocked)
			{
				total += EnvironmentAmount;
			}
			if (whichOutput != OUTPUT_TYPE.RESEARCH && !ResearchLocked)
			{
				total += ResearchAmount;
			}
			return total;
		}

		//This function is called every turn to ensure that a planet don't accidentally starve out or pollute unless locked
		public void SetMinimumFoodAndWaste()
		{
			if (!EnvironmentLocked)
			{
				//Reduce it if too much
				while (CalculatePollutionCleanup() == 0)
				{
					SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, EnvironmentAmount - 1);
				}
				//Now bring it up
				while (EnvironmentAmount < 100 && CalculatePollutionCleanup() > 0)
				{
					SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, EnvironmentAmount + 1);
				}
			}
			if (!AgricultureLocked)
			{
				bool previousLock = EnvironmentLocked;
				EnvironmentLocked = true;
				//Reduce it if too much
				while ((AgricultureOutput - TotalPopulation) / TotalPopulation > 1)
				{
					SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, AgricultureAmount - 1);
				}
				//Now bring it up to 1
				while ((AgricultureAmount + EnvironmentAmount) < 100 && (AgricultureOutput - TotalPopulation) / TotalPopulation < 1 && EnvironmentAmount < 100)
				{
					SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, AgricultureAmount + 1);
				}
				EnvironmentLocked = previousLock;
			}
			
			/*
			//Waste Processing uses the formula: Waste Processing Percentage must be greater than or equal to 1 / (3 + tech improvements + racial bonuses/negatives)
			amount = (1.0f / (3.0f)) * 100.0f; //add tech improvements and racial bonuses/negatives inside (3.0f)
			if (amount - (int)amount > 0)
			{
				amount += 1;
			}
			EnvironmentAmount = (int)amount;

			if (AgricultureAmount + EnvironmentAmount > 100)
			{
				//Starvation ensues, this planet is now unproductive until population drops or migrates
				AgricultureAmount = (AgricultureAmount + EnvironmentAmount) - 100;
				CommerceAmount = 0;
				ResearchAmount = 0;
				ConstructionAmount = 0;
				return; //Nothing more can be done at this point
			}
			int remainingPercentile = 100 - (AgricultureAmount + EnvironmentAmount);
			//If necessary, scale the other percentiles so it all adds up to 100

			int totalMiscPercentile = CommerceAmount + ConstructionAmount + ResearchAmount;
			if (totalMiscPercentile != remainingPercentile && totalMiscPercentile != 0)
			{
				//We need to scale them down or up, even if they're locked
				float scale = (float)remainingPercentile / totalMiscPercentile;
				totalMiscPercentile = 0;
				
				CommerceAmount = (int)(CommerceAmount * scale);
				totalMiscPercentile += CommerceAmount;

				ResearchAmount = (int)(ResearchAmount * scale);
				totalMiscPercentile += ResearchAmount;

				ConstructionAmount = (int)(ConstructionAmount * scale);
				totalMiscPercentile += ConstructionAmount;

				if (totalMiscPercentile < remainingPercentile)
				{
					//We have some extra points left from roundoff
					int highest = 0;
					OUTPUT_TYPE type = OUTPUT_TYPE.COMMERCE;
					if (CommerceAmount > highest)
					{
						type = OUTPUT_TYPE.COMMERCE;
						highest = CommerceAmount;
					}
					if (ResearchAmount > highest)
					{
						type = OUTPUT_TYPE.RESEARCH;
						highest = ResearchAmount;
					}
					if (ConstructionAmount > highest)
					{
						type = OUTPUT_TYPE.CONSTRUCTION;
					}
					switch (type)
					{
						case OUTPUT_TYPE.COMMERCE:
							{
								CommerceAmount += (remainingPercentile - totalMiscPercentile);
							} break;
						case OUTPUT_TYPE.CONSTRUCTION:
							{
								ConstructionAmount += (remainingPercentile - totalMiscPercentile);
							} break;
						case OUTPUT_TYPE.RESEARCH:
							{
								ResearchAmount += (remainingPercentile - totalMiscPercentile);
							} break;
					}
				}
				else if (totalMiscPercentile > remainingPercentile)
				{
					//We have too much points, remove from highest
					int total = totalMiscPercentile - remainingPercentile;
					if (CommerceAmount > total)
					{
						CommerceAmount -= total;
						return;
					}
					if (ResearchAmount > total)
					{
						ResearchAmount -= total;
						return;
					}
					if (ConstructionAmount > total)
					{
						ConstructionAmount -= total;
						return;
					}

					//At this point, subtract from more than one field
					total -= CommerceAmount;
					CommerceAmount = 0;
					if (total > 0)
					{
						total -= ResearchAmount;
						ResearchAmount = 0;
						if (total > 0)
						{
							ConstructionAmount = 0;
						}
					}
				}
			}
			else if (totalMiscPercentile == 0 && remainingPercentile > 0)
			{
				SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, remainingPercentile);
			}
		}

		private void UpdateOutput()
		{
			EnvironmentOutput = (((float)(EnvironmentAmount) / 100.0f) * TotalPopulation) * 4;
			AgricultureOutput = (((float)(AgricultureAmount) / 100.0f) * TotalPopulation) * 4;
			CommerceOutput = ((float)(CommerceAmount) / 100.0f) * TotalPopulation;
			ResearchOutput = ((float)(ResearchAmount) / 100.0f) * TotalPopulation;
			ConstructionOutput = ((float)(ConstructionAmount) / 100.0f) * TotalPopulation;
		}*/

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
			foreach (Race race in races)
			{
				racePopulations[race] += CalculateRaceGrowth(race, foodShortages);
			}

			races.Sort((Race a, Race b) => { return (racePopulations[a].CompareTo(racePopulations[b])); });

			//CalculateOutputs();

			//SetMinimumFoodAndWaste();
			//UpdateOutput();
			CalculateSpaceUsage();
		}

		private float CalculateTotalPopGrowth(Dictionary<Resource, float> shortages)
		{
			//Factor in food surplus/starvation
			//float foodModifier = (AgricultureOutput - TotalPopulation) / TotalPopulation;

			//Calculate normal population growth using formula (rate of growth * population) * (1 - (population / planet's capacity)) with foodModifier in place of 1
			float popGrowth = 0;
			foreach (Race race in races)
			{
				popGrowth += CalculateRaceGrowth(race, shortages);
			}

			return popGrowth;
		}

		private float CalculateRaceGrowth(Race race, Dictionary<Resource, float> shortages)
		{
			//First, find the lowest shortage (a race may consume more than one resource)
			float shortage = 1.0f;
			foreach (var consumption in race.Consumptions)
			{
				if (shortages[consumption.Key] < shortage)
				{
					shortage = shortages[consumption.Key];
				}
			}

			//Last, calculate the growth formula.  If there's a shortage that exceeds the percentage of space used, there's starvation
			//If space usage exceeds the planet's capacity, there's overcrowding and will always exceed the max 1.0f value for shortage
			return racePopulations[race] * 0.05f * (shortage - (spaceUsage / (regions.Count * 10)));
		}

		public void CalculateFoodConsumption(Dictionary<Resource, float> foodConsumption)
		{
			foreach (var race in racePopulations)
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
			foreach (var race in racePopulations)
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

					foreach (var race in racePopulations)
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

					foreach (var race in racePopulations)
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

		//This grabs the resouces, then deducts what it don't need to consume as it is passed to system and empire
		public void TallyAvailableResourcesAndShortages(Dictionary<Resource, float> availableResources, Dictionary<Resource, float> shortages)
		{
			//To-do: verify that this copies the dictionary, and not just the reference.  Don't want float changes to affect the original
			AvailableResources = new Dictionary<Resource, float>(Resources);
			Shortages.Clear();

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
		public void AddSuppliedResources(Dictionary<Resource, float> resources)
		{
			foreach (KeyValuePair<Resource, float> resource in resources)
			{
				if (ResourcesSupplied.ContainsKey(resource.Key))
				{
					ResourcesSupplied[resource.Key] += resource.Value;
				}
				else
				{
					ResourcesSupplied[resource.Key] = resource.Value;
				}
			}
		}

		//Resources that's sent to other planets
		public void AddSharedResources(Dictionary<Resource, float> resources)
		{
			foreach (KeyValuePair<Resource, float> resource in resources)
			{
				if (ResourcesShared.ContainsKey(resource.Key))
				{
					ResourcesShared[resource.Key] += resource.Value;
				}
				else
				{
					ResourcesShared[resource.Key] = resource.Value;
				}
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

					foreach (var race in racePopulations)
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

					foreach (var race in racePopulations)
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
			foreach (var race in racePopulations)
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

		/*private float CalculatePollutionCleanup()
		{
			float amount = AccumulatedWaste - EnvironmentOutput;
			if (amount < 0)
			{
				amount = 0;
			}
			return amount;
		}

		private float CalculatePollution()
		{
			return (AgricultureOutput + ConstructionOutput + CommerceOutput + ResearchOutput) / 2.0f;
		}*/

		/*private void CheckPollutionLevel(PlanetTypeManager planetTypeManager, Random r)
		{
			if (accumulatedWaste >= planetType.PollutionThreshold)
			{
				planetType = planetTypeManager.GetPlanet(planetType.PlanetDegradesTo);
				populationMax = planetType.GetRandomMaxPop(r);
				if (populationMax == 0)
				{
					//This planet is uninhabitable
					RemoveAllRaces();
					Owner.PlanetManager.Planets.Remove(this);
					Owner = null;
				}
			}
		}*/

		public float GetRacePopulation(Race whichRace)
		{
			return racePopulations[whichRace];
		}
		public KeyValuePair<Race, int> GetDominantRacePop()
		{
			int pop = 0;
			Race whichRace = null;
			foreach (Race race in races)
			{
				if (racePopulations[race] > pop)
				{
					pop = (int)racePopulations[race];
					whichRace = race;
				}
			}
			return new KeyValuePair<Race,int>(whichRace, pop);
		}

		public void AddRacePopulation(Race whichRace, float amount)
		{
			if (!racePopulations.ContainsKey(whichRace))
			{
				racePopulations.Add(whichRace, amount);
				races.Add(whichRace);
			}
			else
			{
				racePopulations[whichRace] += amount;
			}
			CalculateSpaceUsage();
		}

		public void RemoveRacePopulation(Race whichRace, float amount)
		{
			racePopulations[whichRace] -= amount;
			if (racePopulations[whichRace] < 0.1f)
			{
				//Must have at least 1.0 to have self-sustaining population
				racePopulations.Remove(whichRace);
				races.Remove(whichRace);
			}
		}

		public void RemoveRace(Race whichRace)
		{
			racePopulations.Remove(whichRace);
			races.Remove(whichRace);
			CalculateSpaceUsage();
		}

		public void RemoveAllRaces()
		{
			racePopulations.Clear();
			races.Clear();
			CalculateSpaceUsage();
		}

		private void CalculateSpaceUsage()
		{
			spaceUsage = 0;
			foreach (var race in racePopulations)
			{
				spaceUsage += race.Value * 1.0f; // race.Key.SpaceUsage[planetType.InternalName];
			}
		}
	}
}
