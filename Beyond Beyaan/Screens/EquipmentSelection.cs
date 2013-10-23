using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Screens
{
	public enum EquipmentType { COMPUTER, SHIELD, ARMOR, ECM, ENGINE, MANEUVER, WEAPON, SPECIAL }

	public class EquipmentSelection : WindowInterface
	{
		private BBStretchButton[] _buttons;
		private BBLabel[] _columnNames;
		private BBLabel[] _column1Values;
		private BBLabel[] _column2Values;
		private BBLabel[] _column3Values;
		private BBLabel[] _column4Values;
		private BBLabel[] _column5Values;

		private List<Equipment> _availableEquipments;
		private BBScrollBar _scrollBar;
		private int _maxVisible;
		private bool _scrollBarVisible;
		private int _numOfColumnsVisible;

		private Ship _shipDesign;
		private Dictionary<TechField, int> _techLevels; 

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 210, (gameMain.ScreenHeight / 2) - 230, 420, 460, StretchableImageType.ThinBorderBG, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_buttons = new BBStretchButton[10];
			_columnNames = new BBLabel[6];
			_column1Values = new BBLabel[10];
			_column2Values = new BBLabel[10];
			_column3Values = new BBLabel[10];
			_column4Values = new BBLabel[10];
			_column5Values = new BBLabel[10];
			_maxVisible = 0;

			for (int i = 0; i < 10; i++)
			{
				_buttons[i] = new BBStretchButton();
				_column1Values[i] = new BBLabel();
				_column2Values[i] = new BBLabel();
				_column3Values[i] = new BBLabel();
				_column4Values[i] = new BBLabel();
				_column5Values[i] = new BBLabel();
				if (!_buttons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _xPos + 10, _yPos + 50 + i * 40, 380, 40, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_column1Values[i].Initialize(_xPos + 150, _yPos + 60 + i * 40, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_column2Values[i].Initialize(_xPos + 200, _yPos + 60 + i * 40, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_column3Values[i].Initialize(_xPos + 250, _yPos + 60 + i * 40, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_column4Values[i].Initialize(_xPos + 300, _yPos + 60 + i * 40, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_column5Values[i].Initialize(_xPos + 350, _yPos + 60 + i * 40, string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
			}
			for (int i = 0; i < 6; i++)
			{
				_columnNames[6] = new BBLabel();
			}
			if (!_columnNames[0].Initialize(_xPos + 15, _yPos + 10, "Name", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_columnNames[1].Initialize(_xPos + 150, _yPos + 10, "Name1", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_columnNames[2].Initialize(_xPos + 200, _yPos + 10, "Name2", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_columnNames[3].Initialize(_xPos + 250, _yPos + 10, "Name3", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_columnNames[4].Initialize(_xPos + 300, _yPos + 10, "Name4", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_columnNames[5].Initialize(_xPos + 350, _yPos + 10, "Name5", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(_xPos + 390, _yPos + 50, 400, 10, 10, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			_maxVisible = 0;
			_numOfColumnsVisible = 0;
			_scrollBarVisible = false;
			return true;
		}

		public void LoadEquipments(Ship shipDesign, EquipmentType equipmentType, List<Technology> _technologies, Dictionary<TechField, int> techLevels)
		{
			_shipDesign = shipDesign;
			_techLevels = techLevels;
			_availableEquipments = new List<Equipment>();
			switch (equipmentType)
			{
				case EquipmentType.ARMOR:
					LoadArmor(_technologies);
					break;
				case EquipmentType.COMPUTER:
					break;
				case EquipmentType.ECM:
					break;
				case EquipmentType.ENGINE:
					break;
				case EquipmentType.MANEUVER:
					break;
				case EquipmentType.SHIELD:
					break;
				case EquipmentType.SPECIAL:
					break;
				case EquipmentType.WEAPON:
					break;
			}
		}

		private void LoadArmor(List<Technology> _technologies)
		{
			foreach (var tech in _technologies)
			{
				Equipment armorI = new Equipment(tech, false);
				Equipment armorII = new Equipment(tech, true);

				_availableEquipments.Add(armorI);
				_availableEquipments.Add(armorII);
			}
			if (_availableEquipments.Count > 10)
			{
				_maxVisible = 10;
				_scrollBarVisible = true;
				_scrollBar.SetAmountOfItems(_availableEquipments.Count);
			}
			else
			{
				_maxVisible = _availableEquipments.Count;
				_scrollBarVisible = false;
				_scrollBar.TopIndex = 0;
			}
			_numOfColumnsVisible = 3;
			ResizeWindow();
			RefreshLabels();
		}

		private void ResizeWindow()
		{
			int height = 60 + _maxVisible * 40;
			int buttonWidth = 150 + _numOfColumnsVisible * 50;
			int width = 20 + buttonWidth + (_scrollBarVisible ? 20 : 0);

			_xPos = (_gameMain.ScreenWidth / 2) - (width / 2);
			_yPos = (_gameMain.ScreenHeight / 2) - (height / 2);
			_backGroundImage.Resize(width, height);
			_backGroundImage.MoveTo(_xPos, _yPos);

			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].ResizeButton(buttonWidth, 40);
				_buttons[i].MoveTo(_xPos + 10, _yPos + 50 + i * 40);
				_column1Values[i].MoveTo(_xPos + 150, _yPos + 60 + i * 40);
				_column2Values[i].MoveTo(_xPos + 200, _yPos + 60 + i * 40);
				_column3Values[i].MoveTo(_xPos + 250, _yPos + 60 + i * 40);
				_column4Values[i].MoveTo(_xPos + 300, _yPos + 60 + i * 40);
				_column5Values[i].MoveTo(_xPos + 350, _yPos + 60 + i * 40);
			}
			for (int i = 0; i < _numOfColumnsVisible; i++)
			{
				_columnNames[i].MoveTo(_xPos + 150 + i * 50, _yPos + 10);
			}
			if (_scrollBarVisible)
			{
				_scrollBar.MoveTo(_xPos + width - 30, _yPos + 50);
			}
		}

		private void RefreshLabels()
		{
			float availableSpace = _shipDesign.TotalSpace - _shipDesign.SpaceUsed;
			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].SetText(_availableEquipments[i + _scrollBar.TopIndex].DisplayName);
				if (_availableEquipments[i + _scrollBar.TopIndex].GetSize(_techLevels, _shipDesign.Size) > availableSpace)
				{
					_buttons[i].Enabled = false;
					_column1Values[i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_column2Values[i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_column3Values[i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_column4Values[i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					_column5Values[i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
				}
				else
				{
					_buttons[i].Enabled = true;
					_column1Values[i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					_column2Values[i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					_column3Values[i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					_column4Values[i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					_column5Values[i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
				}
			}
		}
	}
}
