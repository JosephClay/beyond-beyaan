using System;
using System.Collections.Generic;
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

		private BBStretchableImage _engineBackground;
		private BBStretchButton _engineButton;
		private BBStretchButton _maneuverButton;
		private BBLabel _engineSpeed;
		private BBLabel _combatSpeed;
		private BBLabel _costPerPower;
		private BBLabel _spacePerPower;
		private BBLabel _defenseRating;

		private BBStretchableImage _defensiveEquipmentBackground;
		private BBStretchButton _armorButton;
		private BBStretchButton _shieldButton;
		private BBStretchButton _ECMButton;
		private BBLabel _hitPointsLabel;
		private BBLabel _absorbtionLabel;
		private BBLabel _missileDefenseLabel;

		private BBStretchableImage _computerBackground;
		private BBStretchButton _computerButton;
		private BBLabel _attackRating;

		private BBStretchableImage _weaponsBackground;
		private BBStretchButton[] _weaponButtons;
		private BBNumericUpDown[] _weaponCounts;
		private BBLabel[] _weaponCountLabels;
		private BBLabel[] _weaponDescriptions;

		private BBSprite _shipSprite;

		private Ship _shipDesign;
		private Dictionary<TechField, int> _techLevels;

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

			_engineBackground = new BBStretchableImage();
			_engineButton = new BBStretchButton();
			_maneuverButton = new BBStretchButton();
			_engineSpeed = new BBLabel();
			_combatSpeed = new BBLabel();
			_costPerPower = new BBLabel();
			_spacePerPower = new BBLabel();
			_defenseRating = new BBLabel();

			if (!_engineBackground.Initialize(x + 15, y + 15, 300, 180, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_engineButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 25, y + 25, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_maneuverButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 25, y + 62, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_engineSpeed.Initialize(x + 25, y + 100, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_combatSpeed.Initialize(x + 165, y + 100, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_costPerPower.Initialize(x + 25, y + 120, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_spacePerPower.Initialize(x + 25, y + 140, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_defenseRating.Initialize(x + 25, y + 160, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_defensiveEquipmentBackground = new BBStretchableImage();
			_armorButton = new BBStretchButton();
			_shieldButton = new BBStretchButton();
			_ECMButton = new BBStretchButton();
			_hitPointsLabel = new BBLabel();
			_absorbtionLabel = new BBLabel();
			_missileDefenseLabel = new BBLabel();
			if (!_defensiveEquipmentBackground.Initialize(x + 15, y + 195, 300, 190, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_armorButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 25, y + 206, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shieldButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 25, y + 243, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_ECMButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 25, y + 280, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_hitPointsLabel.Initialize(x + 25, y + 315, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_absorbtionLabel.Initialize(x + 25, y + 335, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_missileDefenseLabel.Initialize(x + 25, y + 355, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_computerBackground = new BBStretchableImage();
			_computerButton = new BBStretchButton();
			_attackRating = new BBLabel();

			if (!_computerBackground.Initialize(x + 315, y + 15, 470, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_computerButton.Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 325, y + 25, 280, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_attackRating.Initialize(x + 610, y + 30, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			_weaponsBackground = new BBStretchableImage();
			_weaponButtons = new BBStretchButton[4];
			_weaponCountLabels = new BBLabel[4];
			_weaponDescriptions = new BBLabel[4];
			_weaponCounts = new BBNumericUpDown[4];

			if (!_weaponsBackground.Initialize(x + 315, y + 70, 470, 220, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			for (int i = 0; i < 4; i++)
			{
				_weaponButtons[i] = new BBStretchButton();
				_weaponCountLabels[i] = new BBLabel();
				_weaponDescriptions[i] = new BBLabel();
				_weaponCounts[i] = new BBNumericUpDown();

				if (!_weaponButtons[i].Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 325, y + 80 + (i * 50), 280, 30, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_weaponCountLabels[i].Initialize(x + 695, y + 85 + (i * 50), "Count:", System.Drawing.Color.White, out reason))
				{
					return false;
				}
				_weaponCountLabels[i].SetAlignment(true);
				if (!_weaponCounts[i].Initialize(x + 700, y + 85 + (i * 50), 70, 1, 99, 1, 1, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_weaponDescriptions[i].Initialize(x + 325, y + 112, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
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
			_engineBackground.Draw();
			_engineButton.Draw();
			_maneuverButton.Draw();
			_engineSpeed.Draw();
			_combatSpeed.Draw();
			_costPerPower.Draw();
			_spacePerPower.Draw();
			_defenseRating.Draw();
			_defensiveEquipmentBackground.Draw();
			_armorButton.Draw();
			_shieldButton.Draw();
			_ECMButton.Draw();
			_hitPointsLabel.Draw();
			_absorbtionLabel.Draw();
			_missileDefenseLabel.Draw();
			_computerBackground.Draw();
			_computerButton.Draw();
			_attackRating.Draw();
			_weaponsBackground.Draw();
			for (int i = 0; i < 4; i++)
			{
				_weaponButtons[i].Draw();
				_weaponCountLabels[i].Draw();
				_weaponCounts[i].Draw();
				_weaponDescriptions[i].Draw();
			}
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
					RefreshAll();
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
			_techLevels = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.GetFieldLevels();
			RefreshAll();
		}

		private void RefreshAll()
		{
			//Used when changing ship size or initial loading of ship design
			RefreshShipSprite();
			RefreshEngineLabels();
			RefreshShield();
			RefreshHP();
			RefreshECM();
			RefreshComputer();
			RefreshWeapons();
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

		private void RefreshEngineLabels()
		{
			_engineButton.SetText(_shipDesign.Engine.Key.DisplayName);
			_maneuverButton.SetText(string.Format("{0} Maneuverability", _shipDesign.ManeuverSpeed.ToString()));
			_engineSpeed.SetText(string.Format("Galaxy Speed: {0}", _shipDesign.GalaxySpeed));
			_combatSpeed.SetText(string.Format("Combat Speed: {0}", (_shipDesign.ManeuverSpeed / 2) + 1));
			_costPerPower.SetText(string.Format("Cost per Power: {0:0.0} BCs", _shipDesign.Engine.Key.GetCost(_techLevels, _shipDesign.Size) / (_shipDesign.GalaxySpeed * 10)));
			_spacePerPower.SetText(string.Format("Space per Power: {0:0.0} DWT", _shipDesign.Engine.Key.GetSize(_techLevels, _shipDesign.Size) / (_shipDesign.GalaxySpeed * 10)));
			_defenseRating.SetText(string.Format("{0} Defense Rating", _shipDesign.DefenseRating));
		}

		private void RefreshHP()
		{
			_armorButton.SetText(_shipDesign.Armor.DisplayName);
			_hitPointsLabel.SetText(string.Format("{0} Hit Points", _shipDesign.MaxHitPoints));
		}

		private void RefreshShield()
		{
			if (_shipDesign.Shield != null)
			{
				_shieldButton.SetText(_shipDesign.Shield.DisplayName);
				_absorbtionLabel.SetText(string.Format("Absorbs {0} damage", _shipDesign.Shield.Technology.Shield));
			}
			else
			{
				_shieldButton.SetText("No Shield");
				_absorbtionLabel.SetText("Absorbs 0 damage");
			}
		}

		private void RefreshECM()
		{
			if (_shipDesign.ECM != null)
			{
				_ECMButton.SetText(_shipDesign.ECM.DisplayName);
				_missileDefenseLabel.SetText(string.Format("{0} Missile Defense Rating", _shipDesign.ECM.Technology.ECM));
			}
			else
			{
				_ECMButton.SetText("No ECM");
				_missileDefenseLabel.SetText("0 Missile Defense Rating");
			}
		}

		private void RefreshComputer()
		{
			if (_shipDesign.Computer != null)
			{
				_computerButton.SetText(_shipDesign.Computer.DisplayName);
				_attackRating.SetText(string.Format("{0} Attack Rating", _shipDesign.Computer.Technology.BattleComputer));
			}
			else
			{
				_computerButton.SetText("No Computer");
				_attackRating.SetText("0 Attack Rating");
			}
		}

		private void RefreshWeapons()
		{
			for (int i = 0; i < 4; i++)
			{
				if (i < _shipDesign.Weapons.Count)
				{
					var weapon = _shipDesign.Weapons[i];
					if (weapon.Key != null)
					{
						_weaponButtons[i].SetText(weapon.Key.DisplayName);
						_weaponCounts[i].SetValue(weapon.Value);
						_weaponCounts[i].Enabled = true;
						string description = string.Format("Damage: {0}-{1}		Range: {2}		", weapon.Key.GetMinDamage(), weapon.Key.GetMaxDamage(), weapon.Key.GetRange());
						if (weapon.Key.Technology.WeaponType == Technology.MISSILE_WEAPON)
						{
							description += string.Format("{0} Shots		", weapon.Key.UseSecondary ? 5 : 2);
						}
						else if (weapon.Key.Technology.NumberOfShots > 0)
						{
							description += string.Format("{0} Shots		", weapon.Key.Technology.NumberOfShots);
						}
						if (weapon.Key.Technology.Streaming)
						{
							description += "Streaming, ";
						}
						if (weapon.Key.Technology.ShieldPiercing)
						{
							description += "Piercing, ";
						}
						if (weapon.Key.Technology.Enveloping)
						{
							description += "Enveloping, ";
						}
						if (weapon.Key.Technology.Dissipating)
						{
							description += "Dissipating, ";
						}
						if (weapon.Key.Technology.TargetingBonus > 0)
						{
							description += string.Format("{0} To Hit ", weapon.Key.Technology.TargetingBonus);
						}
						description = description.Trim().TrimEnd(new[] {','});
						_weaponDescriptions[i].SetText(description);
					}
					else
					{
						_weaponButtons[i].SetText("No Weapon");
						_weaponCounts[i].SetValue(1);
						_weaponCounts[i].Enabled = false;
						_weaponDescriptions[i].SetText(string.Empty);
					}
				}
				else
				{
					_weaponButtons[i].SetText("No Weapon");
					_weaponCounts[i].SetValue(1);
					_weaponCounts[i].Enabled = false;
					_weaponDescriptions[i].SetText(string.Empty);
				}
			}
		}
	}
}
