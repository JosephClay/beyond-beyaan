using System;
using System.Windows.Forms;
using System.IO;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Screens;

namespace Beyond_Beyaan
{
	public enum ScreenEnum { MainMenu, GalaxySetup, PlayerSetup, Galaxy, InGameMenu, Diplomacy, FleetList, Design, Production, Planets, Research, ProcessTurn, Battle, Colonize, Invade };

	public class GameMain
	{
		Form parentForm;
		internal DrawingManagement DrawingManagement;
		internal SpriteManager SpriteManager;
		internal UITypeManager UITypeManager;
		private ScreenManager ScreenManager;
		private ScreenInterface screenInterface;
		private MainGameMenu mainGameMenu;
		private PlayerSetup playerSetup;
		private GalaxySetup galaxySetup;
		private GalaxyScreen galaxyScreen;
		private InGameMenu inGameMenu;
		private DiplomacyScreen diplomacyScreen;
		private FleetListScreen fleetListScreen;
		private ProductionScreen productionScreen;
		private DesignScreen designScreen;
		private PlanetsScreen planetsScreen;
		private ResearchScreen researchScreen;
		private ProcessingTurnScreen processingTurnScreen;
		private SpaceCombat spaceCombat;
		private ColonizeScreen colonizeScreen;
		private InvadeScreen invadeScreen;
		private SituationReport situationReport;
		private TutorialWindow tutorialWindow;
		private ScreenEnum currentScreen;
		internal TaskBar taskBar;
		internal Galaxy galaxy;
		internal EmpireManager empireManager;
		internal RaceManager RaceManager;
		internal ParticleManager ParticleManager;
		internal EffectManager EffectManager;
		internal SectorTypeManager SectorTypeManager;
		internal PlanetTypeManager PlanetTypeManager;
		internal RegionTypeManager RegionTypeManager;
		internal StarTypeManager StarTypeManager;
		internal IconManager IconManager;
		internal ResourceManager ResourceManager;
		internal AIManager aiManager;
		internal ShipScriptManager ShipScriptManager;
		internal MasterItemManager MasterItemManager;
		internal MasterTechnologyList MasterTechnologyList;
		internal int ScreenWidth;
		internal int ScreenHeight;
		internal GorgonLibrary.Graphics.FXShader ShipShader;
		internal DirectoryInfo GameDataSet;
		internal Random r;
		internal Input Input;
		internal Point MousePos;

		private BBSprite Cursor;

		private string logFilePath;
		//private StreamWriter logger;

		internal int Turn { get; private set; }

		public bool Initalize(int screenWidth, int screenHeight, DirectoryInfo dataSet, bool showTutorial, Form parentForm, out string reason)
		{
			MousePos = new Point();

			r = new Random();
			this.parentForm = parentForm;

			reason = string.Empty;
			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;
			GameDataSet = dataSet;

			DirectoryInfo graphicDirectory = new DirectoryInfo(Path.Combine(GameDataSet.FullName, "graphics"));

			if (!GameConfiguration.LoadConfiguration(Path.Combine(GameDataSet.FullName, "config.xml"), out reason))
			{
				return false;
			}
			GameConfiguration.ShowTutorial = showTutorial;
			SpriteManager = new SpriteManager();
			IconManager = new IconManager();
			ResourceManager = new ResourceManager();
			MasterItemManager = new MasterItemManager();
			MasterTechnologyList = new MasterTechnologyList();
			ShipScriptManager = new ShipScriptManager();
			RaceManager = new RaceManager();
			SectorTypeManager = new SectorTypeManager();
			PlanetTypeManager = new PlanetTypeManager();
			RegionTypeManager = new RegionTypeManager();
			StarTypeManager = new StarTypeManager();
			ParticleManager = new ParticleManager();
			EffectManager = new EffectManager();
			DrawingManagement = new DrawingManagement();
			UITypeManager = new UITypeManager();
			ScreenManager = new ScreenManager();

			if (!SpriteManager.LoadSprites(GameDataSet, graphicDirectory, out reason))
			{
				return false;
			}
			if (!DrawingManagement.LoadGraphics(graphicDirectory.FullName, out reason))
			{
				return false;
			}
			if (!LoadTutorial(Path.Combine(GameDataSet.FullName, "tutorial.xml"), out reason))
			{
				return false;
			}
			if (!IconManager.Initialize(GameDataSet.FullName, graphicDirectory.FullName, out reason))
			{
				return false;
			}
			if (!ResourceManager.Initialize(GameDataSet.FullName, IconManager, out reason))
			{
				return false;
			}
			if (!MasterItemManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			if (!MasterTechnologyList.LoadTechnologies(GameDataSet.FullName, ResourceManager, MasterItemManager, out reason))
			{
				return false;
			}
			if (!ShipScriptManager.LoadShipScripts(Path.Combine(Path.Combine(GameDataSet.FullName, "Scripts"), "Ship"), out reason))
			{
				return false;
			}
			if (!RaceManager.LoadRaces(GameDataSet.FullName, graphicDirectory.FullName, MasterTechnologyList, ShipScriptManager, Path.Combine(Path.Combine(GameDataSet.FullName, "Scripts"), "Technology"), IconManager, ResourceManager, out reason))
			{
				return false;
			}
			if (!SectorTypeManager.LoadSectorTypes(Path.Combine(GameDataSet.FullName, "sectorObjects.xml"), this, out reason))
			{
				return false;
			}
			if (!PlanetTypeManager.LoadPlanetTypes(Path.Combine(GameDataSet.FullName, "planets.xml"), Path.Combine(graphicDirectory.FullName, "planets.png"), graphicDirectory.FullName, this, out reason))
			{
				return false;
			}
			if (!RegionTypeManager.LoadRegionTypes(Path.Combine(GameDataSet.FullName, "regions.xml"), this, out reason))
			{
				return false;
			}
			if (!StarTypeManager.LoadStarTypes(Path.Combine(GameDataSet.FullName, "stars.xml"), Path.Combine(graphicDirectory.FullName, "stars.png"), Path.Combine(GameDataSet.FullName, "shaders"), SectorTypeManager, this, out reason))
			{
				return false;
			}
			if (!ParticleManager.Initialize(GameDataSet.FullName, graphicDirectory.FullName, out reason))
			{
				return false;
			}
			if (!EffectManager.Initialize(GameDataSet.FullName, out reason))
			{
				return false;
			}
			if (!UITypeManager.LoadUITypes(GameDataSet, SpriteManager, out reason))
			{
				return false;
			}
			if (!ScreenManager.Initialize(GameDataSet, this, "MainMenu", r, out reason))
			{
				return false;
			}
			Cursor = SpriteManager.GetSprite("Cursor", r);
			if (Cursor == null)
			{
				reason = "Cursor is not defined in sprites.xml";
				return false;
			}
			ChangeToScreen(ScreenEnum.MainMenu);
			//StarShader = GorgonLibrary.Graphics.FXShader.FromFile("StarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			//BGStarShader = GorgonLibrary.Graphics.FXShader.FromFile("BGStarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			return true;
		}

		public void Resize(int screenWidth, int screenHeight)
		{
			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;

			if (screenInterface != null)
			{
				screenInterface.Resize();
			}
			if (taskBar != null)
			{
				taskBar.Resize();
			}
		}

		public void ProcessGame(float frameDeltaTime)
		{
			ScreenManager.DrawCurrentScreen();
			Cursor.Draw(MousePos.X, MousePos.Y);
			ScreenManager.MouseHover(MousePos.X, MousePos.Y, frameDeltaTime);
			Cursor.Update(frameDeltaTime, r);
			/*bool skipUpdate = false;
			bool handleTutorial = false;
			bool handleTaskBar = false;
			if (currentScreen != ScreenEnum.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != ScreenEnum.GalaxySetup && currentScreen != ScreenEnum.PlayerSetup && currentScreen != ScreenEnum.Battle)
				{
					handleTaskBar = true;
				}
			}
			if (handleTutorial)
			{
				if (GameConfiguration.ShowTutorial && tutorialWindow.MouseHover(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (handleTaskBar && !skipUpdate)
			{
				if (taskBar.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
				if (!skipUpdate && situationReport.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (!skipUpdate)
			{
				screenInterface.Update(MousePos.X, MousePos.Y, frameDeltaTime);
			}
			else
			{
				screenInterface.UpdateBackground(frameDeltaTime);
			}
			screenInterface.DrawScreen(DrawingManagement);
			if (handleTaskBar)
			{
				taskBar.Draw(DrawingManagement);
				situationReport.DrawSitRep(DrawingManagement);
			}
			if (GameConfiguration.ShowTutorial)
			{
				tutorialWindow.DrawWindow(DrawingManagement);
			}*/
		}

		public void MouseDown(MouseEventArgs e)
		{
			int whichButton = 0;
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						whichButton = 1;
					} break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						whichButton = 2;
					} break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						whichButton = 3;
					} break;
			}
			ScreenManager.MouseDown(e.X, e.Y, whichButton);
			/*int whichButton = 0;
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						whichButton = 1;
					} break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						whichButton = 2;
					} break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						whichButton = 3;
					} break;
			}
			bool handleTaskBar = false;
			bool handleTutorial = false;
			if (currentScreen != ScreenEnum.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != ScreenEnum.GalaxySetup && currentScreen != ScreenEnum.PlayerSetup && currentScreen != ScreenEnum.Battle)
				{
					handleTaskBar = true;
				}
			}
			if (handleTutorial)
			{
				if (GameConfiguration.ShowTutorial && tutorialWindow.MouseDown(e.X, e.Y))
				{
					return;
				}
			}
			if (handleTaskBar)
			{
				if (taskBar.MouseDown(e.X, e.Y, whichButton))
				{
					return;
				}
				if (situationReport.MouseDown(e.X, e.Y))
				{
					return;
				}
			}
			screenInterface.MouseDown(e.X, e.Y, whichButton);*/
		}

		public void MouseUp(MouseEventArgs e)
		{
			int whichButton = 0;
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						whichButton = 1;
					} break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						whichButton = 2;
					} break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						whichButton = 3;
					} break;
			}
			ScreenManager.MouseUp(e.X, e.Y, whichButton);
			/*bool handleTaskBar = false;
			bool handleTutorial = false;
			if (currentScreen != ScreenEnum.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != ScreenEnum.GalaxySetup && currentScreen != ScreenEnum.PlayerSetup && currentScreen != ScreenEnum.Battle)
				{
					handleTaskBar = true;
				}
			}
			if (handleTutorial)
			{
				if (GameConfiguration.ShowTutorial && tutorialWindow.MouseUp(e.X, e.Y))
				{
					return;
				}
			}
			if (handleTaskBar)
			{
				if (taskBar.MouseUp(e.X, e.Y, whichButton))
				{
					return;
				}
				if (situationReport.MouseUp(e.X, e.Y))
				{
					return;
				}
			}
			screenInterface.MouseUp(e.X, e.Y, whichButton);*/
		}

		public void MouseScroll(int delta)
		{
			//screenInterface.MouseScroll(delta, MousePos.X, MousePos.X);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			//screenInterface.KeyDown(e);
		}

		public void ToggleSitRep()
		{
			situationReport.ToggleVisibility();
		}

		public void ChangeToScreen(ScreenEnum whichScreen)
		{
			if (screenInterface is ProcessingTurnScreen)
			{
				Turn++;
			}
			switch (whichScreen)
			{
				case ScreenEnum.MainMenu: //Any way we get here means everything needs to be cleared out
					//this.DrawingManagement = new DrawingManagement();
					mainGameMenu = new MainGameMenu();
					//mainGameMenu.Initialize(this);
					//screenInterface = mainGameMenu;
					break;
				case ScreenEnum.GalaxySetup:
					if (galaxySetup == null)
					{
						galaxySetup = new GalaxySetup();
						galaxySetup.Initialize(this);
					}
					screenInterface = galaxySetup;
					break;
				case ScreenEnum.PlayerSetup:
					if (playerSetup == null)
					{
						playerSetup = new PlayerSetup();
						playerSetup.Initialize(this);
					}
					Turn = 1;
					screenInterface = playerSetup;
					break;
				case ScreenEnum.Galaxy:
					if (galaxyScreen == null)
					{
						galaxyScreen = new GalaxyScreen();
						galaxyScreen.Initialize(this);
					}
					taskBar.Hide = false;
					galaxyScreen.CenterScreen();
					galaxyScreen.LoadScreen();
					screenInterface = galaxyScreen;
					taskBar.SetToScreen(ScreenEnum.Galaxy);
					break;
				case ScreenEnum.InGameMenu:
					if (inGameMenu == null)
					{
						inGameMenu = new InGameMenu();
						inGameMenu.Initialize(this);
					}
					screenInterface = inGameMenu;
					taskBar.SetToScreen(ScreenEnum.InGameMenu);
					break;
				case ScreenEnum.Diplomacy:
					if (diplomacyScreen == null)
					{
						diplomacyScreen = new DiplomacyScreen();
						diplomacyScreen.Initialize(this);
					}
					diplomacyScreen.SetupScreen();
					screenInterface = diplomacyScreen;
					taskBar.SetToScreen(ScreenEnum.Diplomacy);
					break;
				case ScreenEnum.FleetList:
					if (fleetListScreen == null)
					{
						fleetListScreen = new FleetListScreen();
						fleetListScreen.Initialize(this);
					}
					fleetListScreen.LoadScreen();
					screenInterface = fleetListScreen;
					taskBar.SetToScreen(ScreenEnum.FleetList);
					break;
				case ScreenEnum.Design:
					if (designScreen == null)
					{
						designScreen = new DesignScreen();
						designScreen.Initialize(this);
					}
					screenInterface = designScreen;
					designScreen.LoadScreen();
					taskBar.SetToScreen(ScreenEnum.Design);
					break;
				case ScreenEnum.Production:
					if (productionScreen == null)
					{
						productionScreen = new ProductionScreen();
						productionScreen.Initialize(this);
					}
					productionScreen.Load();
					screenInterface = productionScreen;
					taskBar.SetToScreen(ScreenEnum.Production);
					break;
				case ScreenEnum.Planets:
					if (planetsScreen == null)
					{
						planetsScreen = new PlanetsScreen();
						planetsScreen.Initialize(this);
					}
					screenInterface = planetsScreen;
					planetsScreen.LoadScreen();
					taskBar.SetToScreen(ScreenEnum.Planets);
					break;
				case ScreenEnum.Research:
					if (researchScreen == null)
					{
						researchScreen = new ResearchScreen();
						researchScreen.Initialize(this);
					}
					researchScreen.LoadPoints();
					screenInterface = researchScreen;
					taskBar.SetToScreen(ScreenEnum.Research);
					break;
				case ScreenEnum.ProcessTurn:
					empireManager.CurrentEmpire.ClearTurnData();
					if (processingTurnScreen == null)
					{
						processingTurnScreen = new ProcessingTurnScreen();
						processingTurnScreen.Initialize(this);
					}
					if (!empireManager.ProcessNextEmpire())
					{
						situationReport.Refresh();
						ChangeToScreen(ScreenEnum.Galaxy);
						break;
					}
					taskBar.SetToScreen(ScreenEnum.ProcessTurn);
					screenInterface = processingTurnScreen;
					taskBar.Hide = true;
					break;
				case ScreenEnum.Battle:
					if (spaceCombat == null)
					{
						spaceCombat = new SpaceCombat();
						spaceCombat.Initialize(this);
					}
					screenInterface = spaceCombat;
					break;
				case ScreenEnum.Colonize:
					if (colonizeScreen == null)
					{
						colonizeScreen = new ColonizeScreen();
						colonizeScreen.Initialize(this);
					}
					colonizeScreen.LoadScreen(empireManager.ColonizersToProcess);
					screenInterface = colonizeScreen;
					break;
				case ScreenEnum.Invade:
					if (invadeScreen == null)
					{
						invadeScreen = new InvadeScreen();
						invadeScreen.Initialize(this);
					}
					invadeScreen.LoadScreen(empireManager.InvadersToProcess);
					screenInterface = invadeScreen;
					break;
			}
			currentScreen = whichScreen;
		}

		public void CenterGalaxyScreen(Point point)
		{
			galaxyScreen.CenterScreenToPoint(point);
		}

		public void DrawGalaxyBackground()
		{
			galaxyScreen.DrawGalaxyBackground(DrawingManagement);
		}
		public void UpdateGalaxyBackground(float frameDeltaTime)
		{
			galaxyScreen.UpdateBackground(frameDeltaTime);
		}
		public bool LoadTutorial(string filePath, out string reason)
		{
			if (tutorialWindow == null)
			{
				tutorialWindow = new TutorialWindow(this, DrawingManagement.GetFont("Computer"));
			}
			return tutorialWindow.LoadTutorial(filePath, out reason);
		}
		public void LoadBattle(string filePath)
		{
			if (spaceCombat == null)
			{
				spaceCombat = new SpaceCombat();
				spaceCombat.Initialize(this);
			}
			spaceCombat.LoadBattle(filePath);
		}
		public void RefreshSitRep()
		{
			situationReport.Refresh();
		}
		public void HideSitRep()
		{
			situationReport.Hide();
		}
		public void InitializeSitRep()
		{
			situationReport.Initialize(this);
		}

		public void Log(string message)
		{
			if (!string.IsNullOrEmpty(logFilePath))
			{
				using (StreamWriter writer = new StreamWriter(logFilePath, true))
				{
					writer.WriteLine(message);
				}
			}
			/*if (logger != null)
			{
				try
				{
					logger.WriteLine(message);
				}
				catch
				{
					try
					{
						logger.WriteLine(message);
					}
					catch { } //Give up
				}
			}*/
		}

		public void ExitGame()
		{
			//logger.Close();
			//dispose of any resources in use
			parentForm.Close();
		}
	}
}
