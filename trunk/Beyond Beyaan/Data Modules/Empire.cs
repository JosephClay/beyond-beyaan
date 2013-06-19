using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using GorgonLibrary.Graphics;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	enum PlayerType { HUMAN, CPU }
	class Empire
	{
		#region Member Variables
		private string empireName;
		private int empireID; //used to create unique sprite name
		//private float reserves;
		//private float expenses;
		private Color empireColor;
		private float[] convertedColor;
		private PlayerType type;
		private StarSystem selectedSystem;
		private StarSystem lastSelectedSystem; //the last system selected by the player (can be the current selected system, used for end of turn processing)
		private int planetSelected;
		private int fleetSelected;
		private PlanetManager planetManager;
		//private FleetManager fleetManager;
		private TechnologyManager technologyManager;
		private ContactManager contactManager;
		private FleetGroup selectedFleetGroup;
		private SitRepManager sitRepManager;
		//GorgonLibrary.Graphics.Sprite influenceMap;
		private List<Fleet> visibleOtherFleets;
		private Race race;
		private AI ai;
		private float planetIncome;
		private float totalResearchPoints;
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

		public FleetGroup SelectedFleetGroup
		{
			get { return selectedFleetGroup; }
			set { selectedFleetGroup = value; }
		}

		public int PlanetSelected
		{
			get { return planetSelected; }
			set { planetSelected = value; }
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

		/*public FleetManager FleetManager
		{
			get { return fleetManager; }
		}*/

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

		/*public GorgonLibrary.Graphics.Sprite InfluenceMap
		{
			get { return influenceMap; }
		}*/

		public List<StarSystem> SystemsUnderInfluence
		{
			get;
			set;
		}

		public List<Fleet> VisibleFleets
		{
			get { return visibleOtherFleets; }
		}

		public float EmpirePlanetIncome
		{
			get
			{
				if (Refresh)
				{
					planetIncome = 0;
					foreach (Planet planet in PlanetManager.Planets)
					{
						planetIncome += planet.TotalPopulation;
					}
					UpdateNetIncome();
					Refresh = false;
				}
				return planetIncome;
			}
		}
		public float EmpireTradeIncome
		{
			get;
			private set;
		}
		public float ShipMaintenance
		{
			get;
			private set;
		}
		public float EspionageExpense
		{
			get;
			private set;
		}
		public float SecurityExpense
		{
			get;
			private set;
		}
		public float NetIncome
		{
			get;
			private set;
		}
		public float ResearchPoints
		{
			get { return totalResearchPoints; }
		}
		public Race EmpireRace
		{
			get { return race; }
		}

		public bool Refresh { get; set; }
		#endregion

		#region Constructors
		public Empire(string emperorName, int empireID, Race race, PlayerType type, AI ai, Color color, GameMain gameMain)
		{
			this.empireName = emperorName;
			this.empireID = empireID;
			this.type = type;
			EmpireColor = color;
			technologyManager = new TechnologyManager();
			try
			{
				string techPath = Path.Combine(gameMain.GameDataSet.FullName, "technologies.txt");
				technologyManager.LoadTechnologies(techPath);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			//fleetManager = new FleetManager(this);
			planetManager = new PlanetManager();
			sitRepManager = new SitRepManager();
			//reserves = 0;
			//expenses = 0;
			visibleOtherFleets = new List<Fleet>();
			this.ai = ai;
			this.race = race;
			//this.handicap = 1.0f;
			Refresh = true;
		}
		#endregion

		#region Functions
		public void SetHomeSystem(StarSystem homeSystem, Planet homePlanet)
		{
			selectedSystem = homeSystem;
			lastSelectedSystem = homeSystem;
			planetManager.Planets.Add(homePlanet);
			//fleetManager.SetupStarterFleet(homeSystem.X + homeSystem.Size, homeSystem.Y);
			//homePlanet.ShipBeingBuilt = fleetManager.CurrentDesigns[0];
			//ShipMaintenance = fleetManager.GetExpenses();
			UpdateNetIncome();
		}

		public void SetUpContacts(List<Empire> allEmpires)
		{
			contactManager = new ContactManager(this, allEmpires);
		}

		public void ClearTurnData()
		{
			//Clears all current turn's temporary data to avoid issues
			selectedFleetGroup = null;
			selectedSystem = lastSelectedSystem;
		}

		public void CheckExploredSystems(Galaxy galaxy)
		{
			/*foreach (Fleet fleet in fleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					StarSystem systemExplored = galaxy.GetStarAtPoint(new Point(fleet.GalaxyX - 1, fleet.GalaxyY));
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
					}
					systemExplored = galaxy.GetStarAtPoint(new Point(fleet.GalaxyX + 1, fleet.GalaxyY));
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
					}
					systemExplored = galaxy.GetStarAtPoint(new Point(fleet.GalaxyX, fleet.GalaxyY - 1));
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
					}
					systemExplored = galaxy.GetStarAtPoint(new Point(fleet.GalaxyX, fleet.GalaxyY + 1));
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
					}
				}
			}*/
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

		public void CheckForBuiltShips()
		{
			/*foreach (Planet planet in planetManager.Planets)
			{
				int amount;
				Ship result = planet.CheckIfShipBuilt(out amount);
				if (amount > 0 && result != null)
				{
					Fleet newFleet = new Fleet();
					newFleet.Empire = this;
					newFleet.GalaxyX = planet.System.X + planet.System.Size;
					newFleet.GalaxyY = planet.System.Y;
					newFleet.AddShips(result, amount);
					fleetManager.AddFleet(newFleet);
					sitRepManager.AddItem(new SitRepItem(Screen.Galaxy, planet.System, planet, new Point(planet.System.X, planet.System.Y), planet.Name + " has produced " + amount + " " + result.Name + " ship" + (amount > 1 ? "s." : ".")));
				}
			}
			fleetManager.MergeIdleFleets();
			ShipMaintenance = fleetManager.GetExpenses();
			UpdateNetIncome();*/
		}

		public void SetVisibleFleets(List<Fleet> fleets)
		{
			visibleOtherFleets = fleets;
		}

		private void UpdateNetIncome()
		{
			NetIncome = planetIncome;
			NetIncome += EmpireTradeIncome;

			NetIncome -= ShipMaintenance;
			NetIncome -= EspionageExpense;
			NetIncome -= SecurityExpense;
		}

		public void UpdateResearchPoints()
		{
			totalResearchPoints = 0;
			foreach (Planet planet in planetManager.Planets)
			{
				totalResearchPoints += planet.ResearchOutput;
			}
		}

		public void HandleAIEmpire()
		{
		}
		#endregion
	}
}
