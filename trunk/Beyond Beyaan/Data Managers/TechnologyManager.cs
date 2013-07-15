using System;
using System.Collections.Generic;
using System.Linq;
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
		public List<Technology> ResearchedComputerTechs { get; private set; }
		public List<Technology> ResearchedConstructionTechs { get; private set; }
		public List<Technology> ResearchedForceFieldTechs { get; private set; }
		public List<Technology> ResearchedPlanetologyTechs { get; private set; }
		public List<Technology> ResearchedPropulsionTechs { get; private set; }
		public List<Technology> ResearchedWeaponTechs { get; private set; }

		public List<Technology> UnresearchedComputerTechs { get; private set; }
		public List<Technology> UnresearchedConstructionTechs { get; private set; }
		public List<Technology> UnresearchedForceFieldTechs { get; private set; }
		public List<Technology> UnresearchedPlanetologyTechs { get; private set; }
		public List<Technology> UnresearchedPropulsionTechs { get; private set; }
		public List<Technology> UnresearchedWeaponTechs { get; private set; }

		public Technology WhichComputerBeingResearched { get; set; }
		public Technology WhichConstructionBeingResearched { get; set; }
		public Technology WhichForceFieldBeingResearched { get; set; }
		public Technology WhichPlanetologyBeingResearched { get; set; }
		public Technology WhichPropulsionBeingResearched { get; set; }
		public Technology WhichWeaponBeingResearched { get; set; }

		public float ComputerResearchAmount { get; set; }
		public float ConstructionResearchAmount { get; set; }
		public float ForceFieldResearchAmount { get; set; }
		public float PlanetologyResearchAmount { get; set; }
		public float PropulsionResearchAmount { get; set; }
		public float WeaponResearchAmount { get; set; }

		public int ComputerLevel 
		{ 
			get	
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedComputerTechs.Count; i++)
				{
					if (ResearchedComputerTechs[i].TechLevel > level)
					{
						level = ResearchedComputerTechs[i].TechLevel;
					}
					if (ResearchedComputerTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedComputerTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}
		public int ConstructionLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedConstructionTechs.Count; i++)
				{
					if (ResearchedConstructionTechs[i].TechLevel > level)
					{
						level = ResearchedConstructionTechs[i].TechLevel;
					}
					if (ResearchedConstructionTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedConstructionTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}
		public int ForceFieldLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedForceFieldTechs.Count; i++)
				{
					if (ResearchedForceFieldTechs[i].TechLevel > level)
					{
						level = ResearchedForceFieldTechs[i].TechLevel;
					}
					if (ResearchedForceFieldTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedForceFieldTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}
		public int PlanetologyLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedPlanetologyTechs.Count; i++)
				{
					if (ResearchedPlanetologyTechs[i].TechLevel > level)
					{
						level = ResearchedPlanetologyTechs[i].TechLevel;
					}
					if (ResearchedPlanetologyTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedPlanetologyTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}
		public int PropulsionLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedPropulsionTechs.Count; i++)
				{
					if (ResearchedPropulsionTechs[i].TechLevel > level)
					{
						level = ResearchedPropulsionTechs[i].TechLevel;
					}
					if (ResearchedPropulsionTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedPropulsionTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}
		public int WeaponLevel
		{
			get
			{
				int level = 0;
				int numberOfStartingTechs = 0;
				for (int i = 0; i < ResearchedWeaponTechs.Count; i++)
				{
					if (ResearchedWeaponTechs[i].TechLevel > level)
					{
						level = ResearchedWeaponTechs[i].TechLevel;
					}
					if (ResearchedWeaponTechs[i].TechLevel == 1)
					{
						numberOfStartingTechs++;
					}
				}
				level = (int)(level * 0.8);
				level += ResearchedWeaponTechs.Count + 1 - numberOfStartingTechs;
				return level;
			}
		}

		public bool ComputerLocked { get; set; }
		public bool ConstructionLocked { get; set; }
		public bool ForceFieldLocked { get; set; }
		public bool PlanetologyLocked { get; set; }
		public bool PropulsionLocked { get; set; }
		public bool WeaponLocked { get; set; }

		public int ComputerPercentage { get; private set; }
		public int ConstructionPercentage { get; private set; }
		public int ForceFieldPercentage { get; private set; }
		public int PlanetologyPercentage { get; private set; }
		public int PropulsionPercentage { get; private set; }
		public int WeaponPercentage { get; private set; }

		//1.25 for poor, 1.00 for average, .80 for good, and .60 for excellent
		public float ComputerRaceModifier { get; set; }
		public float ConstructionRaceModifier { get; set; }
		public float ForceFieldRaceModifier { get; set; }
		public float PlanetologyRaceModifier { get; set; }
		public float PropulsionRaceModifier { get; set; }
		public float WeaponRaceModifier { get; set; }

		//20 for Simple, 25 for Easy, 30 for Medium, 35 for Hard, 40 for Impossible
		//Average rating is always used for AI players
		public int DifficultyModifier { get; set; }
		#endregion

		#region Constructor
		public TechnologyManager()
		{
			//Set the initial starting percentages
			ComputerPercentage = 20;
			ConstructionPercentage = 10;
			ForceFieldPercentage = 15;
			PlanetologyPercentage = 15;
			PropulsionPercentage = 20;
			WeaponPercentage = 20;
		}
		#endregion

		#region Functions
		public void SetComputerTechs(List<Technology> techs)
		{
			UnresearchedComputerTechs = techs;
			ResearchedComputerTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedComputerTechs.Add(tech);
					UnresearchedComputerTechs.Remove(tech);
				}
			}
		}
		public void SetConstructionTechs(List<Technology> techs)
		{
			UnresearchedConstructionTechs = techs;
			ResearchedConstructionTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedConstructionTechs.Add(tech);
					UnresearchedConstructionTechs.Remove(tech);
				}
			}
		}
		public void SetForceFieldTechs(List<Technology> techs)
		{
			UnresearchedForceFieldTechs = techs;
			ResearchedForceFieldTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedForceFieldTechs.Add(tech);
					UnresearchedForceFieldTechs.Remove(tech);
				}
			}
		}
		public void SetPlanetologyTechs(List<Technology> techs)
		{
			UnresearchedPlanetologyTechs = techs;
			ResearchedPlanetologyTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedPlanetologyTechs.Add(tech);
					UnresearchedPlanetologyTechs.Remove(tech);
				}
			}
		}
		public void SetPropulsionTechs(List<Technology> techs)
		{
			UnresearchedPropulsionTechs = techs;
			ResearchedPropulsionTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedPropulsionTechs.Add(tech);
					UnresearchedPropulsionTechs.Remove(tech);
				}
			}
		}
		public void SetWeaponTechs(List<Technology> techs)
		{
			UnresearchedWeaponTechs = techs;
			ResearchedWeaponTechs = new List<Technology>();
			foreach (var tech in techs)
			{
				if (tech.TechLevel == 1)
				{
					ResearchedWeaponTechs.Add(tech);
					UnresearchedWeaponTechs.Remove(tech);
				}
			}
		}

		public void ProcessResearchTurn(float researchPoints, Random r, SitRepManager sitRepManager)
		{
			if (ComputerPercentage == 0)
			{
				if (ComputerResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					ComputerResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = ComputerResearchAmount * 0.15f;
				float newPoints = (researchPoints * (ComputerPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				ComputerResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichComputerBeingResearched.ResearchPoints * DifficultyModifier * ComputerRaceModifier);
				if (ComputerResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((ComputerResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichComputerBeingResearched.TechName + " has been discovered."));
						ResearchedComputerTechs.Add(WhichComputerBeingResearched);
						UnresearchedComputerTechs.Remove(WhichComputerBeingResearched);
						WhichComputerBeingResearched = null;
						ComputerResearchAmount = 0;
					}
				}
			}

			if (ConstructionPercentage == 0)
			{
				if (ConstructionResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					ConstructionResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = ConstructionResearchAmount * 0.15f;
				float newPoints = (researchPoints * (ConstructionPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				ConstructionResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichConstructionBeingResearched.ResearchPoints * DifficultyModifier * ConstructionRaceModifier);
				if (ConstructionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((ConstructionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichConstructionBeingResearched.TechName + " has been discovered."));
						ResearchedConstructionTechs.Add(WhichConstructionBeingResearched);
						UnresearchedConstructionTechs.Remove(WhichConstructionBeingResearched);
						WhichConstructionBeingResearched = null;
						ConstructionResearchAmount = 0;
					}
				}
			}

			if (ForceFieldPercentage == 0)
			{
				if (ForceFieldResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					ForceFieldResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = ForceFieldResearchAmount * 0.15f;
				float newPoints = (researchPoints * (ForceFieldPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				ForceFieldResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichForceFieldBeingResearched.ResearchPoints * DifficultyModifier * ForceFieldRaceModifier);
				if (ForceFieldResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((ForceFieldResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichForceFieldBeingResearched.TechName + " has been discovered."));
						ResearchedForceFieldTechs.Add(WhichForceFieldBeingResearched);
						UnresearchedForceFieldTechs.Remove(WhichForceFieldBeingResearched);
						WhichForceFieldBeingResearched = null;
						ForceFieldResearchAmount = 0;
					}
				}
			}

			if (PlanetologyPercentage == 0)
			{
				if (PlanetologyResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					PlanetologyResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = PlanetologyResearchAmount * 0.15f;
				float newPoints = (researchPoints * (PlanetologyPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				PlanetologyResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichPlanetologyBeingResearched.ResearchPoints * DifficultyModifier * PlanetologyRaceModifier);
				if (PlanetologyResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((PlanetologyResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPlanetologyBeingResearched.TechName + " has been discovered."));
						ResearchedPlanetologyTechs.Add(WhichPlanetologyBeingResearched);
						UnresearchedPlanetologyTechs.Remove(WhichPlanetologyBeingResearched);
						WhichPlanetologyBeingResearched = null;
						PlanetologyResearchAmount = 0;
					}
				}
			}

			if (PropulsionPercentage == 0)
			{
				if (PropulsionResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					PropulsionResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = PropulsionResearchAmount * 0.15f;
				float newPoints = (researchPoints * (PropulsionPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				PropulsionResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichPropulsionBeingResearched.ResearchPoints * DifficultyModifier * PropulsionRaceModifier);
				if (PropulsionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((PropulsionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPropulsionBeingResearched.TechName + " has been discovered."));
						ResearchedPropulsionTechs.Add(WhichPropulsionBeingResearched);
						UnresearchedPropulsionTechs.Remove(WhichPropulsionBeingResearched);
						WhichPropulsionBeingResearched = null;
						PropulsionResearchAmount = 0;
					}
				}
			}

			if (WeaponPercentage == 0)
			{
				if (WeaponResearchAmount > 0)
				{
					//Lose 10% of total research invested if no research is being invested
					WeaponResearchAmount *= 0.9f;
				}
			}
			else
			{
				float interest = WeaponResearchAmount * 0.15f;
				float newPoints = (researchPoints * (WeaponPercentage * 0.01f));
				if ((newPoints * 2) < interest)
				{
					//up to 15% interest, but if we contribute less than half the interest, then cap the interest to double the current investment
					interest = newPoints * 2;
				}
				WeaponResearchAmount += (newPoints + interest);
				//See if it is discovered this turn
				int researchPointsRequired = (int)(WhichWeaponBeingResearched.ResearchPoints * DifficultyModifier * WeaponRaceModifier);
				if (WeaponResearchAmount > researchPointsRequired) //We now have a chance of discovering it
				{
					int chance = (int)((WeaponResearchAmount - researchPointsRequired) / (researchPointsRequired * 2));
					if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichWeaponBeingResearched.TechName + " has been discovered."));
						ResearchedWeaponTechs.Add(WhichWeaponBeingResearched);
						UnresearchedWeaponTechs.Remove(WhichWeaponBeingResearched);
						WhichWeaponBeingResearched = null;
						WeaponResearchAmount = 0;
					}
				}
			}
		}

		public void SetPercentage(TechField whichField, int amount)
		{
			int remainingPercentile = 100;
			if (BeamLocked)
			{
				remainingPercentile -= BeamPercentage;
			}
			if (ParticleLocked)
			{
				remainingPercentile -= ParticlePercentage;
			}
			if (MissileLocked)
			{
				remainingPercentile -= MissilePercentage;
			}
			if (TorpedoLocked)
			{
				remainingPercentile -= TorpedoPercentage;
			}
			if (BombLocked)
			{
				remainingPercentile -= BombPercentage;
			}
			if (EngineLocked)
			{
				remainingPercentile -= EnginePercentage;
			}
			if (ShieldLocked)
			{
				remainingPercentile -= ShieldPercentage;
			}
			if (ArmorLocked)
			{
				remainingPercentile -= ArmorPercentage;
			}
			if (ComputerLocked)
			{
				remainingPercentile -= ComputerPercentage;
			}
			if (InfrastructureLocked)
			{
				remainingPercentile -= InfrastructurePercentage;
			}

			if (amount >= remainingPercentile)
			{
				if (!BeamLocked)
				{
					BeamPercentage = 0;
				}
				if (!ParticleLocked)
				{
					ParticlePercentage = 0;
				}
				if (!MissileLocked)
				{
					MissilePercentage = 0;
				}
				if (!TorpedoLocked)
				{
					TorpedoPercentage = 0;
				}
				if (!BombLocked)
				{
					BombPercentage = 0;
				}
				if (!EngineLocked)
				{
					EnginePercentage = 0;
				}
				if (!ShieldLocked)
				{
					ShieldPercentage = 0;
				}
				if (!ArmorLocked)
				{
					ArmorPercentage = 0;
				}
				if (!ComputerLocked)
				{
					ComputerPercentage = 0;
				}
				if (!InfrastructureLocked)
				{
					InfrastructurePercentage = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			switch (whichField)
			{
				case TechField.ARMOR:
					{
						ArmorPercentage = amount;
						remainingPercentile -= ArmorPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.ARMOR);
					} break;
				case TechField.BEAM:
					{
						BeamPercentage = amount;
						remainingPercentile -= BeamPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.BEAM);
					} break;
				case TechField.BOMB:
					{
						BombPercentage = amount;
						remainingPercentile -= BombPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.BOMB);
					} break;
				case TechField.COMPUTER:
					{
						ComputerPercentage = amount;
						remainingPercentile -= ComputerPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.COMPUTER);
					} break;
				case TechField.ENGINE:
					{
						EnginePercentage = amount;
						remainingPercentile -= EnginePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.ENGINE);
					} break;
				case TechField.INFRASTRUCTURE:
					{
						InfrastructurePercentage = amount;
						remainingPercentile -= InfrastructurePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.INFRASTRUCTURE);
					} break;
				case TechField.MISSILE:
					{
						MissilePercentage = amount;
						remainingPercentile -= MissilePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.MISSILE);
					} break;
				case TechField.PARTICLE:
					{
						ParticlePercentage = amount;
						remainingPercentile -= ParticlePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PARTICLE);
					} break;
				case TechField.SHIELD:
					{
						ShieldPercentage = amount;
						remainingPercentile -= ShieldPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.SHIELD);
					} break;
				case TechField.TORPEDO:
					{
						TorpedoPercentage = amount;
						remainingPercentile -= TorpedoPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.TORPEDO);
					} break;
			}

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if (!InfrastructureLocked && whichField != TechField.INFRASTRUCTURE)
				{
					prevValue = InfrastructurePercentage;
					InfrastructurePercentage -= (InfrastructurePercentage >= amountToDeduct ? amountToDeduct : InfrastructurePercentage);
					amountToDeduct -= (prevValue - InfrastructurePercentage);
				}
				if (amountToDeduct > 0)
				{
					if (!EngineLocked && whichField != TechField.ENGINE)
					{
						prevValue = EnginePercentage;
						EnginePercentage -= (EnginePercentage >= amountToDeduct ? amountToDeduct : EnginePercentage);
						amountToDeduct -= (prevValue - EnginePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ShieldLocked && whichField != TechField.SHIELD)
					{
						prevValue = ShieldPercentage;
						ShieldPercentage -= (ShieldPercentage >= amountToDeduct ? amountToDeduct : ShieldPercentage);
						amountToDeduct -= (prevValue - ShieldPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ArmorLocked && whichField != TechField.ARMOR)
					{
						prevValue = ArmorPercentage;
						ArmorPercentage -= (ArmorPercentage >= amountToDeduct ? amountToDeduct : ArmorPercentage);
						amountToDeduct -= (prevValue - ArmorPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ComputerLocked && whichField != TechField.COMPUTER)
					{
						prevValue = ComputerPercentage;
						ComputerPercentage -= (ComputerPercentage >= amountToDeduct ? amountToDeduct : ComputerPercentage);
						amountToDeduct -= (prevValue - ComputerPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!BombLocked && whichField != TechField.BOMB)
					{
						prevValue = BombPercentage;
						BombPercentage -= (BombPercentage >= amountToDeduct ? amountToDeduct : BombPercentage);
						amountToDeduct -= (prevValue - BombPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!BeamLocked && whichField != TechField.BEAM)
					{
						prevValue = BeamPercentage;
						BeamPercentage -= (BeamPercentage >= amountToDeduct ? amountToDeduct : BeamPercentage);
						amountToDeduct -= (prevValue - BeamPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ParticleLocked && whichField != TechField.PARTICLE)
					{
						prevValue = ParticlePercentage;
						ParticlePercentage -= (ParticlePercentage >= amountToDeduct ? amountToDeduct : ParticlePercentage);
						amountToDeduct -= (prevValue - ParticlePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!MissileLocked && whichField != TechField.MISSILE)
					{
						prevValue = MissilePercentage;
						MissilePercentage -= (MissilePercentage >= amountToDeduct ? amountToDeduct : MissilePercentage);
						amountToDeduct -= (prevValue - MissilePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!TorpedoLocked && whichField != TechField.TORPEDO)
					{
						prevValue = TorpedoPercentage;
						TorpedoPercentage -= (TorpedoPercentage >= amountToDeduct ? amountToDeduct : TorpedoPercentage);
						amountToDeduct -= (prevValue - TorpedoPercentage);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!TorpedoLocked && whichField != TechField.TORPEDO)
				{
					TorpedoPercentage += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!MissileLocked && whichField != TechField.MISSILE)
					{
						MissilePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ParticleLocked && whichField != TechField.PARTICLE)
					{
						ParticlePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!BeamLocked && whichField != TechField.BEAM)
					{
						BeamPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!BombLocked && whichField != TechField.BOMB)
					{
						BombPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ComputerLocked && whichField != TechField.COMPUTER)
					{
						ComputerPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ArmorLocked && whichField != TechField.ARMOR)
					{
						ArmorPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ShieldLocked && whichField != TechField.SHIELD)
					{
						ShieldPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!EngineLocked && whichField != TechField.ENGINE)
					{
						EnginePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!InfrastructureLocked && whichField != TechField.INFRASTRUCTURE)
					{
						InfrastructurePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current tech field
					switch (whichField)
					{
						case TechField.ARMOR:
							ArmorPercentage += amountToAdd;
							break;
						case TechField.BEAM:
							BeamPercentage += amountToAdd;
							break;
						case TechField.BOMB:
							BombPercentage += amountToAdd;
							break;
						case TechField.COMPUTER:
							ComputerPercentage += amountToAdd;
							break;
						case TechField.ENGINE:
							EnginePercentage += amountToAdd;
							break;
						case TechField.INFRASTRUCTURE:
							InfrastructurePercentage += amountToAdd;
							break;
						case TechField.MISSILE:
							MissilePercentage += amountToAdd;
							break;
						case TechField.PARTICLE:
							ParticlePercentage += amountToAdd;
							break;
						case TechField.SHIELD:
							ShieldPercentage += amountToAdd;
							break;
						case TechField.TORPEDO:
							TorpedoPercentage += amountToAdd;
							break;
					}
				}
			}
		}

		private int GetTotalPercentageExcludingTypeAndLocked(TechField techField)
		{
			int total = 0;

			if (!BeamLocked && techField != TechField.BEAM)
			{
				total += BeamPercentage;
			}
			if (!ParticleLocked && techField != TechField.PARTICLE)
			{
				total += ParticlePercentage;
			}
			if (!BombLocked && techField != TechField.BOMB)
			{
				total += BombPercentage;
			}
			if (!MissileLocked && techField != TechField.MISSILE)
			{
				total += MissilePercentage;
			}
			if (!TorpedoLocked && techField != TechField.TORPEDO)
			{
				total += TorpedoPercentage;
			}
			if (!ComputerLocked && techField != TechField.COMPUTER)
			{
				total += ComputerPercentage;
			}
			if (!EngineLocked && techField != TechField.ENGINE)
			{
				total += EnginePercentage;
			}
			if (!ArmorLocked && techField != TechField.ARMOR)
			{
				total += ArmorPercentage;
			}
			if (!ShieldLocked && techField != TechField.SHIELD)
			{
				total += ShieldPercentage;
			}
			if (!InfrastructureLocked && techField != TechField.INFRASTRUCTURE)
			{
				total += InfrastructurePercentage;
			}

			return total;
		}
		#endregion
	}
}
