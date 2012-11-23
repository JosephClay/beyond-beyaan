using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using GorgonLibrary.Graphics;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum PlayerType { HUMAN, CPU }
	public class Empire
	{
		#region Member Variables
		private string empireName;
		private int empireID; //used to create unique sprite name
		private float reserves;
		//private float expenses;
		private Color empireColor;
		private float[] convertedColor;
		private PlayerType type;
		private StarSystem selectedSystem;
		private StarSystem lastSelectedSystem; //the last system selected by the player (can be the current selected system, used for end of turn processing)
		//private int planetSelected;
        //private int lastPlanetSelected;
		private int fleetSelected;
		private PlanetManager planetManager;
		private StarSystemManager starSystemManager;
		private FleetManager fleetManager;
		private ItemManager itemManager;
		private TechnologyManager technologyManager;
		private ContactManager contactManager;
		private SquadronGroup selectedFleetGroup;
		private ProjectManager projectManager;
		private SitRepManager sitRepManager;
		//GorgonLibrary.Graphics.Sprite influenceMap;
		//private List<Squadron> visibleOtherFleets;
		private Race race;
		private AI ai;
		//private float totalResearchPoints;


		#region Economy Data
		private Dictionary<Resource, float> productions;
		private Dictionary<Resource, float> consumptions;
		private Dictionary<Resource, float> resources;
		private Dictionary<Resource, float> shortages;
		private Dictionary<Resource, float> availableResources;

		public Dictionary<Resource, float> Productions
		{
			get { return productions; }
		}
		public Dictionary<Resource, float> Consumptions
		{
			get { return consumptions; }
		}
		public Dictionary<Resource, float> Resources
		{
			get { return resources; }
		}
		public Dictionary<Resource, float> Shortages
		{
			get { return shortages; }
		}
		public Dictionary<Resource, float> AvailableResources
		{ 
			get { return availableResources; } 
		}
		#endregion
		//private float handicap;
		#endregion

		#region Properties
		public string EmpireName
		{
			get { return empireName; }
		}
		public int EmpireID
		{
			get { return empireID; }
		}

		public Color EmpireColor
		{
			get { return empireColor; }
			set 
			{ 
				empireColor = value;
				convertedColor = new float[]
				{
					empireColor.R / 255.0f,
					empireColor.G / 255.0f,
					empireColor.B / 255.0f,
					empireColor.A / 255.0f
				};
			}
		}

		public float[] ConvertedColor
		{
			get { return convertedColor; }
		}

		public bool ShowBorders
		{
			get;
			private set;
		}

		public PlayerType Type
		{
			get { return type; }
		}

		public StarSystem SelectedSystem
		{
			get { return selectedSystem; }
			set { selectedSystem = value; }
		}

		public StarSystem LastSelectedSystem
		{
			get { return lastSelectedSystem; }
			set { lastSelectedSystem = value; }
		}

		public SquadronGroup SelectedFleetGroup
		{
			get { return selectedFleetGroup; }
			set { selectedFleetGroup = value; }
		}

		public int FleetSelected
		{
			get { return fleetSelected; }
			set { fleetSelected = value; }
		}

		public PlanetManager PlanetManager
		{
			get { return planetManager; }
		}

		public FleetManager FleetManager
		{
			get { return fleetManager; }
		}

		public ItemManager ItemManager
		{
			get { return itemManager; }
		}

		public TechnologyManager TechnologyManager
		{
			get { return technologyManager; }
		}

		public ContactManager ContactManager
		{
			get { return contactManager; }
		}

		public SitRepManager SitRepManager
		{
			get { return sitRepManager; }
		}

		public ProjectManager ProjectManager
		{
			get { return projectManager; }
		}

		/*public GorgonLibrary.Graphics.Sprite InfluenceMap
		{
			get { return influenceMap; }
		}*/

		public List<StarSystem> SystemsUnderInfluence
		{
			get;
			set;
		}

		/*public List<Squadron> VisibleFleets
		{
			get { return visibleOtherFleets; }
		}*/
		public float EmpireProduction
		{
			get
			{
				float planetProduction = 0;
				foreach (Planet planet in PlanetManager.Planets)
				{
					//planetProduction += planet.ConstructionOutput;
				}
				return planetProduction;
			}
		}
		public float EmpirePlanetResearch
		{
			get
			{
				float planetResearch = 0;
				foreach (Planet planet in PlanetManager.Planets)
				{
					//planetResearch += planet.ResearchOutput;
				}
				return planetResearch;
			}
		}
		public float EmpirePlanetIncome
		{
			get
			{
				float planetIncome = 0;
				foreach (Planet planet in PlanetManager.Planets)
				{
					//planetIncome += planet.CommerceOutput;
				}
				return planetIncome;
			}
		}
		public float EmpireTradeIncome
		{
			get;
			private set;
		}
		public float EmpireTradeResearch
		{
			get;
			private set;
		}
		public float ShipMaintenance
		{
			get
			{
				return fleetManager.GetExpenses();
			}
		}
		public int EspionageExpense
		{
			get
			{
				int amount = 0;
				foreach (Contact contact in contactManager.Contacts)
				{
					if (contact.Contacted)
					{
						amount += contact.SpyEffort;
					}
				}
				return amount;
			}
		}
		public float SecurityExpense
		{
			get
			{
				int amount = 0;
				foreach (Contact contact in contactManager.Contacts)
				{
					if (contact.Contacted)
					{
						amount += contact.AntiSpyEffort;
					}
				}
				return amount;
			}
		}
		public float NetIncome
		{
			get;
			private set;
		}
		/*public float ResearchPoints
		{
			get { return totalResearchPoints; }
		}*/
		public Race EmpireRace
		{
			get { return race; }
		}
		public float Reserves
		{
			get { return reserves; }
		}
		#endregion

		#region Constructors
		public Empire(string empireName, int empireID, Race race, PlayerType type, AI ai, Color color)
		{
			this.empireName = empireName;
			this.empireID = empireID;
			this.type = type;
			EmpireColor = color;
			this.race = race;
			itemManager = new ItemManager();
			technologyManager = new TechnologyManager();
			fleetManager = new FleetManager(this);
			planetManager = new PlanetManager();
			starSystemManager = new StarSystemManager(this);
			sitRepManager = new SitRepManager();
			projectManager = new ProjectManager(this);
			resources = new Dictionary<Resource, float>();
			consumptions = new Dictionary<Resource, float>();
			shortages = new Dictionary<Resource, float>();
			productions = new Dictionary<Resource, float>();
			availableResources = new Dictionary<Resource, float>();
			reserves = 10;
			//expenses = 0;
			//visibleOtherFleets = new List<Squadron>();
			this.ai = ai;
			//this.handicap = 1.0f;
			ShowBorders = false;
		}
		#endregion

		#region Functions
		public void SetHomeSystem(List<StarSystem> startingSystems, List<Planet> planets)
		{
			selectedSystem = startingSystems[0];
			lastSelectedSystem = startingSystems[0];
			planetManager.Planets.AddRange(planets);
			starSystemManager.StarSystems.AddRange(startingSystems);
			//fleetManager.SetupStarterFleet(homeSystem.X + homeSystem.Size, homeSystem.Y);
			//homePlanet.ShipBeingBuilt = fleetManager.ShipDesigns[0];
		}

		public void SetStartingFleets(List<StarSystem> startingSystems, MasterTechnologyList masterTechnologyList, IconManager iconManager)
		{
			for (int i = 0; i < startingSystems.Count; i++)
			{
				if (race.StartingSystems[i].Squadrons.Count == 0)
				{
					continue;
				}
				foreach (StartingSquadron squadron in race.StartingSystems[i].Squadrons)
				{
					Squadron startingSquadron = new Squadron(startingSystems[i]);
					startingSquadron.Name = squadron.StartingName;
					List<ShipDesign> startingShips = new List<ShipDesign>();
					foreach (StartingShip ship in squadron.StartingShips)
					{
						StartingShip shipToConvert = null;
						foreach (StartingShip startingShip in race.StartingShips)
						{
							if (startingShip.name == ship.name)
							{
								shipToConvert = startingShip;
								break;
							}
						}

						//ShipDesign realShip = masterTechnologyList.ConvertStartingShipToRealShip(shipToConvert, race, iconManager);

						// This is where to add single ship vs stacked ship check
						//for (int j = 0; j < ship.Value; j++)
						{
							//startingSquadron.AddShipFromDesign(realShip);
						}
					}
					startingSquadron.Empire = this;
					fleetManager.AddFleet(startingSquadron);
				}
			}
			//Now to add the starting ships to blueprints
			foreach (StartingShip ship in race.StartingShips)
			{
				if (ship.addToBlueprints)
				{
					//ShipDesign realShip = masterTechnologyList.ConvertStartingShipToRealShip(ship, race, iconManager);
					//fleetManager.AddShipDesign(realShip);
				}
			}
			if (fleetManager.ShipDesigns.Count > 0)
			{
				fleetManager.LastShipDesign = new ShipDesign(fleetManager.ShipDesigns[0]);
			}
			else
			{
				fleetManager.LastShipDesign = new ShipDesign();
				fleetManager.LastShipDesign.ShipClass = race.ShipClasses[0];
			}
		}

		public void ClearFleetMovement(Squadron whichFleet)
		{
			whichFleet.TravelNodes = null;
		}

		public void SetUpContacts(List<Empire> allEmpires)
		{
			contactManager = new ContactManager(this, allEmpires);
		}

		public List<StarSystem> GetProductiveSystems()
		{
			List<StarSystem> systems = new List<StarSystem>();
			foreach (Planet planet in planetManager.Planets)
			{
				if (!systems.Contains(planet.System))
				{
					systems.Add(planet.System);
				}
			}
			//Add check for planets with production-capable ships orbiting

			return systems;
		}

		public void ClearTurnData()
		{
			//Clears all current turn's temporary data to avoid issues
			selectedFleetGroup = null;
			selectedSystem = lastSelectedSystem;
		}

		public void UpdateAll()
		{
			//planetManager.UpdateProduction(productions, consumptions, shortages, resources);
		}

		public void ToggleBorder()
		{
			ShowBorders = !ShowBorders;
		}

		public void CheckExploredSystems(Galaxy galaxy)
		{
			foreach (Squadron fleet in fleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					StarSystem systemExplored = fleet.System;
					if (!systemExplored.IsThisSystemExploredByEmpire(this))
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
					}
				}
			}
		}

		/*public void CreateInfluenceMapSprite(GridCell[][] gridCells)
		{
			int squaredSize = 2;

			while (squaredSize < gridCells.Length)
			{
				squaredSize *= 2;
			}
			GorgonLibrary.Graphics.Image image;
			if (GorgonLibrary.Graphics.ImageCache.Images.Contains(empireName + empireID))
			{
				image = GorgonLibrary.Graphics.ImageCache.Images[empireName + empireID];
				image.SetDimensions(squaredSize, squaredSize);
			}
			else
			{
				image = new GorgonLibrary.Graphics.Image(empireName + empireID, squaredSize, squaredSize, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			}
			image.Clear(Color.FromArgb(0, 0, 0, 0));
			GorgonLibrary.Graphics.Image.ImageLockBox newImage = image.GetImageData();
			for (int i = 0; i < gridCells.Length; i++)
			{
				for (int j = 0; j < gridCells[i].Length; j++)
				{
					if (gridCells[i][j].dominantEmpire == this)
					{
						newImage[i, j] = gridCells[i][j].dominantEmpire.empireColor.ToArgb();
					}
				}
			}
			influenceMap = new GorgonLibrary.Graphics.Sprite(empireName + empireID, image);
		}*/

		/*public void UpdateMigration(Galaxy galaxy)
		{
			List<Planet> migratingPlanets = new List<Planet>(); //Planets that have people moving out
			List<Planet> immigratingPlanets = new List<Planet>(); //Planets that have people moving in

			foreach (StarSystem system in SystemsUnderInfluence)
			{
				foreach (Planet planet in system.Planets)
				{
					if (planet.PlanetType == PLANET_TYPE.ASTEROIDS || planet.PlanetType == PLANET_TYPE.GAS_GIANT || (planet.Owner != this && planet.Owner != null))
					{
						//Uninhabitable or foreign owned planets are skipped
						continue;
					}
					if (planet.Population >= (planet.PopulationMax * 0.75f))
					{
						migratingPlanets.Add(planet);
					}
					else if (planet.Population < (planet.PopulationMax * 0.25f))
					{
						immigratingPlanets.Add(planet);
					}
				}
			}
			if (migratingPlanets.Count == 0 || immigratingPlanets.Count == 0)
			{
				//Can't migrate anywhere, so end this function
				return;
			}

			migratingPlanets.Sort((Planet a, Planet b) => 
				{ 
					return (b.Population / b.PopulationMax).CompareTo(a.Population / a.PopulationMax); 
				});
			immigratingPlanets.Sort((Planet a, Planet b) =>
			{
				return (a.Population / a.PopulationMax).CompareTo(b.Population / b.PopulationMax);
			});

			int j = 0;
			for (int i = 0; i < migratingPlanets.Count; i++)
			{
				float amountToMove = (migratingPlanets[i].Population - (migratingPlanets[i].PopulationMax * 0.75f)) * 0.10f;
				while (amountToMove > 0 && j < immigratingPlanets.Count)
				{
					float amountCanMove = ((immigratingPlanets[j].PopulationMax * 0.25f) - immigratingPlanets[j].Population) * 0.17f;
					bool isAlreadyOwned = immigratingPlanets[j].Owner != null;
					immigratingPlanets[j].Owner = this;
					if (amountToMove > amountCanMove)
					{
						migratingPlanets[i].Population -= amountCanMove;
						immigratingPlanets[j].Population += amountCanMove;
						amountToMove -= amountCanMove;
					}
					else
					{
						migratingPlanets[i].Population -= amountToMove;
						immigratingPlanets[j].Population += amountToMove;
						amountToMove = 0;
					}
					if (!isAlreadyOwned)
					{
						//Gotta set the industry to do something
						immigratingPlanets[j].SetMinimumFoodAndWaste();
						if (fleetManager.CurrentDesigns.Count > 0)
						{
							immigratingPlanets[j].ShipBeingBuilt = fleetManager.CurrentDesigns[0];
						}
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, immigratingPlanets[j].System, immigratingPlanets[j], new Point(immigratingPlanets[j].System.X, immigratingPlanets[j].System.Y),
							immigratingPlanets[j].Name + " has been colonized."));
					}
					j++;
				}
			}
		}*/

		public void UpdateProjects(PlanetTypeManager planetTypeManager, Random r)
		{
			projectManager.UpdateProjects(EmpireProduction, planetTypeManager, r);
			//UpdateNetIncome();
		}

		/*public void SetVisibleFleets(List<Squadron> fleets)
		{
			visibleOtherFleets = fleets;
		}*/

		public void RefreshEconomy()
		{
			starSystemManager.TallyConsumption(out consumptions);
			starSystemManager.TallyResources(out resources);
			starSystemManager.TallyAvailableResourcesAndShortages(out availableResources, out shortages);
		}

		public void ProcessTurn()
		{
			//First, deduct the maintenance costs (the player is required to make sure there's sufficient resources for maintenance of buildings and ships before ending turn
			//To-do: Add maintenance deductions

			//Second, deduct feeding for people from resources (accumulate the total consumption, then compare it against available resources)
			/*planetManager.CalculateFoodConsumption(foodConsumptions);

			foodShortages.Clear();
			foreach (var foodConsumption in foodConsumptions)
			{
				if (resources.ContainsKey(foodConsumption.Key))
				{
					if (resources[foodConsumption.Key] < foodConsumption.Value)
					{
						foodShortages[foodConsumption.Key] = resources[foodConsumption.Key] / foodConsumption.Value;
					}
					else
					{
						foodShortages[foodConsumption.Key] = 1.0f;
					}
				}
				else
				{
					//No resource, so nothing to eat
					foodShortages[foodConsumption.Key] = 0.0f;
				}
			}

			planetManager.ConsumeFood(resources, foodShortages);			

			//Third, accumulate all the projects from planets, systems, and empire, and calculate the amount per remaining resources 
			//Example: if two projects costs 100 ores, and one project costs 50 ores, the total cost is 250 ores.
			//Then if the empire produces 30 ores, it is 30/250, then each project gets 0.12 ores for each remaining required ores
			//So the two 100 ore projects will get 12 ores each (0.12 * 100) and the one 50 ore project gets 6 ores (50 * 0.12)

			//Fourth, go through each planet and have them consume the remaining resources, and produce resources
			planetManager.UpdateProduction(productions, consumptions, shortages, resources);

			foreach (var consumption in consumptions)
			{
				if (resources.ContainsKey(consumption.Key))
				{
					resources[consumption.Key] -= consumption.Value;
					if (resources[consumption.Key] < 0)
					{
						//There's a slight rounding error, just set it to flat zero.
						resources[consumption.Key] = 0;
					}
				}
				else
				{
					resources[consumption.Key] = 0;
				}
			}
			foreach (var production in productions)
			{
				if (resources.ContainsKey(production.Key))
				{
					resources[production.Key] += production.Value;
				}
				else
				{
					resources[production.Key] = production.Value;
				}
			}

			//Fifth, and last, calculate population growth, this is last because it affects the consumption/production of regions for next turn
			planetManager.UpdatePopGrowth(foodShortages);*/
		}

		public void HandleAIEmpire()
		{
		}
		#endregion
	}

	class SettlerToProcess
	{
		public Squadron whichFleet;
		public StarSystem whichSystem;
	}
}
