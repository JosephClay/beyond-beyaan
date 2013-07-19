using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class MainGameMenu : ScreenInterface
	{
		GameMain gameMain;
		Button[] buttons;
		Label version;
		int x;
		int y;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this.gameMain = gameMain;

			buttons = new Button[6];

			x = (gameMain.ScreenWidth / 2) - 130;

			buttons[0] = new Button(SpriteName.Continue2, SpriteName.Continue, string.Empty, 400, gameMain.ScreenHeight - 350, 260, 40);
			buttons[1] = new Button(SpriteName.NewGame2, SpriteName.NewGame, string.Empty, 400, gameMain.ScreenHeight - 300, 260, 40);
			buttons[2] = new Button(SpriteName.LoadGame2, SpriteName.LoadGame, string.Empty, 400, gameMain.ScreenHeight - 250, 260, 40);
			buttons[3] = new Button(SpriteName.Customize2, SpriteName.Customize, string.Empty, 400, gameMain.ScreenHeight - 200, 260, 40);
			buttons[4] = new Button(SpriteName.Options2, SpriteName.Options, string.Empty, 400, gameMain.ScreenHeight - 150, 260, 40);
			buttons[5] = new Button(SpriteName.Exit2, SpriteName.Exit, string.Empty, 400, gameMain.ScreenHeight - 100, 260, 40);

			version = new Label("Version 0.4", 5, gameMain.ScreenHeight - 25);

			x = (gameMain.ScreenWidth / 2) - 512;
			y = 25;

			reason = null;
			return true;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			for (int i = 0; i < gameMain.ScreenWidth; i += 1024)
			{
				for (int j = 0; j < gameMain.ScreenHeight; j += 600)
				{
					drawingManagement.DrawSprite(SpriteName.TitleNebula, i, j, 255, System.Drawing.Color.White);
				}
			}
			drawingManagement.DrawSprite(SpriteName.TitlePlanet, x, y, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.TitleName, x, y, 255, System.Drawing.Color.White);
			foreach (Button button in buttons)
			{
				button.Draw(drawingManagement);
			}
			version.Draw();
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			foreach (Button button in buttons)
			{
				button.UpdateHovering(mouseX, mouseY, frameDeltaTime);
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
							gameMain.ChangeToScreen(Screen.NewGame);
							break;
						case 5:
							gameMain.ExitGame();
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
		}
	}
}
