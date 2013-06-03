using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class StarTypeManager
	{
		private Dictionary<string, StarType> starTypes;
		private List<string> codes; //Used for random picking

		public bool LoadStarTypes(string filePath, string spriteFilePath, string shaderDirectory, SectorTypeManager sectorTypeManager, GameMain gameMain, out string reason)
		{
			try
			{
				starTypes = new Dictionary<string, StarType>();
				codes = new List<string>();

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
					starType.Code = element.Attribute("code").Value;
					gameMain.Log("Loading star type \"" + starType.Code + "\"");
					starType.Description = element.Attribute("description").Value;

					gameMain.Log("Attempting to load sprite with value of \"" + element.Attribute("sprite").Value + "\"");
					starType.SpriteName = element.Attribute("sprite").Value;

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
						starType.Shader = gameMain.ShaderManager.GetShader(element.Attribute("shader").Value);
					}

					foreach (XElement sectorTypes in element.Elements())
					{
						if (sectorTypes.Attribute("max") != null)
						{
							starType.MaxAmountForSectorType[sectorTypes.Name.ToString()] = int.Parse(sectorTypes.Attribute("max").Value);
						}
						foreach (XElement sectorType in sectorTypes.Elements())
						{
							SectorObjectType type = sectorTypeManager.GetType(sectorType.Attribute("code").Value);
							if (type == null)
							{
								gameMain.Log("SectorType failed to load, invalid type: " + sectorType.Attribute("code").Value);
								continue;
							}
							starType.SectorObjectTypesInSystem[type] = sectorType.Attribute("amount").Value;
						}
					}

					starTypes.Add(starType.Code, starType);
					codes.Add(starType.Code);
					gameMain.Log("Star type \"" + starType.Code + "\" loaded!");
					gameMain.Log(string.Empty);
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

		public StarType GetType(string code)
		{
			if (starTypes.ContainsKey(code))
			{
				return starTypes[code];
			}
			return null;
		}

		public StarType GetRandomStarType(Random r)
		{
			//Galaxy generation will call this for determining which kind of star the current point should be
			if (starTypes.Count > 0)
			{
				return starTypes[codes[r.Next(starTypes.Count)]];
			}
			return null;  //Should have all probability filled out
		}
	}
}
