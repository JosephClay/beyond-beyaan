using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	public class ColonizeScreen : WindowInterface
	{
		private Fleet _colonizingFleet;
		private List<Ship> _colonyShips; 
		private StarSystem _starSystem;

		//For now, only 1 planet per system
		private BBStretchButton[] _shipButtons;
		private BBLabel _systemNameLabel;
		private BBLabel _instructionLabel;

		private BBButton _colonizeButton;
		private BBButton _cancelButton;
		public Action Completed;

		private int _maxShips;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!this.Initialize(gameMain.ScreenWidth / 2 - 200, gameMain.ScreenHeight / 2 - 300, 400, 250, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_shipButtons = new BBStretchButton[4];
			for (int i = 0; i < _shipButtons.Length; i++)
			{
				_shipButtons[i] = new BBStretchButton();
				if (!_shipButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, xPos + 20, yPos + 60, 200, 40, gameMain.Random, out reason))
				{
					return false;
				}
			}
			_instructionLabel = new BBLabel();
			_systemNameLabel = new BBLabel();
			_cancelButton = new BBButton();
			_colonizeButton = new BBButton();
			if (!_instructionLabel.Initialize(xPos + 20, yPos + 25, "Select a ship to colonize this planet", Color.White, out reason))
			{
				return false;
			}
			if (!_systemNameLabel.Initialize(xPos + 300, yPos + 130, string.Empty, Color.White, out reason))
			{
				return false;
			}
			if (!_cancelButton.Initialize("CancelColonizeBG", "CancelColonizeFG", string.Empty, ButtonTextAlignment.LEFT, xPos + 230, yPos + 195, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_colonizeButton.Initialize("TransferToBG", "TransferToFG", string.Empty, ButtonTextAlignment.LEFT, xPos + 310, yPos + 195, 75, 35, gameMain.Random, out reason))
			{
				return false;
			}

			return true;
		}

		public void LoadFleetAndSystem(Fleet fleet)
		{
			_colonizingFleet = fleet;
			_starSystem = fleet.AdjacentSystem;
			_colonyShips = new List<Ship>();
			foreach (var ship in _colonizingFleet.OrderedShips)
			{
				foreach (var special in ship.Specials)
				{
					if (special.Colony >= _starSystem.Planets[0].ColonyRequirement)
					{
						_colonyShips.Add(ship);
						break;
					}
				}
			}
			//TODO: Add scrollbar to support more than 4 different colony ship designs
			_maxShips = _colonyShips.Count > 4 ? 4 : _colonyShips.Count;
			for (int i = 0; i < _maxShips; i++)
			{
				_shipButtons[i].SetText(_colonyShips[i].Name + (_colonizingFleet.Ships[_colonyShips[i]] > 1 ? " (" + _colonizingFleet.Ships[_colonyShips[i]] + ")" : string.Empty));
			}
			_shipButtons[0].Selected = true;
			_systemNameLabel.SetText(_starSystem.Name);
			_systemNameLabel.MoveTo(xPos + 300 - (int)(_systemNameLabel.GetWidth() / 2), yPos + 130 - (int)(_systemNameLabel.GetHeight() / 2));
		}

		public override void Draw()
		{
			base.Draw();
			_instructionLabel.Draw();
			for (int i = 0; i < _maxShips; i++)
			{
				_shipButtons[i].Draw();
			}
			_systemNameLabel.Draw();
			_starSystem.Planets[0].SmallSprite.Draw(xPos + 285, yPos + 80);
			_cancelButton.Draw();
			_colonizeButton.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			for (int i = 0; i < _maxShips; i++)
			{
				_shipButtons[i].MouseHover(x, y, frameDeltaTime);
			}
			_cancelButton.MouseHover(x, y, frameDeltaTime);
			_colonizeButton.MouseHover(x, y, frameDeltaTime);
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			for (int i = 0; i < _maxShips; i++)
			{
				_shipButtons[i].MouseDown(x, y);
			}
			_cancelButton.MouseDown(x, y);
			_colonizeButton.MouseDown(x, y);
			return false;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < _maxShips; i++)
			{
				if (_shipButtons[i].MouseUp(x, y))
				{
					foreach (var button in _shipButtons)
					{
						button.Selected = false;
					}
					_shipButtons[i].Selected = true;
				}
			}
			if (_cancelButton.MouseUp(x, y))
			{
				if (Completed != null)
				{
					Completed();
				}
			}
			if (_colonizeButton.MouseDown(x, y))
			{
				int whichShip = 0;
				for (int i = 0; i < _maxShips; i++)
				{
					if (_shipButtons[i].Selected)
					{
						whichShip = i;
						break;
					}
				}
				var ship = _colonyShips[whichShip];
				_colonizingFleet.ColonizePlanet(ship);
				if (Completed != null)
				{
					Completed();
				}
			}
			return false;
		}
	}
}
