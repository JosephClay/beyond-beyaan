﻿using System;
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
		private BBLabel _costPerPowerLabel;
		private BBLabel _spacePerPowerLabel;
		private BBLabel _defenseRating;
		private float _costPerPower;
		private float _spacePerPower;

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

		private BBStretchableImage _specialsBackground;
		private BBStretchButton[] _specialButtons;
		private BBTextBox[] _specialDescriptions;

		private BBStretchableImage _statsBackground;
		private BBLabel _spaceLabel;
		private BBLabel _costLabel;
		private BBSingleLineTextBox _nameField;

		private BBButton _clearButton;
		private BBButton _confirmButton;

		private BBSprite _shipSprite;

		private Ship _shipDesign;
		private Dictionary<TechField, int> _techLevels;
		private List<Technology> _availableComputerTechs;
		private List<Technology> _availableShieldTechs;
		private List<Technology> _availableEngineTechs;
		private List<Technology> _availableArmorTechs;
		private List<Technology> _availableECMTechs;
		private List<Technology> _availableWeaponTechs;
		private List<Technology> _availableSpecialTechs; 

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
			_costPerPowerLabel = new BBLabel();
			_spacePerPowerLabel = new BBLabel();
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
			if (!_costPerPowerLabel.Initialize(x + 25, y + 120, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_spacePerPowerLabel.Initialize(x + 25, y + 140, string.Empty, System.Drawing.Color.White, out reason))
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

			_specialsBackground = new BBStretchableImage();
			_specialButtons = new BBStretchButton[3];
			_specialDescriptions = new BBTextBox[3];

			if (!_specialsBackground.Initialize(x + 315, y + 290, 470, 230, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				_specialButtons[i] = new BBStretchButton();
				_specialDescriptions[i] = new BBTextBox();

				if (!_specialButtons[i].Initialize(string.Empty, ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 325, y + 300 + (i * 70), 450, 30, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_specialDescriptions[i].Initialize(x + 325, y + 332 + (i * 70), 450, 38, true, true, "SpecialDesc" + i, gameMain.Random, out reason))
				{
					return false;
				}
			}

			_statsBackground = new BBStretchableImage();
			_spaceLabel = new BBLabel();
			_costLabel = new BBLabel();
			_nameField = new BBSingleLineTextBox();

			if (!_statsBackground.Initialize(x + 315, y + 520, 470, 65, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_spaceLabel.Initialize(x + 450, y + 559, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_costLabel.Initialize(x + 325, y + 559, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_nameField.Initialize(string.Empty, x + 325, y + 527, 250, 30, false, gameMain.Random, out reason))
			{
				return false;
			}

			_clearButton = new BBButton();
			_confirmButton = new BBButton();

			if (!_clearButton.Initialize("CancelBG", "CancelFG", string.Empty, ButtonTextAlignment.CENTER, x + 595, y + 535, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_confirmButton.Initialize("ConfirmBG", "ConfirmFG", string.Empty, ButtonTextAlignment.CENTER, x + 685, y + 535, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_clearButton.SetToolTip("ClearDesign", "Clear Ship Design", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_confirmButton.SetToolTip("ConfirmDesign", "Add Ship Design", gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
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
			_engineBackground.Draw();
			_engineButton.Draw();
			_maneuverButton.Draw();
			_engineSpeed.Draw();
			_combatSpeed.Draw();
			_costPerPowerLabel.Draw();
			_spacePerPowerLabel.Draw();
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
			_specialsBackground.Draw();
			for (int i = 0; i < 3; i++)
			{
				_specialButtons[i].Draw();
				_specialDescriptions[i].Draw();
			}
			_statsBackground.Draw();
			_costLabel.Draw();
			_spaceLabel.Draw();
			_nameField.Draw();
			_clearButton.Draw();
			_confirmButton.Draw();
			_clearButton.DrawToolTip();
			_confirmButton.DrawToolTip();
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

			result = _engineButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _maneuverButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _armorButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _shieldButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _ECMButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _computerButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _confirmButton.MouseHover(x, y, frameDeltaTime) || result;
			result = _clearButton.MouseHover(x, y, frameDeltaTime) || result;

			for (int i = 0; i < _weaponButtons.Length; i++)
			{
				result = _weaponButtons[i].MouseHover(x, y, frameDeltaTime) || result;
				result = _weaponCounts[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			for (int i = 0; i < _specialButtons.Length; i++)
			{
				result = _specialButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}

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
			LoadTechnologies();
			RefreshAll();
			_nameField.SetText(_shipDesign.Name);
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
			RefreshSpecials();
			RefreshStats();
			RefreshValidButtons();
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
			_costPerPower = _shipDesign.Engine.Key.GetCost(_techLevels, _shipDesign.Size) / (_shipDesign.GalaxySpeed * 10);
			_spacePerPower = _shipDesign.Engine.Key.GetSize(_techLevels, _shipDesign.Size) / (_shipDesign.GalaxySpeed * 10);
			_costPerPowerLabel.SetText(string.Format("Cost per Power: {0:0.0} BCs", _costPerPower));
			_spacePerPowerLabel.SetText(string.Format("Space per Power: {0:0.0} DWT", _spacePerPower));
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
		}

		private void RefreshSpecials()
		{
			for (int i = 0; i < 3; i++)
			{
				if (i < _shipDesign.Specials.Count)
				{
					_specialButtons[i].SetText(_shipDesign.Specials[i].DisplayName);
					_specialDescriptions[i].SetText(_shipDesign.Specials[i].Technology.TechDescription);
				}
				else
				{
					_specialButtons[i].SetText("No Special");
					_specialDescriptions[i].SetText(string.Empty);
				}
			}
		}

		private void RefreshStats()
		{
			_costLabel.SetText(string.Format("Cost: {0:0.0} BCs", _shipDesign.Cost));
			_spaceLabel.SetText(string.Format("Space: {0:0.0}/{1:0}", _shipDesign.SpaceUsed, _shipDesign.TotalSpace));
		}

		private void RefreshValidButtons()
		{
			//This function greys out all illegal buttons, and activates all legal buttons.  Illegal meaning that there isn't enough space for incrementation, or decrementation if the weapon count is 1
			float remainingSpace = _shipDesign.TotalSpace - _shipDesign.SpaceUsed;

			_computerButton.SetTextColor(AtLeastOneBetterComputer(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
			_shieldButton.SetTextColor(AtLeastOneBetterShield(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
			_ECMButton.SetTextColor(AtLeastOneBetterECM(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
			_engineButton.SetTextColor(AtLeastOneBetterEngine(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
			_maneuverButton.SetTextColor(AtLeastOneBetterManeuver(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
			_armorButton.SetTextColor(AtLeastOneBetterArmor(remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);

			for (int i = 0; i < 4; i++)
			{
				if (_shipDesign.Weapons.Count > i)
				{
					_weaponButtons[i].SetTextColor(AtLeastOneBetterWeapon(_shipDesign.Weapons[i], remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_weaponCounts[i].Enabled = true;
					if (_shipDesign.Weapons[i].Key.GetSize(_techLevels, _shipDesign.Size) > remainingSpace)
					{
						_weaponCounts[i].UpButtonEnabled = false;
					}
					else
					{
						_weaponCounts[i].UpButtonEnabled = true;
					}
				}
				else
				{
					_weaponButtons[i].SetTextColor(AtLeastOneBetterWeapon(new KeyValuePair<Equipment, int>(null, 0), remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_weaponCounts[i].Enabled = false;
				}
			}
			for (int i = 0; i < 3; i++)
			{
				if (_shipDesign.Specials.Count > i)
				{
					_specialButtons[i].SetTextColor(AtLeastOneHigherLevelSpecial(_shipDesign.Specials[i], remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
				}
				else
				{
					_specialButtons[i].SetTextColor(AtLeastOneHigherLevelSpecial(null, remainingSpace) ? System.Drawing.Color.White : System.Drawing.Color.Tan, System.Drawing.Color.Empty);
				}
			}
		}

		private bool AtLeastOneBetterComputer(float remainingSpace)
		{
			if (_shipDesign.Computer != null && _shipDesign.Computer.Technology == _availableComputerTechs[_availableComputerTechs.Count - 1])
			{
				//Already the best computer
				return false;
			}
			int index = (_shipDesign.Computer == null ? -1 : _availableComputerTechs.IndexOf(_shipDesign.Computer.Technology)) + 1; //Just one level higher will suffice, and we know we will always have at least one computer tech
			return GetSpaceUsed(_availableComputerTechs[index], false) <= remainingSpace;
		}
		private bool AtLeastOneBetterECM(float remainingSpace)
		{
			if (_shipDesign.ECM != null && _shipDesign.ECM.Technology == _availableECMTechs[_availableECMTechs.Count - 1])
			{
				//Already the best ECM
				return false;
			}
			int index = (_shipDesign.ECM == null ? -1 : _availableECMTechs.IndexOf(_shipDesign.ECM.Technology)) + 1; //Just one level higher will suffice
			return index < _availableECMTechs.Count && GetSpaceUsed(_availableECMTechs[index], false) <= remainingSpace;
		}
		private bool AtLeastOneBetterShield(float remainingSpace)
		{
			if (_shipDesign.Shield != null && _shipDesign.Shield.Technology == _availableShieldTechs[_availableShieldTechs.Count - 1])
			{
				//Already the best shield
				return false;
			}
			int index = (_shipDesign.Shield == null ? -1 : _availableShieldTechs.IndexOf(_shipDesign.Shield.Technology)) + 1; //Just one level higher will suffice
			return index < _availableShieldTechs.Count && GetSpaceUsed(_availableShieldTechs[index], false) <= remainingSpace;
		}
		private bool AtLeastOneBetterManeuver(float remainingSpace)
		{
			if (_shipDesign.ManeuverSpeed == _shipDesign.Engine.Key.Technology.ManeuverSpeed)
			{
				//Already at best maneuver speed
				return false;
			}
			int powerRequired = 2; //This is the small size's power requirement
			switch (_shipDesign.Size)
			{
				case Ship.MEDIUM:
					powerRequired = 15;
					break;
				case Ship.LARGE:
					powerRequired = 100;
					break;
				case Ship.HUGE:
					powerRequired = 700;
					break;
			}
			float spaceRequired = (_shipDesign.ManeuverSpeed + 1) * powerRequired * _spacePerPower;
			return spaceRequired <= remainingSpace;
		}
		private bool AtLeastOneBetterEngine(float remainingSpace)
		{
			if (_shipDesign.Engine.Key.Technology == _availableEngineTechs[_availableEngineTechs.Count - 1])
			{
				//Already the best engine
				return false;
			}
			int index = _availableEngineTechs.IndexOf(_shipDesign.Engine.Key.Technology) + 1; //Just one level higher will suffice
			Equipment engine = new Equipment(_availableEngineTechs[index], false);
			float totalSpace = (engine.GetSize(_techLevels, _shipDesign.Size) / (engine.Technology.Speed * 10)) * _shipDesign.PowerUsed;
			return totalSpace <= remainingSpace;
		}
		private bool AtLeastOneBetterArmor(float remainingSpace)
		{
			if (_shipDesign.Armor.Technology == _availableArmorTechs[_availableArmorTechs.Count - 1])
			{
				if (_shipDesign.Armor.UseSecondary)
				{
					//Already the best armor with double hull
					return false;
				}
				Equipment armor = new Equipment(_shipDesign.Armor.Technology, true);
				return armor.GetSize(_techLevels, _shipDesign.Size) <= remainingSpace;
			}
			int index = _availableArmorTechs.IndexOf(_shipDesign.Armor.Technology) + 1; //Just one level higher will suffice
			return GetSpaceUsed(_availableArmorTechs[index], false) <= remainingSpace;
		}
		private bool AtLeastOneBetterWeapon(KeyValuePair<Equipment, int> weapon, float remainingSpace)
		{
			float currentWeaponSpaceUsage = 0;
			int index = -1;
			if (weapon.Key != null)
			{
				currentWeaponSpaceUsage = weapon.Key.GetSize(_techLevels, _shipDesign.Size) * weapon.Value;
				index = _availableWeaponTechs.IndexOf(weapon.Key.Technology);
			}
			float totalAvailableSpace = remainingSpace + currentWeaponSpaceUsage;

			if (weapon.Key != null && !weapon.Key.UseSecondary && weapon.Key.Technology.MaximumSecondaryWeaponDamage > 0)
			{
				//There is a bigger version of the weapon, see if there's room for it
				Equipment bigWeapon = new Equipment(weapon.Key.Technology, true);
				if (bigWeapon.GetSize(_techLevels, _shipDesign.Size) <= totalAvailableSpace)
				{
					return true;
				}
			}
			for (int i = index + 1; i < _availableWeaponTechs.Count; i++)
			{
				if (GetSpaceUsed(_availableWeaponTechs[i], false) <= totalAvailableSpace)
				{
					return true;
				}
			}
			return false;
		}
		private bool AtLeastOneHigherLevelSpecial(Equipment special, float remainingSpace)
		{
			float currentSpecialSpaceUsage = 0;
			int index = -1;
			if (special != null)
			{
				currentSpecialSpaceUsage = special.GetSize(_techLevels, _shipDesign.Size);
				index = _availableSpecialTechs.IndexOf(special.Technology);
			}
			float totalAvailableSpace = currentSpecialSpaceUsage + remainingSpace;

			for (int i = index + 1; i < _availableSpecialTechs.Count; i++)
			{
				if (GetSpaceUsed(_availableSpecialTechs[i], false) <= totalAvailableSpace)
				{
					return true;
				}
			}
			return false;
		}

		private void LoadTechnologies()
		{
			var techManager = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager;

			_availableArmorTechs = new List<Technology>();
			_availableComputerTechs = new List<Technology>();
			_availableECMTechs = new List<Technology>();
			_availableEngineTechs = new List<Technology>();
			_availableShieldTechs = new List<Technology>();
			_availableSpecialTechs = new List<Technology>();
			_availableWeaponTechs = new List<Technology>();

			foreach (var construction in techManager.ResearchedConstructionTechs)
			{
				if (construction.Armor > 0)
				{
					_availableArmorTechs.Add(construction);
				}
				if (construction.Repair > 0)
				{
					_availableSpecialTechs.Add(construction);
				}
			}
			_availableArmorTechs.Sort((a, b) => a.Armor.CompareTo(b.Armor));

			foreach (var computer in techManager.ResearchedComputerTechs)
			{
				if (computer.BattleComputer > 0)
				{
					_availableComputerTechs.Add(computer);
				}
				if (computer.ECM > 0)
				{
					_availableECMTechs.Add(computer);
				}
				if (computer.BattleScanner || computer.OracleInterface || computer.TechnologyNullifier)
				{
					_availableSpecialTechs.Add(computer);
				}
			}
			_availableComputerTechs.Sort((a, b) => a.BattleComputer.CompareTo(b.BattleComputer));
			_availableECMTechs.Sort((a, b) => a.ECM.CompareTo(b.ECM));

			foreach (var propulsion in techManager.ResearchedPropulsionTechs)
			{
				if (propulsion.Speed > 0)
				{
					_availableEngineTechs.Add(propulsion);
				}
				if (propulsion.WarpDissipator || propulsion.InertialStabilizer || propulsion.InertialNullifier || propulsion.EnergyPulsar || propulsion.HighEnergyFocus || propulsion.SubspaceTeleporter || propulsion.IonicPulsar || propulsion.DisplacementDevice)
				{
					_availableSpecialTechs.Add(propulsion);
				}
			}
			_availableEngineTechs.Sort((a, b) => a.Speed.CompareTo(b.Speed));

			foreach (var forceField in techManager.ResearchedForceFieldTechs)
			{
				if (forceField.Shield > 0)
				{
					_availableShieldTechs.Add(forceField);
				}
				if (forceField.RepulsorBeam || forceField.CloakingDevice || forceField.MissileShield > 0 || forceField.StatisField || forceField.BlackHoleGenerator)
				{
					_availableSpecialTechs.Add(forceField);
				}
			}
			_availableShieldTechs.Sort((a, b) => a.Shield.CompareTo(b.Shield));

			//Weapons contains technologies from both Weapons and Planetology fields
			foreach (var weapon in techManager.ResearchedWeaponTechs)
			{
				if (weapon.MaximumWeaponDamage > 0) //Ensure that it's not a special device, MaximumWeaponDamage is used in ALL weapons, while MinimumWeaponDamage is not (i.e. missiles)
				{
					_availableWeaponTechs.Add(weapon);
				}
				if (weapon.AntiMissileRockets || weapon.NeutronStreamProjector)
				{
					_availableSpecialTechs.Add(weapon);
				}
			}
			foreach (var planetology in techManager.ResearchedPlanetologyTechs)
			{
				if (planetology.BioWeapon > 0)
				{
					_availableWeaponTechs.Add(planetology);
				}
				if (planetology.Colony > 0)
				{
					_availableSpecialTechs.Add(planetology);
				}
			}
			_availableWeaponTechs.Sort((a, b) => a.TechLevel.CompareTo(b.TechLevel));
			_availableSpecialTechs.Sort((a, b) => a.TechLevel.CompareTo(b.TechLevel));
		}

		private float GetSpaceUsed(Technology whichTech, bool useSecondary)
		{
			Equipment equipment = new Equipment(whichTech, useSecondary);
			return equipment.GetSize(_techLevels, _shipDesign.Size) + equipment.GetPower(_shipDesign.Size) * _spacePerPower;
		}

		private float GetCostUsed(Technology whichTech, bool useSecondary)
		{
			Equipment equipment = new Equipment(whichTech, useSecondary);
			return equipment.GetCost(_techLevels, _shipDesign.Size) + equipment.GetPower(_shipDesign.Size) * _costPerPower;
		}
	}
}