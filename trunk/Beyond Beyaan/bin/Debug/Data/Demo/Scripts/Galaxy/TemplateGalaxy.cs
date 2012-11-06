using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	//Class must be named GalaxyGenerator
	public class GalaxyGenerator
	{
		//add whatever static variables you want
		static int numStars;
		static int spread;
		
		public static void Main()
		{
			//Leave this main function as is
			GalaxyGenerator gal = new GalaxyGenerator();
		}

		public static Dictionary<string, string> GetVariables()
		{
			//This is where the game gets its list of ui and values needed
			Dictionary<string, string> vars = new Dictionary<string, string>();

			//The name of the variable will be displayed as is in game, so don't use "numStars", use "Number of Stars"

			//upDown is the plus/minus increment.  The values are as follows: minimum value|maximum value|increment amount|initial amount
			vars["Number of Stars"] = "upDown|10|50|10|20";

			//more UI elements will be added later!

			return vars;
		}


		public static List<List<int>> Generate(Dictionary<string, string> vars)
		{
			//The game will pass in values for each of the variable that was listed in "GetVariables" function
			//The List<List<int>> is a list of list of ints, with the inner list consisting of three ints (X, Y, and Size) Size can be between 2 and 4
			
			//You can parse the values into desired types (such as doubles, ints, etc)  If it fails, return null so the game'd know something went wrong
			if (!int.TryParse(vars["Number of Stars"], out numStars)) return null;

			Random rand = new Random();
			List<List<int>> stars = new List<List<int>>();

			for (int i = 0; i < numStars; i++)
			{
				int x = i * 5;
				int y = i * 5;
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