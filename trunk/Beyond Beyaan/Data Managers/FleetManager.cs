using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Beyond_Beyaan
{
	public class FleetManager
	{
		#region Variables

		private List<Fleet> _fleets;
		private Empire _empire;
		private int _currentShipDesignID;
		#endregion

		#region Properties
		public Ship LastShipDesign { get; set; }

		public List<Ship> CurrentDesigns { get; private set; }

		public List<Ship> ObsoleteDesigns { get; private set; }

		#endregion

		public FleetManager(Empire empire)
		{
			this._empire = empire;
			_currentShipDesignID = 0;
			_fleets = new List<Fleet>();
			CurrentDesigns = new List<Ship>();
			ObsoleteDesigns = new List<Ship>();
		}

		public void SetupStarterFleet(StarSystem homeSystem)
		{
			Technology retroEngine = null;
			Technology titaniumArmor = null;
			foreach (var tech in _empire.TechnologyManager.ResearchedPropulsionTechs)
			{
				if (tech.Speed == 1)
				{
					retroEngine = tech;
					break;
				}
			}
			foreach (var tech in _empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.Armor == Technology.TITANIUM_ARMOR)
				{
					titaniumArmor = tech;
					break;
				}
			}

			Ship scout = new Ship();
			scout.Name = "Scout";
			scout.Owner = _empire;
			scout.Size = Ship.SMALL;
			scout.WhichStyle = 0;
			scout.Engine = retroEngine;
			scout.Armor = titaniumArmor;
			foreach (var tech in _empire.TechnologyManager.ResearchedConstructionTechs)
			{
				if (tech.ReserveFuelTanks)
				{
					scout.Specials.Add(tech);
					break;
				}
			}
			scout.DesignID = _currentShipDesignID;
			CurrentDesigns.Add(scout);
			_currentShipDesignID++;

			Ship colonyShip = new Ship();
			colonyShip.Name = "Colony Ship";
			colonyShip.Owner = _empire;
			colonyShip.Size = Ship.LARGE;
			colonyShip.WhichStyle = 0;
			colonyShip.Engine = retroEngine;
			colonyShip.Armor = titaniumArmor;
			foreach (var tech in _empire.TechnologyManager.ResearchedPlanetologyTechs)
			{
				if (tech.Colony == Technology.STANDARD_COLONY)
				{
					colonyShip.Specials.Add(tech);
					break;
				}
			}
			colonyShip.DesignID = _currentShipDesignID;
			CurrentDesigns.Add(colonyShip);
			_currentShipDesignID++;

			LastShipDesign = new Ship(scout); //Make a copy so we don't accidentally modify the original ship

			Fleet starterFleet = new Fleet();
			starterFleet.GalaxyX = homeSystem.X;
			starterFleet.GalaxyY = homeSystem.Y;
			starterFleet.AdjacentSystem = homeSystem;
			starterFleet.Empire = _empire;
			starterFleet.AddShips(CurrentDesigns[0], 2);
			starterFleet.AddShips(CurrentDesigns[1], 1);
			_fleets.Add(starterFleet);
		}

		public List<Fleet> GetFleets()
		{
			return _fleets;
		}

		public Fleet[] ReturnFleetAtPoint(int x, int y)
		{
			List<Fleet> listOfFleets = new List<Fleet>();
			foreach (Fleet fleet in _fleets)
			{
				if (fleet.AdjacentSystem != null)
				{
					if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
					{
						if (x >= fleet.AdjacentSystem.X - 48 && x < fleet.AdjacentSystem.X - 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
					else
					{
						if (x >= fleet.AdjacentSystem.X + 16 && x < fleet.AdjacentSystem.X + 48 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
						{
							listOfFleets.Add(fleet);
						}
					}
				}
				else
				{
					if (x >= fleet.GalaxyX - 16 && x < fleet.GalaxyX + 16 && y >= fleet.GalaxyY - 16 && y < fleet.GalaxyY + 16)
					{
						listOfFleets.Add(fleet);
					}
				}
			}
			return listOfFleets.ToArray();
		}

		public void AddFleet(Fleet fleet)
		{
			_fleets.Add(fleet);
		}

		public void RemoveFleet(Fleet fleet)
		{
			_fleets.Remove(fleet);
		}

		public void AddShipDesign(Ship newShipDesign)
		{
			Ship ship = new Ship(newShipDesign); //Make a copy so it don't get modified by accident
			ship.DesignID = _currentShipDesignID;
			_currentShipDesignID++;
			CurrentDesigns.Add(ship);
		}

		public void ObsoleteShipDesign(Ship shipToObsolete)
		{
			CurrentDesigns.Remove(shipToObsolete);
			ObsoleteDesigns.Add(shipToObsolete);
		}

		public Ship GetShipWithDesignID(int designID)
		{
			//Iterates through both current and obsolete designs
			foreach (var ship in CurrentDesigns)
			{
				if (ship.DesignID == designID)
				{
					return ship;
				}
			}
			foreach (var ship in ObsoleteDesigns)
			{
				if (ship.DesignID == designID)
				{
					return ship;
				}
			}
			throw new Exception("Ship Design not found: " + designID);
		}

		public bool MoveFleets(float frameDeltaTime)
		{
			bool stillHaveMovement = false;
			//This is called during end of turn processing
			foreach (Fleet fleet in _fleets)
			{
				if (fleet.Move(frameDeltaTime))
				{
					stillHaveMovement = true;
				}
			}
			return stillHaveMovement;
		}

		public void ResetFleetMovements()
		{
			foreach (Fleet fleet in _fleets)
			{
				fleet.ResetMove();
			}
		}

		public void MergeIdleFleets()
		{
			for (int i = 0; i < _fleets.Count; i++)
			{
				if (_fleets[i].TravelNodes == null)
				{
					List<Fleet> fleetsToRemove = new List<Fleet>();
					for (int j = i + 1; j < _fleets.Count; j++)
					{
						if (_fleets[j].TravelNodes == null && _fleets[j].AdjacentSystem == _fleets[i].AdjacentSystem)
						{
							//Merge only fleets of the same type (i.e. ships with ships, transports with transports
							if (_fleets[j].Ships.Count > 0 && _fleets[i].Ships.Count > 0)
							{
								foreach (KeyValuePair<Ship, int> ship in _fleets[j].Ships)
								{
									_fleets[i].AddShips(ship.Key, ship.Value);
								}
								fleetsToRemove.Add(_fleets[j]);
							}
							else if (_fleets[j].TransportShips.Count > 0 && _fleets[i].TransportShips.Count > 0)
							{
								foreach (TransportShip ship in _fleets[j].TransportShips)
								{
									_fleets[i].AddTransport(ship.raceOnShip, ship.amount);
								}
								fleetsToRemove.Add(_fleets[j]);
							}
						}
					}
					foreach (Fleet fleet in fleetsToRemove)
					{
						_fleets.Remove(fleet);
					}
				}
			}
		}
		public void ClearEmptyFleets()
		{
			//Clear out any empty fleets left after colonizing, space combat, etc
			List<Fleet> fleetsToRemove = new List<Fleet>();
			for (int i = 0; i < _fleets.Count; i++)
			{
				if (_fleets[i].Ships.Count == 0 && _fleets[i].TransportShips.Count == 0)
				{
					fleetsToRemove.Add(_fleets[i]);
				}
			}
			foreach (var fleet in fleetsToRemove)
			{
				_fleets.Remove(fleet);
			}
		}

		public float GetExpenses()
		{
			float amount = 0;
			foreach (Fleet fleet in _fleets)
			{
				amount += fleet.GetExpenses();
			}
			return amount;
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("CurrentShipDesigns");
			foreach (var ship in CurrentDesigns)
			{
				ship.Save(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("ObsoleteShipDesigns");
			foreach (var ship in ObsoleteDesigns)
			{
				ship.Save(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Fleets");
			foreach (var fleet in _fleets)
			{
				fleet.Save(writer);
			}
			writer.WriteEndElement();
		}

		public void Load(XElement empire, GameMain gameMain)
		{
			var currentDesigns = empire.Element("CurrentShipDesigns");
			foreach (var currentDesign in currentDesigns.Elements())
			{
				var currentShip = new Ship();
				currentShip.Load(currentDesign, gameMain);
				CurrentDesigns.Add(currentShip);
			}
			var obsoleteDesigns = empire.Element("ObsoleteShipDesigns");
			foreach (var obsoleteDesign in obsoleteDesigns.Elements())
			{
				var obsoleteShip = new Ship();
				obsoleteShip.Load(obsoleteDesign, gameMain);
				ObsoleteDesigns.Add(obsoleteShip);
			}
			var fleets = empire.Element("Fleets");
			foreach (var fleet in fleets.Elements())
			{
				var newFleet = new Fleet();
				newFleet.Load(fleet, this, gameMain);
				_fleets.Add(newFleet);
			}
		}
	}
}
