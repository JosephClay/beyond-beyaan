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
		public const int ZORTRIUM_ARMOR = 3;
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
		public const int VOLCANIC_COLONY = 5;
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

		//Space/Cost/Power requirements for ship.  Generic means ship's size don't matter
		public float SmallSize { get; private set; }
		public float SmallCost { get; private set; }
		public float SmallPower { get; private set; }
		public float SmallHP { get; private set; } //Armor points for ship

		public float MediumSize { get; private set; }
		public float MediumCost { get; private set; }
		public float MediumPower { get; private set; }
		public float MediumHP { get; private set; }

		public float LargeSize { get; private set; }
		public float LargeCost { get; private set; }
		public float LargePower { get; private set; }
		public float LargeHP { get; private set; }

		public float HugeSize { get; private set; }
		public float HugeCost { get; private set; }
		public float HugePower { get; private set; }
		public float HugeHP { get; private set; }

		public float GenericSize { get; private set; }
		public float GenericCost { get; private set; }
		public float GenericPower { get; private set; }

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
						bool displacementDevice = false,
						float smallSize = 0,
						float smallCost = 0,
						float smallPower = 0,
						float smallHP = 0,
						float mediumSize = 0,
						float mediumCost = 0,
						float mediumPower = 0,
						float mediumHP = 0,
						float largeSize = 0,
						float largeCost = 0,
						float largePower = 0,
						float largeHP = 0,
						float hugeSize = 0,
						float hugeCost = 0,
						float hugePower = 0,
						float hugeHP = 0,
						float genericSize = 0,
						float genericCost = 0,
						float genericPower = 0
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
			Stargate = stargate;

			//Ship component info
			SmallSize = smallSize;
			SmallCost = smallCost;
			SmallPower = smallPower;
			SmallHP = smallHP;
			MediumSize = mediumSize;
			MediumCost = mediumCost;
			MediumPower = mediumPower;
			MediumHP = mediumHP;
			LargeSize = largeSize;
			LargeCost = largeCost;
			LargePower = largePower;
			LargeHP = largeHP;
			HugeSize = hugeSize;
			HugeCost = hugeCost;
			HugePower = hugePower;
			HugeHP = hugeHP;
			GenericSize = genericSize;
			GenericCost = genericCost;
			GenericPower = genericPower;
		}
	}
}
