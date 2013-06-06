using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class RaceManager
	{
		public List<Race> Races { get; private set; }

		public RaceManager()
		{
			Races = new List<Race>();
			try
			{
				string directory = Path.Combine(Environment.CurrentDirectory, "data");
				directory = Path.Combine(directory, "default");
				directory = Path.Combine(directory, "races");
				DirectoryInfo di = new DirectoryInfo(directory);
				if (!di.Exists)
				{
					//If it don't exist, create one so users can add races
					di.Create();
				}
				foreach (FileInfo fi in di.GetFiles("*.txt"))
				{
					Race race = new Race();
					if (race.Initialize(fi))
					{
						Races.Add(race);
					}
				}
			}
			catch
			{
				//Do nothing, not much we can do at this point
			}
		}
	}
}
