using System;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Managers
{
	public static class GameConfiguration
	{
		public static bool ShowTutorial { get; set; }
		public static string FirstScreen { get; private set; }
		public static string ErrorDialog { get; private set; }

		public static bool LoadConfiguration(string filePath, out string reason)
		{
			try
			{
				XDocument file = XDocument.Load(filePath);
				XElement root = file.Element("Configuration");

				foreach (XAttribute attribute in root.Attributes())
				{
					switch (attribute.Name.ToString().ToLower())
					{
						case "firstscreen": FirstScreen = attribute.Value;
							break;
						case "errordialog": ErrorDialog = attribute.Value;
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
	}
}
