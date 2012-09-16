using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class RaceManager
	{
		public List<Race> Races { get; private set; }

		public RaceManager()
		{
			Races = new List<Race>();
		}

		public RaceManager(string baseDirectory, string graphicDirectory, MasterTechnologyList masterTechnologyList, ShipScriptManager shipScriptManager, string techScriptDirectory, IconManager iconManager, ResourceManager resourceManager)
		{
			Races = new List<Race>();

			XDocument raceDoc = XDocument.Load(Path.Combine(baseDirectory, "races.xml"));
			XElement root = raceDoc.Element("Races");

			foreach (XElement raceElement in root.Elements())
			{
				Race race = new Race();
				if (race.Initialize(raceElement, graphicDirectory, shipScriptManager, iconManager, resourceManager))
				{
					//masterTechnologyList.LoadRacialTechnologies(Path.Combine(raceDirectory.FullName, "race technologies.xml"), techScriptDirectory, race);
					race.ValidateShipDesigns(masterTechnologyList);
					Races.Add(race);
				}
			}
		}
	}
}
