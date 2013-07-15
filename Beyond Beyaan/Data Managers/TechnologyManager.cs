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
	public enum TechField { COMPUTER, ENGINE, SHIELD, ARMOR, INFRASTRUCTURE, BEAM, PARTICLE, MISSILE, TORPEDO, BOMB }

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

		public void ProcessResearchTurn(float researchPoints, SitRepManager sitRepManager)
		{
			bool update = false;
			if (VisibleEngines[WhichEngineBeingResearched].UpdateResearch(researchPoints * (EnginePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleEngines[WhichEngineBeingResearched].GetName() +
					(VisibleEngines[WhichEngineBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleEngines[WhichEngineBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleArmors[WhichArmorBeingResearched].UpdateResearch(researchPoints * (ArmorPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleArmors[WhichArmorBeingResearched].GetName() +
					(VisibleArmors[WhichArmorBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleArmors[WhichArmorBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleShields[WhichShieldBeingResearched].UpdateResearch(researchPoints * (ShieldPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleShields[WhichShieldBeingResearched].GetName() +
					(VisibleShields[WhichShieldBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleShields[WhichShieldBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleComputers[WhichComputerBeingResearched].UpdateResearch(researchPoints * (ComputerPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleComputers[WhichComputerBeingResearched].GetName() +
					(VisibleComputers[WhichComputerBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleComputers[WhichComputerBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleInfrastructures[WhichInfrastructureBeingResearched].UpdateResearch(researchPoints * (InfrastructurePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleInfrastructures[WhichInfrastructureBeingResearched].GetName() +
					(VisibleInfrastructures[WhichInfrastructureBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleInfrastructures[WhichInfrastructureBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleBeams[WhichBeamBeingResearched].UpdateResearch(researchPoints * (BeamPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleBeams[WhichBeamBeingResearched].GetName() +
					(VisibleBeams[WhichBeamBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleBeams[WhichBeamBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleParticles[WhichParticleBeingResearched].UpdateResearch(researchPoints * (ParticlePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleParticles[WhichParticleBeingResearched].GetName() +
					(VisibleParticles[WhichParticleBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleParticles[WhichParticleBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleMissiles[WhichMissileBeingResearched].UpdateResearch(researchPoints * (MissilePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleMissiles[WhichMissileBeingResearched].GetName() +
					(VisibleMissiles[WhichMissileBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleMissiles[WhichMissileBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleTorpedoes[WhichTorpedoBeingResearched].UpdateResearch(researchPoints * (TorpedoPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleTorpedoes[WhichTorpedoBeingResearched].GetName() +
					(VisibleTorpedoes[WhichTorpedoBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleTorpedoes[WhichTorpedoBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleBombs[WhichBombBeingResearched].UpdateResearch(researchPoints * (BombPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleBombs[WhichBombBeingResearched].GetName() +
					(VisibleBombs[WhichBombBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleBombs[WhichBombBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}

			if (update)
			{
				UpdateVisibleTechs();
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
