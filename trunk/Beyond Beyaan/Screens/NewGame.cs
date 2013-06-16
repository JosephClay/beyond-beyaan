using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class NewGame : ScreenInterface
	{
		private const int PLAYER_LIMIT = 16;

		GameMain gameMain;

		ComboBox galaxyComboBox;

		Button[] buttons;

		Button humanPlayer;
		Button aiPlayer;
		Button addPlayer;

		ComboBox raceComboBox;
		ComboBox aiComboBox;

		SingleLineTextBox empireNameTextBox;

		Label minPlanetLabel;
		Label maxPlanetLabel;
		Label galaxySizeLabel;
		Label minPlanetsPerSystemLabel;
		Label maxPlanetsPerSystemLabel;
		Label numOfStarsLabel;
		Label generatingGalaxyLabel;
		Label emperorNameLabel;
		Label raceLabel;
		Label handicapLabel;

		int galaxySize;
		int minPlanets;
		int maxPlanets;
		int generatingGalaxy;
		bool generatingDrawn;

		private List<Empire> empires;
		private Label[] empireNames;
		private Label[] races;
		private ComboBox[] handicapComboBoxes;
		private Button[] removeButtons;

		private GorgonLibrary.Graphics.Sprite miniAvatar;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			List<SpriteName> spriteNames = new List<SpriteName>();
			spriteNames.Add(SpriteName.MiniBackgroundButton);
			spriteNames.Add(SpriteName.MiniForegroundButton);
			spriteNames.Add(SpriteName.ScrollUpBackgroundButton);
			spriteNames.Add(SpriteName.ScrollUpForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBar);
			spriteNames.Add(SpriteName.ScrollDownBackgroundButton);
			spriteNames.Add(SpriteName.ScrollDownForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBackgroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalForegroundButton);

			List<string> items = new List<string>();
			items.Add("Random");
			items.Add("Cluster");
			items.Add("Ring");
			items.Add("Diamond");
			items.Add("Star");

			galaxyComboBox = new ComboBox(spriteNames, items, gameMain.ScreenWidth - 500, 596, 175, 25, 4);

			List<string> names = new List<string>();
			names.Add("Random");
			foreach (Race race in gameMain.RaceManager.Races)
			{
				names.Add(race.RaceName);
			}
			raceComboBox = new ComboBox(spriteNames, names, 200, 530, 175, 25, 6);

			names = new List<string>();
			names.Add("Random");
			foreach (AI ai in gameMain.AIManager.AIs)
			{
				names.Add(ai.AIName);
			}
			aiComboBox = new ComboBox(spriteNames, names, 200, 560, 175, 25, 6);

			humanPlayer = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Human", 10, 530, 175, 25);
			aiPlayer = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Computer", 10, 560, 175, 25);
			humanPlayer.Selected = true;

			empireNameTextBox = new SingleLineTextBox(395, 530, 110, 25, SpriteName.MiniBackgroundButton);
			addPlayer = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Add Player", 395, 560, 110, 25);

			aiComboBox.Active = false;

			buttons = new Button[9];
			buttons[0] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Back", 10, gameMain.ScreenHeight - 35, 175, 25);
			buttons[1] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Next", gameMain.ScreenWidth - 185, gameMain.ScreenHeight - 35, 175, 25);
			buttons[2] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Generate Galaxy", gameMain.ScreenWidth - 185, 596, 175, 25);
			buttons[3] = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, "", gameMain.ScreenWidth - 210, 600, 16, 16);
			buttons[4] = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, "", gameMain.ScreenWidth - 316, 600, 16, 16);
			buttons[5] = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, "", gameMain.ScreenWidth - 210, 524, 16, 16);
			buttons[6] = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, "", gameMain.ScreenWidth - 260, 524, 16, 16);
			buttons[7] = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, "", gameMain.ScreenWidth - 210, 560, 16, 16);
			buttons[8] = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, "", gameMain.ScreenWidth - 260, 560, 16, 16);

			galaxySize = 50;
			minPlanets = 0;
			maxPlanets = 6;

			galaxySizeLabel = new Label(galaxySize + " x " + galaxySize, gameMain.ScreenWidth - 290, 600);
			minPlanetsPerSystemLabel = new Label("Minimum planets per system:", gameMain.ScreenWidth - 500, 524);
			maxPlanetsPerSystemLabel = new Label("Maximum planets per system:", gameMain.ScreenWidth - 500, 560);
			minPlanetLabel = new Label(minPlanets.ToString(), gameMain.ScreenWidth - 240, 524);
			maxPlanetLabel = new Label(maxPlanets.ToString(), gameMain.ScreenWidth - 240, 560);
			numOfStarsLabel = new Label(string.Empty, gameMain.ScreenWidth - 185, 624);

			generatingGalaxy = -1;
			generatingGalaxyLabel = new Label("Generating Galaxy", 0, 0);
			generatingDrawn = false;

			empires = new List<Empire>();
			this.races = new Label[PLAYER_LIMIT];
			empireNames = new Label[PLAYER_LIMIT];
			handicapComboBoxes = new ComboBox[PLAYER_LIMIT];
			removeButtons = new Button[PLAYER_LIMIT];

			names = new List<string>();
			for (int i = 5; i <= 20; i++)
			{
				names.Add((i * 10).ToString() + "%");
			}

			for (int i = 0; i < PLAYER_LIMIT; i++)
			{
				this.races[i] = new Label(string.Empty, 200, 30 + (i * 30));
				empireNames[i] = new Label(string.Empty, 10, 30 + (i * 30));
				handicapComboBoxes[i] = new ComboBox(spriteNames, names, 350, 30 + (i * 30), 75, 25, 6);
				handicapComboBoxes[i].SelectedIndex = 5;
				removeButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, 465, 30 + (i * 30), 25, 25);
			}
			emperorNameLabel = new Label("Empire Name:", 10, 5);
			raceLabel = new Label("Race:", 200, 5);
			handicapLabel = new Label("Handicap:", 350, 5);
		}

		public void Clear()
		{
			humanPlayer.Selected = true;
			aiComboBox.Active = false;

			galaxySize = 50;
			minPlanets = 0;
			maxPlanets = 6;

			galaxySizeLabel = new Label(galaxySize + " x " + galaxySize, gameMain.ScreenWidth - 290, 600);
			minPlanetsPerSystemLabel = new Label("Minimum planets per system:", gameMain.ScreenWidth - 500, 524);
			maxPlanetsPerSystemLabel = new Label("Maximum planets per system:", gameMain.ScreenWidth - 500, 560);
			minPlanetLabel = new Label(minPlanets.ToString(), gameMain.ScreenWidth - 240, 524);
			maxPlanetLabel = new Label(maxPlanets.ToString(), gameMain.ScreenWidth - 240, 560);
			numOfStarsLabel = new Label(string.Empty, gameMain.ScreenWidth - 185, 624);

			generatingGalaxy = -1;
			generatingGalaxyLabel = new Label("Generating Galaxy", 0, 0);
			generatingDrawn = false;

			empires = new List<Empire>();

			for (int i = 0; i < PLAYER_LIMIT; i++)
			{
				this.races[i].SetText(string.Empty);
				empireNames[i].SetText(string.Empty);
				handicapComboBoxes[i].SelectedIndex = 5;
			}
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			DrawGalaxyPreview(drawingManagement);

			emperorNameLabel.Draw();
			raceLabel.Draw();
			handicapLabel.Draw();

			for (int i = empires.Count - 1; i >= 0; i--)
			{
				races[i].Draw();
				empireNames[i].Draw();
				handicapComboBoxes[i].Draw(drawingManagement);
				drawingManagement.DrawSprite(empires[i].Type == PlayerType.HUMAN ? SpriteName.HumanPlayerIcon : SpriteName.CPUPlayerIcon, 435, 30 + (i * 30), 255, 25, 25, System.Drawing.Color.White);
				removeButtons[i].Draw(drawingManagement);
			}

			drawingManagement.DrawSprite(SpriteName.MiniBackgroundButton, gameMain.ScreenWidth - 320, 596, 255, 130, 25, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.MiniBackgroundButton, gameMain.ScreenWidth - 265, 520, 255, 75, 25, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.MiniBackgroundButton, gameMain.ScreenWidth - 265, 556, 255, 75, 25, System.Drawing.Color.White);

			galaxySizeLabel.Draw();
			minPlanetsPerSystemLabel.Draw();
			maxPlanetsPerSystemLabel.Draw();
			minPlanetLabel.Draw();
			maxPlanetLabel.Draw();

			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].Draw(drawingManagement);
			}

			humanPlayer.Draw(drawingManagement);
			aiPlayer.Draw(drawingManagement);
			addPlayer.Draw(drawingManagement);
			empireNameTextBox.Draw(drawingManagement);

			aiComboBox.Draw(drawingManagement);
			raceComboBox.Draw(drawingManagement);

			if (generatingGalaxy != -1)
			{
				generatingDrawn = true;
				drawingManagement.DrawSprite(SpriteName.NormalBackgroundButton, (gameMain.ScreenWidth / 2) - 150, (gameMain.ScreenHeight / 2) - 20, 255, 300, 40, System.Drawing.Color.White);
				generatingGalaxyLabel.Move((int)((gameMain.ScreenWidth / 2) - (generatingGalaxyLabel.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (generatingGalaxyLabel.GetHeight() / 2)));
				generatingGalaxyLabel.Draw();
			}

			if (raceComboBox.SelectedIndex > 0)
			{
				miniAvatar.Draw();
			}
			else
			{
				drawingManagement.DrawSprite(SpriteName.CancelBackground, 10, 600, 255, 128, 128, System.Drawing.Color.White);
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			galaxyComboBox.UpdateHovering(mouseX, mouseY, frameDeltaTime);

			humanPlayer.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			aiPlayer.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			addPlayer.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			empireNameTextBox.Update(frameDeltaTime);

			raceComboBox.UpdateHovering(mouseX, mouseY, frameDeltaTime);
			aiComboBox.UpdateHovering(mouseX, mouseY, frameDeltaTime);

			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}

			for (int i = 0; i < empires.Count; i++)
			{
				removeButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
				handicapComboBoxes[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}
			
			GALAXYTYPE type = GALAXYTYPE.RANDOM;
			if (generatingGalaxy != -1 && generatingDrawn)
			{
				string reason;
				switch (generatingGalaxy)
				{
					case 0:
						type = GALAXYTYPE.RANDOM;
						break;
					case 1:
						type = GALAXYTYPE.CLUSTER;
						break;
					case 2:
						type = GALAXYTYPE.RING;
						break;
					case 3:
						type = GALAXYTYPE.DIAMOND;
						break;
					case 4:
						type = GALAXYTYPE.STAR;
						break;
				}
				gameMain.Galaxy.GenerateGalaxy(type, 1, 1, galaxySize, 4, gameMain.SpriteManager, gameMain.Random, out reason);
				numOfStarsLabel.SetText("Number of stars: " + gameMain.Galaxy.GetAllStars().Count);
				generatingGalaxy = -1;
				generatingDrawn = false;
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			galaxyComboBox.MouseDown(x, y);

			humanPlayer.MouseDown(x, y);
			aiPlayer.MouseDown(x, y);
			addPlayer.MouseDown(x, y);
			empireNameTextBox.MouseDown(x, y);

			for (int i = 0; i < empires.Count; i++)
			{
				removeButtons[i].MouseDown(x, y);
				if (handicapComboBoxes[i].MouseDown(x, y))
				{
					return;
				}
			}

			if (raceComboBox.MouseDown(x, y))
			{
				return;
			}
			if (aiComboBox.MouseDown(x, y))
			{
				return;
			}

			for (int i = 0; i < buttons.Length; i++)
			{
				if (buttons[i].MouseDown(x, y))
				{
					return;
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < empires.Count; i++)
			{
				if (removeButtons[i].MouseUp(x, y))
				{
					for (int j = i; j < empires.Count; j++)
					{
						if (j < empires.Count - 1)
						{
							empireNames[j].SetText(empireNames[j + 1].Text);
							races[j].SetText(races[j + 1].Text);
						}
					}
					empires.RemoveAt(i);
					return;
				}
				if (handicapComboBoxes[i].MouseUp(x, y))
				{
					return;
				}
			}
			if (galaxyComboBox.MouseUp(x, y))
			{
				return;
			}
			empireNameTextBox.MouseUp(x, y);
			if (humanPlayer.MouseUp(x, y))
			{
				humanPlayer.Selected = true;
				aiPlayer.Selected = false;
				aiComboBox.Active = false;
			}
			if (aiPlayer.MouseUp(x, y))
			{
				humanPlayer.Selected = false;
				aiPlayer.Selected = true;
				aiComboBox.Active = true;
			}
			if (addPlayer.MouseUp(x, y))
			{
				if (empires.Count >= PLAYER_LIMIT)
				{
					return;
				}
				Random r = new Random();
				Race race;
				AI ai = null;
				if (raceComboBox.SelectedIndex == 0)
				{
					race = gameMain.RaceManager.Races[r.Next(gameMain.RaceManager.Races.Count)];
				}
				else
				{
					race = gameMain.RaceManager.Races[raceComboBox.SelectedIndex - 1];
				}
				if (aiPlayer.Selected)
				{
					if (aiComboBox.SelectedIndex == 0)
					{
						ai = gameMain.AIManager.AIs[r.Next(gameMain.AIManager.AIs.Count)];
					}
					else
					{
						ai = gameMain.AIManager.AIs[aiComboBox.SelectedIndex - 1];
					}
				}
				int id = 0;
				foreach (Empire empire in empires)
				{
					if (id <= empire.EmpireID)
					{
						id = empire.EmpireID + 1;
					}
				}
				if (string.IsNullOrEmpty(empireNameTextBox.GetString()))
				{
					empireNameTextBox.SetString(race.GetRandomEmperorName());
				}
				Empire newEmpire = new Empire(empireNameTextBox.GetString(), id, race, humanPlayer.Selected ? PlayerType.HUMAN : PlayerType.CPU, ai, 
					System.Drawing.Color.FromArgb(255, r.Next(201) + 55, r.Next(201) + 55, r.Next(201) + 55), gameMain);
				empires.Add(newEmpire);
				empireNames[empires.Count - 1].SetText(empireNameTextBox.GetString());
				if (raceComboBox.SelectedIndex == 0)
				{
					races[empires.Count - 1].SetText("Random");
				}
				else
				{
					races[empires.Count - 1].SetText(race.RaceName);
				}
				empireNameTextBox.SetString(string.Empty);
			}

			if (raceComboBox.MouseUp(x, y))
			{
				if (raceComboBox.SelectedIndex > 0)
				{
					miniAvatar = gameMain.RaceManager.Races[raceComboBox.SelectedIndex - 1].GetMiniAvatar();
					miniAvatar.SetPosition(10, 600);
				}
				return;
			}
			if (aiComboBox.MouseUp(x, y))
			{
				return;
			}

			for (int i = 0; i < buttons.Length; i++)
			{
				if (buttons[i].MouseUp(x, y))
				{
					switch(i)
					{
						case 0:
							gameMain.ChangeToScreen(Screen.MainMenu);
							break;
						case 1:
							if (gameMain.Galaxy.GalaxySize > 0)
							{
								int habitableStars = gameMain.Galaxy.GetAllStars().Count;
								if (empires.Count > habitableStars || empires.Count < 2)
								{
									return;
								}
								bool hasHuman = false;
								foreach (Empire empire in empires)
								{
									if (empire.Type == PlayerType.HUMAN)
									{
										hasHuman = true;
										break;
									}
								}
								if (!hasHuman)
								{
									return;
								}
								foreach (Empire empire in empires)
								{
									Planet homePlanet;
									gameMain.EmpireManager.AddEmpire(empire);
									StarSystem homeSystem = gameMain.Galaxy.SetHomeworld(empire, out homePlanet);
									empire.SetHomeSystem(homeSystem, homePlanet);
								}
								gameMain.EmpireManager.SetupContacts();
								gameMain.EmpireManager.UpdateInfluenceMaps(gameMain.Galaxy);
								gameMain.EmpireManager.SetInitialEmpireTurn();
								//gameMain.EmpireManager.ProcessNextEmpire(); //This will process the AI players, then set the current empire to human controlled one
								gameMain.RefreshSitRep();
								gameMain.Galaxy.ConstructQuadTree();
								gameMain.ChangeToScreen(Screen.Galaxy);
							}
							break;
						case 2:
							generatingGalaxy = galaxyComboBox.SelectedIndex;
							break;
						case 3:
							galaxySize += 10;
							galaxySizeLabel.SetText(galaxySize + " x " + galaxySize);
							break;
						case 4:
							if (galaxySize > 50)
							{
								galaxySize -= 10;
								galaxySizeLabel.SetText(galaxySize + " x " + galaxySize);
							} break;
						case 5:
							if (minPlanets < maxPlanets)
							{
								minPlanets++;
								minPlanetLabel.SetText(minPlanets.ToString());
							} break;
						case 6:
							if (minPlanets > 0)
							{
								minPlanets--;
								minPlanetLabel.SetText(minPlanets.ToString());
							} break;
						case 7:
							if (maxPlanets < 10)
							{
								maxPlanets++;
								maxPlanetLabel.SetText(maxPlanets.ToString());
							} break;
						case 8:
							if (maxPlanets > minPlanets && maxPlanets > 1)
							{
								maxPlanets--;
								maxPlanetLabel.SetText(maxPlanets.ToString());
							} break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (empireNameTextBox.KeyDown(e))
			{
				return;
			}
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			drawingManagement.DrawSprite(SpriteName.Screen, gameMain.ScreenWidth - 500, 0, 255, System.Drawing.Color.White);

			GorgonLibrary.Graphics.Sprite nebula = gameMain.Galaxy.Nebula;
			if (nebula != null)
			{
				nebula.SetPosition(gameMain.ScreenWidth - 499, 1);
				float scale = (498.0f / (gameMain.Galaxy.GalaxySize + 3));
				nebula.SetScale(scale, scale);
				gameMain.Galaxy.Nebula.Draw();
			}

			List<StarSystem> systems = gameMain.Galaxy.GetAllStars();

			galaxyComboBox.Draw(drawingManagement);

			if (systems.Count > 0)
			{
				foreach (StarSystem system in systems)
				{
					int x = (gameMain.ScreenWidth - 499) + (int)(480.0f * (system.X / (float)gameMain.Galaxy.GalaxySize));
					int y = (int)(480.0f * (system.Y / (float)gameMain.Galaxy.GalaxySize)) + 1;
					GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
					gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
					system.Sprite.Draw(x, y, 0.4f, 0.4f);
					//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}

				numOfStarsLabel.Draw();
			}
		}
	}
}
