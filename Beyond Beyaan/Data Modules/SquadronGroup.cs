using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class SquadronGroup
	{
		#region Member Variables

		//private List<Ship> ships; //Used for UI component, since Fleet stores a dictionary of ships, which means no iterator

		//private Squadron squadronToSplit;
		//private StarSystem currentlyAdjacentSystem;
		private StarSystem origin;

		//private int fleetIndex;
		//private int shipIndex;
		private int lastIndex;

		private List<KeyValuePair<StarSystem, Starlane>> travelNodes;
		private List<KeyValuePair<StarSystem, Starlane>> tentativeNodes;

		private double tentativeAmount;
		//private double remainingAmount;

		private double maxSpeed;
		private double travelSpeed;

		private double lengthTravelled;
		#endregion

		#region Properties

		public List<Squadron> SelectedSquadron { get; private set; }

		/*public Squadron SquadronToSplit
		{
			get { return squadronToSplit; }
		}*/

		public List<Squadron> Squadrons { get; private set; }

		public List<KeyValuePair<StarSystem, Starlane>> TravelNodes
		{
			get { return travelNodes; }
			set { travelNodes = value; }
		}

		public List<KeyValuePair<StarSystem, Starlane>> TentativeNodes
		{
			get { return tentativeNodes; }
			set { tentativeNodes = value; }
		}

		public int Length { get; private set; }

		public float Angle { get; private set; }

		public int TentativeLength { get; private set; }

		public float TentativeAngle { get; private set; }

		public int TentativeETA
		{
			get { return maxSpeed <= 0 ? -1 : (int)((tentativeAmount / maxSpeed) + ((tentativeAmount % maxSpeed > 0) ? 1 : 0)); }
		}

		public Empire Empire
		{
			get;
			private set;
		}

		/*public int FleetIndex
		{
			get { return fleetIndex; }
			set { fleetIndex = value; }
		}*/

		/*public int ShipIndex
		{
			get { return shipIndex; }
			set 
			{ 
				shipIndex = value;
				if (fleetToSplit.Ships.Count > 8)
				{
					if (shipIndex > fleetToSplit.Ships.Count - 8)
					{
						shipIndex = fleetToSplit.Ships.Count - 8;
					}
				}
				else
				{
					shipIndex = 0;
				}
				int maxShip = fleetToSplit.Ships.Count > 8 ? 8 : fleetToSplit.Ships.Count;

				ships = new List<Ship>();
				int i = 0;
				foreach (KeyValuePair<Ship, int> ship in fleetToSplit.Ships)
				{
					if (i >= shipIndex && i < shipIndex + maxShip)
					{
						ships.Add(ship.Key);
					}
					i++;
					if (i >= shipIndex + maxShip)
					{
						break;
					}
				}
			}
		}*/
		#endregion

		#region Constructors
		public SquadronGroup(List<Squadron> squadrons, Empire currentEmpire, StarSystem adjacentSystem)
		{
			SelectedSquadron = new List<Squadron>();
			this.Squadrons = squadrons;
			//this.currentlyAdjacentSystem = adjacentSystem;
			if (squadrons != null && squadrons.Count > 0)
			{
				SelectSquadron(0, currentEmpire, false, false);
			}
		}
		#endregion

		#region Functions
		public void SelectSquadron(int whichSquadron, Empire currentEmpire, bool ctrlDown, bool shiftDown)
		{
			if (whichSquadron < Squadrons.Count)
			{
				if (!ctrlDown && !shiftDown)
				{
					SelectedSquadron.Clear();
					SelectedSquadron.Add(Squadrons[whichSquadron]);
					lastIndex = whichSquadron;
				}
				else if (ctrlDown && !shiftDown)
				{
					if (Squadrons[whichSquadron].Empire == currentEmpire)
					{
						if (SelectedSquadron.Contains(Squadrons[whichSquadron]))
						{
							SelectedSquadron.Remove(Squadrons[whichSquadron]);
						}
						else
						{
							SelectedSquadron.Add(Squadrons[whichSquadron]);
						}
						lastIndex = whichSquadron;
					}
				}
				else if (!ctrlDown)
				{
					List<int> indexes = new List<int>();
					foreach (Squadron selectedSquadron in SelectedSquadron)
					{
						indexes.Add(Squadrons.IndexOf(selectedSquadron));
					}
					indexes.Sort();
					if (lastIndex < whichSquadron)
					{
						for (int i = lastIndex + 1; i <= whichSquadron; i++)
						{
							if (!SelectedSquadron.Contains(Squadrons[i]))
							{
								SelectedSquadron.Add(Squadrons[i]);
							}
							else
							{
								SelectedSquadron.Remove(Squadrons[i]);
							}
						}
					}
					else if (lastIndex > whichSquadron)
					{
						for (int i = whichSquadron; i < lastIndex; i++)
						{
							if (!SelectedSquadron.Contains(Squadrons[i]))
							{
								SelectedSquadron.Add(Squadrons[i]);
							}
							else
							{
								SelectedSquadron.Remove(Squadrons[i]);
							}
						}
					}
					lastIndex = whichSquadron;
				}
			}
			if (SelectedSquadron.Count > 0)
			{
				List<KeyValuePair<StarSystem, Starlane>> firstSquadronNodes = SelectedSquadron[0].TravelNodes;
				lengthTravelled = SelectedSquadron[0].LengthTravelled;
				Empire = SelectedSquadron[0].Empire;
				origin = SelectedSquadron[0].System;
				bool same = true;

				for (int i = 1; i < SelectedSquadron.Count; i++)
				{
					if ((firstSquadronNodes != null && SelectedSquadron[i].TravelNodes == null) || (firstSquadronNodes == null && SelectedSquadron[i].TravelNodes != null) || lengthTravelled != SelectedSquadron[i].LengthTravelled || origin != SelectedSquadron[i].System)
					{
						same = false;
						break;
					}
					if (firstSquadronNodes != null)
					{
						if (firstSquadronNodes.Count != SelectedSquadron[i].TravelNodes.Count)
						{
							same = false;
							break;
						}
						for (int j = 0; j < firstSquadronNodes.Count; j++)
						{
							if (firstSquadronNodes[j].Key != SelectedSquadron[i].TravelNodes[j].Key ||
								firstSquadronNodes[j].Value != SelectedSquadron[i].TravelNodes[j].Value)
							{
								same = false;
								break;
							}
						}
						if (!same)
						{
							break;
						}
					}
				}
				if (same)
				{
					travelNodes = firstSquadronNodes;
					travelSpeed = double.MaxValue;

					Angle = SelectedSquadron[0].Angle;
					Length = SelectedSquadron[0].Length;

					for (int i = 0; i < SelectedSquadron.Count; i++)
					{
						if (travelSpeed > SelectedSquadron[i].TravelSpeed)
						{
							travelSpeed = SelectedSquadron[i].TravelSpeed;
						}
					}
				}
				else
				{
					lengthTravelled = -1;
					origin = null;
				}
				maxSpeed = double.MaxValue;
				for (int i = 0; i < SelectedSquadron.Count; i++)
				{
					if (maxSpeed > SelectedSquadron[i].MaxSpeed)
					{
						maxSpeed = SelectedSquadron[i].MaxSpeed;
					}
					if (Empire != SelectedSquadron[i].Empire)
					{
						Empire = null;
					}
				}
			}
		}

		public void SetTentativePath(StarSystem destinationSystem, Galaxy galaxy, bool direct, Empire currentEmpire)
		{
			if (destinationSystem == null)
			{
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}
			/*if (travelNodes == null && destinationSystem == adjacentSystem)
			{
				//if destination is same as origin, don't bother.
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}*/
			if (travelNodes != null && ((travelNodes[travelNodes.Count - 1].Key == destinationSystem && !direct) || (direct && travelNodes.Count == 2 && lengthTravelled == 0)))
			{
				//Same path as current path
				tentativeNodes = new List<KeyValuePair<StarSystem, Starlane>>(travelNodes.ToArray());
				tentativeAmount = Utility.CalculatePathCost(tentativeNodes);

				int x = (tentativeNodes[1].Key.X * 32 + tentativeNodes[1].Key.Type.Width / 2) - SelectedSquadron[0].FleetLocation.X;
				int y = (tentativeNodes[1].Key.Y * 32 + tentativeNodes[1].Key.Type.Height / 2) - SelectedSquadron[0].FleetLocation.Y;
				TentativeLength = (int)Math.Sqrt((x * x) + (y * y));
				TentativeAngle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
				return;
			}
			if (tentativeNodes != null && ((destinationSystem == tentativeNodes[tentativeNodes.Count - 1].Key && !direct && tentativeNodes.Count > 2) || (direct && tentativeNodes.Count == 2)))
			{
				// Existing tentative path
				return;
			}

			/*StarSystem systemToStartFrom = origin == null ? travelNodes[0].Key : origin;
			if (lengthTravelled > 0)
			{
				systemToStartFrom = travelNodes[1].Key;
			}*/
			List<KeyValuePair<StarSystem, Starlane>> path = null; // galaxy.GetPath(systemToStartFrom, destinationSystem, direct, lengthTravelled > 0 ? origin : null, currentEmpire, out tentativeAmount);
			if (path == null)
			{
				tentativeAmount = 0;
				tentativeNodes = null;
				return;
			}

			tentativeNodes = path;
			if (tentativeNodes.Count == 0)
			{
				tentativeNodes = null;
				tentativeAmount = 0;
			}
			else
			{
				int x = (tentativeNodes[1].Key.X * 32 + tentativeNodes[1].Key.Type.Width / 2) - SelectedSquadron[0].FleetLocation.X;
				int y = (tentativeNodes[1].Key.Y * 32 + tentativeNodes[1].Key.Type.Height / 2) - SelectedSquadron[0].FleetLocation.Y;
				TentativeLength = (int)Math.Sqrt((x * x) + (y * y));
				TentativeAngle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
			}
		}

		public void ConfirmPath()
		{
			foreach (Squadron squadron in SelectedSquadron)
			{
				squadron.SetPath(tentativeNodes, tentativeAmount);
			}
		}

		/*public void SplitFleet(Empire empire, Dictionary<Race, int> population)
		{
			Squadron fleet = new Squadron(squadronToSplit.System);
			fleet.Empire = squadronToSplit.Empire;
			fleet.TravelNodes = squadronToSplit.TravelNodes;
			fleet.RemainingMovement = squadronToSplit.RemainingMovement;
			fleet.Angle = squadronToSplit.Angle;
			fleet.Length = squadronToSplit.Length;

			foreach (KeyValuePair<Race, int> race in population)
			{
				fleet.AddPeople(race.Key, race.Value);
				selectedSquadron.SubtractPeople(race.Key, race.Value);
			}

			fleet.AddShipsThemselves(squadronToSplit.Ships);
			selectedSquadron.SubtractShips(squadronToSplit.Ships);
			if (selectedSquadron.Ships.Count == 0)
			{
				squadrons.Remove(selectedSquadron);
				empire.FleetManager.RemoveFleet(selectedSquadron);
			}
			if (fleet.Ships.Count > 0)
			{
				squadrons.Add(fleet);
				empire.FleetManager.AddFleet(fleet);
			}
			fleet.RefreshPath();
		}*/

		/*public List<Ship> GetShipsForDisplay()
		{
			return ships;
		}*/
		#endregion
	}
}
