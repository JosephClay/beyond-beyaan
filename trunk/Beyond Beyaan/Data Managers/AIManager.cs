using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class AIManager
	{
		public List<AI> AIs { get; private set; }

		public AIManager()
		{
			AIs = new List<AI>();
			try
			{
				string directory = Path.Combine(Environment.CurrentDirectory, "data");
				directory = Path.Combine(directory, "default");
				directory = Path.Combine(directory, "ai");
				DirectoryInfo di = new DirectoryInfo(directory);
				if (!di.Exists)
				{
					//If it don't exist, create one so users can add races
					di.Create();
				}
				foreach (FileInfo fi in di.GetFiles("*.txt"))
				{
					AI ai = new AI();
					if (ai.Initialize(fi))
					{
						AIs.Add(ai);
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
