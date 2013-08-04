using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum PLANET_TYPE { TERRAN = 0, JUNGLE, OCEAN, BADLAND, STEPPE, DESERT, ARCTIC, BARREN, TUNDRA, DEAD, VOLCANIC, TOXIC, RADIATED, NONE }
	public enum OUTPUT_TYPE { RESEARCH, DEFENSE, INFRASTRUCTURE, ENVIRONMENT, CONSTRUCTION }
	public enum PLANET_CONSTRUCTION_BONUS { DEARTH, POOR, AVERAGE, COPIOUS, RICH }
	public enum PLANET_ENVIRONMENT_BONUS { INFERTILE, AVERAGE, FERTILE, LUSH }
	public enum PLANET_RESEARCH_BONUS { AVERAGE, SENSATIONAL, EXCITING }
	public class Planet
	{
		#region Member Variables
		private StarSystem whichSystem;
		private PLANET_TYPE planetType;
		private string planetTypeString;
		private Empire owner;
		string name;
		private float populationMax;
		private float infrastructure;
		private Dictionary<Race, float> racePopulations;
		private List<Race> races;
		private int shipSelected;
		private Ship shipBeingBuilt;
		private float constructionTotal;
		private bool isWetClimate;
		#endregion

		#region Properties
		public StarSystem System
		{
			get { return whichSystem; }
		}
		public PLANET_TYPE PlanetType
		{
			get { return planetType; }
		}
		public int ColonyRequirement
		{
			get
			{
				switch (planetType)
				{
					case PLANET_TYPE.TERRAN:
					case PLANET_TYPE.OCEAN:
					case PLANET_TYPE.JUNGLE:
					case PLANET_TYPE.DESERT:
					case PLANET_TYPE.BADLAND:
					case PLANET_TYPE.STEPPE:
					case PLANET_TYPE.ARCTIC:
						return 1;
					case PLANET_TYPE.BARREN:
						return 2;
					case PLANET_TYPE.TUNDRA:
						return 3;
					case PLANET_TYPE.DEAD:
						return 4;
					case PLANET_TYPE.VOLCANIC:
						return 5;
					case PLANET_TYPE.TOXIC:
						return 6;
					case PLANET_TYPE.RADIATED:
						return 7;
					default:
						return int.MaxValue;
				}
			}
		}
		public string PlanetTypeString
		{
			get { return planetTypeString; }
		}
		public BBSprite SmallSprite { get; private set; }
		public BBSprite GroundSprite { get; private set; }
		public string Name
		{
			get { return name; }
			set { name = value; }
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
		public float InfrastructureTotal
		{
			get { return infrastructure; }
		}
		public float TotalProduction
		{
			get { return (TotalPopulation * 0.5f) + (infrastructure); }
		}
		public float ActualProduction
		{
			get { return TotalProduction * (1.0f - owner.ExpensesPercentage); }
		}
		public List<Race> Races
		{
			get	{ return races;	}
		}
		public float PopulationMax
		{
			get { return populationMax; }
		}
		public int ShipSelected
		{
			get { return shipSelected; }
			set { shipSelected = value; }
		}
		public Ship ShipBeingBuilt
		{
			get { return shipBeingBuilt; }
			set { shipBeingBuilt = value; }
		}
		public TravelNode RelocateToSystem { get; set; }
		public KeyValuePair<TravelNode, int> TransferSystem { get; set; }
		public float ShipConstructionLength
		{
			get
			{
				if (ConstructionAmount <= 0)
				{
					return -1;
				}
				float remaining = (shipBeingBuilt.Cost - constructionTotal);
				return remaining / (ConstructionAmount * 0.01f * ActualProduction);
			}
		}

		public PLANET_CONSTRUCTION_BONUS ConstructionBonus
		{
			get;
			private set;
		}
		public PLANET_ENVIRONMENT_BONUS EnvironmentBonus
		{
			get;
			private set;
		}
		public PLANET_RESEARCH_BONUS ResearchBonus
		{
			get;
			private set;
		}
		public string InfrastructureStringOutput
		{
			get 
			{
				if (InfrastructureAmount > 0)
				{
					if (infrastructure >= populationMax * 2)
					{
						return string.Format("{0} Buildings (+{1:0.0} BC)", (int)infrastructure, InfrastructureAmount * 0.01 * 0.5 * ActualProduction);
					}
					else
					{
						float amountRemaining = (populationMax * 2) - infrastructure;
						float amountBuildThisTurn = (InfrastructureAmount * 0.01f * ActualProduction) / 10;
						if (amountBuildThisTurn > amountRemaining) //Will put some into reserve
						{
							float difference = (amountBuildThisTurn - amountRemaining) * 5; //This is now BC for excess
							return string.Format("{0} (+{1:0.0}) Buildings (+{2:0.0} BC)", (int)infrastructure, amountRemaining, difference);
						}
						else
						{
							return string.Format("{0} (+{1:0.0}) Buildings", (int)infrastructure, amountBuildThisTurn);
						}
					}
				}
				else
				{
					return string.Format("{0} Buildings", (int)infrastructure);
				}
			}
		}
		public string ResearchStringOutput
		{
			get
			{
				if (ResearchAmount > 0)
				{
					return string.Format("{0:0.0} Research Points", (ResearchAmount * 0.01 * ActualProduction));
				}
				return "Not Researching";
			}
		}
		public string DefenseStringOutput
		{
			get
			{
				return "0 Bases";
			}
		}
		public string EnvironmentStringOutput
		{
			get
			{
				/*float increaseAmount = CalculateMaxPopGrowth();
				if (increaseAmount < 0)
				{
					return String.Format("Polluting ({0:0.00} Max Pop)", increaseAmount);
				}
				if (increaseAmount > 0.05f)
				{
					return String.Format("Terraforming ({0:0.00} Max Pop)", increaseAmount);
				}*/
				return "Clean";
			}
		}
		public string ConstructionStringOutput
		{
			get
			{
				float length = ShipConstructionLength;
				if (length < 0)
				{
					return ShipBeingBuilt.Name + " - No activity";
				}
				else if (length <= 1)
				{
					int amount = (int)(1.0f / length);
					if (amount == 1)
					{
						return ShipBeingBuilt.Name + " in 1 year";
					}
					else
					{
						return ShipBeingBuilt.Name + " x " + amount + " in 1 year";
					}
				}
				else
				{
					int turns = (int)length;
					if (length - turns > 0)
					{
						turns++;
					}
					return ShipBeingBuilt.Name + " in " + turns + " years";
				}
			}
		}

		public int ResearchAmount { get; private set; }
		public int DefenseAmount { get; private set; }
		public int InfrastructureAmount { get; private set; }
		public int EnvironmentAmount { get; private set; }
		public int ConstructionAmount { get; private set; }

		public bool InfrastructureLocked { get; set; }
		public bool EnvironmentLocked { get; set; }
		public bool ResearchLocked { get; set; }
		public bool DefenseLocked { get; set; }
		public bool ConstructionLocked { get; set; }
		#endregion

		#region Constructor
		public Planet(string name, Random r, StarSystem system)
		{
			whichSystem = system;
			this.name = name;
			races = new List<Race>();
			racePopulations = new Dictionary<Race, float>();
			TransferSystem = new KeyValuePair<TravelNode, int>(new TravelNode(), 0);

			isWetClimate = r.Next(2) == 0;
			populationMax = r.Next(0, 150) - 25;

			ConstructionBonus = PLANET_CONSTRUCTION_BONUS.AVERAGE;
			EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.AVERAGE;
			ResearchBonus = PLANET_RESEARCH_BONUS.AVERAGE;

			try //So I can use finally block
			{
				int industryBonusAdjustment = 0;
				int fertilityBonusAdjustment = 0;
				if (populationMax < 5)
				{
					planetType = PLANET_TYPE.NONE;
					SmallSprite = SpriteManager.GetSprite("AsteroidsPlanetSmall", r);
					populationMax = 0;
				}
				else if (populationMax <= 10)
				{
					planetType = PLANET_TYPE.RADIATED;
					SmallSprite = SpriteManager.GetSprite("RadiatedPlanetSmall", r);
					GroundSprite = SpriteManager.GetSprite("RadiatedGround", r);
					industryBonusAdjustment = 500;
				}
				else if (populationMax <= 15)
				{
					planetType = PLANET_TYPE.TOXIC;
					SmallSprite = SpriteManager.GetSprite("ToxicPlanetSmall", r);
					GroundSprite = SpriteManager.GetSprite("ToxicGround", r);
					industryBonusAdjustment = 300;
				}
				else if (populationMax <= 20)
				{
					planetType = isWetClimate ? PLANET_TYPE.ARCTIC : PLANET_TYPE.VOLCANIC;
					SmallSprite = isWetClimate ? SpriteManager.GetSprite("ArcticPlanetSmall", r) : SpriteManager.GetSprite("VolcanicPlanetSmall", r);
					GroundSprite = isWetClimate ? SpriteManager.GetSprite("ArcticGround", r) : SpriteManager.GetSprite("VolcanicGround", r);
					industryBonusAdjustment = 200;
				}
				else if (populationMax <= 30)
				{
					planetType = isWetClimate ? PLANET_TYPE.DEAD : PLANET_TYPE.BARREN;
					SmallSprite = isWetClimate ? SpriteManager.GetSprite("DeadPlanetSmall", r) : SpriteManager.GetSprite("BarrenPlanetSmall", r);
					GroundSprite = isWetClimate ? SpriteManager.GetSprite("DeadGround", r) : SpriteManager.GetSprite("BarrenGround", r);
					industryBonusAdjustment = 100;
				}
				else if (populationMax <= 40)
				{
					planetType = isWetClimate ? PLANET_TYPE.TUNDRA : PLANET_TYPE.BADLAND;
					SmallSprite = isWetClimate ? SpriteManager.GetSprite("TundraPlanetSmall", r) : SpriteManager.GetSprite("BadlandsPlanetSmall", r);
					GroundSprite = isWetClimate ? SpriteManager.GetSprite("TundraGround", r) : SpriteManager.GetSprite("BadlandsGround", r);
				}
				else if (populationMax <= 70)
				{
					planetType = isWetClimate ? PLANET_TYPE.OCEAN : PLANET_TYPE.DESERT;
					SmallSprite = isWetClimate ? SpriteManager.GetSprite("OceanicPlanetSmall", r) : SpriteManager.GetSprite("DesertPlanetSmall", r);
					GroundSprite = isWetClimate ? SpriteManager.GetSprite("OceanicGround", r) : SpriteManager.GetSprite("DesertGround", r);
					fertilityBonusAdjustment = 200;
				}
				else if (populationMax <= 90)
				{
					planetType = isWetClimate ? PLANET_TYPE.JUNGLE : PLANET_TYPE.STEPPE;
					SmallSprite = isWetClimate ? SpriteManager.GetSprite("JunglePlanetSmall", r) : SpriteManager.GetSprite("SteppePlanetSmall", r);
					GroundSprite = isWetClimate ? SpriteManager.GetSprite("JungleGround", r) : SpriteManager.GetSprite("SteppeGround", r);
					fertilityBonusAdjustment = 300;
				}
				else
				{
					planetType = PLANET_TYPE.TERRAN;
					SmallSprite = SpriteManager.GetSprite("TerranPlanetSmall", r);
					GroundSprite = SpriteManager.GetSprite("TerranGround", r);
					fertilityBonusAdjustment = 500;
				}

				//Get the construction bonus
				int number = r.Next(1000) + industryBonusAdjustment;
				if (!isWetClimate)
				{
					number += 25;
				}
				if (number < 50)
				{
					ConstructionBonus = PLANET_CONSTRUCTION_BONUS.DEARTH;
				}
				else if (number < 200)
				{
					ConstructionBonus = PLANET_CONSTRUCTION_BONUS.POOR;
				}
				else if (number >= 950)
				{
					ConstructionBonus = PLANET_CONSTRUCTION_BONUS.RICH;
				}
				else if (number >= 800)
				{
					ConstructionBonus = PLANET_CONSTRUCTION_BONUS.COPIOUS;
				}

				//Get the environment bonus
				number = r.Next(1000) + fertilityBonusAdjustment;
				if (isWetClimate)
				{
					number += 25;
				}
				if (number < 200)
				{
					EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.INFERTILE;
				}
				else if (number >= 950)
				{
					EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.LUSH;
				}
				else if (number >= 800)
				{
					EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.FERTILE;
				}

				//Get the research bonus
				number = r.Next(1000);
				if (number >= 950)
				{
					ResearchBonus = PLANET_RESEARCH_BONUS.EXCITING;
				}
				else if (number >= 800)
				{
					ResearchBonus = PLANET_RESEARCH_BONUS.SENSATIONAL;
				}
			}
			finally
			{
				planetTypeString = Utility.PlanetTypeToString(planetType);
			}
			RelocateToSystem = null;
		}
		#endregion

		public void SetHomeworld(Empire owner, Random r) //Set this planet as homeworld
		{
			this.owner = owner;
			planetType = PLANET_TYPE.TERRAN;
			planetTypeString = Utility.PlanetTypeToString(planetType);
			SmallSprite = SpriteManager.GetSprite("TerranPlanetSmall", r);
			populationMax = 100;
			races.Add(owner.EmpireRace);
			racePopulations.Add(owner.EmpireRace, 50.0f);
			infrastructure = 30;
			SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, 100, true);
			ResearchBonus = PLANET_RESEARCH_BONUS.AVERAGE;
			ConstructionBonus = PLANET_CONSTRUCTION_BONUS.AVERAGE;
			EnvironmentBonus = PLANET_ENVIRONMENT_BONUS.AVERAGE;
		}

		public void SetOutputAmount(OUTPUT_TYPE outputType, int amount, bool forceChange)
		{
			if (forceChange)
			{
				//First, try and change it without forcing it
				SetOutputAmount(outputType, amount, false);
				switch (outputType)
				{
					case OUTPUT_TYPE.CONSTRUCTION:
						{
							if (ConstructionAmount == amount)
							{
								//Success
								return;
							}
						} break;
					case OUTPUT_TYPE.DEFENSE:
						{
							if (DefenseAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.ENVIRONMENT:
						{
							if (EnvironmentAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.INFRASTRUCTURE:
						{
							if (InfrastructureAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
					case OUTPUT_TYPE.RESEARCH:
						{
							if (ResearchAmount == amount)
							{
								//Success
								return;
							}
						}
						break;
				}
			}
			
			int remainingPercentile = 100;
			if (InfrastructureLocked && !forceChange)
			{
				remainingPercentile -= InfrastructureAmount;
			}
			if (EnvironmentLocked && !forceChange)
			{
				remainingPercentile -= EnvironmentAmount;
			}
			if (DefenseLocked && !forceChange)
			{
				remainingPercentile -= DefenseAmount;
			}
			if (ConstructionLocked && !forceChange)
			{
				remainingPercentile -= ConstructionAmount;
			}
			if (ResearchLocked && !forceChange)
			{
				remainingPercentile -= ResearchAmount;
			}

			//if the player set the slider to or beyond the allowed percentile, all other sliders are set to 0
			if (amount >= remainingPercentile)
			{
				//set all sliders to 0, and change amount to remainingPercentile
				if (!InfrastructureLocked)
				{
					InfrastructureAmount = 0;
				}
				if (!EnvironmentLocked)
				{
					EnvironmentAmount = 0;
				}
				if (!DefenseLocked)
				{
					DefenseAmount = 0;
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
			int totalPointsExcludingSelectedType = GetPointsExcludingSelectedTypeAndLockedTypes(outputType);
			switch (outputType)
			{
				case OUTPUT_TYPE.INFRASTRUCTURE:
					{
						InfrastructureAmount = amount;
						remainingPercentile -= InfrastructureAmount;
					} break;
				case OUTPUT_TYPE.ENVIRONMENT:
					{
						EnvironmentAmount = amount;
						remainingPercentile -= EnvironmentAmount;
					} break;
				case OUTPUT_TYPE.DEFENSE:
					{
						DefenseAmount = amount;
						remainingPercentile -= DefenseAmount;
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
			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if ((!ResearchLocked || forceChange) && outputType != OUTPUT_TYPE.RESEARCH)
				{
					prevValue = ResearchAmount;
					ResearchAmount -= (ResearchAmount >= amountToDeduct ? amountToDeduct : ResearchAmount);
					amountToDeduct -= (prevValue - ResearchAmount);
				}
				if (amountToDeduct > 0)
				{
					if ((!ConstructionLocked || forceChange) && outputType != OUTPUT_TYPE.CONSTRUCTION)
					{
						prevValue = ConstructionAmount;
						ConstructionAmount -= (ConstructionAmount >= amountToDeduct ? amountToDeduct : ConstructionAmount);
						amountToDeduct -= (prevValue - ConstructionAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!DefenseLocked || forceChange) && outputType != OUTPUT_TYPE.DEFENSE)
					{
						prevValue = DefenseAmount;
						DefenseAmount -= (DefenseAmount >= amountToDeduct ? amountToDeduct : DefenseAmount);
						amountToDeduct -= (prevValue - DefenseAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!InfrastructureLocked || forceChange) && outputType != OUTPUT_TYPE.INFRASTRUCTURE)
					{
						prevValue = InfrastructureAmount;
						InfrastructureAmount -= (InfrastructureAmount >= amountToDeduct ? amountToDeduct : InfrastructureAmount);
						amountToDeduct -= (prevValue - InfrastructureAmount);
					}
				}
				if (amountToDeduct > 0)
				{
					if ((!EnvironmentLocked || forceChange) && outputType != OUTPUT_TYPE.ENVIRONMENT)
					{
						prevValue = EnvironmentAmount;
						EnvironmentAmount -= (EnvironmentAmount >= amountToDeduct ? amountToDeduct : EnvironmentAmount);
						amountToDeduct -= (prevValue - EnvironmentAmount);
					}
				}
			}
			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!InfrastructureLocked && outputType != OUTPUT_TYPE.INFRASTRUCTURE)
				{
					InfrastructureAmount += amountToAdd;
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
					if (!DefenseLocked && outputType != OUTPUT_TYPE.DEFENSE)
					{
						DefenseAmount += amountToAdd;
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
				if (amountToAdd > 0) //All other sliders has been locked, allocate the remaining points back to this output
				{
					switch (outputType)
					{
						case OUTPUT_TYPE.INFRASTRUCTURE:
								{
									InfrastructureAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.RESEARCH:
								{
									ResearchAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.ENVIRONMENT:
								{
									EnvironmentAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.DEFENSE:
								{
									DefenseAmount += amountToAdd;
									break;
								}
						case OUTPUT_TYPE.CONSTRUCTION:
								{
									ConstructionAmount += amountToAdd;
									break;
								}
					}
				}
			}
		}

		private int GetPointsExcludingSelectedTypeAndLockedTypes(OUTPUT_TYPE type)
		{
			int points = 0;
			if (type != OUTPUT_TYPE.ENVIRONMENT && !EnvironmentLocked)
			{
				points += EnvironmentAmount;
			}
			if (type != OUTPUT_TYPE.RESEARCH && !ResearchLocked)
			{
				points += ResearchAmount;
			}
			if (type != OUTPUT_TYPE.CONSTRUCTION && !ConstructionLocked)
			{
				points += ConstructionAmount;
			}
			if (type != OUTPUT_TYPE.DEFENSE && !DefenseLocked)
			{
				points += DefenseAmount;
			}
			if (type != OUTPUT_TYPE.INFRASTRUCTURE && !InfrastructureLocked)
			{
				points += InfrastructureAmount;
			}
			return points;
		}

		public void SetCleanup()
		{
			//Waste Processing uses the formula: (Number of buildings * 0.5 * lowest pollution tech percentage) / production * 100
			float wasteAmount = (infrastructure * 0.5f); //add tech improvements and racial bonuses/negatives inside (3.0f)
			float amountOfProductionUsed = (wasteAmount / ActualProduction) * 100;
			int percentage = (int)amountOfProductionUsed + ((amountOfProductionUsed - (int)amountOfProductionUsed) > 0 ? 1 : 0);
			
			if (EnvironmentAmount < percentage)
			{
				//Only set it if it's below the desired amount
				SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, percentage, true);
			}
		}

		//Used for end of turn processing
		public void UpdatePlanet()
		{
			//Update ship construction
			if (ConstructionAmount > 0)
			{
				constructionTotal += ConstructionAmount * 0.01f * ActualProduction;
			}
			if (InfrastructureAmount > 0)
			{
				if (infrastructure < populationMax * 2)
				{
					float remaining = (populationMax * 2) - infrastructure;
					float amountToBuild = (InfrastructureAmount * 0.01f * ActualProduction) / 10;
					if (amountToBuild > remaining)
					{
						infrastructure += remaining;
						amountToBuild -= remaining;
						//TODO: add BC
					}
					else
					{
						infrastructure += amountToBuild;
					}
				}
				else
				{
					//TODO: add BC
				}
			}

			//Calculate normal population growth using formula (rate of growth * population) * (1 - (population / planet's capacity)) with foodModifier in place of 1
			foreach (Race race in races)
			{
				racePopulations[race] += (racePopulations[race] * (0.05f)) * (1 - (TotalPopulation / PopulationMax));
			}

			races.Sort((Race a, Race b) => { return (racePopulations[a].CompareTo(racePopulations[b])); });
		}

		public Ship CheckIfShipBuilt(out int amount)
		{
			if (constructionTotal >= shipBeingBuilt.Cost)
			{
				amount = 0;
				while (constructionTotal >= shipBeingBuilt.Cost)
				{
					amount++;
					constructionTotal -= shipBeingBuilt.Cost;
				}
				return shipBeingBuilt;
			}
			amount = 0;
			return null;
		}

		private float CalculateTotalPopGrowth()
		{
			//Calculate normal population growth using formula (rate of growth * population) * (1 - (population / planet's capacity)) with foodModifier in place of 1
			float popGrowth = 0;
			foreach (Race race in races)
			{
				popGrowth += (racePopulations[race] * (0.05f)) * (1 - (TotalPopulation / PopulationMax));
			}

			return popGrowth;
		}

		private float CalculateOptimalCleanEffort()
		{
			return TotalPopulation / 2;
		}

		public float GetRacePopulation(Race whichRace)
		{
			return racePopulations[whichRace];
		}

		public void AddRacePopulation(Race whichRace, float amount)
		{
			racePopulations[whichRace] += amount;
		}

		public void RemoveRacePopulation(Race whichRace, float amount)
		{
			racePopulations[whichRace] -= amount;
		}

		public void RemoveRace(Race whichRace)
		{
			racePopulations.Remove(whichRace);
			races.Remove(whichRace);
		}

		public void Colonize(Empire whichEmpire)
		{
			owner = whichEmpire;
			racePopulations = new Dictionary<Race, float>();
			racePopulations.Add(whichEmpire.EmpireRace, 2);
			races.Add(whichEmpire.EmpireRace);
			SetOutputAmount(OUTPUT_TYPE.INFRASTRUCTURE, 100, true);
			shipBeingBuilt = whichEmpire.FleetManager.CurrentDesigns[0];
			System.UpdateOwners();
		}
	}
}
