using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class SituationReport
	{
		private const int AMOUNT_VISIBLE = 10;
		private GameMain gameMain;

		private Label title;
		//private Button[] buttons;
		private ScrollBar scrollBar;
		private int topIndex;
		private bool isVisible;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			/*buttons = new Button[AMOUNT_VISIBLE];
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, x + 5, y + 35 + (i * 40), 775, 40);
			}*/
			scrollBar = new ScrollBar(x + 780, y + 25, 16, 574, AMOUNT_VISIBLE, AMOUNT_VISIBLE, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont());
			topIndex = 0;
			isVisible = false;
			title = new Label("Situation Report", x + 5, y + 5, gameMain.FontManager.GetDefaultFont());
		}

		public void ResetIndex()
		{
			topIndex = 0;
		}

		public void DrawSitRep(DrawingManagement drawingManagement)
		{
			//if (!isVisible)
			{
				return;
			}
			/*SitRepManager sitRepManager = gameMain.empireManager.CurrentEmpire.SitRepManager;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			//drawingManagement.DrawSprite(SpriteName.Screen, x, y, 255, 800, 600, System.Drawing.Color.White);
			int maxVisible = AMOUNT_VISIBLE;
			if (sitRepManager.Items.Count > AMOUNT_VISIBLE)
			{
				scrollBar.Draw(drawingManagement);
			}
			else
			{
				maxVisible = sitRepManager.Items.Count;
			}

			title.Draw();

			for (int i = 0; i < maxVisible; i++)
			{
				buttons[i].Draw(drawingManagement);
			}*/
		}

		public bool Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.empireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				if (scrollBar.MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					topIndex = scrollBar.TopIndex;
					RefreshLabels(sitRepManager);
				}
			}
			for (int i = 0; i < maxVisible; i++)
			{
				//buttons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			return true;
		}

		public bool MouseDown(int x, int y)
		{
			if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.empireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				scrollBar.MouseDown(x, y);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				//buttons[i].MouseDown(x, y);
			}
			return true;
		}

		public bool MouseUp(int x, int y)
		{
			if (!isVisible)
			{
				return false;
			}

			SitRepManager sitRepManager = gameMain.empireManager.CurrentEmpire.SitRepManager;
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}
			else
			{
				if (scrollBar.MouseUp(x, y))
				{
					topIndex = scrollBar.TopIndex;
					RefreshLabels(sitRepManager);
				}
			}
			for (int i = 0; i < maxVisible; i++)
			{
				/*if (buttons[i].MouseUp(x, y))
				{
					SitRepItem item = sitRepManager.Items[i + topIndex];
					gameMain.ChangeToScreen(item.ScreenEventIsIn);
					if (item.SystemEventOccuredAt != null)
					{
						gameMain.empireManager.CurrentEmpire.SelectedSystem = gameMain.empireManager.CurrentEmpire.SelectedSystem = item.SystemEventOccuredAt;
						gameMain.empireManager.CurrentEmpire.SelectedFleetGroup = null;
					}
					if (item.PlanetEventOccuredAt != null)
					{
						for (int j = 0; j < item.SystemEventOccuredAt.Planets.Count; j++)
						{
							if (item.SystemEventOccuredAt.Planets[j] == item.PlanetEventOccuredAt)
							{
								item.SystemEventOccuredAt.SetPlanetSelected(gameMain.empireManager.CurrentEmpire, j);
							}
						}
					}
					if (item.PointEventOccuredAt != null)
					{
						gameMain.CenterGalaxyScreen(item.PointEventOccuredAt);
					}
					isVisible = false;
				}*/
			}
			return true;
		}

		public void Refresh()
		{
			scrollBar.SetAmountOfItems(gameMain.empireManager.CurrentEmpire.SitRepManager.Items.Count);
			scrollBar.TopIndex = 0;
			topIndex = 0;
			isVisible = gameMain.empireManager.CurrentEmpire.SitRepManager.HasItems;
			RefreshLabels(gameMain.empireManager.CurrentEmpire.SitRepManager);
		}

		public void Clear()
		{
			scrollBar.SetAmountOfItems(10);
			scrollBar.TopIndex = 0;
			topIndex = 0;
			isVisible = false;
		}

		public void ToggleVisibility()
		{
			isVisible = !isVisible;
		}

		public void Show()
		{
			isVisible = true;
		}

		public void Hide()
		{
			isVisible = false;
		}

		private void RefreshLabels(SitRepManager sitRepManager)
		{
			int maxVisible = AMOUNT_VISIBLE;

			if (sitRepManager.Items.Count < AMOUNT_VISIBLE)
			{
				maxVisible = sitRepManager.Items.Count;
			}

			for (int i = 0; i < maxVisible; i++)
			{
				//buttons[i].SetButtonText(sitRepManager.Items[topIndex + i].EventMessage);
			}
		}
	}
}
