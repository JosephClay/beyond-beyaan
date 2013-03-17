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
			vars["Size"] = "upDown|30|-1|10|30";

			return vars;
		}


		public static List<Dictionary<string, object>> Generate(Dictionary<string, string> vars)
		{
			if (!int.TryParse(vars["Size"], out radius)) return null;
			if (!int.TryParse(vars["Max Number of Stars"], out maxStars)) return null;

			Random rand = new Random();
			List<Dictionary<string, object>> stars = new List<Dictionary<string, object>>();

			string[,] galaxy = new string[radius, radius];

			for (int i = 0; i < maxStars; i++)
			{
				int whichStar = rand.Next(8);

				int size = 0;
				string starType = "Yellow Star";

				switch (whichStar)
				{
					case 0: 
						size = 2;
						starType = "Yellow Star";
						break;
					case 1:
						size = 2;
						starType = "Blue Star";
						break;
					case 2:
						size = 2;
						starType = "Orange Star";
						break;
					case 3:
						size = 2;
						starType = "White Star";
						break;
					case 4:
						size = 2;
						starType = "Green Star";
						break;
					case 5:
						size = 2;
						starType = "Red Giant";
						break;
					case 6:
						size = 2;
						starType = "Black Dwarf";
						break;
					case 7:
						size = 2;
						starType = "Black Hole";
						break;
				}

				bool occupied = true;
				int attempts = 0;
				int x = 0;
				int y = 0;

				while (occupied)
				{
					attempts++;
					if (attempts > 5)
					{
						break;
					}
					occupied = false;

					x = rand.Next(radius);
					y = rand.Next(radius);

					for (int k = -1; k < 1 + size; k++)
					{
						for (int j = -1; j < 1 + size; j++)
						{
							if (x + k >= 0 && x + k < radius && y + j >= 0 && y + j < radius && galaxy[x + k, y + j] != null)
							{
								occupied = true;
								break;
							}
						}
						if (occupied)
						{
							break;
						}
					}
				}
				if (attempts > 5)
				{
					continue;
				}

				for (int k = 0; k < size; k++)
				{
					for (int j = 0; j < size; j++)
					{
						if (x + k >= 0 && x + k < radius && y + j >= 0 && y + j < radius)
						{
							galaxy[x + k, y + j] = starType;
						}
					}
				}

				Dictionary<string, object> newStar = new Dictionary<string, object>();
				newStar["type"] = starType;
				newStar["x"] = x;
				newStar["y"] = y;

				stars.Add(newStar);
			}

			return stars;
		}

	}
}