using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan
{
	public enum GALAXYTYPE { RANDOM, CLUSTER, STAR, DIAMOND, RING };

	public class Gateway
	{
		public SectorObjectType Type { get; set; }
		public StarSystem SystemA { get; set; }
		public StarSystem SystemB { get; set; }

		public double Length { get; private set; }
		public float Angle { get; private set; }

		public Gateway(SectorObjectType type, StarSystem systemA, StarSystem systemB)
		{
			Type = type;
			SystemA = systemA;
			SystemB = systemB;

			float deltaX = systemB.X - systemA.X;
			float deltaY = systemB.Y - systemA.Y;
			double angle = Math.Atan2(deltaY, deltaX);
			Angle = (float)(angle * (180 / Math.PI));
			deltaX *= 32;
			deltaY *= 32;
			Length = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
		}
	}

	public class Galaxy
	{
		private List<StarSystem> starSystems = new List<StarSystem>();

		public List<Gateway> Gateways { get; set; }
		QuadNode ParentNode;
		//GorgonLibrary.Graphics.Sprite nebula;

		public int GalaxySize { get; private set; }

		/*public GorgonLibrary.Graphics.Sprite Nebula 
		{
			get { return nebula; }
		}*/

		/// <summary>
		/// Set up the galaxy
		/// </summary>
		public void GenerateGalaxy(System.Reflection.MethodInfo genGalaxyFunc, Object scriptInstance, Dictionary<string, string> vars, StarTypeManager starTypeManager, SectorTypeManager sectorTypeManager, SpriteManager spriteManager)
		{
			Random r = new Random();
			
			List<Dictionary<string, object>> ret = (List<Dictionary<string, object>>)genGalaxyFunc.Invoke(scriptInstance, new object[] { vars });

			// we have a list of stars.  go through the list and find the highest x/y value.  this will be the galaxy size
			GalaxySize = 0;
			foreach (Dictionary<string, object> star in ret)
			{
				if ((int)star["x"] >= GalaxySize) GalaxySize = (int)star["x"];
				if ((int)star["y"] >= GalaxySize) GalaxySize = (int)star["y"];
			}
			GalaxySize += 4;

			FillGalaxyWithStars(ret, starTypeManager, sectorTypeManager, spriteManager);

			Gateways = new List<Gateway>();
			ConnectGateways(sectorTypeManager, r);

			//ConvertNebulaToSprite();
		}

		public void ConstructQuadTree()
		{
			ParentNode = new QuadNode(0, 0, GalaxySize, GalaxySize, starSystems);
		}

		#region Star Retrieval Functions
		/// <summary>
		/// Get all stars in the area for drawing purposes
		/// </summary>
		/// <param name="top"></param>
		/// <param name="left"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public List<StarSystem> GetStarsInArea(int left, int top, int width, int height)
		{
			List<StarSystem> starsInArea = new List<StarSystem>();
			ParentNode.GetStarsInArea(left, top, width + 4, height + 4, starsInArea);
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
				if (starSystem.X <= point.X && starSystem.X + (starSystem.Sprite.Width / 32) > point.X && starSystem.Y <= point.Y && starSystem.Y + (starSystem.Sprite.Height / 32) > point.Y)
				{
					return starSystem;
				}
			}
			return null;
		}
		public void UpdateStars(float frameDeltaTime, Random r)
		{
			float time = frameDeltaTime*200;
			foreach (StarSystem system in starSystems)
			{
				system.Sprite.Update(time, r);
			}
		}
		#endregion

		#region Galaxy Filling Functions
		private void FillGalaxyWithStars(List<Dictionary<string, object>> starPoints, StarTypeManager starTypeManager, SectorTypeManager sectorTypeManager, SpriteManager spriteManager)
		{
			starSystems = new List<StarSystem>();
			NameGenerator nameGenerator = new NameGenerator();
			Random r = new Random();

			foreach (Dictionary<string, object> newStar in starPoints)
			{
				StarType type = starTypeManager.GetType((string)newStar["type"]);
				int x = (int)newStar["x"];
				int y = (int)newStar["y"];
				StarSystem newStarSystem = new StarSystem(nameGenerator.GetName(), x, y, type, sectorTypeManager, spriteManager, r);
				starSystems.Add(newStarSystem);
			}
		}
		#endregion

		#region Galaxy Enhancements
		public void ConnectGateways(SectorTypeManager sectorTypeManager, Random r)
		{
			// spencer's new and improved super-awesome nebula-generating code of awesomeness-ness
			/*LibNoise.RidgedMultifractal density = new LibNoise.RidgedMultifractal();
			LibNoise.Modfiers.CurveOutput curve = new LibNoise.Modfiers.CurveOutput(density);
			LibNoise.Modfiers.CurveControlPoint pt1 = new LibNoise.Modfiers.CurveControlPoint();
			LibNoise.Modfiers.CurveControlPoint pt2 = new LibNoise.Modfiers.CurveControlPoint();
			LibNoise.Modfiers.CurveControlPoint pt3 = new LibNoise.Modfiers.CurveControlPoint();
			LibNoise.Modfiers.CurveControlPoint pt4 = new LibNoise.Modfiers.CurveControlPoint();
			pt1.Input = -0.85;
			pt1.Output = 0.05;
			pt2.Input = 0.15;
			pt2.Output = 0.15;
			pt3.Input = 0.5;
			pt3.Output = 0.50;
			pt4.Input = 1;
			pt4.Output = 1;
			curve.ControlPoints.Add(pt1);
			curve.ControlPoints.Add(pt2);
			curve.ControlPoints.Add(pt3);
			curve.ControlPoints.Add(pt4);
			LibNoise.RidgedMultifractal red = new LibNoise.RidgedMultifractal();
			LibNoise.RidgedMultifractal green = new LibNoise.RidgedMultifractal();
			LibNoise.RidgedMultifractal blue = new LibNoise.RidgedMultifractal();
			density.Seed = DateTime.Now.Millisecond + DateTime.Now.Hour;
			density.Frequency = 0.015;
			red.Seed = DateTime.Now.Millisecond;
			red.Frequency = 0.005;
			green.Seed = DateTime.Now.Second + DateTime.Now.Millisecond;
			green.Frequency = 0.01;
			blue.Seed = DateTime.Now.Minute + DateTime.Now.Millisecond;
			blue.Frequency = 0.02;*/

			foreach (var gateWayType in sectorTypeManager.GetGatewayTypes())
			{
				//Not a direct connection, so no need to hook it up
				if (!gateWayType.ConnectsToAnother)
				{
					continue;
				}
				switch (gateWayType.ConnectionAlgorithm)
				{
					case ConnectionAlgorithm.CLOSEST:
						{
							ConnectClosest(gateWayType);
						} break;
					case ConnectionAlgorithm.FARTHEST:
						{
							ConnectFarthest(gateWayType);
						} break;
					case ConnectionAlgorithm.RANDOM:
						{
							ConnectRandom(gateWayType, r);
						} break;
					case ConnectionAlgorithm.MINIMUM:
						{
							ConnectMinimumSpanning(gateWayType, r);
						} break;
				}
			}
		}

		#region Connecting Functions
		private void ConnectClosest(SectorObjectType type)
		{
			List<StarSystem> systemsWithType = new List<StarSystem>();
			foreach (var system in starSystems)
			{
				foreach (var sector in system.SectorObjects)
				{
					if (sector.Type == type)
					{
						systemsWithType.Add(system);
						break;
					}
				}
			}
			List<Gateway> possibleGateways = new List<Gateway>();
			for (int i = 0; i < systemsWithType.Count - 1; i++)
			{
				for (int j = i + 1; j < systemsWithType.Count; j++)
				{
					Gateway gateway = new Gateway(type, systemsWithType[i], systemsWithType[j]);
					possibleGateways.Add(gateway);
				}
			}
			possibleGateways.Sort((a, b) => { return a.Length.CompareTo(b.Length); });
			foreach (var possibleGateway in possibleGateways)
			{
				SectorObject sectorObjectA = null;
				SectorObject sectorObjectB = null;
				bool isConnected = false;
				foreach (SectorObject sectorObject in possibleGateway.SystemA.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemB)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectA = sectorObject;
						break;
					}
				}
				foreach (SectorObject sectorObject in possibleGateway.SystemB.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemA)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectB = sectorObject;
						break;
					}
				}
				if (sectorObjectA != null && sectorObjectB != null && !isConnected)
				{
					sectorObjectA.ConnectsTo = possibleGateway.SystemB;
					sectorObjectB.ConnectsTo = possibleGateway.SystemA;
					Gateways.Add(possibleGateway);
				}
			}
		}

		private void ConnectFarthest(SectorObjectType type)
		{
			List<StarSystem> systemsWithType = new List<StarSystem>();
			foreach (var system in starSystems)
			{
				foreach (var sector in system.SectorObjects)
				{
					if (sector.Type == type)
					{
						systemsWithType.Add(system);
						break;
					}
				}
			}
			List<Gateway> possibleGateways = new List<Gateway>();
			for (int i = 0; i < systemsWithType.Count - 1; i++)
			{
				for (int j = i + 1; j < systemsWithType.Count; j++)
				{
					Gateway gateway = new Gateway(type, systemsWithType[i], systemsWithType[j]);
					possibleGateways.Add(gateway);
				}
			}
			possibleGateways.Sort((a, b) => { return b.Length.CompareTo(a.Length); });
			foreach (var possibleGateway in possibleGateways)
			{
				SectorObject sectorObjectA = null;
				SectorObject sectorObjectB = null;
				bool isConnected = false;
				foreach (SectorObject sectorObject in possibleGateway.SystemA.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemB)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectA = sectorObject;
						break;
					}
				}
				foreach (SectorObject sectorObject in possibleGateway.SystemB.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemA)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectB = sectorObject;
						break;
					}
				}
				if (sectorObjectA != null && sectorObjectB != null && !isConnected)
				{
					sectorObjectA.ConnectsTo = possibleGateway.SystemB;
					sectorObjectB.ConnectsTo = possibleGateway.SystemA;
					Gateways.Add(possibleGateway);
				}
			}
		}

		private void ConnectRandom(SectorObjectType type, Random r)
		{
			List<StarSystem> systemsWithType = new List<StarSystem>();
			foreach (var system in starSystems)
			{
				foreach (var sector in system.SectorObjects)
				{
					if (sector.Type == type)
					{
						systemsWithType.Add(system);
						break;
					}
				}
			}
			List<Gateway> possibleGateways = new List<Gateway>();
			for (int i = 0; i < systemsWithType.Count - 1; i++)
			{
				for (int j = i + 1; j < systemsWithType.Count; j++)
				{
					Gateway gateway = new Gateway(type, systemsWithType[i], systemsWithType[j]);
					possibleGateways.Add(gateway);
				}
			}
			List<Gateway> randomizedGateways = new List<Gateway>();
			while (possibleGateways.Count > 0)
			{
				int random = r.Next(possibleGateways.Count);
				randomizedGateways.Add(possibleGateways[random]);
				possibleGateways.RemoveAt(random);
			}
			foreach (var possibleGateway in randomizedGateways)
			{
				SectorObject sectorObjectA = null;
				SectorObject sectorObjectB = null;
				bool isConnected = false;
				foreach (SectorObject sectorObject in possibleGateway.SystemA.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemB)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectA = sectorObject;
						break;
					}
				}
				foreach (SectorObject sectorObject in possibleGateway.SystemB.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemA)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectB = sectorObject;
						break;
					}
				}
				if (sectorObjectA != null && sectorObjectB != null && !isConnected)
				{
					sectorObjectA.ConnectsTo = possibleGateway.SystemB;
					sectorObjectB.ConnectsTo = possibleGateway.SystemA;
					Gateways.Add(possibleGateway);
				}
			}
		}

		private void ConnectMinimumSpanning(SectorObjectType type, Random r)
		{
			List<StarSystem> systemsWithType = new List<StarSystem>();
			foreach (var system in starSystems)
			{
				foreach (var sector in system.SectorObjects)
				{
					if (sector.Type == type)
					{
						systemsWithType.Add(system);
						break;
					}
				}
			}
			StarSystem[][] grid = new StarSystem[GalaxySize][];
			for (int i = 0; i < GalaxySize; i++)
			{
				grid[i] = new StarSystem[GalaxySize];
				for (int j = 0; j < GalaxySize; j++)
				{
					long distance = long.MaxValue;
					StarSystem closestSystem = null;
					foreach (StarSystem system in systemsWithType)
					{
						long value = (system.X - i)*(system.X - i) + (system.Y - j)*(system.Y - j);
						if (value < distance)
						{
							closestSystem = system;
							distance = value;
						}
					}
					grid[i][j] = closestSystem;
				}
			}
			
			List<Gateway> possibleGateways = new List<Gateway>();
			for (int i = 0; i < GalaxySize; i++)
			{
				for (int j = 0; j < GalaxySize; j++)
				{
					if (i > 0) //so we don't go out of bounds
					{
						if (grid[i - 1][j] != grid[i][j] && !ContainsPossibleGatewayAlready(grid[i - 1][j], grid[i][j], possibleGateways)) //it's an adjacent star
						{
							possibleGateways.Add(new Gateway(type, grid[i - 1][j], grid[i][j]));
						}
					}
					if (i < GalaxySize - 1) //so we don't go out of bounds
					{
						if (grid[i + 1][j] != grid[i][j] && !ContainsPossibleGatewayAlready(grid[i + 1][j], grid[i][j], possibleGateways)) //it's an adjacent star
						{
							possibleGateways.Add(new Gateway(type, grid[i + 1][j], grid[i][j]));
						}
					}
					if (j > 0) //so we don't go out of bounds
					{
						if (grid[i][j - 1] != grid[i][j] && !ContainsPossibleGatewayAlready(grid[i][j - 1], grid[i][j], possibleGateways)) //it's an adjacent star
						{
							possibleGateways.Add(new Gateway(type, grid[i][j - 1], grid[i][j]));
						}
					}
					if (j < GalaxySize - 1) //so we don't go out of bounds
					{
						if (grid[i][j + 1] != grid[i][j] && !ContainsPossibleGatewayAlready(grid[i][j + 1], grid[i][j], possibleGateways)) //it's an adjacent star
						{
							possibleGateways.Add(new Gateway(type, grid[i][j + 1], grid[i][j]));
						}
					}
				}
			}
			possibleGateways.Sort((a, b) => { return a.Length.CompareTo(b.Length); });
			foreach (var possibleGateway in possibleGateways)
			{
				SectorObject sectorObjectA = null;
				SectorObject sectorObjectB = null;
				bool isConnected = false;
				foreach (SectorObject sectorObject in possibleGateway.SystemA.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemB)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectA = sectorObject;
						break;
					}
				}
				foreach (SectorObject sectorObject in possibleGateway.SystemB.SectorObjects)
				{
					if (sectorObject.Type != type)
					{
						continue;
					}
					if (sectorObject.ConnectsTo != null)
					{
						if (sectorObject.ConnectsTo == possibleGateway.SystemA)
						{
							isConnected = true;
							break;
						}
					}
					else
					{
						sectorObjectB = sectorObject;
						break;
					}
				}
				if (sectorObjectA != null && sectorObjectB != null && !isConnected)
				{
					sectorObjectA.ConnectsTo = possibleGateway.SystemB;
					sectorObjectB.ConnectsTo = possibleGateway.SystemA;
					Gateways.Add(possibleGateway);
				}
			}
		}
		private static bool ContainsPossibleGatewayAlready(StarSystem a, StarSystem b, List<Gateway> possibleGateways)
		{
			foreach (var gateway in possibleGateways)
			{
				if ((gateway.SystemA == a && gateway.SystemB == b) ||
				    (gateway.SystemB == a && gateway.SystemA == b))
				{
					return true;
				}
			}
			return false;
		}
		#endregion
		/*private void ConvertNebulaToSprite()
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
			newImage.Lock(true);
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < gridCells[i].Length; j++)
				{
					newImage[i, j] = System.Drawing.Color.FromArgb(255, (int)((gridCells[i][j].nebulaDensityR / 100.0f) * 255), (int)((gridCells[i][j].nebulaDensityG / 100.0f) * 255), (int)((gridCells[i][j].nebulaDensityB / 100.0f) * 255)).ToArgb();
				}
			}
			newImage.Unlock();
			nebula = new GorgonLibrary.Graphics.Sprite("nebula", image);
			//nebula.Smoothing = GorgonLibrary.Graphics.Smoothing.Smooth;
		}*/
		#endregion

		#region Galaxy Setup
		/*public List<StarSystem> SetStartingSystems(Empire empire, PlanetTypeManager planetTypeManager, RegionTypeManager regionTypeManager, ResourceManager resourceManager, out List<Sector> startingSectors)
		{
			Random r = new Random();
			List<StarSystem> startingSystems = new List<StarSystem>();
			startingSectors = new List<Sector>();
			List<StarSystem> addedSystems = new List<StarSystem>(); //for tracking purposes
			for (int i = 0; i < empire.EmpireRace.StartingSystems.Count; i++)
			{
				List<Sector> ownedSectors = new List<Sector>();
				if (i == 0) //First system will be randomly placed
				{
					while (true)
					{
						int starIter = r.Next(starSystems.Count);
						if (starSystems[starIter].Type.Inhabitable && starSystems[starIter].EmpiresWithSectorsInThisSystem.Count == 0)
						{
							starSystems[starIter].SetSystem(empire, empire.EmpireRace.StartingSystems[i], planetTypeManager, regionTypeManager, resourceManager, r, out ownedSectors);
							starSystems[starIter].UpdateOwners();
							startingSystems.Add(starSystems[starIter]);
							startingSectors.AddRange(ownedSectors);
							addedSystems.Add(starSystems[starIter]);
							break;
						}
					}
				}
				else
				{
					int distance = int.MaxValue;
					StarSystem potentialSystem = null;
					//Find the closest inhabitable system that are unowned
					for (int s = 0; s < starSystems.Count; s++)
					{
						if (!starSystems[s].Type.Inhabitable || starSystems[s].EmpiresWithSectorsInThisSystem.Count != 0)
						{
							continue;
						}
						for (int t = 0; t < addedSystems.Count; t++)
						{
							int xDist = addedSystems[t].X - starSystems[s].X;
							int yDist = addedSystems[t].Y - starSystems[s].Y;
							int currentDistance = (xDist * xDist) + (yDist * yDist);
							if (currentDistance < distance)
							{
								potentialSystem = starSystems[s];
								distance = currentDistance;
							}
						}
					}
					potentialSystem.SetSystem(empire, empire.EmpireRace.StartingSystems[i], planetTypeManager, regionTypeManager, resourceManager, r, out ownedSectors);
					potentialSystem.UpdateOwners();
					startingSectors.AddRange(ownedSectors);
					startingSystems.Add(potentialSystem);
					addedSystems.Add(potentialSystem);
				}
			}
			return startingSystems;
		}*/

		/*private void AddStarlane(StarSystem system1, StarSystem system2, double speed)
		{
			foreach (Starlane starlane in Starlanes)
			{
				if ((starlane.SystemA == system1 || starlane.SystemA == system2) &&
					(starlane.SystemB == system1 || starlane.SystemB == system2))
				{
					return;
				}
			}
			Starlane newStarlane = new Starlane();
			newStarlane.SystemA = system1;
			newStarlane.SystemB = system2;
			newStarlane.Visible = false;
			newStarlane.Speed = speed;
			float deltaX = system2.X - system1.X;
			float deltaY = system2.Y - system1.Y;
			double angle = Math.Atan2(deltaY, deltaX);
			newStarlane.Angle = (float)(angle * (180 / Math.PI));
			//newStarlane.Angle = (float)(Math.Atan2(system2.X - system1.X, system2.Y - system1.Y) * (180 / Math.PI));
			newStarlane.Length = Math.Sqrt(((system2.X - system1.X) * 32) * ((system2.X - system1.X) * 32) + ((system2.Y - system1.Y) * 32) * ((system2.Y - system1.Y) * 32));

			system1.AddStarlane(newStarlane);
			system2.AddStarlane(newStarlane);
			Starlanes.Add(newStarlane);
		}

		private void AddInvisibleStarlane(StarSystem system1, StarSystem system2, double speed)
		{
			Starlane newStarlane = new Starlane();
			newStarlane.SystemA = system1;
			newStarlane.SystemB = system2;
			newStarlane.Visible = false;
			newStarlane.Speed = speed;
			float deltaX = system2.X - system1.X;
			float deltaY = system2.Y - system1.Y;
			double angle = Math.Atan2(deltaY, deltaX);
			newStarlane.Angle = (float)(angle * (180 / Math.PI));
			//newStarlane.Angle = (float)(Math.Atan2(system2.X - system1.X, system2.Y - system1.Y) * (180 / Math.PI));
			newStarlane.Length = Math.Sqrt(((system2.X - system1.X) * 32) * ((system2.X - system1.X) * 32) + ((system2.Y - system1.Y) * 32) * ((system2.Y - system1.Y) * 32));

			system1.AddInvisibleStarlane(newStarlane);
			system2.AddInvisibleStarlane(newStarlane);
			InvisibleStarlanes.Add(newStarlane);
		}*/
		#endregion

		#region Pathfinding functions

		/*public List<KeyValuePair<StarSystem, Starlane>> GetPath(StarSystem startingSystem, StarSystem destinationSystem, bool direct, StarSystem systemToAddAtStart, Empire currentEmpire, out double length)
		{
			length = 0;

			if (startingSystem == destinationSystem)
			{
				//If destination is same as origin, don't bother.
				return null;
			}

			foreach (StarSystem starSystem in starSystems)
			{
				starSystem.Distance = double.MaxValue;
				starSystem.PreviousSystem = null;
			}
			startingSystem.Distance = 0;
			List<StarSystem> processedSystems = new List<StarSystem>();
			List<StarSystem> orderedList = new List<StarSystem>();
			 
			//Add the starting point
			orderedList.Add(startingSystem);

			StarSystem currentSystem = orderedList[0];

			if (direct)
			{
				bool found = false;
				foreach (Starlane starlane in currentSystem.Starlanes)
				{
					if (starlane.SystemA == currentSystem && starlane.SystemB == destinationSystem)
					{
						starlane.SystemB.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
						starlane.SystemB.PreviousSystem = starlane.SystemA;
						starlane.SystemB.PreviousStarlane = starlane;

						found = true;
						currentSystem = destinationSystem;
						break;
					}
					else if (starlane.SystemB == currentSystem && starlane.SystemA == destinationSystem)
					{
						starlane.SystemA.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
						starlane.SystemA.PreviousSystem = starlane.SystemB;
						starlane.SystemA.PreviousStarlane = starlane;

						found = true;
						currentSystem = destinationSystem;
						break;
					}
				}
				if (!found)
				{
					foreach (Starlane starlane in currentSystem.InvisibleStarlanes)
					{
						if (starlane.SystemA == currentSystem && starlane.SystemB == destinationSystem)
						{
							starlane.SystemB.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
							starlane.SystemB.PreviousSystem = starlane.SystemA;
							starlane.SystemB.PreviousStarlane = starlane;

							found = true;
							currentSystem = destinationSystem;
							break;
						}
						else if (starlane.SystemB == currentSystem && starlane.SystemA == destinationSystem)
						{
							starlane.SystemA.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
							starlane.SystemA.PreviousSystem = starlane.SystemB;
							starlane.SystemA.PreviousStarlane = starlane;

							found = true;
							currentSystem = destinationSystem;
							break;
						}
					}
				}
			}
			else
			{
				while (currentSystem != destinationSystem)
				{
					processedSystems.Add(currentSystem);

					if (currentSystem.IsThisSystemExploredByEmpire(currentEmpire))
					{
						foreach (Starlane starlane in currentSystem.Starlanes)
						{
							if (starlane.SystemA == currentSystem && !processedSystems.Contains(starlane.SystemB))
							{
								if (currentSystem.Distance + starlane.Length * starlane.Speed < starlane.SystemB.Distance)
								{
									//Update the distance
									starlane.SystemB.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
									starlane.SystemB.PreviousSystem = starlane.SystemA;
									starlane.SystemB.PreviousStarlane = starlane;

									if (!orderedList.Contains(starlane.SystemB))
									{
										orderedList.Add(starlane.SystemB);
									}
								}
							}
							else if (starlane.SystemB == currentSystem && !processedSystems.Contains(starlane.SystemA))
							{
								if (currentSystem.Distance + starlane.Length * starlane.Speed < starlane.SystemA.Distance)
								{
									//Update the distance
									starlane.SystemA.Distance = currentSystem.Distance + starlane.Length * starlane.Speed;
									starlane.SystemA.PreviousSystem = starlane.SystemB;
									starlane.SystemA.PreviousStarlane = starlane;

									if (!orderedList.Contains(starlane.SystemA))
									{
										orderedList.Add(starlane.SystemA);
									}
								}
							}
						}
					}
					foreach (Starlane invisibleStarlane in currentSystem.InvisibleStarlanes)
					{
						if (invisibleStarlane.SystemA == currentSystem && invisibleStarlane.SystemB == destinationSystem)
						{
							if (currentSystem.Distance + (invisibleStarlane.Length * invisibleStarlane.Speed) < invisibleStarlane.SystemB.Distance)
							{
								//Update the distance
								invisibleStarlane.SystemB.Distance = currentSystem.Distance + invisibleStarlane.Length * invisibleStarlane.Speed;
								invisibleStarlane.SystemB.PreviousSystem = invisibleStarlane.SystemA;
								invisibleStarlane.SystemB.PreviousStarlane = invisibleStarlane;

								if (!orderedList.Contains(invisibleStarlane.SystemB))
								{
									orderedList.Add(invisibleStarlane.SystemB);
								}
							}
						}
						else if (invisibleStarlane.SystemB == currentSystem && invisibleStarlane.SystemA == destinationSystem)
						{
							if (currentSystem.Distance + (invisibleStarlane.Length * invisibleStarlane.Speed) < invisibleStarlane.SystemA.Distance)
							{
								//Update the distance
								invisibleStarlane.SystemA.Distance = currentSystem.Distance + invisibleStarlane.Length * invisibleStarlane.Speed;
								invisibleStarlane.SystemA.PreviousSystem = invisibleStarlane.SystemB;
								invisibleStarlane.SystemA.PreviousStarlane = invisibleStarlane;

								if (!orderedList.Contains(invisibleStarlane.SystemA))
								{
									orderedList.Add(invisibleStarlane.SystemA);
								}
							}
						}
					}

					orderedList.Sort((SystemA, SystemB) => { return SystemA.Distance.CompareTo(SystemB.Distance); });

					currentSystem = orderedList[0];
					orderedList.RemoveAt(0);
				}
			}

			List<KeyValuePair<StarSystem, Starlane>> route = new List<KeyValuePair<StarSystem, Starlane>>();

			while (currentSystem != startingSystem)
			{
				route.Insert(0, new KeyValuePair<StarSystem, Starlane>(currentSystem, currentSystem.PreviousStarlane));
				length += currentSystem.PreviousStarlane.Length * currentSystem.PreviousStarlane.Speed;
				currentSystem = currentSystem.PreviousSystem;
			}
			if (systemToAddAtStart != null && currentSystem != systemToAddAtStart)
			{
				bool found = false;
				foreach (Starlane lane in systemToAddAtStart.Starlanes)
				{
					if ((lane.SystemB == currentSystem || lane.SystemA == currentSystem) && (lane.SystemA == systemToAddAtStart || lane.SystemB == systemToAddAtStart))
					{
						route.Insert(0, new KeyValuePair<StarSystem, Starlane>(currentSystem, lane));
						length += lane.Length * lane.Speed;
						found = true;
						break;
					}
				}
				if (!found)
				{
					foreach (Starlane lane in systemToAddAtStart.InvisibleStarlanes)
					{
						if ((lane.SystemB == currentSystem || lane.SystemA == currentSystem) && (lane.SystemA == systemToAddAtStart || lane.SystemB == systemToAddAtStart))
						{
							route.Insert(0, new KeyValuePair<StarSystem, Starlane>(currentSystem, lane));
							length += lane.Length * lane.Speed;
							found = true;
							break;
						}
					}
				}
				route.Insert(0, new KeyValuePair<StarSystem, Starlane>(systemToAddAtStart, null));
			}
			else
			{
				route.Insert(0, new KeyValuePair<StarSystem, Starlane>(currentSystem, null));
			}

			return route;
		}*/

		/*public List<Point> GetPath(int startX, int startY, int endX, int endY, Empire limitToEmpireInfluence)
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
				BinaryHeap binaryHeap = new BinaryHeap();
				//List<Point> openList = new List<Point>();

				//Add the starting point
				//openList.Add(new Point(startX, startY));
				binaryHeap.AddPoint(new Point(startX, startY), gridCells);

				currentLoc = binaryHeap.GetNextPoint(gridCells);
				//currentLoc = openList[0];

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
									//Skip non-passable cells
									if (!gridCells[newX][newY].passable)
									{
										continue;
									}
									if (limitToEmpireInfluence != null && limitToEmpireInfluence != gridCells[newX][newY].dominantEmpire)
									{
										//If we're limited to influence, and this grid cell is not under dominant influence of selected empire, skip
										continue;
									}

									int nodeCost = gridCells[newX][newY].movementCost;
									if (i == j || (i == 0 && j == 2) || (i == 2 && j == 0))
									{
										nodeCost = gridCells[newX][newY].diagonalMovementCost;
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
										if (gridCells[newX][newY].G > newG)
										{
											//Need to update this cost, and update binary heap
											int H = (remainingX < remainingY ? remainingX * 7 + ((remainingY - remainingX) * 5) : remainingY * 7 + ((remainingX - remainingY) * 5));
											gridCells[newX][newY].G = newG;
											gridCells[newX][newY].F = newG + H;
											gridCells[newX][newY].parent = currentLoc;
											gridCells[newX][newY].status = Open_Value;
											//openList.Add(new Point(newX, newY));
											binaryHeap.UpdatePoint(new Point(newX, newY), gridCells);
										}
									}
									else
									{
										//It's both passable and not in open or closed list
										int H = (remainingX < remainingY ? remainingX * 7 + ((remainingY - remainingX) * 5) : remainingY * 7 + ((remainingX - remainingY) * 5));
										gridCells[newX][newY].G = newG;
										gridCells[newX][newY].F = newG + H;
										gridCells[newX][newY].parent = currentLoc;
										gridCells[newX][newY].status = Open_Value;
										binaryHeap.AddPoint(new Point(newX, newY), gridCells);
										//openList.Add(new Point(newX, newY));
									}
								}
							}
						}
					}
					currentLoc = binaryHeap.GetNextPoint(gridCells);
					//Find the next lowest F
					/*int lowestF = int.MaxValue;
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

		private class BinaryHeap
		{
			List<Point> points;

			public BinaryHeap()
			{
				points = new List<Point>();
				points.Add(new Point(-1, -1)); //We won't be using points[0]
			}

			public void AddPoint(Point point, GridCell[][] gridCells)
			{
				points.Add(point);
				gridCells[point.X][point.Y].iter = points.Count - 1;
				int m = points.Count - 1;
				while (m > 1)
				{
					if (gridCells[points[m].X][points[m].Y].F < gridCells[points[m / 2].X][points[m / 2].Y].F)
					{
						Point p = points[m / 2];
						points[m / 2] = points[m];
						points[m] = p;
						gridCells[points[m].X][points[m].Y].iter = m;
						gridCells[points[m / 2].X][points[m / 2].Y].iter = m / 2;
						m = m / 2;
					}
					else
					{
						break;
					}
				}
			}

			public Point GetNextPoint(GridCell[][] gridCells)
			{
				//Grab the topmost point (this is the lowest f score, then remove it
				//Then move the last point to top of list, and re-arrange points as needed
				Point point = points[1];
				points[1] = points[points.Count - 1];
				points.RemoveAt(points.Count - 1);
				if (points.Count == 1)
				{
					return point;
					//List is empty, nothing to do here
				}
				gridCells[points[1].X][points[1].Y].iter = 1;

				int m = 1;

				while (true)
				{
					int u = (m * 2);
					int v = (m * 2 + 1);
					int n = m;
					if (v <= Count) //Does both children exist
					{
						//Find which child is the lower of two
						bool lowest = (gridCells[points[u].X][points[u].Y].F < gridCells[points[v].X][points[v].Y].F);

						//Is parent higher than child?
						if (gridCells[points[m].X][points[m].Y].F > gridCells[points[lowest ? u : v].X][points[lowest ? u : v].Y].F)
						{
							n = lowest ? u : v;
						}
					}
					else if (u <= Count) //Does it have one child?
					{
						//Is parent higher than child?
						if (gridCells[points[m].X][points[m].Y].F > gridCells[points[u].X][points[u].Y].F)
						{
							n = u;
						}
					}

					if (n != m)
					{
						//Parent is greater than child, swap places
						Point p = points[m];
						points[m] = points[n];
						points[n] = p;
						gridCells[points[m].X][points[m].Y].iter = m;
						gridCells[points[n].X][points[n].Y].iter = n;
						m = n;
					}
					else
					{
						//reached the end
						break;
					}
				}

				return point;
			}

			public void UpdatePoint(Point point, GridCell[][] gridCells)
			{
				int m = gridCells[point.X][point.Y].iter;
				while (m > 1)
				{
					if (gridCells[points[m].X][points[m].Y].F < gridCells[points[m / 2].X][points[m / 2].Y].F)
					{
						Point p = points[m / 2];
						points[m / 2] = points[m];
						points[m] = p;
						gridCells[points[m].X][points[m].Y].iter = m;
						gridCells[points[m / 2].X][points[m / 2].Y].iter = m / 2;
						m = m / 2;
					}
					else
					{
						break;
					}
				}
			}

			public int Count
			{
				get { return points.Count - 1; }
			}
		}*/
		#endregion
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

	internal class QuadNode
	{
		internal QuadNode[] nodes;
		internal int X;
		internal int Y;
		internal int Width;
		internal int Height;
		internal StarSystem starSystem;

		public QuadNode(int x, int y, int width, int height, List<StarSystem> systems)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;

			if (height < 1 || width < 1)
			{
				//End of branch
				return;
			}

			//find which star this cell refers to
			foreach (StarSystem system in systems)
			{
				if (system.X >= X && system.X <= X + width &&
					system.Y >= Y && system.Y <= Y + height)
				{
					if (starSystem == null)
					{
						starSystem = system;
					}
					else
					{
						//More than one star system lies within this quadrant, divide this into smaller quadrants until only at most one system is in a quadrant
						starSystem = null;
						if (width > 1 && height == 1)
						{
							nodes = new QuadNode[2];
							nodes[0] = new QuadNode(x, y, width / 2, height, systems);
							nodes[1] = new QuadNode(x + (width / 2), y, width - (width / 2), height, systems);
						}
						else if (height > 1 && width == 1)
						{
							nodes = new QuadNode[2];
							nodes[0] = new QuadNode(x, y, width, height / 2, systems);
							nodes[1] = new QuadNode(x, y + (height / 2), width, height - (height / 2), systems);
						}
						else
						{
							nodes = new QuadNode[4];
							nodes[0] = new QuadNode(x, y, width / 2, height / 2, systems);
							nodes[1] = new QuadNode(x + (width / 2), y, width - (width / 2), height / 2, systems);
							nodes[2] = new QuadNode(x, y + (height / 2), width / 2, height - (height / 2), systems);
							nodes[3] = new QuadNode(x + (width / 2), y + (height / 2), width - (width / 2), height - (height / 2), systems);
						}
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
				if (x <= X && x + width >= X + Width && y <= Y && y + height >= Y + Height)
				{
					//This node is completely enclosed, grab all stars within
					GetAllStars(starSystems);
				}
				else
				{
					//Part or all of the node may be inside the viewing area, if so, check
					foreach (QuadNode node in nodes)
					{
						if (!(x > node.X + node.Width || x + width < node.X || y > node.Y + node.Height || y + height < node.Y))
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
