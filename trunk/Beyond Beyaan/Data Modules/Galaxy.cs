﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum GALAXYTYPE { SMALL, MEDIUM, LARGE, HUGE/*, CLUSTER, STAR, DIAMOND, RING*/ };

	public class Galaxy
	{
		public const int PARSEC_SIZE_IN_PIXELS = 60;

		private List<StarSystem> starSystems = new List<StarSystem>();
		private BackgroundWorker _bw = new BackgroundWorker();

		public Action OnGenerateComplete;

		/*#region Pathfinding values
		private int Open_Value = 0;
		private int Closed_Value = 1;
		private int Current_Value = 0;

		int originalX = -1;
		int originalY = -1;
		#endregion*/
		private class GalaxyArgs
		{
			public GALAXYTYPE GalaxyType { get; set; }
			public int MinPlanets { get; set; }
			public int MaxPlanets { get; set; }
			public Random Random { get; set; }
		}

		public int GalaxySize { get; private set; }

		/// <summary>
		/// Set up the galaxy
		/// </summary>
		/// <param name="galaxyType"></param>
		/// <param name="minPlanets"></param>
		/// <param name="maxPlanets"></param>
		/// <param name="r"></param>
		/// <param name="reason"></param>
		public bool GenerateGalaxy(GALAXYTYPE galaxyType, int minPlanets, int maxPlanets, Random r, out string reason)
		{
			if (_bw.IsBusy)
			{
				reason = "Already in process of generating a galaxy";
				return false;
			}
			_bw.DoWork += GenerateGalaxyThread;
			_bw.RunWorkerCompleted += GenerateGalaxyCompleted;
			GalaxyArgs args = new GalaxyArgs();
			args.GalaxyType = galaxyType;
			args.MinPlanets = minPlanets;
			args.MaxPlanets = maxPlanets;
			args.Random = r;
			_bw.RunWorkerAsync(args);

			reason = null;
			return true;
		}

		private void GenerateGalaxyCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (OnGenerateComplete != null)
			{
				OnGenerateComplete();
			}
		}

		private void GenerateGalaxyThread(object sender, DoWorkEventArgs e)
		{
			GALAXYTYPE galaxyType;
			int minPlanets;
			int maxPlanets;
			Random r;
			try
			{
				GalaxyArgs args = (GalaxyArgs)e.Argument;
				galaxyType = args.GalaxyType;
				minPlanets = args.MinPlanets;
				maxPlanets = args.MaxPlanets;
				r = args.Random;
			}
			catch (Exception)
			{
				return;
			}

			bool[][] grid = GenerateRandom(galaxyType);
			/*switch (galaxyType)
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
			}*/

			GalaxySize = grid.Length;
			int numberOfStars = 0;
			switch (galaxyType)
			{
				case GALAXYTYPE.SMALL:
					numberOfStars = 24;
					break;
				case GALAXYTYPE.MEDIUM:
					numberOfStars = 48;
					break;
				case GALAXYTYPE.LARGE:
					numberOfStars = 70;
					break;
				case GALAXYTYPE.HUGE:
					numberOfStars = 108;
					break;
			}

			FillGalaxyWithStars(numberOfStars, minPlanets, maxPlanets, grid, r);
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
		private static bool[][] GenerateRandom(GALAXYTYPE type)
		{
			int width = 0;
			int height = 0;

			switch (type)
			{
				case GALAXYTYPE.SMALL:
				{
					width = 21;
					height = 15;
				} break;
				case GALAXYTYPE.MEDIUM:
					{
						width = 27;
						height = 21;
					} break;
				case GALAXYTYPE.LARGE:
					{
						width = 33;
						height = 24;
					} break;
				case GALAXYTYPE.HUGE:
					{
						width = 39;
						height = 27;
					} break;
			}
			bool[][] grid = new bool[width][];
			for (int i = 0; i < grid.Length; i++)
			{
				grid[i] = new bool[height];
				for (int j = 0; j < height; j++)
				{
					grid[i][j] = true;
				}
			}

			return grid;
		}

		/*
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
		}*/
		#endregion

		#region Galaxy Filling Functions
		private void FillGalaxyWithStars(int numberOfStars, int minPlanets, int maxPlanets, bool[][] grid, Random r)
		{
			starSystems = new List<StarSystem>();

			StarNode starTree = new StarNode(0, 0, grid.Length - 1, grid.Length - 1);

			//Set area where stars can be placed (circle, random, star, etc shaped galaxy)
			for (int i = 0; i < grid.Length; i++)
			{
				for (int j = 0; j < grid[i].Length; j++)
				{
					if (!grid[i][j])
					{
						starTree.RemoveNodeAtPosition(i, j);
					}
				}
			}

			int starId = 0;
			while (starTree.nodes.Count > 0 && numberOfStars > 0)
			{
				int x;
				int y;

				starTree.GetRandomStarPosition(r, out x, out y);

				//int newSize = r.Next(3) + 2;

				Color starColor = Color.White;
				string description = string.Empty;

				switch (r.Next(6)) //type of star
				{
					case 0: starColor = Color.Red;
						description = "Red stars are old, dull stars that commonly have poor planets";
						break;
					case 1: starColor = Color.Green;
						description = "Green stars are moderately bright and have a wide range of planetary types";
						break;
					case 2: starColor = Color.Purple;
						description = "Neutron stars are rare and offer the greatest chance of finding rich planets";
						break;
					case 3: starColor = Color.Blue;
						description = "Blue stars are relatively young stars with mineral rich lifeless planets";
						break;
					case 4: starColor = Color.White;
						description = "White stars burn incredibly hot and generally have hostile planets";
						break;
					case 5: starColor = Color.Yellow;
						description = "Yellow stars offer the best chance of discovering planets with friendly environment";
						break;
				}

				starSystems.Add(new StarSystem(NameGenerator.GetStarName(r), starId, x * PARSEC_SIZE_IN_PIXELS + (r.Next(PARSEC_SIZE_IN_PIXELS)), y * PARSEC_SIZE_IN_PIXELS + (r.Next(PARSEC_SIZE_IN_PIXELS)), starColor, description, minPlanets, maxPlanets, r));

				bool[][] invalidatedArea = Utility.CalculateDisc(2, 1);

				for (int i = 0; i < invalidatedArea.Length; i++)
				{
					for (int j = 0; j < invalidatedArea.Length; j++)
					{
						int xToInvalidate = (x - 2) + i;
						int yToInvalidate = (y - 2) + j;

						starTree.RemoveNodeAtPosition(xToInvalidate, yToInvalidate);
					}
				}

				numberOfStars--;
				starId++;
			}
		}
		#endregion

		#region Galaxy Setup
		public StarSystem SetHomeworld(Empire empire, out Planet homePlanet)
		{
			Random r = new Random();
			List<StarSystem> potentialSystems = new List<StarSystem>(starSystems);
			while (true)
			{
				if (potentialSystems.Count == 0)
				{
					homePlanet = null;
					return null;
				}
				var potentialSystem = potentialSystems[r.Next(potentialSystems.Count)];
				if (potentialSystem.EmpiresWithPlanetsInThisSystem.Count > 0)
				{
					potentialSystems.Remove(potentialSystem);
					continue;
				}
				//Validation checks
				bool FourParsecsSystem = false;
				bool SixParsecsSystem = false;
				bool AtLeastSixParsecsAwayFromOthers = true;
				foreach (StarSystem starSystem in starSystems)
				{
					if (potentialSystem != starSystem)
					{
						int x = potentialSystem.X - starSystem.X;
						int y = potentialSystem.Y - starSystem.Y;
						double distance = Math.Sqrt((x * x) + (y * y));
						if (distance <= PARSEC_SIZE_IN_PIXELS * 4)
						{
							if (!FourParsecsSystem)
							{
								FourParsecsSystem = true;
							}
							else
							{
								//Already have another star system that's within 4 parsecs
								SixParsecsSystem = true;
							}
							if (starSystem.EmpiresWithPlanetsInThisSystem.Count > 0)
							{
								AtLeastSixParsecsAwayFromOthers = false;
								break;
							}
						}
						else if (distance <= PARSEC_SIZE_IN_PIXELS * 6)
						{
							SixParsecsSystem = true;
							if (starSystem.EmpiresWithPlanetsInThisSystem.Count > 0)
							{
								AtLeastSixParsecsAwayFromOthers = false;
								break;
							}
						}
					}
				}
				if (FourParsecsSystem && SixParsecsSystem && AtLeastSixParsecsAwayFromOthers)
				{
					potentialSystem.SetHomeworld(empire, out homePlanet, r);
					return potentialSystem;
				}
				potentialSystems.Remove(potentialSystem);
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
		/// <param name="hasExtended"></param>
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
				newNode.IsValid = true; //IsDestinationValid(newDestination, hasExtended, whichEmpire);
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
				newNode.IsValid = true; // IsDestinationValid(newDestination, hasExtended, whichEmpire);
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

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Galaxy");
			writer.WriteStartElement("Stars");
			foreach (var star in starSystems)
			{
				writer.WriteStartElement("Star");
				writer.WriteAttributeString("ID", star.ID.ToString());
				writer.WriteAttributeString("Name", star.Name);
				writer.WriteAttributeString("Description", star.Description);
				writer.WriteAttributeString("XPos", star.X.ToString());
				writer.WriteAttributeString("YPos", star.Y.ToString());
				writer.WriteAttributeString("Color", star.StarColor[0] + "," + star.StarColor[1] + "," + star.StarColor[2] + "," + star.StarColor[3]);
				string exploredByList = string.Empty;
				foreach (var exploredBy in star.ExploredBy)
				{
					exploredByList = exploredByList + exploredBy.EmpireID + ",";
				}
				exploredByList = exploredByList.TrimEnd(new[] {','});
				writer.WriteAttributeString("ExploredBy", exploredByList);
				foreach (var planet in star.Planets)
				{
					writer.WriteStartElement("Planet");
					writer.WriteAttributeString("Name", planet.Name);
					writer.WriteAttributeString("Type", planet.PlanetTypeString);
					writer.WriteAttributeString("Owner", planet.Owner == null ? "-1" : planet.Owner.EmpireID.ToString());
					writer.WriteAttributeString("MaxPopulation", planet.PopulationMax.ToString());
					writer.WriteAttributeString("Buildings", planet.InfrastructureTotal.ToString());
					writer.WriteAttributeString("EnvironmentPercentage", planet.EnvironmentAmount.ToString());
					writer.WriteAttributeString("InfrastructurePercentage", planet.InfrastructureAmount.ToString());
					writer.WriteAttributeString("DefensePercentage", planet.DefenseAmount.ToString());
					writer.WriteAttributeString("ConstructionPercentage", planet.ConstructionAmount.ToString());
					writer.WriteAttributeString("ResearchPercentage", planet.ResearchAmount.ToString());
					writer.WriteAttributeString("EnvironmentLocked", planet.EnvironmentLocked.ToString());
					writer.WriteAttributeString("InfrastructureLocked", planet.InfrastructureLocked.ToString());
					writer.WriteAttributeString("DefenseLocked", planet.DefenseLocked.ToString());
					writer.WriteAttributeString("ConstructionLocked", planet.ConstructionLocked.ToString());
					writer.WriteAttributeString("ResearchLocked", planet.ResearchLocked.ToString());
					writer.WriteAttributeString("ShipBuilding", planet.ShipBeingBuilt == null ? "-1" : planet.ShipBeingBuilt.DesignID.ToString());
					writer.WriteAttributeString("AmountBuilt", planet.ShipConstructionAmount.ToString());
					writer.WriteAttributeString("RelocatingTo", planet.RelocateToSystem == null ? "-1" : planet.RelocateToSystem.StarSystem.ID.ToString());
					if (planet.TransferSystem.Key.StarSystem != null)
					{
						writer.WriteStartElement("TransferTo");
						writer.WriteAttributeString("StarSystem", planet.TransferSystem.Key.StarSystem.ID.ToString());
						writer.WriteAttributeString("Amount", planet.TransferSystem.Value.ToString());
						writer.WriteEndElement();
					}
					writer.WriteStartElement("Races");
					foreach (var race in planet.Races)
					{
						writer.WriteStartElement("Race");
						writer.WriteAttributeString("RaceName", race.RaceName);
						writer.WriteAttributeString("Amount", planet.GetRacePopulation(race).ToString());
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			//When neutral stuff are added, like space crystals, put them here
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public bool Load(XElement root, GameMain gameMain)
		{
			var galaxyElement = root.Element("Galaxy");
			if (galaxyElement == null)
			{
				return false;
			}
			starSystems = new List<StarSystem>();
			var starsElement = galaxyElement.Element("Stars");
			Dictionary<int, StarSystem> quickLookupSystems = new Dictionary<int, StarSystem>();
			foreach (var star in starsElement.Elements())
			{
				string name = star.Attribute("Name").Value;
				string description = star.Attribute("Description").Value;
				int id = int.Parse(star.Attribute("ID").Value);
				int xPos = int.Parse(star.Attribute("XPos").Value);
				int yPos = int.Parse(star.Attribute("YPos").Value);
				string[] color = star.Attribute("Color").Value.Split(new[] {','});
				float[] starColor = new float[4];
				for (int i = 0; i < 4; i++)
				{
					starColor[i] = float.Parse(color[i]);
				}
				StarSystem newStar = new StarSystem(name, id, xPos, yPos, Color.FromArgb((int)(255 * starColor[0]), (int)(255 * starColor[1]), (int)(255 * starColor[2]), (int)(255 * starColor[3])), description, gameMain.Random);
				string[] exploredBy = star.Attribute("ExploredBy").Value.Split(new [] {','});
				foreach (var explored in exploredBy)
				{
					newStar.AddEmpireExplored(gameMain.EmpireManager.GetEmpire(int.Parse(explored)));
				}
				foreach (var planetElement in star.Elements())
				{
					string planetName = planetElement.Attribute("Name").Value;
					string type = planetElement.Attribute("Type").Value;
					int owner = -1;
					if (!string.IsNullOrEmpty(planetElement.Attribute("Owner").Value))
					{
						owner = int.Parse(planetElement.Attribute("Owner").Value);
					}
					float populationMax = float.Parse(planetElement.Attribute("MaxPopulation").Value);
					var newPlanet = new Planet(planetName, type, populationMax, null, newStar, gameMain.Random);
					newPlanet.OwnerID = owner;
					newPlanet.InfrastructureTotal = float.Parse(planetElement.Attribute("Buildings").Value);
					newPlanet.EnvironmentAmount = int.Parse(planetElement.Attribute("EnvironmentPercentage").Value);
					newPlanet.InfrastructureAmount = int.Parse(planetElement.Attribute("InfrastructurePercentage").Value);
					newPlanet.DefenseAmount = int.Parse(planetElement.Attribute("DefensePercentage").Value);
					newPlanet.ConstructionAmount = int.Parse(planetElement.Attribute("ConstructionPercentage").Value);
					newPlanet.ResearchAmount = int.Parse(planetElement.Attribute("ResearchPercentage").Value);
					newPlanet.EnvironmentLocked = bool.Parse(planetElement.Attribute("EnvironmentLocked").Value);
					newPlanet.InfrastructureLocked = bool.Parse(planetElement.Attribute("InfrastructureLocked").Value);
					newPlanet.DefenseLocked = bool.Parse(planetElement.Attribute("DefenseLocked").Value);
					newPlanet.ConstructionLocked = bool.Parse(planetElement.Attribute("ConstructionLocked").Value);
					newPlanet.ResearchLocked = bool.Parse(planetElement.Attribute("ResearchLocked").Value);
					newPlanet.ShipBeingBuiltID = int.Parse(planetElement.Attribute("ShipBuilding").Value);
					newPlanet.ShipConstructionAmount = float.Parse(planetElement.Attribute("AmountBuilt").Value);
					newPlanet.RelocateToSystemID = int.Parse(planetElement.Attribute("RelocatingTo").Value);
					var transferTo = planetElement.Element("TransferTo");
					if (transferTo != null)
					{
						newPlanet.TransferSystemID = new KeyValuePair<int, int>(int.Parse(transferTo.Attribute("StarSystem").Value), int.Parse(transferTo.Attribute("Amount").Value));
					}
					var races = planetElement.Element("Races");
					foreach (var race in races.Elements())
					{
						newPlanet.AddRacePopulation(gameMain.RaceManager.GetRace(race.Attribute("RaceName").Value), float.Parse(race.Attribute("Amount").Value));
					}
					newStar.Planets.Add(newPlanet);
				}
				starSystems.Add(newStar);
				quickLookupSystems.Add(newStar.ID, newStar);
			}
			//Now match up IDs to actual classes
			foreach (var starSystem in starSystems)
			{
				foreach (var planet in starSystem.Planets)
				{
					if (planet.RelocateToSystemID != -1)
					{
						planet.RelocateToSystem = new TravelNode {StarSystem = quickLookupSystems[planet.RelocateToSystemID]};
						planet.RelocateToSystemID = -1;
					}
					if (!planet.TransferSystemID.Equals(new KeyValuePair<int, int>()))
					{
						planet.TransferSystem = new KeyValuePair<TravelNode, int>(new TravelNode { StarSystem = quickLookupSystems[planet.TransferSystemID.Key]}, planet.TransferSystemID.Value);
					}
				}
			}
			return true;
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
