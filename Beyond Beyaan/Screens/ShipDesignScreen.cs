using System;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class ShipDesignScreen : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchableImage _shipStyleBackground;
		private BBStretchButton[] _shipSizeButtons;
		private BBButton _prevShipStyleButton;
		private BBButton _nextShipStyleButton;

		private BBSprite _shipSprite;

		private Ship _shipDesign;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 800, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}
			_shipStyleBackground = new BBStretchableImage();
			_shipSizeButtons = new BBStretchButton[4];
			_prevShipStyleButton = new BBButton();
			_nextShipStyleButton = new BBButton();

			if (!_shipStyleBackground.Initialize(x + 15, y + 385, 220, 200, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_prevShipStyleButton.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", string.Empty, ButtonTextAlignment.CENTER, x + 22, y + 477, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_nextShipStyleButton.Initialize("ScrollRightBGButton", "ScrollRightFGButton", string.Empty, ButtonTextAlignment.CENTER, x + 212, y + 477, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _shipSizeButtons.Length; i++)
			{
				_shipSizeButtons[i] = new BBStretchButton();
			}
			if (!_shipSizeButtons[0].Initialize("Small", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 235, y + 385, 80, 50, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipSizeButtons[1].Initialize("Medium", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 235, y + 435, 80, 50, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipSizeButtons[2].Initialize("Large", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 235, y + 485, 80, 50, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipSizeButtons[3].Initialize("Huge", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 235, y + 535, 80, 50, gameMain.Random, out reason))
			{
				return false;
			}

			return true;
		}

		public override void Draw()
		{
			base.Draw();
			_shipStyleBackground.Draw();
			foreach (var button in _shipSizeButtons)
			{
				button.Draw();
			}
			_prevShipStyleButton.Draw();
			_nextShipStyleButton.Draw();
			GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
			_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(_gameMain.EmpireManager.CurrentEmpire.ConvertedColor);
			_shipSprite.Draw(_xPos + 125, _yPos + 485);
			GorgonLibrary.Gorgon.CurrentShader = null;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			foreach (var button in _shipSizeButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			result = _prevShipStyleButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _nextShipStyleButton.MouseHover(x, y, frameDeltaTime) || result;

			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			foreach (var button in _shipSizeButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			result = _prevShipStyleButton.MouseDown(x, y) || result;
			result = _nextShipStyleButton.MouseDown(x, y) || result;
			return base.MouseDown(x, y) || result;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < _shipSizeButtons.Length; i++)
			{
				if (_shipSizeButtons[i].MouseUp(x, y))
				{
					_shipDesign.Size = i;
					RefreshShipSprite();
					return true;
				}
			}
			if (_prevShipStyleButton.MouseUp(x, y))
			{
				_shipDesign.WhichStyle--;
				if (_shipDesign.WhichStyle < 0)
				{
					_shipDesign.WhichStyle = 5;
				}
				RefreshShipSprite();
				return true;
			}
			if (_nextShipStyleButton.MouseUp(x, y))
			{
				_shipDesign.WhichStyle++;
				if (_shipDesign.WhichStyle > 5)
				{
					_shipDesign.WhichStyle = 0;
				}
				RefreshShipSprite();
				return true;
			}
			if (!base.MouseUp(x, y))
			{
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return false;
		}

		public void Load()
		{
			_shipDesign = _gameMain.EmpireManager.CurrentEmpire.FleetManager.LastShipDesign;
			RefreshShipSprite();
		}

		private void RefreshShipSprite()
		{
			var empire = _gameMain.EmpireManager.CurrentEmpire;
			//Load up the last ship design
			_shipSprite = empire.EmpireRace.GetShip(_shipDesign.Size, _shipDesign.WhichStyle);
			foreach (var button in _shipSizeButtons)
			{
				button.Selected = false;
			}
			_shipSizeButtons[_shipDesign.Size].Selected = true;
		}
	}
}
