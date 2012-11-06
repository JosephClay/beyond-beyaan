using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class GalaxyGenerator
	{
		static int maxStars;
		static int radius;
		
		/*public static void Main()
		{
			GalaxyGenerator gal = new GalaxyGenerator();
		}*/

		public static Dictionary<string, string> GetVariables()
		{
			Dictionary<string, string> vars = new Dictionary<string, string>();

			vars["Max Number of Stars"] = "upDown|25|-1|25|100";
			vars["Radius"] = "upDown|30|-1|10|30";

			return vars;
		}


		public static List<List<int>> Generate(Dictionary<string, string> vars)
		{
			if (!int.TryParse(vars["Radius"], out radius)) return null;
			if (!int.TryParse(vars["Max Number of Stars"], out maxStars)) return null;

			Random rand = new Random();
			List<List<int>> stars = new List<List<int>>();
			int size = radius * 2;

			for (int i = 0; i < maxStars; i++)
			{
				int x = rand.Next(size) - radius;
				int y = rand.Next(size) - radius;
				int s = 1;
				
				if (x + s > size || y + s > size || (x*x + y*y) > radius*radius)
				{
					i--;
					continue;
				}
				x += radius;
				y += radius;

				List<int> l = new List<int>();
				l.Add(x);
				l.Add(y);
				l.Add(s);
				stars.Add(l);
			}

			return stars;
		}

	}
}