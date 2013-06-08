using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Screens;

namespace Beyond_Beyaan
{
	enum Screen { MainMenu, NewGame, Galaxy, InGameMenu, Diplomacy, FleetList, Design, Planets, Research, ProcessTurn, Battle };

	class GameMain
	{
		Form parentForm;
		internal Random Random;
		internal SpriteManager SpriteManager;
		internal DrawingManagement drawingManagement;
		private ScreenInterface screenInterface;
		private MainGameMenu mainGameMenu;
		private NewGame newGame;
		private GalaxyScreen galaxyScreen;
		private InGameMenu inGameMenu;
		private DiplomacyScreen diplomacyScreen;
		private FleetListScreen fleetListScreen;
		private DesignScreen designScreen;
		private PlanetsScreen planetsScreen;
		private ResearchScreen researchScreen;
		private ProcessingTurnScreen processingTurnScreen;
		private SpaceCombat spaceCombat;
		private TaskBar taskBar;
		private SituationReport situationReport;
		private Screen currentScreen;
		internal Galaxy galaxy;
		internal EmpireManager empireManager;
		internal RaceManager raceManager;
		internal AIManager aiManager;
		internal int ScreenWidth;
		internal int ScreenHeight;
		internal GorgonLibrary.Graphics.FXShader ShipShader;
		internal GorgonLibrary.Graphics.FXShader StarShader;

		#region Mouse Stuff
		internal Point MousePos;
		private BBSprite Cursor;
		#endregion

		public bool Initalize(int screenWidth, int screenHeight, Form parentForm, out string reason)
		{
			this.parentForm = parentForm;

			Random = new Random();

			MousePos = new Point();

			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;

			drawingManagement = new DrawingManagement();
			galaxy = new Galaxy();
			empireManager = new EmpireManager();

			mainGameMenu = new MainGameMenu();
			mainGameMenu.Initialize(this);

			screenInterface = mainGameMenu;
			currentScreen = Screen.MainMenu;

			taskBar = new TaskBar(this);
			situationReport = new SituationReport(this);

			raceManager = new RaceManager();
			aiManager = new AIManager();

			ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			StarShader = GorgonLibrary.Graphics.FXShader.FromFile("StarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);

			SpriteManager = new SpriteManager();
			if (!SpriteManager.Initialize(new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Default\\"), new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Default\\graphics\\"), out reason))
			{
				return false;
			}

			Cursor = SpriteManager.GetSprite("Cursor", Random);
			if (Cursor == null)
			{
				reason = "Cursor is not defined in sprites.xml";
				return false;
			}

			reason = string.Empty;
			return true;
		}

		public void ClearAll()
		{
			//Used when exiting out of current game (new game for example)
			taskBar.SetToScreen(Screen.MainMenu);
			empireManager = new EmpireManager();
			aiManager = new AIManager();
			raceManager = new RaceManager();
			situationReport.Clear();
			newGame.Clear();
		}

		public void Resize(int screenWidth, int screenHeight)
		{
			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;

			screenInterface.Resize();
			taskBar.Resize();
		}

		public void ProcessGame(float frameDeltaTime)
		{
			bool skipUpdate = false;
			bool handleTaskBar = false;
			if (currentScreen != Screen.MainMenu && currentScreen != Screen.NewGame && currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (taskBar.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
				if (situationReport.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (!skipUpdate)
			{
				screenInterface.Update(MousePos.X, MousePos.Y, frameDeltaTime);
			}
			screenInterface.DrawScreen(drawingManagement);
			if (handleTaskBar)
			{
				taskBar.Draw(drawingManagement);
				situationReport.DrawSitRep(drawingManagement);
			}
			Cursor.Draw(MousePos.X, MousePos.Y);
			Cursor.Update(frameDeltaTime, Random);
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
			if (currentScreen != Screen.MainMenu && currentScreen != Screen.NewGame && currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
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
			if (currentScreen != Screen.MainMenu && currentScreen != Screen.NewGame && currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
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

		public void MouseScroll(int delta)
		{
			screenInterface.MouseScroll(delta, MousePos.X, MousePos.X);
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
			switch (whichScreen)
			{
				case Screen.MainMenu:
					if (mainGameMenu == null)
					{
						mainGameMenu = new MainGameMenu();
						mainGameMenu.Initialize(this);
					}
					screenInterface = mainGameMenu;
					break;
				case Screen.NewGame:
					if (newGame == null)
					{
						newGame = new NewGame();
						newGame.Initialize(this);
					}
					screenInterface = newGame;
					break;
				case Screen.Galaxy:
					if (galaxyScreen == null)
					{
						galaxyScreen = new GalaxyScreen();
						galaxyScreen.Initialize(this);
					}
					taskBar.Hide = false;
					galaxyScreen.CenterScreen();
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
					empireManager.CurrentEmpire.UpdateResearchPoints();
					if (researchScreen == null)
					{
						researchScreen = new ResearchScreen();
						researchScreen.Initialize(this);
					}
					researchScreen.LoadPoints(empireManager.CurrentEmpire.ResearchPoints);
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
					spaceCombat.SetupScreen();
					screenInterface = spaceCombat;
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
			galaxyScreen.DrawGalaxyBackground(drawingManagement);
		}

		public void RefreshSitRep()
		{
			situationReport.Refresh();
		}
		public void HideSitRep()
		{
			situationReport.Hide();
		}

		public void ExitGame()
		{
			//dispose of any resources in use
			parentForm.Close();
		}
	}
}
