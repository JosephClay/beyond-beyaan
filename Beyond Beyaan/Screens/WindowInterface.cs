using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Screens
{
	public class WindowInterface
	{
		protected GameMain gameMain;

		protected int xPos;
		protected int yPos;
		protected int mouseX;
		protected int mouseY;
		protected int origX;
		protected int origY;
		protected int windowWidth;
		protected int windowHeight;
		protected bool moving;
		protected bool moveable;

		protected StretchableImage backGroundImage;

		public WindowInterface(int x, int y, int width, int height, string title, GameMain gameMain, bool moveable)
		{
			xPos = x;
			yPos = y;
			windowWidth = width;
			windowHeight = height + 20;
			this.moveable = moveable;

			moving = false;
			this.gameMain = gameMain;

			backGroundImage = new StretchableImage(x, y, width, height, 60, 60, DrawingManagement.BorderBorderBG);
		}

		public virtual void DrawWindow(DrawingManagement drawingManagement)
		{
			backGroundImage.Draw(drawingManagement);
		}

		public virtual bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (moving)
			{
				xPos = (x - mouseX) + origX;
				yPos = (y - mouseY) + origY;

				if (xPos + windowWidth > gameMain.ScreenWidth)
				{
					xPos = gameMain.ScreenWidth - windowWidth;
				}
				if (yPos + windowHeight > gameMain.ScreenHeight)
				{
					yPos = gameMain.ScreenHeight - windowHeight;
				}
				if (xPos < 0)
				{
					xPos = 0;
				}
				if (yPos < 0)
				{
					yPos = 0;
				}
				return true;
			}
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				//Don't want items behind this window to be highlighted
				return true;
			}
			return false;
		}

		public virtual bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				if (moveable)
				{
					moving = true;
					mouseX = x;
					mouseY = y;
					origX = xPos;
					origY = yPos;
				}
				return true;
			}
			return false;
		}

		public virtual bool MouseUp(int x, int y)
		{
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				if (moving)
				{
					moving = false;
				}

				return true;
			}
			return false;
		}

		public virtual void MoveWindow()
		{
		}
	}
}
