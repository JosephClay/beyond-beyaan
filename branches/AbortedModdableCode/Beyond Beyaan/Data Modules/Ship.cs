using System;
using System.Collections.Generic;
using System.Xml.Linq;
using GorgonLibrary.Graphics;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Ship
	{
		#region Properties
		public string ClassName { get; private set; }
		public int Size { get; private set; }
		public int NumberOfStyles { get; private set; }
		public List<Sprite> Sprites { get; private set; }
		public List<Icon> DisplayIcons { get; private set; }
		public List<Icon> DesignIcons { get; private set; }

		public Dictionary<string, object> Values { get; private set; }
		public ShipScript ShipScript { get; private set; }
		#endregion

		#region Constructors
		public Ship(XElement shipClass, Sprite graphicSprite, string raceName, ShipScript shipScript, IconManager iconManager)
		{

			Values = new Dictionary<string,object>();

			foreach (XAttribute attribute in shipClass.Attributes())
			{
				KeyValuePair<string, object> newValue = ConvertElementToObject(attribute.Name.LocalName, attribute.Value);
				Values.Add(newValue.Key, newValue.Value);
			}

			ClassName = (string)Values["name"];
			Size = (int)Values["size"];

			int i = 0;
			Sprites = new List<Sprite>();
			foreach (XElement element in shipClass.Elements())
			{
				int x = int.Parse(element.Attribute("xPos").Value);
				int y = int.Parse(element.Attribute("yPos").Value);
				int spriteSize = int.Parse(element.Attribute("size").Value);
				Sprite ship = new Sprite(raceName + Size + ClassName + i.ToString(), graphicSprite.Image, x, y, spriteSize, spriteSize);
				Sprites.Add(ship);
				i++;
			}
			if (Sprites.Count == 0)
			{
				throw new Exception("Class " + ClassName + " have no sprites");
			}
			NumberOfStyles = Sprites.Count;
			ShipScript = shipScript;

			DisplayIcons = new List<Icon>();
			DesignIcons = new List<Icon>();
			string[] icons = ((string)Values["shipIcons"]).Split(new[] { '|' });
			foreach (string icon in icons)
			{
				DisplayIcons.Add(iconManager.GetIcon(icon));
			}
			icons = ((string)Values["designIcons"]).Split(new[] { '|' });
			foreach (string icon in icons)
			{
				DesignIcons.Add(iconManager.GetIcon(icon));
			}
		}
		#endregion

		private KeyValuePair<string, object> ConvertElementToObject(string name, string value)
		{
			switch (name)
			{
				case "size": return new KeyValuePair<string, object>(name, int.Parse(value));
				default: return new KeyValuePair<string, object>(name, value);
			}
		}
	}
}
