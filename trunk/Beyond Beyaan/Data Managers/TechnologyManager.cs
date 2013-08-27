using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum TechField { COMPUTER, CONSTRUCTION, FORCE_FIELD, PLANETOLOGY, PROPULSION, WEAPON }

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
		public int FuelRange { get; private set; }

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

			ComputerRaceModifier = 1;
			ConstructionRaceModifier = 1;
			ForceFieldRaceModifier = 1;
			PlanetologyRaceModifier = 1;
			PropulsionRaceModifier = 1;
			WeaponRaceModifier = 1;

			ResearchedComputerTechs = new List<Technology>();
			ResearchedConstructionTechs = new List<Technology>();
			ResearchedForceFieldTechs = new List<Technology>();
			ResearchedPlanetologyTechs = new List<Technology>();
			ResearchedPropulsionTechs = new List<Technology>();
			ResearchedWeaponTechs = new List<Technology>();

			UpdateValues();
		}
		#endregion

		#region Functions
		public void SetComputerTechs(List<Technology> techs)
		{
			UnresearchedComputerTechs = new List<Technology>(techs);
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
			UnresearchedConstructionTechs = new List<Technology>(techs);
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
			UnresearchedForceFieldTechs = new List<Technology>(techs);
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
			UnresearchedPlanetologyTechs = new List<Technology>(techs);
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
			UnresearchedPropulsionTechs = new List<Technology>(techs);
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
			UnresearchedWeaponTechs = new List<Technology>(techs);
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

		public void AccureResearch(float researchPoints)
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
			}
		}

		//Returns a list of fields that needs a new item to be researched
		public List<TechField> RollForDiscoveries(Random r, SitRepManager sitRepManager)
		{
			List<TechField> fieldsNeedingNewItems = new List<TechField>();
			//Only items being currently researched have a chance of being discovered, otherwise they degrade (handled in AccureResearch function)
			if (ComputerPercentage > 0)
			{
				if (WhichComputerBeingResearched == null && UnresearchedComputerTechs.Count > 0 && ComputerResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.COMPUTER);
				}
				else if (WhichComputerBeingResearched != null)
				{
					int researchPointsRequired = (int)(WhichComputerBeingResearched.ResearchPoints * DifficultyModifier * ComputerRaceModifier);
					if (ComputerResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((ComputerResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichComputerBeingResearched.TechName + " has been discovered."));
							ResearchedComputerTechs.Add(WhichComputerBeingResearched);
							UnresearchedComputerTechs.Remove(WhichComputerBeingResearched);
							ComputerResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.COMPUTER);
						}
					}
				}
			}
			

			if (ConstructionPercentage > 0)
			{
				if (WhichConstructionBeingResearched == null && UnresearchedConstructionTechs.Count > 0 && ConstructionResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.CONSTRUCTION);
				}
				else if (WhichConstructionBeingResearched != null)
				{
					//See if it is discovered this turn
					int researchPointsRequired = (int)(WhichConstructionBeingResearched.ResearchPoints * DifficultyModifier * ConstructionRaceModifier);
					if (ConstructionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((ConstructionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichConstructionBeingResearched.TechName + " has been discovered."));
							ResearchedConstructionTechs.Add(WhichConstructionBeingResearched);
							UnresearchedConstructionTechs.Remove(WhichConstructionBeingResearched);
							ConstructionResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.CONSTRUCTION);
						}
					}
				}
			}

			if (ForceFieldPercentage > 0)
			{
				if (WhichForceFieldBeingResearched == null && UnresearchedForceFieldTechs.Count > 0 && ForceFieldResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.FORCE_FIELD);
				}
				else if (WhichForceFieldBeingResearched != null)
				{
					int researchPointsRequired = (int)(WhichForceFieldBeingResearched.ResearchPoints * DifficultyModifier * ForceFieldRaceModifier);
					if (ForceFieldResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((ForceFieldResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichForceFieldBeingResearched.TechName + " has been discovered."));
							ResearchedForceFieldTechs.Add(WhichForceFieldBeingResearched);
							UnresearchedForceFieldTechs.Remove(WhichForceFieldBeingResearched);
							ForceFieldResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.FORCE_FIELD);
						}
					}
				}
			}

			if (PlanetologyPercentage > 0)
			{
				if (WhichPlanetologyBeingResearched == null && UnresearchedPlanetologyTechs.Count > 0 && PlanetologyResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.PLANETOLOGY);
				}
				else if (WhichPlanetologyBeingResearched != null)
				{
					//See if it is discovered this turn
					int researchPointsRequired = (int)(WhichPlanetologyBeingResearched.ResearchPoints * DifficultyModifier * PlanetologyRaceModifier);
					if (PlanetologyResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((PlanetologyResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPlanetologyBeingResearched.TechName + " has been discovered."));
							ResearchedPlanetologyTechs.Add(WhichPlanetologyBeingResearched);
							UnresearchedPlanetologyTechs.Remove(WhichPlanetologyBeingResearched);
							PlanetologyResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.PLANETOLOGY);
						}
					}
				}
			}

			if (PropulsionPercentage > 0)
			{
				if (WhichPropulsionBeingResearched == null && UnresearchedPropulsionTechs.Count > 0 && PropulsionResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.PROPULSION);
				}
				else if (WhichPropulsionBeingResearched != null)
				{
					//See if it is discovered this turn
					int researchPointsRequired = (int)(WhichPropulsionBeingResearched.ResearchPoints * DifficultyModifier * PropulsionRaceModifier);
					if (PropulsionResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((PropulsionResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichPropulsionBeingResearched.TechName + " has been discovered."));
							ResearchedPropulsionTechs.Add(WhichPropulsionBeingResearched);
							UnresearchedPropulsionTechs.Remove(WhichPropulsionBeingResearched);
							PropulsionResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.PROPULSION);
						}
					}
				}
			}

			if (WeaponPercentage > 0)
			{
				if (WhichWeaponBeingResearched == null && UnresearchedWeaponTechs.Count > 0 && WeaponResearchAmount > 0)
				{
					fieldsNeedingNewItems.Add(TechField.WEAPON);
				}
				else if (WhichWeaponBeingResearched != null)
				{
					//See if it is discovered this turn
					int researchPointsRequired = (int)(WhichWeaponBeingResearched.ResearchPoints * DifficultyModifier * WeaponRaceModifier);
					if (WeaponResearchAmount > researchPointsRequired) //We now have a chance of discovering it
					{
						int chance = (int)(((WeaponResearchAmount - researchPointsRequired) / (researchPointsRequired * 2)) * 100);
						if ((r.Next(100) + 1) < chance) //Eureka!  We've discovered the tech!  +1 is to change from 0-99 to 1-100
						{
							sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), WhichWeaponBeingResearched.TechName + " has been discovered."));
							ResearchedWeaponTechs.Add(WhichWeaponBeingResearched);
							UnresearchedWeaponTechs.Remove(WhichWeaponBeingResearched);
							WeaponResearchAmount = 0;
							fieldsNeedingNewItems.Add(TechField.WEAPON);
						}
					}
				}
			}

			UpdateValues();

			return fieldsNeedingNewItems;
		}

		public void SetPercentage(TechField whichField, int amount)
		{
			int remainingPercentile = 100;
			if (ComputerLocked)
			{
				remainingPercentile -= ComputerPercentage;
			}
			if (ConstructionLocked)
			{
				remainingPercentile -= ConstructionPercentage;
			}
			if (ForceFieldLocked)
			{
				remainingPercentile -= ForceFieldPercentage;
			}
			if (PlanetologyLocked)
			{
				remainingPercentile -= PlanetologyPercentage;
			}
			if (PropulsionLocked)
			{
				remainingPercentile -= PropulsionPercentage;
			}
			if (WeaponLocked)
			{
				remainingPercentile -= WeaponPercentage;
			}

			if (amount >= remainingPercentile)
			{
				if (!ComputerLocked)
				{
					ComputerPercentage = 0;
				}
				if (!ConstructionLocked)
				{
					ConstructionPercentage = 0;
				}
				if (!ForceFieldLocked)
				{
					ForceFieldPercentage = 0;
				}
				if (!PlanetologyLocked)
				{
					PlanetologyPercentage = 0;
				}
				if (!PropulsionLocked)
				{
					PropulsionPercentage = 0;
				}
				if (!WeaponLocked)
				{
					WeaponPercentage = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			switch (whichField)
			{
				case TechField.COMPUTER:
					{
						ComputerPercentage = amount;
						remainingPercentile -= ComputerPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.COMPUTER);
					} break;
				case TechField.CONSTRUCTION:
					{
						ConstructionPercentage = amount;
						remainingPercentile -= ConstructionPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.CONSTRUCTION);
					} break;
				case TechField.FORCE_FIELD:
					{
						ForceFieldPercentage = amount;
						remainingPercentile -= ForceFieldPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.FORCE_FIELD);
					} break;
				case TechField.PLANETOLOGY:
					{
						PlanetologyPercentage = amount;
						remainingPercentile -= PlanetologyPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PLANETOLOGY);
					} break;
				case TechField.PROPULSION:
					{
						PropulsionPercentage = amount;
						remainingPercentile -= PropulsionPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PROPULSION);
					} break;
				case TechField.WEAPON:
					{
						WeaponPercentage = amount;
						remainingPercentile -= WeaponPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.WEAPON);
					} break;
			}

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if (!ComputerLocked && whichField != TechField.COMPUTER)
				{
					prevValue = ComputerPercentage;
					ComputerPercentage -= (ComputerPercentage >= amountToDeduct ? amountToDeduct : ComputerPercentage);
					amountToDeduct -= (prevValue - ComputerPercentage);
				}
				if (amountToDeduct > 0)
				{
					if (!ConstructionLocked && whichField != TechField.CONSTRUCTION)
					{
						prevValue = ConstructionPercentage;
						ConstructionPercentage -= (ConstructionPercentage >= amountToDeduct ? amountToDeduct : ConstructionPercentage);
						amountToDeduct -= (prevValue - ConstructionPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ForceFieldLocked && whichField != TechField.FORCE_FIELD)
					{
						prevValue = ForceFieldPercentage;
						ForceFieldPercentage -= (ForceFieldPercentage >= amountToDeduct ? amountToDeduct : ForceFieldPercentage);
						amountToDeduct -= (prevValue - ForceFieldPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!PlanetologyLocked && whichField != TechField.PLANETOLOGY)
					{
						prevValue = PlanetologyPercentage;
						PlanetologyPercentage -= (PlanetologyPercentage >= amountToDeduct ? amountToDeduct : PlanetologyPercentage);
						amountToDeduct -= (prevValue - PlanetologyPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!PropulsionLocked && whichField != TechField.PROPULSION)
					{
						prevValue = PropulsionPercentage;
						PropulsionPercentage -= (PropulsionPercentage >= amountToDeduct ? amountToDeduct : PropulsionPercentage);
						amountToDeduct -= (prevValue - PropulsionPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!WeaponLocked && whichField != TechField.WEAPON)
					{
						prevValue = WeaponPercentage;
						WeaponPercentage -= (WeaponPercentage >= amountToDeduct ? amountToDeduct : WeaponPercentage);
						amountToDeduct -= (prevValue - WeaponPercentage);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!ComputerLocked && whichField != TechField.COMPUTER)
				{
					ComputerPercentage += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!ConstructionLocked && whichField != TechField.CONSTRUCTION)
					{
						ConstructionPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ForceFieldLocked && whichField != TechField.FORCE_FIELD)
					{
						ForceFieldPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!PlanetologyLocked && whichField != TechField.PLANETOLOGY)
					{
						PlanetologyPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!PropulsionLocked && whichField != TechField.PROPULSION)
					{
						PropulsionPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!WeaponLocked && whichField != TechField.WEAPON)
					{
						WeaponPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current tech field
					switch (whichField)
					{
						case TechField.COMPUTER:
							ComputerPercentage += amountToAdd;
							break;
						case TechField.CONSTRUCTION:
							ConstructionPercentage += amountToAdd;
							break;
						case TechField.FORCE_FIELD:
							ForceFieldPercentage += amountToAdd;
							break;
						case TechField.PLANETOLOGY:
							PlanetologyPercentage += amountToAdd;
							break;
						case TechField.PROPULSION:
							PropulsionPercentage += amountToAdd;
							break;
						case TechField.WEAPON:
							WeaponPercentage += amountToAdd;
							break;
					}
				}
			}
		}

		private int GetTotalPercentageExcludingTypeAndLocked(TechField techField)
		{
			int total = 0;

			if (!ComputerLocked && techField != TechField.COMPUTER)
			{
				total += ComputerPercentage;
			}
			if (!ConstructionLocked && techField != TechField.CONSTRUCTION)
			{
				total += ConstructionPercentage;
			}
			if (!ForceFieldLocked && techField != TechField.FORCE_FIELD)
			{
				total += ForceFieldPercentage;
			}
			if (!PlanetologyLocked && techField != TechField.PLANETOLOGY)
			{
				total += PlanetologyPercentage;
			}
			if (!PropulsionLocked && techField != TechField.PROPULSION)
			{
				total += PropulsionPercentage;
			}
			if (!WeaponLocked && techField != TechField.WEAPON)
			{
				total += WeaponPercentage;
			}

			return total;
		}

		private void UpdateValues()
		{
			//After researching or obtaining a technology, update all values
			FuelRange = 3;
			foreach (var tech in ResearchedPropulsionTechs)
			{
				if (tech.FuelRange > FuelRange)
				{
					FuelRange = tech.FuelRange;
				}
			}
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Technologies");
			writer.WriteStartElement("Computer");
			writer.WriteAttributeString("Researching", WhichComputerBeingResearched == null ? string.Empty : WhichComputerBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ComputerPercentage.ToString());
			writer.WriteAttributeString("Locked", ComputerLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedComputerTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedComputerTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Construction");
			writer.WriteAttributeString("Researching", WhichConstructionBeingResearched == null ? string.Empty : WhichConstructionBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ConstructionPercentage.ToString());
			writer.WriteAttributeString("Locked", ConstructionLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedConstructionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedConstructionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("ForceField");
			writer.WriteAttributeString("Researching", WhichForceFieldBeingResearched == null ? string.Empty : WhichForceFieldBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", ForceFieldPercentage.ToString());
			writer.WriteAttributeString("Locked", ForceFieldLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedForceFieldTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedForceFieldTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Planetology");
			writer.WriteAttributeString("Researching", WhichPlanetologyBeingResearched == null ? string.Empty : WhichPlanetologyBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", PlanetologyPercentage.ToString());
			writer.WriteAttributeString("Locked", PlanetologyLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedPlanetologyTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedPlanetologyTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Propulsion");
			writer.WriteAttributeString("Researching", WhichPropulsionBeingResearched == null ? string.Empty : WhichPropulsionBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", PropulsionPercentage.ToString());
			writer.WriteAttributeString("Locked", PropulsionLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedPropulsionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedPropulsionTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteStartElement("Weapon");
			writer.WriteAttributeString("Researching", WhichWeaponBeingResearched == null ? string.Empty : WhichWeaponBeingResearched.TechName);
			writer.WriteAttributeString("Percentage", WeaponPercentage.ToString());
			writer.WriteAttributeString("Locked", WeaponLocked ? "True" : "False");
			writer.WriteStartElement("Researched");
			foreach (var tech in ResearchedWeaponTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Unresearched");
			foreach (var tech in UnresearchedWeaponTechs)
			{
				writer.WriteElementString("Technology", tech.TechName);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		#endregion
	}
}
