using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	class TaskBar
	{
		GameMain gameMain;
		BBButton[] TaskButtons;

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
			this.gameMain = gameMain;
			TaskButtons = new BBButton[9];

			left = (gameMain.ScreenWidth / 2) - 180;
			top = gameMain.ScreenHeight - 40;

			TaskButtons[0] = new BBButton();
			if (!TaskButtons[0].Initialize("GameMenuBG", "GameMenuFG", string.Empty, left, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[1] = new BBButton();
			if (!TaskButtons[1].Initialize("GalaxyBG", "GalaxyFG", string.Empty, left + 40, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[2] = new BBButton();
			if (!TaskButtons[2].Initialize("DiplomacyBG", "DiplomacyFG", string.Empty, left + 80, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[3] = new BBButton();
			if (!TaskButtons[3].Initialize("FleetListBG", "FleetListFG", string.Empty, left + 120, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[4] = new BBButton();
			if (!TaskButtons[4].Initialize("DesignBG", "DesignFG", string.Empty, left + 160, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[5] = new BBButton();
			if (!TaskButtons[5].Initialize("FleetListBG", "FleetListFG", string.Empty, left + 200, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[6] = new BBButton();
			if (!TaskButtons[6].Initialize("PlanetsListBG", "PlanetsListFG", string.Empty, left + 240, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[7] = new BBButton();
			if (!TaskButtons[7].Initialize("ResearchBG", "ResearchFG", string.Empty, left + 280, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}
			TaskButtons[8] = new BBButton();
			if (!TaskButtons[8].Initialize("EndTurnBG", "EndTurnFG", string.Empty, left + 320, top, 40, 40, gameMain.Random, out reason))
			{
				return false;
			}

			TaskButtons[1].Selected = true;
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
			foreach (BBButton button in TaskButtons)
			{
				button.Draw();
			}
		}

		public bool Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (hide)
			{
				return false;
			}
			foreach (BBButton button in TaskButtons)
			{
				button.UpdateHovering(mouseX, mouseY, frameDeltaTime);
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
			foreach (BBButton button in TaskButtons)
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
			for (int i = 0; i < TaskButtons.Length; i++)
			{
				if (TaskButtons[i].MouseUp(mouseX, mouseY))
				{
					switch (i)
					{
						case 0: gameMain.ChangeToScreen(Screen.InGameMenu);
							break;
						case 1: gameMain.ChangeToScreen(Screen.Galaxy);
							break;
						case 2: gameMain.ChangeToScreen(Screen.Diplomacy);
							break;
						case 3: gameMain.ChangeToScreen(Screen.FleetList);
							break;
						case 4: gameMain.ChangeToScreen(Screen.Design);
							break;
						case 6: gameMain.ChangeToScreen(Screen.Planets);
							break;
						case 7: gameMain.ChangeToScreen(Screen.Research);
							break;
						case 8: gameMain.ChangeToScreen(Screen.ProcessTurn);
							gameMain.HideSitRep();
							break;
					}
					return true;
				}
			}
			return false;
		}

		public void SetToScreen(Screen whichScreen)
		{
			foreach (BBButton button in TaskButtons)
			{
				button.Selected = false;
			}
			switch (whichScreen)
			{
				case Screen.InGameMenu:
					TaskButtons[0].Selected = true;
					break;
				case Screen.Galaxy:
					TaskButtons[1].Selected = true;
					break;
				case Screen.Diplomacy:
					TaskButtons[2].Selected = true;
					break;
				case Screen.FleetList:
					TaskButtons[3].Selected = true;
					break;
				case Screen.Design:
					TaskButtons[4].Selected = true;
					break;
				case Screen.Planets:
					TaskButtons[6].Selected = true;
					break;
				case Screen.Research:
					TaskButtons[7].Selected = true;
					break;
				case Screen.ProcessTurn:
					TaskButtons[8].Selected = true;
					break;
			}
		}
	}
}
