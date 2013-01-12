using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class PlayerSetup : ScreenInterface
	{
		private class Player
		{
			public string empireName;
			public Race race;
			public bool isHuman;
			public int aiScript;
			public int team;
			public System.Drawing.Color color;
		}

		#region Member Variables
		private GameMain gameMain;

		private int xPos;
		private int yPos;

		int habitableStars;

		private Race selectedRace;
		private RaceSelection raceSelection;
		private ColorSelection colorSelection;
		private int whichPlayerSelectingColor;
		private bool showingRaceSelection;
		private bool showingColorSelection;

		private List<Player> players;

		#region UI Elements
		private StretchableImage background;

		//List elements
		private StretchableImage listBackground;
		private Label[] empireInfos;
		private NumericUpDown[] teams;
		private Label[] teamLabels;
		private Button[] removeButtons;
		private Button[] colorButtons;
		private ScrollBar empireListScrollBar;
		private Label listIsEmptyLabel;
		private StretchableImage[] playerBorders;
		private int maxVisible;

		//New Player elements
		private StretchableImage newPlayerBackground;
		CheckBox humanPlayer;
		CheckBox computerPlayer;
		Label empireName;
		SingleLineTextBox empireNameTextBox;
		ComboBox aiComboBox;
		StretchableImage raceBackground;
		Label currentRaceLabel;
		StretchButton selectRaceButton;
		StretchButton addButton;

		//Back/Next buttons
		StretchButton galaxySetup;
		StretchButton startGame;

		//Color block
		private GorgonLibrary.Graphics.Sprite colorBlock;
		#endregion
		#endregion

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			habitableStars = 0;
			foreach (StarSystem system in gameMain.galaxy.GetAllStars())
			{
				if (system.Type.Inhabitable)
				{
					habitableStars++;
				}
			}

			xPos = gameMain.ScreenWidth / 2 - 400;
			yPos = gameMain.ScreenHeight / 2 - 300;

			raceSelection = new RaceSelection(xPos + 100, yPos + 50, gameMain, OnRaceOkClick, OnRaceCancelClick);
			colorSelection = new ColorSelection(xPos + 200, yPos + 50, gameMain, OnColorOkClick, OnColorCancelClick);

			showingRaceSelection = false;

			List<SpriteName> backgroundScreenSections = new List<SpriteName>
			{
				SpriteName.ScreenUL,
				SpriteName.ScreenUC,
				SpriteName.ScreenUR,
				SpriteName.ScreenCL,
				SpriteName.ScreenCC,
				SpriteName.ScreenCR,
				SpriteName.ScreenBL,
				SpriteName.ScreenBC,
				SpriteName.ScreenBR,
			};
			background = new StretchableImage(xPos - 20, yPos - 20, 840, 640, 200, 200, backgroundScreenSections);

			listBackground = new StretchableImage(xPos + 20, yPos + 20, 760, 350, 60, 60, DrawingManagement.BorderBorder);
			newPlayerBackground = new StretchableImage(xPos + 20, yPos + 375, 760, 150, 60, 60, DrawingManagement.BorderBorder);

			empireListScrollBar = new ScrollBar(xPos + 745, yPos + 35, 16, 285, 7, 20, false, false, DrawingManagement.VerticalScrollBar);
			empireListScrollBar.SetEnabledState(false);

			List<SpriteName> radioButtonImage = new List<SpriteName>
			{
				SpriteName.RBUBG,
				SpriteName.RBUFG,
				SpriteName.RBCBG,
				SpriteName.RBCFG,
			};

			humanPlayer = new CheckBox(radioButtonImage, "Human Player", xPos + 40, yPos + 395, 150, 30, 19, true);
			computerPlayer = new CheckBox(radioButtonImage, "Computer Player", xPos + 40, yPos + 425, 150, 30, 19, true);

			List<string> items = new List<string>();
			items.Add("Random");
			foreach (AI ai in gameMain.aiManager.AIs)
			{
				items.Add(ai.AIName);
			}

			aiComboBox = new ComboBox(DrawingManagement.ComboBox, items, xPos + 50, yPos + 465, 150, 35, 4, true);

			humanPlayer.IsChecked = true;
			aiComboBox.Active = false;

			currentRaceLabel = new Label("Random", xPos + 265, yPos + 415, System.Drawing.Color.White);
			selectRaceButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Select Race", xPos + 250, yPos + 465, 250, 35);
			raceBackground = new StretchableImage(xPos + 250, yPos + 400, 250, 50, 30, 13, DrawingManagement.BoxBorder);

			empireName = new Label("Empire Name:", xPos + 525, yPos + 390, System.Drawing.Color.White);
			empireNameTextBox = new SingleLineTextBox(xPos + 525, yPos + 415, 230, 35, DrawingManagement.TextBox);

			addButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Add Player", xPos + 525, yPos + 465, 230, 35);

			galaxySetup = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Back to Galaxy Setup", xPos + 35, yPos + 545, 250, 35);
			startGame = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Start Game", xPos + 520, yPos + 545, 250, 35);
			startGame.Active = false;

			players = new List<Player>();

			listIsEmptyLabel = new Label("The player list is empty", 0, 0, System.Drawing.Color.DarkRed);
			listIsEmptyLabel.MoveTo(xPos + 400 - (int)(listIsEmptyLabel.GetWidth() / 2), yPos + 175 - (int)(listIsEmptyLabel.GetHeight() / 2));

			playerBorders = new StretchableImage[7];
			for (int i = 0; i < playerBorders.Length; i++)
			{
				playerBorders[i] = new StretchableImage(xPos + 40, yPos + 40 + (45 * i), 700, 40, 30, 13, DrawingManagement.BoxBorder);
			}
			empireInfos = new Label[playerBorders.Length];
			removeButtons = new Button[playerBorders.Length];
			teams = new NumericUpDown[playerBorders.Length];
			teamLabels = new Label[playerBorders.Length];
			colorButtons = new Button[playerBorders.Length];
			for (int i = 0; i < playerBorders.Length; i++)
			{
				empireInfos[i] = new Label(xPos + 80, yPos + 48 + (45 * i));
				removeButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, xPos + 715, yPos + 52 + (i * 45), 16, 16);
				teams[i] = new NumericUpDown(xPos + 570, yPos + 50 + (i * 45), 70, 1, 16, 1);
				teamLabels[i] = new Label("Team: ", xPos + 565, yPos + 50 + (i * 45), System.Drawing.Color.White);
				teamLabels[i].SetAlignment(true);
				colorButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, xPos + 646, yPos + 45 + (i * 45), 32, 30);
			}

			GorgonLibrary.Graphics.Image block = new GorgonLibrary.Graphics.Image("playerColorBlock", 1, 1, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			block.Clear(System.Drawing.Color.White);
			colorBlock = new GorgonLibrary.Graphics.Sprite("playerColor", block);
			colorBlock.SetScale(25, 25);
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (showingRaceSelection)
			{
				raceSelection.MouseHover(x, y, frameDeltaTime);
			}
			else if (showingColorSelection)
			{
				colorSelection.MouseHover(x, y, frameDeltaTime);
			}
			else
			{
				if (empireListScrollBar.MouseHover(x, y, frameDeltaTime))
				{
					UpdatePlayerList();
				}
				humanPlayer.MouseHover(x, y, frameDeltaTime);
				computerPlayer.MouseHover(x, y, frameDeltaTime);
				aiComboBox.MouseHover(x, y, frameDeltaTime);
				empireNameTextBox.Update(frameDeltaTime);
				addButton.MouseHover(x, y, frameDeltaTime);
				selectRaceButton.MouseHover(x, y, frameDeltaTime);
				galaxySetup.MouseHover(x, y, frameDeltaTime);
				startGame.MouseHover(x, y, frameDeltaTime);
				for (int i = 0; i < maxVisible; i++)
				{
					removeButtons[i].MouseHover(x, y, frameDeltaTime);
					colorButtons[i].MouseHover(x, y, frameDeltaTime);
					teams[i].MouseHover(x, y, frameDeltaTime);
				}
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (showingRaceSelection)
			{
				raceSelection.MouseDown(x, y);
			}
			else if (showingColorSelection)
			{
				colorSelection.MouseDown(x, y);
			}
			else
			{
				empireListScrollBar.MouseDown(x, y);
				humanPlayer.MouseDown(x, y);
				computerPlayer.MouseDown(x, y);
				aiComboBox.MouseDown(x, y);
				empireNameTextBox.MouseDown(x, y);
				addButton.MouseDown(x, y);
				selectRaceButton.MouseDown(x, y);
				galaxySetup.MouseDown(x, y);
				startGame.MouseDown(x, y);
				for (int i = 0; i < maxVisible; i++)
				{
					removeButtons[i].MouseDown(x, y);
					colorButtons[i].MouseDown(x, y);
					teams[i].MouseDown(x, y);
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (showingRaceSelection)
			{
				raceSelection.MouseUp(x, y);
			}
			else if (showingColorSelection)
			{
				colorSelection.MouseUp(x, y);
			}
			else
			{
				if (empireListScrollBar.MouseUp(x, y))
				{
					UpdatePlayerList();
				}
				aiComboBox.MouseUp(x, y);
				if (humanPlayer.MouseUp(x, y))
				{
					computerPlayer.IsChecked = false;
					humanPlayer.IsChecked = true;
					aiComboBox.Active = false;
				}
				if (computerPlayer.MouseUp(x, y))
				{
					computerPlayer.IsChecked = true;
					humanPlayer.IsChecked = false;
					aiComboBox.Active = true;
				}
				if (selectRaceButton.MouseUp(x, y))
				{
					showingRaceSelection = true;
				}
				empireNameTextBox.MouseUp(x, y);

				if (addButton.MouseUp(x, y))
				{
					Player newPlayer = new Player();
					newPlayer.race = selectedRace;
					newPlayer.isHuman = humanPlayer.IsChecked;
					newPlayer.empireName = empireNameTextBox.GetString();
					newPlayer.color = colorSelection.GetPresetColor(players.Count);
					if (!newPlayer.isHuman)
					{
						newPlayer.aiScript = aiComboBox.SelectedIndex;
						if (newPlayer.aiScript == 0)
						{
							Random r = new Random();
							newPlayer.aiScript = r.Next(1, aiComboBox.Count);
						}
					}
					else
					{
						newPlayer.aiScript = 1;
					}
					newPlayer.team = players.Count + 1;
					players.Add(newPlayer);
					UpdatePlayerList();
					empireNameTextBox.SetString(string.Empty);
				}

				for (int i = 0; i < maxVisible; i++)
				{
					if (removeButtons[i].MouseUp(x, y))
					{
						players.RemoveAt(i + empireListScrollBar.TopIndex);
						UpdatePlayerList();
					}
					if (teams[i].MouseUp(x, y))
					{
						players[i + empireListScrollBar.TopIndex].team = teams[i].Value;
					}
					if (colorButtons[i].MouseUp(x, y))
					{
						whichPlayerSelectingColor = i + empireListScrollBar.TopIndex;
						colorSelection.SetColor(players[i + empireListScrollBar.TopIndex].color);
						showingColorSelection = true;
					}
				}

				if (galaxySetup.MouseUp(x, y))
				{
					gameMain.ChangeToScreen(Screen.GalaxySetup);
				}

				if (startGame.MouseUp(x, y))
				{
					Random r = new Random();
					int i = 0;

					foreach (Player player in players)
					{
						if (player.race == null)
						{
							player.race = gameMain.raceManager.Races[r.Next(gameMain.raceManager.Races.Count)];
							if (string.IsNullOrEmpty(player.empireName))
							{
								player.empireName = player.race.GetRandomEmperorName();
							}
						}
						Empire empire = new Empire(player.empireName, i, player.race, player.isHuman ? PlayerType.HUMAN : PlayerType.CPU, gameMain.aiManager.AIs[player.aiScript - 1], player.color);
						i++;

						gameMain.empireManager.AddEmpire(empire);
						List<Sector> ownedSectors;
						List<StarSystem> startingSystems = gameMain.galaxy.SetStartingSystems(empire, gameMain.planetTypeManager, gameMain.regionTypeManager, gameMain.resourceManager, out ownedSectors);
						empire.SetHomeSystem(startingSystems, ownedSectors);
						empire.TechnologyManager.AddTechnologies(gameMain.masterTechnologyList.GetRandomizedTechnologies(empire.EmpireRace));
						empire.TechnologyManager.SetInitialBracket(0, 5, empire.ItemManager);
						empire.SetStartingFleets(startingSystems, gameMain.masterTechnologyList, gameMain.iconManager);
						empire.RefreshEconomy();
					}
					gameMain.empireManager.SetupContacts();
					//gameMain.empireManager.UpdateInfluenceMaps(gameMain.galaxy);
					gameMain.empireManager.SetInitialEmpireTurn();
					gameMain.galaxy.ConstructQuadTree();
					gameMain.InitializeSitRep();
					if (gameMain.taskBar == null)
					{
						gameMain.taskBar = new TaskBar(gameMain);
					}
					if (gameMain.ShipShader == null)
					{
						gameMain.ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
					}
					gameMain.ChangeToScreen(Screen.Galaxy);
				}
			}
		}

		public void MouseScroll(int direction, int xScreenPos, int yScreenPos)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (empireNameTextBox.KeyDown(e))
			{
				return;
			}
		}

		public void Resize()
		{
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			for (int i = 0; i < gameMain.ScreenWidth; i += 1024)
			{
				for (int j = 0; j < gameMain.ScreenHeight; j += 600)
				{
					drawingManagement.DrawSprite(SpriteName.TitleNebula, i, j, 255, System.Drawing.Color.White);
				}
			}
			background.Draw(drawingManagement);
			listBackground.Draw(drawingManagement);
			newPlayerBackground.Draw(drawingManagement);

			empireListScrollBar.Draw(drawingManagement);

			humanPlayer.Draw(drawingManagement);
			computerPlayer.Draw(drawingManagement);

			raceBackground.Draw(drawingManagement);
			currentRaceLabel.Draw();
			selectRaceButton.Draw(drawingManagement);

			empireName.Draw();
			empireNameTextBox.Draw(drawingManagement);

			addButton.Draw(drawingManagement);

			galaxySetup.Draw(drawingManagement);
			startGame.Draw(drawingManagement);

			aiComboBox.Draw(drawingManagement);

			if (players.Count > 0)
			{
				for (int i = 0; i < maxVisible; i++)
				{
					playerBorders[i].Draw(drawingManagement);
					empireInfos[i].Draw();
					drawingManagement.DrawSprite(players[i].isHuman ? SpriteName.HumanPlayerIcon : SpriteName.CPUPlayerIcon, xPos + 55, yPos + 48 + (i * 45));
					removeButtons[i].Draw(drawingManagement);
					teams[i].Draw(drawingManagement);
					teamLabels[i].Draw(drawingManagement);
					colorButtons[i].Draw(drawingManagement);
					colorBlock.SetPosition(xPos + 650, yPos + 47 + (i * 45));
					colorBlock.Color = players[i + empireListScrollBar.TopIndex].color;
					colorBlock.Draw();
				}
			}
			else
			{
				listIsEmptyLabel.Draw();
			}

			if (showingRaceSelection)
			{
				raceSelection.DrawWindow(drawingManagement);
			}
			if (showingColorSelection)
			{
				colorSelection.DrawWindow(drawingManagement);
			}
		}

		private void UpdatePlayerList()
		{
			if (players.Count < 2)
			{
				startGame.Active = false;
			}
			else
			{
				startGame.Active = true;
			}
			int amountOfSystemsRequired = 0;
			foreach (Player player in players)
			{
				amountOfSystemsRequired++;
			}
			//Add special systems like Beyaan, and others, here
			if (habitableStars < amountOfSystemsRequired)
			{
				startGame.Active = false;
			}
			if (players.Count > playerBorders.Length)
			{
				maxVisible = playerBorders.Length;
				int topIndex = empireListScrollBar.TopIndex;
				empireListScrollBar.SetEnabledState(true);
				empireListScrollBar.SetAmountOfItems(players.Count);
				empireListScrollBar.TopIndex = topIndex;
			}
			else
			{
				maxVisible = players.Count;
				empireListScrollBar.SetEnabledState(false);
				empireListScrollBar.TopIndex = 0;
				empireListScrollBar.SetAmountOfItems(20);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				string empireInfo = players[i + empireListScrollBar.TopIndex].race == null ? "Random Race" : players[i + empireListScrollBar.TopIndex].race.RaceName;
				if (!string.IsNullOrEmpty(players[i + empireListScrollBar.TopIndex].empireName))
				{
					empireInfo += " - " + players[i + empireListScrollBar.TopIndex].empireName;
				}
				else
				{
					empireInfo += " - Random Name";
				}
				empireInfos[i].SetText(empireInfo);
				teams[i].SetValue(players[i + empireListScrollBar.TopIndex].team);
			}
		}

		private void OnRaceOkClick(Race selectedRace)
		{
			this.selectedRace = selectedRace;
			if (selectedRace == null)
			{
				currentRaceLabel.SetText("Random");
			}
			else
			{
				currentRaceLabel.SetText(selectedRace.RaceName);
			}
			showingRaceSelection = false;
		}

		private void OnRaceCancelClick()
		{
			showingRaceSelection = false;
		}

		private void OnColorOkClick(System.Drawing.Color color)
		{
			players[whichPlayerSelectingColor].color = color;
			showingColorSelection = false;
		}
		private void OnColorCancelClick()
		{
			showingColorSelection = false;
		}
	}
}
