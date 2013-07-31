using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class NewGame : ScreenInterface
	{
		private GameMain _gameMain;

		private int _xPos;
		private int _yPos;

		private BBStretchableImage _background;
		private BBSprite _nebulaBackground;
		private BBSprite _randomRaceSprite;

		private BBStretchableImage _playerBackground;
		private BBStretchableImage _playerInfoBackground;
		private BBStretchButton _playerRaceButton;
		private BBSingleLineTextBox _playerEmperorName;
		private BBSingleLineTextBox _playerHomeworldName;
		private BBSprite _playerRaceSprite;
		private BBTextBox _playerRaceDescription;
		private BBLabel[] _playerLabels;
		//private Race _playerRace;

		private BBStretchableImage _AIBackground;
		private BBStretchButton[] _AIRaceButtons;
		private BBSprite[] _AIRaceSprites;
		private Race[] _AIRaces;

		private BBStretchableImage _galaxyBackground;
		private BBComboBox _galaxyComboBox;

		private BBLabel _difficultyLabel;
		private BBComboBox _difficultyComboBox;

		private BBLabel _numberOfAILabel;
		private BBNumericUpDown _numericUpDownAI;
		//private Camera camera;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this._gameMain = gameMain;

			_background = new BBStretchableImage();
			_playerBackground = new BBStretchableImage();
			_playerInfoBackground = new BBStretchableImage();
			_playerRaceButton = new BBStretchButton();
			_playerRaceDescription = new BBTextBox();
			_playerLabels = new BBLabel[3];
			_playerEmperorName = new BBSingleLineTextBox();
			_playerHomeworldName = new BBSingleLineTextBox();
			_AIBackground = new BBStretchableImage();
			_AIRaceButtons = new BBStretchButton[5];
			_AIRaceSprites = new BBSprite[5];
			_numberOfAILabel = new BBLabel();
			_numericUpDownAI = new BBNumericUpDown();

			_difficultyComboBox = new BBComboBox();
			_difficultyLabel = new BBLabel();

			_nebulaBackground = SpriteManager.GetSprite("TitleNebula", gameMain.Random);

			_xPos = (gameMain.ScreenWidth / 2) - 440;
			_yPos = (gameMain.ScreenHeight / 2) - 340;
			if (_nebulaBackground == null)
			{
				reason = "TitleNebula sprite doesn't exist.";
				return false;
			}
			if (!_background.Initialize(_xPos, _yPos, 880, 680, StretchableImageType.MediumBorder, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_playerBackground.Initialize(_xPos + 30, _yPos + 30, 820, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerInfoBackground.Initialize(_xPos + 40, _yPos + 60, 295, 130, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerRaceButton.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 340, _yPos + 40, 500, 150, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_playerRaceDescription.Initialize(_xPos + 485, _yPos + 51, 345, 130, true, "RaceDescriptionTextBox", gameMain.Random, out reason))
			{
				return false;
			}
			_playerLabels[0] = new BBLabel();
			if (!_playerLabels[0].Initialize(_xPos + 90, _yPos + 36, "Player Race Information", Color.White, out reason))
			{
				return false;
			}
			_playerLabels[1] = new BBLabel();
			if (!_playerLabels[1].Initialize(_xPos + 45, _yPos + 70, "Emperor Name:", Color.White, out reason))
			{
				return false;
			}
			if (!_playerEmperorName.Initialize(string.Empty, _xPos + 50, _yPos + 90, 275, 35, false, gameMain.Random, out reason))
			{
				return false;
			}
			_playerLabels[2] = new BBLabel();
			if (!_playerLabels[2].Initialize(_xPos + 45, _yPos + 125, "Homeworld Name:", Color.White, out reason))
			{
				return false;
			}
			if (!_playerHomeworldName.Initialize(string.Empty, _xPos + 50, _yPos + 145, 275, 35, false, gameMain.Random, out reason))
			{
				return false;
			}
			_playerRaceDescription.SetText("A random race will be chosen.  If the Emperor and/or Homeworld name fields are left blank, default race names for those will be used.");
			_randomRaceSprite = SpriteManager.GetSprite("RandomRace", gameMain.Random);
			if (_randomRaceSprite == null)
			{
				reason = "RandomRace sprite does not exist.";
				return false;
			}
			_playerRaceSprite = _randomRaceSprite;
			//_playerRace = null;
			_AIRaces = new Race[5];

			if (!_AIBackground.Initialize(_xPos + 30, _yPos + 205, 820, 220, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_difficultyLabel.Initialize(_xPos + 40, _yPos + 220, "Difficulty Level:", Color.White, out reason))
			{
				return false;
			}
			List<string> difficultyItems = new List<string>
											{
												"Extra Mild",
												"Mild",
												"Medium",
												"Hot",
												"Extra Hot"
											};
			if (!_difficultyComboBox.Initialize(difficultyItems, _xPos + 170, _yPos + 215, 200, 35, 5, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_numberOfAILabel.Initialize(_xPos + 430, _yPos + 220, "Number of Computer Players:", Color.White, out reason))
			{
				return false;
			}
			if (!_numericUpDownAI.Initialize(_xPos + 615, _yPos + 222, 75, 1, 5, 5, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _AIRaceButtons.Length; i++)
			{
				_AIRaceButtons[i] = new BBStretchButton();
				if (!_AIRaceButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos + 40 + (i * 155), _yPos + 260, 150, 150, gameMain.Random, out reason))
				{
					return false;
				}
				_AIRaceSprites[i] = _randomRaceSprite;
			}

			_galaxyBackground = new BBStretchableImage();
			_galaxyComboBox = new BBComboBox();

			List<string> items = new List<string>();
			items.Add("Small Galaxy");
			items.Add("Medium Galaxy");
			items.Add("Large Galaxy");
			items.Add("Huge Galaxy");

			if (!_galaxyBackground.Initialize(_xPos + 30, _yPos + 430, 240, 235, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_galaxyComboBox.Initialize(items, _xPos + 30, _yPos + 430, 240, 35, 4, gameMain.Random, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}

		public void Clear()
		{
			//_playerRace = null;
			_AIRaces = new Race[5];
			_numericUpDownAI.SetValue(1);
			_playerEmperorName.SetText(string.Empty);
			_playerHomeworldName.SetText(string.Empty);
			_playerRaceSprite = _randomRaceSprite;
			for (int i = 0; i < _AIRaceSprites.Length; i++)
			{
				_AIRaceSprites[i] = _randomRaceSprite;
			}
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			for (int i = 0; i < _gameMain.ScreenWidth; i += (int)_nebulaBackground.Width)
			{
				for (int j = 0; j < _gameMain.ScreenHeight; j += (int)_nebulaBackground.Height)
				{
					_nebulaBackground.Draw(i, j);
				}
			}
			_background.Draw();
			if (GameConfiguration.AllowGalaxyPreview)
			{
				DrawGalaxyPreview();
			}
			_playerBackground.Draw();
			_playerInfoBackground.Draw();
			for (int i = 0; i < _playerLabels.Length; i++)
			{
				_playerLabels[i].Draw();
			}
			_playerRaceButton.Draw();
			_playerRaceSprite.Draw(_xPos + 350, _yPos + 51);
			_playerRaceDescription.Draw();
			_playerEmperorName.Draw();
			_playerHomeworldName.Draw();

			_AIBackground.Draw();
			_difficultyLabel.Draw();

			_numberOfAILabel.Draw();
			_numericUpDownAI.Draw();
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				_AIRaceButtons[i].Draw();
				_AIRaceSprites[i].Draw(_xPos + 51 + (i * 155), _yPos + 271);
			}

			_galaxyBackground.Draw();

			//Comboboxes should be drawn last, due to their "drop-down" feature
			_galaxyComboBox.Draw();
			_difficultyComboBox.Draw();
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*_galaxyComboBox.MouseHover(mouseX, mouseY, frameDeltaTime);

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
				_gameMain.Galaxy.GenerateGalaxy(type, 1, 1, galaxySize, 2, _gameMain.Random, out reason);
				camera = new Camera(_gameMain.Galaxy.GalaxySize * 32, _gameMain.Galaxy.GalaxySize * 32, 500, 500);
				camera.CenterCamera(camera.Width / 2, camera.Height / 2, camera.MaxZoom);
				numOfStarsLabel.SetText("Number of stars: " + _gameMain.Galaxy.GetAllStars().Count);
				generatingGalaxy = -1;
				generatingDrawn = false;
			}*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*_galaxyComboBox.MouseDown(x, y);

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
			}*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*for (int i = 0; i < empires.Count; i++)
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
			if (_galaxyComboBox.MouseUp(x, y))
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
					race = _gameMain.RaceManager.Races[r.Next(_gameMain.RaceManager.Races.Count)];
				}
				else
				{
					race = _gameMain.RaceManager.Races[raceComboBox.SelectedIndex - 1];
				}
				if (aiPlayer.Selected)
				{
					if (aiComboBox.SelectedIndex == 0)
					{
						ai = _gameMain.AIManager.AIs[r.Next(_gameMain.AIManager.AIs.Count)];
					}
					else
					{
						ai = _gameMain.AIManager.AIs[aiComboBox.SelectedIndex - 1];
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
				if (string.IsNullOrEmpty(empireNameTextBox.Text))
				{
					empireNameTextBox.SetText(race.GetRandomEmperorName());
				}
				Empire newEmpire = new Empire(empireNameTextBox.Text, id, race, humanPlayer.Selected ? PlayerType.HUMAN : PlayerType.CPU, ai, 
					System.Drawing.Color.FromArgb(255, r.Next(201) + 55, r.Next(201) + 55, r.Next(201) + 55), _gameMain);
				empires.Add(newEmpire);
				empireNames[empires.Count - 1].SetText(empireNameTextBox.Text);
				if (raceComboBox.SelectedIndex == 0)
				{
					races[empires.Count - 1].SetText("Random");
				}
				else
				{
					races[empires.Count - 1].SetText(race.RaceName);
				}
				empireNameTextBox.SetText(string.Empty);
			}

			if (raceComboBox.MouseUp(x, y))
			{
				if (raceComboBox.SelectedIndex > 0)
				{
					miniAvatar = _gameMain.RaceManager.Races[raceComboBox.SelectedIndex - 1].GetMiniAvatar();
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
							_gameMain.ChangeToScreen(Screen.MainMenu);
							break;
						case 1:
							if (_gameMain.Galaxy.GalaxySize > 0)
							{
								int habitableStars = _gameMain.Galaxy.GetAllStars().Count;
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
									_gameMain.EmpireManager.AddEmpire(empire);
									StarSystem homeSystem = _gameMain.Galaxy.SetHomeworld(empire, out homePlanet);
									empire.SetHomeSystem(homeSystem, homePlanet);
								}
								_gameMain.EmpireManager.SetupContacts();
								//_gameMain.EmpireManager.UpdateInfluenceMaps(_gameMain.Galaxy);
								_gameMain.EmpireManager.SetInitialEmpireTurn();
								//_gameMain.EmpireManager.ProcessNextEmpire(); //This will process the AI players, then set the current empire to human controlled one
								_gameMain.RefreshSitRep();
								//_gameMain.Galaxy.ConstructQuadTree();
								_gameMain.ChangeToScreen(Screen.Galaxy);
							}
							break;
						case 2:
							generatingGalaxy = _galaxyComboBox.SelectedIndex;
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
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			/*if (empireNameTextBox.KeyDown(e))
			{
				return;
			}*/
		}

		private void DrawGalaxyPreview()
		{
			/*drawingManagement.DrawSprite(SpriteName.Screen, _gameMain.ScreenWidth - 500, 0, 255, System.Drawing.Color.White);

			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();

			_galaxyComboBox.Draw();

			if (systems.Count > 0)
			{
				foreach (StarSystem system in systems)
				{
					int x = (_gameMain.ScreenWidth - 499) + (int)((system.X - camera.CameraX) * camera.ZoomDistance);
					int y = (int)((system.Y - camera.CameraY) * camera.ZoomDistance) + 1;
					GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
					_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
					system.Sprite.Draw(x, y, 0.4f, 0.4f);
					//drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}

				//numOfStarsLabel.Draw();
			}*/
		}
	}
}
