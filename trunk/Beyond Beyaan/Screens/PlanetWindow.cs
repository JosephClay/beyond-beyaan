using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class PlanetWindow : WindowInterface
	{
		#region Constants
		private const int MAX_VISIBLE = 5;
		#endregion

		//GorgonLibrary.Graphics.Sprite planetSprite;
		private SingleLineTextBox planetName;
		private StretchableImage planetInformationBackground;
		private StretchableImage planetSpecialsBackground;
		private StretchableImage planetPopulationBackground;
		private StretchableImage planetProductionBackground;

		private TextBox planetDescription;

		//private StretchableImage slidersBackground;
		private List<StretchButton> regionButtons;
		private StretchButton planetOverviewButton;
		private List<string> outputs;
		private ScrollBar regionScrollBar;
		private int regionsVisible;
		private List<Label> regionNames;
		//private List<Label> regionTypes;

		//private List<ScrollBar> outputSliders;

		/*private StretchButton planetProjectButton;

		private StretchableImage specialsBackground;
		private StretchableImage informationBackground;
		private StretchableImage outputBackground;*/

		/*private ScrollBar[] outputScrollBars;
		private Button[] outputFieldLocks;
		private Label[] outputLabels;
		private Label totalPopLabel;
		private Label dominantPopLabel;
		private Button transferButton;
		private Label planetLabel;
		private StretchableImage popBorder;*/

		//private StretchableImage planetBackground;
		//private StretchableImage regionsBackground;

		private Planet selectedPlanet;
		private int x;
		private int y;

		//private bool showSliders; //Show only if owned

		public PlanetWindow(int x, int y, GameMain gameMain)
			: base(x, y, 500, 220, string.Empty, gameMain, false)
		{
			this.x = x;
			this.y = y - 310;
			/*totalPopLabel = new Label(0, 0);
			dominantPopLabel = new Label(0, 0);

			outputScrollBars = new ScrollBar[5];
			outputFieldLocks = new Button[5];
			outputLabels = new Label[5];

			for (int i = 0; i < 5; i++)
			{
				outputScrollBars[i] = new ScrollBar(x + 30, y + 10 + (i * 25), 16, 188, 1, 101, true, true, DrawingManagement.HorizontalSliderBar);
				outputFieldLocks[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, x + 240, y + 30 + (i * 25), 16, 16);
				outputLabels[i] = new Label(x + 260, y + 30 + (i * 25));
			}

			transferButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Transfer Population", x + 255, y + 370, 240, 25);*/
			backGroundImage = new StretchableImage(x, this.y, 300, 620, 30, 13, DrawingManagement.BoxBorderBG);
			selectedPlanet = null;
			planetName = new SingleLineTextBox(x + 10, this.y + 12, 281, 35, DrawingManagement.TextBox, gameMain.FontManager.GetDefaultFont());
			planetInformationBackground = new StretchableImage(x + 10, this.y + 48, 281, 75, 30, 13, DrawingManagement.BoxBorderBG);
			planetSpecialsBackground = new StretchableImage(x + 10, this.y + 123, 281, 40, 30, 13, DrawingManagement.BoxBorderBG);
			planetPopulationBackground = new StretchableImage(x + 10, this.y + 163, 281, 40, 30, 13, DrawingManagement.BoxBorderBG);
			planetProductionBackground = new StretchableImage(x + 10, this.y + 203, 281, 150, 30, 13, DrawingManagement.BoxBorderBG);

			planetDescription = new TextBox(x + 15, this.y + 53, 271, 63, "planetDescriptionTextBox", string.Empty, gameMain.FontManager.GetDefaultFont(), DrawingManagement.VerticalScrollBar);
			//slidersBackground = new StretchableImage(x + 291, this.y + 355, 415, 250, 30, 13, DrawingManagement.BoxBorder);
			planetOverviewButton = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, x + 10, this.y + 558, 281, 52, 30, 13, gameMain.FontManager.GetDefaultFont());

			/*planetProjectButton = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, x + 291, this.y + 340, 310, 60, 30, 13);

			specialsBackground = new StretchableImage(x + 15, this.y + 400, 275, 50, 30, 13, DrawingManagement.BoxBorder);
			informationBackground = new StretchableImage(x + 15, this.y + 450, 275, 150, 30, 13, DrawingManagement.BoxBorder);
			outputBackground = new StretchableImage(x + 291, this.y + 400, 310, 200, 30, 13, DrawingManagement.BoxBorder);*/
		}

		public void LoadPlanet(Planet planet)
		{
			selectedPlanet = planet;
			planetName.SetString(planet.Name);
			planetDescription.SetMessage(planet.PlanetType.Description);
			//planetSprite = planet.PlanetType.LargeSprite;

			regionButtons = new List<StretchButton>();
			outputs = new List<string>();
			regionScrollBar = new ScrollBar(x + 276, y + 355, 16, 168, MAX_VISIBLE, MAX_VISIBLE, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont()); 
			foreach (Region region in planet.Regions)
			{
				if (!outputs.Contains(region.RegionType.RegionTypeName))
				{
					outputs.Add(region.RegionType.RegionTypeName);
				}
			}
			if (planet.Regions.Count > MAX_VISIBLE)
			{
				regionScrollBar.SetEnabledState(true);
				regionScrollBar.SetAmountOfItems(planet.Regions.Count);
				regionsVisible = MAX_VISIBLE;
			}
			else
			{
				regionScrollBar.SetEnabledState(false);
				regionScrollBar.SetAmountOfItems(MAX_VISIBLE);
				regionsVisible = planet.Regions.Count;
			}
			for (int i = 0; i < regionsVisible; i++)
			{
				regionButtons.Add(new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, x + 10, y + 355 + (i * 40), 265, 40, 30, 13, gameMain.FontManager.GetDefaultFont()));
			}
			RefreshRegionButtons();
			/*planetLabel.SetText(selectedPlanet.Name);
			if (selectedPlanet.Owner != null)
			{
				planetLabel.SetColor(selectedPlanet.Owner.EmpireColor);
				totalPopLabel.SetText("Population: " + (int)(selectedPlanet.TotalPopulation) + "/" + (int)(selectedPlanet.PopulationMax));
				if (selectedPlanet.Races.Count > 0)
				{
					KeyValuePair<Race, int> dominantRace = selectedPlanet.GetDominantRacePop();
					dominantPopLabel.SetText("Dominant Pop: " + dominantRace.Key.RaceName + " (" + dominantRace.Value + ")");
				}
				else
				{
					dominantPopLabel.SetText("Dominant Pop: None");
				}
				showSliders = selectedPlanet.Owner == gameMain.empireManager.CurrentEmpire;
			}
			else
			{
				planetLabel.SetColor(System.Drawing.Color.White);
				totalPopLabel.SetText("Max Population: " + (int)(selectedPlanet.PopulationMax));
				KeyValuePair<Race, int> dominantRace = selectedPlanet.GetDominantRacePop();
				dominantPopLabel.SetText("Accumulated Waste: " + Utility.ConvertNumberToFourDigits(selectedPlanet.AccumulatedWaste));
				showSliders = false;
			}
			MoveWindow();
			RefreshUI();*/
		}

		private void RefreshRegionButtons()
		{
			regionNames = new List<Label>();
			//regionTypes = new List<Label>();

			for (int i = 0; i < regionsVisible; i++)
			{
				regionNames.Add(new Label("Region " + Utility.ConvertNumberToRomanNumberical(i + regionScrollBar.TopIndex + 1) + " - " + selectedPlanet.Regions[i + regionScrollBar.TopIndex].RegionType.RegionTypeName, x + 25, y + 365 + (i * 40),
					System.Drawing.Color.FromArgb((int)(selectedPlanet.Regions[i + regionScrollBar.TopIndex].RegionType.Color[0] * 255),
												  (int)(selectedPlanet.Regions[i + regionScrollBar.TopIndex].RegionType.Color[1] * 255),
												  (int)(selectedPlanet.Regions[i + regionScrollBar.TopIndex].RegionType.Color[2] * 255)), gameMain.FontManager.GetDefaultFont()));
				//regionTypes.Add(new Label(selectedPlanet.Regions[i + regionScrollBar.TopIndex].RegionType.RegionTypeName, x + 25, y + 385 + (i * 60), System.Drawing.Color.White));
			}
		}

		private void RefreshUI()
		{
			/*if (showSliders)
			{
				outputScrollBars[AGRICULTURE].TopIndex = selectedPlanet.AgricultureAmount;
				outputScrollBars[WASTE].TopIndex = selectedPlanet.EnvironmentAmount;
				outputScrollBars[COMMERCE].TopIndex = selectedPlanet.CommerceAmount;
				outputScrollBars[RESEARCH].TopIndex = selectedPlanet.ResearchAmount;
				outputScrollBars[CONSTRUCTION].TopIndex = selectedPlanet.ConstructionAmount;

				outputScrollBars[AGRICULTURE].SetEnabledState(!selectedPlanet.AgricultureLocked);
				outputScrollBars[WASTE].SetEnabledState(!selectedPlanet.EnvironmentLocked);
				outputScrollBars[COMMERCE].SetEnabledState(!selectedPlanet.CommerceLocked);
				outputScrollBars[RESEARCH].SetEnabledState(!selectedPlanet.ResearchLocked);
				outputScrollBars[CONSTRUCTION].SetEnabledState(!selectedPlanet.ConstructionLocked);

				outputFieldLocks[AGRICULTURE].Selected = selectedPlanet.AgricultureLocked;
				outputFieldLocks[WASTE].Selected = selectedPlanet.EnvironmentLocked;
				outputFieldLocks[COMMERCE].Selected = selectedPlanet.CommerceLocked;
				outputFieldLocks[RESEARCH].Selected = selectedPlanet.ResearchLocked;
				outputFieldLocks[CONSTRUCTION].Selected = selectedPlanet.ConstructionLocked;

				outputLabels[AGRICULTURE].SetText(selectedPlanet.AgricultureStringOutput);
				outputLabels[WASTE].SetText(selectedPlanet.EnvironmentStringOutput);
				outputLabels[COMMERCE].SetText(String.Format("{0:0.00} BC", selectedPlanet.CommerceOutput));
				outputLabels[RESEARCH].SetText(String.Format("{0:0.00} RP", selectedPlanet.ResearchOutput));
				outputLabels[CONSTRUCTION].SetText(String.Format("{0:0.00} PP", selectedPlanet.ConstructionOutput));

				gameMain.taskBar.UpdateDisplays();
			}
			else
			{
				planetDescription.SetMessage(selectedPlanet.PlanetType.Description);
			}*/
		}

		public override void MoveWindow()
		{
			/*if (xPos < 0)
			{
				xPos = 0;
			}
			if (xPos + 500 > gameMain.ScreenWidth)
			{
				xPos = gameMain.ScreenWidth - 500;
			}
			backGroundImage.MoveTo(xPos, yPos);
			if (showSliders)
			{
				for (int i = 0; i < 5; i++)
				{
					outputScrollBars[i].MoveTo(xPos + 35, yPos + 30 + (i * 25));
					outputFieldLocks[i].MoveTo(xPos + 260, yPos + 30 + (i * 25));
					outputLabels[i].MoveTo(xPos + 280, yPos + 30 + (i * 25));
				}
			}
			else
			{
				planetDescription.MoveTo(xPos + 35, yPos + 30);
			}
			planetLabel.MoveTo((int)(xPos + 250 - (planetLabel.GetWidth() / 2)), yPos + 5);

			totalPopLabel.MoveTo(xPos + 25, yPos + 155);
			dominantPopLabel.MoveTo(xPos + 25, yPos + 175);
			popBorder.MoveTo(xPos + 20, yPos + 150);*/
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);
			//planetSprite.SetPosition(x + 25, y + 59);
			//planetSprite.Draw();
			planetOverviewButton.Draw(drawingManagement);

			planetName.Draw(drawingManagement);
			planetInformationBackground.Draw(drawingManagement);
			planetSpecialsBackground.Draw(drawingManagement);
			planetPopulationBackground.Draw(drawingManagement);
			planetProductionBackground.Draw(drawingManagement);

			planetDescription.Draw(drawingManagement);

			regionScrollBar.Draw(drawingManagement);
			for (int i = 0; i < regionsVisible; i++)
			{
				regionButtons[i].Draw(drawingManagement);
				regionNames[i].Draw();
				//regionTypes[i].Draw();
			}

			//slidersBackground.Draw(drawingManagement);

			/*planetProjectButton.Draw(drawingManagement);

			specialsBackground.Draw(drawingManagement);
			informationBackground.Draw(drawingManagement);
			outputBackground.Draw(drawingManagement);*/
			/*if (showSliders)
			{
				//Draw the icons
				drawingManagement.DrawSprite(SpriteName.AgricultureIcon, xPos + 15, yPos + 30);
				drawingManagement.DrawSprite(SpriteName.EnvironmentIcon, xPos + 15, yPos + 55);
				drawingManagement.DrawSprite(SpriteName.CommerceIcon, xPos + 15, yPos + 80);
				drawingManagement.DrawSprite(SpriteName.ResearchIcon, xPos + 15, yPos + 105);
				drawingManagement.DrawSprite(SpriteName.ConstructionIcon, xPos + 15, yPos + 130);

				for (int i = 0; i < 5; i++)
				{
					outputScrollBars[i].Draw(drawingManagement);
					outputFieldLocks[i].Draw(drawingManagement);
					outputLabels[i].Draw();
				}
			}
			else
			{
				planetDescription.Draw(drawingManagement);
			}
			planetLabel.Draw();

			popBorder.Draw(drawingManagement);
			totalPopLabel.Draw();
			dominantPopLabel.Draw();*/
		}

		public override bool MouseDown(int x, int y)
		{
			/*bool result = false;
			foreach (ScrollBar scrollBar in outputScrollBars)
			{
				result = scrollBar.MouseDown(x, y) || result;
			}
			foreach (Button button in outputFieldLocks)
			{
				result = button.MouseDown(x, y) || result;
			}
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				result = true;
			}
			return result;*/
			return false;
		}

		public override bool MouseUp(int x, int y)
		{
			/*for (int i = 0; i < 5; i++)
			{
				if (outputScrollBars[i].MouseUp(x, y))
				{
					switch (i)
					{
						case AGRICULTURE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, outputScrollBars[i].TopIndex);
							break;
						case WASTE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, outputScrollBars[i].TopIndex);
							break;
						case COMMERCE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.COMMERCE, outputScrollBars[i].TopIndex);
							break;
						case RESEARCH:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.RESEARCH, outputScrollBars[i].TopIndex);
							break;
						case CONSTRUCTION:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, outputScrollBars[i].TopIndex);
							break;
					}
					RefreshUI();
					return true;
				}
				if (outputFieldLocks[i].MouseUp(x, y))
				{
					outputFieldLocks[i].Selected = !outputFieldLocks[i].Selected;
					switch (i)
					{
						case AGRICULTURE:
							selectedPlanet.AgricultureLocked = outputFieldLocks[i].Selected;
							break;
						case WASTE:
							selectedPlanet.EnvironmentLocked = outputFieldLocks[i].Selected;
							break;
						case COMMERCE:
							selectedPlanet.CommerceLocked = outputFieldLocks[i].Selected;
							break;
						case RESEARCH:
							selectedPlanet.ResearchLocked = outputFieldLocks[i].Selected;
							break;
						case CONSTRUCTION:
							selectedPlanet.ConstructionLocked = outputFieldLocks[i].Selected;
							break;
					}
					RefreshUI();
					return true;
				}
			}
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				return true;
			}*/
			return false;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			/*bool result = false;
			for (int i = 0; i < 5; i++)
			{
				if (outputScrollBars[i].MouseHover(x, y, frameDeltaTime))
				{
					switch (i)
					{
						case AGRICULTURE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, outputScrollBars[i].TopIndex);
							break;
						case WASTE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, outputScrollBars[i].TopIndex);
							break;
						case COMMERCE:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.COMMERCE, outputScrollBars[i].TopIndex);
							break;
						case RESEARCH:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.RESEARCH, outputScrollBars[i].TopIndex);
							break;
						case CONSTRUCTION:
							selectedPlanet.SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, outputScrollBars[i].TopIndex);
							break;
					}
					RefreshUI();
					result = true;
				}
				result = outputFieldLocks[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				result = true;
			}
			return result;*/
			return false;
		}
	}
}
