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
	public class TechnologyManager
	{
		#region Properties
		private Dictionary<TechnologyItem, int> Technologies { get; set; }

		public List<TechnologyItem> VisibleItems { get; private set; }

		public List<TechnologyItem> ResearchedItems { get; private set; }

		public List<string> TechCategories { get; private set; }

		public List<TechnologyProject> WhichTechsBeingResearched { get; private set; }

		public Dictionary<string, int> TechBracketLevel { get; private set; }
		#endregion

		#region Constructor
		public TechnologyManager()
		{
			Technologies = new Dictionary<TechnologyItem, int>();
			VisibleItems = new List<TechnologyItem>();
			ResearchedItems = new List<TechnologyItem>();
			TechCategories = new List<string>();
			WhichTechsBeingResearched = new List<TechnologyProject>();
			TechBracketLevel = new Dictionary<string, int>();
		}
		#endregion

		#region Functions
		public void AddTechnologies(Dictionary<TechnologyItem, int> technologies)
		{
			foreach (KeyValuePair<TechnologyItem, int> item in technologies)
			{
				Technologies.Add(item.Key, item.Value);
			}
		}
		/// <summary>
		/// Adds a discovered technology to list to research
		/// </summary>
		/// <param name="item"></param>
		/// <param name="whichField"></param>
		public void AddTechnology(TechnologyItem item)
		{
			if (!ResearchedItems.Contains(item) && !VisibleItems.Contains(item))
			{
				VisibleItems.Add(item);
			}
		}

		public void SetInitialBracket(int startingLevel, int initialBracketLevel, ItemManager itemManager)
		{
			TechBracketLevel = new Dictionary<string, int>();
			ResearchedItems = new List<TechnologyItem>();
			VisibleItems = new List<TechnologyItem>();
			//Technologies start at level 1
			for (int i = 0; i < TechCategories.Count; i++)
			{
				TechBracketLevel[TechCategories[i]] = initialBracketLevel;
			}
			List<TechnologyItem> itemsToRemoveFromUnresearched = new List<TechnologyItem>();
			foreach (KeyValuePair<TechnologyItem, int> item in Technologies)
			{
				if (item.Value <= startingLevel)
				{
					ResearchedItems.Add(item.Key);
					itemManager.UpdateItems(item.Key.ItemsToAdd(), item.Key.ItemsToRemove());
					itemsToRemoveFromUnresearched.Add(item.Key);
				}
				else if (item.Value <= initialBracketLevel)
				{
					VisibleItems.Add(item.Key);
					itemsToRemoveFromUnresearched.Add(item.Key);
				}
			}
		}

		public void ProcessResearchTurn(Dictionary<Resource, float> researchPoints, ItemManager itemManager, SitRepManager sitRepManager)
		{
			foreach (KeyValuePair<Resource, float> resource in researchPoints)
			{
				//First, gather all the projects that uses this resource, then divide the resources between them.
				List<TechnologyProject> projects = new List<TechnologyProject>();
				float amount = 0;
				foreach (TechnologyProject project in WhichTechsBeingResearched)
				{
					if (project.TechBeingResearched.Cost.ContainsKey(resource.Key))
					{
						projects.Add(project);
						amount += project.Percentage;
					}
				}

				foreach (TechnologyProject project in projects)
				{
					float amountToDistribute = project.Percentage / amount;
					if (project.InvestProject(resource.Key, amountToDistribute * resource.Value))
					{
						//Returned true if project is fully paid
						ResearchedItems.Add(project.TechBeingResearched);
						VisibleItems.Remove(project.TechBeingResearched);
						itemManager.UpdateItems(project.TechBeingResearched.ItemsToAdd(), project.TechBeingResearched.ItemsToRemove());
						if (Technologies[project.TechBeingResearched] > TechBracketLevel[project.TechBeingResearched.Category] - 5)
						{
							//Advance this field
							AdvanceBracket(project.TechBeingResearched.Category);
						}
					}
				}
			}
		}

		private void AdvanceBracket(string whichCategory)
		{
		}

			/*for (int i = 0; i < VisibleItems.Count; i++)
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
		}*/

		/*public void SetPercentage(int whichField, int amount)
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
		}*/
		#endregion
	}
}
