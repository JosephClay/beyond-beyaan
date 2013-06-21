using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class TravelNode
	{
		public StarSystem StarSystem { get; set; }

		//For drawing
		public float Length { get; set; }
		public float Angle { get; set; }
	}

	public class Fleet
	{
		#region Member Variables
		private float galaxyX;
		private float galaxyY;

		private Empire empire;
		private List<TravelNode> travelNodes;
		private List<TravelNode> tentativeNodes;
		private Dictionary<Ship, int> ships;
		private List<Ship> orderedShips; //For reference uses
		private List<TransportShip> transportShips;
		private StarSystem adjacentSystem;

		private int maxSpeed;
		private float remainingMoves;
		#endregion

		#region Properties
		public float GalaxyX
		{
			get { return galaxyX; }
			set { galaxyX = value; }
		}

		public float GalaxyY
		{
			get { return galaxyY; }
			set { galaxyY = value; }
		}

		public Empire Empire
		{
			get { return empire; }
			set { empire = value; }
		}

		public List<TravelNode> TravelNodes
		{
			get { return travelNodes; }
			set { travelNodes = value; }
		}

		public List<TravelNode> TentativeNodes
		{
			get { return tentativeNodes; }
			set { tentativeNodes = value; }
		}

		public StarSystem AdjacentSystem
		{
			get { return adjacentSystem; }
			set { adjacentSystem = value; }
		}

		public int ETA
		{
			get
			{
				float amount = 0;
				if (travelNodes != null)
				{
					for (int i = 0; i < travelNodes.Count; i++)
					{
						amount += travelNodes[i].Length;
					}
				}
				return (int)((amount / maxSpeed) + ((amount % maxSpeed > 0) ? 1 : 0));
			}
		}

		public int TentativeETA
		{
			get
			{
				float amount = 0;
				if (tentativeNodes != null)
				{
					for (int i = 0; i < tentativeNodes.Count; i++)
					{
						amount += tentativeNodes[i].Length;
					}
				}
				return (int)((amount / maxSpeed) + ((amount % maxSpeed > 0) ? 1 : 0));
			}
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

		public void SetTentativePath(StarSystem destination, Galaxy galaxy)
		{
			if (destination == adjacentSystem)
			{
				//if destination is same as origin, don't bother.
				tentativeNodes = null;
				return;
			}
			if (travelNodes != null && travelNodes[travelNodes.Count - 1].StarSystem == destination)
			{
				//Same path as current path
				tentativeNodes = null;
				return;
			}
			if (tentativeNodes != null && tentativeNodes[tentativeNodes.Count - 1].StarSystem == destination)
			{
				// Existing tentative path
				return;
			}

			StarSystem currentDestination = null;
			if (adjacentSystem == null) //Has left a system
			{
				currentDestination = travelNodes[0].StarSystem;
			}
			List<TravelNode> path = galaxy.GetPath(galaxyX, galaxyY, currentDestination, destination, empire);
			if (path == null)
			{
				tentativeNodes = null;
				return;
			}

			tentativeNodes = path;
			if (tentativeNodes.Count == 0)
			{
				tentativeNodes = null;
			}
		}

		public void ConfirmPath()
		{
			if (tentativeNodes != null)
			{
				TravelNode[] nodes = new TravelNode[tentativeNodes.Count];
				tentativeNodes.CopyTo(nodes);
				tentativeNodes = null;

				travelNodes = new List<TravelNode>(nodes);
			}
		}

		private float CalculatePathCost(List<TravelNode> nodes)
		{
			float amount = 0;
			for (int i = 0; i < nodes.Count; i++)
			{
				float x;
				float y;
				if (i == 0)
				{
					x = (galaxyX - nodes[i].StarSystem.X);
					y = (galaxyY - nodes[i].StarSystem.Y);
				}
				else
				{
					x = nodes[i - 1].StarSystem.X - nodes[i].StarSystem.X;
					y = nodes[i - 1].StarSystem.Y - nodes[i].StarSystem.Y;
				}
				amount += (float)Math.Sqrt(x * x + y * y);
			}
			return amount;
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
			UpdateSpeed();
			remainingMoves = maxSpeed;
		}

		public bool Move(float frameDeltaTime)
		{
			if (remainingMoves > 0)
			{
				float amountToMove = frameDeltaTime * maxSpeed;
				if (amountToMove > remainingMoves)
				{
					amountToMove = remainingMoves;
				}
				remainingMoves -= amountToMove;

				float xMov = (float)(Math.Cos(travelNodes[0].Angle) * amountToMove);
				float yMov = (float)(Math.Sin(travelNodes[0].Angle) * amountToMove);

				bool isLeftOfNode = galaxyX <= travelNodes[0].StarSystem.X;
				bool isTopOfNode = galaxyY <= travelNodes[0].StarSystem.Y;

				galaxyX += xMov;
				galaxyY += yMov;

				if ((galaxyX > travelNodes[0].StarSystem.X && isLeftOfNode) ||
					(galaxyX <= travelNodes[0].StarSystem.X && !isLeftOfNode) ||
					(galaxyY > travelNodes[0].StarSystem.Y && isTopOfNode) ||
					(galaxyY <= travelNodes[0].StarSystem.Y && !isTopOfNode))
				{
					//TODO: Carry over excess movement to next node

					//It has arrived at destination
					galaxyX = travelNodes[0].StarSystem.X;
					galaxyY = travelNodes[0].StarSystem.Y;
					adjacentSystem = travelNodes[0].StarSystem;
					travelNodes.RemoveAt(0);
					if (travelNodes.Count == 0)
					{
						travelNodes = null;
						remainingMoves = 0;
					}
				}
			}
			return remainingMoves > 0;
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
