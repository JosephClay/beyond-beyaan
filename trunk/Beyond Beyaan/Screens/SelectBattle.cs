using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Beyond_Beyaan.Screens
{
	class SelectBattle : WindowInterface
	{
		public delegate void CancelFunction();
		public delegate void CommenceBattle(string fileName);

		private CancelFunction cancelFunction;
		private CommenceBattle commenceFunction;

		private StretchButton commenceButton;
		private StretchButton cancelButton;
		private InvisibleStretchButton[] battleButtons;
		private ScrollBar scrollBar;

		private string path;
		private List<string> battles;
		private int maxBattleVisible;
		private int battleSelected;

		public SelectBattle(int centerX, int centerY, GameMain gameMain, CancelFunction cancelFunction, CommenceBattle commenceFunction)
			: base(centerX, centerY, 400, 400, string.Empty, gameMain, false)
		{
			backGroundImage = new StretchableImage(0, 0, 400, 400, 60, 60, DrawingManagement.BorderBorder);
			backGroundImage.MoveTo(centerX - 200, centerY - 200);
			this.cancelFunction = cancelFunction;
			this.commenceFunction = commenceFunction;

			commenceButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Commence Battle", centerX + 25, centerY + 150, 150, 35);
			cancelButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Cancel", centerX - 175, centerY + 150, 150, 35);

			battleButtons = new InvisibleStretchButton[8];
			for (int i = 0; i < battleButtons.Length; i++)
			{
				battleButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, centerX - 180, centerY - 190 + (i * 40), 350, 40, 30, 13, System.Drawing.Color.White);
			}

			scrollBar = new ScrollBar(centerX + 170, centerY - 190, 16, 288, 8, 8, false, false, DrawingManagement.VerticalScrollBar);
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);

			cancelButton.Draw(drawingManagement);
			commenceButton.Draw(drawingManagement);

			for (int i = 0; i < maxBattleVisible; i++)
			{
				battleButtons[i].Draw(drawingManagement);
			}
			scrollBar.Draw(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			commenceButton.MouseHover(x, y, frameDeltaTime);
			cancelButton.MouseHover(x, y, frameDeltaTime);

			for (int i = 0; i < maxBattleVisible; i++)
			{
				battleButtons[i].MouseHover(x, y, frameDeltaTime);
			}
			if (scrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshLabels();
			}
			return true;
		}

		public override bool MouseDown(int x, int y)
		{
			commenceButton.MouseDown(x, y);
			cancelButton.MouseDown(x, y);

			for (int i = 0; i < maxBattleVisible; i++)
			{
				battleButtons[i].MouseDown(x, y);
			}
			scrollBar.MouseDown(x, y);
			return true;
		}

		public override bool MouseUp(int x, int y)
		{
			if (cancelButton.MouseUp(x, y))
			{
				if (cancelFunction != null)
				{
					cancelFunction();
				}
			}
			if (commenceButton.MouseUp(x, y))
			{
				gameMain.LoadBattle(Path.Combine(path, battles[battleSelected]));
				gameMain.ChangeToScreen(Screen.Battle);
			}
			for (int i = 0; i < maxBattleVisible; i++)
			{
				if (battleButtons[i].MouseUp(x, y))
				{
					battleSelected = i + scrollBar.TopIndex;
					commenceButton.Active = true;
					RefreshLabels();
				}
			}
			if (scrollBar.MouseUp(x, y))
			{
				RefreshLabels();
			}
			return true;
		}

		public void LoadBattles()
		{
			path = Path.Combine(Environment.CurrentDirectory, "Data");
			path = Path.Combine(path, gameMain.GameDataSet);
			path = Path.Combine(path, "battles");

			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[] files = di.GetFiles("*.xml");

			battles = new List<string>();
			foreach (FileInfo file in files)
			{
				battles.Add(file.Name);
			}

			scrollBar.TopIndex = 0;
			if (battles.Count > 8)
			{
				maxBattleVisible = 8;
				scrollBar.SetAmountOfItems(battles.Count);
				scrollBar.SetEnabledState(true);
			}
			else
			{
				maxBattleVisible = battles.Count;
				scrollBar.SetAmountOfItems(8);
				scrollBar.SetEnabledState(false);
			}

			battleSelected = -1;
			commenceButton.Active = false;

			RefreshLabels();
		}

		private void RefreshLabels()
		{
			for (int i = 0; i < maxBattleVisible; i++)
			{
				battleButtons[i].Selected = false;
				battleButtons[i].SetButtonText(battles[i + scrollBar.TopIndex]);
				if (i == (battleSelected - scrollBar.TopIndex))
				{
					battleButtons[i].Selected = true;
				}
			}
		}
	}
}
