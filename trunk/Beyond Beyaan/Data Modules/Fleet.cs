using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class TravelNode
	{
		public StarSystem StarSystem { get; set; }

		//For drawing
		public float Length { get; set; }
		public float Angle { get; set; }
		public bool IsValid { get; set; }
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

		public bool HasReserveTanks
		{
			get
			{
				if (HasTransports)
				{
					return false;
				}
				foreach (var ship in ships)
				{
					if (ship.Value == 0)
					{
						//Skip ships that won't be split off with this fleet
						continue;
					}
					bool hasReserve = false;
					foreach (var special in ship.Key.Specials)
					{
						if (special.ReserveFuelTanks)
						{
							hasReserve = true;
							break;
						}
					}
					if (!hasReserve)
					{
						return false;
					}
				}
				return true;
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
		public int ShipCount
		{
			get
			{
				int amount = 0;
				foreach (var ship in ships)
				{
					amount += ship.Value;
				}
				return amount;
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
			remainingMoves = maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
		}
		#endregion

		#region Functions
		private void UpdateSpeed()
		{
			maxSpeed = int.MaxValue;
			if (orderedShips.Count > 0)
			{
				foreach (Ship ship in orderedShips)
				{
					if (ship.Engine.Speed < maxSpeed)
					{
						maxSpeed = ship.Engine.Speed;
					}
				}
			}
			else
			{
				//Placeholder for now til I add technology check for best engine speed
				maxSpeed = 1;
			}
			remainingMoves = maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
		}

		public void SetTentativePath(StarSystem destination, bool hasExtendedFuelTanks, Galaxy galaxy)
		{
			if (destination == null || destination == adjacentSystem)
			{
				//if destination is same as origin, or nowhere, clear the tentative path
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
			List<TravelNode> path = galaxy.GetPath(galaxyX, galaxyY, currentDestination, destination, hasExtendedFuelTanks, empire);
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

		public bool ConfirmPath()
		{
			if (tentativeNodes != null)
			{
				foreach (var node in tentativeNodes)
				{
					if (!node.IsValid)
					{
						return false;
					}
				}
				TravelNode[] nodes = new TravelNode[tentativeNodes.Count];
				tentativeNodes.CopyTo(nodes);
				tentativeNodes = null;

				travelNodes = new List<TravelNode>(nodes);
			}
			else
			{
				//Null because target is either invalid or the system the fleet is currently adjacent
				travelNodes = null;
				tentativeNodes = null;
			}
			return true;
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
				transportShips.Add(transport);
			}
			maxSpeed = 1;
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
			List<TransportShip> transportsToRemove = new List<TransportShip>();
			foreach (var transport in transportShips)
			{
				if (transport.amount <= 0)
				{
					transportsToRemove.Add(transport);
				}
			}
			foreach (var transport in transportsToRemove)
			{
				transportShips.Remove(transport);
			}
			UpdateSpeed();
		}

		public void ResetMove()
		{
			UpdateSpeed();
			remainingMoves = maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
		}

		public bool Move(float frameDeltaTime)
		{
			if (travelNodes == null)
			{
				return false;
			}
			if (remainingMoves > 0)
			{
				adjacentSystem = null; //Left the system
				float amountToMove = frameDeltaTime * maxSpeed * Galaxy.PARSEC_SIZE_IN_PIXELS;
				if (amountToMove > remainingMoves)
				{
					amountToMove = remainingMoves;
				}
				remainingMoves -= amountToMove;

				float xMov = (float)(Math.Cos(travelNodes[0].Angle * (Math.PI / 180)) * amountToMove);
				float yMov = (float)(Math.Sin(travelNodes[0].Angle * (Math.PI / 180)) * amountToMove);

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
				else
				{
					float x = travelNodes[0].StarSystem.X - galaxyX;
					float y = travelNodes[0].StarSystem.Y - galaxyY;
					travelNodes[0].Length = (float)Math.Sqrt((x * x) + (y * y));
					travelNodes[0].Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
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

		public void ColonizePlanet(Ship whichShip)
		{
			//This assumes that whichShip is not null, adjacentSystem is not null, and everything has already been validated (i.e. ship has correct colony pod)
			ships[whichShip]--;
			if (ships[whichShip] == 0)
			{
				//only one ship, so remove the entry for it
				ships.Remove(whichShip);
				orderedShips.Remove(whichShip);
			}
			adjacentSystem.Planets[0].Colonize(empire);
		}
		#endregion
	}
}
