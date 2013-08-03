using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class InGameMenu : ScreenInterface
	{
		GameMain gameMain;
		Button[] buttons;
		Label version;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this.gameMain = gameMain;

			buttons = new Button[5];

			buttons[0] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Save Game", 400, 200, 175, 25);
			buttons[1] = new Button(SpriteName.NewGame, SpriteName.NewGame, string.Empty, 400, 275, 260, 40);
			buttons[2] = new Button(SpriteName.LoadGame, SpriteName.LoadGame, string.Empty, 400, 350, 260, 40);
			buttons[3] = new Button(SpriteName.Options, SpriteName.Options, string.Empty, 400, 500, 260, 40);
			buttons[4] = new Button(SpriteName.Exit, SpriteName.Exit, string.Empty, 400, 575, 260, 40);

			version = new Label("Version 0.4", 5, gameMain.ScreenHeight - 25);

			reason = null;
			return true;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			foreach (Button button in buttons)
			{
				button.Draw(drawingManagement);
			}
			version.Draw();
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			foreach (Button button in buttons)
			{
				button.UpdateHovering(x, y, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			foreach (Button button in buttons)
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
						case 1:
							gameMain.ClearAll();
							gameMain.ChangeToScreen(Screen.NewGame);
							break;
						case 4:
							gameMain.ClearAll();
							gameMain.ChangeToScreen(Screen.MainMenu);
							break;
						default:
							break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
		}
	}
}
