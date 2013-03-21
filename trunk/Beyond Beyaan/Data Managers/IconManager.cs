using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class IconManager
	{
		private Dictionary<string, Icon> icons;

		public IconManager()
		{
			icons = new Dictionary<string, Icon>();
		}

		public bool Initialize(string dataDirectory, string graphicDirectory, out string reason)
		{
			try
			{
				XDocument doc = XDocument.Load(Path.Combine(dataDirectory, "icons.xml"));
				XElement root = doc.Element("Icons");

				GorgonLibrary.Graphics.Sprite iconGraphic = new GorgonLibrary.Graphics.Sprite("MainIconGraphic", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicDirectory, "icons.png")));

				foreach (XElement element in root.Elements())
				{
					string name = element.Attribute("name").Value;
					if (icons.ContainsKey(name))
					{
						reason = "Duplicate icon name: " + name;
						return false;
					}
					Icon icon = new Icon(name, element, iconGraphic);
					icons.Add(name, icon);
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

		public Icon GetIcon(string iconName)
		{
			if (icons.ContainsKey(iconName))
			{
				return icons[iconName];
			}
			//An more friendly error message than "Key does not exist"
			throw new Exception("Trying to get icon '" + iconName + "' but it doesn't exist");
		}
	}
}
