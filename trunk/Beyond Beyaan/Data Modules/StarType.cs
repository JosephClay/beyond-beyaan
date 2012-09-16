using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public class StarType
	{
		#region Graphics
		public float[] ShaderValue { get; set; }
		public GorgonLibrary.Graphics.FXShader Shader { get; set; }
		public GorgonLibrary.Graphics.Sprite Sprite { get; set; }
		#endregion

		#region Star Properties
		public string Name { get; set; }
		public string InternalName { get; set; }
		public string Description { get; set; }
		public Dictionary<string, int> AllowedPlanets { get; set; }
		public int MaxPlanets { get; set; }
		public int MinPlanets { get; set; }
		public int Probability { get; set; }
		public bool Inhabitable { get; set; } //This is to prevent home systems from being set up here (black holes for example)
		public int Width { get; set; }
		public int Height { get; set; }
		#endregion

		public string GetRandomPlanetType(Random r)
		{
			int value = r.Next(100);

			foreach (KeyValuePair<string, int> planet in AllowedPlanets)
			{
				value -= planet.Value;
				if (value < 0)
				{
					return planet.Key;
				}
			}
			return null;
		}
	}
}
