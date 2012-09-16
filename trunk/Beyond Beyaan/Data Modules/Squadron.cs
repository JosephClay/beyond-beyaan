using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Squadron
	{
		#region Member Variables
		private StarSystem system;

		private Empire empire;
		private List<KeyValuePair<StarSystem, Starlane>> travelNodes;
		private List<ShipInstance> ships;

		private double maxSpeed;
		private double fifthMaxSpeed;
		private double remainingMoves;
		private double lengthTravelled;
		private double remainingAmount;
		private bool finishedMovingThisTurn;
		private List<string> colonizablePlanets;
		private int transportCapacity;
		private Dictionary<Race, int> populationInTransit;
		#endregion

		#region Properties
		public StarSystem System
		{
			get { return system; }
		}

		public Point FleetLocation
		{
			get;
			private set;
		}

		public Empire Empire
		{
			get { return empire; }
			set { empire = value; }
		}

		public List<KeyValuePair<StarSystem, Starlane>> TravelNodes
		{
			get { return travelNodes; }
			set { travelNodes = value; }
		}

		public double LengthTravelled
		{
			get { return lengthTravelled; }
		}

		public List<string> ColonizablePlanets
		{
			get { return colonizablePlanets; }
		}

		public int TransportCapacity
		{
			get { return transportCapacity; }
		}

		public int TotalPeopleInTransit
		{
			get
			{
				int amount = 0;
				foreach (KeyValuePair<Race, int> race in populationInTransit)
				{
					amount += race.Value;
				}
				return amount;
			}
		}

		public Dictionary<Race, int> PopulationInTransit
		{
			get { return populationInTransit; }
		}

		public int ETA
		{
			get { return maxSpeed <= 0 ? -1 : (int)((remainingAmount / maxSpeed) + ((remainingAmount % maxSpeed > 0) ? 1 : 0)); }
		}

		public double RemainingMovement
		{
			get { return remainingAmount; }
			set { remainingAmount = value; }
		}

		public double MaxSpeed
		{
			get { return maxSpeed; }
		}

		public double TravelSpeed
		{
			get;
			set;
		}

		public int Length
		{
			get;
			private set;
		}

		public float Angle
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<ShipInstance> Ships
		{
			get
			{
				return ships;
			}
		}
		#endregion

		#region Constructors
		public Squadron(StarSystem system)
		{
			ships = new List<ShipInstance>();
			remainingMoves = maxSpeed;
			this.system = system;
			finishedMovingThisTurn = false;
			populationInTransit = new Dictionary<Race, int>();
		}
		public Squadron(Squadron squadronToCopy)
		{
			ships = new List<ShipInstance>();
			populationInTransit = new Dictionary<Race, int>();
			system = squadronToCopy.system;
			AddShipsThemselves(squadronToCopy.Ships);
			Name = squadronToCopy.Name;
		}
		#endregion

		#region Functions
		//Updates speed, radar range, and any other special attributes
		private void UpdateData()
		{
			maxSpeed = float.MaxValue;
			foreach (ShipInstance ship in ships)
			{
				float speed = ship.GetGalaxySpeed();
				if (speed < maxSpeed)
				{
					maxSpeed = speed;
				}
			}

			remainingMoves = maxSpeed;
			fifthMaxSpeed = maxSpeed / 5.0;

			colonizablePlanets = new List<string>();
			transportCapacity = 0;
			foreach (ShipInstance ship in ships)
			{
				foreach (EquipmentInstance equipment in ship.Equipments)
				{
					if (equipment.EquipmentType == EquipmentType.SPECIAL)
					{
						if (equipment.ItemType.AttributeValues.ContainsKey("colonizes"))
						{
							string value = (string)equipment.ItemType.AttributeValues["colonizes"];
							string[] planets = value.Split(new[] { ',' });
							foreach (string planet in planets)
							{
								if (!colonizablePlanets.Contains(planet))
								{
									colonizablePlanets.Add(planet);
								}
							}
						}
						if (equipment.ItemType.AttributeValues.ContainsKey("transferCapacity"))
						{
							transportCapacity += (int)equipment.ItemType.AttributeValues["transferCapacity"];
						}
					}
				}
			}

			SetLocation();
		}

		private void SetLocation()
		{
			if (travelNodes == null)
			{
				FleetLocation = new Point(system.X * 32 + system.Type.Width + 16, system.Y * 32 + 16);
			}
			else
			{
				if (lengthTravelled == 0)
				{
					FleetLocation = new Point(system.X * 32 - 16, system.Y * 32 + 16);
				}
				else
				{
					int x;
					int y;
					if (travelNodes[1].Value.SystemA == system)
					{
						x = (int)((Math.Cos(travelNodes[1].Value.Angle * (Math.PI / 180)) * lengthTravelled) + (system.X * 32 + (system.Type.Width / 2.0f)));
						y = (int)((Math.Sin(travelNodes[1].Value.Angle * (Math.PI / 180)) * lengthTravelled) + (system.Y * 32 + (system.Type.Height / 2.0f)));
						FleetLocation = new Point(x, y);
					}
					else
					{
						x = (int)((Math.Cos(travelNodes[1].Value.Angle * (Math.PI / 180) + Math.PI) * lengthTravelled) + (system.X * 32 + (system.Type.Width / 2.0f)));
						y = (int)((Math.Sin(travelNodes[1].Value.Angle * (Math.PI / 180) + Math.PI) * lengthTravelled) + (system.Y * 32 + (system.Type.Height / 2.0f)));
						FleetLocation = new Point(x, y);
					}

					x = (travelNodes[1].Key.X * 32 + travelNodes[1].Key.Type.Width / 2) - FleetLocation.X;
					y = (travelNodes[1].Key.Y * 32 + travelNodes[1].Key.Type.Height / 2) - FleetLocation.Y;
					Length = (int)Math.Sqrt((x * x) + (y * y));
					Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
				}
			}
		}

		public void SetPath(List<KeyValuePair<StarSystem, Starlane>> path, double pathLength)
		{
			if (path != null)
			{
				travelNodes = new List<KeyValuePair<StarSystem, Starlane>>(path);
				remainingAmount = pathLength;

				SetLocation();

				int x = (travelNodes[1].Key.X * 32 + travelNodes[1].Key.Type.Width / 2) - FleetLocation.X;
				int y = (travelNodes[1].Key.Y * 32 + travelNodes[1].Key.Type.Height / 2) - FleetLocation.Y;
				Length = (int)Math.Sqrt((x * x) + (y * y));
				Angle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
			}
		}

		public void RefreshPath()
		{
			if (travelNodes == null)
			{
				return;
			}
			remainingAmount = Utility.CalculatePathCost(travelNodes);
		}

		public void AddShipFromDesign(ShipDesign shipToAdd)
		{
			ships.Add(new ShipInstance(shipToAdd, empire));
			UpdateData();
		}

		public void AddShipsFromDesign(List<ShipDesign> shipsToAdd)
		{
			foreach (ShipDesign ship in shipsToAdd)
			{
				ships.Add(new ShipInstance(ship, empire));
			}
			UpdateData();
		}

		public void AddShipItself(ShipInstance shipToAdd)
		{
			ships.Add(shipToAdd);
			UpdateData();
		}

		public void AddShipsThemselves(List<ShipInstance> shipsToAdd)
		{
			foreach (ShipInstance ship in shipsToAdd)
			{
				ships.Add(ship);
			}
			UpdateData();
		}

		public void SubtractShip(ShipInstance shipToRemove)
		{
			if (ships.Contains(shipToRemove))
			{
				ships.Remove(shipToRemove);
			}
			UpdateData();
		}

		public void SubtractShips(List<ShipInstance> shipsToRemove)
		{
			foreach (ShipInstance ship in shipsToRemove)
			{
				if (ships.Contains(ship))
				{
					ships.Remove(ship);
				}
			}
			UpdateData();
		}

		public void AddPeople(Race race, int amount)
		{
			if (!populationInTransit.ContainsKey(race))
			{
				populationInTransit.Add(race, amount);
			}
			else
			{
				populationInTransit[race] += amount;
			}
		}

		public void SubtractPeople(Race race, int amount)
		{
			populationInTransit[race] -= amount;
			if (populationInTransit[race] == 0)
			{
				populationInTransit.Remove(race);
			}
		}

		public void ClearShips()
		{
			ships.Clear();
		}

		/*public void ClearEmptyShips()
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
			foreach (TransportShip ship in transportShips)
			{
				if (ship.amount <= 0)
				{
					transportsToRemove.Add(ship);
				}
			}
			foreach (TransportShip ship in transportsToRemove)
			{
				transportShips.Remove(ship);
			}
			UpdateSpeed();
		}*/

		public void ResetMove()
		{
			finishedMovingThisTurn = false;
			remainingMoves = maxSpeed;
		}

		public bool Move()
		{
			if (!finishedMovingThisTurn)
			{
				if (remainingMoves > 0 && travelNodes != null)
				{
					remainingMoves -= fifthMaxSpeed;
					if (remainingMoves < 0)
					{
						remainingMoves = 0;
					}
					lengthTravelled += fifthMaxSpeed;

					while (lengthTravelled >= travelNodes[1].Value.Length)
					{
						system = travelNodes[1].Key;
						lengthTravelled -= travelNodes[1].Value.Length;
						
						//To-do: add checks for conflict

						travelNodes.RemoveAt(0);
						if (travelNodes.Count == 1) //reached destination
						{
							travelNodes = null;
							finishedMovingThisTurn = true;
							lengthTravelled = 0;
							SetLocation();
							return false;
						}
					}
					SetLocation();
					return remainingMoves > 0 && travelNodes != null;
				}
				finishedMovingThisTurn = true;
			}
			return false;
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (ShipInstance ship in ships)
			{
				amount += ship.Maintenance;
			}
			return amount;
		}
		#endregion
	}
}
