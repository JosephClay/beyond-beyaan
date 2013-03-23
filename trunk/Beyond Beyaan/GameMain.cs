using System;
using System.Windows.Forms;
using System.IO;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Screens;

namespace Beyond_Beyaan
{
	public enum Screen { MainMenu, GalaxySetup, PlayerSetup, Galaxy, InGameMenu, Diplomacy, FleetList, Design, Production, Planets, Research, ProcessTurn, Battle, Colonize, Invade };

	public class GameMain
	{
		Form parentForm;
		internal DrawingManagement DrawingManagement;
		internal SpriteManager SpriteManager;
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
		private Screen currentScreen;
		internal TaskBar taskBar;
		internal Galaxy galaxy;
		internal EmpireManager empireManager;
		internal RaceManager raceManager;
		internal ParticleManager particleManager;
		internal EffectManager effectManager;
		internal SectorTypeManager sectorTypeManager;
		internal PlanetTypeManager planetTypeManager;
		internal RegionTypeManager regionTypeManager;
		internal StarTypeManager starTypeManager;
		internal IconManager iconManager;
		internal ResourceManager resourceManager;
		internal AIManager aiManager;
		internal ShipScriptManager shipScriptManager;
		internal MasterItemManager masterItemManager;
		internal MasterTechnologyList masterTechnologyList;
		internal int ScreenWidth;
		internal int ScreenHeight;
		internal GorgonLibrary.Graphics.FXShader ShipShader;
		internal GorgonLibrary.Graphics.FXShader RegionShader;
		//internal GorgonLibrary.Graphics.FXShader StarShader;
		//internal GorgonLibrary.Graphics.FXShader BGStarShader;
		internal string GameDataSet;
		internal Random r;
		internal Input Input;

		private string logFilePath;
		//private StreamWriter logger;

		internal int Turn { get; private set; }

		public bool Initalize(int screenWidth, int screenHeight, Form parentForm, out string reason)
		{
			r = new Random();
			this.parentForm = parentForm;

			reason = string.Empty;
			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;

			ChangeToScreen(Screen.MainMenu);
			//StarShader = GorgonLibrary.Graphics.FXShader.FromFile("StarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			//BGStarShader = GorgonLibrary.Graphics.FXShader.FromFile("BGStarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			return true;
		}

		public void ClearAll()
		{
			logFilePath = Path.Combine(Path.GetTempPath(), "Beyond Beyaan");
			if (!Directory.Exists(logFilePath))
			{
				try
				{
					Directory.CreateDirectory(logFilePath);
				}
				catch
				{
					logFilePath = string.Empty;
				}
			}
			//Used when exiting back to main menu, need to clear all memory
			if (!string.IsNullOrEmpty(logFilePath))
			{
				logFilePath = Path.Combine(logFilePath, "loading.log");
				try
				{
					if (File.Exists(logFilePath))
					{
						File.Delete(logFilePath);
					}
				}
				catch
				{ }
			}
			SpriteManager = new SpriteManager();
			taskBar = null;
			empireManager = new EmpireManager();
			playerSetup = null;
			galaxySetup = null;
			aiManager = new AIManager();
			shipScriptManager = new ShipScriptManager();
			galaxyScreen = null;
			inGameMenu = null;
			masterItemManager = new MasterItemManager();
			masterTechnologyList = new MasterTechnologyList();
			starTypeManager = new StarTypeManager();
			sectorTypeManager = new SectorTypeManager();
			planetTypeManager = new PlanetTypeManager();
			regionTypeManager = new RegionTypeManager();
			particleManager = new ParticleManager();
			effectManager = new EffectManager();
			iconManager = new IconManager();
			resourceManager = new ResourceManager();
			invadeScreen = null;
			tutorialWindow = null;
			colonizeScreen = null;
			spaceCombat = null;
			galaxy = new Galaxy();
			situationReport = new SituationReport();
			raceManager = null;
			researchScreen = null;
			productionScreen = null;
			processingTurnScreen = null;
			planetsScreen = null;
			fleetListScreen = null;
			diplomacyScreen = null;
			designScreen = null;
			mainGameMenu = null;
		}

		public void LoadOrRefreshGame()
		{
			if (planetTypeManager == null)
			{
				planetTypeManager = new PlanetTypeManager();
			}
			if (sectorTypeManager == null)
			{
				sectorTypeManager = new SectorTypeManager();
			}

			if (regionTypeManager == null)
			{
				regionTypeManager = new RegionTypeManager();
			}

			if (starTypeManager == null)
			{
				starTypeManager = new StarTypeManager();
			}

			if (masterItemManager == null)
			{
				masterItemManager = new MasterItemManager();
			}

			if (masterTechnologyList == null)
			{
				masterTechnologyList = new MasterTechnologyList();
			}

			if (particleManager == null)
			{
				particleManager = new ParticleManager();
			}
			if (effectManager == null)
			{
				effectManager = new EffectManager();
			}

			if (DrawingManagement == null)
			{
				DrawingManagement = new DrawingManagement();
			}
			if (SpriteManager == null)
			{
				SpriteManager = new SpriteManager();
			}
			galaxy = new Galaxy();
			empireManager = new EmpireManager();

			if (mainGameMenu == null)
			{
				mainGameMenu = new MainGameMenu();
				mainGameMenu.Initialize(this);
			}

			screenInterface = mainGameMenu;
			currentScreen = Screen.MainMenu;

			if (taskBar == null)
			{
				taskBar = new TaskBar(this);
			}
			if (situationReport == null)
			{
				situationReport = new SituationReport();
				situationReport.Initialize(this);
			}
			if (tutorialWindow == null)
			{
				tutorialWindow = new TutorialWindow(this, DrawingManagement.GetFont("Computer"));
			}

			if (aiManager == null)
			{
				aiManager = new AIManager();
			}
			if (shipScriptManager == null)
			{
				shipScriptManager = new ShipScriptManager();
			}
			if (iconManager == null)
			{
				iconManager = new IconManager();
			}
			if (resourceManager == null)
			{
				resourceManager = new ResourceManager();
			}

			if (ShipShader == null)
			{
				ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			}
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

		public void ProcessGame(Mouse mouse, float frameDeltaTime)
		{
			bool skipUpdate = false;
			bool handleTutorial = false;
			bool handleTaskBar = false;
			if (currentScreen != Screen.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != Screen.GalaxySetup && currentScreen != Screen.PlayerSetup && currentScreen != Screen.Battle)
				{
					handleTaskBar = true;
				}
			}
			if (handleTutorial)
			{
				if (GameConfiguration.ShowTutorial && tutorialWindow.MouseHover((int)mouse.Position.X, (int)mouse.Position.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (handleTaskBar && !skipUpdate)
			{
				if (taskBar.Update((int)mouse.Position.X, (int)mouse.Position.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
				if (!skipUpdate && situationReport.Update((int)mouse.Position.X, (int)mouse.Position.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (!skipUpdate)
			{
				screenInterface.Update((int)mouse.Position.X, (int)mouse.Position.Y, frameDeltaTime);
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
			}
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
			bool handleTaskBar = false;
			bool handleTutorial = false;
			if (currentScreen != Screen.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != Screen.GalaxySetup && currentScreen != Screen.PlayerSetup && currentScreen != Screen.Battle)
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
			screenInterface.MouseDown(e.X, e.Y, whichButton);
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
			bool handleTaskBar = false;
			bool handleTutorial = false;
			if (currentScreen != Screen.MainMenu)
			{
				handleTutorial = true;
				if (currentScreen != Screen.GalaxySetup && currentScreen != Screen.PlayerSetup && currentScreen != Screen.Battle)
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
			screenInterface.MouseUp(e.X, e.Y, whichButton);
		}

		public void MouseScroll(int direction, int mouseX, int mouseY)
		{
			screenInterface.MouseScroll(direction, mouseX, mouseY);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			screenInterface.KeyDown(e);
		}

		public void ToggleSitRep()
		{
			situationReport.ToggleVisibility();
		}

		public void ChangeToScreen(Screen whichScreen)
		{
			if (screenInterface is ProcessingTurnScreen)
			{
				Turn++;
			}
			switch (whichScreen)
			{
				case Screen.MainMenu: //Any way we get here means everything needs to be cleared out
					ClearAll();
					this.DrawingManagement = new DrawingManagement();
					mainGameMenu = new MainGameMenu();
					mainGameMenu.Initialize(this);
					screenInterface = mainGameMenu;
					break;
				case Screen.GalaxySetup:
					if (galaxySetup == null)
					{
						galaxySetup = new GalaxySetup();
						galaxySetup.Initialize(this);
					}
					screenInterface = galaxySetup;
					break;
				case Screen.PlayerSetup:
					if (playerSetup == null)
					{
						playerSetup = new PlayerSetup();
						playerSetup.Initialize(this);
					}
					Turn = 1;
					screenInterface = playerSetup;
					break;
				case Screen.Galaxy:
					if (galaxyScreen == null)
					{
						galaxyScreen = new GalaxyScreen();
						galaxyScreen.Initialize(this);
					}
					taskBar.Hide = false;
					galaxyScreen.CenterScreen();
					galaxyScreen.LoadScreen();
					screenInterface = galaxyScreen;
					taskBar.SetToScreen(Screen.Galaxy);
					break;
				case Screen.InGameMenu:
					if (inGameMenu == null)
					{
						inGameMenu = new InGameMenu();
						inGameMenu.Initialize(this);
					}
					screenInterface = inGameMenu;
					taskBar.SetToScreen(Screen.InGameMenu);
					break;
				case Screen.Diplomacy:
					if (diplomacyScreen == null)
					{
						diplomacyScreen = new DiplomacyScreen();
						diplomacyScreen.Initialize(this);
					}
					diplomacyScreen.SetupScreen();
					screenInterface = diplomacyScreen;
					taskBar.SetToScreen(Screen.Diplomacy);
					break;
				case Screen.FleetList:
					if (fleetListScreen == null)
					{
						fleetListScreen = new FleetListScreen();
						fleetListScreen.Initialize(this);
					}
					fleetListScreen.LoadScreen();
					screenInterface = fleetListScreen;
					taskBar.SetToScreen(Screen.FleetList);
					break;
				case Screen.Design:
					if (designScreen == null)
					{
						designScreen = new DesignScreen();
						designScreen.Initialize(this);
					}
					screenInterface = designScreen;
					designScreen.LoadScreen();
					taskBar.SetToScreen(Screen.Design);
					break;
				case Screen.Production:
					if (productionScreen == null)
					{
						productionScreen = new ProductionScreen();
						productionScreen.Initialize(this);
					}
					productionScreen.Load();
					screenInterface = productionScreen;
					taskBar.SetToScreen(Screen.Production);
					break;
				case Screen.Planets:
					if (planetsScreen == null)
					{
						planetsScreen = new PlanetsScreen();
						planetsScreen.Initialize(this);
					}
					screenInterface = planetsScreen;
					planetsScreen.LoadScreen();
					taskBar.SetToScreen(Screen.Planets);
					break;
				case Screen.Research:
					if (researchScreen == null)
					{
						researchScreen = new ResearchScreen();
						researchScreen.Initialize(this);
					}
					researchScreen.LoadPoints();
					screenInterface = researchScreen;
					taskBar.SetToScreen(Screen.Research);
					break;
				case Screen.ProcessTurn:
					empireManager.CurrentEmpire.ClearTurnData();
					if (processingTurnScreen == null)
					{
						processingTurnScreen = new ProcessingTurnScreen();
						processingTurnScreen.Initialize(this);
					}
					if (!empireManager.ProcessNextEmpire())
					{
						situationReport.Refresh();
						ChangeToScreen(Screen.Galaxy);
						break;
					}
					taskBar.SetToScreen(Screen.ProcessTurn);
					screenInterface = processingTurnScreen;
					taskBar.Hide = true;
					break;
				case Screen.Battle:
					if (spaceCombat == null)
					{
						spaceCombat = new SpaceCombat();
						spaceCombat.Initialize(this);
					}
					screenInterface = spaceCombat;
					break;
				case Screen.Colonize:
					if (colonizeScreen == null)
					{
						colonizeScreen = new ColonizeScreen();
						colonizeScreen.Initialize(this);
					}
					colonizeScreen.LoadScreen(empireManager.ColonizersToProcess);
					screenInterface = colonizeScreen;
					break;
				case Screen.Invade:
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
