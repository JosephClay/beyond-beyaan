using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	//Class must be named GalaxyGenerator
	public class GalaxyGenerator
	{
		//add whatever static variables you want
		static int amountStars;
		static int rotations;
		static int size;
		static int thickness;
		static int arms;
		
		public static void Main()
		{
			//Leave this main function as is
			GalaxyGenerator gal = new GalaxyGenerator();
		}

		public static Dictionary<string, string> GetVariables()
		{
			//This is where the game gets its list of ui and values needed
			Dictionary<string, string> vars = new Dictionary<string, string>();

			//upDown is the plus/minus increment.  The values are as follows: minimum value|maximum value|increment amount|initial amount
			vars["Amount of Stars"] = "upDown|25|-1|25|100";
			vars["Rotations"] = "upDown|1|-1|1|2";
			vars["Size"] = "upDown|10|-1|10|100";
			vars["Arm Thickness"] = "upDown|10|-1|10|100";
			vars["Arms"] = "upDown|1|-1|1|2";

			return vars;
		}


		public static List<List<int>> Generate(Dictionary<string, string> vars)
		{
			//The game will pass in values for each of the variable that was listed in "GetVariables" function
			//The List<List<int>> is a list of list of ints, with the inner list consisting of three ints (X, Y, and Size) Size can be between 2 and 4
			
			//You can parse the values into desired types (such as doubles, ints, etc)  If it fails, return null so the game'd know something went wrong
			if (!int.TryParse(vars["Amount of Stars"], out amountStars)) return null;
			if (!int.TryParse(vars["Rotations"], out rotations)) return null;
			if (!int.TryParse(vars["Size"], out size)) return null;
			if (!int.TryParse(vars["Arm Thickness"], out thickness)) return null;
			if (!int.TryParse(vars["Arms"], out arms)) return null;

			Random random = new Random();
			List<List<int>> stars = new List<List<int>>();

			int origin = size / 2;
			size = size / rotations;

			for (int i = 0; i < amountStars; i++)
			{
				List<int> l;
				int k = 0;
				while (k < 3)
				{
					int s = 1;

					bool isNegative = random.Next(2) == 1;
					int arm = random.Next(arms);
					double th = random.NextDouble() * Math.PI * rotations;
					double r = th * size;

					double x = Math.Cos(th - ((arm * 2.0) * Math.PI / arms));
					double y = Math.Sin(th - ((arm * 2.0) * Math.PI / arms));

					double xPos = (x * r) + origin;
					double yPos = (y * r) + origin;

					double th2 = random.NextDouble() * Math.PI * 2;
					double r2 = (1 - (th / (Math.PI * rotations))) * thickness;

					xPos = xPos + (Math.Cos(th2) * r2);
					yPos = yPos + (Math.Sin(th2) * r2);

					l = new List<int>();
					l.Add((int)xPos);
					l.Add((int)yPos);
					l.Add(s);
					if (LegitStarSpot(stars, l))
					{
						stars.Add(l);
						break;
					}
					k++;
				}
			}

			int minX = 0;
			int minY = 0;

			for (int i = 0; i < stars.Count; i++)
			{
				if (stars[i][0] < minX)
				{
					minX = stars[i][0];
				}
				if (stars[i][1] < minY)
				{
					minY = stars[i][1];
				}
			}

			if (minX < 0 || minY < 0)
			{
				for (int i = 0; i < stars.Count; i++)
				{
					stars[i][0] -= minX;
					stars[i][1] -= minY;
				}
			}

			return stars;
		}

		private static bool LegitStarSpot(List<List<int>> stars, List<int> desiredSpot)
		{
			for (int i = 0; i < stars.Count; i++)
			{
				if (((desiredSpot[0] >= stars[i][0] - 1 && desiredSpot[0] < stars[i][0] + stars[i][2] + 1) || (desiredSpot[0] <= stars[i][0] - 1 && desiredSpot[0] + desiredSpot[2] + 1 > stars[i][0])) &&
						((desiredSpot[1] >= stars[i][1] - 1 && desiredSpot[1] < stars[i][1] + stars[i][2] + 1) || (desiredSpot[1] <= stars[i][1] - 1 && desiredSpot[1] + desiredSpot[2] + 1 > stars[i][2])))
				{
					return false;
				}
			}
			return true;
		}

	}
}