using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	class TaskBar
	{
		GameMain gameMain;
		Button[] TaskButtons;

		private int left;
		private int top;
		private bool hide;

		public bool Hide
		{
			get { return hide; }
			set { hide = value; }
		}

		public TaskBar(GameMain gameMain)
		{
			this.gameMain = gameMain;
			TaskButtons = new Button[9];

			left = (gameMain.ScreenWidth / 2) - 180;
			top = gameMain.ScreenHeight - 40;

			TaskButtons[0] = new Button(SpriteName.GameMenu, SpriteName.HighlightedGameMenu, "", left, top, 40, 40);
			TaskButtons[1] = new Button(SpriteName.Galaxy, SpriteName.HighlightedGalaxy, "", left + 40, top, 40, 40);
			TaskButtons[2] = new Button(SpriteName.Diplomacy, SpriteName.HighlightedDiplomacy, "", left + 80, top, 40, 40);
			TaskButtons[3] = new Button(SpriteName.FleetList, SpriteName.HighlightedFleetList, "", left + 120, top, 40, 40);
			TaskButtons[4] = new Button(SpriteName.Design, SpriteName.HighlightedDesign, "", left + 160, top, 40, 40);
			TaskButtons[5] = new Button(SpriteName.DesignList, SpriteName.HighlightedDesignList, "", left + 200, top, 40, 40);
			TaskButtons[6] = new Button(SpriteName.PlanetsList, SpriteName.HighlightedPlanetsList, "", left + 240, top, 40, 40);
			TaskButtons[7] = new Button(SpriteName.Research, SpriteName.HighlightedResearch, "", left + 280, top, 40, 40);
			TaskButtons[8] = new Button(SpriteName.EOT, SpriteName.HighlightedEOT, "", left + 320, top, 40, 40);

			TaskButtons[1].Selected = true;
			hide = false;
		}

		public void Draw(DrawingManagement drawingManagement)
		{
			if (hide)
			{
				return;
			}
			foreach (Button button in TaskButtons)
			{
				button.Draw(drawingManagement);
			}
		}

		public bool Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (hide)
			{
				return false;
			}
			foreach (Button button in TaskButtons)
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
			foreach (Button button in TaskButtons)
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
			foreach (Button button in TaskButtons)
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

		public void Resize()
		{
			int left = (gameMain.ScreenWidth / 2) - 140;
			int top = gameMain.ScreenHeight - 40;

			for (int i = 0; i < TaskButtons.Length; i++)
			{
				TaskButtons[i].MoveButton(left + (i * 40), top);
			}
		}
	}
}
