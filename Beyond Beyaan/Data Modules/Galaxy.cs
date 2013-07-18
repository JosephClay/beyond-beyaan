using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum GALAXYTYPE { RANDOM, CLUSTER, STAR, DIAMOND, RING };

	public class Galaxy
	{
		public const int PARSEC_SIZE_IN_PIXELS = 60;

		private List<StarSystem> starSystems = new List<StarSystem>();

		/*#region Pathfinding values
		private int Open_Value = 0;
		private int Closed_Value = 1;
		private int Current_Value = 0;

		int originalX = -1;
		int originalY = -1;
		#endregion*/

		public int GalaxySize { get; private set; }

		/// <summary>
		/// Set up the galaxy
		/// </summary>
		/// <param name="galaxyType"></param>
		/// <param name="minPlanets"></param>
		/// <param name="maxPlanets"></param>
		/// <param name="size"></param>
		/// <param name="minDistance"></param>
		/// <param name="spriteManager"></param>
		/// <param name="r"></param>
		/// <param name="reason"></param>
		public bool GenerateGalaxy(GALAXYTYPE galaxyType, int minPlanets, int maxPlanets, int size, int minDistance, Random r, out string reason)
		{
			bool[][] grid = null;
			switch (galaxyType)
			{
				case GALAXYTYPE.RANDOM:
					{
						grid = GenerateRandom(size);
					} break;
				case GALAXYTYPE.CLUSTER:
					{
						grid = GenerateCluster(size);
					} break;
				case GALAXYTYPE.STAR:
					{
						grid = GenerateStar(size);
					} break;
				case GALAXYTYPE.DIAMOND:
					{
						grid = GenerateDiamond(size);
					} break;
				case GALAXYTYPE.RING:
					{
						grid = GenerateRing(size);
					} break;
			}

			GalaxySize = grid.Length;

			if (!FillGalaxyWithStars(minDistance, minPlanets, maxPlanets, grid, r, out reason))
			{
				return false;
			}

			//SetBlackHoles(10, r);

			//GenerateNebulaField(r);

			//ConvertNebulaToSprite();

			reason = null;
			return true;
		}

		/*public void ConstructQuadTree()
		{
			ParentNode = new QuadNode(0, 0, GalaxySize, starSystems);
		}*/

		#region Star Retrieval Functions
		/// <summary>
		/// Get all stars in the area for drawing purposes
		/// </summary>
		/// <param name="top"></param>
		/// <param name="left"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public List<StarSystem> GetStarsInArea(float left, float top, float width, float height)
		{
			List<StarSystem> starsInArea = new List<StarSystem>();
			//ParentNode.GetStarsInArea(left, top, width + 4, height + 4, starsInArea);
			foreach (StarSystem star in starSystems)
			{
				if (star.X + (star.Size * 16) < left || star.Y + (star.Size * 16) < top || star.X - (star.Size * 16) > left + width || star.Y - (star.Size * 16) > top + height)
				{
					continue;
				}
				starsInArea.Add(star);
			}
			return starsInArea;
		}

		public List<StarSystem> GetAllStars()
		{
			return starSystems;
		}

		public StarSystem GetStarAtPoint(Point point)
		{
			foreach (StarSystem starSystem in starSystems)
			{
				if (starSystem.X - starSystem.Size * 16 <= point.X && starSystem.X + starSystem.Size * 16 > point.X && starSystem.Y - starSystem.Size * 16 <= point.Y && starSystem.Y + starSystem.Size * 16 > point.Y)
				{
					return starSystem;
				}
			}
			return null;
		}
		#endregion

		#region Galaxy Shape Functions
		private bool[][] GenerateCluster(int size)
		{
			//Size is actually a diameter, change to radius
			return Utility.CalculateDisc(size / 2, 1);
		}

		private bool[][] GenerateRing(int size)
		{
			//Size is actually a diameter, change to radius
			bool[][] grid = Utility.CalculateDisc(size / 2, 1);

			int quarterSize = size / 4;

			bool[][] discToSubtract = Utility.CalculateDisc(quarterSize, 1);

			for (int i = 0; i < discToSubtract.Length; i++)
			{
				for (int j = 0; j < discToSubtract[i].Length; j++)
				{
					if (discToSubtract[i][j])
					{
						grid[quarterSize + i][quarterSize + j] = false;
					}
				}
			}

			return grid;
		}

		private bool[][] GenerateRandom(int size)
		{
			bool[][] grid = new bool[size][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[size];
			}

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					grid[i][j] = true;
				}
			}

			return grid;
		}

		private bool[][] GenerateStar(int size)
		{
			bool[][] grid = new bool[size][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[size];
			}
			int halfSize = size / 2;

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					int x = i - halfSize;
					int y = halfSize - j;
					if (x < 0)
					{
						x *= -1;
					}
					if (y < 0)
					{
						y *= -1;
					}
					if ((x * x) * (y * y) <= (size * size * (halfSize / 6)))
					{
						grid[i][j] = true;
					}
				}
			}

			return grid;
		}

		private bool[][] GenerateDiamond(int size)
		{
			bool[][] grid = new bool[size][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[size];
			}
			int halfSize = size / 2;

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					int x = i - halfSize;
					int y = halfSize - j;
					if (x < 0)
					{
						x *= -1;
					}
					if (y < 0)
					{
						y *= -1;
					}
					if (x + y <= halfSize)
					{
						grid[i][j] = true;
					}
				}
			}

			return grid;
		}
		#endregion

		#region Galaxy Filling Functions
		private bool FillGalaxyWithStars(int minDistance, int minPlanets, int maxPlanets, bool[][] grid, Random r, out string reason)
		{
			starSystems = new List<StarSystem>();
			NameGenerator nameGenerator = new NameGenerator();

			StarNode starTree = new StarNode(0, 0, grid.Length - 1, grid.Length - 1);

			//Set area where stars can be placed (circle, random, star, etc shaped galaxy)
			for (int i = 0; i < grid.Length; i++)
			{
				for (int j = 0; j < grid.Length; j++)
				{
					if (!grid[i][j])
					{
						starTree.RemoveNodeAtPosition(i, j);
					}
				}
			}

			while (starTree.nodes.Count > 0)
			{
				int x;
				int y;

				starTree.GetRandomStarPosition(r, out x, out y);

				//int newSize = r.Next(3) + 2;

				Color starColor = Color.White;

				switch (r.Next(8)) //type of star
				{
					case 0: starColor = Color.Red;
						break;
					case 1: starColor = Color.Orange;
						break;
					case 2: starColor = Color.Green;
						break;
					case 3: starColor = Color.Purple;
						break;
					case 4: starColor = Color.Blue;
						break;
					case 5: starColor = Color.Brown;
						break;
					case 6: starColor = Color.White;
						break;
					case 7: starColor = Color.Yellow;
						break;
				}

				starSystems.Add(new StarSystem(nameGenerator.GetStarName(r), x * 32 + (r.Next(32)), y * 32 + (r.Next(32)), starColor, minPlanets, maxPlanets, r));

				int adjustedMinDistance = minDistance + r.Next(4);

				bool[][] invalidatedArea = Utility.CalculateDisc(adjustedMinDistance, 1);

				for (int i = 0; i < invalidatedArea.Length; i++)
				{
					for (int j = 0; j < invalidatedArea.Length; j++)
					{
						int xToInvalidate = (x - adjustedMinDistance) + i;
						int yToInvalidate = (y - adjustedMinDistance) + j;

						starTree.RemoveNodeAtPosition(xToInvalidate, yToInvalidate);
					}
				}
			}
			reason = null;
			return true;
		}
		#endregion

		#region Galaxy Setup
		public StarSystem SetHomeworld(Empire empire, out Planet homePlanet)
		{
			Random r = new Random();
			while (true)
			{
				int starIter = r.Next(starSystems.Count);
				if (starSystems[starIter].EmpiresWithPlanetsInThisSystem.Count == 0)
				{
					starSystems[starIter].SetHomeworld(empire, out homePlanet, r);
					return starSystems[starIter];
				}
			}
		}
		#endregion

		#region Pathfinding functions

		/// <summary>
		/// Pathfinding function
		/// </summary>
		/// <param name="currentX">Fleet's Galaxy X</param>
		/// <param name="currentY">Fleet's Galaxy Y</param>
		/// <param name="currentDestination">Fleet's current destination for when empire don't have hyperspace communications</param>
		/// <param name="newDestination">New destination</param>
		/// <param name="whichEmpire">For fuel range and other info</param>
		/// <returns></returns>
		public List<TravelNode> GetPath(float currentX, float currentY, StarSystem currentDestination, StarSystem newDestination, bool hasExtended, Empire whichEmpire)
		{
			// TODO: When Hyperspace communication is implemented, add this
			/*
			if (whichEmpire.HasHyperspaceCommunications())
			{
				
			}
			else
			{
			}
			*/
			// TODO: When adding stargates and wormholes, add actual pathfinding
			List<TravelNode> nodes = new List<TravelNode>();
			if (currentDestination != null)
			{
				TravelNode newNode = new TravelNode();
				newNode.StarSystem = currentDestination;
				float x = currentDestination.X - currentX;
				float y = currentDestination.Y - currentY;
				newNode.Length = (float)Math.Sqrt((x * x) + (y * y));
				newNode.Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
				newNode.IsValid = true;
				nodes.Add(newNode);
				newNode = new TravelNode();
				newNode.StarSystem = newDestination;
				x = newDestination.X - currentDestination.X;
				y = newDestination.Y - currentDestination.Y;
				newNode.Length = (float)Math.Sqrt((x * x) + (y * y));
				newNode.Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
				newNode.IsValid = IsDestinationValid(newDestination, hasExtended, whichEmpire);
				nodes.Add(newNode);
			}
			else
			{
				TravelNode newNode = new TravelNode();
				newNode.StarSystem = newDestination;
				float x = newDestination.X - currentX;
				float y = newDestination.Y - currentY;
				newNode.Length = (float)Math.Sqrt((x * x) + (y * y));
				newNode.Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
				newNode.IsValid = IsDestinationValid(newDestination, hasExtended, whichEmpire);
				nodes.Add(newNode);
			}
			
			return nodes;
		}
		private bool IsDestinationValid(StarSystem destination, bool hasExtended, Empire whichEmpire)
		{
			if (destination.Planets[0].Owner == whichEmpire)
			{
				//By default, always true if destination is owned
				return true;
			}
			int fuelRange = (whichEmpire.TechnologyManager.FuelRange + (hasExtended ? 3 : 0)) * PARSEC_SIZE_IN_PIXELS;
			fuelRange *= fuelRange; //To avoid square rooting
			foreach (StarSystem system in starSystems)
			{
				if (system.Planets[0].Owner == whichEmpire)
				{
					float x = destination.X - system.X;
					float y = destination.Y - system.Y;
					if ((x * x) + (y * y) <= fuelRange)
					{
						return true;
					}
				}
			}
			return false;
		}
		#endregion

		public void Update(float frameDeltaTime, Random r)
		{
			foreach (StarSystem system in starSystems)
			{
				system.Sprite.Update(frameDeltaTime, r);
			}
		}
	}

	#region QuadNode Classes
	internal class StarNode
	{
		internal List<StarNode> nodes;
		internal int X;
		internal int Y;
		internal int Width;
		internal int Height;

		public StarNode(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			if (width == 1 && height == 1)
			{
				//End of branch
				return;
			}
			else
			{
				nodes = new List<StarNode>();
				if (width > 1 && height == 1)
				{
					nodes.Add(new StarNode(x, y, width / 2, height));
					nodes.Add(new StarNode(x + (width / 2), y, width - (width / 2), height));
				}
				else if (height > 1 && width == 1)
				{
					nodes.Add(new StarNode(x, y, width, height / 2));
					nodes.Add(new StarNode(x, y + (height / 2), width, height - (height / 2)));
				}
				else
				{
					nodes.Add(new StarNode(x, y, width / 2, height / 2));
					nodes.Add(new StarNode(x + (width / 2), y, width - (width / 2), height / 2));
					nodes.Add(new StarNode(x, y + (height / 2), width / 2, height - (height / 2)));
					nodes.Add(new StarNode(x + (width / 2), y + (height / 2), width - (width / 2), height - (height / 2)));
				}
			}
		}

		public void RemoveNodeAtPosition(int x, int y)
		{
			if (nodes != null)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					if (nodes[i].X <= x && nodes[i].X + nodes[i].Width >= x &&
						nodes[i].Y <= y && nodes[i].Y + nodes[i].Height >= y)
					{
						if (nodes[i].Height == 1 && nodes[i].Width == 1)
						{
							nodes.Remove(nodes[i]);
						}
						else
						{
							nodes[i].RemoveNodeAtPosition(x, y);
							if (nodes[i].nodes.Count == 0)
							{
								nodes.Remove(nodes[i]);
							}
						}
						break;
					}
				}
			}
		}

		public void GetRandomStarPosition(Random r, out int x, out int y)
		{
			if (Width == 1 && Height == 1)
			{
				x = X;
				y = Y;
			}
			else
			{
				nodes[r.Next(nodes.Count)].GetRandomStarPosition(r, out x, out y);
			}
		}
	}
	#endregion
}
