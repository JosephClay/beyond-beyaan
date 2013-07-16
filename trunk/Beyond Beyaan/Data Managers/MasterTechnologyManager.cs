using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Managers
{
	public class MasterTechnologyManager
	{
		private GameMain _gameMain;

		public List<Technology> ComputerTechs { get; private set; }
		public List<Technology> ConstructionTechs { get; private set; }
		public List<Technology> ForceFieldTechs { get; private set; }
		public List<Technology> PlanetologyTechs { get; private set; }
		public List<Technology> PropulsionTechs { get; private set; }
		public List<Technology> WeaponTechs { get; private set; }

		#region Initialization functions
		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;

			//Later we'll replace with data files, but for now, all techs are hard-coded
			ComputerTechs = new List<Technology>();
			ConstructionTechs = new List<Technology>();
			ForceFieldTechs = new List<Technology>();
			PlanetologyTechs = new List<Technology>();
			PropulsionTechs = new List<Technology>();
			WeaponTechs = new List<Technology>();

			LoadComputerTechs(20);
			LoadConstructionTechs(20);
			LoadForceFieldTechs(20);
			LoadPlanetologyTechs(20);
			LoadPropulsionTechs(20);
			LoadWeaponTechs(20);

			reason = null;
			return true;
		}

		private void LoadComputerTechs(int diffModifier)
		{
			ComputerTechs.Add(new Technology("Robotic Controls 2", "Controls 2 Buildings per population", 1, roboticControl: 2));
			ComputerTechs.Add(new Technology("Battle Computer Mark I", "Level 1 targeting computer", 1, battleComputer: 1));
			ComputerTechs.Add(new Technology("Battle Scanner", "Displays enemy ship stats, adds +3 initiative to the equipped ship, and adds 1 to the targeting system of the equipped computer.", 1, battleScanner: true));
			ComputerTechs.Add(new Technology("ECM Jammer Mark 1", "Adds 1 level to the missile defense rating of any ship that has it equipped.", 2, ECM: 1));
			ComputerTechs.Add(new Technology("Deep Space Scanner", "This technology allows your colonies to detect ships within a distance of 5 from each of your colonies and allows your ships to detect enemy ships at a distance of 1.", 4, spaceScanner: Technology.DEEP_SPACE_SCANNER));
			ComputerTechs.Add(new Technology("Battle Computer Mark II", "Increases attack rating by two.", 5, battleComputer: 2));
			ComputerTechs.Add(new Technology("ECM Jammer Mark II", "Increases missile defense rating by two.", 7, ECM: 2));
			ComputerTechs.Add(new Technology("Improved Robotic Controls III", "Allows each population unit to control 3 buildings.", 8, roboticControl: 3));
			ComputerTechs.Add(new Technology("Battle Computer Mark III", "Increases attack rating by three.", 10, battleComputer: 3));
			ComputerTechs.Add(new Technology("ECM Jammer Mark III", "Increases missile defense rating by three.", 12, ECM: 3));
			ComputerTechs.Add(new Technology("Improved Space Scanner", "Allows your colonies to detect enemy ships at a distance of 7 and allows your ships to detect enemy ships at a distance of 2. These scanners also allow you to determine the planet the enemy ships are travelling to and their ETA.", 13, spaceScanner: Technology.IMPROVED_SPACE_SCANNER));
			ComputerTechs.Add(new Technology("Battle Computer Mark IV", "Increases attack rating by four.", 15, battleComputer: 4));
			ComputerTechs.Add(new Technology("ECM Jammer Mark IV", "Increases missile defense rating by four.", 17, ECM: 4));
			ComputerTechs.Add(new Technology("Improved Robotic Controls IV", "Allows each population unit to control 4 buildings.", 18, roboticControl: 4));
			ComputerTechs.Add(new Technology("Battle Computer Mark V", "Increases attack rating by five.", 20, battleComputer: 5));
			ComputerTechs.Add(new Technology("ECM Jammer Mark V", "Increases missile defense rating by five.", 22, ECM: 5));
			ComputerTechs.Add(new Technology("Advanced Space Scanner", "Allows your colonies to detect enemy ships at a distance of 9 and allows your ships to detect enemy ships at a distance of 3. Allows you to determine planet size, environment, habitability, and resources at a distance of 9 on the galactic map without having a ship in orbit over the colony.", 23, spaceScanner: Technology.ADVANCED_SPACE_SCANNER));
			ComputerTechs.Add(new Technology("Battle Computer Mark VI", "Increases attack rating by six.", 25, battleComputer: 6));
			ComputerTechs.Add(new Technology("ECM Jammer Mark VI", "Increases missile defense rating by six.", 27, ECM: 6));
			ComputerTechs.Add(new Technology("Improved Robotic Controls V", "Allows each population unit to control 5 buildings.", 28, roboticControl: 5));
			ComputerTechs.Add(new Technology("Battle Computer Mark VII", "Increases attack rating by seven.", 30, battleComputer: 7));
			ComputerTechs.Add(new Technology("ECM Jammer Mark VII", "Increases missile defense rating by 7.", 32, ECM: 7));
			ComputerTechs.Add(new Technology("Hyperspace Communications", "Allows you to change destination orders of your ships in transit.", 34, hyperSpaceCommunicator: true));
			ComputerTechs.Add(new Technology("Battle Computer Mark VIII", "Increases attack rating by 8.", 35, battleComputer: 8));
			ComputerTechs.Add(new Technology("ECM Jammer Mark VIII", "Increases missile defense rating by 8.", 37, ECM: 8));
			ComputerTechs.Add(new Technology("Improved Robotic Controls VI", "Allows each population unit to control 6 buildings.", 38, roboticControl: 6));
			ComputerTechs.Add(new Technology("Battle Computer Mark IX", "Increases attack rating by 9.", 40, battleComputer: 9));
			ComputerTechs.Add(new Technology("ECM Jammer Mark IX", "Increases missile defense rating by 9.", 42, ECM: 9));
			ComputerTechs.Add(new Technology("Battle Computer Mark X", "Increases attack rating by 10.", 45, battleComputer: 10));
			ComputerTechs.Add(new Technology("Oracle Interface", "Allows all \"direct fire\" weapons on the equipped ship to act as if they have the \"armor piercing\" quality. This includes pretty much everything except missiles, torpedoes, bombs, and anything that counts as a special system.", 46, oracleInterface: true));
			ComputerTechs.Add(new Technology("ECM Jammer Mark X", "Increases missile defense rating by 10.", 47, ECM: 10));
			ComputerTechs.Add(new Technology("Improved Robotic Controls VII", "Allows each population unit to control 7 factories.", 48, roboticControl: 7));
			ComputerTechs.Add(new Technology("Technology Nullifier", "Decreases the enemy's attack rating by 2 - 6 each time it is fired.", 49, technologyNullifier: true));
			ComputerTechs.Add(new Technology("Battle Computer Mark XI", "Increases attack rating by 11.", 50, battleComputer: 11));
		}

		private void LoadConstructionTechs(int diffModifier)
		{
			ConstructionTechs.Add(new Technology("Reserve Fuel Tanks", "Extends the range of a ship by an additional 3 parsecs", 1, reserveFuelTanks: true));
			ConstructionTechs.Add(new Technology("Titanium Armor", "Standard armor for ships and missile bases. Gives small ships 3 hit points, medium ships 18 hit points, large ships 100 hit points, and huge ships 600 hit points. Gives missile bases 15 hit points.", 1, armor: Technology.TITANIUM_ARMOR));
			ConstructionTechs.Add(new Technology("Industrial Tech 9", "Reduces the cost to construct a factory to 9 BCs.", 3, industrialTech: 9));
			ConstructionTechs.Add(new Technology("Reduced Industrial Waste 80%", "Reduces the pollution by factories to 80% of its original amount.", 5, industrialWaste:80));
			ConstructionTechs.Add(new Technology("Industrial Tech 8", "Reduces the cost to construct a factory to 8 BCs.", 8, industrialTech: 8));
			ConstructionTechs.Add(new Technology("Duralloy Armor", "Increases the armor protection on ships that equip it by 50% (rounded down). Adds 5 to ground combat rolls. Replaces titanium armor.", 10, armor: Technology.DURALLOY_ARMOR));
			ConstructionTechs.Add(new Technology("Battle Suits", "Adds 10 to ground combat rolls.", 11, groundArmor: Technology.BATTLE_SUITS));
			ConstructionTechs.Add(new Technology("Industrial Tech 7", "Reduces the cost to construct a factory to 7 BCs.", 13, industrialTech: 7));
			ConstructionTechs.Add(new Technology("Automated Repair Unit", "Repairs each equipped ship 15% of its maximum hit points per turn.", 14, repair: Technology.AUTOMATED_REPAIR));
			ConstructionTechs.Add(new Technology("Reduced Industrial Waste 60%", "Reduces the pollution by factories to 60% of its original amount.", 15, industrialWaste: 60));
			ConstructionTechs.Add(new Technology("Zoritum Armor", "Increases the armor protection on ships that equip it by 100%. Adds 10 to ground combat rolls. Replaces any other kind of armor on ground combat troops that is lower than itself.", 17, armor: Technology.ZORITUM_ARMOR));
			ConstructionTechs.Add(new Technology("Industrial Tech 6", "Reduces the cost to construct a factory to 6 BCs.", 18, industrialTech: 6));
			ConstructionTechs.Add(new Technology("Industrial Tech 5", "Reduces the cost to construct a factory to 5 BCs.", 23, industrialTech: 5));
			ConstructionTechs.Add(new Technology("Armored Exoskeleton", "Adds 20 to ground combat rolls. Replaces Battle Suits.", 24, groundArmor: Technology.ARMORED_EXOSKELETON));
			ConstructionTechs.Add(new Technology("Reduced Industrial Waste 40%", "Reduces pollution by factories to 40% of its original amount.", 25, industrialWaste: 40));
			ConstructionTechs.Add(new Technology("Andrium Armor", "Increases the hit points of equipped ships by 150% of the original amount (round down). Adds 15 to ground combat rolls. Replaces any other kind of armor on ground combat troops that is lower than itself.", 26, armor: Technology.ANDRIUM_ARMOR));
			ConstructionTechs.Add(new Technology("Industrial Tech 4", "Reduces the cost to construct a factory to 4 BCs.", 28, industrialTech: 4));
			ConstructionTechs.Add(new Technology("Industrial Tech 3", "Reduces the cost to construct a factory to 3 BCs.", 33, industrialTech: 3));
			ConstructionTechs.Add(new Technology("Tritanium Armor", "Increases the hit points of equipped ships by 200% of the original amount. Adds 20 to ground combat rolls. Replaces any other kind of armor on ground combat troops that is lower than itself.", 34, armor: Technology.TRITANIUM_ARMOR));
			ConstructionTechs.Add(new Technology("Reduced Industrial Waste 20%", "Reduces pollution by factories to 20% of its original amount.", 35, industrialWaste: 20));
			ConstructionTechs.Add(new Technology("Advanced Damage Control Unit", "Repairs each equipped ship 30% of its maximum hit points per turn.", 36, repair: Technology.ADVANCED_REPAIR));
			ConstructionTechs.Add(new Technology("Industrial Tech 2", "Reduces the cost to construct a factory to 2 BCs.", 38, industrialTech: 2));
			ConstructionTechs.Add(new Technology("Powered Armor", "Adds 30 to ground combat rolls. Replaces Battle Suits and Armored Exoskeleton.", 40, groundArmor: Technology.POWERED_ARMOR));
			ConstructionTechs.Add(new Technology("Adamantium Armor", "Increases the hit points of equipped ships by 250% of the original amount (round down). Adds 25 to ground combat rolls. Replaces any other kind of armor on ground combat troops that is lower than itself.", 42, armor: Technology.ADAMANTIUM_ARMOR));
			ConstructionTechs.Add(new Technology("Industrial Waste Elimination", "Reduces pollution by factories to 0% of its original amount. Completely eliminates all pollution by factories owned.", 45, industrialWaste: 0));
			ConstructionTechs.Add(new Technology("Neutronium Armor", "Increases the hit points of equipped ships by 300% of the original amount. Adds 30 to ground combat rolls. Replaces any other kind of armor on ground combat troops that is lower than itself.", 50, armor: Technology.NEUTRONIUM_ARMOR));
		}

		private void LoadForceFieldTechs(int diffModifier)
		{
			ForceFieldTechs.Add(new Technology("Class I Deflector Shield", "Reduces all damage to equipped targets by 1.", 1, shield: 1));
			ForceFieldTechs.Add(new Technology("Class II Deflector Shield", "Reduces all damage to equipped targets by 2.", 4, shield: 2));
			ForceFieldTechs.Add(new Technology("Personal Deflector Shield", "Adds 10 to ground combat rolls.", 8, personalShield: Technology.PERSONAL_DEFLECTOR));
			ForceFieldTechs.Add(new Technology("Class III Deflector Shield", "Reduces all damage to equipped targets by 3.", 10, shield: 3));
			ForceFieldTechs.Add(new Technology("Class V Planetary Shield", "Reduces all damage done to planetary missile bases by 5 when constructed.", 12, planetaryShield: Technology.PLANETARY_V_SHIELD));
			ForceFieldTechs.Add(new Technology("Class IV Deflector Shield", "Reduces all damage to equipped targets by 4.", 14, shield: 4));
			ForceFieldTechs.Add(new Technology("Repulsor Beam", "Pushes away all ships who come within 1 space of the equipped ship, keeping them at a distance of at least 2 in the combat grid.", 16, repulsorBeam: true));
			ForceFieldTechs.Add(new Technology("Class V Deflector Shield", "Reduces all damage to equipped targets by 5.", 20, shield: 5));
			ForceFieldTechs.Add(new Technology("Personal Absorption Shield", "Adds 20 to ground combat rolls. Replaces Personal Deflector Shield.", 21, personalShield: Technology.PERSONAL_ABSORPTION));
			ForceFieldTechs.Add(new Technology("Class X Planetary Shield", "Reduces all damage done to planetary missile bases by 10 when constructed.", 22, planetaryShield: Technology.PLANETARY_X_SHIELD));
			ForceFieldTechs.Add(new Technology("Class VI Deflector Shield", "Reduces all damage to equipped targets by 6.", 24, shield: 6));
			ForceFieldTechs.Add(new Technology("Cloaking Device", "When activated, adds 5 to the defense rating of equipped ships. Must be deactivated to fire weapons. Cannot be re-activated until the equipped ship ends a turn without firing any weapons.", 27, cloakingDevice: true));
			ForceFieldTechs.Add(new Technology("Class VII Deflector Shield", "Reduces all damage to equipped targets by 7.", 30, shield: 7));
			ForceFieldTechs.Add(new Technology("Zyro Shield", "Has a 75% chance per missile to destroy all missiles connecting with an equipped ship minus 1% per technology level of the missile.", 31, missileShield: Technology.ZYRO_SHIELD));
			ForceFieldTechs.Add(new Technology("Class XV Planetary Shield", "Reduces all damage done to planetary missile bases by 15 when constructed.", 32, planetaryShield: Technology.PLANETARY_XV_SHIELD));
			ForceFieldTechs.Add(new Technology("Class IX Deflector Shield", "Reduces all damage to equipped targets by 9.", 34, shield: 9));
			ForceFieldTechs.Add(new Technology("Stasis Field", "Removes the target from combat completely for 1 turn. Combat proceeds as if the target didn't exist.", 37, statisField: true));
			ForceFieldTechs.Add(new Technology("Personal Barrier Shield", "Adds 30 to all ground combat rolls. Replaces Personal Deflector Shield and Personal Absorbtion Shield.", 38, personalShield: Technology.PERSONAL_BARRIER));
			ForceFieldTechs.Add(new Technology("Class XI Deflector Shield", "Reduces all damage to equipped targets by 11.", 40, shield: 11));
			ForceFieldTechs.Add(new Technology("Class XX Planetary Shield", "Reduces all damage done to planetary missile bases by 20 when constructed.", 42, planetaryShield: Technology.PLANETARY_XX_SHIELD));
			ForceFieldTechs.Add(new Technology("Black Hole Generator", "Creates a black hole that instantly sucks in enemy targets. Completely destroys 100% of enemy targets minus 2% per Shield Class of the target.", 43, blackHoleGenerator: true));
			ForceFieldTechs.Add(new Technology("Class XIII Deflector Shield", "Reduces all damage to equipped targets by 13.", 44, shield: 13));
			ForceFieldTechs.Add(new Technology("Lightning Shield", "Has a 100% chance per missile to destroy all missiles connecting with an equipped ship minus 1% per technology level of the missile.", 46, missileShield: Technology.LIGHTNING_SHIELD));
			ForceFieldTechs.Add(new Technology("Class XV Deflector Shield", "Reduces all damage to equipped targets by 15.", 50, shield: 15));

		}

		private void LoadPlanetologyTechs(int diffModifier)
		{
			PlanetologyTechs.Add(new Technology("Standard Colony Base", "Allows ships equipped with this to sacrifice themselves to create a colony.", 1, colony: Technology.STANDARD_COLONY));
			PlanetologyTechs.Add(new Technology("Ecological Restoration", "Eliminates 2 units of pollution for 1 BC.", 1, ecoCleanup: 2));
			PlanetologyTechs.Add(new Technology("Terraforming +10", "Increases maximum colonist units on the terraformed planet by 10, for a cost of 50 BC.", 2, terraforming: 10));
			PlanetologyTechs.Add(new Technology("Controlled Barren Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a barren or better environment.", 3, colony: Technology.BARREN_COLONY));
			PlanetologyTechs.Add(new Technology("Improved Ecological Restoration", "Eliminates 3 units of pollution for 1 BC.", 5, ecoCleanup: 3));
			PlanetologyTechs.Add(new Technology("Controlled Tundra Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a tundra or better environment.", 6, colony: Technology.TUNDRA_COLONY));
			PlanetologyTechs.Add(new Technology("Terraforming +20", "Increases maximum colonist units on the terraformed planet by 20, for a cost of 100 BC.", 9, terraforming: 20));
			PlanetologyTechs.Add(new Technology("Controlled Dead Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a dead or better environment.", 10, colony: Technology.DEAD_COLONY));
			PlanetologyTechs.Add(new Technology("Death Spores", "Ship weapon that reduces the maximum population of a planet by 1 each time it is fired. This is not affected by shields.", 11, bioWeapon: Technology.DEATH_SPORES));
			PlanetologyTechs.Add(new Technology("Controlled Inferno Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a inferno or better environment.", 12, colony: Technology.INFERNO_COLONY));
			PlanetologyTechs.Add(new Technology("Enhanced Ecological Restoration", "Eliminates 5 units of pollution for 1 BC.", 13, ecoCleanup: 5));
			PlanetologyTechs.Add(new Technology("Terraforming +30", "Increases maximum colonist units on the terraformed planet by 30, for a cost of 150 BC.", 14, terraforming: 30));
			PlanetologyTechs.Add(new Technology("Controlled Toxic Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a Toxic or better environment.", 15, colony: Technology.TOXIC_COLONY));
			PlanetologyTechs.Add(new Technology("Soil Enrichment", "Converts normal environments to fertile environments for a cost of 150 BC. Increases the base size of the planet by 25%.", 16, enrichment: Technology.SOIL_ENRICHMENT));
			PlanetologyTechs.Add(new Technology("Bio Toxin Antidote", "Reduces the damage to population from all death weapons by 1.", 17, bioAntidote: Technology.BIO_TOXIN_ANTIDOTE));
			PlanetologyTechs.Add(new Technology("Controlled Radiated Environment", "Allows ships equipped with this to sacrifice themselves to create a colony on a planet with a radiated or better environment.", 18, colony: Technology.RADIATED_COLONY));
			PlanetologyTechs.Add(new Technology("Terraforming +40", "Increases maximum colonist units on the terraformed planet by 40, for a cost of 200 BC.", 20, terraforming: 40));
			PlanetologyTechs.Add(new Technology("Cloning", "Reduces the cost to create one unit of population to 10 BC.", 21, cloning: 10));
			PlanetologyTechs.Add(new Technology("Atmospheric Terraforming", "Converts hostile environments to normal environments for a cost of 150 BCs.", 22));
			PlanetologyTechs.Add(new Technology("Advanced Ecological Restoration", "Eliminates 10 units of pollution for 1 BC.", 24, ecoCleanup: 10));
			PlanetologyTechs.Add(new Technology("Terraforming +50", "Increases maximum colonist units on the terraformed planet by 50, for a cost of 250 BC.", 26, terraforming: 50));
			PlanetologyTechs.Add(new Technology("Doom Virus", "Ship weapon that reduces the maximum population of a planet by 2 each time it is fired. This is not affected by shields.", 27, bioWeapon: Technology.DOOM_VIRUS));
			PlanetologyTechs.Add(new Technology("Advanced Soil Enrichment", "Converts fertile environments to gaia environments for a cost of 300 BC. Increases the base size of the planet by 50%.", 30, enrichment: Technology.ADV_SOIL_ENRICHMENT));
			PlanetologyTechs.Add(new Technology("Terraforming +60", "Increases maximum colonist units on the terraformed planet by 60, for a cost of 300 BC.", 32, terraforming: 60));
			PlanetologyTechs.Add(new Technology("Complete Ecological Restoration", "Eliminates 20 units of pollution for 1 BC.", 34, ecoCleanup: 20));
			PlanetologyTechs.Add(new Technology("Universal Antidote", "Reduces the damage to population from all death weapons by 2.", 36, bioAntidote: Technology.UNIVERSAL_ANTIDOTE));
			PlanetologyTechs.Add(new Technology("Terraforming +80", "Increases maximum colonist units on the terraformed planet by 80, for a cost of 400 BC.", 38, terraforming: 80));
			PlanetologyTechs.Add(new Technology("Bio Terminator", "Ship weapon that reduces the maximum population of a planet by 3 each time it is fired. This is not affected by shields.", 40, bioWeapon: Technology.BIO_TERMINATOR));
			PlanetologyTechs.Add(new Technology("Advanced Cloning", "Reduces the cost to create one unit of population to 5 BC.", 42, cloning: 5));
			PlanetologyTechs.Add(new Technology("Terraforming +100", "Increases maximum colonist units on the terraformed planet by 100, for a cost of 500 BC.", 44, terraforming: 100));
			PlanetologyTechs.Add(new Technology("Complete Terraforming", "Increases maximum colonist units on the terraformed planet by 120, for a cost of 600 BC.", 50, terraforming: 120));
		}

		private void LoadPropulsionTechs(int diffModifier)
		{
			PropulsionTechs.Add(new Technology("Retro Engines", "Allows equipped ships to travel at 1 parsec per turn and gives a maximum number of movement squares in space combat of 1.", 1, speed: 1, maneuverSpeed: 1));
			PropulsionTechs.Add(new Technology("Hydrogen Fuel Cells", "Allows all ships to move a distance of 4 away from the owner's colonies.", 2, fuelRange: 4));
			PropulsionTechs.Add(new Technology("Deutrium Fuel Cells", "Allows all ships to move a distance of 5 away from the owner's colonies.", 5, fuelRange: 5));
			PropulsionTechs.Add(new Technology("Nuclear Engines", "Allows equipped ships to travel at 2 parsecs per turn and gives a maximum number of movement squares in space combat of 1.", 6, speed: 2, maneuverSpeed: 1));
			PropulsionTechs.Add(new Technology("Irridium Fuel Cells", "Allows all ships to move a distance of 6 away from the owner's colonies.", 9, fuelRange: 6));
			PropulsionTechs.Add(new Technology("Inertial Stabilizer", "Increases the maneuverability rating of the equipped ship by 2 and increases the maximum number of movement squares in space combat by 1.", 10, inertialstabilizer: true));
			PropulsionTechs.Add(new Technology("Sub Light Drives", "Allows equipped ships to travel at 3 parsecs per turn and gives a maximum number of movement squares in space combat of 1.", 12, speed: 3, maneuverSpeed: 1));
			PropulsionTechs.Add(new Technology("Dotomite Crystals", "Allows all ships to move a distance of 7 away from the owner's colonies.", 14, fuelRange: 7));
			PropulsionTechs.Add(new Technology("Energy Pulsar", "Deals 5 damage to all adjacent and diagonally adjacent ships plus 1 damage per 2 attacking ships.", 16, energypulsar: true));
			PropulsionTechs.Add(new Technology("Fusion Drives", "Allows equipped ships to travel at 4 parsecs per turn and gives a maximum number of movement squares in space combat of 2.", 18, speed: 4, maneuverSpeed: 2));
			PropulsionTechs.Add(new Technology("Uridium Fuel Cells", "Allows all ships to move a distance of 8 away from the owner's colonies.", 19, fuelRange: 8));
			PropulsionTechs.Add(new Technology("Warp Dissipator", "Ship special that reduces the defender's maneuverability rating by 0 - 1 per turn.", 20, warpDissipator: true));
			PropulsionTechs.Add(new Technology("Reajax II Fuel Cells", "Allows all ships to move a distance of 9 away from the owner's colonies.", 23, fuelRange: 9));
			PropulsionTechs.Add(new Technology("Impulse Drives", "Allows equipped ships to travel at 5 parsecs per turn and gives a maximum number of movement squares in space combat of 2.", 24, speed: 5, maneuverSpeed: 2));
			PropulsionTechs.Add(new Technology("Intergalactic Star Gates", "Ships can travel between any colonies controlled by the owner that have star gates in 1 turn. Costs 3000 BCs to create. Constructed as if it were a ship design.", 27, stargate: true));
			PropulsionTechs.Add(new Technology("Trilithium Crystals", "Allows all ships to move a distance of 10 away from the owner's colonies.", 29, fuelRange: 10));
			PropulsionTechs.Add(new Technology("Ion Drives", "Allows equipped ships to travel at 6 parsecs per turn and gives a maximum number of movement squares in space combat of 3.", 30, speed: 6, maneuverSpeed: 3));
			PropulsionTechs.Add(new Technology("High Energy Focus", "Increases the attack range of all the direct fire weapons on the equipped ship by 3. This includes all weapons that are not missiles, torpedoes, death weapons, and bombs.", 34, highEnergyFocus: true));
			PropulsionTechs.Add(new Technology("Anti-Matter Drives", "Allows equipped ships to travel at 7 parsecs per turn and gives a maximum number of movement squares in space combat of 3.", 36, speed: 7, maneuverSpeed: 3));
			PropulsionTechs.Add(new Technology("Sub Space Teleporter", "Equipped ships always move first in combat and can teleport directly to any square on the battle grid.", 38, subSpaceTeleporter: true));
			PropulsionTechs.Add(new Technology("Ionic Pulsar", "Deals 10 damage to all adjacent and diagonally adjacent ships plus 1 damage per 2 attacking ships.", 40, ionicPulsar: true));
			PropulsionTechs.Add(new Technology("Thorium Cells", "Allows ships to travel an infinite distance from their owner's colonies.", 41, fuelRange: int.MaxValue));
			PropulsionTechs.Add(new Technology("Inter-phased Drives", "Allows equipped ships to travel at 8 parsecs per turn and gives a maximum number of movement squares in space combat of 4.", 42, speed: 8, maneuverSpeed: 4));
			PropulsionTechs.Add(new Technology("Sub Space Interdictor", "Nullifies the functionality of all sub-space teleporters in use over the owner's planets.", 43, subspaceInterdictor: true));
			PropulsionTechs.Add(new Technology("Combat Transporters", "Ensures at least 50% of transporters land on the targeted planet.", 45, combatTransporters: true));
			PropulsionTechs.Add(new Technology("Inertial Nullifier", "Increases the maneuverability rating of the equipped ship by 4 and increases the maximum number of movement squares in space combat by 2.", 46, inertialNullifier: true));
			PropulsionTechs.Add(new Technology("Hyper Drives", "Allows equipped ships to travel at 9 parsecs per turn and gives a maximum number of movement squares in space combat of 4.", 48, speed: 9, maneuverSpeed: 4));
			PropulsionTechs.Add(new Technology("Displacement Device", "Causes 33% of all attacks to miss the target automatically.", 50, displacementDevice: true));
		}

		private void LoadWeaponTechs(int diffModifier)
		{
			WeaponTechs.Add(new Technology("Lasers", "Deals 1 - 4 damage to the target, heavy deals 1 - 7 damage to the target with a range of 2.", 1));
			WeaponTechs.Add(new Technology("Nuclear Missile", "Deals 4 damage to the target. Adds +1 level to the attacker's attack rating for this missile only.", 1));
			WeaponTechs.Add(new Technology("Nuclear Bomb", "Deals 3 - 12 damage to the target. Can only attack planets.", 1));
			WeaponTechs.Add(new Technology("Hand Lasers", "Adds 5 to ground combat rolls.", 2));
			WeaponTechs.Add(new Technology("Hyper V Rockets", "Deals 6 damage to the target. Adds +1 level to the attacker's attack rating for this missile only.", 4));
			WeaponTechs.Add(new Technology("Gatling Lasers", "Fires 4 shots per turn. Deals 1 - 4 damage per hit. Requires the same space as 3 lasers.", 5));
			WeaponTechs.Add(new Technology("Anti-missile Rockets", "Defensive Special Equipment. Destroys 40% of incoming missiles minus 1% per tech level of the missile.", 6));
			WeaponTechs.Add(new Technology("Neutron Pellet Gun", "Deals 2 - 5 damage, halves the effectiveness of the target's shields.", 7));
			WeaponTechs.Add(new Technology("Hyper X Rockets", "Deals 8 damage to the target. Adds +1 level to the attacker's attack rating for this missile only.", 8));
			WeaponTechs.Add(new Technology("Fusion Bomb", "Deals 4 - 16 damage to the target. Can only attack planets.", 9));
			WeaponTechs.Add(new Technology("Ion Cannon", "Deals 3 - 8 damage to the target, heavy deals 3 - 15 damage to the target with a 2 space range.", 10));
			WeaponTechs.Add(new Technology("Scatter Pack V Missiles", "Splits into 5 missiles that each deal 6 points of damage to the target.", 11));
			WeaponTechs.Add(new Technology("Ion Rifle", "Adds 10 to ground combat rolls. Replaces lower level rifle technologies.", 12));
			WeaponTechs.Add(new Technology("Mass Driver", "Deals 5 - 8 damage, halves the effectiveness of the enemy's shields.", 13));
			WeaponTechs.Add(new Technology("Mercullite Missiles", "Deals 10 damage to the target. Adds +2 level to the attacker's attack rating for this missile only.", 14));
			WeaponTechs.Add(new Technology("Neutron Blaster", "Regular is 3 - 12 damage with 1 space range. Heavy is 3 - 24 with a 2 space range.", 15));
			WeaponTechs.Add(new Technology("Anti-matter Bomb", "Deals 5 - 20 damage to the target. Can only attack planets.", 16));
			WeaponTechs.Add(new Technology("Graviton Beam", "Deals 1 - 15 damage to the target. Damage beyond what is needed to kill the ship transfers to the next ship.", 17));
			WeaponTechs.Add(new Technology("Stinger Missiles", "Deals 15 damage to the target. Adds + 3 level to the attacker's attack rating for this missile only.", 18));
			WeaponTechs.Add(new Technology("Hard Beam", "Deals 8 - 12 damage, halves the effectiveness of the enemy's shields.", 19));
			WeaponTechs.Add(new Technology("Fusion Beam", "Deals 4 - 16 damage with a 1 space range. Also researches Heavy Fusion Beam - Deals 4 - 30 damage with a 2 space range.", 20));
			WeaponTechs.Add(new Technology("Ion Stream Projector", "Deals damage equal to 20% of the target's hit points plus 1/2% for each firing ship. Round down, minimum damage of 1.", 21));
			WeaponTechs.Add(new Technology("Omega Bomb", "Deals 10 - 50 damage to the target. Can only attack planets.", 22));
			WeaponTechs.Add(new Technology("Anti-matter Torpedoes", "Deals 30 damage, can only be fired every other turn.", 23));
			WeaponTechs.Add(new Technology("Fusion Rifle", "Adds 15 to ground combat rolls. Replaces lower level rifle technologies.", 24));
			WeaponTechs.Add(new Technology("Megabolt Cannon", "Deals 2 - 20 damage. Wide beam gives the computer a + 3 attack rating when firing this weapon.", 25));
			WeaponTechs.Add(new Technology("Phasor", "Regular is 5 - 20. Heavy is 5 - 40 with a 2 space range.", 26));
			WeaponTechs.Add(new Technology("Scatter Pack VII Missiles", "Splits into 7 missiles that each deal 10 points of damage to the target.", 27));
			WeaponTechs.Add(new Technology("Auto Blaster", "Deals 4 - 16 damage. Fires 3 times per turn.", 28));
			WeaponTechs.Add(new Technology("Pulson Missiles", "Deals 20 damage to the target. Adds + 4 level to the attacker's attack rating for this missile only.", 29));
			WeaponTechs.Add(new Technology("Tachyon Beam", "Deals 1 - 25 damage to the target. Damage beyond what is needed to kill the ship transfers to the next ship.", 30));
			WeaponTechs.Add(new Technology("Fusion Rifle", "Adds 20 to ground combat rolls. Replaces lower level rifle technologies.", 31));
			WeaponTechs.Add(new Technology("Gauss Autocannon", "Deals 7 - 10 damage. Fires 3 times per turn. Halves the effectiveness of the target's shields.", 32));
			WeaponTechs.Add(new Technology("Particle Beam", "Deals 10 - 20 damage, halves the effectiveness of the enemy's shields.", 33));
			WeaponTechs.Add(new Technology("Hercular Missiles", "Deals 25 damage to the target. Adds + 5 level to the attacker's attack rating for this missile only.", 34));
			WeaponTechs.Add(new Technology("Plasma Cannon", "Deals 6 - 30 damage.", 35));
			WeaponTechs.Add(new Technology("Disruptor", "10 - 40 damage, Range 2.", 37));
			WeaponTechs.Add(new Technology("Pulse Phasor", "Deals 5 - 20 damage. Fires 3 times per turn.", 38));
			WeaponTechs.Add(new Technology("Neutronium Bomb", "Deals 40 - 70 damage. Can only attack planetary targets.", 39));
			WeaponTechs.Add(new Technology("Hellfire Torpedoes", "Deals 4 attacks to the target per hit, each for 25 damage. Fires every other turn.", 40));
			WeaponTechs.Add(new Technology("Zeon Missiles", "Deals 30 damage to the target. Adds + 7 level to the attacker's attack rating for this missile only.", 41));
			WeaponTechs.Add(new Technology("Plasma Rifle", "Adds 30 to ground combat rolls. Replaces lower level rifle technologies.", 42));
			WeaponTechs.Add(new Technology("Proton Torpedoes", "Deals 75 damage to the target. Fires once every other turn.", 43));
			WeaponTechs.Add(new Technology("Scatter Pack X Missiles", "Splits into 10 missiles that each deal 15 points of damage to the target.", 44));
			WeaponTechs.Add(new Technology("Tri Focus Plasma Cannon", "Deals 20 - 50 damage.", 45));
			WeaponTechs.Add(new Technology("Stellar Converter", "Deals 4 attacks to the target, each dealing 10 - 35 damage.", 46));
			WeaponTechs.Add(new Technology("Neutron Stream Projector", "Deals damage equal to 40% of the target's hit points plus 1/2% for each firing ship. Round down, minimum damage of 1.", 47));
			WeaponTechs.Add(new Technology("Mauler Device", "Deals 20 - 100 damage.", 48));
			WeaponTechs.Add(new Technology("Plasma Torpedoes", "Deals 150 damage to the target. Fires every other turn. Damage is reduced by 15 for each space travelled.", 50));
		}
		#endregion

		public List<Technology> GetRandomizedComputerTechs()
		{
			while (true)
			{
				bool isValid = false; //Must include at least one robotic factory tech
				List<Technology> randomList = new List<Technology>();
				foreach (var tech in ComputerTechs)
				{
					if (tech.TechLevel == 1)
					{
						//Include starting levels
						randomList.Add(tech);
					}
					else if (_gameMain.Random.Next(100) < 50)
					{
						randomList.Add(tech);
						if (tech.RoboticControl > 2)
						{
							isValid = true;
						}
					}
				}
				if (isValid)
				{
					return randomList;
				}
			}
		}
		public List<Technology> GetRandomizedConstructionTechs()
		{
			List<Technology> randomList = new List<Technology>();
			foreach (var tech in ConstructionTechs)
			{
				if (tech.TechLevel == 1)
				{
					//Include starting levels
					randomList.Add(tech);
				}
				else if (_gameMain.Random.Next(100) < 50)
				{
					randomList.Add(tech);
				}
			}
			return randomList;
		}
		public List<Technology> GetRandomizedForceFieldTechs()
		{
			while (true)
			{
				bool isValid = false; //Must include at least one planetary shield tech
				List<Technology> randomList = new List<Technology>();
				foreach (var tech in ForceFieldTechs)
				{
					if (tech.TechLevel == 1)
					{
						//Include starting levels
						randomList.Add(tech);
					}
					else if (_gameMain.Random.Next(100) < 50)
					{
						randomList.Add(tech);
						if (tech.PlanetaryShield > 0)
						{
							isValid = true;
						}
					}
				}
				if (isValid)
				{
					return randomList;
				}
			}
		}
		public List<Technology> GetRandomizedPlanetologyTechs()
		{
			List<Technology> randomList = new List<Technology>();
			foreach (var tech in PlanetologyTechs)
			{
				if (tech.TechLevel == 1)
				{
					//Include starting levels
					randomList.Add(tech);
				}
				else if (_gameMain.Random.Next(100) < 50)
				{
					randomList.Add(tech);
				}
			}
			return randomList;
		}
		public List<Technology> GetRandomizedPropulsionTechs()
		{
			while (true)
			{
				bool isValid = false; //Must include at least one planetary shield tech
				List<Technology> randomList = new List<Technology>();
				foreach (var tech in PropulsionTechs)
				{
					if (tech.TechLevel == 1)
					{
						//Include starting levels
						randomList.Add(tech);
					}
					else if (_gameMain.Random.Next(100) < 50)
					{
						randomList.Add(tech);
						if (tech.FuelRange == 4 || tech.FuelRange == 5)
						{
							isValid = true;
						}
					}
				}
				if (isValid)
				{
					return randomList;
				}
			}
		}
		public List<Technology> GetRandomizedWeaponTechs()
		{
			while (true)
			{
				bool isValid = false; //Must include at least one planetary shield tech
				List<Technology> randomList = new List<Technology>();
				foreach (var tech in WeaponTechs)
				{
					if (tech.TechLevel == 1)
					{
						//Include starting levels
						randomList.Add(tech);
					}
					else if (_gameMain.Random.Next(100) < 50)
					{
						randomList.Add(tech);
						if (tech.TechName.Contains("Missile") || tech.TechName.Contains("Torpedo") || tech.TechName.Contains("Hyper"))
						{
							isValid = true;
						}
					}
				}
				if (isValid)
				{
					return randomList;
				}
			}
		}
	}
}
