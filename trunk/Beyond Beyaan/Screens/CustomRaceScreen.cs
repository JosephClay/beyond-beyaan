using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class CustomRaceScreen : ScreenInterface
	{
		private GameMain gameMain;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			reason = null;
			this.gameMain = gameMain;
			return true;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
		}
		
		public void MouseDown(int x, int y, int whichButton)
		{
		}

		public void MouseUp(int x, int y, int whichButton)
		{
		}

		public void MouseScroll(int x, int y, int whichDirection)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}
	}
}
