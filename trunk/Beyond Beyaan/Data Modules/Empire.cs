using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
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
		//private float reserves;
		//private float expenses;
		private Color empireColor;
		private PlayerType type;
		private StarSystem selectedSystem;
		private StarSystem lastSelectedSystem; //the last system selected by the player (can be the current selected system, used for end of turn processing)
		private int planetSelected;
		private int fleetSelected;
		private FleetGroup selectedFleetGroup;
		//GorgonLibrary.Graphics.Sprite influenceMap;
		private List<Fleet> visibleOtherFleets;
		private float planetIncome;
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
				ConvertedColor = new[]
				{
					empireColor.R / 255.0f,
					empireColor.G / 255.0f,
					empireColor.B / 255.0f,
					empireColor.A / 255.0f
				};
			}
		}

		public float[] ConvertedColor { get; private set; }

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

		public PlanetManager PlanetManager { get; private set; }

		public FleetManager FleetManager { get; private set; }

		public TechnologyManager TechnologyManager { get; private set; }

		public ContactManager ContactManager { get; private set; }

		public SitRepManager SitRepManager { get; private set; }

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
				planetIncome = 0;
				foreach (Planet planet in PlanetManager.Planets)
				{
					planetIncome += planet.TotalPopulation * 0.5f;
					planetIncome += planet.InfrastructureTotal;
				}
				if (Refresh)
				{
					UpdateNetIncome();
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
		public float NetExpenses
		{
			get;
			private set;
		}
		public float ExpensesPercentage
		{
			get;
			private set;
		}

		public float ResearchPoints { get; private set; }

		public Race EmpireRace { get; private set; }

		public bool Refresh { get; set; }
		#endregion

		#region Constructors
		public Empire(string emperorName, int empireID, Race race, PlayerType type, int difficultyModifier, Color color, GameMain gameMain) : this()
		{
			this.empireName = emperorName;
			this.empireID = empireID;
			this.type = type;
			EmpireColor = color;
			TechnologyManager.DifficultyModifier = difficultyModifier;
			try
			{
				TechnologyManager.SetComputerTechs(gameMain.MasterTechnologyManager.GetRandomizedComputerTechs());
				TechnologyManager.SetConstructionTechs(gameMain.MasterTechnologyManager.GetRandomizedConstructionTechs());
				TechnologyManager.SetForceFieldTechs(gameMain.MasterTechnologyManager.GetRandomizedForceFieldTechs());
				TechnologyManager.SetPlanetologyTechs(gameMain.MasterTechnologyManager.GetRandomizedPlanetologyTechs());
				TechnologyManager.SetPropulsionTechs(gameMain.MasterTechnologyManager.GetRandomizedPropulsionTechs());
				TechnologyManager.SetWeaponTechs(gameMain.MasterTechnologyManager.GetRandomizedWeaponTechs());
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			this.EmpireRace = race;
			//this.handicap = 1.0f;
		}
		public Empire()
		{
			FleetManager = new FleetManager(this);
			TechnologyManager = new TechnologyManager();
			PlanetManager = new PlanetManager();
			SitRepManager = new SitRepManager();
			//reserves = 0;
			//expenses = 0;
			visibleOtherFleets = new List<Fleet>();
			Refresh = true;
		}
		#endregion

		#region Functions
		public void SetHomeSystem(StarSystem homeSystem, Planet homePlanet)
		{
			selectedSystem = homeSystem;
			lastSelectedSystem = homeSystem;
			PlanetManager.AddOwnedPlanet(homePlanet);
			FleetManager.SetupStarterFleet(homeSystem);
			homePlanet.ShipBeingBuilt = FleetManager.CurrentDesigns[0];
			ShipMaintenance = FleetManager.GetExpenses();
			Refresh = true;
			UpdateNetIncome();
			homePlanet.SetCleanup();
		}

		public void SetUpContacts(List<Empire> allEmpires)
		{
			ContactManager = new ContactManager(this, allEmpires);
		}

		public void ClearTurnData()
		{
			//Clears all current turn's temporary data to avoid issues
			selectedFleetGroup = null;
			selectedSystem = lastSelectedSystem;
		}

		public List<StarSystem> CheckExploredSystems(Galaxy galaxy)
		{
			List<StarSystem> exploredSystems = new List<StarSystem>();
			foreach (Fleet fleet in FleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					StarSystem systemExplored = fleet.AdjacentSystem;
					if (systemExplored != null && !systemExplored.IsThisSystemExploredByEmpire(this))
					{
						SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, systemExplored, null, new Point(systemExplored.X, systemExplored.Y), systemExplored.Name + " has been explored."));
						systemExplored.AddEmpireExplored(this);
						exploredSystems.Add(systemExplored);
					}
				}
			}
			return exploredSystems;
		}

		public List<Fleet> CheckColonizableSystems(Galaxy galaxy)
		{
			List<Fleet> colonizingFleets = new List<Fleet>();
			foreach (Fleet fleet in FleetManager.GetFleets())
			{
				if (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0)
				{
					if (fleet.AdjacentSystem.Planets[0].Owner != null)
					{
						continue;
					}
					int colonyReq = fleet.AdjacentSystem.Planets[0].ColonyRequirement;
					foreach (Ship ship in fleet.OrderedShips)
					{
						foreach (var special in ship.Specials)
						{
							if (special.Colony >= colonyReq)
							{
								colonizingFleets.Add(fleet);
								break;
							}
						}
					}
				}
			}
			return colonizingFleets;
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
				image.Resize(squaredSize, squaredSize);
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

		public void LaunchTransports()
		{
			foreach (var planet in PlanetManager.Planets)
			{
				if (planet.TransferSystem.Key.StarSystem != null)
				{
					Fleet newFleet = new Fleet();
					newFleet.Empire = this;
					newFleet.GalaxyX = planet.System.X;
					newFleet.GalaxyY = planet.System.Y;
					newFleet.AddTransport(planet.Races[0], planet.TransferSystem.Value);
					newFleet.TravelNodes = new List<TravelNode> {planet.TransferSystem.Key };
					planet.RemoveRacePopulation(planet.Races[0], planet.TransferSystem.Value);
					planet.TransferSystem = new KeyValuePair<TravelNode,int>(new TravelNode(), 0);
					newFleet.ResetMove();
					FleetManager.AddFleet(newFleet);
				}
			}
		}

		public void LandTransports()
		{
			List<Fleet> fleetsToRemove = new List<Fleet>();
			foreach (var fleet in FleetManager.GetFleets())
			{
				if (fleet.TransportShips.Count > 0 && (fleet.TravelNodes == null || fleet.TravelNodes.Count == 0) && fleet.AdjacentSystem != null)
				{
					if (fleet.AdjacentSystem.Planets[0].Owner == this)
					{
						foreach (var transport in fleet.TransportShips)
						{
							fleet.AdjacentSystem.Planets[0].AddRacePopulation(transport.raceOnShip, transport.amount);
						}
						fleetsToRemove.Add(fleet);
					}
				}
			}
			foreach (var fleet in fleetsToRemove)
			{
				FleetManager.RemoveFleet(fleet);
			}
		}

		public void CheckForBuiltShips()
		{
			foreach (Planet planet in PlanetManager.Planets)
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
					FleetManager.AddFleet(newFleet);
					SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, planet.System, planet, new Point(planet.System.X, planet.System.Y), planet.Name + " has produced " + amount + " " + result.Name + " ship" + (amount > 1 ? "s." : ".")));
				}
			}
			FleetManager.MergeIdleFleets();
			ShipMaintenance = FleetManager.GetExpenses();
			UpdateNetIncome();
		}

		public void SetVisibleFleets(List<Fleet> fleets)
		{
			visibleOtherFleets = fleets;
		}

		private void UpdateNetIncome()
		{
			Refresh = false; //We're already refreshing stuff at this point
			NetIncome = EmpirePlanetIncome;
			NetIncome += EmpireTradeIncome;

			NetExpenses = ShipMaintenance;
			NetExpenses += EspionageExpense;
			NetExpenses += SecurityExpense;
			
			ExpensesPercentage = NetExpenses / NetIncome;
		}

		public void UpdateResearchPoints()
		{
			ResearchPoints = 0;
			foreach (Planet planet in PlanetManager.Planets)
			{
				ResearchPoints += planet.ResearchAmount * 0.01f * planet.ActualProduction;
			}
		}

		public void HandleAIEmpire()
		{
		}

		public void Save(XmlWriter writer)
		{
			writer.WriteStartElement("Empire");
			writer.WriteAttributeString("ID", empireID.ToString());
			writer.WriteAttributeString("Name", empireName);
			writer.WriteAttributeString("Color", empireColor.ToArgb().ToString());
			writer.WriteAttributeString("Race", EmpireRace.RaceName);
			writer.WriteAttributeString("IsHumanPlayer", type == PlayerType.HUMAN ? "True" : "False");
			writer.WriteAttributeString("SelectedSystem", lastSelectedSystem.ID.ToString());
			TechnologyManager.Save(writer);
			FleetManager.Save(writer);
			//sitRepManager.Save(writer);
			writer.WriteEndElement();
		}
		public void Load(XElement empireToLoad, GameMain gameMain)
		{
			empireID = int.Parse(empireToLoad.Attribute("ID").Value);
			empireName = empireToLoad.Attribute("Name").Value;
			EmpireColor = Color.FromArgb(int.Parse(empireToLoad.Attribute("Color").Value));
			EmpireRace = gameMain.RaceManager.GetRace(empireToLoad.Attribute("Race").Value);
			type = bool.Parse(empireToLoad.Attribute("IsHumanPlayer").Value) ? PlayerType.HUMAN : PlayerType.CPU;
			lastSelectedSystem = gameMain.Galaxy.GetStarWithID(int.Parse(empireToLoad.Attribute("SelectedSystem").Value));
			TechnologyManager.Load(empireToLoad, gameMain.MasterTechnologyManager);
			FleetManager.Load(empireToLoad, this, gameMain);
		}
		#endregion
	}
}
