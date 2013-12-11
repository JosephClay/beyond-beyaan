using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class FleetSpecsWindow : WindowInterface
	{
		private BBStretchButton[] _shipButtons;
		private BBButton[] _scrapButtons;
		private BBSprite[] _shipSprites;
		private BBLabel[] _shipNameLabels;
		private BBStretchableImage _equipmentBackground;
		private BBStretchableImage _weaponsBackground;
		private BBStretchableImage _specialsBackground;

		private BBLabel[] _equipmentLabels;
		private BBLabel[] _weaponLabels;
		private BBLabel[] _specialLabels;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 350;
			int y = (gameMain.ScreenHeight / 2) - 320;
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 370, (gameMain.ScreenHeight / 2) - 320, 740, 640, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}
			_shipButtons = new BBStretchButton[6];
			_scrapButtons = new BBButton[6];
			_shipSprites = new BBSprite[6];
			_shipNameLabels = new BBLabel[6];
			for (int i = 0; i < 6; i++)
			{
				_shipButtons[i] = new BBStretchButton();
				_scrapButtons[i] = new BBButton();
				_shipNameLabels[i] = new BBLabel();
			}
			if (!_shipButtons[0].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, y, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipButtons[1].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, y + 200, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipButtons[2].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x, y + 400, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipButtons[3].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 200, y, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipButtons[4].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 200, y + 200, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipButtons[5].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 200, y + 400, 200, 200, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[0].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 68, y + 155, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[1].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 68, y + 355, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[2].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 68, y + 555, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[3].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 268, y + 155, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[4].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 268, y + 355, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrapButtons[5].Initialize("ScrapShipBG", "ScrapShipFG", string.Empty, ButtonTextAlignment.LEFT, x + 268, y + 555, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 6; i++)
			{
				if (!_scrapButtons[i].SetToolTip("ScrapShipToolTip" + i, "Scrap Ship Design", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
				{
					return false;
				}
			}
			if (!_shipNameLabels[0].Initialize(x + 10, y + 10, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_shipNameLabels[1].Initialize(x + 10, y + 210, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_shipNameLabels[2].Initialize(x + 10, y + 410, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_shipNameLabels[3].Initialize(x + 210, y + 10, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_shipNameLabels[4].Initialize(x + 210, y + 210, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_shipNameLabels[5].Initialize(x + 210, y + 410, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_equipmentBackground = new BBStretchableImage();
			_weaponsBackground = new BBStretchableImage();
			_specialsBackground = new BBStretchableImage();

			if (!_equipmentBackground.Initialize(x + 400, y, 300, 200, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_weaponsBackground.Initialize(x + 400, y + 200, 300, 200, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_specialsBackground.Initialize(x + 400, y + 400, 300, 200, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			_equipmentLabels = new BBLabel[5];
			for (int i = 0; i < 5; i++)
			{
				_equipmentLabels[i] = new BBLabel();
				if (!_equipmentLabels[i].Initialize(x + 420, y + 20 + (i * 30), string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
			}
			_weaponLabels = new BBLabel[4];
			for (int i = 0; i < 4; i++)
			{
				_weaponLabels[i] = new BBLabel();
				if (!_weaponLabels[i].Initialize(x + 420, y + 220 + (i * 30), string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
			}
			_specialLabels = new BBLabel[3];
			for (int i = 0; i < 3; i++)
			{
				_specialLabels[i] = new BBLabel();
				if (!_specialLabels[i].Initialize(x + 420, y + 420 + (i * 30), string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
			}

			reason = null;
			return true;
		}

		public void LoadDesigns()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			bool moreThanOneDesign = currentEmpire.FleetManager.CurrentDesigns.Count > 1;
			for (int i = 0; i < 6; i++)
			{
				if (i < currentEmpire.FleetManager.CurrentDesigns.Count)
				{
					var shipDesign = currentEmpire.FleetManager.CurrentDesigns[i];
					_shipSprites[i] = currentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
					_shipNameLabels[i].SetText(string.Format("{0} ({1})", shipDesign.Name, currentEmpire.FleetManager.GetShipCount(shipDesign)));
					_scrapButtons[i].Active = moreThanOneDesign;
					_shipButtons[i].Enabled = true;
				}
				else
				{
					_shipSprites[i] = null;
					_shipNameLabels[i].SetText(string.Empty);
					_scrapButtons[i].Active = false;
					_shipButtons[i].Enabled = false;
				}
			}
			LoadShipStats(0);
		}

		private void LoadShipStats(int whichShip)
		{
			for (int i = 0; i < 6; i++)
			{
				_shipButtons[i].Selected = false;
			}
			_shipButtons[whichShip].Selected = true;
			var ship = _gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns[whichShip];
			_equipmentLabels[0].SetText(ship.Armor.DisplayName);
			_equipmentLabels[1].SetText(ship.Engine.Key.DisplayName);
			if (ship.Shield != null)
			{
				_equipmentLabels[2].SetText(ship.Shield.DisplayName);
			}
			else
			{
				_equipmentLabels[2].SetText("No Shield");
			}
			if (ship.ECM != null)
			{
				_equipmentLabels[2].SetText(ship.ECM.DisplayName);
			}
			else
			{
				_equipmentLabels[2].SetText("No ECM");
			}
			if (ship.Computer != null)
			{
				_equipmentLabels[2].SetText(ship.Computer.DisplayName);
			}
			else
			{
				_equipmentLabels[2].SetText("No Computer");
			}
			for (int i = 0; i < 4; i++)
			{
				if (ship.Weapons[i].Key != null)
				{
					_weaponLabels[i].SetText(string.Format("({0}) {1}", ship.Weapons[i].Value, ship.Weapons[i].Key.DisplayName));
				}
				else
				{
					_weaponLabels[i].SetText(string.Empty);
				}
			}
			for (int i = 0; i < 3; i++)
			{
				if (ship.Specials[i] != null)
				{
					_specialLabels[i].SetText(ship.Specials[i].DisplayName);
				}
				else
				{
					_weaponLabels[i].SetText(string.Empty);
				}
			}
		}

		public override void Draw()
		{
			base.Draw();
			for (int i = 0; i < 6; i++)
			{
				_shipButtons[i].Draw();
				_shipNameLabels[i].Draw();
				_scrapButtons[i].Draw();
			}
			_equipmentBackground.Draw();
			_weaponsBackground.Draw();
			_specialsBackground.Draw();
			for (int i = 0; i < 5; i++)
			{
				_equipmentLabels[i].Draw();
			}
			for (int i = 0; i < 4; i++)
			{
				_weaponLabels[i].Draw();
			}
			for (int i = 0; i < 3; i++)
			{
				_specialLabels[i].Draw();
			}
		}
	}
}
