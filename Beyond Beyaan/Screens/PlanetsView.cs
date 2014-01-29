using System;

namespace Beyond_Beyaan.Screens
{
	public class PlanetsView : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchButton[] _columnHeaders;
		private BBStretchButton[][] _columnCells;
		private BBScrollBar _scrollBar;

		//private StarSystem _selectedSystem;
		//private Empire _currentEmpire;

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
			if (!_columnHeaders[5].Initialize("Prod", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
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

			return true;
		}
		#endregion

		public void Load()
		{
			
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
			_scrollBar.Draw();
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

		public void LoadPlanets()
		{
			
		}
	}
}
