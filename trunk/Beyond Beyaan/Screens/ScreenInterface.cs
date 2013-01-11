﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	interface ScreenInterface
	{
		void Initialize(GameMain gameMain);

		void DrawScreen(DrawingManagement drawingManagement);

		void Update(int mouseX, int mouseY, float frameDeltaTime);

		void UpdateBackground(float frameDeltaTime);

		void MouseDown(int x, int y, int whichButton);

		void MouseUp(int x, int y, int whichButton);

		void MouseScroll(int direction, int x, int y);

		void KeyDown(KeyboardInputEventArgs e);

		void Resize();
	}
}