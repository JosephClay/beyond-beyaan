using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	public class PlanetsView : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchButton[] _columnHeaders;
		private BBStretchButton[][] _columnCells;
		private BBScrollBar _scrollBar;

		private BBStretchableImage _expensesBackground;
		private BBStretchableImage _incomeBackground;
		private BBStretchableImage _reserves;

		private BBLabel _expenseTitle;
		private BBLabel _incomeTitle;
		private BBLabel _reservesTitle;

		private BBStretchButton[] _expenses;
		private BBStretchButton[] _incomes;
		private BBLabel[] _expenseLabels;
		private BBLabel[] _incomeLabels;

		//private StarSystem _selectedSystem;
		//private Empire _currentEmpire;

		private int _maxVisible;

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 533;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 1066, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			x += 20;
			y += 20;

			_columnHeaders = new BBStretchButton[8];
			for (int i = 0; i < _columnHeaders.Length; i++)
			{
				_columnHeaders[i] = new BBStretchButton();
			}
			_columnCells = new BBStretchButton[8][];
			for (int i = 0; i < _columnCells.Length; i++)
			{
				_columnCells[i] = new BBStretchButton[13];
				for (int j = 0; j < _columnCells[i].Length; j++)
				{
					_columnCells[i][j] = new BBStretchButton();
				}
			}

			if (!_columnHeaders[0].Initialize("Planet", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 280, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[0][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 280, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 280;
			if (!_columnHeaders[1].Initialize("Population", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 90, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[1][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 90, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 90;
			if (!_columnHeaders[2].Initialize("Buildings", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 90, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[2][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 90, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 90;
			if (!_columnHeaders[3].Initialize("Bases", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[3][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[4].Initialize("Waste", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[4][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[5].Initialize("Industry", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[5][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[6].Initialize("Constructing", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 250, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[6][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 250, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 250;
			if (!_columnHeaders[7].Initialize("Notes", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 60, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[7][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 60, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}
			x += 60;
			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(x, y + 30, 390, 13, 13, false, false, _gameMain.Random, out reason))
			{
				return false;
			}

			_expensesBackground = new BBStretchableImage();
			_incomeBackground = new BBStretchableImage();
			_reserves = new BBStretchableImage();

			_expenseTitle = new BBLabel();
			_incomeTitle = new BBLabel();
			_reservesTitle = new BBLabel();

			_expenses = new BBStretchButton[4];
			_expenseLabels = new BBLabel[4];
			_incomes = new BBStretchButton[2];
			_incomeLabels = new BBLabel[2];
			for (int i = 0; i < 4; i++)
			{
				_expenses[i] = new BBStretchButton();
				_expenseLabels[i] = new BBLabel();
			}
			for (int i = 0; i < 2; i++)
			{
				_incomes[i] = new BBStretchButton();
				_incomeLabels[i] = new BBLabel();
			}

			x = (gameMain.ScreenWidth / 2) - 513;
			y = (gameMain.ScreenHeight / 2) + 143;

			if (!_expensesBackground.Initialize(x, y, 476, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_expenseTitle.Initialize(0, 0, "Expenses", Color.Gold, "LargeComputerFont", out reason))
			{
				return false;
			}
			_expenseTitle.MoveTo((int)(x + 238 - _expenseTitle.GetWidth() / 2), y + 5);
			if (!_expenses[0].Initialize("Ships", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 50,  228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[0].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[0].Initialize(x + 228, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[0].SetAlignment(true);
			if (!_expenses[1].Initialize("Bases", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 90, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[1].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[1].Initialize(x + 228, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[1].SetAlignment(true);
			if (!_expenses[2].Initialize("Spying", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 238, y + 50, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[2].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[2].Initialize(x + 456, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[2].SetAlignment(true);
			if (!_expenses[3].Initialize("Security", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 238, y + 90, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[3].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[3].Initialize(x + 456, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[3].SetAlignment(true);
			x += 476;
			if (!_incomeBackground.Initialize(x, y, 250, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_incomeTitle.Initialize(0, 0, "Incomes", Color.Gold, "LargeComputerFont", out reason))
			{
				return false;
			}
			if (!_incomes[0].Initialize("Planets", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 50, 230, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_incomes[0].SetTextColor(Color.Orange, Color.Empty);
			if (!_incomeLabels[0].Initialize(x + 230, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_incomeLabels[0].SetAlignment(true);
			if (!_incomes[1].Initialize("Trade", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 90, 230, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_incomes[1].SetTextColor(Color.Orange, Color.Empty);
			if (!_incomeLabels[0].Initialize(x + 230, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_incomeLabels[0].SetAlignment(true);
			_incomeTitle.MoveTo((int)(x + 125 - _incomeTitle.GetWidth() / 2), y + 5);
			x += 250;
			if (!_reserves.Initialize(x, y, 300, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}

			return true;
		}
		#endregion

		public void Load()
		{
			var planets = _gameMain.EmpireManager.CurrentEmpire.PlanetManager.Planets;
			if (planets.Count > 13)
			{
				_maxVisible = 13;
				_scrollBar.SetEnabledState(true);
				_scrollBar.SetAmountOfItems(planets.Count);
			}
			else
			{
				_maxVisible = planets.Count;
				_scrollBar.SetEnabledState(false);
				_scrollBar.SetAmountOfItems(13);
			}
			LoadPlanets(planets);
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			_expenseLabels[0].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.ShipMaintenancePercentage * 100, currentEmpire.ShipMaintenance));
			_expenseLabels[1].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.BaseMaintenancePercentage * 100, currentEmpire.BaseMaintenance));
			_expenseLabels[2].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.EspionageExpensePercentage * 100, currentEmpire.EspionageExpense));
			_expenseLabels[3].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.SecurityExpensePercentage * 100, currentEmpire.SecurityExpense));

			_incomeLabels[0].SetText(string.Format("{0:0.0} BC", currentEmpire.PlanetIncome));
			_incomeLabels[1].SetText(string.Format("{0:0.0} BC", currentEmpire.TradeIncome));
		}

		public override void Draw()
		{
			base.Draw();

			for (int i = 0; i < _columnHeaders.Length; i++)
			{
				_columnHeaders[i].Draw();
			}
			for (int i = 0; i < _columnCells.Length; i++)
			{
				for (int j = 0; j < _columnCells[i].Length; j++)
				{
					_columnCells[i][j].Draw();
				}
			}
			_expensesBackground.Draw();
			_incomeBackground.Draw();
			_reserves.Draw();
			_expenseTitle.Draw();
			_incomeTitle.Draw();
			_scrollBar.Draw();
			for (int i = 0; i < 4; i++)
			{
				_expenses[i].Draw();
				_expenseLabels[i].Draw();
			}
			for (int i = 0; i < 2; i++)
			{
				_incomes[i].Draw();
				_incomeLabels[i].Draw();
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool MouseDown(int x, int y)
		{
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
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

		public void LoadPlanets(List<Planet> planets)
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				_columnCells[0][i].SetText(planets[i].Name);
				_columnCells[1][i].SetText(planets[i].TotalPopulation.ToString());
				_columnCells[2][i].SetText(planets[i].InfrastructureTotal.ToString());
				_columnCells[3][i].SetText(string.Empty); //TODO: Add missile bases
				_columnCells[4][i].SetText(string.Empty); //TODO: Add waste
				_columnCells[5][i].SetText(((int)planets[i].ActualProduction).ToString());
				_columnCells[6][i].SetText(planets[i].ConstructionAmount > 0 ? planets[i].ShipBeingBuilt.Name : string.Empty);
				for (int j = 0; j < 8; j++)
				{
					_columnCells[j][i].Enabled = true;
				}
			}
			for (int i = _maxVisible; i < 13; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					_columnCells[j][i].Enabled = false;
				}
			}
		}
	}
}
