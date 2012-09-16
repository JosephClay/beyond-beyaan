using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ShipScriptManager
	{
		private List<ShipScript> ShipScripts { get; set; }
		private List<string> ScriptNames { get; set; }

		public void LoadShipScripts(string directoryPath)
		{
			ShipScripts = new List<ShipScript>();
			ScriptNames = new List<string>();
			DirectoryInfo di = new DirectoryInfo(directoryPath);

			FileInfo[] files = di.GetFiles("*.cs");
			foreach (FileInfo file in files)
			{
				ShipScript shipScript = new ShipScript(file);
				ShipScripts.Add(shipScript);
				ScriptNames.Add(file.Name.Substring(0, file.Name.IndexOf(".cs")));
			}
		}

		public ShipScript GetShipScript(string scriptName)
		{
			for (int i = 0; i < ShipScripts.Count; i++)
			{
				if (ScriptNames[i] == scriptName)
				{
					return ShipScripts[i];
				}
			}
			return null;
		}
	}
}
