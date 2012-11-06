using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class GalaxyGenerator
	{
		static int numStars;
		static int spread;
		
		public static void Main()
		{
			GalaxyGenerator gal = new GalaxyGenerator();
		}

		public static Dictionary<string, string> GetVariables()
		{
			Dictionary<string, string> vars = new Dictionary<string, string>();

			vars["Number of Stars"] = "upDown|50|200|10|200";
			vars["Spread Factor"] = "upDown|5|25|1|5";

			return vars;
		}


		public static List<List<int>> Generate(Dictionary<string, string> vars)
		{
			if (!int.TryParse(vars["Spread Factor"], out spread)) return null;
			if (!int.TryParse(vars["Number of Stars"], out numStars)) return null;

			Random rand = new Random();
			List<List<int>> stars = new List<List<int>>();
			int size = (int)Math.Sqrt(numStars) * spread;

			for (int i = 0; i < numStars; i++)
			{
				int x = rand.Next(size);
				int y = rand.Next(size);
				int s = 1;

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