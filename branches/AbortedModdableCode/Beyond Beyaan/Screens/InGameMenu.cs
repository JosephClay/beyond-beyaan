using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class InGameMenu : ScreenInterface
	{
		GameMain gameMain;
		StretchableImage background;
		StretchButton[] buttons;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			buttons = new StretchButton[5];

			buttons[0] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, "New Game", 400, 200, 175, 30, 30, 13, gameMain.FontManager.GetDefaultFont());
			buttons[1] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, "Load Game", 400, 275, 175, 30, 30, 13, gameMain.FontManager.GetDefaultFont());
			buttons[2] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, "Save Game", 400, 350, 175, 30, 30, 13, gameMain.FontManager.GetDefaultFont());
			buttons[3] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, "Options", 400, 425, 175, 30, 30, 13, gameMain.FontManager.GetDefaultFont());
			buttons[4] = new StretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, "Exit to Main Menu", 400, 500, 175, 30, 30, 13, gameMain.FontManager.GetDefaultFont());

			background = new StretchableImage(350, 150, 275, 450, 60, 60, DrawingManagement.BorderBorder);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			background.Draw(drawingManagement);
			foreach (StretchButton button in buttons)
			{
				button.Draw(drawingManagement);
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			foreach (StretchButton button in buttons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			foreach (StretchButton button in buttons)
			{
				button.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				if (buttons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0:
							gameMain.ChangeToScreen(ScreenEnum.GalaxySetup);
							break;
						case 4:
							gameMain.ChangeToScreen(ScreenEnum.MainMenu);
							break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void Resize()
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(ScreenEnum.Galaxy);
			}
		}
	}
}
