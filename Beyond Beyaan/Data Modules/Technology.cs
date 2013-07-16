﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	public class Technology
	{
		#region Constants
		public const int DEEP_SPACE_SCANNER = 1;
		public const int IMPROVED_SPACE_SCANNER = 2;
		public const int ADVANCED_SPACE_SCANNER = 3;

		public const int TITANIUM_ARMOR = 1;
		public const int DURALLOY_ARMOR = 2;
		public const int ZORITUM_ARMOR = 3;
		public const int ANDRIUM_ARMOR = 4;
		public const int TRITANIUM_ARMOR = 5;
		public const int ADAMANTIUM_ARMOR = 6;
		public const int NEUTRONIUM_ARMOR = 7;

		public const int BATTLE_SUITS = 1;
		public const int ARMORED_EXOSKELETON = 2;
		public const int POWERED_ARMOR = 3;

		public const int PERSONAL_DEFLECTOR = 1;
		public const int PERSONAL_ABSORPTION = 2;
		public const int PERSONAL_BARRIER = 3;

		public const int AUTOMATED_REPAIR = 1;
		public const int ADVANCED_REPAIR = 2;

		public const int PLANETARY_V_SHIELD = 1;
		public const int PLANETARY_X_SHIELD = 2;
		public const int PLANETARY_XV_SHIELD = 3;
		public const int PLANETARY_XX_SHIELD = 4;

		public const int ZYRO_SHIELD = 1;
		public const int LIGHTNING_SHIELD = 2;

		public const int STANDARD_COLONY = 1;
		public const int BARREN_COLONY = 2;
		public const int TUNDRA_COLONY = 3;
		public const int DEAD_COLONY = 4;
		public const int INFERNO_COLONY = 5;
		public const int TOXIC_COLONY = 6;
		public const int RADIATED_COLONY = 7;

		public const int DEATH_SPORES = 1;
		public const int DOOM_VIRUS = 2;
		public const int BIO_TERMINATOR = 3;

		public const int BIO_TOXIN_ANTIDOTE = 1;
		public const int UNIVERSAL_ANTIDOTE = 2;

		public const int SOIL_ENRICHMENT = 1;
		public const int ADV_SOIL_ENRICHMENT = 2;
		#endregion

		public int TechLevel { get; private set; }
		public string TechName { get; private set; }
		public string TechDescription { get; private set; }
		public int ResearchPoints
		{
			get { return TechLevel * TechLevel; }
		}

		public int RoboticControl { get; private set; }
		public int BattleComputer { get; private set; }
		public int ECM { get; private set; }
		public int SpaceScanner { get; private set; }
		public int Armor { get; private set; }
		public int IndustrialTech { get; private set; }
		public int IndustrialWaste { get; private set; }
		public int GroundArmor { get; private set; }
		public int Repair { get; private set; }
		public int Shield { get; private set; }
		public int PersonalShield { get; private set; }
		public int PlanetaryShield { get; private set; }
		public int MissileShield { get; private set; }
		public int EcoCleanup { get; private set; }
		public int Terraforming { get; private set; }
		public int Colony { get; private set; }
		public int Cloning { get; private set; }
		public int BioWeapon { get; private set; }
		public int BioAntidote { get; private set; }
		public int Enrichment { get; private set; }
		public int Speed { get; private set; }
		public int ManeuverSpeed { get; private set; }
		public int FuelRange { get; private set; }

		public bool ReserveFuelTanks { get; private set; }
		public bool BattleScanner { get; private set; }
		public bool HyperSpaceCommunicator { get; private set; }
		public bool OracleInterface { get; private set; }
		public bool TechnologyNullifier { get; private set; }
		public bool RepulsorBeam { get; private set; }
		public bool CloakingDevice { get; private set; }
		public bool StatisField { get; private set; }
		public bool BlackHoleGenerator { get; private set; }
		public bool InertialStabilizer { get; private set; }
		public bool EnergyPulsar { get; private set; }
		public bool WarpDissipator { get; private set; }
		public bool HighEnergyFocus { get; private set; }
		public bool Stargate { get; private set; }
		public bool SubspaceTeleporter { get; private set; }
		public bool IonicPulsar { get; private set; }
		public bool SubspaceInterdictor { get; private set; }
		public bool CombatTransporters { get; private set; }
		public bool InertialNullifier { get; private set; }
		public bool DisplacementDevice { get; private set; }

		public Technology(string name, string desc, int level,
						//Optional arguments goes here
						int roboticControl = 0,
						int battleComputer = 0,
						bool battleScanner = false,
						int ECM = 0,
						int spaceScanner = 0,
						bool hyperSpaceCommunicator = false,
						bool oracleInterface = false,
						bool technologyNullifier = false,
						int armor = 0,
						bool reserveFuelTanks = false,
						int industrialTech = 10,
						int industrialWaste = 100,
						int groundArmor = 0,
						int repair = 0,
						int shield = 0,
						int personalShield = 0,
						int planetaryShield = 0,
						bool repulsorBeam = false,
						bool cloakingDevice = false,
						int missileShield = 0,
						bool statisField = false,
						bool blackHoleGenerator = false,
						int ecoCleanup = 0,
						int terraforming = 0,
						int colony = 0,
						int cloning = 20,
						int bioWeapon = 0,
						int bioAntidote = 0,
						int enrichment = 0,
						int speed = 0,
						int maneuverSpeed = 0,
						int fuelRange = 0,
						bool inertialstabilizer = false,
						bool energypulsar = false,
						bool warpDissipator = false,
						bool highEnergyFocus = false,
						bool stargate = false,
						bool subSpaceTeleporter = false,
						bool ionicPulsar = false,
						bool subspaceInterdictor = false,
						bool combatTransporters = false,
						bool inertialNullifier = false,
						bool displacementDevice = false
						)
		{
			TechLevel = level;
			TechName = name;
			TechDescription = desc;
			RoboticControl = roboticControl;
			BattleComputer = battleComputer;
			BattleScanner = battleScanner;
			this.ECM = ECM;
			SpaceScanner = spaceScanner;
			HyperSpaceCommunicator = hyperSpaceCommunicator;
			OracleInterface = oracleInterface;
			TechnologyNullifier = technologyNullifier;
			Armor = armor;
			ReserveFuelTanks = reserveFuelTanks;
			IndustrialTech = industrialTech;
			IndustrialWaste = industrialWaste;
			GroundArmor = groundArmor;
			Repair = repair;
			Shield = shield;
			PersonalShield = personalShield;
			PlanetaryShield = planetaryShield;
			RepulsorBeam = repulsorBeam;
			CloakingDevice = cloakingDevice;
			MissileShield = missileShield;
			StatisField = statisField;
			BlackHoleGenerator = blackHoleGenerator;
			EcoCleanup = ecoCleanup;
			Terraforming = terraforming;
			Colony = colony;
			Cloning = cloning;
			BioWeapon = bioWeapon;
			BioAntidote = bioAntidote;
			Enrichment = enrichment;
			Speed = speed;
			ManeuverSpeed = maneuverSpeed;
			FuelRange = fuelRange;
			InertialStabilizer = inertialstabilizer;
			EnergyPulsar = energypulsar;
			WarpDissipator = warpDissipator;
			HighEnergyFocus = highEnergyFocus;
			SubspaceTeleporter = subSpaceTeleporter;
			IonicPulsar = ionicPulsar;
			SubspaceInterdictor = subspaceInterdictor;
			CombatTransporters = combatTransporters;
			InertialNullifier = inertialNullifier;
			DisplacementDevice = displacementDevice;
		}
	}

	/*public class Beam : Technology
	{
		private int baseDamage;
		private int accuracy;
		private int space;
		private int cost;
		public Beam()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["accuracy"], out accuracy))
			{
				reason = "accuracy value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"accuracy",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetAccuracy()
		{
			//this will factor in tech level increase
			return accuracy;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}
		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Particle : Technology
	{
		private int baseDamage;
		private int accuracy;
		private int space;
		private int cost;
		public Particle()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["accuracy"], out accuracy))
			{
				reason = "accuracy value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"accuracy",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetAccuracy()
		{
			//this will factor in tech level increase
			return accuracy;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Torpedo : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Torpedo()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Missile : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Missile()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Bomb : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Bomb()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			return space;
		}

		public int GetCost()
		{
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Armor : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;
		private float baseHP;

		public Armor()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not int";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!float.TryParse(items["basehp"], out baseHP))
			{
				reason = "basehp value is not float";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"basehp",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetHP(int size)
		{
			return (int)(size * (baseHP / 100.0f));
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Shield : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;
		private int baseResistance;

		public Shield()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["baseresistance"], out baseResistance))
			{
				reason = "baseresistance value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"baseresistance",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetResistance()
		{
			return baseResistance;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Computer : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;

		public Computer()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not int";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Engine : Technology
	{
		private int space;
		private int cost;
		private int baseGalaxySpeed;
		private int baseCombatSpeed;

		public Engine()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["basegalaxyspeed"], out baseGalaxySpeed))
			{
				reason = "basegalaxyspeed value is not integer";
				return false;
			}
			if (!int.TryParse(items["basecombatspeed"], out baseCombatSpeed))
			{
				reason = "basecombatspeed value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"space",
				"cost",
				"basegalaxyspeed",
				"basecombatspeed",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetGalaxySpeed()
		{
			return baseGalaxySpeed;
		}

		public int GetCombatSpeed()
		{
			return baseCombatSpeed;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Special : Technology
	{
		private int space;
		private int cost;

		public Special()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	public class Infrastructure : Technology
	{
		public Infrastructure()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}*/
}
