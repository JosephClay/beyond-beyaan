using System;
using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class InGameMenu : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchButton[] _buttons;
		private BBStretchableImage _saveGameListBackground;
		private BBInvisibleStretchButton[] _saveGameButtons;
		private BBScrollBar _scrollBar;
		private int _maxVisible;
		private int _selectedGame;

		private List<string> _fileNames;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 250, (gameMain.ScreenHeight / 2) - 200, 500, 400, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_buttons = new BBStretchButton[4];

			_buttons[0] = new BBStretchButton();
			if (!_buttons[0].Initialize("New Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 30, yPos + 350, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[1] = new BBStretchButton();
			if (!_buttons[1].Initialize("Save Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 270, yPos + 300, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[2] = new BBStretchButton();
			if (!_buttons[2].Initialize("Load Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 30, yPos + 300, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}
			_buttons[2].Enabled = false;

			_buttons[3] = new BBStretchButton();
			if (!_buttons[3].Initialize("Exit Game", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 270, yPos + 350, 200, 35, gameMain.Random, out reason))
			{
				return false;
			}

			_saveGameListBackground = new BBStretchableImage();
			if (!_saveGameListBackground.Initialize(xPos + 20, yPos + 20, 460, 325, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			_saveGameButtons = new BBInvisibleStretchButton[8];
			for (int i = 0; i < _saveGameButtons.Length; i++)
			{
				_saveGameButtons[i] = new BBInvisibleStretchButton();
				if (!_saveGameButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, xPos + 30, yPos + 35 + (i * 32), 420, 32, gameMain.Random, out reason))
				{
					return false;
				}
			}

			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(xPos + 455, yPos + 37, 256, _saveGameButtons.Length, _saveGameButtons.Length, false, false, gameMain.Random, out reason))
			{
				return false;
			}

			_maxVisible = 0;
			_scrollBar.SetEnabledState(false);
			_selectedGame = -1;
			_fileNames = new List<string>();

			return true;
		}

		public override void Draw()
		{
			base.Draw();

			_saveGameListBackground.Draw();

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i].Draw();
			}

			for (int i = 0; i < _maxVisible; i++)
			{
				_saveGameButtons[i].Draw();
			}

			_scrollBar.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			for (int i = 0; i < _buttons.Length; i++)
			{
				result = _buttons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _saveGameButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (_scrollBar.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
				RefreshSaveButtons();
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = _scrollBar.MouseDown(x, y);
			for (int i = 0; i < _buttons.Length; i++)
			{
				result = _buttons[i].MouseDown(x, y) || result;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _saveGameButtons[i].MouseDown(x, y) || result;
			}
			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			if (_scrollBar.MouseUp(x, y))
			{
				RefreshSaveButtons();
				return true;
			}
			if (_buttons[0].MouseUp(x, y))
			{
				_gameMain.ClearAll();
				_gameMain.ChangeToScreen(Screen.NewGame);
				return true;
			}
			if (_buttons[1].MouseUp(x, y))
			{
				//TODO: Add a prompt for save file name
				_gameMain.SaveGame("TestSave");
				return true;
			}
			if (_buttons[2].MouseUp(x, y))
			{
				//StartLoadGameCommand
				//_gameMain.LoadGame(saveFileName);
				//FinishLoadGameCommand
				return true;
			}
			if (_buttons[3].MouseUp(x, y))
			{
				//TODO: Add prompt to ensure user really want to exit
				_gameMain.ExitGame();
				return true;
			}

			for (int i = 0; i < _maxVisible; i++)
			{
				if (_saveGameButtons[i].MouseUp(x, y))
				{
					foreach (var button in _saveGameButtons)
					{
						button.Selected = false;
					}
					_saveGameButtons[i].Selected = true;
					_selectedGame = i + _scrollBar.TopIndex;
					return true;
				}
			}

			if (!base.MouseUp(x, y))
			{
				//Clicked outside the window, close this window
				if (CloseWindow != null)
				{
					CloseWindow();
				}
			}
			return false;
		}

		public void GetSaveList()
		{
			//TODO: Retrieve a list of save games here
		}

		private void RefreshSaveButtons()
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				_saveGameButtons[i].SetText(_fileNames[i + _scrollBar.TopIndex]);
				_saveGameButtons[i].Selected = false;
				if (i + _scrollBar.TopIndex == _selectedGame)
				{
					_saveGameButtons[i].Selected = true;
				}
			}
		}
	}
}
