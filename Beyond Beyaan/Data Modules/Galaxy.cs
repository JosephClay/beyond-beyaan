using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace Beyond_Beyaan
{
	enum GALAXYTYPE { RANDOM, CLUSTER, STAR, DIAMOND, RING };

	class Galaxy
	{
		private GridCell[][] gridCells;
		private List<StarSystem> starSystems = new List<StarSystem>();
		QuadNode ParentNode;
		GorgonLibrary.Graphics.Sprite nebula;

		#region Pathfinding values
		private int Open_Value = 0;
		private int Closed_Value = 1;
		private int Current_Value = 0;

		int originalX = -1;
		int originalY = -1;
		#endregion

		public int GalaxySize { get; private set; }

		public GorgonLibrary.Graphics.Sprite Nebula 
		{
			get { return nebula; }
		}

		/// <summary>
		/// Set up the galaxy
		/// </summary>
		/// <param name="galaxyType"></param>
		/// <param name="starCount"></param>
		public void GenerateGalaxy(GALAXYTYPE galaxyType, int minPlanets, int maxPlanets, int size, int minDistance, Random r)
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

			FillGalaxyWithStars(minDistance, minPlanets, maxPlanets, grid, r);

			//SetBlackHoles(10, r);

			GenerateNebulaField(r);

			ConvertNebulaToSprite();
		}

		public void ConstructQuadTree()
		{
			ParentNode = new QuadNode(0, 0, GalaxySize, starSystems);
		}

		#region Star Retrieval Functions
		/// <summary>
		/// Get all stars in the area for drawing purposes
		/// </summary>
		/// <param name="top"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		public List<StarSystem> GetStarsInArea(int left, int top, int width, int height)
		{
			List<StarSystem> starsInArea = new List<StarSystem>();
			ParentNode.GetStarsInArea(left, top, width + 4, height + 4, starsInArea);
			return starsInArea;
		}

		public GridCell[][] GetGridCells()
		{
			return gridCells;
		}

		public List<StarSystem> GetAllStars()
		{
			return starSystems;
		}

		public StarSystem GetStarAtPoint(Point point)
		{
			foreach (StarSystem starSystem in starSystems)
			{
				if (starSystem.X <= point.X && starSystem.X + starSystem.Size > point.X && starSystem.Y <= point.Y && starSystem.Y + starSystem.Size > point.Y)
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
		private void FillGalaxyWithStars(int minDistance, int minPlanets, int maxPlanets, bool[][] grid, Random r)
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

				int newSize = r.Next(3) + 2;

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

				starSystems.Add(new StarSystem(nameGenerator.GetStarName(r), x, y, starColor, newSize, minPlanets, maxPlanets, r));

				int adjustedMinDistance = minDistance + r.Next(9);

				bool[][] invalidatedArea = Utility.CalculateDisc(adjustedMinDistance, newSize);

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
		}
		#endregion

		#region Galaxy Enhancements
		/*public void SetBlackHoles(int percentage, Random r)
		{
			int amountToChange = (int)(starSystems.Count * (percentage / 100.0f));

			for (int i = 0; i < amountToChange; i++)
			{
				bool changed = false;
				while (!changed)
				{
					//impossible to have more black holes than stars so this loop won't be infinite
					int iter = r.Next(starSystems.Count());
					if (starSystems[iter].Type != StarType.BLACK_HOLE)
					{
						starSystems[iter].SetBlackHole();
						changed = true;
					}
				}
			}
		}*/

		public void GenerateNebulaField(Random r)
		{
			GridCell[][] blurredGridCells = new GridCell[GalaxySize + 3][];
			gridCells = new GridCell[GalaxySize + 3][];
			for (int i = 0; i < gridCells.Length; i++)
			{
				gridCells[i] = new GridCell[GalaxySize + 3];
				blurredGridCells[i] = new GridCell[GalaxySize + 3];
				for (int j = 0; j < gridCells[i].Length; j++)
				{
					/*blurredGridCells[i][j].nebulaDensity = -10;
					blurredGridCells[i][j].passable = true;*/
					gridCells[i][j].nebulaDensity = -10;
					//gridCells[i][j].passable = true;
				}
			}

			gridCells[0][0].nebulaDensity = r.Next(40);
			gridCells[gridCells.Length - 1][0].nebulaDensity = r.Next(40);
			gridCells[0][gridCells.Length - 1].nebulaDensity = r.Next(40);
			gridCells[gridCells.Length - 1][gridCells.Length - 1].nebulaDensity = r.Next(40);

			//Dictionary<Point, int> points = new Dictionary<Point, int>();

			foreach (StarSystem starSystem in starSystems)
			{
				int density = 5;
				switch (starSystem.Size)
				{
					case 2:
						{
							density = 30; //starSystem.Type == 30; StarType.BLACK_HOLE ? 70 : 30;
						} break;
					case 3:
						{
							density = 15; //starSystem.Type == 15StarType.BLACK_HOLE ? 85 : 15;
						} break;
					case 4:
						{
							density = 5; //starSystem.Type == StarType.BLACK_HOLE ? 100 : 5;
						} break;
				}
				for (int i = 0; i < starSystem.Size; i++)
				{
					for (int j = 0; j < starSystem.Size; j++)
					{
						gridCells[starSystem.X + i][starSystem.Y + j].nebulaDensity = density;
					}
				}
				/*points.Add(new Point(starSystem.X, starSystem.Y), density);
				GridCell cell = new GridCell();
				cell.nebulaDensity = density;
				for (int i = 0; i < starSystem.Size; i++)
				{
					for (int j = 0; j < starSystem.Size; j++)
					{
						gridCells[starSystem.X + i][starSystem.Y + j].nebulaDensity = density;
						gridCells[starSystem.X + i][starSystem.Y + j].passable = false;
					}
				}*/
			}

			CalculateNebulaField(0, 0, gridCells.Length, gridCells.Length, gridCells[0][0].nebulaDensity, gridCells[gridCells.Length - 1][0].nebulaDensity,
				gridCells[gridCells.Length - 1][gridCells.Length - 1].nebulaDensity, gridCells[0][gridCells.Length - 1].nebulaDensity, 50, r);

			/*for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < gridCells[i].Length; j++)
				{
					gridCells[i][j].nebulaDensity = CalculateNebulaDensity(i, j, points);
				}
			}
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < blurredGridCells[i].Length; j++)
				{
					blurredGridCells[i][j].nebulaDensity = BlurNebulaDensity(i, j);
				}
			}
			gridCells = blurredGridCells;*/
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < blurredGridCells[i].Length; j++)
				{
					blurredGridCells[i][j].nebulaDensity = BlurNebulaDensity(i, j);
				}
			}
			gridCells = blurredGridCells;
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < blurredGridCells[i].Length; j++)
				{
					blurredGridCells[i][j].passable = true;
					blurredGridCells[i][j].nebulaDensity = BlurNebulaDensity(i, j);
					blurredGridCells[i][j].movementCost = (blurredGridCells[i][j].nebulaDensity / 20) + 1;
					blurredGridCells[i][j].diagonalMovementCost = (int)((blurredGridCells[i][j].nebulaDensity * 1.414) / 20) + 1;
				}
			}
			gridCells = blurredGridCells;
			foreach (StarSystem starSystem in starSystems)
			{
				for (int i = 0; i < starSystem.Size; i++)
				{
					for (int j = 0; j < starSystem.Size; j++)
					{
						gridCells[starSystem.X + i][starSystem.Y + j].passable = false;
					}
				}
			}
		}

		private int CalculateNebulaDensity(int x, int y, Dictionary<Point, int> points)
		{
			int density = 0;
			int amount = 0;
			foreach (KeyValuePair<Point, int> keyValuePair in points)
			{
				int distance = ((x - keyValuePair.Key.X) * (x - keyValuePair.Key.X)) + ((y - keyValuePair.Key.Y) * (y - keyValuePair.Key.Y));
				if (distance <= 400)
				{
					amount++;
					density += (int)(keyValuePair.Value * (1.0 - (distance / 400) * (distance / 400)));
				}
			}
			if (density == 0)
			{
				density = 40;
				amount = 1;
			}
			density /= amount;
			if (density > 100)
			{
				density = 100;
			}
			return density;
		}

		private int BlurNebulaDensity(int x, int y)
		{
			int density = 0;
			int tempX;
			int tempY;
			for (int i = -2; i < 3; i++)
			{
				for (int j = -2; j < 3; j++)
				{
					tempX = i + x;
					tempY = j + y;
					if (tempX < 0 || tempX >= gridCells.Length)
					{
						tempX = -i + x;
					}
					
					if (tempY < 0 || tempY >= gridCells[tempX].Length)
					{
						tempY = -j + y;
					}
					density += gridCells[tempX][tempY].nebulaDensity;
				}
			}
			density /= 25;
			if (density > 100)
			{
				density = 100;
			}
			return density;
		}

		public void CalculateNebulaField(int x, int y, int width, int height, int corner1, int corner2, int corner3, int corner4, float roughness, Random r)
		{
			int edge1;
			int edge2;
			int edge3;
			int edge4;
			int middle;

			int newWidth = width / 2;
			int newHeight = height / 2;

			if (x >= 0 && x < gridCells.Length && y >= 0 && y < gridCells[x].Length)
			{
				if (gridCells[x][y].nebulaDensity != -10)
				{
					corner1 = gridCells[x][y].nebulaDensity;
				}
			}
			if (x + width >= 0 && x + width < gridCells.Length && y >= 0 && y < gridCells[x + width].Length)
			{
				if (gridCells[x + width][y].nebulaDensity != -10)
				{
					corner2 = gridCells[x + width][y].nebulaDensity;
				}
			}
			if (x + width >= 0 && x + width < gridCells.Length && y + height >= 0 && y + height < gridCells[x + width].Length)
			{
				if (gridCells[x + width][y + height].nebulaDensity != -10)
				{
					corner3 = gridCells[x + width][y + height].nebulaDensity;
				}
			}
			if (x >= 0 && x < gridCells.Length && y + height >= 0 && y + height < gridCells[x].Length)
			{
				if (gridCells[x][y + height].nebulaDensity != -10)
				{
					corner4 = gridCells[x][y + height].nebulaDensity;
				}
			}

			if (width > 1 || height > 1)
			{
				middle = ((corner1 + corner2 + corner3 + corner4) / 4) + Displace(newHeight + newWidth, roughness, r);
				edge1 = ((corner1 + corner2 + middle + 
					((y - newHeight >= 0)
						? (gridCells[x + newWidth][y - newHeight].nebulaDensity != -10 ? gridCells[x + newWidth][y - newHeight].nebulaDensity : Displace(GalaxySize, roughness, r)) 
						:  Displace(GalaxySize, roughness, r))) / 4);
				edge2 = ((corner2 + corner3 + middle +
					((x + width + newWidth < gridCells.Length)
						? (gridCells[x + width + newWidth][y + newHeight].nebulaDensity != -10 ? gridCells[x + width + newWidth][y + newHeight].nebulaDensity : 
						Displace(GalaxySize, roughness, r))	: Displace(GalaxySize, roughness, r))) / 4);
				edge3 = ((corner3 + corner4 + middle +
					((y + height + newHeight < gridCells.Length)
						? (gridCells[x + newWidth][y + height + newHeight].nebulaDensity != -10 ? gridCells[x + newWidth][y + height + newHeight].nebulaDensity : 
						Displace(GalaxySize, roughness, r))	: Displace(GalaxySize, roughness, r))) / 4);
				edge4 = ((corner4 + corner1 + middle + 
					((x - newWidth >= 0)
						? (gridCells[x - newWidth][y + newHeight].nebulaDensity != -10 ? gridCells[x - newWidth][y + newHeight].nebulaDensity : Displace(GalaxySize, roughness, r)) 
						:  Displace(GalaxySize, roughness, r))) / 4);
				/*edge1 = ((corner1 + corner2) / 2);
				edge2 = ((corner2 + corner3) / 2);
				edge3 = ((corner3 + corner4) / 2);
				edge4 = ((corner4 + corner1) / 2);*/

				middle = Rectify(middle);
				edge1 = Rectify(edge1);
				edge2 = Rectify(edge2);
				edge3 = Rectify(edge3);
				edge4 = Rectify(edge4);

				CalculateNebulaField(x, y, newWidth, newHeight, corner1, edge1, middle, edge4, roughness, r);
				CalculateNebulaField(x + newWidth, y, width - newWidth, newHeight, edge1, corner2, edge2, middle, roughness, r);
				CalculateNebulaField(x + newWidth, y + newHeight, width - newWidth, height - newHeight, middle, edge2, corner3, edge3, roughness, r);
				CalculateNebulaField(x, y + newHeight, newWidth, height - newHeight, corner1, edge4, middle, edge3, corner4, r);
			}
			else
			{
				if (x >= 0 && x < gridCells.Length && y >= 0 && y < gridCells[x].Length)
				{
					if (gridCells[x][y].nebulaDensity == -10)
					{
						int density = (corner1 + corner2 + corner3 + corner4) / 4;
						gridCells[x][y].nebulaDensity = density;
						gridCells[x][y].movementCost = (density / 20) + 1;
						gridCells[x][y].diagonalMovementCost = (int)((density * 1.414) / 20) + 1;
					}
				}
			}
		}

		private int Displace(float smallSize, float roughness, Random r)
		{
			float max = (smallSize / GalaxySize) * roughness;
			return (int)((r.NextDouble() - 0.5) * max);
		}
		private int Rectify(int num)
		{
			if (num < 5)
			{
				num = 5;
			}
			else if (num > 100)
			{
				num = 100;
			}
			return num;
		}

		private void ConvertNebulaToSprite()
		{
			int squaredSize = 2;

			while (squaredSize < gridCells.Length)
			{
				squaredSize *= 2;
			}

			GorgonLibrary.Graphics.Image image;
			if (GorgonLibrary.Graphics.ImageCache.Images.Contains("nebula"))
			{
				image = GorgonLibrary.Graphics.ImageCache.Images["nebula"];
				image.SetDimensions(squaredSize, squaredSize);
			}
			else
			{
				image = new GorgonLibrary.Graphics.Image("nebula", squaredSize, squaredSize, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			}
			image.Clear(Color.FromArgb(0, 0, 0, 0));
			GorgonLibrary.Graphics.Image.ImageLockBox newImage = image.GetImageData();
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < gridCells[i].Length; j++)
				{
					newImage[i, j] = System.Drawing.Color.FromArgb(255, 0, 0, (int)((gridCells[i][j].nebulaDensity / 100.0f) * 255)).ToArgb();
				}
			}
			nebula = new GorgonLibrary.Graphics.Sprite("nebula", image);
			//nebula.Smoothing = GorgonLibrary.Graphics.Smoothing.Smooth;
		}
		#endregion

		#region Galaxy Setup
		public StarSystem SetHomeworld(Empire empire, out Planet homePlanet)
		{
			Random r = new Random();
			bool placed = false;
			while (!placed)
			{
				int starIter = r.Next(starSystems.Count);
				if (starSystems[starIter].EmpiresWithPlanetsInThisSystem.Count == 0)
				{
					starSystems[starIter].SetHomeworld(empire, out homePlanet, r);
					return starSystems[starIter];
				}
			}
			homePlanet = null;
			return null;
		}
		#endregion

		#region Pathfinding functions

		public List<Point> GetPath(int startX, int startY, int endX, int endY, Empire limitToEmpireInfluence)
		{
			if (startX == endX && startY == endY)
			{
				//if destination is same as origin, don't bother.
				return null;
			}
			if (endX < 0 || endX >= gridCells.Length || endY < 0 || endY >= gridCells.Length)
			{
				//if destination is outside the game border, don't bother.
				return null;
			}
			if (!gridCells[endX][endY].passable)
			{
				//if destination is unpassable, it means a star occupies this spot
				//Find the shortest path to this star
				StarSystem destinationSystem = GetStarAtPoint(new Point(endX, endY));
				int travelLength = int.MaxValue;
				List<Point> currentPath = new List<Point>();
				for (int i = 0; i < destinationSystem.Size; i++)
				{
					travelLength = CheckPath(startX, startY, destinationSystem.X - 1, destinationSystem.Y + i, travelLength, limitToEmpireInfluence, ref currentPath);
					travelLength = CheckPath(startX, startY, destinationSystem.X + i, destinationSystem.Y - 1, travelLength, limitToEmpireInfluence, ref currentPath);
					travelLength = CheckPath(startX, startY, destinationSystem.X + destinationSystem.Size, destinationSystem.Y + i, travelLength, limitToEmpireInfluence, ref currentPath);
					travelLength = CheckPath(startX, startY, destinationSystem.X + i, destinationSystem.Y + destinationSystem.Size, travelLength, limitToEmpireInfluence, ref currentPath);
				}
				if (currentPath != null)
				{
					currentPath.Add(new Point(endX, endY)); //So we can skip search if the player is hovering above the same grid cell
				}
				return currentPath;
			}

			Point currentLoc;
			currentLoc.X = -1;
			currentLoc.Y = -1;

			bool alreadyResearched = false;
			if (gridCells[endX][endY].status == Closed_Value && originalX == startX && originalY == startY)
			{
				//Check to see if the end target is already researched
				alreadyResearched = true;
				currentLoc.X = endX;
				currentLoc.Y = endY;
			}
			else
			{
				originalX = startX;
				originalY = startY;
				Current_Value += 2;
				if (Current_Value == int.MaxValue - 1)
				{
					//prevent overflow, this only happens if too many pathfinding requests are made in one turn
					Current_Value = 0;
				}
				Open_Value = Current_Value;
				Closed_Value = Current_Value + 1;
			}

			if (!alreadyResearched)
			{
				List<Point> openList = new List<Point>();

				//Add the starting point
				openList.Add(new Point(startX, startY));

				currentLoc = openList[0];

				while (!(currentLoc.X == endX && currentLoc.Y == endY))
				{
					//Ran out of available places to try, shouldn't happen, but this will prevent infinite loops
					if (openList.Count == 0)
					{
						//unable to find a path to the endpoint
						return null;
					}
					openList.Remove(currentLoc);
					bool skip = false;
					if (gridCells[currentLoc.X][currentLoc.Y].status == Closed_Value)
					{
						skip = true;
					}
					if (!skip)
					{
						gridCells[currentLoc.X][currentLoc.Y].status = Closed_Value;
						for (int i = 0; i < 3; i++)
						{
							for (int j = 0; j < 3; j++)
							{
								if (i == 1 && j == 1)
								{
									//This is the current position, skip
									continue;
								}
								int newX = currentLoc.X + (i - 1);
								int newY = currentLoc.Y + (j - 1);
								//Make sure it's within legal range
								if (newX >= 0 && newX < gridCells.Length &&
									newY >= 0 && newY < gridCells.Length)
								{
									if (!gridCells[newX][newY].passable)
									{
										continue;
									}
									if (limitToEmpireInfluence != null && limitToEmpireInfluence != gridCells[newX][newY].dominantEmpire)
									{
										//If we're limited to influence, and this grid cell is not under dominant influence of selected empire, skip
										continue;
									}

									int nodeCost = gridCells[newX][newY].nebulaDensity;
									if (i == j || (i == 0 && j == 2) || (i == 2 && j == 0))
									{
										nodeCost = (int)(gridCells[newX][newY].nebulaDensity * 1.414);
									}

									int remainingX = (endX - newX);
									int remainingY = (endY - newY);
									if (remainingX < 0)
									{
										remainingX *= -1;
									}
									if (remainingY < 0)
									{
										remainingY *= -1;
									}
									int newG = gridCells[currentLoc.X][currentLoc.Y].G + nodeCost;
									if (gridCells[newX][newY].status == Open_Value || gridCells[newX][newY].status == Closed_Value)
									{
										if (gridCells[newX][newY].G <= newG)
										{
											//less cost than the current path, so skip
											continue;
										}
									}

									//It's both passable and not in closed list
									int H = (remainingX < remainingY ? remainingX * 7 + ((remainingY - remainingX) * 5) : remainingY * 7 + ((remainingX - remainingY) * 5));
									gridCells[newX][newY].G = newG;
									gridCells[newX][newY].F = newG + H;
									gridCells[newX][newY].parent = currentLoc;
									gridCells[newX][newY].status = Open_Value;
									openList.Add(new Point(newX, newY));
								}
							}
						}
					}
					//Find the next lowest F
					int lowestF = int.MaxValue;
					foreach (Point node in openList)
					{
						if (gridCells[node.X][node.Y].F < lowestF)
						{
							currentLoc = node;
							lowestF = gridCells[node.X][node.Y].F;
						}
					}
				}
			}

			List<Point> path = new List<Point>();
			while (!(currentLoc.X == startX && currentLoc.Y == startY))
			{
				path.Add(currentLoc);
				currentLoc = gridCells[currentLoc.X][currentLoc.Y].parent;
			}

			path.Reverse();
			return path;
		}

		private int CheckPath(int startX, int startY, int endX, int endY, int travelLength, Empire whichEmpire, ref List<Point> currentPath)
		{
			List<Point> tempPath = GetPath(startX, startY, endX, endY, whichEmpire);
			if (tempPath != null)
			{
				int tempAmount = 0;
				for (int i = 0; i < tempPath.Count; i++)
				{
					if (i < tempPath.Count - 1)
					{
						if (tempPath[i].X == tempPath[i + 1].X || tempPath[i].Y == tempPath[i + 1].Y)
						{
							//vertical/horizontal movement
							tempAmount += gridCells[tempPath[i].X][tempPath[i].Y].movementCost;
						}
						else
						{
							//diagonal movement
							tempAmount += gridCells[tempPath[i].X][tempPath[i].Y].diagonalMovementCost;
						}
					}
				}
				if (tempAmount < travelLength)
				{
					currentPath = tempPath;
					return tempAmount;
				}
			}
			return travelLength;
		}
		#endregion
	}

	#region GridCell Class
	internal struct GridCell
	{
		internal int nebulaDensity;
		internal int movementCost;
		internal int diagonalMovementCost;
		internal bool passable;

		internal Point parent;
		internal int status;
		internal int F;
		internal int G;

		internal Empire dominantEmpire;
		internal Empire secondaryEmpire;
	}
	#endregion

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

	internal class QuadNode
	{
		internal QuadNode[] nodes;
		internal int X;
		internal int Y;
		internal int Length;
		internal StarSystem starSystem;

		public QuadNode(int x, int y, int length, List<StarSystem> systems)
		{
			X = x;
			Y = y;
			Length = length;

			//find which star this cell refers to
			foreach (StarSystem system in systems)
			{
				if (system.X >= X && system.X <= X + length &&
					system.Y >= Y && system.Y <= Y + length)
				{
					if (starSystem == null)
					{
						starSystem = system;
					}
					else
					{
						//More than one star system lies within this quadrant, divide this into smaller quadrants until only at most one system is in a quadrant
						starSystem = null;
						nodes = new QuadNode[4];
						nodes[0] = new QuadNode(x, y, length / 2, systems);
						nodes[1] = new QuadNode(x + length / 2, y, length / 2, systems);
						nodes[2] = new QuadNode(x, y + length / 2, length / 2, systems);
						nodes[3] = new QuadNode(x + length / 2, y + length / 2, length / 2, systems);
						break;
					}
				}
			}
		}

		public void GetStarsInArea(int x, int y, int width, int height, List<StarSystem> starSystems)
		{
			if (starSystem != null)
			{
				if (starSystem.X >= x && starSystem.X <= x + width && starSystem.Y >= y && starSystem.Y <= y + height)
				{
					starSystems.Add(starSystem);
				}
			}
			else if (nodes != null)
			{
				if (x <= X && x + width >= X + Length && y <= Y && y + height >= Y + Length)
				{
					//This node is completely enclosed, grab all stars within
					GetAllStars(starSystems);
				}
				else
				{
					//Part or all of the node may be inside the viewing area, if so, check
					foreach (QuadNode node in nodes)
					{
						if (!(x > node.X + node.Length || x + width < node.X || y > node.Y + node.Length || y + height < node.Y))
						{
							node.GetStarsInArea(x, y, width, height, starSystems);
						}
					}
				}
			}
		}

		public void GetAllStars(List<StarSystem> starSystems)
		{
			//This happens when the node is completely enclosed by the viewing area
			if (starSystem != null)
			{
				starSystems.Add(starSystem);
			}
			else if (nodes != null)
			{
				foreach (QuadNode node in nodes)
				{
					node.GetAllStars(starSystems);
				}
			}
		}
	}
	#endregion
}
