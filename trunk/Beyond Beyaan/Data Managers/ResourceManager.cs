using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;
using System.Globalization;

namespace Beyond_Beyaan.Data_Managers
{
	public class ResourceManager
	{
		private Dictionary<string, Resource> resources;

		public ResourceManager()
		{
			resources = new Dictionary<string, Resource>();
		}

		public bool Initialize(string dataDirectory, IconManager iconManager, out string reason)
		{
			try
			{
				XDocument doc = XDocument.Load(Path.Combine(dataDirectory, "resources.xml"));
				XElement root = doc.Element("Resources");

				foreach (XElement element in root.Elements())
				{
					Resource newResource = new Resource();
					newResource.Name = element.Attribute("name").Value;
					newResource.Abbreviation = element.Attribute("abbreviation").Value;
					string icon = element.Attribute("icon").Value;
					newResource.Icon = iconManager.GetIcon(icon);
					newResource.Storable = bool.Parse(element.Attribute("storable").Value);
					if (element.Attribute("convertsTo") != null)
					{
						newResource.ConvertsTo = resources[element.Attribute("convertsTo").Value];
						newResource.ConversionRatio = float.Parse(element.Attribute("ratio").Value, CultureInfo.InvariantCulture);
					}
					switch (element.Attribute("limitTo").Value)
					{
						case "Planet": newResource.LimitTo = LimitTo.PLANET;
							break;
						case "System": newResource.LimitTo = LimitTo.SYSTEM;
							break;
						case "Empire": newResource.LimitTo = LimitTo.EMPIRE;
							break;
					}
					resources.Add(newResource.Name, newResource);
				}
			}
			catch (Exception exception)
			{
				reason = exception.Message;
				return false;
			}
			reason = null;
			return true;
		}

		public Resource GetResource(string resourceName)
		{
			if (resources.ContainsKey(resourceName))
			{
				return resources[resourceName];
			}
			//An more friendly error message than "Key does not exist"
			throw new Exception("Trying to get resource '" + resourceName + "' but it doesn't exist");
		}
	}
}
