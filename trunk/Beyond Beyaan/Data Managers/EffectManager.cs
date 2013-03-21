using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	class EffectManager
	{
		private List<Effect> effects;
		private List<string> effectNames;

		public EffectManager()
		{
			effects = new List<Effect>();
			effectNames = new List<string>();
		}

		public bool Initialize(string dataDirectory)
		{
			XDocument doc = XDocument.Load(Path.Combine(dataDirectory, "effects.xml"));
			XElement root = doc.Element("Effects");

			//GorgonLibrary.Graphics.Sprite particleGraphic = new GorgonLibrary.Graphics.Sprite("MainParticleGraphic", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicDirectory, "particles.png")));

			foreach (XElement element in root.Elements())
			{
				string name = element.Attribute("name").Value;
				if (effectNames.Contains(name))
				{
					return false;
				}
				effectNames.Add(name);
				Effect effect = new Effect(element, Path.Combine(Path.Combine(dataDirectory, "Scripts"), "Effect"));
				effects.Add(effect);
			}
			return true;
		}

		public EffectInstance SpawnEffect(string effectName, ShipInstance ship, Dictionary<string, object> values)
		{
			for (int i = 0; i < effectNames.Count; i++)
			{
				if (effectName == effectNames[i])
				{
					return new EffectInstance(effects[i], ship, values);
				}
			}
			return null;
		}
	}
}
