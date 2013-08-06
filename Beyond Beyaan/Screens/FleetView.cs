using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class FleetView : WindowInterface
	{
		#region Member Variables
		private FleetGroup _selectedFleetGroup;
		private Fleet _selectedFleet;

		private BBStretchableImage _fleetBackground;
		private BBStretchableImage[] _shipBackground;
		private BBStretchableImage _shipPreview;

		private BBScrollBar _fleetScrollBar;
		private BBScrollBar _shipScrollBar;

		private BBInvisibleStretchButton[] _fleetButtons;
		private BBScrollBar[] _shipSliders;
		private BBLabel[] _shipLabels;

		private int _maxFleetVisible;
		private int _maxShipVisible;

		private bool _useFleetScrollBar;
		private bool _useShipScrollBar;

		private bool _showingPreview;
		private BBSprite _shipSprite;
		private Point _shipPoint;
		private float[] _empireColor;
		#endregion

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 240, 300, 480, StretchableImageType.ThinBorderBG, gameMain, true, gameMain.Random, out reason))
			{
				return false;
			}

			_fleetBackground = new BBStretchableImage();
			_shipBackground = new BBStretchableImage[6];
			_shipPreview = new BBStretchableImage();

			_fleetScrollBar = new BBScrollBar();
			_shipScrollBar = new BBScrollBar();

			_fleetButtons = new BBInvisibleStretchButton[3];
			_shipSliders = new BBScrollBar[6];
			_shipLabels = new BBLabel[6];

			if (!_shipPreview.Initialize(0, 0, 170, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_fleetBackground.Initialize(xPos + 10, yPos + 10, 280, 120, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _fleetButtons.Length; i++)
			{
				_fleetButtons[i] = new BBInvisibleStretchButton();
				if (!_fleetButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 15, yPos + 15 + (i * 35), 250, 35, gameMain.Random, out reason))
				{
					return false;
				}
			}
			for (int i = 0; i < _shipBackground.Length; i++)
			{
				_shipBackground[i] = new BBStretchableImage();
				_shipLabels[i] = new BBLabel();
				_shipSliders[i] = new BBScrollBar();

				if (!_shipBackground[i].Initialize(xPos + 10, yPos + 135 + (i * 55), 260, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_shipLabels[i].Initialize(xPos + 15, yPos + 145 + (i * 55), "Test", Color.White, out reason))
				{
					return false;
				}
				if (!_shipSliders[i].Initialize(xPos + 15, yPos + 165 + (i * 55), 250, 1, 1, true, true, gameMain.Random, out reason))
				{
					return false;
				}
			}
			if (!_fleetScrollBar.Initialize(xPos + 267, yPos + 20, 100, _fleetButtons.Length, _fleetButtons.Length, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_shipScrollBar.Initialize(xPos + 275, yPos + 135, 330, _shipBackground.Length, _shipBackground.Length, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			return true;
		}
		#endregion

		public override void Draw()
		{
			base.Draw();
			_fleetBackground.Draw();
			for (int i = 0; i < _maxFleetVisible; i++)
			{
				_fleetButtons[i].Draw();
			}
			for (int i = 0; i < _maxShipVisible; i++)
			{
				_shipBackground[i].Draw();
				_shipLabels[i].Draw();
				_shipSliders[i].Draw();
			}
			_fleetScrollBar.Draw();
			_shipScrollBar.Draw();
			if (_showingPreview)
			{
				_shipPreview.Draw();
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.ShipShader;
				_gameMain.ShipShader.Parameters["EmpireColor"].SetValue(_empireColor);
				_shipSprite.Draw(_shipPoint.X, _shipPoint.Y);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
		}

		public override void MoveWindow()
		{
			base.MoveWindow();

			_fleetBackground.MoveTo(xPos + 10, yPos + 10);
			
			for (int i = 0; i < _fleetButtons.Length; i++)
			{
				_fleetButtons[i].MoveTo(xPos + 15, yPos + 15 + (i * 35));
			}
			for (int i = 0; i < _shipBackground.Length; i++)
			{
				_shipBackground[i].MoveTo(xPos + 10, yPos + 135 + (i * 55));
				_shipLabels[i].MoveTo(xPos + 15, yPos + 145 + (i * 55));
				_shipSliders[i].MoveTo(xPos + 15, yPos + 165 + (i * 55));
			}
			_fleetScrollBar.MoveTo(xPos + 267, yPos + 20);
			_shipScrollBar.MoveTo(xPos + 275, yPos + 135);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			for (int i = 0; i < _maxFleetVisible; i++)
			{
				result = _fleetButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			bool withinX = (x >= xPos + 10 && x < xPos + 270);
			bool showingPreview = false;
			for (int i = 0; i < _maxShipVisible; i++)
			{
				if (_shipSliders[i].MouseHover(x, y, frameDeltaTime))
				{
					result = true;
					if (_selectedFleet.Ships.Count > 0)
					{
						Ship ship = _selectedFleet.OrderedShips[i + _shipScrollBar.TopIndex];
						_selectedFleetGroup.FleetToSplit.Ships[ship] = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_shipSliders[i].TopIndex + " x " + ship.Name);
					}
					else //Transports
					{
						_selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].amount = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].amount + " x " + _selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].raceOnShip.RaceName);
					}
				}
				int tempY = yPos + 135 + (i * 55);
				if (_selectedFleet.Ships.Count > 0 && withinX && y >= tempY && y < tempY + 55)
				{
					var ship = _selectedFleet.OrderedShips[i + _shipScrollBar.TopIndex];
					_shipSprite = ship.Owner.EmpireRace.GetShip(ship.Size, ship.WhichStyle);
					_empireColor = ship.Owner.ConvertedColor;
					//Show ship preview for this ship
					if (xPos > 170)
					{
						_shipPreview.MoveTo(xPos - 170, tempY - 62);
						_shipPoint.X = xPos - 85;
						_shipPoint.Y = tempY + 23;
						showingPreview = true;
					}
					else
					{
						_shipPreview.MoveTo(xPos + 300, tempY - 62);
						_shipPoint.X = xPos + 385;
						_shipPoint.Y = tempY + 23;
						showingPreview = true;
					}
				}
			}
			_showingPreview = showingPreview;
			if (_fleetScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshFleets();
				result = true;
			}
			if (_shipScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshShips();
				result = true;
			}
			return base.MouseHover(x, y, frameDeltaTime) || result;
		}

		public override bool MouseDown(int x, int y)
		{
			for (int i = 0; i < _maxFleetVisible; i++)
			{
				if (_fleetButtons[i].MouseDown(x, y))
				{
					return true;
				}
			}
			if (_fleetScrollBar.MouseDown(x, y))
			{
				return true;
			}
			if (_shipScrollBar.MouseDown(x, y))
			{
				return true;
			}
			for (int i = 0; i < _maxShipVisible; i++)
			{
				if (_shipSliders[i].MouseDown(x, y))
				{
					return true;
				}
			}
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < _maxFleetVisible; i++)
			{
				if (_fleetButtons[i].MouseUp(x, y))
				{
					_selectedFleetGroup.SelectFleet(i + _fleetScrollBar.TopIndex);
					for (int j = 0; j < _maxFleetVisible; j++)
					{
						_fleetButtons[j].Selected = false;
					}
					_fleetButtons[i].Selected = true;
					_selectedFleet = _selectedFleetGroup.SelectedFleet;
					LoadShips();
					result = true;
				}
			}
			if (_fleetScrollBar.MouseUp(x, y))
			{
				RefreshFleets();
				result = true;
			}
			if (_shipScrollBar.MouseUp(x, y))
			{
				RefreshShips();
				result = true;
			}
			for (int i = 0; i < _maxShipVisible; i++)
			{
				if (_shipSliders[i].MouseUp(x, y))
				{
					if (_selectedFleet.OrderedShips.Count > 0)
					{
						Ship ship = _selectedFleet.OrderedShips[i + _shipScrollBar.TopIndex];
						_selectedFleetGroup.FleetToSplit.Ships[ship] = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_shipSliders[i].TopIndex + " x " + ship.Name);
					}
					else //Transports
					{
						_selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].amount = _shipSliders[i].TopIndex;
						_shipLabels[i].SetText(_selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].amount + " x " + _selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].raceOnShip.RaceName);
					}
					result = true;
				}
			}
			return base.MouseUp(x, y) || result;
		}

		public void LoadFleetGroup(FleetGroup selectedFleetGroup)
		{
			_selectedFleetGroup = selectedFleetGroup;
			_selectedFleet = selectedFleetGroup.SelectedFleet;
			_maxFleetVisible = Math.Min(selectedFleetGroup.Fleets.Count, _fleetButtons.Length);
			_useFleetScrollBar = selectedFleetGroup.Fleets.Count > _fleetButtons.Length;

			_fleetScrollBar.TopIndex = 0;
			if (_useFleetScrollBar)
			{
				_fleetScrollBar.SetEnabledState(true);
				_fleetScrollBar.SetAmountOfItems(selectedFleetGroup.Fleets.Count);
			}
			else
			{
				_fleetScrollBar.SetEnabledState(false);
				_fleetScrollBar.SetAmountOfItems(_fleetButtons.Length);
			}
			if (selectedFleetGroup.FleetIndex < _fleetButtons.Length)
			{
				_fleetButtons[selectedFleetGroup.FleetIndex].Selected = true;
			}
			
			RefreshFleets();
			LoadShips();
		}

		private void LoadShips()
		{
			if (_selectedFleet.Ships.Count > 0)
			{
				_maxShipVisible = Math.Min(_selectedFleet.Ships.Count, _shipBackground.Length);
				_useShipScrollBar = _selectedFleet.Ships.Count > _shipBackground.Length;
			}
			else //Transports
			{
				_maxShipVisible = Math.Min(_selectedFleet.TransportShips.Count, _shipBackground.Length);
				_useShipScrollBar = _selectedFleet.TransportShips.Count > _shipBackground.Length;
			}
			
			_shipScrollBar.TopIndex = 0;
			if (_useShipScrollBar)
			{
				_shipScrollBar.SetEnabledState(true);
				_shipScrollBar.SetAmountOfItems(_selectedFleet.Ships.Count);
			}
			else
			{
				_shipScrollBar.SetEnabledState(false);
				_shipScrollBar.SetAmountOfItems(_shipBackground.Length);
			}
			bool isOwned = _selectedFleet.Empire == _gameMain.EmpireManager.CurrentEmpire;
			foreach (var slider in _shipSliders)
			{
				slider.SetEnabledState(isOwned);
			}
			RefreshShips();
		}

		private void RefreshShips()
		{
			for (int i = 0; i < _maxShipVisible; i++)
			{
				if (_selectedFleet.OrderedShips.Count > 0)
				{
					Ship ship = _selectedFleet.OrderedShips[i + _shipScrollBar.TopIndex];
					_shipSliders[i].SetAmountOfItems(_selectedFleet.Ships[ship] + 1);
					int amount = _selectedFleetGroup.FleetToSplit.Ships[ship];
					_shipLabels[i].SetText(amount + " x " + ship.Name);
					_shipSliders[i].TopIndex = amount;
				}
				else
				{
					_shipSliders[i].SetAmountOfItems(_selectedFleet.TransportShips[i + _shipScrollBar.TopIndex].amount);
					int amount = _selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].amount;
					_shipLabels[i].SetText(amount + " x " + _selectedFleetGroup.FleetToSplit.TransportShips[i + _shipScrollBar.TopIndex].raceOnShip.RaceName);
					_shipSliders[i].TopIndex = amount;
				}
			}
		}

		private void RefreshFleets()
		{
			for (int i = 0; i < _maxFleetVisible; i++)
			{
				if (_selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].ShipCount > 0)
				{
					_fleetButtons[i].SetText(_selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].ShipCount + " " + _selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].Empire.EmpireRace.SingularRaceName + " Ships");
				}
				else
				{
					_fleetButtons[i].SetText(_selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].TransportShips[0].amount + " " + _selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].TransportShips[0].raceOnShip.SingularRaceName + " Transports");
				}
				_fleetButtons[i].SetTextColor(_selectedFleetGroup.Fleets[i + _fleetScrollBar.TopIndex].Empire.EmpireColor);
				_fleetButtons[i].Selected = false;
			}
			_fleetButtons[_selectedFleetGroup.FleetIndex - _fleetScrollBar.TopIndex].Selected = true;
		}
	}
}
