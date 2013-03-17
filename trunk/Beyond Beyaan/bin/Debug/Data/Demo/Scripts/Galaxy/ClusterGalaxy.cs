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
				int whichStar = rand.Next(21);

				int size = 0;
				string starType = "Yellow Star";

				switch (whichStar)
				{
					case 0: 
						size = 1;
						starType = "Yellow Star";
						break;
					case 1:
						size = 1;
						starType = "Blue Star";
						break;
					case 2:
						size = 1;
						starType = "Orange Star";
						break;
					case 3:
						size = 1;
						starType = "White Star";
						break;
					case 4:
						size = 1;
						starType = "Green Star";
						break;
					case 5:
						size = 1;
						starType = "Red Giant";
						break;
					case 6:
						size = 1;
						starType = "Black Dwarf";
						break;
					case 7:
						size = 4;
						starType = "Nebula1";
						break;
					case 8:
						size = 3;
						starType = "Nebula2";
						break;
					case 9:
						size = 3;
						starType = "Nebula3";
						break;
					case 10:
						size = 2;
						starType = "Nebula4";
						break;
					case 11:
						size = 4;
						starType = "Nebula5";
						break;
					case 12:
						size = 3;
						starType = "Nebula6";
						break;
					case 13:
						size = 1;
						starType = "Black Hole";
						break;
					case 14:
						size = 1;
						starType = "Yellow Star";
						break;
					case 15:
						size = 1;
						starType = "Blue Star";
						break;
					case 16:
						size = 1;
						starType = "Orange Star";
						break;
					case 17:
						size = 1;
						starType = "White Star";
						break;
					case 18:
						size = 1;
						starType = "Green Star";
						break;
					case 19:
						size = 1;
						starType = "Red Giant";
						break;
					case 20:
						size = 1;
						starType = "Black Dwarf";
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