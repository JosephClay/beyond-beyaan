using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Managers
{
	public class GameConfiguration
	{
		public bool ShowTutorial { get; set; }

		public bool LoadConfiguration(string filePath, out string reason)
		{
			SetDefaults();
			try
			{
				XDocument file = XDocument.Load(filePath);
				XElement root = file.Element("Configuration");

				foreach (XAttribute attribute in root.Attributes())
				{
					switch (attribute.Name.ToString().ToLower())
					{
						case "showtutorial": ShowTutorial = bool.Parse(attribute.Value);
							break;
					}
				}
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			reason = null;
			return true;
		}

		public void SetDefaults()
		{
			ShowTutorial = false;
		}
	}
}
