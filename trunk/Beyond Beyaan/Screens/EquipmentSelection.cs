using System.Collections.Generic;

namespace Beyond_Beyaan.Screens
{
	public enum EquipmentType { COMPUTER, SHIELD, ARMOR, ECM, ENGINE, MANEUVER, WEAPON, SPECIAL }

	public class EquipmentSelection : WindowInterface
	{
		private BBStretchButton[] _buttons;
		private List<BBLabel[]> _columnValues;
		private List<BBLabel> _columnNames;

		private List<Equipment> _availableEquipments;
		private BBScrollBar _scrollBar;
		private int _maxVisible;
		private bool _scrollBarVisible;
		private int _numOfColumnsVisible;
		private int _middleX;
		private int _middleY;

		private Ship _shipDesign;
		private Dictionary<TechField, int> _techLevels; 

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_middleX = gameMain.ScreenWidth / 2;
			_middleY = gameMain.ScreenHeight / 2;
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 210, (gameMain.ScreenHeight / 2) - 230, 420, 460, StretchableImageType.ThinBorderBG, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_buttons = new BBStretchButton[10];
			_columnValues = new List<BBLabel[]>();
			_columnNames = new List<BBLabel>();
			_maxVisible = 0;

			for (int i = 0; i < 10; i++)
			{
				_buttons[i] = new BBStretchButton();
				if (!_buttons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _xPos + 10, _yPos + 50 + i * 40, 380, 40, gameMain.Random, out reason))
				{
					return false;
				}
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
			SetControlsAndWindow();
			RefreshLabels();
		}

		private void SetControlsAndWindow()
		{
			_columnValues.Clear();
			int width = 150 + _numOfColumnsVisible * 50 + (_scrollBarVisible ? 16 : 0);
			int height = 40 + _maxVisible * 40;
			int left = (_middleX - (width / 2));
			int top = (_middleY - (height / 2));
			string reason; //Unused, since at this point we've already initialized at least once, meaning everything is set up correctly
			_columnNames.Clear();
			_columnNames.Add(new BBLabel());
			_columnNames[0].Initialize(left, top, "Name", System.Drawing.Color.White, out reason);
			top += 40;
			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].MoveTo(left, top + 40 + (i * 40));
				_buttons[i].ResizeButton(150 + _numOfColumnsVisible * 50, 40);
			}
			left += 150;
			for (int i = 0; i < _numOfColumnsVisible; i++)
			{
				_columnNames.Add(new BBLabel());
				_columnNames[i + 1].Initialize(left + (i * 50), top, string.Empty, System.Drawing.Color.White, out reason);
				_columnValues.Add(new BBLabel[_maxVisible]);
				for (int j = 0; j < _maxVisible; j++)
				{
					_columnValues[i][j] = new BBLabel();
					_columnValues[i][j].Initialize(left + (i * 50), top + (j * 40), string.Empty, System.Drawing.Color.White, out reason);
				}
			}
			//Move and resize the window to fit
			_xPos = (_gameMain.ScreenWidth / 2) - (width / 2) - 10;
			_yPos = (_gameMain.ScreenHeight / 2) - (height / 2) - 10;
			_backGroundImage.Resize(width + 20, height + 20);
			_backGroundImage.MoveTo(_xPos, _yPos);
			_scrollBar.MoveTo(left + _numOfColumnsVisible * 50, top);
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
					for (int j = 0; j < _numOfColumnsVisible; j++)
					{
						_columnValues[i][j].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					}
				}
				else
				{
					_buttons[i].Enabled = true;
					for (int j = 0; j < _numOfColumnsVisible; j++)
					{
						_columnValues[i][j].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					}
				}
			}
		}
	}
}
