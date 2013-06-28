using System;
using System.IO;
using System.Windows.Forms;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Screens;

namespace Beyond_Beyaan
{
	public enum Screen { MainMenu, NewGame, Galaxy, InGameMenu, Diplomacy, FleetList, Design, Planets, Research, ProcessTurn, Battle };

	public class GameMain
	{
		#region Screens
		private ScreenInterface _screenInterface;
		private MainGameMenu _mainGameMenu;
		private NewGame _newGame;
		private GalaxyScreen _galaxyScreen;
		private InGameMenu _inGameMenu;
		private DiplomacyScreen _diplomacyScreen;
		private FleetListScreen _fleetListScreen;
		private DesignScreen _designScreen;
		private PlanetsScreen _planetsScreen;
		private ResearchScreen _researchScreen;
		private ProcessingTurnScreen _processingTurnScreen;
		private SpaceCombat _spaceCombat;
		private TaskBar _taskBar;
		private SituationReport _situationReport;
		private Screen _currentScreen;
		#endregion

		private Form _parentForm;

		#region Properties
		internal Random Random { get; private set; }
		internal DrawingManagement DrawingManagement { get; private set; }
		internal Galaxy Galaxy { get; private set; }
		internal EmpireManager EmpireManager { get; private set; }
		internal RaceManager RaceManager { get; private set; }
		internal AIManager AIManager { get; private set; }
		internal int ScreenWidth { get; private set; }
		internal int ScreenHeight { get; private set; }
		internal GorgonLibrary.Graphics.FXShader ShipShader { get; private set; }
		internal GorgonLibrary.Graphics.FXShader StarShader { get; private set; }
		internal DirectoryInfo GameDataSet { get; private set; }
		#endregion

		#region Mouse Stuff
		internal Point MousePos;
		private BBSprite Cursor;
		#endregion

		public bool Initalize(int screenWidth, int screenHeight, DirectoryInfo dataSet, bool showTutorial, Form parentForm, out string reason)
		{
			_parentForm = parentForm;

			Random = new Random();

			MousePos = new Point();

			ScreenWidth = screenWidth;
			ScreenHeight = screenHeight;
			GameDataSet = dataSet;

			Galaxy = new Galaxy();
			EmpireManager = new EmpireManager();

			ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
			StarShader = GorgonLibrary.Graphics.FXShader.FromFile("StarShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);

			if (!SpriteManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			if (!FontManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}
			DrawingManagement = new DrawingManagement();
			if (!DrawingManagement.LoadGraphics(Path.Combine(GameDataSet.FullName, "graphics"), out reason))
			{
				return false;
			}
			RaceManager = new RaceManager();
			if (!RaceManager.Initialize(GameDataSet, Random, out reason))
			{
				return false;
			}
			AIManager = new AIManager();
			if (!AIManager.Initialize(GameDataSet, out reason))
			{
				return false;
			}

			_mainGameMenu = new MainGameMenu();
			_mainGameMenu.Initialize(this);

			_screenInterface = _mainGameMenu;
			_currentScreen = Screen.MainMenu;

			_taskBar = new TaskBar();
			if (!_taskBar.Initialize(this, out reason))
			{
				return false;
			}
			_situationReport = new SituationReport(this);

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
			_taskBar.SetToScreen(Screen.MainMenu);
			EmpireManager = new EmpireManager();
			AIManager = new AIManager();
			RaceManager = new RaceManager();
			_situationReport.Clear();
			_newGame.Clear();
		}

		public void ProcessGame(float frameDeltaTime)
		{
			bool skipUpdate = false;
			bool handleTaskBar = false;
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_taskBar.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
				if (_situationReport.Update(MousePos.X, MousePos.Y, frameDeltaTime))
				{
					skipUpdate = true;
				}
			}
			if (!skipUpdate)
			{
				_screenInterface.Update(MousePos.X, MousePos.Y, frameDeltaTime);
			}
			_screenInterface.DrawScreen(DrawingManagement);
			if (handleTaskBar)
			{
				_taskBar.Draw();
				_situationReport.DrawSitRep(DrawingManagement);
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
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_taskBar.MouseDown(e.X, e.Y, whichButton))
				{
					return;
				}
				if (_situationReport.MouseDown(e.X, e.Y))
				{
					return;
				}
			}
			_screenInterface.MouseDown(e.X, e.Y, whichButton);
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
			if (_currentScreen != Screen.MainMenu && _currentScreen != Screen.NewGame && _currentScreen != Screen.Battle)
			{
				handleTaskBar = true;
			}
			if (handleTaskBar)
			{
				if (_taskBar.MouseUp(e.X, e.Y, whichButton))
				{
					return;
				}
				if (_situationReport.MouseUp(e.X, e.Y))
				{
					return;
				}
			}
			_screenInterface.MouseUp(e.X, e.Y, whichButton);
		}

		public void MouseScroll(int delta)
		{
			_screenInterface.MouseScroll(delta, MousePos.X, MousePos.Y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			_screenInterface.KeyDown(e);
		}

		public void ToggleSitRep()
		{
			_situationReport.ToggleVisibility();
		}

		public void ChangeToScreen(Screen whichScreen)
		{
			switch (whichScreen)
			{
				case Screen.MainMenu:
					if (_mainGameMenu == null)
					{
						_mainGameMenu = new MainGameMenu();
						_mainGameMenu.Initialize(this);
					}
					_screenInterface = _mainGameMenu;
					break;
				case Screen.NewGame:
					if (_newGame == null)
					{
						_newGame = new NewGame();
						_newGame.Initialize(this);
					}
					_screenInterface = _newGame;
					break;
				case Screen.Galaxy:
					if (_galaxyScreen == null)
					{
						_galaxyScreen = new GalaxyScreen();
						_galaxyScreen.Initialize(this);
					}
					_taskBar.Hide = false;
					_galaxyScreen.CenterScreen();
					_screenInterface = _galaxyScreen;
					_taskBar.SetToScreen(Screen.Galaxy);
					break;
				case Screen.InGameMenu:
					if (_inGameMenu == null)
					{
						_inGameMenu = new InGameMenu();
						_inGameMenu.Initialize(this);
					}
					_screenInterface = _inGameMenu;
					_taskBar.SetToScreen(Screen.InGameMenu);
					break;
				case Screen.Diplomacy:
					if (_diplomacyScreen == null)
					{
						_diplomacyScreen = new DiplomacyScreen();
						_diplomacyScreen.Initialize(this);
					}
					_diplomacyScreen.SetupScreen();
					_screenInterface = _diplomacyScreen;
					_taskBar.SetToScreen(Screen.Diplomacy);
					break;
				case Screen.FleetList:
					if (_fleetListScreen == null)
					{
						_fleetListScreen = new FleetListScreen();
						_fleetListScreen.Initialize(this);
					}
					_fleetListScreen.LoadScreen();
					_screenInterface = _fleetListScreen;
					_taskBar.SetToScreen(Screen.FleetList);
					break;
				case Screen.Design:
					if (_designScreen == null)
					{
						_designScreen = new DesignScreen();
						_designScreen.Initialize(this);
					}
					_screenInterface = _designScreen;
					_designScreen.LoadScreen();
					_taskBar.SetToScreen(Screen.Design);
					break;
				case Screen.Planets:
					if (_planetsScreen == null)
					{
						_planetsScreen = new PlanetsScreen();
						_planetsScreen.Initialize(this);
					}
					_screenInterface = _planetsScreen;
					_planetsScreen.LoadScreen();
					_taskBar.SetToScreen(Screen.Planets);
					break;
				case Screen.Research:
					EmpireManager.CurrentEmpire.UpdateResearchPoints();
					if (_researchScreen == null)
					{
						_researchScreen = new ResearchScreen();
						_researchScreen.Initialize(this);
					}
					_researchScreen.LoadPoints(EmpireManager.CurrentEmpire.ResearchPoints);
					_screenInterface = _researchScreen;
					_taskBar.SetToScreen(Screen.Research);
					break;
				case Screen.ProcessTurn:
					EmpireManager.CurrentEmpire.ClearTurnData();
					if (_processingTurnScreen == null)
					{
						_processingTurnScreen = new ProcessingTurnScreen();
						_processingTurnScreen.Initialize(this);
					}
					if (!EmpireManager.ProcessNextEmpire())
					{
						_situationReport.Refresh();
						ChangeToScreen(Screen.Galaxy);
						break;
					}
					_taskBar.SetToScreen(Screen.ProcessTurn);
					_screenInterface = _processingTurnScreen;
					_taskBar.Hide = true;
					break;
				case Screen.Battle:
					if (_spaceCombat == null)
					{
						_spaceCombat = new SpaceCombat();
						_spaceCombat.Initialize(this);
					}
					_spaceCombat.SetupScreen();
					_screenInterface = _spaceCombat;
					break;
			}
			_currentScreen = whichScreen;
		}

		public void CenterGalaxyScreen(Point point)
		{
			_galaxyScreen.CenterScreenToPoint(point);
		}

		public void DrawGalaxyBackground()
		{
			_galaxyScreen.DrawGalaxy(DrawingManagement);
		}

		public void RefreshSitRep()
		{
			_situationReport.Refresh();
		}
		public void HideSitRep()
		{
			_situationReport.Hide();
		}

		public void ExitGame()
		{
			//dispose of any resources in use
			_parentForm.Close();
		}
	}
}
