using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	class TaskBar
	{
		private GameMain gameMain;
		private Button[] TaskButtons;

		private StretchableImage incomeSummaryBackground;
		private StretchableImage turnSummaryBackground;
		private Label incomeSummary;
		private Label turnSummary;

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
			//prevScreen = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, DrawingManagement.BoxBorder, "Main Menu", "Go back to Main Menu and an extra goodness", "BackToMainMenu", gameMain.drawingManagement.GetFont("Computer"), xScreenPos + 15, yScreenPos + 560, 200, 35, 60, 13, 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[0] = new Button(SpriteName.GameMenu, SpriteName.HighlightedGameMenu, "", left, top, 40, 40);
			TaskButtons[1] = new Button(SpriteName.Galaxy, SpriteName.HighlightedGalaxy, "", left + 40, top, 40, 40);
			TaskButtons[2] = new Button(SpriteName.Diplomacy, SpriteName.HighlightedDiplomacy, "", left + 80, top, 40, 40);
			TaskButtons[3] = new Button(SpriteName.FleetList, SpriteName.HighlightedFleetList, "", left + 120, top, 40, 40);
			TaskButtons[4] = new Button(SpriteName.Design, SpriteName.HighlightedDesign, "", left + 160, top, 40, 40);
			TaskButtons[5] = new Button(SpriteName.ProductionList, SpriteName.HighlightedDesignList, "", left + 200, top, 40, 40);
			TaskButtons[6] = new Button(SpriteName.PlanetsList, SpriteName.HighlightedPlanetsList, "", left + 240, top, 40, 40);
			TaskButtons[7] = new Button(SpriteName.Research, SpriteName.HighlightedResearch, "", left + 280, top, 40, 40);
			TaskButtons[8] = new Button(SpriteName.EOT, SpriteName.HighlightedEOT, "", left + 320, top, 40, 40);

			GorgonLibrary.Graphics.Font font = DrawingManagement.GetFont("Computer");

			TaskButtons[0].SetToolTip(DrawingManagement.BoxBorderBG, font, "Game Menu", "gameMenuToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[1].SetToolTip(DrawingManagement.BoxBorderBG, font, "Galaxy Screen", "galaxyScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[2].SetToolTip(DrawingManagement.BoxBorderBG, font, "Diplomacy Screen", "diplomacyScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[3].SetToolTip(DrawingManagement.BoxBorderBG, font, "Fleet Management Screen", "fleetManagementScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[4].SetToolTip(DrawingManagement.BoxBorderBG, font, "Ship Design Screen", "shipDesignScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[5].SetToolTip(DrawingManagement.BoxBorderBG, font, "Production List Screen", "productionListScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[6].SetToolTip(DrawingManagement.BoxBorderBG, font, "Planet Management Screen", "planetManagementScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[7].SetToolTip(DrawingManagement.BoxBorderBG, font, "Research Screen", "researchScreenToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			TaskButtons[8].SetToolTip(DrawingManagement.BoxBorderBG, font, "End Turn", "endTurnToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			TaskButtons[1].Selected = true;
			hide = false;

			incomeSummaryBackground = new StretchableImage(0, gameMain.ScreenHeight - 40, 200, 40, 30, 13, DrawingManagement.BoxBorderBG);
			turnSummaryBackground = new StretchableImage(gameMain.ScreenWidth - 200, gameMain.ScreenHeight - 40, 200, 40, 30, 13, DrawingManagement.BoxBorderBG);
			incomeSummary = new Label(35, gameMain.ScreenHeight - 30);
			turnSummary = new Label(gameMain.ScreenWidth - 190, gameMain.ScreenHeight - 30);
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
			incomeSummaryBackground.Draw(drawingManagement);
			turnSummaryBackground.Draw(drawingManagement);
			drawingManagement.DrawSprite(SpriteName.MoneyIcon, 10, gameMain.ScreenHeight - 30);
			incomeSummary.Draw();
			turnSummary.Draw();
			foreach (Button button in TaskButtons)
			{
				button.DrawToolTip(drawingManagement);
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
						/*case 2: gameMain.ChangeToScreen(Screen.Diplomacy);
							break;
						case 3: gameMain.ChangeToScreen(Screen.FleetList);
							break;*/
						case 4: gameMain.ChangeToScreen(Screen.Design);
							break;
						case 5: gameMain.ChangeToScreen(Screen.Production);
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
				case Screen.Production:
					TaskButtons[5].Selected = true;
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
				TaskButtons[i].MoveTo(left + (i * 40), top);
			}
		}

		private void CheckValidTurn()
		{
			if (gameMain.empireManager.CurrentEmpire.Reserves + gameMain.empireManager.CurrentEmpire.NetIncome < 0)
			{
				TaskButtons[8].SetToolTip("Your net income and reserves are not sufficient to pay expenses for this turn.  Scrap ships or increase revenues before continuing.");
				TaskButtons[8].Active = false;
			}
			else
			{
				TaskButtons[8].SetToolTip("End Turn");
				TaskButtons[8].Active = true;
			}
		}

		public void UpdateDisplays()
		{
			gameMain.empireManager.CurrentEmpire.UpdateAll();
			float reserves = gameMain.empireManager.CurrentEmpire.Reserves;
			float income = gameMain.empireManager.CurrentEmpire.NetIncome;
			incomeSummary.SetText(Utility.ConvertNumberToFourDigits(reserves) + " (" + Utility.ConvertNumberToFourDigits(income) + ")");
			if (reserves + income < 0)
			{
				incomeSummary.SetColor(System.Drawing.Color.Red);
			}
			else if (income < 0)
			{
				incomeSummary.SetColor(System.Drawing.Color.Yellow);
			}
			else
			{
				incomeSummary.SetColor(System.Drawing.Color.Green);
			}
			turnSummary.SetText("Turn " + gameMain.Turn);
			CheckValidTurn();
		}
	}
}
