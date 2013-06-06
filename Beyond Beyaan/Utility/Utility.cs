using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	static class Utility
	{
		/// <summary>
		/// Calculates a full circle
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="originSize">Size of the object</param>
		/// <returns></returns>
		public static bool[][] CalculateDisc(int radius, int originSize)
		{
			if (originSize < 1)
			{
				return null;
			}
			bool[][] grid = CalculateCircle(radius, originSize);

			for (int i = 0; i < grid.Length; i++)
			{
				int top = 0;
				int bottom = grid[i].Length - 1;
				bool foundTop = false;
				bool foundBottom = false;

				while (top <= bottom)
				{
					if (grid[i][top])
					{
						foundTop = true;
					}
					if (grid[i][bottom])
					{
						foundBottom = true;
					}
					if (foundTop)
					{
						grid[i][top] = true;
					}
					if (foundBottom)
					{
						grid[i][bottom] = true;
					}
					top++;
					bottom--;
				}
			}

			return grid;
		}

		public static bool[][] CalculateCircle(int radius, int originSize)
		{
			if (originSize < 1)
			{
				return null;
			}
			int size = (radius * 2) + originSize;

			bool[][] grid = new bool[size][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[size];
			}

			int f = 1 - radius;
			int ddF_x = 1;
			int ddF_y = -2 * radius;
			int x = 0;
			int y = radius;

			int origin = radius;
			int modifiedOriginSize = originSize - 1;

			for (int i = 0; i < originSize; i++)
			{
				grid[origin + i][origin + radius + modifiedOriginSize] = true;
				grid[origin + i][0] = true;
				grid[origin + radius + modifiedOriginSize][origin + i] = true;
				grid[0][origin + i] = true;
			}

			while (x < y)
			{
				if (f >= 0)
				{
					y--;
					ddF_y += 2;
					f += ddF_y;
				}
				x++;
				ddF_x += 2;
				f += ddF_x;


				grid[origin + modifiedOriginSize + x][origin + modifiedOriginSize + y] = true;
				grid[origin - x][origin + modifiedOriginSize + y] = true;
				grid[origin + modifiedOriginSize + x][origin - y] = true;
				grid[origin - x][origin - y] = true;

				grid[origin + modifiedOriginSize + y][origin + modifiedOriginSize + x] = true;
				grid[origin - y][origin + modifiedOriginSize + x] = true;
				grid[origin + modifiedOriginSize + y][origin - x] = true;
				grid[origin - y][origin - x] = true;
			}

			return grid;
		}

		/// <summary>
		/// Converts from a numeric value to roman numbers
		/// </summary>
		/// <param name="value">Value to convert</param>
		/// <returns>Resulting Roman number</returns>
		public static string ConvertNumberToRomanNumberical(int value)
		{
			//This algorithm is courtesy of
			//http://www.blackwasp.co.uk/NumberToRoman.aspx

			if (value < 0 || value > 3999)
			{
				throw new ArgumentException("Value must be in the range 0 - 3,999.");
			}
			if (value == 0)
			{
				return "N";
			}
			int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

			StringBuilder result = new StringBuilder();

			// Loop through each of the values to diminish the number
			for (int i = 0; i < 13; i++)
			{
				// If the number being converted is less than the test value, append
				// the corresponding numeral or numeral pair to the resultant string
				while (value >= values[i])
				{
					value -= values[i];
					result.Append(numerals[i]);
				}
			}

			return result.ToString();
		}

		public static string PlanetTypeToString(PLANET_TYPE planetType)
		{
			switch (planetType)
			{
				case PLANET_TYPE.ARCTIC: return "Arctic";
				case PLANET_TYPE.ASTEROIDS: return "Asteroids";
				case PLANET_TYPE.BADLAND: return "Badlands";
				case PLANET_TYPE.BARREN: return "Barren";
				case PLANET_TYPE.DEAD: return "Dead";
				case PLANET_TYPE.DESERT: return "Desert";
				case PLANET_TYPE.GAS_GIANT: return "Gas Giant";
				case PLANET_TYPE.JUNGLE: return "Jungle";
				case PLANET_TYPE.OCEAN: return "Oceanic";
				case PLANET_TYPE.RADIATED: return "Radiated";
				case PLANET_TYPE.STEPPE: return "Steppe";
				case PLANET_TYPE.TERRAN: return "Terran";
				case PLANET_TYPE.TOXIC: return "Toxic";
				case PLANET_TYPE.TUNDRA: return "Tundra";
				case PLANET_TYPE.VOLCANIC: return "Volcanic";
			}
			return String.Empty;
		}

		public static SpriteName PlanetTypeToSprite(PLANET_TYPE planetType)
		{
			switch (planetType)
			{
				case PLANET_TYPE.ARCTIC: return SpriteName.Arctic;
				case PLANET_TYPE.ASTEROIDS: return SpriteName.Asteroids;
				case PLANET_TYPE.BADLAND: return SpriteName.Badlands;
				case PLANET_TYPE.BARREN: return SpriteName.Barren;
				case PLANET_TYPE.DEAD: return SpriteName.Dead;
				case PLANET_TYPE.DESERT: return SpriteName.Desert;
				case PLANET_TYPE.GAS_GIANT: return SpriteName.GasGiant;
				case PLANET_TYPE.JUNGLE: return SpriteName.Jungle;
				case PLANET_TYPE.OCEAN: return SpriteName.Ocean;
				case PLANET_TYPE.RADIATED: return SpriteName.Radiated;
				case PLANET_TYPE.STEPPE: return SpriteName.Steppe;
				case PLANET_TYPE.TERRAN: return SpriteName.Terran;
				case PLANET_TYPE.TOXIC: return SpriteName.Toxic;
				case PLANET_TYPE.TUNDRA: return SpriteName.Tundra;
				case PLANET_TYPE.VOLCANIC: return SpriteName.Volcanic;
			}
			return SpriteName.Terran;
		}
		public static SpriteName PlanetConstructionBonusToSprite(PLANET_CONSTRUCTION_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_CONSTRUCTION_BONUS.DEARTH: return SpriteName.PlanetConstructionBonus1;
				case PLANET_CONSTRUCTION_BONUS.POOR: return SpriteName.PlanetConstructionBonus2;
				case PLANET_CONSTRUCTION_BONUS.COPIOUS: return SpriteName.PlanetConstructionBonus3;
				case PLANET_CONSTRUCTION_BONUS.RICH: return SpriteName.PlanetConstructionBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}
		public static SpriteName PlanetEnvironmentBonusToSprite(PLANET_ENVIRONMENT_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_ENVIRONMENT_BONUS.DESOLATE: return SpriteName.PlanetEnvironmentBonus1;
				case PLANET_ENVIRONMENT_BONUS.INFERTILE: return SpriteName.PlanetEnvironmentBonus2;
				case PLANET_ENVIRONMENT_BONUS.FERTILE: return SpriteName.PlanetEnvironmentBonus3;
				case PLANET_ENVIRONMENT_BONUS.LUSH: return SpriteName.PlanetEnvironmentBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}
		public static SpriteName PlanetEntertainmentBonusToSprite(PLANET_ENTERTAINMENT_BONUS bonus)
		{
			switch (bonus)
			{
				case PLANET_ENTERTAINMENT_BONUS.INSIPID: return SpriteName.PlanetEntertainmentBonus1;
				case PLANET_ENTERTAINMENT_BONUS.DULL: return SpriteName.PlanetEntertainmentBonus2;
				case PLANET_ENTERTAINMENT_BONUS.SENSATIONAL: return SpriteName.PlanetEntertainmentBonus3;
				case PLANET_ENTERTAINMENT_BONUS.EXCITING: return SpriteName.PlanetEntertainmentBonus4;
			}
			//If it reaches here, something went wrong
			return SpriteName.CancelBackground;
		}

		public static string ShipSizeToString(int size)
		{
			switch (size)
			{
				case 1: return "Lancer";
				case 2: return "Corvette";
				case 3: return "Frigate";
				case 4: return "Destroyer";
				case 5: return "Cruiser";
				case 6: return "Battlecruiser";
				case 7: return "Battleship";
				case 8: return "Behemoth";
				case 9: return "Titan";
				case 10: return "Leviathian";
			}
			return string.Empty;
		}

		public static string RelationToLabel(int relation)
		{
			if (relation < 25)
			{
				return "Hate";
			}
			if (relation < 50)
			{
				return "Loathe";
			}
			if (relation < 75)
			{
				return "Despise";
			}
			if (relation < 95)
			{
				return "Dislike";
			}
			if (relation < 105)
			{
				return "Neutral";
			}
			if (relation < 125)
			{
				return "Like";
			}
			if (relation < 150)
			{
				return "Respect";
			}
			if (relation < 175)
			{
				return "Esteem";
			}
			return "Venerate";
		}
	}

	public struct Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
