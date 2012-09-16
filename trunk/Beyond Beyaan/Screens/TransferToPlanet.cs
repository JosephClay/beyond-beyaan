using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class TransferToPlanet : WindowInterface
	{
		public delegate void DoneFunction();
		private DoneFunction doneFunction;

		private GroundCombatScreen groundCombatScreen;
		private bool showingCombat;

		private InvisibleStretchButton[] planetButtons;
		private Label[] planetNumericNames;
		private Button transferButton;
		private Button doneButton;
		private ScrollBar[] popScrollBars;
		private Label[] popLabels;
		private ScrollBar raceScrollBar;

		private Squadron selectedFleet;
		private StarSystem selectedSystem;

		private int maxPlanetVisible;
		private int maxRaceVisible;
		private int planetOffset;

		private int selectedPlanet;

		private int[] populationToMove;
		private List<Race> races;

		private bool[] transferrable;

		public TransferToPlanet(int centerX, int centerY, GameMain gameMain, DoneFunction doneFunction)
			: base(centerX, centerY, 10, 10, string.Empty, gameMain, false)
		{
			this.doneFunction = doneFunction;

			groundCombatScreen = new GroundCombatScreen(centerX, centerY, gameMain, CombatEnded);

			popScrollBars = new ScrollBar[4];
			popLabels = new Label[popScrollBars.Length];

			for (int i = 0; i < popScrollBars.Length; i++)
			{
				popScrollBars[i] = new ScrollBar(centerX - 75, centerY + 160 + (45 * i), 16, 128, 1, 100, true, true, DrawingManagement.HorizontalSliderBar);
				popLabels[i] = new Label(centerX - 75, centerY + 132 + (45 * i));
			}

			transferButton = new Button(SpriteName.LandTroopButtonBG, SpriteName.LandTroopButtonFG, string.Empty, centerX + 10, yPos + 350, 75, 35);
			doneButton = new Button(SpriteName.PlanetDoneButtonBG, SpriteName.PlanetDoneButtonFG, string.Empty, centerX - 85, yPos + 350, 75, 35);

			transferButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Confirm landing of population", "confirmLandingOfPopulationToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			doneButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Exit", "exitConfirmLandingOfPopulationToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			raceScrollBar = new ScrollBar(centerX + 87, centerY + 132, 16, 158, 4, 4, false, false, DrawingManagement.VerticalScrollBar);

			windowHeight = 400;
			showingCombat = false;
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);

			for (int i = 0; i < maxPlanetVisible; i++)
			{
				planetButtons[i].Draw(drawingManagement);
				GorgonLibrary.Graphics.Sprite planet = selectedSystem.Planets[i].PlanetType.Sprite;
				planet.SetPosition(xPos - planetOffset + (i * 70) + 15, yPos + 35);
				if (transferrable[i])
				{
					planet.Color = System.Drawing.Color.White;
				}
				else
				{
					planet.Color = System.Drawing.Color.FromArgb(255, 75, 75, 75);
				}
				planet.Draw();
				planetNumericNames[i].Draw();
				/*if (selectedSystem.Planets[i].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(selectedSystem.Planets[i].ConstructionBonus), xPos - planetOffset + (i * 70) + 10, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(selectedSystem.Planets[i].EnvironmentBonus), xPos - planetOffset + (i * 70) + 27, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(selectedSystem.Planets[i].EntertainmentBonus), xPos - planetOffset + (i * 70) + 44, yPos + 95, 255, System.Drawing.Color.White);
				}*/
			}
			for (int i = 0; i < maxRaceVisible; i++)
			{
				popScrollBars[i].Draw(drawingManagement);
				popLabels[i].Draw();
			}
			doneButton.Draw(drawingManagement);
			transferButton.Draw(drawingManagement);
			raceScrollBar.Draw(drawingManagement);

			transferButton.DrawToolTip(drawingManagement);
			doneButton.DrawToolTip(drawingManagement);

			if (showingCombat)
			{
				groundCombatScreen.DrawWindow(drawingManagement);
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (showingCombat)
			{
				return groundCombatScreen.MouseHover(x, y, frameDeltaTime);
			}
			foreach (InvisibleStretchButton button in planetButtons)
			{
				button.MouseHover(x, y, frameDeltaTime);
			}
			for (int i = 0; i < maxRaceVisible; i++)
			{
				if (popScrollBars[i].MouseHover(x, y, frameDeltaTime))
				{
					populationToMove[i + raceScrollBar.TopIndex] = popScrollBars[i].TopIndex;
					popLabels[i].SetText(popScrollBars[i].TopIndex + " / " + (int)selectedFleet.PopulationInTransit[races[i + raceScrollBar.TopIndex]] + " " + races[i + raceScrollBar.TopIndex].RaceName);
					ValidateData();
				}
			}
			doneButton.MouseHover(x, y, frameDeltaTime);
			transferButton.MouseHover(x, y, frameDeltaTime);
			return true;
		}

		public override bool MouseDown(int x, int y)
		{
			if (showingCombat)
			{
				return groundCombatScreen.MouseDown(x, y);
			}
			foreach (InvisibleStretchButton button in planetButtons)
			{
				button.MouseDown(x, y);
			}
			for (int i = 0; i < maxRaceVisible; i++)
			{
				popScrollBars[i].MouseDown(x, y);
			}
			doneButton.MouseDown(x, y);
			transferButton.MouseDown(x, y);
			return true;
		}

		public override bool MouseUp(int x, int y)
		{
			if (showingCombat)
			{
				return groundCombatScreen.MouseUp(x, y);
			}
			for (int i = 0; i < planetButtons.Length; i++)
			{
				if (planetButtons[i].MouseUp(x, y))
				{
					selectedPlanet = i;
					foreach (InvisibleStretchButton button in planetButtons)
					{
						button.Selected = false;
					}
					planetButtons[i].Selected = true;
				}
			}
			for (int i = 0; i < maxRaceVisible; i++)
			{
				if (popScrollBars[i].MouseUp(x, y))
				{
					populationToMove[i + raceScrollBar.TopIndex] = popScrollBars[i].TopIndex;
					popLabels[i].SetText(popScrollBars[i].TopIndex + " / " + (int)selectedFleet.PopulationInTransit[races[i + raceScrollBar.TopIndex]] + " " + races[i + raceScrollBar.TopIndex].RaceName);
					ValidateData();
				}
			}
			if (doneButton.MouseUp(x, y) && doneFunction != null)
			{
				doneFunction();
			}
			if (transferButton.MouseUp(x, y))
			{
				for (int i = 0; i < races.Count; i++)
				{
					if (populationToMove[i] > 0)
					{
						selectedFleet.SubtractPeople(races[i], populationToMove[i]);
					}
				}
				Dictionary<Race, int> invaders = new Dictionary<Race,int>();
				//Add landing handling
				for (int i = 0; i < races.Count; i++)
				{
					if (populationToMove[i] > 0)
					{
						invaders.Add(races[i], populationToMove[i]);
					}
				}

				showingCombat = true;
				groundCombatScreen.LoadScreen(selectedSystem.Planets[selectedPlanet], selectedFleet.Empire, invaders);
			}
			return true;
		}

		public void LoadTransfer(StarSystem selectedSystem, Squadron selectedFleet)
		{
			this.selectedFleet = selectedFleet;
			this.selectedSystem = selectedSystem;

			maxPlanetVisible = selectedSystem.Planets.Count;
			planetOffset = (int)((maxPlanetVisible / 2.0f) * 70);

			planetButtons = new InvisibleStretchButton[maxPlanetVisible];
			planetNumericNames = new Label[maxPlanetVisible];
			transferrable = new bool[maxPlanetVisible];

			bool selected = false;
			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos - planetOffset + (70 * i), yPos + 28, 70, 100, 30, 13);
				transferrable[i] = selectedSystem.Planets[i].Owner != null;
				if (transferrable[i])
				{
					planetNumericNames[i] = new Label(selectedSystem.Planets[i].NumericName, 0, 0, selectedSystem.Planets[i].Owner.EmpireColor);
					planetNumericNames[i].MoveTo(xPos - planetOffset + (70 * i) - (int)(planetNumericNames[i].GetWidth() / 2) + 35, yPos + 73);
					if (!selected)
					{
						selectedPlanet = i;
						planetButtons[i].Selected = true;
						selected = true;
					}
				}
				else
				{
					planetNumericNames[i] = new Label(selectedSystem.Planets[i].NumericName, 0, 0, System.Drawing.Color.FromArgb(255, 75, 75, 75));
					planetNumericNames[i].MoveTo(xPos - planetOffset + (70 * i) - (int)(planetNumericNames[i].GetWidth() / 2) + 35, yPos + 73);
					planetButtons[i].Active = false;
				}
			}

			windowWidth = maxPlanetVisible * 70 + 40;
			if (windowWidth < 220)
			{
				windowWidth = 220;
			}

			backGroundImage.MoveTo(xPos - windowWidth / 2, yPos);
			backGroundImage.SetDimensions(windowWidth, windowHeight);

			RefreshData();
		}

		private void RefreshData()
		{
			races = new List<Race>();
			foreach (KeyValuePair<Race, int> race in selectedFleet.PopulationInTransit)
			{
				races.Add(race.Key);
			}
			if (races.Count == 0)
			{
				doneFunction();
			}

			populationToMove = new int[races.Count];

			if (races.Count > popScrollBars.Length)
			{
				maxRaceVisible = popScrollBars.Length;
				raceScrollBar.SetEnabledState(true);
				raceScrollBar.SetAmountOfItems(races.Count);
			}
			else
			{
				maxRaceVisible = races.Count;
				raceScrollBar.SetEnabledState(false);
				raceScrollBar.SetAmountOfItems(popScrollBars.Length);
			}
			raceScrollBar.TopIndex = 0;

			for (int i = 0; i < maxRaceVisible; i++)
			{
				popScrollBars[i].SetAmountOfItems(selectedFleet.PopulationInTransit[races[i]] + 1);
				popScrollBars[i].TopIndex = 0;
				popLabels[i].SetText("0 / " + (int)selectedFleet.PopulationInTransit[races[i]] + " " + races[i].RaceName);
			}

			ValidateData();
		}

		private void ValidateData()
		{
			for (int i = 0; i < populationToMove.Length; i++)
			{
				if (populationToMove[i] > 0)
				{
					//At least 1 unit will be transferred
					transferButton.Active = true;
					return;
				}
			}
			transferButton.Active = false;
		}

		private void CombatEnded()
		{
			showingCombat = false;

			//Now refresh
			RefreshData();
		}
	}
}
