using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class MasterTechnologyList
	{
		private List<TechnologyItem>[] genericTechnologies;
		//private Dictionary<Race, List<TechnologyItem>[]> racialTechnologies;
		private List<string> technologyNames; //This is to ensure that no two technologies have the same name

		public void ResetAll()
		{
			technologyNames = new List<string>();
			genericTechnologies = new List<TechnologyItem>[6];
			//racialTechnologies = new Dictionary<Race, List<TechnologyItem>[]>();
		}

		public void LoadGenericTechnologies(string filePath)
		{
			for (int i = 0; i < genericTechnologies.Length; i++)
			{
				genericTechnologies[i] = new List<TechnologyItem>();
			}
			XDocument doc = XDocument.Load(Path.Combine(filePath, "technologies.xml"));
			XElement root = doc.Element("Technologies");
			foreach (XElement element in root.Elements())
			{
				int whichField = TechnologyManager.ELECTRONICS;
				TechnologyField whichFieldEnum = TechnologyField.ELECTRONICS;
				switch (element.Attribute("name").Value)
				{
					case "Electronics":
						{
							whichField = TechnologyManager.ELECTRONICS;
							whichFieldEnum = TechnologyField.ELECTRONICS;
						} break;
					case "Metallurgy":
						{
							whichField = TechnologyManager.METALLURGY;
							whichFieldEnum = TechnologyField.METALLURGY;
						} break;
					case "Energy":
						{
							whichField = TechnologyManager.ENERGY;
							whichFieldEnum = TechnologyField.ENERGY;
						} break;
					case "Chemistry":
						{
							whichField = TechnologyManager.CHEMISTRY;
							whichFieldEnum = TechnologyField.CHEMISTRY;
						} break;
					case "Physics":
						{
							whichField = TechnologyManager.PHYSICS;
							whichFieldEnum = TechnologyField.PHYSICS;
						} break;
					case "Construction":
						{
							whichField = TechnologyManager.CONSTRUCTION;
							whichFieldEnum = TechnologyField.CONSTRUCTION;
						} break;
				}
				foreach (XElement subElement in element.Elements())
				{
					TechnologyItem item = new TechnologyItem(subElement, whichFieldEnum, Path.Combine(Path.Combine(filePath, "Scripts"), "Technology"));
					if (technologyNames.Contains(item.Name))
					{
						throw new Exception("Duplicate Technology Name: " + item.Name);
					}
					technologyNames.Add(item.Name);
					genericTechnologies[whichField].Add(item);
				}
			}
		}

		/*public void LoadRacialTechnologies(string raceTechFile, string techScriptDirectory, Race whichRace)
		{
			List<TechnologyItem>[] technologies = new List<TechnologyItem>[6];
			for (int i = 0; i < technologies.Length; i++)
			{
				technologies[i] = new List<TechnologyItem>();
			}
			XDocument doc = XDocument.Load(raceTechFile);
			XElement root = doc.Element("Technologies");
			foreach (XElement element in root.Elements())
			{
				int whichField = TechnologyManager.ELECTRONICS;
				TechnologyField whichFieldEnum = TechnologyField.ELECTRONICS;
				switch (element.Attribute("name").Value)
				{
					case "Electronics":
						{
							whichField = TechnologyManager.ELECTRONICS;
							whichFieldEnum = TechnologyField.ELECTRONICS;
						} break;
					case "Metallurgy":
						{
							whichField = TechnologyManager.METALLURGY;
							whichFieldEnum = TechnologyField.METALLURGY;
						} break;
					case "Energy":
						{
							whichField = TechnologyManager.ENERGY;
							whichFieldEnum = TechnologyField.ENERGY;
						} break;
					case "Chemistry":
						{
							whichField = TechnologyManager.CHEMISTRY;
							whichFieldEnum = TechnologyField.CHEMISTRY;
						} break;
					case "Physics":
						{
							whichField = TechnologyManager.PHYSICS;
							whichFieldEnum = TechnologyField.PHYSICS;
						} break;
					case "Construction":
						{
							whichField = TechnologyManager.CONSTRUCTION;
							whichFieldEnum = TechnologyField.CONSTRUCTION;
						} break;
				}
				foreach (XElement subElement in element.Elements())
				{
					TechnologyItem item = new TechnologyItem(subElement, whichFieldEnum, techScriptDirectory);
					if (technologyNames.Contains(item.Name))
					{
						throw new Exception("Duplicate Technology Name: " + item.Name);
					}
					technologyNames.Add(item.Name);
					technologies[whichField].Add(item);
				}
			}
			racialTechnologies.Add(whichRace, technologies);
		}*/

		public void ValidateEquipment(string mainItem, string mountItem, List<string> modifiers, Race whichRace)
		{
			TechnologyItem mainTech = null;
			TechnologyItem mountTech = null;
			List<TechnologyItem> modifierTechs = new List<TechnologyItem>();
			List<string> modifiersCopy = new List<string>(modifiers);
			foreach (List<TechnologyItem> items in genericTechnologies)
			{
				foreach (TechnologyItem item in items)
				{
					if (mainItem == item.Name)
					{
						mainTech = item;
					}
					else if (mountItem == item.Name)
					{
						mountTech = item;
					}
					else
					{
						int k = -1;
						for (int i = 0; i < modifiersCopy.Count; i++)
						{
							if (modifiersCopy[i] == item.Name)
							{
								modifierTechs.Add(item);
								k = i;
								break;
							}
						}
						if (k >= 0)
						{
							modifiersCopy.RemoveAt(k);
						}
					}
				}
			}
			/*foreach (List<TechnologyItem> items in racialTechnologies[whichRace])
			{
				foreach (TechnologyItem item in items)
				{
					if (mainItem == item.Name)
					{
						mainTech = item;
					}
					else if (mountItem == item.Name)
					{
						mountTech = item;
					}
					else
					{
						int k = -1;
						for (int i = 0; i < modifiersCopy.Count; i++)
						{
							if (modifiersCopy[i] == item.Name)
							{
								modifierTechs.Add(item);
								k = i;
								break;
							}
						}
						if (k >= 0)
						{
							modifiersCopy.RemoveAt(k);
						}
					}
				}
			}*/
			if (mainTech == null)
			{
				throw new Exception("Main item: " + mainItem + " was not found in the technology table for " + whichRace.RaceName + " race");
			}
			if (mainTech.TechType == TechType.INFRASTRUCTURE)
			{
				throw new Exception("Main item: " + mainItem + " is an invalid tech type for ship equipment in " + whichRace.RaceName + " race");
			}
			if (mainTech.TechType != TechType.SPECIAL && mountTech == null)
			{
				throw new Exception("Main item: " + mainItem + " have an invalid or missing mount: " + mountItem + " in " + whichRace.RaceName + " race");
			}
			if (modifiersCopy.Count > 0)
			{
				string modifierItems = string.Empty;
				foreach (string item in modifiersCopy)
				{
					modifierItems += item + ",";
				}
				modifierItems = modifierItems.TrimEnd(new[] {','});
				throw new Exception("Main item: " + mainItem + " with mount: " + mountItem + " has invalid modifiers: " + modifierItems + " in " + whichRace.RaceName + " race");
			}
		}

		public ShipDesign ConvertStartingShipToRealShip(StartingShip startingShip, Race whichRace, IconManager iconManager)
		{
			ShipDesign realShip = new ShipDesign();
			realShip.Name = startingShip.name;
			foreach (Ship shipClass in whichRace.ShipClasses)
			{
				if (shipClass.ClassName == startingShip.shipClass)
				{
					realShip.ShipClass = shipClass;
					break;
				}
			}
			realShip.WhichStyle = startingShip.style;
			realShip.Race = whichRace;
			foreach (StartingEquipment equipment in startingShip.equipment)
			{
				TechnologyItem mainItem = ConvertStartingTechToRealTech(equipment.mainItem, whichRace);
				TechnologyItem mountItem = null;
				List<TechnologyItem> modifierItems = new List<TechnologyItem>();
				Dictionary<string, object> modifiableValues = new Dictionary<string, object>();
				if (mainItem.TechType != TechType.SPECIAL)
				{
					mountItem = ConvertStartingTechToRealTech(equipment.mountItem, whichRace);
					foreach (string modifierItem in equipment.modifiers)
					{
						modifierItems.Add(ConvertStartingTechToRealTech(modifierItem, whichRace));
					}
				}
				foreach (string modifiableValue in equipment.modifiableValues)
				{
					string[] values = modifiableValue.Split(new[] {','});
					modifiableValues.Add(values[0], values[1]);
				}
				Equipment newEquipment = new Equipment(Utility.TechTypeToEquipmentType(mainItem.TechType), mainItem, mountItem, modifierItems, modifiableValues, iconManager);
				realShip.Equipments.Add(newEquipment);
			}
			return realShip;
		}

		public TechnologyItem ConvertStartingTechToRealTech(string techName, Race whichRace)
		{
			foreach (List<TechnologyItem> items in genericTechnologies)
			{
				foreach (TechnologyItem item in items)
				{
					if (techName == item.Name)
					{
						return item;
					}
				}
			}
			/*foreach (List<TechnologyItem> items in racialTechnologies[whichRace])
			{
				foreach (TechnologyItem item in items)
				{
					if (techName == item.Name)
					{
						return item;
					}
				}
			}*/
			throw new Exception("This shouldn't be reached, missing technology in tech table after everything is validated: " + techName);
		}

		public Dictionary<TechnologyItem, int>[] GetRandomizedTechnologies(Race race)
		{
			Random r = new Random(); //The randomizitator, mwhahaha
			Dictionary<TechnologyItem, int>[] technologies = new Dictionary<TechnologyItem, int>[6];
			int percentage = 80; //Replace with race's creativity trait here
			for (int i = 0; i < technologies.Length; i++)
			{
				technologies[i] = new Dictionary<TechnologyItem, int>();
				foreach (TechnologyItem item in genericTechnologies[i])
				{
					if (item.IsRequired)
					{
						int level = item.TechLevel + (r.Next(0, item.FudgeFactor) - (item.FudgeFactor / 2));
						technologies[i].Add(item, level);
					}
					else
					{
						if (r.Next(0, 100) < percentage) //Randomly exclude some technologies
						{
							int level = item.TechLevel + (r.Next(0, item.FudgeFactor) - (item.FudgeFactor / 2));
							technologies[i].Add(item, level);
						}
					}
				}
				/*foreach (TechnologyItem item in racialTechnologies[race][i])
				{
					if (item.IsRequired)
					{
						int level = item.TechLevel + (r.Next(0, item.FudgeFactor) - (item.FudgeFactor / 2));
						technologies[i].Add(item, level);
					}
					else
					{
						if (r.Next(0, 100) < percentage) //Randomly exclude some technologies
						{
							int level = item.TechLevel + (r.Next(0, item.FudgeFactor) - (item.FudgeFactor / 2));
							technologies[i].Add(item, level);
						}
					}
				}*/
			}

			return technologies;
		}
	}
}
