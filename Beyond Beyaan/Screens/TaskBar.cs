using System;

namespace Beyond_Beyaan
{
	public class TaskBar
	{
		private GameMain _gameMain;
		private BBButton[] _taskButtons;

		public Action ShowGameMenu;
		public Action ShowResearchScreen;
		public Action ShowShipDesignScreen;
		public Action ShowPlanetsScreen;
		public Action EndTurn;

		private int left;
		private int top;
		private bool hide;

		public bool Hide
		{
			get { return hide; }
			set { hide = value; }
		}

		#region Constructors
		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			_taskButtons = new BBButton[8];

			left = (gameMain.ScreenWidth / 2) - 160;
			top = gameMain.ScreenHeight - 40;

			_taskButtons[0] = new BBButton();
			if (!_taskButtons[0].Initialize("GameMenuBG", "GameMenuFG", string.Empty, ButtonTextAlignment.CENTER, left, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[1] = new BBButton();
			if (!_taskButtons[1].Initialize("GalaxyBG", "GalaxyFG", string.Empty, ButtonTextAlignment.CENTER, left + 40, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[2] = new BBButton();
			if (!_taskButtons[2].Initialize("DiplomacyBG", "DiplomacyFG", string.Empty, ButtonTextAlignment.CENTER, left + 80, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[3] = new BBButton();
			if (!_taskButtons[3].Initialize("FleetListBG", "FleetListFG", string.Empty, ButtonTextAlignment.CENTER, left + 120, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[4] = new BBButton();
			if (!_taskButtons[4].Initialize("DesignBG", "DesignFG", string.Empty, ButtonTextAlignment.CENTER, left + 160, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[5] = new BBButton();
			if (!_taskButtons[5].Initialize("PlanetsListBG", "PlanetsListFG", string.Empty, ButtonTextAlignment.CENTER, left + 200, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[6] = new BBButton();
			if (!_taskButtons[6].Initialize("ResearchBG", "ResearchFG", string.Empty, ButtonTextAlignment.CENTER, left + 240, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			_taskButtons[7] = new BBButton();
			if (!_taskButtons[7].Initialize("EndTurnBG", "EndTurnFG", string.Empty, ButtonTextAlignment.CENTER, left + 280, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[0].SetToolTip("TaskBarMenu", "Game Menu", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[1].SetToolTip("TaskBarGalaxy", "Galaxy Overview", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[2].SetToolTip("TaskBarDiplomacy", "Diplomacy", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[3].SetToolTip("TaskBarFleets", "Fleets Overview", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[4].SetToolTip("TaskBarDesign", "Design Ships", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[5].SetToolTip("TaskBarPlanets", "Planets Overview", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[6].SetToolTip("TaskBarResearch", "Manage Research", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_taskButtons[7].SetToolTip("TaskBarEndTurn", "End Turn", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}

			hide = false;
			reason = null;
			return true;
		}
		#endregion

		public void Draw()
		{
			if (hide)
			{
				return;
			}
			foreach (BBButton button in _taskButtons)
			{
				button.Draw();
			}
			foreach (BBButton button in _taskButtons)
			{
				button.DrawToolTip();
			}
		}

		public bool MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (hide)
			{
				return false;
			}
			foreach (BBButton button in _taskButtons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			if (mouseX >= left && mouseX < left + 360 && mouseY > top && mouseY < top + 39)
			{
				return true;
			}
			return false;
		}

		public bool MouseDown(int mouseX, int mouseY, int whichButton)
		{
			if (hide)
			{
				return false;
			}
			if (whichButton != 1)
			{
				return false;
			}
			foreach (BBButton button in _taskButtons)
			{
				if (button.MouseDown(mouseX, mouseY))
				{
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int mouseX, int mouseY, int whichButton)
		{
			if (hide)
			{
				return false;
			}
			if (whichButton != 1)
			{
				return false;
			}
			for (int i = 0; i < _taskButtons.Length; i++)
			{
				if (_taskButtons[i].MouseUp(mouseX, mouseY))
				{
					switch (i)
					{
						case 0:
						{
							if (ShowGameMenu != null)
							{
								ShowGameMenu();
								SetToScreen(Screen.InGameMenu);
							}
							break;
						}
						/*case 1: _gameMain.ChangeToScreen(Screen.Galaxy);
							break;
						case 2: _gameMain.ChangeToScreen(Screen.Diplomacy);
							break;
						case 3: _gameMain.ChangeToScreen(Screen.FleetList);
							break;*/
						case 4:
						{
							if (ShowShipDesignScreen != null)
							{
								ShowShipDesignScreen();
								SetToScreen(Screen.Design);
							}
							break;
						}
						case 5: 
							if (ShowPlanetsScreen != null)
							{
								ShowPlanetsScreen();
								SetToScreen(Screen.Planets);
							}
							break;
						case 6:
						{
							if (ShowResearchScreen != null)
							{
								ShowResearchScreen();
								SetToScreen(Screen.Research);
							}
							break;
						}
						case 7:
						{
							Clear();
							if (EndTurn != null)
							{
								EndTurn();
							}
							_gameMain.ChangeToScreen(Screen.ProcessTurn);
							_gameMain.HideSitRep();
							break;
						}
					}
					return true;
				}
			}
			return false;
		}

		public void SetToScreen(Screen whichScreen)
		{
			Clear();
			switch (whichScreen)
			{
				case Screen.InGameMenu:
					_taskButtons[0].Selected = true;
					break;
				case Screen.Galaxy:
					_taskButtons[1].Selected = true;
					break;
				case Screen.Diplomacy:
					_taskButtons[2].Selected = true;
					break;
				case Screen.FleetList:
					_taskButtons[3].Selected = true;
					break;
				case Screen.Design:
					_taskButtons[4].Selected = true;
					break;
				case Screen.Planets:
					_taskButtons[5].Selected = true;
					break;
				case Screen.Research:
					_taskButtons[6].Selected = true;
					break;
				case Screen.ProcessTurn:
					_taskButtons[7].Selected = true;
					break;
			}
		}

		public void Clear()
		{
			foreach (BBButton button in _taskButtons)
			{
				button.Selected = false;
			}
		}
	}
}
