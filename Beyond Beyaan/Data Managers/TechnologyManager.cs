using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum TechType
	{
		COMPUTER, COMPUTER_MOUNT, COMPUTER_MODIFICATION,
		STELLAR_ENGINE, STELLAR_ENGINE_MOUNT, STELLAR_MODIFICATION,
		SYSTEM_ENGINE, SYSTEM_ENGINE_MOUNT, SYSTEM_MODIFICATION,
		SHIELD, SHIELD_MOUNT, SHIELD_MODIFICATION,
		ARMOR, ARMOR_PLATING, ARMOR_MODIFICATION,
		REACTOR, REACTOR_MOUNT, REACTOR_MODIFICATION,
		INFRASTRUCTURE,
		BEAM, BEAM_MOUNT, BEAM_MODIFICATION,
		PROJECTILE, PROJECTILE_MOUNT, PROJECTILE_MODIFICATION,
		MISSILE_WARHEAD, MISSILE_BODY, MISSILE_MODIFICATION,
		TORPEDO, TORPEDO_LAUNCHER, TORPEDO_MODIFICATION,
		BOMB, BOMB_BODY, BOMB_MODIFICATION,
		SHOCKWAVE, SHOCKWAVE_EMITTER, SHOCKWAVE_MODIFICATION,
		SPECIAL,
		UNKNOWN
	}
	public enum TechnologyField { ELECTRONICS, METALLURGY, PHYSICS, ENERGY, CHEMISTRY, CONSTRUCTION }

	public class TechnologyManager
	{
		#region Constants
		public const int CONSTRUCTION = 0;
		public const int CHEMISTRY = 1;
		public const int ELECTRONICS = 2;
		public const int ENERGY = 3;
		public const int METALLURGY = 4;
		public const int PHYSICS = 5;
		#endregion

		#region Properties
		private Dictionary<TechnologyItem, int>[] UnresearchedItems { get; set; }

		public List<TechnologyItem>[] VisibleItems { get; private set; }

		public List<TechnologyItem>[] ResearchedItems { get; private set; }

		public int[] WhichTechBeingResearched;

		public int[] TechBracketLevel;

		public bool[] TechLocked;

		public int[] TechPercentages;

		public float[] AmountResearched { get; private set; }

		#region Technology Items
		public List<TechnologyItem> Infrastructures { get; private set; }
		public List<TechnologyItem> Computers { get; private set; }
		public List<TechnologyItem> ComputerMounts { get; private set; }
		public List<TechnologyItem> ComputerMods { get; private set; }
		public List<TechnologyItem> StellarEngines { get; private set; }
		public List<TechnologyItem> StellarEngineMounts { get; private set; }
		public List<TechnologyItem> StellarEngineMods { get; private set; }
		public List<TechnologyItem> SystemEngines { get; private set; }
		public List<TechnologyItem> SystemEngineMounts { get; private set; }
		public List<TechnologyItem> SystemEngineMods { get; private set; }
		public List<TechnologyItem> Shields { get; private set; }
		public List<TechnologyItem> ShieldMounts { get; private set; }
		public List<TechnologyItem> ShieldMods { get; private set; }
		public List<TechnologyItem> Reactors { get; private set; }
		public List<TechnologyItem> ReactorMounts { get; private set; }
		public List<TechnologyItem> ReactorMods { get; private set; }
		public List<TechnologyItem> Armors { get; private set; }
		public List<TechnologyItem> ArmorPlatings { get; private set; }
		public List<TechnologyItem> ArmorMods { get; private set; }
		public List<TechnologyItem> Beams { get; private set; }
		public List<TechnologyItem> BeamMounts { get; private set; }
		public List<TechnologyItem> BeamMods { get; private set; }
		public List<TechnologyItem> Projectiles { get; private set; }
		public List<TechnologyItem> ProjectileMounts { get; private set; }
		public List<TechnologyItem> ProjectileMods { get; private set; }
		public List<TechnologyItem> MissileWarheads { get; private set; }
		public List<TechnologyItem> MissileBodies { get; private set; }
		public List<TechnologyItem> MissileMods { get; private set; }
		public List<TechnologyItem> Torpedoes { get; private set; }
		public List<TechnologyItem> TorpedoLaunchers { get; private set; }
		public List<TechnologyItem> TorpedoMods { get; private set; }
		public List<TechnologyItem> Bombs { get; private set; }
		public List<TechnologyItem> BombBodies { get; private set; }
		public List<TechnologyItem> BombMods { get; private set; }
		public List<TechnologyItem> Shockwaves { get; private set; }
		public List<TechnologyItem> ShockwaveEmitters { get; private set; }
		public List<TechnologyItem> ShockwaveMods { get; private set; }
		public List<TechnologyItem> SpecialEquipment { get; private set; }
		#endregion
		#endregion

		#region Constructor
		public TechnologyManager()
		{
			UnresearchedItems = new Dictionary<TechnologyItem, int>[6];
			VisibleItems = new List<TechnologyItem>[6];
			ResearchedItems = new List<TechnologyItem>[6];

			for (int i = 0; i < UnresearchedItems.Length; i++)
			{
				UnresearchedItems[i] = new Dictionary<TechnologyItem, int>();
				VisibleItems[i] = new List<TechnologyItem>();
				ResearchedItems[i] = new List<TechnologyItem>();
			}

			TechPercentages = new int[6];
			TechPercentages[CONSTRUCTION] = 15;
			TechPercentages[CHEMISTRY] = 15;
			TechPercentages[PHYSICS] = 20;
			TechPercentages[ENERGY] = 20;
			TechPercentages[METALLURGY] = 15;
			TechPercentages[ELECTRONICS] = 15;

			WhichTechBeingResearched = new int[6];
			TechBracketLevel = new int[6];
			TechLocked = new bool[6];
			AmountResearched = new float[6];

			Infrastructures = new List<TechnologyItem>();
			Computers = new List<TechnologyItem>();
			ComputerMounts = new List<TechnologyItem>();
			ComputerMods = new List<TechnologyItem>();
			StellarEngines = new List<TechnologyItem>();
			StellarEngineMounts = new List<TechnologyItem>();
			StellarEngineMods = new List<TechnologyItem>();
			SystemEngines = new List<TechnologyItem>();
			SystemEngineMounts = new List<TechnologyItem>();
			SystemEngineMods = new List<TechnologyItem>();
			Shields = new List<TechnologyItem>();
			ShieldMounts = new List<TechnologyItem>();
			ShieldMods = new List<TechnologyItem>();
			Reactors = new List<TechnologyItem>();
			ReactorMounts = new List<TechnologyItem>();
			ReactorMods = new List<TechnologyItem>();
			Armors = new List<TechnologyItem>();
			ArmorPlatings = new List<TechnologyItem>();
			ArmorMods = new List<TechnologyItem>();
			Beams = new List<TechnologyItem>();
			BeamMounts = new List<TechnologyItem>();
			BeamMods = new List<TechnologyItem>();
			Projectiles = new List<TechnologyItem>();
			ProjectileMounts = new List<TechnologyItem>();
			ProjectileMods = new List<TechnologyItem>();
			MissileWarheads = new List<TechnologyItem>();
			MissileBodies = new List<TechnologyItem>();
			MissileMods = new List<TechnologyItem>();
			Torpedoes = new List<TechnologyItem>();
			TorpedoLaunchers = new List<TechnologyItem>();
			TorpedoMods = new List<TechnologyItem>();
			Bombs = new List<TechnologyItem>();
			BombBodies = new List<TechnologyItem>();
			BombMods = new List<TechnologyItem>();
			Shockwaves = new List<TechnologyItem>();
			ShockwaveEmitters = new List<TechnologyItem>();
			ShockwaveMods = new List<TechnologyItem>();
			SpecialEquipment = new List<TechnologyItem>();
		}
		#endregion

		#region Functions
		public void AddTechnologies(Dictionary<TechnologyItem, int>[] technologies)
		{
			for (int i = 0; i < technologies.Length; i++)
			{
				foreach (KeyValuePair<TechnologyItem, int> item in technologies[i])
				{
					UnresearchedItems[i].Add(item.Key, item.Value);
				}
			}
		}
		/// <summary>
		/// Adds a discovered technology to list to research
		/// </summary>
		/// <param name="item"></param>
		/// <param name="whichField"></param>
		public void AddTechnology(TechnologyItem item, TechnologyField whichField)
		{
			switch (whichField)
			{
				case TechnologyField.CONSTRUCTION: VisibleItems[CONSTRUCTION].Add(item);
					break;
				case TechnologyField.CHEMISTRY: VisibleItems[CHEMISTRY].Add(item);
					break;
				case TechnologyField.ELECTRONICS: VisibleItems[ELECTRONICS].Add(item);
					break;
				case TechnologyField.ENERGY: VisibleItems[ENERGY].Add(item);
					break;
				case TechnologyField.METALLURGY: VisibleItems[METALLURGY].Add(item);
					break;
				case TechnologyField.PHYSICS: VisibleItems[PHYSICS].Add(item);
					break;
			}
		}

		public void SetInitialBracket(int startingLevel, int initialBracketLevel)
		{
			//Technologies start at level 1
			for (int i = 0; i < WhichTechBeingResearched.Length; i++)
			{
				WhichTechBeingResearched[i] = -1;
			}
			for (int i = 0; i < TechBracketLevel.Length; i++)
			{
				TechBracketLevel[i] = initialBracketLevel;
			}
			for (int i = 0; i < UnresearchedItems.Length; i++)
			{
				if (UnresearchedItems[i].Count > 0)
				{
					List<TechnologyItem> researched = new List<TechnologyItem>();
					List<TechnologyItem> available = new List<TechnologyItem>();
					foreach (KeyValuePair <TechnologyItem, int> item in UnresearchedItems[i])
					{
						if (item.Value <= startingLevel)
						{
							researched.Add(item.Key);
						}
						else if (item.Value <= initialBracketLevel)
						{
							available.Add(item.Key);
						}
					}
					foreach (TechnologyItem item in researched)
					{
						UnresearchedItems[i].Remove(item);
						ResearchedItems[i].Add(item);
						AddTechnologyItemToList(item);
					}
					foreach (TechnologyItem item in available)
					{
						UnresearchedItems[i].Remove(item);
						VisibleItems[i].Add(item);
					}
				}
			}
		}

		public void ProcessResearchTurn(float researchPoints, SitRepManager sitRepManager)
		{
			for (int i = 0; i < TechPercentages.Length; i++)
			{
				AmountResearched[i] += researchPoints * TechPercentages[i] * 0.01f;
			}

			for (int i = 0; i < VisibleItems.Length; i++)
			{
				if (VisibleItems[i].Count > 0 && WhichTechBeingResearched[i] >= 0 && AmountResearched[i] >= VisibleItems[i][WhichTechBeingResearched[i]].ResearchPoints)
				{
					AmountResearched[i] = 0;
					TechnologyItem item = VisibleItems[i][WhichTechBeingResearched[i]];
					ResearchedItems[i].Add(item);
					VisibleItems[i].Remove(item);
					AddTechnologyItemToList(item);
					WhichTechBeingResearched[i] = -1;
					if (item.TechLevel > TechBracketLevel[i] - 5)
					{
						TechBracketLevel[i] += 5;
						while (UnresearchedItems[i].Count > 0)
						{
							List<TechnologyItem> itemsToAdd = new List<TechnologyItem>();
							foreach (KeyValuePair <TechnologyItem, int> techItem in UnresearchedItems[i])
							{
								if (techItem.Value <= TechBracketLevel[i])
								{
									itemsToAdd.Add(techItem.Key);
								}
							}
							foreach (TechnologyItem techItem in itemsToAdd)
							{
								VisibleItems[i].Add(techItem);
								UnresearchedItems[i].Remove(techItem);
							}
							if (itemsToAdd.Count > 0)
							{
								break;
							}
						}
					}
				}
			}
		}

		public void SetPercentage(int whichField, int amount)
		{
			int remainingPercentile = 100;
			for (int i = 0; i < TechLocked.Length; i++)
			{
				if (TechLocked[i])
				{
					remainingPercentile -= TechPercentages[i];
				}
			}

			if (amount >= remainingPercentile)
			{
				for (int i = 0; i < TechLocked.Length; i++)
				{
					if (!TechLocked[i])
					{
						TechPercentages[i] = 0;
					}
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			TechPercentages[whichField] = amount;
			remainingPercentile -= TechPercentages[whichField];
			totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(whichField);

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;

				for (int i = 0; i < TechLocked.Length; i++)
				{
					if (amountToDeduct <= 0)
					{
						break;
					}
					if (!TechLocked[i] && whichField != i)
					{
						prevValue = TechPercentages[i];
						TechPercentages[i] -= TechPercentages[i] >= amountToDeduct ? amountToDeduct : TechPercentages[i];
						amountToDeduct -= (prevValue - TechPercentages[i]);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				for (int i = 0; i < TechLocked.Length; i++)
				{
					if (amountToAdd <= 0)
					{
						break;
					}
					if (!TechLocked[i] && whichField != i)
					{
						TechPercentages[i] += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current tech field
					TechPercentages[whichField] += amountToAdd;
				}
			}
		}

		private int GetTotalPercentageExcludingTypeAndLocked(int techField)
		{
			int total = 0;
			for (int i = 0; i < TechLocked.Length; i++)
			{
				if (!TechLocked[i] && i != techField)
				{
					total += TechPercentages[i];
				}
			}

			return total;
		}

		private void AddTechnologyItemToList(TechnologyItem item)
		{
			switch (item.TechType)
			{
				case TechType.ARMOR: Armors.Add(item);
					break;
				case TechType.ARMOR_PLATING: ArmorPlatings.Add(item);
					break;
				case TechType.ARMOR_MODIFICATION: ArmorMods.Add(item);
					break;
				case TechType.BEAM: Beams.Add(item);
					break;
				case TechType.BEAM_MODIFICATION: BeamMods.Add(item);
					break;
				case TechType.BEAM_MOUNT: BeamMounts.Add(item);
					break;
				case TechType.BOMB: Bombs.Add(item);
					break;
				case TechType.BOMB_BODY: BombBodies.Add(item);
					break;
				case TechType.BOMB_MODIFICATION: BombMods.Add(item);
					break;
				case TechType.COMPUTER: Computers.Add(item);
					break;
				case TechType.COMPUTER_MOUNT: ComputerMounts.Add(item);
					break;
				case TechType.COMPUTER_MODIFICATION: ComputerMods.Add(item);
					break;
				case TechType.INFRASTRUCTURE: Infrastructures.Add(item);
					break;
				case TechType.MISSILE_BODY: MissileBodies.Add(item);
					break;
				case TechType.MISSILE_MODIFICATION: MissileMods.Add(item);
					break;
				case TechType.MISSILE_WARHEAD: MissileWarheads.Add(item);
					break;
				case TechType.PROJECTILE: Projectiles.Add(item);
					break;
				case TechType.PROJECTILE_MODIFICATION: ProjectileMods.Add(item);
					break;
				case TechType.PROJECTILE_MOUNT: ProjectileMounts.Add(item);
					break;
				case TechType.SHIELD: Shields.Add(item);
					break;
				case TechType.SHIELD_MOUNT: ShieldMounts.Add(item);
					break;
				case TechType.SHIELD_MODIFICATION: ShieldMods.Add(item);
					break;
				case TechType.SHOCKWAVE: Shockwaves.Add(item);
					break;
				case TechType.SHOCKWAVE_EMITTER: ShockwaveEmitters.Add(item);
					break;
				case TechType.SHOCKWAVE_MODIFICATION: ShockwaveMods.Add(item);
					break;
				case TechType.STELLAR_ENGINE: StellarEngines.Add(item);
					break;
				case TechType.STELLAR_ENGINE_MOUNT: StellarEngineMounts.Add(item);
					break;
				case TechType.STELLAR_MODIFICATION: StellarEngineMods.Add(item);
					break;
				case TechType.SYSTEM_ENGINE: SystemEngines.Add(item);
					break;
				case TechType.SYSTEM_ENGINE_MOUNT: SystemEngineMounts.Add(item);
					break;
				case TechType.SYSTEM_MODIFICATION: SystemEngineMods.Add(item);
					break;
				case TechType.TORPEDO: Torpedoes.Add(item);
					break;
				case TechType.TORPEDO_LAUNCHER: TorpedoLaunchers.Add(item);
					break;
				case TechType.TORPEDO_MODIFICATION: TorpedoMods.Add(item);
					break;
				case TechType.SPECIAL: SpecialEquipment.Add(item);
					break;
			}
		}
		#endregion
	}
}
