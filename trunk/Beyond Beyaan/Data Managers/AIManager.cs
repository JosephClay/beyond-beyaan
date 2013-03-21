using System;
using System.Collections.Generic;
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
				directory = Path.Combine(directory, "demo");
				directory = Path.Combine(directory, "ai");
				DirectoryInfo di = new DirectoryInfo(directory);
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
