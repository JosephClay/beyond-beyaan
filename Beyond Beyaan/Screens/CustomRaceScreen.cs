using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class CustomRaceScreen : ScreenInterface
	{
		private GameMain gameMain;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
		}

		public void UpdateBackground(float frameDeltaTime)
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

		public void Resize()
		{
		}
	}
}
