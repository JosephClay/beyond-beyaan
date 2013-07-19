using System;
using GorgonLibrary.InputDevices;

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

		protected BBStretchableImage backGroundImage;

		public bool Initialize(int x, int y, int width, int height, GameMain gameMain, bool moveable, Random r, out string reason)
		{
			xPos = x;
			yPos = y;
			windowWidth = width;
			windowHeight = height;
			this.moveable = moveable;

			moving = false;
			this.gameMain = gameMain;

			backGroundImage = new BBStretchableImage();
			if (!backGroundImage.Initialize(x, y, width, height, StretchableImageType.MediumBorder, r, out reason))
			{
				return false;
			}
			reason = null;
			return true;
		}

		public virtual void Draw()
		{
			backGroundImage.Draw();
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
				MoveWindow();
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
			if (moving)
			{
				//If it was moving, no matter what, it should capture the mouse up since it's releasing the moving grip
				moving = false;
				return true;
			}
			if (x >= xPos && x < xPos + windowWidth && y >= yPos && y < yPos + windowHeight)
			{
				return true;
			}
			return false;
		}

		public virtual bool KeyDown(KeyboardInputEventArgs e)
		{
			return false;
		}

		public virtual void MoveWindow()
		{
			backGroundImage.MoveTo(xPos, yPos);
		}
	}
}
