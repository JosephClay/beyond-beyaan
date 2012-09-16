using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class RegionTypeManager
	{
		private Dictionary<string, RegionType> regionTypes;

		public void LoadRegionTypes(string filePath, GameMain gameMain)
		{
			regionTypes = new Dictionary<string, RegionType>();

			gameMain.Log(string.Empty);
			gameMain.Log("Loading PlanetTypes from " + filePath);

			XDocument doc = XDocument.Load(filePath);
			gameMain.Log("Getting \"RegionData\" Element");
			XElement root = doc.Element("RegionData");

			XElement regions = root.Element("Regions");

			foreach (XElement element in regions.Elements())
			{
				gameMain.Log(string.Empty);
				RegionType regionType = new RegionType();
				regionType.RegionTypeName = element.Attribute("name").Value;
				regionType.Color = new float[3];
				string[] color = element.Attribute("color").Value.Split(new[] { ',' });
				for (int i = 0; i < 3; i++)
				{
					regionType.Color[i] = float.Parse(color[i]);
				}
				if (element.Attribute("produces") != null)
				{
					string[] values = element.Attribute("produces").Value.Split(new[] { '|' });
					foreach (string value in values)
					{
						string[] split = value.Split(new[] { ',' });
						if (split.Length == 2)
						{
							regionType.Productions.Add(gameMain.resourceManager.GetResource(split[0]), float.Parse(split[1]));
						}
					}
				}
				if (element.Attribute("consumes") != null)
				{
					string[] values = element.Attribute("consumes").Value.Split(new[] { '|' });
					foreach (string value in values)
					{
						string[] split = value.Split(new[] { ',' });
						if (split.Length == 2)
						{
							regionType.Consumptions.Add(gameMain.resourceManager.GetResource(split[0]), float.Parse(split[1]));
						}
					}
				}
				regionTypes.Add(regionType.RegionTypeName, regionType);
			}
		}

		public RegionType GetRegionType(string regionType)
		{
			if (regionTypes.ContainsKey(regionType))
			{
				return regionTypes[regionType];
			}
			return null;
		}
	}
}
