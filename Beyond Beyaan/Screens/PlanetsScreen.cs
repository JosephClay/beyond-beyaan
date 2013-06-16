using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class PlanetsScreen : ScreenInterface
	{
		GameMain gameMain;

		List<Planet> ownedPlanets;
		List<Planet> neutralPlanets;
		List<Planet> otherPlanets;
		List<Planet> planetsShowing;

		Button ownedPlanetButton;
		Button otherPlanetButton;
		Button neutralPlanetButton;
		Button[] planetButtons; //So we can show in minimap where they are

		Label planetIncome;
		Label tradeIncome;
		Label shipExpense;
		Label espionageExpense;
		Label securityExpense;
		Label netIncome;
		Label incomeLabel;
		Label expenseLabel;
		Label planetIncomeLabel;
		Label tradeIncomeLabel;
		Label shipExpenseLabel;
		Label espionageExpenseLabel;
		Label securityExpenseLabel;
		Label netIncomeLabel;

		Label[] planetName;
		Label[] planetType;
		Label[] population;
		Label[] empireOwner;
		Label[] agriculture;
		Label[] waste;
		Label[] commerce;
		Label[] research;
		Label[] construction;

		ScrollBar scrollBar;

		private int planetIndex;
		private int selectedPlanet;
		private int maxVisible;
		private StarSystem selectedSystem;
		private StarSystem hoveringSystem;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			ownedPlanetButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Owned Planets", x + 5, y + 410, 125, 25);
			neutralPlanetButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Neutral Planets", x + 135, y + 410, 125, 25);
			otherPlanetButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Foreign Planets", x + 270, y + 410, 125, 25);

			planetIncome = new Label(string.Empty, x + 300, y + 440);
			planetIncomeLabel = new Label("Planets:", x + 200, y + 440);
			tradeIncome = new Label(string.Empty, x + 300, y + 465);
			tradeIncomeLabel = new Label("Trade:", x + 200, y + 465);
			incomeLabel = new Label("Revenues", x + 5, y + 440);

			shipExpense = new Label(string.Empty, x + 300, y + 495);
			shipExpenseLabel = new Label("Ships:", x + 200, y + 495);
			espionageExpense = new Label(string.Empty, x + 300, y + 520);
			espionageExpenseLabel = new Label("Espionage:", x + 200, y + 520);
			securityExpense = new Label(string.Empty, x + 300, y + 545);
			securityExpenseLabel = new Label("Security:", x + 200, y + 545);
			expenseLabel = new Label("Expenses", x + 5, y + 495);

			netIncome = new Label(string.Empty, x + 300, y + 575);
			netIncomeLabel = new Label("Net Income", x + 5, y + 575);

			planetButtons = new Button[6];

			planetName = new Label[planetButtons.Length];
			planetType = new Label[planetButtons.Length];
			population = new Label[planetButtons.Length];
			empireOwner = new Label[planetButtons.Length];
			agriculture = new Label[planetButtons.Length];
			waste = new Label[planetButtons.Length];
			commerce = new Label[planetButtons.Length];
			research = new Label[planetButtons.Length];
			construction = new Label[planetButtons.Length];

			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, x + 400, y + (i * 100), 384, 100);
				planetName[i] = new Label(x + 403, y + 3 + (i * 100));
				planetType[i] = new Label(x + 403, y + 28 + (i * 100));
				population[i] = new Label(x + 403, y + 53 + (i * 100));
				empireOwner[i] = new Label(x + 403, y + 78 + (i * 100));
				agriculture[i] = new Label(x + 503, y + 2 + (i * 100));
				waste[i] = new Label(x + 503, y + 21 + (i * 100));
				commerce[i] = new Label(x + 503, y + 40 + (i * 100));
				research[i] = new Label(x + 503, y + 59 + (i * 100));
				construction[i] = new Label(x + 503, y + 78 + (i * 100));
			}

			scrollBar = new ScrollBar(x + 784, y, 16, 568, 6, 10, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
				SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton,
				SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x, y, 255, 800, 600, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.Screen, x, y, 255, 399, 399, System.Drawing.Color.White);

			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i].Draw(drawingManagement);
			}

			int maxVisible = planetsShowing.Count > planetButtons.Length ? planetButtons.Length : planetsShowing.Count;

			for (int i = 0; i < maxVisible; i++)
			{
				planetName[i].Draw();
				planetType[i].Draw();
				population[i].Draw();
				empireOwner[i].Draw();
				agriculture[i].Draw();
				waste[i].Draw();
				commerce[i].Draw();
				research[i].Draw();
				construction[i].Draw();
			}

			DrawGalaxyPreview();

			ownedPlanetButton.Draw(drawingManagement);
			neutralPlanetButton.Draw(drawingManagement);
			otherPlanetButton.Draw(drawingManagement);

			planetIncome.Draw();
			planetIncomeLabel.Draw();
			incomeLabel.Draw();
			tradeIncome.Draw();
			tradeIncomeLabel.Draw();

			expenseLabel.Draw();
			shipExpenseLabel.Draw();
			shipExpense.Draw();
			espionageExpense.Draw();
			espionageExpenseLabel.Draw();
			securityExpense.Draw();
			securityExpenseLabel.Draw();

			netIncome.Draw();
			netIncomeLabel.Draw();

			scrollBar.DrawScrollBar(drawingManagement);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			ownedPlanetButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			neutralPlanetButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			otherPlanetButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);

			if (scrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
			{
				planetIndex = scrollBar.TopIndex;
				RefreshLabels();

				foreach (Button button in planetButtons)
				{
					button.Selected = false;
				}
				int newSelection = selectedPlanet - planetIndex;
				if (newSelection >= 0 && newSelection < planetButtons.Length)
				{
					planetButtons[newSelection].Selected = true;
				}
			}

			hoveringSystem = null;
			for (int i = 0; i < maxVisible; i++)
			{
				if (planetButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime))
				{
					hoveringSystem = planetsShowing[i + planetIndex].System;
				}
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			ownedPlanetButton.MouseDown(x, y);
			neutralPlanetButton.MouseDown(x, y);
			otherPlanetButton.MouseDown(x, y);
			scrollBar.MouseDown(x, y);

			for (int i = 0; i < maxVisible; i++)
			{
				planetButtons[i].MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (ownedPlanetButton.MouseUp(x, y))
			{
				ownedPlanetButton.Selected = !ownedPlanetButton.Selected;
				RefreshShowingPlanets();
			}
			if (neutralPlanetButton.MouseUp(x, y))
			{
				neutralPlanetButton.Selected = !neutralPlanetButton.Selected;
				RefreshShowingPlanets();
			}
			if (otherPlanetButton.MouseUp(x, y))
			{
				otherPlanetButton.Selected = !otherPlanetButton.Selected;
				RefreshShowingPlanets();
			}
			if (scrollBar.MouseUp(x, y))
			{
				planetIndex = scrollBar.TopIndex;
				RefreshLabels();

				foreach (Button button in planetButtons)
				{
					button.Selected = false;
				}
				int newSelection = selectedPlanet - planetIndex;
				if (newSelection >= 0 && newSelection < planetButtons.Length)
				{
					planetButtons[newSelection].Selected = true;
				}
			}

			for (int i = 0; i < maxVisible; i++)
			{
				if (planetButtons[i].MouseUp(x, y))
				{
					foreach (Button button in planetButtons)
					{
						button.Selected = false;
					}
					planetButtons[i].Selected = true;
					selectedPlanet = i + planetIndex;
					selectedSystem = planetsShowing[i + planetIndex].System;
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
			if (e.Key == KeyboardKeys.Space)
			{
				gameMain.ToggleSitRep();
			}
		}

		public void LoadScreen()
		{
			Empire currentEmpire = gameMain.EmpireManager.CurrentEmpire;
			ownedPlanets = currentEmpire.PlanetManager.Planets;
			otherPlanets = new List<Planet>();
			neutralPlanets = new List<Planet>();
			
			foreach (StarSystem system in gameMain.Galaxy.GetAllStars())
			{
				if (system.IsThisSystemExploredByEmpire(currentEmpire))
				{
					foreach (Planet planet in system.Planets)
					{
						if (planet.Owner == null)
						{
							neutralPlanets.Add(planet);
						}
						else if (planet.Owner != currentEmpire)
						{
							otherPlanets.Add(planet);
						}
					}
				}
			}

			ownedPlanetButton.Selected = true;
			otherPlanetButton.Selected = true;
			neutralPlanetButton.Selected = true;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			planetIncome.SetText(currentEmpire.EmpirePlanetIncome + " BC");
			planetIncome.Move((x + 395) - (int)planetIncome.GetWidth(), y + 440);
			tradeIncome.SetText("0 BC");
			tradeIncome.Move((x + 395) - (int)tradeIncome.GetWidth(), y + 465);

			shipExpense.SetText(currentEmpire.ShipMaintenance + " BC");
			shipExpense.Move((x + 395) - (int)shipExpense.GetWidth(), y + 495);
			espionageExpense.SetText("0 BC");
			espionageExpense.Move((x + 395) - (int)espionageExpense.GetWidth(), y + 520);
			securityExpense.SetText("0 BC");
			securityExpense.Move((x + 395) - (int)securityExpense.GetWidth(), y + 545);

			netIncome.SetText(currentEmpire.NetIncome + " BC");
			netIncome.Move((x + 395) - (int)netIncome.GetWidth(), y + 575);

			RefreshShowingPlanets();
		}

		private void RefreshShowingPlanets()
		{
			planetsShowing = new List<Planet>();
			if (ownedPlanetButton.Selected)
			{
				foreach (Planet planet in ownedPlanets)
				{
					planetsShowing.Add(planet);
				}
			}
			if (otherPlanetButton.Selected)
			{
				foreach (Planet planet in otherPlanets)
				{
					planetsShowing.Add(planet);
				}
			}
			if (neutralPlanetButton.Selected)
			{
				foreach (Planet planet in neutralPlanets)
				{
					planetsShowing.Add(planet);
				}
			}

			planetsShowing.Sort((Planet a, Planet b) => { return string.Compare(a.Name, b.Name); });
			planetIndex = 0;

			maxVisible = planetsShowing.Count > planetButtons.Length ? planetButtons.Length : planetsShowing.Count;
			if (maxVisible >= planetsShowing.Count)
			{
				scrollBar.SetEnabledState(false);
				scrollBar.SetAmountOfItems(10);
			}
			else
			{
				scrollBar.SetEnabledState(true);
				scrollBar.SetAmountOfItems(planetsShowing.Count);
			}

			RefreshLabels();
		}

		private void RefreshLabels()
		{			
			Empire empire = gameMain.EmpireManager.CurrentEmpire;

			for (int i = 0; i < maxVisible; i++)
			{
				planetName[i].SetText(planetsShowing[i + planetIndex].Name);
				planetType[i].SetText(planetsShowing[i + planetIndex].PlanetTypeString);
				if (planetsShowing[i + planetIndex].Owner != null)
				{
					population[i].SetText(planetsShowing[i + planetIndex].TotalPopulation + "/" + planetsShowing[i + planetIndex].PopulationMax);
					empireOwner[i].SetText(planetsShowing[i + planetIndex].Owner.EmpireName);
					empireOwner[i].SetColor(planetsShowing[i + planetIndex].Owner.EmpireColor);
				}
				else
				{
					population[i].SetText("Max Pop: " + planetsShowing[i + planetIndex].PopulationMax);
					empireOwner[i].SetText(string.Empty);
				}
				if (planetsShowing[i + planetIndex].Owner == empire)
				{
					agriculture[i].SetText("Infrastructure: " + planetsShowing[i + planetIndex].InfrastructureOutput);
					waste[i].SetText("Waste: " + planetsShowing[i + planetIndex].EnvironmentOutput);
					commerce[i].SetText("Defense: " + planetsShowing[i + planetIndex].DefenseOutput);
					research[i].SetText("Research: " + planetsShowing[i + planetIndex].ResearchOutput);
					construction[i].SetText("Construction: " + planetsShowing[i + planetIndex].ConstructionOutput);
				}
				else
				{
					agriculture[i].SetText(string.Empty);
					waste[i].SetText(string.Empty);
					commerce[i].SetText(string.Empty);
					research[i].SetText(string.Empty);
					construction[i].SetText(string.Empty);
				}
			}
		}

		private void DrawGalaxyPreview()
		{
			List<StarSystem> systems = gameMain.Galaxy.GetAllStars();

			foreach (StarSystem system in systems)
			{
				int x = (gameMain.ScreenWidth / 2) - 400 + (int)(386.0f * (system.X / (float)gameMain.Galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (system.Y / (float)gameMain.Galaxy.GalaxySize));

				GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
				gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				if (system == selectedSystem || system == hoveringSystem)
				{
					system.Sprite.Draw(x, y, 0.6f, 0.6f);
					//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 12 * system.Size, 12 * system.Size, System.Drawing.Color.White);
				}
				else
				{
					system.Sprite.Draw(x, y, 0.4f, 0.4f);
					//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
				}
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
		}
	}
}
