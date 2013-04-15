using System;
using System.Collections.Generic;
using System.Text;
using Beyond_Beyaan.Data_Modules;
using System.Globalization;

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

		public static bool LineRectangleIntersected(int lineX1, int lineY1, int lineX2, int lineY2, int rectX1, int rectY1, int rectX2, int rectY2)
		{
			//quick checks
			if ((lineX1 >= rectX1 && lineX1 <= rectX2) &&
				(lineX2 >= rectX1 && lineX2 <= rectX2) &&
				(lineY1 >= rectY1 && lineY1 <= rectY2) &&
				(lineY2 >= rectY1 && lineY2 <= rectY2))
			{
				return true;
			}

			if (LineLineIntersected(lineX1, lineY1, lineX2, lineY2, rectX1, rectY1, rectX1, rectY2)) return true;
			if (LineLineIntersected(lineX1, lineY1, lineX2, lineY2, rectX1, rectY1, rectX2, rectY1)) return true;
			if (LineLineIntersected(lineX1, lineY1, lineX2, lineY2, rectX2, rectY1, rectX2, rectY2)) return true;
			if (LineLineIntersected(lineX1, lineY1, lineX2, lineY2, rectX1, rectY2, rectX2, rectY2)) return true;

			return false;
		}
		public static bool LineLineIntersected(int lineAX1, int lineAY1, int lineAX2, int lineAY2, int lineBX1, int lineBY1, int lineBX2, int lineBY2)
		{
			float denom = ((lineBY2 - lineBY1) * (lineAX2 - lineAX1)) - ((lineBX2 - lineBX1) * (lineAY2 - lineAY1));
			int num1 = ((lineBX2 - lineBX1) * (lineAY1 - lineBY1)) - ((lineBY2 - lineBY1) * (lineAX1 - lineBX1));
			int num2 = ((lineAX2 - lineAX1) * (lineAY1 - lineBY1)) - ((lineAY2 - lineAY1) * (lineAX1 - lineBX1));

			if (denom == 0)
			{
				return false;
			}
			if (num1 == 0 && num2 == 0)
			{
				return false;
			}

			float ua = num1 / denom;
			float ub = num2 / denom;

			return (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f);
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
			int[] values = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

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

		public static string ConvertNumberToFourDigits(double amount)
		{
			int divisions = 0;
			while (amount >= 1000)
			{
				amount /= 1000.0f;
				divisions++;
			}
			string letter = string.Empty;
			switch (divisions)
			{
				case 1: letter = " k";
					break;
				case 2: letter = " M";
					break;
				case 3: letter = " B";
					break;
			}
			return string.Format("{0:0.0}" + letter, amount);
		}

		public static int GetIntValue(string value, Random r)
		{
			//There are three possible types of "value"
			//The first is a basic value, like "5", it is always returned
			//The second is a range of values, like "0,5", randomly pick one inside the range
			//The third is a weighted range of values, like "0,1,0.05" which leans toward 0 with 5% chance of picking 1
			string[] parts = value.Split(new[] { ',' });
			if (parts.Length == 1)
			{
				//Just return the value
				return int.Parse(parts[0]); 
			}
			if (parts.Length == 2)
			{
				//return a value in the range
				return r.Next(int.Parse(parts[0]), int.Parse(parts[1]) + 1);
			}
			if (parts.Length == 3)
			{
				//return a value in the weighted range
				double randVal;
				do
				{
					randVal = r.NextDouble();
				} while (randVal == 0); //Make sure it's not 0, otherwise it'd throw an exception in the next line of code

				double weight = NormalCDFInverse(randVal);
				int min = int.Parse(parts[0]);
				int max = int.Parse(parts[1]);
				float shift = float.Parse(parts[2], CultureInfo.InvariantCulture); //Shift moves the standard distribution left or right (does not skew it, just moves it, 0.5 is default)
				int newValue = (int)(((min + max) * shift) + (weight * (max - min)));
				if (newValue < min)
				{
					return min;
				}
				if (newValue > max)
				{
					return max;
				}
				return newValue;
			}
			throw new Exception("GetIntValue cannot parse the value of '" + value + "'");
		}

		//This function was obtained from http://www.johndcook.com/csharp_phi_inverse.html

		//Brent here: It seems like LogOnePlusX isn't actually used, perhaps it's a faster but less accurate version of Math.Log?  To-do: Investigate.

		// compute log(1+x) without losing precision for small values of x
		private static double LogOnePlusX(double x)
		{
			if (x <= -1.0)
			{
				string msg = String.Format("Invalid input argument: {0}", x);
				throw new ArgumentOutOfRangeException(msg);
			}

			if (Math.Abs(x) > 1e-4)
			{
				// x is large enough that the obvious evaluation is OK
				return Math.Log(1.0 + x);
			}

			// Use Taylor approx. 
			// log(1 + x) = x - x^2/2 with error roughly x^3/3
			// Since |x| < 10^-4, |x|^3 < 10^-12, 
			// relative error less than 10^-8

			return (-0.5*x + 1.0)*x;
		}

		private static double RationalApproximation(double t)
		{
			// Abramowitz and Stegun formula 26.2.23.
			// The absolute value of the error should be less than 4.5 e-4.
			double[] c = {2.515517, 0.802853, 0.010328};
			double[] d = {1.432788, 0.189269, 0.001308};
			return t - ((c[2]*t + c[1])*t + c[0]) / 
						(((d[2]*t + d[1])*t + d[0])*t + 1.0);
		}

		private static double NormalCDFInverse(double p)
		{
			if (p <= 0.0 || p >= 1.0)
			{
				string msg = String.Format("Invalid input argument: {0}.", p);
				throw new ArgumentOutOfRangeException(msg);
			}

			// See article above for explanation of this section.
			if (p < 0.5)
			{
				// F^-1(p) = - G^-1(p)
				return -RationalApproximation( Math.Sqrt(-2.0*Math.Log(p)) );
			}
			// F^-1(p) = G^-1(1-p)
			return RationalApproximation( Math.Sqrt(-2.0*Math.Log(1.0 - p)) );
		}


		/*public static SpriteName PlanetConstructionBonusToSprite(PLANET_CONSTRUCTION_BONUS bonus)
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

		public static EquipmentType TechTypeToEquipmentType(TechType type)
		{
			switch (type)
			{
				case TechType.ARMOR:
				case TechType.ARMOR_MODIFICATION:
				case TechType.ARMOR_PLATING: return EquipmentType.ARMOR;

				case TechType.BEAM:
				case TechType.BEAM_MODIFICATION:
				case TechType.BEAM_MOUNT: return EquipmentType.BEAM;

				case TechType.BOMB:
				case TechType.BOMB_BODY:
				case TechType.BOMB_MODIFICATION: return EquipmentType.BOMB;

				case TechType.COMPUTER:
				case TechType.COMPUTER_MODIFICATION:
				case TechType.COMPUTER_MOUNT: return EquipmentType.COMPUTER;

				case TechType.MISSILE_BODY:
				case TechType.MISSILE_MODIFICATION:
				case TechType.MISSILE_WARHEAD: return EquipmentType.MISSILE;

				case TechType.PROJECTILE:
				case TechType.PROJECTILE_MODIFICATION:
				case TechType.PROJECTILE_MOUNT: return EquipmentType.PROJECTILE;

				case TechType.REACTOR:
				case TechType.REACTOR_MODIFICATION:
				case TechType.REACTOR_MOUNT: return EquipmentType.REACTOR;

				case TechType.SHIELD:
				case TechType.SHIELD_MODIFICATION:
				case TechType.SHIELD_MOUNT: return EquipmentType.SHIELD;

				case TechType.SHOCKWAVE:
				case TechType.SHOCKWAVE_EMITTER:
				case TechType.SHOCKWAVE_MODIFICATION: return EquipmentType.SHOCKWAVE;

				case TechType.SPECIAL: return EquipmentType.SPECIAL;

				case TechType.STELLAR_ENGINE:
				case TechType.STELLAR_ENGINE_MOUNT:
				case TechType.STELLAR_MODIFICATION: return EquipmentType.STELLAR_ENGINE;

				case TechType.SYSTEM_ENGINE:
				case TechType.SYSTEM_ENGINE_MOUNT:
				case TechType.SYSTEM_MODIFICATION: return EquipmentType.SYSTEM_ENGINE;

				case TechType.TORPEDO:
				case TechType.TORPEDO_LAUNCHER:
				case TechType.TORPEDO_MODIFICATION: return EquipmentType.TORPEDO;
			}
			throw new Exception("Invalid TechType enum called");
		}*/

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

		public static double CalculatePathCost(List<KeyValuePair<StarSystem, Starlane>> nodes)
		{
			double amount = 0;
			foreach (KeyValuePair<StarSystem, Starlane> path in nodes)
			{
				if (path.Value != null)
				{
					amount += path.Value.Length;
				}
			}
			return amount;
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
	public struct PointF
	{
		public float X;
		public float Y;

		public PointF(float x, float y)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(PointF p1, PointF p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(PointF p1, PointF p2)
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
