using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	class Fleet
	{
		#region Member Variables
		private int galaxyX;
		private int galaxyY;

		private Empire empire;
		private List<Point> travelNodes;
		private List<Point> tentativeNodes;
		private Dictionary<Ship, int> ships;
		private List<Ship> orderedShips; //For reference uses
		private List<TransportShip> transportShips;

		private int maxSpeed;
		private int remainingMoves;
		private int nodeRemainingMoves;
		private int remainingAmount;
		private int tentativeAmount;
		private bool finishedMovingThisTurn;
		#endregion

		#region Properties
		public int GalaxyX
		{
			get { return galaxyX; }
			set { galaxyX = value; }
		}

		public int GalaxyY
		{
			get { return galaxyY; }
			set { galaxyY = value; }
		}

		public Empire Empire
		{
			get { return empire; }
			set { empire = value; }
		}

		public List<Point> TravelNodes
		{
			get { return travelNodes; }
			set { travelNodes = value; }
		}

		public List<Point> TentativeNodes
		{
			get { return tentativeNodes; }
			set { tentativeNodes = value; }
		}

		public int ETA
		{
			get { return (remainingAmount / maxSpeed) + ((remainingAmount % maxSpeed > 0) ? 1 : 0); }
		}

		public int TentativeETA
		{
			get { return (tentativeAmount / maxSpeed) + ((tentativeAmount % maxSpeed > 0) ? 1 : 0); }
		}

		public int RemainingMovement
		{
			get { return remainingAmount; }
			set { remainingAmount = value; }
		}

		public Dictionary<Ship, int> Ships
		{
			get
			{
				return ships;
			}
		}
		public List<Ship> OrderedShips
		{
			get
			{
				return orderedShips;
			}
		}
		public List<TransportShip> TransportShips
		{
			get { return transportShips; }
		}
		public bool HasTransports
		{
			get { return transportShips.Count > 0; }
		}
		#endregion

		#region Constructors
		public Fleet()
		{
			ships = new Dictionary<Ship, int>();
			orderedShips = new List<Ship>();
			transportShips = new List<TransportShip>();
			remainingMoves = maxSpeed;
			finishedMovingThisTurn = false;
		}
		#endregion

		#region Functions
		private void UpdateSpeed()
		{
			maxSpeed = int.MaxValue;
			foreach (Ship ship in orderedShips)
			{
				if (ship.engine.GetGalaxySpeed() < maxSpeed)
				{
					maxSpeed = ship.engine.GetGalaxySpeed();
				}
			}
			if (transportShips.Count > 0)
			{
				maxSpeed -= 1;
			}
			remainingMoves = maxSpeed;
		}

		public void SetTentativePath(int x, int y, Galaxy galaxy)
		{
			if (galaxyX == x && galaxyY == y)
			{
				//if destination is same as origin, don't bother.
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}
			if (travelNodes != null && x == travelNodes[travelNodes.Count - 1].X && y == travelNodes[travelNodes.Count - 1].Y)
			{
				//Same path as current path
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}
			if (tentativeNodes != null && x == tentativeNodes[tentativeNodes.Count - 1].X && y == tentativeNodes[tentativeNodes.Count - 1].Y)
			{
				// Existing tentative path
				return;
			}

			List<Point> path = galaxy.GetPath(galaxyX, galaxyY, x, y, null);
			if (path == null)
			{
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}

			tentativeAmount = CalculatePathCost(galaxy.GetGridCells(), path);
			tentativeNodes = path;
			if (tentativeNodes.Count == 0)
			{
				tentativeNodes = null;
				tentativeAmount = 0;
			}
		}

		public void ConfirmPath()
		{
			if (tentativeNodes != null)
			{
				Point[] nodes = new Point[tentativeNodes.Count];
				tentativeNodes.CopyTo(nodes);
				tentativeNodes = null;

				remainingAmount = tentativeAmount;
				tentativeAmount = 0;
				travelNodes = new List<Point>(nodes);
			}
		}

		public void RefreshPath(GridCell[][] gridCells)
		{
			remainingAmount = CalculatePathCost(gridCells, travelNodes);
			if (travelNodes.Count > 0)
			{
				nodeRemainingMoves = GetNodeMovementCost(gridCells, travelNodes[0]);
			}
		}

		private int CalculatePathCost(GridCell[][] gridCells, List<Point> nodes)
		{
			int amount = 0;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (gridCells[nodes[i].X][nodes[i].Y].passable)
				{
					if (i > 0)
					{
						if (nodes[i].X == nodes[i - 1].X || nodes[i].Y == nodes[i - 1].Y)
						{
							//vertical/horizontal movement
							amount += gridCells[nodes[i].X][nodes[i].Y].movementCost;
						}
						else
						{
							//diagonal movement
							amount += gridCells[nodes[i].X][nodes[i].Y].diagonalMovementCost;
						}
					}
					else
					{
						if (nodes[i].X == galaxyX || nodes[i].Y == galaxyY)
						{
							//vertical/horizontal movement
							amount += gridCells[nodes[i].X][nodes[i].Y].movementCost;
						}
						else
						{
							//diagonal movement
							amount += gridCells[nodes[i].X][nodes[i].Y].diagonalMovementCost;
						}
					}
				}
			}
			return amount;
		}

		private int GetNodeMovementCost(GridCell[][] gridCells, Point node)
		{
			if (gridCells[node.X][node.Y].passable)
			{
				if (node.X == galaxyX || node.Y == galaxyY)
				{
					//vertical/horizontal movement
					return gridCells[node.X][node.Y].movementCost;
				}
				else
				{
					//diagonal movement
					return gridCells[node.X][node.Y].diagonalMovementCost;
				}
			}
			return -1;
		}

		public void AddShips(Ship ship, int amount)
		{
			if (ships.ContainsKey(ship))
			{
				ships[ship] += amount;
			}
			else
			{
				ships.Add(ship, amount);
				orderedShips.Add(ship);
				UpdateSpeed();
			}
		}

		public void SubtractShips(Ship ship, int amount)
		{
			if (ships.ContainsKey(ship))
			{
				ships[ship] -= amount;
			}
		}

		public void AddTransport(Race race, int amount)
		{
			bool added = false;
			foreach (TransportShip transport in transportShips)
			{
				if (transport.raceOnShip == race)
				{
					transport.amount += amount;
					added = true;
					break;
				}
			}
			if (!added)
			{
				TransportShip transport = new TransportShip();
				transport.raceOnShip = race;
				transport.amount = amount;
			}
		}

		public void SubtractTransport(Race race, int amount)
		{
			TransportShip transportShipToRemove = null;
			foreach (TransportShip transport in transportShips)
			{
				if (transport.raceOnShip == race)
				{
					transport.amount -= amount;
					if (transport.amount <= 0)
					{
						transportShipToRemove = transport;
					}
				}
			}
			if (transportShipToRemove != null)
			{
				TransportShips.Remove(transportShipToRemove);
			}
		}

		public void ClearEmptyShips()
		{
			List<Ship> shipsToRemove = new List<Ship>();
			foreach (KeyValuePair<Ship, int> ship in ships)
			{
				if (ship.Value <= 0)
				{
					shipsToRemove.Add(ship.Key);
				}
			}
			foreach (Ship ship in shipsToRemove)
			{
				ships.Remove(ship);
				orderedShips.Remove(ship);
			}
			UpdateSpeed();
		}

		public void ResetMove()
		{
			finishedMovingThisTurn = false;
			UpdateSpeed();
			remainingMoves = maxSpeed;
		}

		public bool Move(GridCell[][] gridCells)
		{
			if (!finishedMovingThisTurn)
			{
				if (remainingMoves > 0 && nodeRemainingMoves > 0)
				{
					remainingMoves--;
					nodeRemainingMoves--;
					if (nodeRemainingMoves <= 0)
					{
						galaxyX = travelNodes[0].X;
						galaxyY = travelNodes[0].Y;
						travelNodes.Remove(travelNodes[0]);
						if (travelNodes.Count > 0)
						{
							nodeRemainingMoves = GetNodeMovementCost(gridCells, travelNodes[0]);
							if (nodeRemainingMoves < 0)
							{
								//can't travel into star or something else
								travelNodes = null;
								nodeRemainingMoves = 0;
							}
						}
						else
						{
							//either it ends inside a star or finished moving.  In either case, clear the path
							travelNodes = null;
							nodeRemainingMoves = 0;
						}
					}
					return remainingMoves > 0 && nodeRemainingMoves > 0;
				}
				finishedMovingThisTurn = true;
			}
			return false;
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (KeyValuePair<Ship, int> ship in ships)
			{
				amount += (ship.Key.Maintenance * ship.Value);
			}
			return amount;
		}
		#endregion
	}
}
