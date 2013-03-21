using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using GorgonLibrary.Graphics;
using Beyond_Beyaan.Data_Managers;
using System.Globalization;

namespace Beyond_Beyaan.Data_Modules
{
	public class StartingSystem
	{
		public List<StartingPlanet> Planets { get; set; }
		public List<StartingSquadron> Squadrons { get; set; }
		public bool OverrideSystem { get; set; }
		public bool InOrder { get; set; }
		public bool ReplacePlanets { get; set; }

		public StartingSystem()
		{
			Planets = new List<StartingPlanet>();
			Squadrons = new List<StartingSquadron>();
		}
	}
	public class StartingPlanet
	{
		public string PlanetType { get; set; }
		public bool Owned { get; set; }
		public int Population { get; set; }
		public List<string> Regions { get; set; }
		public Dictionary<string, float> Resources { get; set; }
	}
	public class StartingSquadron
	{
		public List<StartingShip> StartingShips { get; set; }
		public string StartingName { get; set; }

		public StartingSquadron()
		{
			StartingShips = new List<StartingShip>();
		}
	}
	public class StartingShip
	{
		public string shipClass;
		public int style;
		public string name;
		public bool addToBlueprints;
		public List<StartingEquipment> equipment;
	}
	public class StartingEquipment
	{
		public string mainItem;
		public string mountItem;
		public List<string> modifiers;
		public List<string> modifiableValues; //Mount count, ammo count, etc
	}
	public enum Expression { PLEASED, NEUTRAL, ANNOYED }
	public class Race
	{
		public Dictionary<string, float> ProductionBonuses { get; private set; }
		public Dictionary<string, float> ConsumptionBonuses { get; private set; }
		public Dictionary<Resource, float> Consumptions { get; private set; }
		public Dictionary<Resource, float> Productions { get; private set; }

		public string RaceName { get; private set; }
		public string RaceDescription { get; private set; }

		public List<Ship> ShipClasses { get; private set; }
		/*public List<int> ShipSizes { get { return shipSizes; } }
		public Dictionary<int, string> ShipSizeLabels { get { return shipSizeLabels; } }
		public Dictionary<int, List<Sprite>> ShipSprites { get { return shipSprites; } }*/

		public List<StartingSystem> StartingSystems { get; private set; }
		public List<StartingShip> StartingShips { get; private set; }

		//public TechnologyList RacialTechnologies { get; private set; }

		private Dictionary<int, List<string>> shipNames;
		private Sprite neutralAvatar;
		private Sprite annoyedAvatar;
		private Sprite pleasedAvatar;
		private Sprite miniAvatar;
		/*private Dictionary<int, List<Sprite>> shipSprites;
		private Dictionary<int, string> shipSizeLabels;
		private List<int> shipSizes;*/
		private Sprite groundUnit;
		private Sprite groundUnitDying;
		private Sprite fleetIcon;
		private Sprite city;
		private Sprite troopShip;

		public bool Initialize(XElement race, string graphicDirectory, ShipScriptManager shipScriptManager, IconManager iconManager, ResourceManager resourceManager, out string reason)
		{
			List<string> errors = new List<string>();
			shipNames = new Dictionary<int, List<string>>();
			ProductionBonuses = new Dictionary<string, float>();
			ConsumptionBonuses = new Dictionary<string, float>();
			Consumptions = new Dictionary<Resource, float>();
			Productions = new Dictionary<Resource, float>();

			XElement attributes = race.Element("Attributes");
			RaceName = attributes.Attribute("name").Value;
			string raceImage = attributes.Attribute("image").Value;
			RaceDescription = attributes.Attribute("description").Value;
			string[] consumptions = attributes.Attribute("consumes").Value.Split(new[] { '|' });
			foreach (string consumption in consumptions)
			{
				string[] values = consumption.Split(new[] { ',' });
				if (values.Length == 2)
				{
                    Consumptions.Add(resourceManager.GetResource(values[0]), float.Parse(values[1], CultureInfo.InvariantCulture));
				}
			}
			string[] productions = attributes.Attribute("produces").Value.Split(new[] { '|' });
			foreach (string production in productions)
			{
				string[] values = production.Split(new[] { ',' });
				if (values.Length == 2)
				{
					Productions.Add(resourceManager.GetResource(values[0]), float.Parse(values[1], CultureInfo.InvariantCulture));
				}
			}
			XElement bonusAttributes = race.Element("ProductionBonuses");
			foreach (XElement element in bonusAttributes.Elements())
			{
				string[] values = element.Attribute("multiplier").Value.ToString().Split(new[] { ',' });
				if (ProductionBonuses.ContainsKey(values[0]))
				{
					reason = "Duplicate " + values[0] + " production bonus definition for " + RaceName;
					return false;
				}
                ProductionBonuses.Add(values[0], float.Parse(values[1], CultureInfo.InvariantCulture));
			}

			bonusAttributes = race.Element("ConsumptionBonuses");
			foreach (XElement element in bonusAttributes.Elements())
			{
				string[] values = element.Attribute("multiplier").Value.ToString().Split(new[] { ',' });
				if (ConsumptionBonuses.ContainsKey(values[0]))
				{
					reason = "Duplicate " + values[0] + " consumption bonus definition for " + RaceName;
					return false;
				}
                ConsumptionBonuses.Add(values[0], float.Parse(values[1], CultureInfo.InvariantCulture));
			}

			XElement shipName = race.Element("ShipNames");
			foreach (XElement element in shipName.Elements())
			{
				int size = int.Parse(element.Attribute("size").Value);
				string name = element.Attribute("name").Value;
				if (!shipNames.ContainsKey(size))
				{
					shipNames.Add(size, new List<string>());
				}
				shipNames[size].Add(name);
			}
			XElement shipClasses = race.Element("ShipClasses");

			//Load in the ship and profile sprites

			string graphic = Path.Combine(graphicDirectory, raceImage + ".png");
			if (!File.Exists(graphic))
			{
				reason = "Graphic file " + graphic + " does not exist";
				return false;
			}
			if (string.IsNullOrEmpty(RaceName))
			{
				reason = "Race name is missing in data file";
				return false;
			}
			if (ImageCache.Images.Contains(RaceName))
			{
				reason = "Duplicate race name: " + RaceName;
				return false;
			}
			Sprite raceGraphic = new Sprite(RaceName, Image.FromFile(graphic));

			int x;
			int y;

			ShipClasses = new List<Ship>();

			foreach (XElement element in shipClasses.Elements())
			{
				Ship newShipClass = new Ship(element, raceGraphic, RaceName, shipScriptManager.GetShipScript(element.Attribute("script").Value), iconManager);
				ShipClasses.Add(newShipClass);
			}

			/*foreach (XElement element in shipSize.Elements())
			{
				int size = int.Parse(element.Attribute("size").Value);
				if (shipSizes.Contains(size))
				{
					throw new Exception("Duplicate ship size: " + size);
				}
				shipSizes.Add(size);
				string styleName = element.Attribute("name").Value;
				shipSizeLabels.Add(size, styleName);
				shipSprites.Add(size, new List<Sprite>());
				int i = 0;
				foreach (XElement style in element.Elements())
				{
					x = int.Parse(style.Attribute("xPos").Value);
					y = int.Parse(style.Attribute("yPos").Value);
					int spriteSize = int.Parse(style.Attribute("size").Value);
					Sprite ship = new Sprite(RaceName + size + styleName + i.ToString(), raceGraphic.Image, x, y, spriteSize, spriteSize);
					shipSprites[size].Add(ship);
					i++;
				}
				if (shipSprites[size].Count == 0)
				{
					throw new Exception("Size " + size + " have no sprites");
				}
			}*/

			XElement miniAvatarElement = race.Element("MiniAvatar");
			x = int.Parse(miniAvatarElement.Attribute("xPos").Value);
			y = int.Parse(miniAvatarElement.Attribute("yPos").Value);
			miniAvatar = new Sprite(RaceName + "miniAvatar", raceGraphic.Image, x, y, 128, 128);

			XElement neutralAvatarElement = race.Element("NeutralAvatar");
			x = int.Parse(neutralAvatarElement.Attribute("xPos").Value);
			y = int.Parse(neutralAvatarElement.Attribute("yPos").Value);
			neutralAvatar = new Sprite(RaceName + "neutralAvatar", raceGraphic.Image, x, y, 300, 300);

			XElement pleasedAvatarElement = race.Element("PleasedAvatar");
			x = int.Parse(pleasedAvatarElement.Attribute("xPos").Value);
			y = int.Parse(pleasedAvatarElement.Attribute("yPos").Value);
			pleasedAvatar = new Sprite(RaceName + "pleasedAvatar", raceGraphic.Image, x, y, 300, 300);

			XElement annoyedAvatarElement = race.Element("AnnoyedAvatar");
			x = int.Parse(annoyedAvatarElement.Attribute("xPos").Value);
			y = int.Parse(annoyedAvatarElement.Attribute("yPos").Value);
			annoyedAvatar = new Sprite(RaceName + "annoyedAvatar", raceGraphic.Image, x, y, 300, 300);

			XElement groundUnitElement = race.Element("GroundUnit");
			x = int.Parse(groundUnitElement.Attribute("xPos").Value);
			y = int.Parse(groundUnitElement.Attribute("yPos").Value);
			groundUnit = new Sprite(RaceName + "groundUnit", raceGraphic.Image, x, y, 32, 32);

			XElement groundUnitDyingElement = race.Element("GroundUnitDying");
			x = int.Parse(groundUnitDyingElement.Attribute("xPos").Value);
			y = int.Parse(groundUnitDyingElement.Attribute("yPos").Value);
			groundUnitDying = new Sprite(RaceName + "groundUnitDying", raceGraphic.Image, x, y, 32, 32);

			XElement fleetIconElement = race.Element("FleetIcon");
			x = int.Parse(fleetIconElement.Attribute("xPos").Value);
			y = int.Parse(fleetIconElement.Attribute("yPos").Value);
			fleetIcon = new Sprite(RaceName + "fleetIcon", raceGraphic.Image, x, y, 32, 32);
			fleetIcon.SetAxis(16, 16);

			XElement cityElement = race.Element("City");
			x = int.Parse(cityElement.Attribute("xPos").Value);
			y = int.Parse(cityElement.Attribute("yPos").Value);
			city = new Sprite(RaceName + "city", raceGraphic.Image, x, y, 300, 200);

			XElement troopShipElement = race.Element("TroopShip");
			x = int.Parse(troopShipElement.Attribute("xPos").Value);
			y = int.Parse(troopShipElement.Attribute("yPos").Value);
			troopShip = new Sprite(RaceName + "troopShip", raceGraphic.Image, x, y, 300, 200);

			StartingShips = new List<StartingShip>();
			/*XElement startShip = race.Element("StartShips");
			foreach (XElement element in startShip.Elements())
			{
				StartingShip ship = new StartingShip();
				ship.shipClass = element.Attribute("class").Value;
				ship.style = int.Parse(element.Attribute("style").Value);
				ship.name = element.Attribute("name").Value;
				ship.addToBlueprints = bool.Parse(element.Attribute("addToBlueprints").Value);
				ship.equipment = new List<StartingEquipment>();
				foreach (XElement equipment in element.Elements())
				{
					StartingEquipment newEquipment = new StartingEquipment();
					newEquipment.modifiers = new List<string>();
					newEquipment.modifiableValues = new List<string>();
					newEquipment.mainItem = equipment.Attribute("mainItem").Value;
					if (equipment.Attribute("mountItem") != null)
					{
						newEquipment.mountItem = equipment.Attribute("mountItem").Value;
						if (equipment.Attribute("modifierItems") != null)
						{
							newEquipment.modifiers.AddRange(equipment.Attribute("modifierItems").Value.Split(new[] { ',' }));
						}
					}
					if (equipment.Attribute("modifiableValues") != null)
					{
						foreach (string value in equipment.Attribute("modifiableValues").Value.Split(new[] { '|' }))
						{
							if (!string.IsNullOrEmpty(value))
							{
								newEquipment.modifiableValues.Add(value);
							}
						}
					}
					ship.equipment.Add(newEquipment);
				}
				startingShips.Add(ship);
			}*/

			StartingSystems = new List<StartingSystem>();
			XElement startSystem = race.Element("StartSystems");
			foreach (XElement element in startSystem.Elements())
			{
				StartingSystem system = new StartingSystem();
				system.OverrideSystem = bool.Parse(element.Attribute("override").Value);
				system.InOrder = bool.Parse(element.Attribute("inOrder").Value);
				if (!system.OverrideSystem)
				{
					system.ReplacePlanets = bool.Parse(element.Attribute("replacePlanets").Value);
				}
				foreach (XElement item in element.Elements())
				{
					if (item.Name == "Planet")
					{
						StartingPlanet planet = new StartingPlanet();
						planet.PlanetType = item.Attribute("type").Value;
						planet.Owned = bool.Parse(item.Attribute("owned").Value);
						if (planet.Owned)
						{
							planet.Population = int.Parse(item.Attribute("population").Value);
						}
						planet.Regions = new List<string>();
						planet.Resources = new Dictionary<string, float>();
						foreach (XElement value in item.Elements())
						{
							switch (value.Name.LocalName)
							{
								case "Region": planet.Regions.Add(value.Attribute("type").Value);
									break;
								case "Resource": planet.Resources.Add(value.Attribute("type").Value, float.Parse(value.Attribute("amount").Value, CultureInfo.InvariantCulture));
									break;
							}
						}
						system.Planets.Add(planet);
					}
					/*else if (item.Name == "Squadron")
					{
						StartingSquadron startingSquadron = new StartingSquadron();
						startingSquadron.StartingName = item.Attribute("name") == null ? "Squadron" : item.Attribute("name").Value;
						foreach (XElement ship in item.Elements())
						{
							StartingShip startingShip = new StartingShip();
							startingShip.name = ship.Attribute("name").Value;
							startingSquadron.StartingShips.Add(startingShip);
						}
						system.Squadrons.Add(startingSquadron);
					}*/
				}
				StartingSystems.Add(system);
			}

			reason = null;
			return errors.Count == 0;
		}

		public void ValidateShipDesigns(MasterTechnologyList masterTechnologyList)
		{
			/*foreach (StartingShip ship in startingShips)
			{
				foreach (StartingEquipment equipment in ship.equipment)
				{
					masterTechnologyList.ValidateEquipment(equipment.mainItem, equipment.mountItem, equipment.modifiers, this);
				}
			}*/
		}

		public string GetRandomShipName(int size)
		{
			//This attempts to get a random name from list of ship names.  If no such list exists, it uses a random generator
			if (shipNames.ContainsKey(size) && shipNames[size].Count > 0)
			{
				Random r = new Random();
				return shipNames[size][r.Next(shipNames[size].Count)];
			}

			NameGenerator nameGenerator = new NameGenerator();
			return nameGenerator.GetName();
		}

		public Sprite GetMiniAvatar()
		{
			return miniAvatar;
		}

		public Sprite GetGroundUnit()
		{
			return groundUnit;
		}

		public Sprite GetGroundUnitDying()
		{
			return groundUnitDying;
		}

		public Sprite GetFleetIcon()
		{
			return fleetIcon;
		}

		public Sprite GetCity()
		{
			return city;
		}

		public Sprite GetTroopShip()
		{
			return troopShip;
		}

		public Sprite GetAvatar(Expression whichExpression)
		{
			switch (whichExpression)
			{
				case Expression.ANNOYED:
					return annoyedAvatar;
				case Expression.PLEASED:
					return pleasedAvatar;
			}
			return neutralAvatar;
		}

		public string GetRandomEmperorName()
		{
			//There will be a list of emperor names, or unique name generator, but for now, just use random name generator
			NameGenerator nameGenerator = new NameGenerator();
			return nameGenerator.GetName();
		}
	}
}
