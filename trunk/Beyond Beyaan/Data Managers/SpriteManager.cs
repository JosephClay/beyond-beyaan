using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class SpriteManager
	{
		public Dictionary<string, BaseSprite> Sprites { get; private set; }

		public SpriteManager()
		{
			Sprites = new Dictionary<string, BaseSprite>();
		}

		public bool LoadSprites(DirectoryInfo baseDirectory, DirectoryInfo graphicDirectory, out string reason)
		{
			string file = Path.Combine(baseDirectory.FullName, "sprites.xml");
			if (!File.Exists(file))
			{
				reason = "Sprites.xml file does not exist";
				return false;
			}

			XDocument doc = XDocument.Load(file);
			XElement root = doc.Element("Sprites");
			foreach (XElement sprite in root.Elements())
			{
				var newSprite = new BaseSprite();
				if (!newSprite.LoadSprite(sprite, graphicDirectory, out reason))
				{
					return false;
				}
				Sprites.Add(newSprite.Name, newSprite);
			}
			reason = null;
			return true;
		}

		public BBSprite GetSprite(string name)
		{
			if (Sprites.ContainsKey(name))
			{
				return new BBSprite(Sprites[name]);
			}
			return null;
		}
	}
}
