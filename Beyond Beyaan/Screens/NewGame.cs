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
		private BBTextBox _playerRaceDescription;
		private BBLabel[] _playerLabels;

		private BBStretchableImage _AIBackground;
		private BBStretchButton[] _AIRaceButtons;

		private Race[] _playerRaces;
		private BBSprite[] _raceSprites;
		private Color[] _playerColors;

		private BBStretchableImage _galaxyBackground;
		private BBComboBox _galaxyComboBox;

		private BBLabel _difficultyLabel;
		private BBComboBox _difficultyComboBox;

		private BBLabel _numberOfAILabel;
		private BBNumericUpDown _numericUpDownAI;
		//private Camera camera;

		private RaceSelection _raceSelection;
		private bool _showingSelection;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this._gameMain = gameMain;

			_showingSelection = false;
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
			_raceSprites = new BBSprite[6];
			_playerRaces = new Race[6];
			_playerColors = new Color[6];
			_numberOfAILabel = new BBLabel();
			_numericUpDownAI = new BBNumericUpDown();

			_difficultyComboBox = new BBComboBox();
			_difficultyLabel = new BBLabel();

			_nebulaBackground = SpriteManager.GetSprite("TitleNebula", gameMain.Random);

			_playerColors[0] = Color.Red;
			_playerColors[1] = Color.Blue;
			_playerColors[2] = Color.Yellow;
			_playerColors[3] = Color.Green;
			_playerColors[4] = Color.Purple;
			_playerColors[5] = Color.Orange;

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

			for (int i = 0; i < _raceSprites.Length; i++)
			{
				_raceSprites[i] = _randomRaceSprite;
			}

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

			_raceSelection = new RaceSelection();
			if (!_raceSelection.Initialize(gameMain, out reason))
			{
				return false;
			}
			_raceSelection.OnOkClick = OnRaceSelectionOKClick;

			reason = null;
			return true;
		}

		public void Clear()
		{
			//_playerRace = null;
			_playerRaces = new Race[6];
			_numericUpDownAI.SetValue(1);
			_playerEmperorName.SetText(string.Empty);
			_playerHomeworldName.SetText(string.Empty);
			for (int i = 0; i < _raceSprites.Length; i++)
			{
				_raceSprites[i] = _randomRaceSprite;
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
			_raceSprites[0].Draw(_xPos + 350, _yPos + 51);
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
				_raceSprites[i + 1].Draw(_xPos + 51 + (i * 155), _yPos + 271);
			}

			_galaxyBackground.Draw();

			//Comboboxes should be drawn last, due to their "drop-down" feature
			_galaxyComboBox.Draw();
			_difficultyComboBox.Draw();

			if (_showingSelection)
			{
				_raceSelection.Draw();
			}
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (_showingSelection)
			{
				_raceSelection.MouseHover(x, y, frameDeltaTime);
				return;
			}
			_playerEmperorName.Update(frameDeltaTime);
			_playerHomeworldName.Update(frameDeltaTime);
			_galaxyComboBox.MouseHover(x, y, frameDeltaTime);
			_difficultyComboBox.MouseHover(x, y, frameDeltaTime);
			_playerRaceButton.MouseHover(x, y, frameDeltaTime);
			
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				_AIRaceButtons[i].MouseHover(x, y, frameDeltaTime);
			}

			_numericUpDownAI.MouseHover(x, y, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_showingSelection)
			{
				_raceSelection.MouseDown(x, y);
				return;
			}
			if (_galaxyComboBox.MouseDown(x, y))
			{
				return;
			}
			if (_difficultyComboBox.MouseDown(x, y))
			{
				return;
			}
			if (_playerEmperorName.MouseDown(x, y))
			{
				return;
			}
			if (_playerHomeworldName.MouseDown(x, y))
			{
				return;
			}
			if (_playerRaceButton.MouseDown(x, y))
			{
				return;
			}
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				if (_AIRaceButtons[i].MouseDown(x, y))
				{
					return;
				}
			}
			_numericUpDownAI.MouseDown(x, y);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton != 1)
			{
				return;
			}
			if (_showingSelection)
			{
				_raceSelection.MouseUp(x, y);
				return;
			}
			_playerEmperorName.MouseUp(x, y);
			_playerHomeworldName.MouseUp(x, y);
			_numericUpDownAI.MouseUp(x, y);
			_difficultyComboBox.MouseUp(x, y);
			if (_galaxyComboBox.MouseUp(x, y))
			{
				//Update galaxy here
			}
			if (_playerRaceButton.MouseUp(x, y))
			{
				_showingSelection = true;
				_raceSelection.SetCurrentPlayerInfo(0, _playerRaces[0], _playerColors[0]);
			}
			for (int i = 0; i < _numericUpDownAI.Value; i++)
			{
				if (_AIRaceButtons[i].MouseUp(x, y))
				{
					_showingSelection = true;
					_raceSelection.SetCurrentPlayerInfo(i + 1, _playerRaces[i + 1], _playerColors[i + 1]);
				}
			}
			/*		

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
			if (_playerEmperorName.KeyDown(e))
			{
				return;
			}
			if (_playerHomeworldName.KeyDown(e))
			{
				return;
			}
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

		private void OnRaceSelectionOKClick(int whichPlayer, Race whichRace, Color color)
		{
			_showingSelection = false;
			for (int i = 0; i < _playerRaces.Length; i++)
			{
				if (i != whichPlayer && _playerRaces[i] == whichRace)
				{
					_playerRaces[i] = null;
					_raceSprites[i] = _randomRaceSprite;
					break;
				}
			}
			_playerRaces[whichPlayer] = whichRace;
			_raceSprites[whichPlayer] = whichRace.MiniAvatar;
			for (int i = 0; i < _playerColors.Length; i++)
			{
				if (i != whichPlayer && _playerColors[i] == color)
				{
					_playerColors[i] = _playerColors[whichPlayer];
					_playerColors[whichPlayer] = color;
					break;
				}
			}
		}
	}
}
