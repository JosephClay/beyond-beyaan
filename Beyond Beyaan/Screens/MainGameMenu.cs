using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class MainGameMenu : ScreenInterface
	{
		private BBSprite _background;
		private BBSprite _planet;
		private BBSprite _title;
		private GameMain _gameMain;
		private BBButton[] _buttons;
		private BBLabel _versionLabel;
		private int _x;
		private int _y;

		private List<FileInfo> _files;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this._gameMain = gameMain;

			_buttons = new BBButton[4];

			_x = (gameMain.ScreenWidth / 2) - 130;
			_y = (gameMain.ScreenHeight / 2);

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i] = new BBButton();
			}

			if (!_buttons[0].Initialize("ContinueGameBG", "ContinueGameFG", string.Empty, ButtonTextAlignment.CENTER, _x, _y, 260, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_buttons[1].Initialize("NewGameBG", "NewGameFG", string.Empty, ButtonTextAlignment.CENTER, _x, _y + 50, 260, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_buttons[2].Initialize("LoadGameBG", "LoadGameFG", string.Empty, ButtonTextAlignment.CENTER, _x, _y + 100, 260, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_buttons[3].Initialize("ExitGameBG", "ExitGameFG", string.Empty, ButtonTextAlignment.CENTER, _x, _y + 150, 260, 40, gameMain.Random, out reason))
			{
				return false;
			}

			_versionLabel = new BBLabel();
			if (!_versionLabel.Initialize(10, _gameMain.ScreenHeight - 30, "Version 0.5", System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_background = SpriteManager.GetSprite("MainBackground", gameMain.Random);
			_planet = SpriteManager.GetSprite("MainPlanetBackground", gameMain.Random);
			_title = SpriteManager.GetSprite("Title", gameMain.Random);

			_x = (gameMain.ScreenWidth / 2) - 512;
			_y = (gameMain.ScreenHeight / 2) - 300;

			_files = Utility.GetSaveGames(gameMain.GameDataSet.FullName);
			if (_files.Count == 0)
			{
				_buttons[0].Active = false; //Disabled Continue and Load buttons since there's no games to load
				_buttons[2].Active = false;
			}

			reason = null;
			return true;
		}

		public void DrawScreen()
		{
			_background.Draw(0, 0, _gameMain.ScreenWidth / _background.Width, _gameMain.ScreenHeight / _background.Height);
			_planet.Draw(_x, _y);
			_title.Draw(_x, _y);
			foreach (BBButton button in _buttons)
			{
				button.Draw();
			}
			_versionLabel.Draw();
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			foreach (BBButton button in _buttons)
			{
				button.MouseHover(x, y, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			foreach (BBButton button in _buttons)
			{
				button.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < _buttons.Length; i++)
			{
				if (_buttons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0: //Continue button
							FileInfo mostRecentGame = null;
							foreach (var file in _files)
							{
								if (mostRecentGame == null)
								{
									mostRecentGame = file;
								}
								else if (mostRecentGame.LastWriteTime.CompareTo(file.LastWriteTime) < 0)
								{
									mostRecentGame = file;
								}
							}
							if (mostRecentGame != null)
							{
								_gameMain.LoadGame(mostRecentGame.Name);
								_gameMain.ChangeToScreen(Screen.Galaxy);
							}
							break;
						case 1: //New Game button
							_gameMain.ChangeToScreen(Screen.NewGame);
							break;
						case 3:
							_gameMain.ExitGame();
							break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}
	}
}
