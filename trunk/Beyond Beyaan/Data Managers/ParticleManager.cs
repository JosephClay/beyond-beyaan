using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ParticleManager
	{
		private List<Particle> particles;
		private List<string> particleNames; //For collision checking - don't want duplicate particle names, and for lookup

		public ParticleManager()
		{
			particles = new List<Particle>();
			particleNames = new List<string>();
		}

		public bool Initialize(string dataDirectory, string graphicDirectory)
		{
			XDocument doc = XDocument.Load(Path.Combine(dataDirectory, "particles.xml"));
			XElement root = doc.Element("Particles");

			//GorgonLibrary.Graphics.Sprite particleGraphic = new GorgonLibrary.Graphics.Sprite("MainParticleGraphic", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicDirectory, "particles.png")));

			foreach (XElement element in root.Elements())
			{
				string name = element.Attribute("name").Value;
				if (particleNames.Contains(name))
				{
					return false;
				}
				particleNames.Add(name);
				Particle particle = new Particle(element, graphicDirectory, Path.Combine(Path.Combine(dataDirectory, "Scripts"), "Particle"));
				particles.Add(particle);
			}
			return true;
		}

		public ParticleInstance SpawnParticle(string particleName, ShipInstance ship, Dictionary<string, object> values)
		{
			for (int i = 0; i < particleNames.Count; i++)
			{
				if (particleName == particleNames[i])
				{
					return new ParticleInstance(particles[i], ship, values);
				}
			}
			return null;
		}
	}
}
