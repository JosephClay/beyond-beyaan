using System;
using System.Collections.Generic;
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
		}

		public bool Initialize(DirectoryInfo directory, SpriteManager spriteManager, Random r, out string reason)
		{
			try
			{
				string path = Path.Combine(directory.FullName, "races");
				DirectoryInfo di = new DirectoryInfo(path);
				if (!di.Exists)
				{
					//If it don't exist, create one so users can add races
					di.Create();
				}
				foreach (FileInfo fi in di.GetFiles("*.xml"))
				{
					Race race = new Race();
					if (!race.Initialize(fi, spriteManager, r, out reason))
					{
						return false;
					}
					Races.Add(race);
				}
				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
		}
	}
}
