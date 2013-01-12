using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class PlanetsScreen : ScreenInterface
	{
		private GameMain gameMain;

		private List<Planet> planetsShowing;

		private StretchButton[] planetButtons; //So we can show in minimap where they are
		private CheckBox[] filterButtons; //Which planets to filter out

		private Label[] planetName;
		private Label[] population;
		private Label[] agriculture;
		private Label[] waste;
		private Label[] commerce;
		private Label[] research;
		private Label[] construction;

		private ScrollBar scrollBar;

		private int planetIndex;
		private int selectedPlanet;
		private int maxVisible;
		private StarSystem selectedSystem;
		private StarSystem hoveringSystem;

		private StretchableImage background;
		private StretchableImage galaxyBackground;
		private StretchableImage filterBackground;
		private StretchableImage outputBackground;
		private StretchableImage empireProdBackground;

		private ScrollBar[] outputScrollbars;
		private Label[] outputLabels;
		private Button[] lockButtons;
		private Button applyButton;

		private Label totalPP;
		private Label totalBC;
		private Label totalRP;

		private float rotation;
		private int xPos;
		private int yPos;

		private bool modifyOutput;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			xPos = (gameMain.ScreenWidth / 2) - 420;
			yPos = (gameMain.ScreenHeight / 2) - 320;

			planetButtons = new StretchButton[8];

			planetName = new Label[planetButtons.Length];
			population = new Label[planetButtons.Length];
			agriculture = new Label[planetButtons.Length];
			waste = new Label[planetButtons.Length];
			commerce = new Label[planetButtons.Length];
			research = new Label[planetButtons.Length];
			construction = new Label[planetButtons.Length];

			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + 325, yPos + 25 + (i * 50), 465, 50, 30, 13);
				planetName[i] = new Label(xPos + 375, yPos + 29 + (i * 50));
				population[i] = new Label(xPos + 375, yPos + 51 + (i * 50));
				agriculture[i] = new Label(xPos + 508, yPos + 31 + (i * 50));
				waste[i] = new Label(xPos + 583, yPos + 51 + (i * 50));
				commerce[i] = new Label(xPos + 583, yPos + 31 + (i * 50));
				research[i] = new Label(xPos + 658, yPos + 31 + (i * 50));
				construction[i] = new Label(xPos + 733, yPos + 31 + (i * 50));
			}

			scrollBar = new ScrollBar(xPos + 792, yPos + 25, 16, 368, 8, 8, false, false, DrawingManagement.VerticalScrollBar);

			background = new StretchableImage(xPos, yPos, 840, 640, 200, 200, DrawingManagement.ScreenBorder);
			galaxyBackground = new StretchableImage(xPos + 25, yPos + 25, 300, 300, 60, 60, DrawingManagement.BorderBorder);
			filterBackground = new StretchableImage(xPos + 25, yPos + 325, 300, 290, 60, 60, DrawingManagement.BorderBorder);
			outputBackground = new StretchableImage(xPos + 325, yPos + 425, 490, 190, 60, 60, DrawingManagement.BorderBorder);
			empireProdBackground = new StretchableImage(xPos + 340, yPos + 555, 345, 40, 30, 13, DrawingManagement.BoxBorder);

			filterButtons = new CheckBox[12];

			filterButtons[0] = new CheckBox(DrawingManagement.RadioButton, "Owned Planets", xPos + 40, yPos + 340, 280, 20, 19, false);
			filterButtons[1] = new CheckBox(DrawingManagement.RadioButton, "Foreign Planets", xPos + 40, yPos + 361, 280, 20, 19, false);
			filterButtons[2] = new CheckBox(DrawingManagement.RadioButton, "Unowned Planets", xPos + 40, yPos + 382, 280, 20, 19, false);
			filterButtons[3] = new CheckBox(DrawingManagement.RadioButton, "Rich Planets", xPos + 40, yPos + 403, 280, 20, 19, false);
			filterButtons[4] = new CheckBox(DrawingManagement.RadioButton, "Normal Planets", xPos + 40, yPos + 424, 280, 20, 19, false);
			filterButtons[5] = new CheckBox(DrawingManagement.RadioButton, "Poor Planets", xPos + 40, yPos + 445, 280, 20, 19, false);
			filterButtons[6] = new CheckBox(DrawingManagement.RadioButton, "Exotic Planets", xPos + 40, yPos + 466, 280, 20, 19, false);
			filterButtons[7] = new CheckBox(DrawingManagement.RadioButton, "Mediocre Planets", xPos + 40, yPos + 487, 280, 20, 19, false);
			filterButtons[8] = new CheckBox(DrawingManagement.RadioButton, "Dull Planets", xPos + 40, yPos + 508, 280, 20, 19, false);
			filterButtons[9] = new CheckBox(DrawingManagement.RadioButton, "Fertile Planets", xPos + 40, yPos + 529, 280, 20, 19, false);
			filterButtons[10] = new CheckBox(DrawingManagement.RadioButton, "Regular Planets", xPos + 40, yPos + 550, 280, 20, 19, false);
			filterButtons[11] = new CheckBox(DrawingManagement.RadioButton, "Infertile Planets", xPos + 40, yPos + 571, 280, 20, 19, false);

			for (int i = 0; i < filterButtons.Length; i++)
			{
				filterButtons[i].IsChecked = true;
			}

			outputScrollbars = new ScrollBar[5];
			lockButtons = new Button[5];
			outputLabels = new Label[5];

			for (int i = 0; i < 5; i++)
			{
				outputScrollbars[i] = new ScrollBar(xPos + 360, yPos + 440 + (i * 22), 16, 268, 1, 100, true, true, DrawingManagement.HorizontalSliderBar);
				lockButtons[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, xPos + 665, yPos + 440 + (i * 22), 16, 16);
				outputLabels[i] = new Label(xPos + 685, yPos + 440 + (i * 22));
				if (i < 2)
				{
					//lock the agriculture and environment by default
					outputScrollbars[i].SetEnabledState(false);
					lockButtons[i].Selected = true;
				}
			}

			totalPP = new Label(xPos + 368, yPos + 566);
			totalBC = new Label(xPos + 478, yPos + 566);
			totalRP = new Label(xPos + 588, yPos + 566);

			applyButton = new Button(SpriteName.ApplyButtonBG, SpriteName.ApplyButtonFG, string.Empty, xPos + 695, yPos + 555, 85, 40);
			applyButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Apply percentages to selected planets", "applyPercentagesToSelectedPlanetsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			rotation = 0;
			modifyOutput = false;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();
			background.Draw(drawingManagement);
			galaxyBackground.Draw(drawingManagement);
			filterBackground.Draw(drawingManagement);
			outputBackground.Draw(drawingManagement);

			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i].Draw(drawingManagement);
			}

			int maxVisible = planetsShowing.Count > planetButtons.Length ? planetButtons.Length : planetsShowing.Count;

			for (int i = 0; i < maxVisible; i++)
			{
				GorgonLibrary.Graphics.Sprite planet = planetsShowing[i + scrollBar.TopIndex].PlanetType.Sprite;
				planet.SetPosition(xPos + 330, yPos + 30 + (50 * i));
				planet.Color = System.Drawing.Color.White;
				planet.Draw();
				drawingManagement.DrawSprite(SpriteName.AgricultureIcon, xPos + 490, yPos + 31 + (i * 50));
				drawingManagement.DrawSprite(SpriteName.CommerceIcon, xPos + 565, yPos + 31 + (i * 50));
				drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 640, yPos + 31 + (i * 50));
				drawingManagement.DrawSprite(SpriteName.ConstructionIcon, xPos + 715, yPos + 31 + (i * 50));
				drawingManagement.DrawSprite(SpriteName.EnvironmentIcon, xPos + 565, yPos + 51 + (i * 50));
				planetName[i].Draw();
				population[i].Draw();
				agriculture[i].Draw();
				waste[i].Draw();
				commerce[i].Draw();
				research[i].Draw();
				construction[i].Draw();
				/*if (planetsShowing[i + scrollBar.TopIndex].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(planetsShowing[i + scrollBar.TopIndex].ConstructionBonus), xPos + 490, yPos + 51 + (i * 50), 255, System.Drawing.Color.White);
				}
				if (planetsShowing[i + scrollBar.TopIndex].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(planetsShowing[i + scrollBar.TopIndex].EnvironmentBonus), xPos + 507, yPos + 51 + (i * 50), 255, System.Drawing.Color.White);
				}
				if (planetsShowing[i + scrollBar.TopIndex].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(planetsShowing[i + scrollBar.TopIndex].EntertainmentBonus), xPos + 524, yPos + 51 + (i * 50), 255, System.Drawing.Color.White);
				}*/
			}

			DrawGalaxyPreview(drawingManagement);

			for (int i = 0; i < filterButtons.Length; i++)
			{
				filterButtons[i].Draw(drawingManagement);
			}
			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				outputScrollbars[i].Draw(drawingManagement);
				lockButtons[i].Draw(drawingManagement);
				outputLabels[i].Draw();
			}
			drawingManagement.DrawSprite(SpriteName.AgricultureIcon, xPos + 340, yPos + 440);
			drawingManagement.DrawSprite(SpriteName.EnvironmentIcon, xPos + 340, yPos + 462);
			drawingManagement.DrawSprite(SpriteName.CommerceIcon, xPos + 340, yPos + 484);
			drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 340, yPos + 506);
			drawingManagement.DrawSprite(SpriteName.ConstructionIcon, xPos + 340, yPos + 528);

			empireProdBackground.Draw(drawingManagement);

			drawingManagement.DrawSprite(SpriteName.ConstructionIcon, xPos + 350, yPos + 568);
			drawingManagement.DrawSprite(SpriteName.CommerceIcon, xPos + 460, yPos + 568);
			drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 570, yPos + 568);

			totalPP.Draw();
			totalBC.Draw();
			totalRP.Draw();

			applyButton.Draw(drawingManagement);
			scrollBar.Draw(drawingManagement);

			applyButton.DrawToolTip(drawingManagement);
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			rotation -= frameDeltaTime * 100;
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			UpdateBackground(frameDeltaTime);

			if (scrollBar.MouseHover(mouseX, mouseY, frameDeltaTime))
			{
				planetIndex = scrollBar.TopIndex;
				RefreshLabels();

				foreach (StretchButton button in planetButtons)
				{
					button.Selected = false;
				}
				int newSelection = selectedPlanet - planetIndex;
				if (newSelection >= 0 && newSelection < planetButtons.Length)
				{
					planetButtons[newSelection].Selected = true;
				}
			}

			for (int i = 0; i < filterButtons.Length; i++)
			{
				filterButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}

			hoveringSystem = null;
			for (int i = 0; i < maxVisible; i++)
			{
				if (planetButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					hoveringSystem = planetsShowing[i + planetIndex].System;
				}
			}

			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				if (outputScrollbars[i].MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					RefreshOutputLabels();
				}
				lockButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			applyButton.MouseHover(mouseX, mouseY, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			scrollBar.MouseDown(x, y);

			for (int i = 0; i < filterButtons.Length; i++)
			{
				filterButtons[i].MouseDown(x, y);
			}

			for (int i = 0; i < maxVisible; i++)
			{
				planetButtons[i].MouseDown(x, y);
			}

			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				outputScrollbars[i].MouseDown(x, y);
				lockButtons[i].MouseDown(x, y);
			}
			applyButton.MouseDown(x, y);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < filterButtons.Length; i++)
			{
				if (filterButtons[i].MouseUp(x, y))
				{
					RefreshShowingPlanets();
				}
			}
			if (scrollBar.MouseUp(x, y))
			{
				planetIndex = scrollBar.TopIndex;
				RefreshLabels();

				foreach (StretchButton button in planetButtons)
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
					foreach (StretchButton button in planetButtons)
					{
						button.Selected = false;
					}
					planetButtons[i].Selected = true;
					selectedPlanet = i + planetIndex;
					selectedSystem = planetsShowing[i + planetIndex].System;
				}
			}
			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				if (outputScrollbars[i].MouseUp(x, y))
				{
					RefreshOutputLabels();
				}
				if (lockButtons[i].MouseUp(x, y))
				{
					lockButtons[i].Selected = !lockButtons[i].Selected;
					outputScrollbars[i].TopIndex = 0;
					outputScrollbars[i].SetEnabledState(!lockButtons[i].Selected);
					RefreshOutputLabels();
				}
			}
			if (applyButton.MouseUp(x, y))
			{
				float[] percentages = new float[5];
				float total = 0;
				for (int i = 0; i < outputScrollbars.Length; i++)
				{
					total += outputScrollbars[i].TopIndex;
				}
				for (int i = 0; i < outputScrollbars.Length; i++)
				{
					if (lockButtons[i].Selected)
					{
						percentages[i] = -1;
					}
					else
					{
						percentages[i] = outputScrollbars[i].TopIndex / total;
					}
				}
				/*foreach (Planet planet in planetsShowing)
				{
					planet.SetOutputs(percentages);
				}*/
				RefreshLabels();
				RefreshEmpireLabels();
				gameMain.taskBar.UpdateDisplays();
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void Resize()
		{
			/*int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			incomeLabel.MoveTo(x + 5, y + 440);
			planetIncome.MoveTo((x + 395) - (int)planetIncome.GetWidth(), y + 440);
			tradeIncome.MoveTo((x + 395) - (int)tradeIncome.GetWidth(), y + 465);

			expenseLabel.MoveTo(x + 5, y + 495);
			shipExpense.MoveTo((x + 395) - (int)shipExpense.GetWidth(), y + 495);
			espionageExpense.MoveTo((x + 395) - (int)espionageExpense.GetWidth(), y + 520);
			securityExpense.MoveTo((x + 395) - (int)securityExpense.GetWidth(), y + 545);

			netIncomeLabel.MoveTo(x + 5, y + 575);
			netIncome.MoveTo((x + 395) - (int)netIncome.GetWidth(), y + 575);*/
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
			RefreshEmpireLabels();
			RefreshShowingPlanets();
		}

		private void RefreshShowingPlanets()
		{
			bool showAllMineral = (!filterButtons[3].IsChecked && !filterButtons[4].IsChecked && !filterButtons[5].IsChecked);
			bool showAllFertility = (!filterButtons[9].IsChecked && !filterButtons[10].IsChecked && !filterButtons[11].IsChecked);
			bool showAllCommerce = (!filterButtons[6].IsChecked && !filterButtons[7].IsChecked && !filterButtons[8].IsChecked);
			planetsShowing = new List<Planet>();
			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			foreach (StarSystem system in gameMain.galaxy.GetAllStars())
			{
				if (!system.IsThisSystemExploredByEmpire(currentEmpire))
				{
					//Skip unexplored systems
					continue;
				}
				/*foreach (Planet planet in system.Planets)
				{
					if (!filterButtons[0].IsChecked && planet.Owner == currentEmpire)
					{
						//Don't want to see owned planets
						continue;
					}
					if (!filterButtons[1].IsChecked && (planet.Owner != currentEmpire && planet.Owner != null))
					{
						//Don't want to see foreign planets
						continue;
					}
					if (!filterButtons[2].IsChecked && planet.Owner == null)
					{
						//Don't want to see neutral planets
						continue;
					}
					/*if (!showAllMineral)
					{
						if ((planet.ConstructionBonus == PLANET_CONSTRUCTION_BONUS.COPIOUS || planet.ConstructionBonus == PLANET_CONSTRUCTION_BONUS.RICH) && !filterButtons[3].IsChecked)
						{
							//Is rich, but don't want to see rich
							continue;
						}
						if (planet.ConstructionBonus == PLANET_CONSTRUCTION_BONUS.AVERAGE && !filterButtons[4].IsChecked)
						{
							//Is normal, but don't want to see normal
							continue;
						}
						if ((planet.ConstructionBonus == PLANET_CONSTRUCTION_BONUS.POOR || planet.ConstructionBonus == PLANET_CONSTRUCTION_BONUS.DEARTH) && !filterButtons[5].IsChecked)
						{
							//Is poor, but don't want to see poor
							continue;
						}
					}
					if (!showAllCommerce)
					{
						if ((planet.EntertainmentBonus == PLANET_ENTERTAINMENT_BONUS.EXCITING || planet.EntertainmentBonus == PLANET_ENTERTAINMENT_BONUS.SENSATIONAL) && !filterButtons[6].IsChecked)
						{
							//Is exciting, but don't want to see exciting
							continue;
						}
						if (planet.EntertainmentBonus == PLANET_ENTERTAINMENT_BONUS.AVERAGE && !filterButtons[7].IsChecked)
						{
							//Is normal, but don't want to see normal
							continue;
						}
						if ((planet.EntertainmentBonus == PLANET_ENTERTAINMENT_BONUS.DULL || planet.EntertainmentBonus == PLANET_ENTERTAINMENT_BONUS.INSIPID) && !filterButtons[8].IsChecked)
						{
							//Is dull, but don't want to see dull
							continue;
						}
					}
					if (!showAllFertility)
					{
						if ((planet.EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.FERTILE || planet.EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.LUSH) && !filterButtons[9].IsChecked)
						{
							//Is exciting, but don't want to see exciting
							continue;
						}
						if (planet.EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.AVERAGE && !filterButtons[10].IsChecked)
						{
							//Is normal, but don't want to see normal
							continue;
						}
						if ((planet.EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.INFERTILE || planet.EnvironmentBonus == PLANET_ENVIRONMENT_BONUS.DESOLATE) && !filterButtons[11].IsChecked)
						{
							//Is dull, but don't want to see dull
							continue;
						}
					}*/
					//If it reaches here, it've passed all the filter conditions
					//planetsShowing.Add(planet);
				//}
			}

			planetsShowing.Sort((Planet a, Planet b) => { return string.Compare(a.Name, b.Name); });
			planetIndex = 0;

			maxVisible = planetsShowing.Count > planetButtons.Length ? planetButtons.Length : planetsShowing.Count;
			if (maxVisible >= planetsShowing.Count)
			{
				scrollBar.SetEnabledState(false);
				scrollBar.SetAmountOfItems(8);
			}
			else
			{
				scrollBar.SetEnabledState(true);
				scrollBar.SetAmountOfItems(planetsShowing.Count);
			}

			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				outputScrollbars[i].TopIndex = 0;
			}

			modifyOutput = filterButtons[0].IsChecked && !filterButtons[1].IsChecked && !filterButtons[2].IsChecked;
			applyButton.Active = modifyOutput;
			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				lockButtons[i].Active = modifyOutput;
				outputScrollbars[i].SetEnabledState(!(lockButtons[i].Active && lockButtons[i].Selected) && modifyOutput);
			}

			RefreshLabels();
		}

		private void RefreshLabels()
		{			
			Empire empire = gameMain.empireManager.CurrentEmpire;

			for (int i = 0; i < maxVisible; i++)
			{
				planetName[i].SetText(planetsShowing[i + planetIndex].Name);
				/*if (planetsShowing[i + planetIndex].Owner != null)
				{
					population[i].SetText(string.Format("{0:0}%", (planetsShowing[i].SpaceUsage / (planetsShowing[i].Regions.Count * 10)) * 100.0f) + " (" + planetsShowing[i].Regions.Count + ")");
					planetName[i].SetColor(planetsShowing[i + planetIndex].Owner.EmpireColor);
				}
				else
				{
					population[i].SetText("0% (" + planetsShowing[i].Regions.Count + ")");
					planetName[i].SetColor(System.Drawing.Color.White);
				}*/
				//if (planetsShowing[i + planetIndex].Owner == empire)
				{
					/*agriculture[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].AgricultureOutput));
					waste[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].EnvironmentOutput) + "/" + Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].AccumulatedWaste) + " Barrels");
					commerce[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].CommerceOutput));
					research[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].ResearchOutput));
					construction[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].ConstructionOutput));*/
				}
				/*else if (planetsShowing[i + planetIndex].Owner != null)
				{
					agriculture[i].SetText(" ? ");
					waste[i].SetText(" ? ");
					commerce[i].SetText(" ? ");
					research[i].SetText(" ? ");
					construction[i].SetText(" ? ");
				}
				else
				{
					agriculture[i].SetText(" - ");
					waste[i].SetText(Utility.ConvertNumberToFourDigits(planetsShowing[i + planetIndex].AccumulatedWaste) + " Barrels");
					commerce[i].SetText(" - ");
					research[i].SetText(" - ");
					construction[i].SetText(" - ");
				}*/
			}

			RefreshOutputLabels();
		}

		private void RefreshOutputLabels()
		{
			if (!modifyOutput)
			{
				foreach (Label label in outputLabels)
				{
					label.SetText(string.Empty);
				}
				return;
			}
			float total = 0;
			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				if (!lockButtons[i].Selected)
				{
					total += outputScrollbars[i].TopIndex;
				}
			}
			applyButton.Active = total != 0;
			for (int i = 0; i < outputScrollbars.Length; i++)
			{
				if (lockButtons[i].Selected)
				{
					outputLabels[i].SetText("Locked");
				}
				else
				{
					if (total == 0)
					{
						outputLabels[i].SetText("0%");
						continue;
					}
					float percentage = outputScrollbars[i].TopIndex / total;
					outputLabels[i].SetText(string.Format("{0:0}%", percentage * 100));
				}
			}
		}

		private void RefreshEmpireLabels()
		{
			totalBC.SetText(string.Format("{0} BC", Utility.ConvertNumberToFourDigits(gameMain.empireManager.CurrentEmpire.EmpirePlanetIncome)));
			totalPP.SetText(string.Format("{0} PP", Utility.ConvertNumberToFourDigits(gameMain.empireManager.CurrentEmpire.EmpireProduction)));
			totalRP.SetText(string.Format("{0} RP", Utility.ConvertNumberToFourDigits(gameMain.empireManager.CurrentEmpire.EmpirePlanetResearch)));
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = gameMain.galaxy.GetAllStars();

			foreach (StarSystem system in systems)
			{
				int x = (gameMain.ScreenWidth / 2) - 390 + (int)(276.0f * (system.X / (float)gameMain.galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 290) + (int)(276.0f * (system.Y / (float)gameMain.galaxy.GalaxySize));
				GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
				if (system.Type.Shader != null)
				{
					system.Type.Shader.Parameters["StarColor"].SetValue(system.Type.ShaderValue);
				}
				system.Type.Sprite.SetPosition(x, y);
				system.Type.Sprite.SetScale(0.1f, 0.1f);
				system.Type.Sprite.Draw();
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
			if (hoveringSystem != null)
			{
				int x = (gameMain.ScreenWidth / 2) - 389 + (int)(276.0f * ((hoveringSystem.X + (hoveringSystem.Type.Width / 64)) / (float)gameMain.galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 289) + (int)(276.0f * ((hoveringSystem.Y + (hoveringSystem.Type.Height / 64)) / (float)gameMain.galaxy.GalaxySize));
				drawingManagement.GetSprite(SpriteName.SelectedStar).Rotation = rotation;
				drawingManagement.DrawSprite(SpriteName.SelectedStar, x, y, 255, hoveringSystem.Type.Width / 8, hoveringSystem.Type.Height / 8, System.Drawing.Color.White);
			}
			else if (selectedSystem != null)
			{
				int x = (gameMain.ScreenWidth / 2) - 389 + (int)(276.0f * ((selectedSystem.X + (selectedSystem.Type.Width / 64)) / (float)gameMain.galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 289) + (int)(276.0f * ((selectedSystem.Y + (selectedSystem.Type.Height / 64)) / (float)gameMain.galaxy.GalaxySize));
				drawingManagement.GetSprite(SpriteName.SelectedStar).Rotation = rotation;
				drawingManagement.DrawSprite(SpriteName.SelectedStar, x, y, 255, selectedSystem.Type.Width / 8, selectedSystem.Type.Height / 8, System.Drawing.Color.White);
			}
		}
	}
}
