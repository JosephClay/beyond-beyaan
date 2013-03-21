using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class PlanetTypeManager
	{
		private Dictionary<string, PlanetType> planetTypes;
		private int minRegions;
		private int maxRegions;

		public void LoadPlanetTypes(string filePath, string spriteFilePath, string graphicsDirectory, GameMain gameMain)
		{
			planetTypes = new Dictionary<string, PlanetType>();

			gameMain.Log(string.Empty);
			gameMain.Log("Loading PlanetTypes from " + filePath);

			XDocument doc = XDocument.Load(filePath);
			gameMain.Log("Getting \"Planets\" Element");
			XElement root = doc.Element("Planets");

			minRegions = int.Parse(root.Attribute("minRegions").Value);
			maxRegions = int.Parse(root.Attribute("maxRegions").Value);

			gameMain.Log("Creating PlanetsSpriteSheet");
			GorgonLibrary.Graphics.Sprite planetsSprite = new GorgonLibrary.Graphics.Sprite("planetsSpriteSheet", GorgonLibrary.Graphics.Image.FromFile(spriteFilePath));

			foreach (XElement element in root.Elements())
			{
				gameMain.Log(string.Empty);
				PlanetType planetType = new PlanetType();
				planetType.Name = element.Attribute("name").Value;
				planetType.InternalName = element.Attribute("internalName").Value;
				gameMain.Log("Loading star type \"" + planetType.InternalName + "\"");
				planetType.Description = element.Attribute("description").Value;
				gameMain.Log("Attempting to bool.parse inhabitable with value of \"" + element.Attribute("inhabitable").Value + "\"");
				planetType.Inhabitable = bool.Parse(element.Attribute("inhabitable").Value);
				planetType.DefaultRegions = element.Attribute("defaultRegionType").Value.Split(new[] { '|' });
				gameMain.Log("Attempting to int.parse pollutionThreshold with value of \"" + element.Attribute("pollutionThreshold").Value + "\"");
				planetType.PollutionThreshold = int.Parse(element.Attribute("pollutionThreshold").Value);
				if (element.Attribute("terraformCost") != null)
				{
					gameMain.Log("Attempting to int.parse terraformCost with value of \"" + element.Attribute("terraformCost").Value + "\"");
					planetType.CostForTerraforming = int.Parse(element.Attribute("terraformCost").Value);
				}
				if (element.Attribute("requiredProductionCapacity") != null)
				{
					gameMain.Log("Attempting to int.parse requiredProductionCapacity with value of \"" + element.Attribute("requiredProductionCapacity").Value + "\"");
					planetType.ProductionCapacityRequiredForTerraforming = int.Parse(element.Attribute("requiredProductionCapacity").Value);
				}
				if (element.Attribute("planetDegradesTo") != null)
				{
					planetType.PlanetDegradesTo = element.Attribute("planetDegradesTo").Value;
				}
				if (element.Attribute("planetTerraformsTo") != null)
				{
					planetType.PlanetTerraformsTo = element.Attribute("planetTerraformsTo").Value;
				}
				gameMain.Log("Attempting to int.parse spriteX with value of \"" + element.Attribute("spriteX").Value + "\"");
				int x = int.Parse(element.Attribute("spriteX").Value);
				gameMain.Log("Attempting to int.parse spriteY with value of \"" + element.Attribute("spriteY").Value + "\"");
				int y = int.Parse(element.Attribute("spriteY").Value);
				GorgonLibrary.Graphics.Sprite planetSprite = new GorgonLibrary.Graphics.Sprite(planetType.InternalName, planetsSprite.Image, x, y, 40, 40);
				planetType.Sprite = planetSprite;

				gameMain.Log("Attempting to load large view");
				planetType.LargeSprite = new GorgonLibrary.Graphics.Sprite(planetType.InternalName + "_largeSprite", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicsDirectory, element.Attribute("largeGraphic").Value)));

				gameMain.Log("Attempting to load ground view");
				planetType.GroundView = new GorgonLibrary.Graphics.Sprite(planetType.InternalName + "_groundView", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicsDirectory, element.Attribute("groundGraphic").Value)));

				planetTypes.Add(planetType.InternalName, planetType);
				gameMain.Log("Planet type \"" + planetType.InternalName + "\" loaded!");
				gameMain.Log(string.Empty);
			}
		}

		public PlanetType GetPlanet(string planetName)
		{
			if (planetTypes.ContainsKey(planetName))
			{
				return planetTypes[planetName];
			}
			return null;
		}

		public Planet GenerateRandomPlanet(string planetName, string planetNumerical, string planetType, Random r, StarSystem system, RegionTypeManager regionTypeManager)
		{
			if (planetTypes.ContainsKey(planetType))
			{
				return new Planet(planetName, planetNumerical, planetTypes[planetType], r, system, r.Next(minRegions, maxRegions + 1), regionTypeManager);
			}
			int random = r.Next(planetTypes.Count);
			foreach (KeyValuePair<string, PlanetType> randomPlanetType in planetTypes)
			{
				random--;
				if (random < 0)
				{
					return new Planet(planetName, planetNumerical, randomPlanetType.Value, r, system, r.Next(minRegions, maxRegions + 1), regionTypeManager);
				}
			}
			return null;
		}
	}
}
