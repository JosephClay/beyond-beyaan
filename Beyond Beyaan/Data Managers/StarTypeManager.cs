using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class StarTypeManager
	{
		private List<StarType> starTypes;

		public void LoadStarTypes(string filePath, string spriteFilePath, string shaderDirectory, GameMain gameMain)
		{
			starTypes = new List<StarType>();

			gameMain.Log(string.Empty);
			gameMain.Log("Loading StarTypes from " + filePath);
			XDocument doc = XDocument.Load(filePath);
			gameMain.Log("Getting \"Stars\" Element");
			XElement root = doc.Element("Stars");

			gameMain.Log("Creating StarsSpriteSheet");
			GorgonLibrary.Graphics.Sprite starsSprite = new GorgonLibrary.Graphics.Sprite("starsSpriteSheet", GorgonLibrary.Graphics.Image.FromFile(spriteFilePath));

			foreach (XElement element in root.Elements())
			{
				gameMain.Log(string.Empty);
				StarType starType = new StarType();
				starType.Name = element.Attribute("name").Value;
				starType.InternalName = element.Attribute("internalName").Value;
				gameMain.Log("Loading star type \"" + starType.InternalName + "\"");
				starType.Description = element.Attribute("description").Value;
				gameMain.Log("Attempting to int.parse maxPlanets with value of \"" + element.Attribute("maxPlanets").Value + "\"");
				starType.MaxPlanets = int.Parse(element.Attribute("maxPlanets").Value);
				gameMain.Log("Attempting to int.parse minPlanets with value of \"" + element.Attribute("minPlanets").Value + "\"");
				starType.MinPlanets = int.Parse(element.Attribute("minPlanets").Value);
				gameMain.Log("Attempting to bool.parse inhabitable with value of \"" + element.Attribute("inhabitable").Value + "\"");
				starType.Inhabitable = bool.Parse(element.Attribute("inhabitable").Value);
				if (starType.MinPlanets > starType.MaxPlanets)
				{
					gameMain.Log(starType.InternalName + " min planets value is higher than max planets value");
					throw new Exception(starType.InternalName + " min planets value is higher than max planets value");
				}
				gameMain.Log("Attempting to int.parse probability with value of \"" + element.Attribute("probability").Value + "\"");
				starType.Probability = int.Parse(element.Attribute("probability").Value);
				if (starType.Probability <= 0 || starType.Probability > 100)
				{
					gameMain.Log(starType.InternalName + " probability range is out of valid range (1 - 100)");
					throw new Exception(starType.InternalName + " probability range is out of valid range (1 - 100)");
				}

				gameMain.Log("Attempting to int.parse spriteX with value of \"" + element.Attribute("spriteX").Value + "\"");
				int x = int.Parse(element.Attribute("spriteX").Value);
				gameMain.Log("Attempting to int.parse spriteY with value of \"" + element.Attribute("spriteY").Value + "\"");
				int y = int.Parse(element.Attribute("spriteY").Value);

				gameMain.Log("Attempting to int.parse spriteWidth with value of \"" + element.Attribute("spriteWidth").Value + "\"");
				int width = int.Parse(element.Attribute("spriteWidth").Value);
				gameMain.Log("Attempting to int.parse spriteHeight with value of \"" + element.Attribute("spriteHeight").Value + "\"");
				int height = int.Parse(element.Attribute("spriteHeight").Value);

				GorgonLibrary.Graphics.Sprite starSprite = new GorgonLibrary.Graphics.Sprite(starType.InternalName, starsSprite.Image, x, y, width, height);
				starSprite.SetAxis(width / 2.0f, height / 2.0f);
				starType.Sprite = starSprite;
				starType.Width = width;
				starType.Height = height;

				if (element.Attribute("shaderValue") != null)
				{
					gameMain.Log("Attempting to int.parse shaderValue with value of \"" + element.Attribute("shaderValue").Value + "\"");
					string value = element.Attribute("shaderValue").Value;
					string[] splitValues = value.Split(new[] { ',' });
					float[] shaderValues = new float[splitValues.Length];
					for (int i = 0; i < splitValues.Length; i++)
					{
						shaderValues[i] = float.Parse(splitValues[i], System.Globalization.CultureInfo.InvariantCulture);
					}
					starType.ShaderValue = shaderValues;
				}

				if (element.Attribute("shader") != null)
				{
					gameMain.Log("Attempting to load shader \"" + element.Attribute("shader").Value + "\"");
					starType.Shader = GorgonLibrary.Graphics.FXShader.FromFile(Path.Combine(shaderDirectory, (element.Attribute("shader").Value + ".fx")), GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
				}

				starType.AllowedPlanets = new Dictionary<string, int>();
				foreach (XElement planet in element.Elements())
				{
					starType.AllowedPlanets.Add(planet.Attribute("internalName").Value, int.Parse(planet.Attribute("probability").Value));
				}
				starTypes.Add(starType);
				gameMain.Log("Star type \"" + starType.InternalName + "\" loaded!");
				gameMain.Log(string.Empty);
			}
		}

		public StarType GetRandomStarType(Random r)
		{
			//Galaxy generation will call this for determining which kind of star the current point should be
			int value = r.Next(100);
			foreach (StarType starType in starTypes)
			{
				value -= starType.Probability;
				if (value < 0)
				{
					return starType;
				}
			}
			return null;  //Should have all probability filled out
		}
	}
}
