using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class ColonizeScreen : ScreenInterface
	{
		/*private class GroundUnit
		{
			public Point Location;
			public Race race;
			public GorgonLibrary.Graphics.Sprite sprite;
			public bool dying;
		}

		private GameMain gameMain;
		private SettlerToProcess currentSettler;
		private Button[] planetButtons;
		private ScrollBar planetScrollBar;
		private Button cancelButton;
		private Button landButton;
		private Label[] planetNameLabels;
		private Label[] planetPopLabels;
		private Label[] planetTerrainTypeLabels;
		private int x;
		private int y;
		private int maxVisible;
		private int whichPlanetSelected;
		private List<GroundUnit> defendingUnits;
		private List<GroundUnit> attackingUnits;

		private bool showingMenu;
		//private bool landing;
		private bool isGroundCombat;
		private float tickTilNextUnit;
		private float tickShowingEffect;
		private bool showingVictory;
		*/
		private GameMain gameMain;

		private ColonizeWindow colonizeWindow;
		//private List<SettlerToProcess> settlersToProcess;
		//private SettlerToProcess currentSettlerToProcess;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			colonizeWindow = new ColonizeWindow((gameMain.ScreenWidth / 2), 75, gameMain, ColonizeFunction, DoneFunction);
			/*x = (gameMain.ScreenWidth / 2) + 80;
			y = (gameMain.ScreenHeight / 2) - 161;
				 
			planetButtons = new Button[6];
			planetNameLabels = new Label[6];
			planetPopLabels = new Label[6];
			planetTerrainTypeLabels = new Label[6];
			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x, y + (47 * i), 215, 47);
				planetNameLabels[i] = new Label(x + 45, y + 2 + (i * 47));
				planetPopLabels[i] = new Label(x + 135, y + 25 + (i * 47));
				planetTerrainTypeLabels[i] = new Label(x + 45, y + 25 + (i * 47));
			}

			planetScrollBar = new ScrollBar(x + 215, y, 16, 250, 6, 10, false, false, DrawingManagement.VerticalScrollBar);

			cancelButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Cancel", x, y + 290, 100, 30);
			landButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Land", x + 130, y + 290, 100, 30);
			isGroundCombat = false;*/
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			/*gameMain.DrawGalaxyBackground();

			if (currentSettlerToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				colonizeWindow.DrawWindow(drawingManagement);
			}
			else
			{
				//Draw message that says processing AI
			}

			/*if (showingMenu)
			{
				drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 5, y - 5, 255, 244, 330, System.Drawing.Color.DarkGray);
				for (int i = 0; i < maxVisible; i++)
				{
					planetButtons[i].Draw(drawingManagement);
					drawingManagement.DrawSprite(Utility.PlanetTypeToSprite(currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].PlanetType), x + 2, y + 2 + (i * 47), 255, System.Drawing.Color.White);
					if (currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
					{
						drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].ConstructionBonus), x + 155, y + 2 + (i * 47), 255, System.Drawing.Color.White);
					}
					if (currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
					{
						drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].EnvironmentBonus), x + 175, y + 2 + (i * 47), 255, System.Drawing.Color.White);
					}
					if (currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
					{
						drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex].EntertainmentBonus), x + 195, y + 2 + (i * 47), 255, System.Drawing.Color.White);
					}
					planetTerrainTypeLabels[i].Draw();
					planetPopLabels[i].Draw();
					planetNameLabels[i].Draw();
				}
				landButton.Draw(drawingManagement);
				cancelButton.Draw(drawingManagement);
				planetScrollBar.Draw(drawingManagement);
			}
			else if (isGroundCombat || showingVictory)
			{
				x = gameMain.ScreenWidth / 2 - 400;
				y = gameMain.ScreenHeight / 2 - 300;

				drawingManagement.DrawSprite(SpriteName.TerranGround, x, y, 255, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.Colony, x + 50, y + 400, 255, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.TransportShipGround, x + 450, y + 400, 255, System.Drawing.Color.White);

				foreach (GroundUnit unit in defendingUnits)
				{
					unit.sprite.HorizontalFlip = true;
					unit.sprite.SetPosition(unit.Location.X, unit.Location.Y);
					unit.sprite.Draw();
					if (unit.dying)
					{
						drawingManagement.DrawSprite(SpriteName.GroundUnitExplosion, unit.Location.X, unit.Location.Y, 255, System.Drawing.Color.White);
					}
				}
				foreach (GroundUnit unit in attackingUnits)
				{
					unit.sprite.HorizontalFlip = false;
					unit.sprite.SetPosition(unit.Location.X, unit.Location.Y);
					unit.sprite.Draw();
					if (unit.dying)
					{
						drawingManagement.DrawSprite(SpriteName.GroundUnitExplosion, unit.Location.X, unit.Location.Y, 255, System.Drawing.Color.White);
					}
				}
			}*/
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*UpdateBackground(frameDeltaTime);

			if (currentSettlerToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				colonizeWindow.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			else
			{
				//Ai processing here
				settlersToProcess.Remove(currentSettlerToProcess);
				if (settlersToProcess.Count > 0)
				{
					currentSettlerToProcess = settlersToProcess[0];
				}
				else
				{
					//All done, change back to processing screen
					gameMain.ChangeToScreen(Screen.ProcessTurn);
				}
			}
			/*if (showingMenu)
			{
				for (int i = 0; i < maxVisible; i++)
				{
					planetButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
				}
				landButton.MouseHover(mouseX, mouseY, frameDeltaTime);
				cancelButton.MouseHover(mouseX, mouseY, frameDeltaTime);
				if (planetScrollBar.MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					LoadLabels();
				}
			}
			/*else if (landing)
			{
			}*/
			/*else if (isGroundCombat)
			{
				if (tickTilNextUnit < 0.1f)
				{
					tickTilNextUnit += frameDeltaTime;
				}
				else if (tickShowingEffect < 0)
				{
					tickShowingEffect = 0;
					Random r = new Random();
					int result = r.Next(200);
					if (result < 100)
					{
						//defending unit lost
						defendingUnits[0].dying = true;
					}
					else
					{
						//attacking unit lost
						attackingUnits[0].dying = true;
					}
				}
				else
				{
					tickShowingEffect += frameDeltaTime;
					if (tickShowingEffect >= 0.25)
					{
						tickShowingEffect = -1.0f;
						tickTilNextUnit = 0;
						if (defendingUnits[0].dying)
						{
							defendingUnits.Remove(defendingUnits[0]);
							if (defendingUnits.Count == 0)
							{
								showingVictory = true;
								isGroundCombat = false;
							}
						}
						else
						{
							attackingUnits.Remove(attackingUnits[0]);
							if (attackingUnits.Count == 0)
							{
								showingVictory = true;
								isGroundCombat = false;
							}
						}
					}
				}
			}
			else if (!showingVictory)
			{
				showingMenu = true;
				//Move to next fleet
				gameMain.empireManager.SettlersToProcess.Remove(currentSettler);
				if (gameMain.empireManager.SettlersToProcess.Count == 0)
				{
					gameMain.ChangeToScreen(Screen.ProcessTurn);
				}
				else
				{
					currentSettler = gameMain.empireManager.SettlersToProcess[0];
					LoadSettler();
				}
			}*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*if (currentSettlerToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				colonizeWindow.MouseDown(x, y);
			}
			/*if (showingMenu)
			{
				for (int i = 0; i < maxVisible; i++)
				{
					planetButtons[i].MouseDown(x, y);
				}
				landButton.MouseDown(x, y);
				cancelButton.MouseDown(x, y);
				planetScrollBar.MouseDown(x, y);
			}*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*if (currentSettlerToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				colonizeWindow.MouseUp(x, y);
			}
			/*if (showingMenu)
			{
				for (int i = 0; i < maxVisible; i++)
				{
					if (planetButtons[i].MouseUp(x, y))
					{
						foreach (Button button in planetButtons)
						{
							button.Selected = false;
						}
						planetButtons[i].Selected = true;
						whichPlanetSelected = i + planetScrollBar.TopIndex;
						if (currentSettler.whichSystem.Planets[whichPlanetSelected].PlanetType == PLANET_TYPE.ASTEROIDS ||
							currentSettler.whichSystem.Planets[whichPlanetSelected].PlanetType == PLANET_TYPE.GAS_GIANT)
						{
							landButton.Active = false;
						}
						else
						{
							landButton.Active = true;
						}
					}
				}
				if (planetScrollBar.MouseUp(x, y))
				{
					LoadLabels();
				}
				if (landButton.MouseUp(x, y))
				{
					if (whichPlanetSelected < 0)
					{
						return;
					}
					//Process landing of transport here
					if (currentSettler.whichSystem.Planets[whichPlanetSelected].Owner == null)
					{
						//Colonized!
						currentSettler.whichSystem.Planets[whichPlanetSelected].Owner = currentSettler.whichFleet.Empire;
						foreach (TransportShip ship in currentSettler.whichFleet.TransportShips)
						{
							currentSettler.whichSystem.Planets[whichPlanetSelected].AddRacePopulation(ship.raceOnShip, ship.amount);
						}
						currentSettler.whichFleet.TransportShips.Clear();
						currentSettler.whichSystem.Planets[whichPlanetSelected].SetMinimumFoodAndWaste();
						/*if (currentSettler.whichFleet.Empire.FleetManager.ShipDesigns.Count > 0)
						{
							currentSettler.whichSystem.Planets[whichPlanetSelected].ShipBeingBuilt = currentSettler.whichFleet.Empire.FleetManager.ShipDesigns[0];
						}*/
						/*currentSettler.whichSystem.UpdateOwners();
						currentSettler.whichFleet.Empire.SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, currentSettler.whichSystem, currentSettler.whichSystem.Planets[whichPlanetSelected], new Point(currentSettler.whichSystem.X, currentSettler.whichSystem.Y),
							currentSettler.whichSystem.Planets[whichPlanetSelected].Name + " has been colonized."));
						if (currentSettler.whichFleet.OrderedShips.Count == 0)
						{
							currentSettler.whichFleet.Empire.FleetManager.RemoveFleet(currentSettler.whichFleet);
						}
						showingMenu = false;
						//landing = true;
						//landingAnimation = 0;
					}
					else if (currentSettler.whichSystem.Planets[whichPlanetSelected].Owner == currentSettler.whichFleet.Empire)
					{
						//Add population to this planet
						foreach (TransportShip ship in currentSettler.whichFleet.TransportShips)
						{
							currentSettler.whichSystem.Planets[whichPlanetSelected].AddRacePopulation(ship.raceOnShip, ship.amount);
						}
						currentSettler.whichFleet.TransportShips.Clear();
						if (currentSettler.whichFleet.OrderedShips.Count == 0)
						{
							currentSettler.whichFleet.Empire.FleetManager.RemoveFleet(currentSettler.whichFleet);
						}
						showingMenu = false;
					}
					else
					{
						showingMenu = false;
						//landing = true;
						isGroundCombat = true;
						//landingAnimation = 0;
						Planet planet = currentSettler.whichSystem.Planets[whichPlanetSelected];
						defendingUnits = new List<GroundUnit>();
						attackingUnits = new List<GroundUnit>();
						int i = 0;
						int j = 0;
						x = gameMain.ScreenWidth / 2 - 400;
						y = gameMain.ScreenHeight / 2 - 300;
						foreach (Race race in planet.Races)
						{
							for (int k = 0; k < (int)planet.GetRacePopulation(race); k++)
							{
								GroundUnit unit = new GroundUnit();
								unit.race = race;
								unit.sprite = unit.race.GetGroundUnit();
								unit.Location = new Point(x + (352 - (16 * i)), y + 550 - (j * 32) + (i % 2 == 1 ? 16 : 0));
								i++;
								if (i >= 21)
								{
									i = 0;
									j++;
								}
								defendingUnits.Add(unit);
							}
						}
						i = 0;
						j = 0;
						foreach (TransportShip ship in currentSettler.whichFleet.TransportShips)
						{
							for (int k = 0; k < ship.amount; k++)
							{
								GroundUnit unit = new GroundUnit();
								unit.race = ship.raceOnShip;
								unit.sprite = unit.race.GetGroundUnit();
								unit.Location = new Point(x + 398 + (16 * i), y + 550 - (j * 32) + (i % 2 == 1 ? 16 : 0));
								i++;
								if (i >= 21)
								{
									i = 0;
									j++;
								}
								attackingUnits.Add(unit);
							}
						}
						tickShowingEffect = -1.0f;
						tickTilNextUnit = 0.0f;
						showingVictory = false;
					}
				}
				if (cancelButton.MouseUp(x, y))
				{
					//Skip to next one
					gameMain.empireManager.SettlersToProcess.Remove(currentSettler);
					if (gameMain.empireManager.SettlersToProcess.Count == 0)
					{
						gameMain.ChangeToScreen(Screen.ProcessTurn);
					}
					else
					{
						currentSettler = gameMain.empireManager.SettlersToProcess[0];
						LoadSettler();
					}
				}
			}
			else if (showingVictory)
			{
				currentSettler.whichSystem.Planets[whichPlanetSelected].RemoveAllRaces();
				if (defendingUnits.Count > 0)
				{
					foreach (GroundUnit unit in defendingUnits)
					{
						currentSettler.whichSystem.Planets[whichPlanetSelected].AddRacePopulation(unit.race, 1);
					}
				}
				else
				{
					foreach (GroundUnit unit in attackingUnits)
					{
						currentSettler.whichSystem.Planets[whichPlanetSelected].AddRacePopulation(unit.race, 1);
					}
					currentSettler.whichSystem.Planets[whichPlanetSelected].Owner.PlanetManager.Planets.Remove(currentSettler.whichSystem.Planets[whichPlanetSelected]);
					currentSettler.whichSystem.Planets[whichPlanetSelected].Owner = currentSettler.whichFleet.Empire;
					currentSettler.whichSystem.Planets[whichPlanetSelected].SetMinimumFoodAndWaste();
					//currentSettler.whichSystem.Planets[whichPlanetSelected].ShipBeingBuilt = currentSettler.whichFleet.Empire.FleetManager.ShipDesigns[0];
					currentSettler.whichSystem.UpdateOwners();
				}
				currentSettler.whichFleet.TransportShips.Clear();
				if (currentSettler.whichFleet.OrderedShips.Count == 0)
				{
					currentSettler.whichFleet.Empire.FleetManager.RemoveFleet(currentSettler.whichFleet);
				}
				gameMain.empireManager.SettlersToProcess.Remove(currentSettler);
				if (gameMain.empireManager.SettlersToProcess.Count == 0)
				{
					gameMain.ChangeToScreen(Screen.ProcessTurn);
				}
				else
				{
					currentSettler = gameMain.empireManager.SettlersToProcess[0];
					LoadSettler();
				}
				showingVictory = false;
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}

		public void Resize()
		{
		}

		public void LoadScreen(List<SettlerToProcess> settlersToProcess)
		{
			/*this.settlersToProcess = settlersToProcess;

			if (settlersToProcess.Count > 0)
			{
				currentSettlerToProcess = settlersToProcess[0];
				if (currentSettlerToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
				{
					colonizeWindow.LoadScreen(currentSettlerToProcess.whichSystem, currentSettlerToProcess.whichFleet);
					gameMain.CenterGalaxyScreen(new Point(currentSettlerToProcess.whichSystem.X, currentSettlerToProcess.whichSystem.Y - 5));
				}
				else
				{
					//Let AI process this
				}
			}*/
		}

		private void ColonizeFunction(Planet planet)
		{
			//Do nothing for now
		}
		private void DoneFunction()
		{
			/*settlersToProcess.RemoveAt(0);
			if (settlersToProcess.Count == 0)
			{
				gameMain.ChangeToScreen(Screen.ProcessTurn);
			}
			else
			{
				currentSettlerToProcess = settlersToProcess[0];
				colonizeWindow.LoadScreen(currentSettlerToProcess.whichSystem, currentSettlerToProcess.whichFleet);
			}*/
		}
		/*
		public void SetUpScreen()
		{
			currentSettler = gameMain.empireManager.SettlersToProcess[0];
			LoadSettler();
		}

		private void LoadSettler()
		{
			gameMain.CenterGalaxyScreen(new Point(currentSettler.whichFleet.GalaxyX, currentSettler.whichFleet.GalaxyY));
			//isGroundCombat = false;
			//landing = false;
			showingMenu = true;
			if (currentSettler.whichSystem.Planets.Count > 6)
			{
				maxVisible = 6;
				planetScrollBar.SetEnabledState(true);
				planetScrollBar.SetAmountOfItems(currentSettler.whichSystem.Planets.Count);
				planetScrollBar.TopIndex = 0;
			}
			else
			{
				maxVisible = currentSettler.whichSystem.Planets.Count;
				planetScrollBar.SetEnabledState(false);
				planetScrollBar.SetAmountOfItems(10);
				planetScrollBar.TopIndex = 0;
			}
			foreach (Button button in planetButtons)
			{
				button.Selected = false;
			}
			whichPlanetSelected = -1;
			LoadLabels();
		}

		private void LoadLabels()
		{
			foreach (Button button in planetButtons)
			{
				button.Selected = false;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				Planet planet = currentSettler.whichSystem.Planets[i + planetScrollBar.TopIndex];
				planetNameLabels[i].SetText(planet.Name);
				planetTerrainTypeLabels[i].SetText(planet.PlanetTypeString);
				if (planet.Owner != null)
				{
					planetNameLabels[i].SetColor(planet.Owner.EmpireColor);
					planetPopLabels[i].SetText(String.Format("{0:0}", planet.TotalPopulation) + "/" + planet.PopulationMax);
				}
				else
				{
					planetNameLabels[i].SetColor(System.Drawing.Color.White);
					if (planet.PlanetType != PLANET_TYPE.ASTEROIDS && planet.PlanetType != PLANET_TYPE.GAS_GIANT)
					{
						planetPopLabels[i].SetText(String.Format("{0:0}", planet.PopulationMax));
					}
					else
					{
						planetPopLabels[i].SetText(string.Empty);
					}
				}
				if (whichPlanetSelected == planetScrollBar.TopIndex + i)
				{
					planetButtons[i].Selected = true;
				}
			}
		}*/
	}
}
