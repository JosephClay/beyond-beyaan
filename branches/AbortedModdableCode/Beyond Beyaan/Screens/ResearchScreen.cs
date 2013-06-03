using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class ResearchScreen : ScreenInterface
	{
		GameMain gameMain;

		//private float totalResearchPoints;
		private Label planetResearchPointsLabel;
		private Label tradeResearchPointsLabel;
		private Label totalResearchPointsLabel;
		private ScrollBar[] techScrollBars;
		private Button[] lockedButtons;
		private ProgressBar[] techFieldProgresses;
		private Label[] fieldLabels;
		private Label[] progressLabels;
		private Label[] etaLabels;
		//private StretchButton[] availableTechs;
		private ScrollBar availableScrollBar;

		//private int whichField;
		//private int maxVisible;

		private StretchableImage background;
		private StretchableImage researchedListBackground;
		private StretchableImage descriptionBackground;
		private StretchableImage outputBackground;

		private int xPos;
		private int yPos;

		private StretchButton[] fieldButtons;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			xPos = (gameMain.ScreenWidth / 2) - 420;
			yPos = (gameMain.ScreenHeight / 2) - 320;

			background = new StretchableImage(xPos, yPos, 840, 640, 200, 200, DrawingManagement.ScreenBorder);
			researchedListBackground = new StretchableImage(xPos + 420, yPos + 28, 395, 350, 60, 60, DrawingManagement.BorderBorder);
			descriptionBackground = new StretchableImage(xPos + 420, yPos + 378, 395, 186, 30, 13, DrawingManagement.BoxBorder);
			outputBackground = new StretchableImage(xPos + 420, yPos + 564, 395, 40, 30, 13, DrawingManagement.BoxBorder);

			fieldButtons = new StretchButton[6];
			for (int i = 0; i < fieldButtons.Length; i++)
			{
				fieldButtons[i] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + 25, yPos + 28 + (i * 96), 395, 95, 30, 13, gameMain.FontManager.GetDefaultFont());
			}

			planetResearchPointsLabel = new Label(xPos + 445, yPos + 574, gameMain.FontManager.GetDefaultFont());
			tradeResearchPointsLabel = new Label(xPos + 575, yPos + 574, gameMain.FontManager.GetDefaultFont());
			totalResearchPointsLabel = new Label(xPos + 705, yPos + 574, gameMain.FontManager.GetDefaultFont());

			techScrollBars = new ScrollBar[6];
			lockedButtons = new Button[6];
			techFieldProgresses = new ProgressBar[6];
			fieldLabels = new Label[6];
			progressLabels = new Label[6];
			etaLabels = new Label[6];
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i] = new ScrollBar(xPos + 35, yPos + 98 + (i * 96), 16, 168, 1, 100, true, true, DrawingManagement.HorizontalSliderBar, gameMain.FontManager.GetDefaultFont());
				lockedButtons[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, xPos + 240, yPos + 98 + (i * 96), 16, 16, gameMain.FontManager.GetDefaultFont());
				techFieldProgresses[i] = new ProgressBar(xPos + 260, yPos + 100 + (i * 96), 150, 16, 100, 0, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				fieldLabels[i] = new Label(xPos + 35, yPos + 38 + (i * 96), gameMain.FontManager.GetDefaultFont());
				progressLabels[i] = new Label(xPos + 35, yPos + 58 + (i * 96), gameMain.FontManager.GetDefaultFont());
				etaLabels[i] = new Label(xPos + 235, yPos + 58 + (i * 96), gameMain.FontManager.GetDefaultFont());
			}
			availableScrollBar = new ScrollBar(xPos + 780, yPos + 43, 16, 283, 9, 9, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont());
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();

			background.Draw(drawingManagement);
			researchedListBackground.Draw(drawingManagement);
			descriptionBackground.Draw(drawingManagement);
			outputBackground.Draw(drawingManagement);

			for (int i = 0; i < fieldButtons.Length; i++)
			{
				fieldButtons[i].Draw(drawingManagement);
				techScrollBars[i].Draw(drawingManagement);
				lockedButtons[i].Draw(drawingManagement);
				techFieldProgresses[i].Draw(drawingManagement);
				fieldLabels[i].Draw();
				progressLabels[i].Draw();
				etaLabels[i].Draw();
			}

			planetResearchPointsLabel.Draw();
			tradeResearchPointsLabel.Draw();
			totalResearchPointsLabel.Draw();
			drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 428, yPos + 577);
			drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 558, yPos + 577);
			drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 688, yPos + 577);

			availableScrollBar.Draw(drawingManagement);
			/*for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].Draw(drawingManagement);
			}*/
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*UpdateBackground(frameDeltaTime);
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					gameMain.empireManager.CurrentEmpire.TechnologyManager.SetPercentage(i, techScrollBars[i].TopIndex);
					SetPercentages(gameMain.empireManager.CurrentEmpire.TechnologyManager);
				}
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				lockedButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			for (int i = 0; i < fieldButtons.Length; i++)
			{
				fieldButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			if (availableScrollBar.MouseHover(mouseX, mouseY, frameDeltaTime))
			{
				RefreshResearchedTechs(gameMain.empireManager.CurrentEmpire.TechnologyManager);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i].MouseDown(x, y);
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				lockedButtons[i].MouseDown(x, y);
			}
			for (int i = 0; i < fieldButtons.Length; i++)
			{
				fieldButtons[i].MouseDown(x, y);
			}
			availableScrollBar.MouseDown(x, y);
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].MouseDown(x, y);
			}*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].MouseUp(x, y))
				{
					gameMain.empireManager.CurrentEmpire.TechnologyManager.SetPercentage(i, techScrollBars[i].TopIndex);
					SetPercentages(gameMain.empireManager.CurrentEmpire.TechnologyManager);
				}
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				if (lockedButtons[i].MouseUp(x, y))
				{
					gameMain.empireManager.CurrentEmpire.TechnologyManager.TechLocked[i] = !gameMain.empireManager.CurrentEmpire.TechnologyManager.TechLocked[i];
					techScrollBars[i].SetEnabledState(lockedButtons[i].Selected);
					lockedButtons[i].Selected = !lockedButtons[i].Selected;
				}
			}
			for (int i = 0; i < fieldButtons.Length; i++)
			{
				if (fieldButtons[i].MouseUp(x, y))
				{
					whichField = i;
					LoadResearchedTechs();
					foreach (StretchButton button in fieldButtons)
					{
						button.Selected = false;
					}
					fieldButtons[i].Selected = true;
				}
			}
			if (availableScrollBar.MouseUp(x, y))
			{
				RefreshResearchedTechs(gameMain.empireManager.CurrentEmpire.TechnologyManager);
				return;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (availableTechs[i].MouseUp(x, y))
				{
					TechnologyManager techManager = gameMain.empireManager.CurrentEmpire.TechnologyManager;

					//Update the description here
					foreach (StretchButton button in availableTechs)
					{
						button.Selected = false;
					}
					availableTechs[i].Selected = true;
				}
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void Resize()
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(ScreenEnum.Galaxy);
			}
		}

		public void LoadPoints()
		{
			/*float planetPoints = gameMain.empireManager.CurrentEmpire.EmpirePlanetResearch;
			planetResearchPointsLabel.SetText(Utility.ConvertNumberToFourDigits(planetPoints) + " RP");
			float tradePoints = gameMain.empireManager.CurrentEmpire.EmpireTradeResearch;
			tradeResearchPointsLabel.SetText(Utility.ConvertNumberToFourDigits(tradePoints) + " RP");
			totalResearchPoints = planetPoints + tradePoints;
			totalResearchPointsLabel.SetText(Utility.ConvertNumberToFourDigits(totalResearchPoints) + " RP");

			TechnologyManager techManager = gameMain.empireManager.CurrentEmpire.TechnologyManager;

			for (int i = 0; i < fieldButtons.Length; i++)
			{
				string field = string.Empty;
				switch (i)
				{
					case TechnologyManager.CONSTRUCTION: field = "Construction";
						break;
					case TechnologyManager.CHEMISTRY: field = "Chemistry";
						break;
					case TechnologyManager.ELECTRONICS: field = "Electronics";
						break;
					case TechnologyManager.ENERGY: field = "Energy";
						break;
					case TechnologyManager.METALLURGY: field = "Metallurgy";
						break;
					case TechnologyManager.PHYSICS: field = "Physics";
						break;
				}
				if (techManager.WhichTechBeingResearched[i] >= 0)
				{
					TechnologyItem item = techManager.VisibleItems[i][techManager.WhichTechBeingResearched[i]];
					fieldLabels[i].SetText(field + " - " + item.Name);
					progressLabels[i].SetText(Utility.ConvertNumberToFourDigits(techManager.AmountResearched[i]) + "/" + item.ResearchPoints + " RP");
					float rp = (techManager.TechPercentages[i] * 0.01f) * totalResearchPoints;
					int turns = (int)(((item.ResearchPoints - techManager.AmountResearched[i]) / rp) + 0.999999);
					etaLabels[i].SetText("ETA: " + turns + " turns");
					techFieldProgresses[i].SetMaxProgress(item.ResearchPoints);
					techFieldProgresses[i].SetProgress((int)techManager.AmountResearched[i]);
				}
				else
				{
					fieldLabels[i].SetText(field + " - Nothing being researched");
					progressLabels[i].SetText(string.Empty);
					etaLabels[i].SetText(string.Empty);
					techFieldProgresses[i].SetMaxProgress(100);
					techFieldProgresses[i].SetProgress(0);
					techFieldProgresses[i].SetPotentialProgress(0);
				}

				lockedButtons[i].Selected = techManager.TechLocked[i];
				techScrollBars[i].SetEnabledState(!techManager.TechLocked[i]);
			}

			SetPercentages(techManager);
			whichField = TechnologyManager.ELECTRONICS;
			foreach (StretchButton button in fieldButtons)
			{
				button.Selected = false;
			}
			fieldButtons[whichField].Selected = true;
			LoadResearchedTechs();*/
		}

		private void SetPercentages(TechnologyManager techManager)
		{
			/*for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i].TopIndex = techManager.TechPercentages[i];
				techFieldProgresses[i].SetPotentialProgress((int)((techManager.TechPercentages[i] * 0.01f) * totalResearchPoints));
			}*/
		}

		private void LoadResearchedTechs()
		{
			/*TechnologyManager techManager = gameMain.empireManager.CurrentEmpire.TechnologyManager;

			List<TechnologyItem> items = techManager.ResearchedItems[whichField];
			maxVisible = items.Count > 9 ? 9 : items.Count;
			availableScrollBar.TopIndex = 0;
			availableTechs = new StretchButton[maxVisible];

			RefreshResearchedTechs(techManager);*/
		}

		private void RefreshResearchedTechs(TechnologyManager techManager)
		{
			/*List<TechnologyItem> items = techManager.ResearchedItems[whichField];
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, items[i + availableScrollBar.TopIndex].Name, xPos + 435, yPos + 43 + (i * 35), 345, 35, 30, 13);
			}
			availableScrollBar.SetAmountOfItems(items.Count > 9 ? items.Count : 9);
			availableScrollBar.SetEnabledState(items.Count > 9);*/
		}
	}
}
