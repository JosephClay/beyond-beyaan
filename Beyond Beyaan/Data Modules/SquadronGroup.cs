using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class SquadronGroup
	{
		#region Member Variables
		private List<Squadron> squadrons;
		//private List<Ship> ships; //Used for UI component, since Fleet stores a dictionary of ships, which means no iterator

		private List<Squadron> selectedSquadrons;
		//private Squadron squadronToSplit;
		private StarSystem currentlyAdjacentSystem;
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

		private int length;
		private float angle;

		private int tentativeLength;
		private float tentativeAngle;

		private double lengthTravelled;
		#endregion

		#region Properties
		public List<Squadron> SelectedSquadron
		{
			get { return selectedSquadrons; }
		}
		/*public Squadron SquadronToSplit
		{
			get { return squadronToSplit; }
		}*/

		public List<Squadron> Squadrons
		{
			get { return squadrons; }
		}

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

		public int Length
		{
			get { return length; }
		}

		public float Angle
		{
			get { return angle; }
		}

		public int TentativeLength
		{
			get { return tentativeLength; }
		}

		public float TentativeAngle
		{
			get { return tentativeAngle; }
		}

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
			selectedSquadrons = new List<Squadron>();
			this.squadrons = squadrons;
			this.currentlyAdjacentSystem = adjacentSystem;
			if (squadrons != null && squadrons.Count > 0)
			{
				SelectSquadron(0, currentEmpire, false, false);
			}
		}
		#endregion

		#region Functions
		public void SelectSquadron(int whichSquadron, Empire currentEmpire, bool ctrlDown, bool shiftDown)
		{
			if (whichSquadron < squadrons.Count)
			{
				if (!ctrlDown && !shiftDown)
				{
					selectedSquadrons.Clear();
					selectedSquadrons.Add(squadrons[whichSquadron]);
					lastIndex = whichSquadron;
				}
				else if (ctrlDown && !shiftDown)
				{
					if (squadrons[whichSquadron].Empire == currentEmpire)
					{
						if (selectedSquadrons.Contains(squadrons[whichSquadron]))
						{
							selectedSquadrons.Remove(squadrons[whichSquadron]);
						}
						else
						{
							selectedSquadrons.Add(squadrons[whichSquadron]);
						}
						lastIndex = whichSquadron;
					}
				}
				else if (!ctrlDown && shiftDown)
				{
					List<int> indexes = new List<int>();
					foreach (Squadron selectedSquadron in selectedSquadrons)
					{
						indexes.Add(squadrons.IndexOf(selectedSquadron));
					}
					indexes.Sort();
					if (lastIndex < whichSquadron)
					{
						for (int i = lastIndex + 1; i <= whichSquadron; i++)
						{
							if (!selectedSquadrons.Contains(squadrons[i]))
							{
								selectedSquadrons.Add(squadrons[i]);
							}
							else
							{
								selectedSquadrons.Remove(squadrons[i]);
							}
						}
					}
					else if (lastIndex > whichSquadron)
					{
						for (int i = whichSquadron; i < lastIndex; i++)
						{
							if (!selectedSquadrons.Contains(squadrons[i]))
							{
								selectedSquadrons.Add(squadrons[i]);
							}
							else
							{
								selectedSquadrons.Remove(squadrons[i]);
							}
						}
					}
					lastIndex = whichSquadron;
				}
			}
			if (selectedSquadrons.Count > 0)
			{
				List<KeyValuePair<StarSystem, Starlane>> firstSquadronNodes = selectedSquadrons[0].TravelNodes;
				lengthTravelled = selectedSquadrons[0].LengthTravelled;
				Empire = SelectedSquadron[0].Empire;
				origin = SelectedSquadron[0].System;
				bool same = true;

				for (int i = 1; i < selectedSquadrons.Count; i++)
				{
					if ((firstSquadronNodes != null && selectedSquadrons[i].TravelNodes == null) || (firstSquadronNodes == null && selectedSquadrons[i].TravelNodes != null) || lengthTravelled != selectedSquadrons[i].LengthTravelled || origin != selectedSquadrons[i].System)
					{
						same = false;
						break;
					}
					if (firstSquadronNodes != null)
					{
						if (firstSquadronNodes.Count != selectedSquadrons[i].TravelNodes.Count)
						{
							same = false;
							break;
						}
						for (int j = 0; j < firstSquadronNodes.Count; j++)
						{
							if (firstSquadronNodes[j].Key != selectedSquadrons[i].TravelNodes[j].Key ||
								firstSquadronNodes[j].Value != selectedSquadrons[i].TravelNodes[j].Value)
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

					angle = selectedSquadrons[0].Angle;
					length = selectedSquadrons[0].Length;

					for (int i = 0; i < selectedSquadrons.Count; i++)
					{
						if (travelSpeed > selectedSquadrons[i].TravelSpeed)
						{
							travelSpeed = selectedSquadrons[i].TravelSpeed;
						}
					}
				}
				else
				{
					lengthTravelled = -1;
					origin = null;
				}
				maxSpeed = double.MaxValue;
				for (int i = 0; i < selectedSquadrons.Count; i++)
				{
					if (maxSpeed > selectedSquadrons[i].MaxSpeed)
					{
						maxSpeed = selectedSquadrons[i].MaxSpeed;
					}
					if (Empire != selectedSquadrons[i].Empire)
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

				int x = (tentativeNodes[1].Key.X * 32 + tentativeNodes[1].Key.Type.Width / 2) - selectedSquadrons[0].FleetLocation.X;
				int y = (tentativeNodes[1].Key.Y * 32 + tentativeNodes[1].Key.Type.Height / 2) - selectedSquadrons[0].FleetLocation.Y;
				tentativeLength = (int)Math.Sqrt((x * x) + (y * y));
				tentativeAngle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
				return;
			}
			if (tentativeNodes != null && ((destinationSystem == tentativeNodes[tentativeNodes.Count - 1].Key && !direct && tentativeNodes.Count > 2) || (direct && tentativeNodes.Count == 2)))
			{
				// Existing tentative path
				return;
			}

			StarSystem systemToStartFrom = origin == null ? travelNodes[0].Key : origin;
			if (lengthTravelled > 0)
			{
				systemToStartFrom = travelNodes[1].Key;
			}
			List<KeyValuePair<StarSystem, Starlane>> path = galaxy.GetPath(systemToStartFrom, destinationSystem, direct, lengthTravelled > 0 ? origin : null, currentEmpire, out tentativeAmount);
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
				int x = (tentativeNodes[1].Key.X * 32 + tentativeNodes[1].Key.Type.Width / 2) - selectedSquadrons[0].FleetLocation.X;
				int y = (tentativeNodes[1].Key.Y * 32 + tentativeNodes[1].Key.Type.Height / 2) - selectedSquadrons[0].FleetLocation.Y;
				tentativeLength = (int)Math.Sqrt((x * x) + (y * y));
				tentativeAngle = (float)(Math.Atan2(y, x) * (180 / Math.PI)) + 180;
			}
		}

		public void ConfirmPath()
		{
			foreach (Squadron squadron in selectedSquadrons)
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
