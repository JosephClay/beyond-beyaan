using System;
using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class AIManager
	{
		public List<AI> AIs { get; private set; }

		public bool Initialize(DirectoryInfo path, out string reason)
		{
			AIs = new List<AI>();
			try
			{
				DirectoryInfo di = new DirectoryInfo(Path.Combine(path.FullName, "AI"));
				foreach (FileInfo fi in di.GetFiles("*.txt"))
				{
					AI ai = new AI();
					if (ai.Initialize(fi))
					{
						AIs.Add(ai);
					}
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
