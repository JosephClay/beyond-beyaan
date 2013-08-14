using System;
using System.Collections.Generic;

namespace Beyond_Beyaan.Screens
{
	public class ResearchPrompt : WindowInterface
	{
		public Action Completed;

		private List<Technology> _discoveredTechs;
		private List<TechField> _fieldsNeedingNewTopics;
		private Dictionary<TechField, List<Technology>> _availableTopics;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!this.Initialize(gameMain.ScreenWidth / 2 - 330, gameMain.ScreenHeight / 2 - 330, 660, 660, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_discoveredTechs = new List<Technology>();
			_fieldsNeedingNewTopics = new List<TechField>();
			_availableTopics = new Dictionary<TechField, List<Technology>>();

			return true;
		}

		public void LoadEmpire(Empire empire, List<TechField> fields)
		{
			_discoveredTechs.Clear();
			_fieldsNeedingNewTopics = new List<TechField>(fields);
			_availableTopics.Clear();

			foreach (var techField in _fieldsNeedingNewTopics)
			{
				switch (techField)
				{
					case TechField.COMPUTER:
					{
						if (empire.TechnologyManager.WhichComputerBeingResearched != null)
						{
							_discoveredTechs.Add(empire.TechnologyManager.WhichComputerBeingResearched);
							empire.TechnologyManager.WhichComputerBeingResearched = null;
						}
						if (empire.TechnologyManager.UnresearchedComputerTechs.Count == 0)
						{
							break;
						}
						_availableTopics.Add(techField, new List<Technology>());
						//Now find the next tier of techs
						int highestTech = 0;
						foreach (var tech in empire.TechnologyManager.ResearchedComputerTechs)
						{
							if (tech.TechLevel > highestTech)
							{
								highestTech = tech.TechLevel;
							}
						}
						highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
						foreach (var tech in empire.TechnologyManager.UnresearchedComputerTechs)
						{
							if (tech.TechLevel <= highestTech)
							{
								_availableTopics[techField].Add(tech);
							}
						}
					} break;
					case TechField.CONSTRUCTION:
						{
							if (empire.TechnologyManager.WhichConstructionBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichConstructionBeingResearched);
								empire.TechnologyManager.WhichConstructionBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedConstructionTechs.Count == 0)
							{
								break;
							}
							_availableTopics.Add(techField, new List<Technology>());
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedConstructionTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
							foreach (var tech in empire.TechnologyManager.UnresearchedConstructionTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.FORCE_FIELD:
						{
							if (empire.TechnologyManager.WhichForceFieldBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichForceFieldBeingResearched);
								empire.TechnologyManager.WhichForceFieldBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedForceFieldTechs.Count == 0)
							{
								break;
							}
							_availableTopics.Add(techField, new List<Technology>());
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedForceFieldTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
							foreach (var tech in empire.TechnologyManager.UnresearchedForceFieldTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.PLANETOLOGY:
						{
							if (empire.TechnologyManager.WhichPlanetologyBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichPlanetologyBeingResearched);
								empire.TechnologyManager.WhichPlanetologyBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedPlanetologyTechs.Count == 0)
							{
								break;
							}
							_availableTopics.Add(techField, new List<Technology>());
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedPlanetologyTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
							foreach (var tech in empire.TechnologyManager.UnresearchedPlanetologyTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.PROPULSION:
						{
							if (empire.TechnologyManager.WhichPropulsionBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichPropulsionBeingResearched);
								empire.TechnologyManager.WhichPropulsionBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedPropulsionTechs.Count == 0)
							{
								break;
							}
							_availableTopics.Add(techField, new List<Technology>());
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedPropulsionTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
							foreach (var tech in empire.TechnologyManager.UnresearchedPropulsionTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.WEAPON:
						{
							if (empire.TechnologyManager.WhichWeaponBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichWeaponBeingResearched);
								empire.TechnologyManager.WhichWeaponBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedWeaponTechs.Count == 0)
							{
								break;
							}
							_availableTopics.Add(techField, new List<Technology>());
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedWeaponTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = ((highestTech / 5) + (highestTech % 5 > 0 ? 1 : 0)) * 5;
							foreach (var tech in empire.TechnologyManager.UnresearchedWeaponTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
				}
			}
		}
	}
}
