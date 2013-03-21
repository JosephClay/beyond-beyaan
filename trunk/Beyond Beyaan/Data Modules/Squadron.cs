using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Squadron
	{
		#region Member Variables

		private Empire empire;
		private List<KeyValuePair<StarSystem, Starlane>> travelNodes;

		private double fifthMaxSpeed;
		private double remainingMoves;
		private double remainingAmount;
		private bool finishedMovingThisTurn;

		#endregion

		#region Properties

		public StarSystem System { get; private set; }

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

		public double LengthTravelled { get; private set; }

		public List<string> ColonizablePlanets { get; private set; }

		public int TransportCapacity { get; private set; }

		public int TotalPeopleInTransit
		{
			get
			{
				int amount = 0;
				foreach (KeyValuePair<Race, int> race in PopulationInTransit)
				{
					amount += race.Value;
				}
				return amount;
			}
		}

		public Dictionary<Race, int> PopulationInTransit { get; private set; }

		public int ETA
		{
			get { return MaxSpeed <= 0 ? -1 : (int)((remainingAmount / MaxSpeed) + ((remainingAmount % MaxSpeed > 0) ? 1 : 0)); }
		}

		public double RemainingMovement
		{
			get { return remainingAmount; }
			set { remainingAmount = value; }
		}

		public double MaxSpeed { get; private set; }

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

		public List<ShipInstance> Ships { get; private set; }

		#endregion

		#region Constructors
		public Squadron(StarSystem system)
		{
			Ships = new List<ShipInstance>();
			remainingMoves = MaxSpeed;
			this.System = system;
			finishedMovingThisTurn = false;
			PopulationInTransit = new Dictionary<Race, int>();
		}
		public Squadron(Squadron squadronToCopy)
		{
			Ships = new List<ShipInstance>();
			PopulationInTransit = new Dictionary<Race, int>();
			System = squadronToCopy.System;
			AddShipsThemselves(squadronToCopy.Ships);
			Name = squadronToCopy.Name;
		}
		#endregion

		#region Functions
		//Updates speed, radar range, and any other special attributes
		private void UpdateData()
		{
			MaxSpeed = float.MaxValue;
			foreach (ShipInstance ship in Ships)
			{
				float speed = ship.GetGalaxySpeed();
				if (speed < MaxSpeed)
				{
					MaxSpeed = speed;
				}
			}

			remainingMoves = MaxSpeed;
			fifthMaxSpeed = MaxSpeed / 5.0;

			ColonizablePlanets = new List<string>();
			TransportCapacity = 0;
			/*foreach (ShipInstance ship in ships)
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
			}*/

			SetLocation();
		}

		private void SetLocation()
		{
			if (travelNodes == null)
			{
				FleetLocation = new Point(System.X * 32 + System.Type.Width + 16, System.Y * 32 + 16);
			}
			else
			{
				if (LengthTravelled == 0)
				{
					FleetLocation = new Point(System.X * 32 - 16, System.Y * 32 + 16);
				}
				else
				{
					int x;
					int y;
					if (travelNodes[1].Value.SystemA == System)
					{
						x = (int)((Math.Cos(travelNodes[1].Value.Angle * (Math.PI / 180)) * LengthTravelled) + (System.X * 32 + (System.Type.Width / 2.0f)));
						y = (int)((Math.Sin(travelNodes[1].Value.Angle * (Math.PI / 180)) * LengthTravelled) + (System.Y * 32 + (System.Type.Height / 2.0f)));
						FleetLocation = new Point(x, y);
					}
					else
					{
						x = (int)((Math.Cos(travelNodes[1].Value.Angle * (Math.PI / 180) + Math.PI) * LengthTravelled) + (System.X * 32 + (System.Type.Width / 2.0f)));
						y = (int)((Math.Sin(travelNodes[1].Value.Angle * (Math.PI / 180) + Math.PI) * LengthTravelled) + (System.Y * 32 + (System.Type.Height / 2.0f)));
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
			Ships.Add(new ShipInstance(shipToAdd, empire));
			UpdateData();
		}

		public void AddShipsFromDesign(List<ShipDesign> shipsToAdd)
		{
			foreach (ShipDesign ship in shipsToAdd)
			{
				Ships.Add(new ShipInstance(ship, empire));
			}
			UpdateData();
		}

		public void AddShipItself(ShipInstance shipToAdd)
		{
			Ships.Add(shipToAdd);
			UpdateData();
		}

		public void AddShipsThemselves(List<ShipInstance> shipsToAdd)
		{
			foreach (ShipInstance ship in shipsToAdd)
			{
				Ships.Add(ship);
			}
			UpdateData();
		}

		public void SubtractShip(ShipInstance shipToRemove)
		{
			if (Ships.Contains(shipToRemove))
			{
				Ships.Remove(shipToRemove);
			}
			UpdateData();
		}

		public void SubtractShips(List<ShipInstance> shipsToRemove)
		{
			foreach (ShipInstance ship in shipsToRemove)
			{
				if (Ships.Contains(ship))
				{
					Ships.Remove(ship);
				}
			}
			UpdateData();
		}

		public void AddPeople(Race race, int amount)
		{
			if (!PopulationInTransit.ContainsKey(race))
			{
				PopulationInTransit.Add(race, amount);
			}
			else
			{
				PopulationInTransit[race] += amount;
			}
		}

		public void SubtractPeople(Race race, int amount)
		{
			PopulationInTransit[race] -= amount;
			if (PopulationInTransit[race] == 0)
			{
				PopulationInTransit.Remove(race);
			}
		}

		public void ClearShips()
		{
			Ships.Clear();
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
			remainingMoves = MaxSpeed;
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
					LengthTravelled += fifthMaxSpeed;

					while (LengthTravelled >= travelNodes[1].Value.Length)
					{
						System = travelNodes[1].Key;
						LengthTravelled -= travelNodes[1].Value.Length;
						
						//To-do: add checks for conflict

						travelNodes.RemoveAt(0);
						if (travelNodes.Count == 1) //reached destination
						{
							travelNodes = null;
							finishedMovingThisTurn = true;
							LengthTravelled = 0;
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

		public Dictionary<Resource, float> GetExpenses()
		{
			// TODO: Tally up expenses
			/*float amount = 0;
			foreach (ShipInstance ship in ships)
			{
				amount += ship.Maintenance;
			}*/
			return new Dictionary<Resource, float>();
		}
		#endregion
	}
}
