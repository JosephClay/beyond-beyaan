using System;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	class RaceSelection : WindowInterface
	{
		private BBSprite _randomSprite;
		private BBStretchButton[] _raceButtons;
		private BBStretchableImage _raceBackground;
		private BBStretchButton[] _colorButtons;
		private BBScrollBar _raceScrollBar;

		public int WhichPlayerSelecting { get; private set; }
		public Race WhichRaceSelected { get; private set; }
		public Color WhichColorSelected { get; private set; }

		public Action<int, Race, Color> OnOkClick { get; set; }

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			WhichPlayerSelecting = 0;

			if (!base.Initialize((gameMain.ScreenWidth / 2) - 350, (gameMain.ScreenHeight / 2) - 320, 700, 640, StretchableImageType.MediumBorder, gameMain, true, gameMain.Random, out reason))
			{
				return false;
			}
			_randomSprite = SpriteManager.GetSprite("RandomRace", gameMain.Random);
			if (_randomSprite == null)
			{
				reason = "RandomRace sprite does not exist.";
				return false;
			}

			_raceButtons = new BBStretchButton[4];
			_colorButtons = new BBStretchButton[6];
			_raceScrollBar = new BBScrollBar();
			_raceBackground = new BBStretchableImage();

			for (int i = 0; i < _raceButtons.Length; i++)
			{
				_raceButtons[i] = new BBStretchButton();
				if (!_raceButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 10, yPos + 10 + (i * 150), 350, 150, gameMain.Random, out reason))
				{
					return false;
				}
			}
			int minRaces = gameMain.RaceManager.Races.Count < 4 ? 4 : gameMain.RaceManager.Races.Count;
			if (!_raceScrollBar.Initialize(xPos + 360, yPos + 10, 600, 4, minRaces, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			if (gameMain.RaceManager.Races.Count <= 4)
			{
				_raceScrollBar.SetEnabledState(false);
			}
			if (!_raceBackground.Initialize(xPos + 380, yPos + 10, 310, 310, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _colorButtons.Length; i++)
			{
				_colorButtons[i] = new BBStretchButton();
				if (!_colorButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 380 + (i * 50), yPos + 330, 50, 50, gameMain.Random, out reason))
				{
					return false;
				}
			}
			reason = null;
			return true;
		}

		public void SetCurrentPlayerInfo(int whichPlayer, Race race, Color color)
		{
			WhichPlayerSelecting = whichPlayer;
			WhichRaceSelected = race;
			WhichColorSelected = color;
		}

		public override void Draw()
		{
			base.Draw();
			_raceBackground.Draw();
			for (int i = 0; i < _raceButtons.Length; i++)
			{
				_raceButtons[i].Draw();
			}
			_raceScrollBar.Draw();
			for (int i = 0; i < _colorButtons.Length; i++)
			{
				_colorButtons[i].Draw();
			}
		}
	}
}
