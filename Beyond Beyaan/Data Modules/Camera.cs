using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	class Camera
	{
		#region Member Variables
		GameMain gameMain;

		private int width;
		private int height;

		private float xVel; //for camera movement
		private float yVel; //for camera movement

		private float cameraX, cameraY; //The position where the player is looking at
		private float zoomDistance; //The distance from the galaxy "plane" the player is looking from

		private float maxZoom;
		#endregion

		#region Auto Properties
		public int Width { get { return width; } }
		public int Height { get { return height; } }

		public float CameraX { get { return cameraX; } }
		public float CameraY { get { return cameraY; } }

		public float ZoomDistance { get { return zoomDistance; } }
		public float MaxZoom { get { return maxZoom; } }
		#endregion

		#region Constructors
		public Camera(int width, int height, GameMain gameMain)
		{
			this.gameMain = gameMain;

			this.width = width;
			this.height = height;

			maxZoom = 1.0f;
			while (true)
			{
				maxZoom -= 0.05f;
				if (width * maxZoom < gameMain.ScreenWidth && height * maxZoom < gameMain.ScreenHeight)
				{
					break;
				}
				if (maxZoom <= 0)
				{
					maxZoom = 0.05f;
					break;
				}
			}

			zoomDistance = 1.0f;
		}
		#endregion

		#region Functions
		public void CenterCamera(int x, int y, float zoomDis)
		{
			zoomDistance = zoomDis;
			if (zoomDistance < maxZoom)
			{
				zoomDistance = maxZoom;
			}

			cameraX = x - (gameMain.ScreenWidth / zoomDistance) / 2;
			cameraX = y - (gameMain.ScreenHeight / zoomDistance) / 2;

			CheckPosition();
		}

		public void HandleUpdate(int mouseX, int mouseY, float frameDeltaTime)
		{
			xVel = 0;
			yVel = 0;
			if (mouseY < 40 && cameraY > ((gameMain.ScreenHeight / zoomDistance) / -2))
			{
				int y = mouseY - 40;
				y = y * (-y);
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseY >= gameMain.ScreenHeight - 40 && cameraY < (height - (gameMain.ScreenHeight / zoomDistance) / 2))
			{
				int y = 40 - (gameMain.ScreenHeight - mouseY);
				y = y * y;
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			if (mouseX < 40 && cameraX > ((gameMain.ScreenWidth / zoomDistance) / -2))
			{
				int x = mouseX - 40;
				x = x * (-x);
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseX >= gameMain.ScreenWidth - 40 && cameraX < (width - (gameMain.ScreenWidth / zoomDistance) / 2))
			{
				int x = 40 - (gameMain.ScreenWidth - mouseX);
				x = x * x;
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}

			if (xVel != 0 || yVel != 0)
			{
				cameraX += xVel;
				cameraY += yVel;

				CheckPosition();
			}
		}

		public void MouseWheel(int direction, int mouseX, int mouseY)
		{
			if (direction > 0)
			{
				if (zoomDistance < 1)
				{
					float oldScale = zoomDistance;
					zoomDistance += 0.05f;
					if (zoomDistance >= 1)
					{
						zoomDistance = 1;
					}

					float xScale = (mouseX - cameraX) / (float)gameMain.ScreenWidth;
					float yScale = (mouseY - cameraY) / (float)gameMain.ScreenHeight;

					cameraX -= ((width / zoomDistance) - (width / (oldScale))) * xScale;
					cameraY -= ((height / zoomDistance) - (height / (oldScale))) * yScale;
				}
			}
			else
			{
				if (zoomDistance > maxZoom)
				{
					float oldScale = zoomDistance;
					zoomDistance -= 0.05f;
					if (zoomDistance < maxZoom)
					{
						zoomDistance = maxZoom;
					}

					cameraX -= ((width / zoomDistance) - (width / (oldScale))) / 2;
					cameraY -= ((height / zoomDistance) - (height / (oldScale))) / 2;
				}
			}
			CheckPosition();
		}

		private void CheckPosition()
		{
			if (cameraX > (width - (gameMain.ScreenWidth / zoomDistance) / 2))
			{
				cameraX = (width - (gameMain.ScreenWidth / zoomDistance) / 2);
			}
			if (cameraX < ((gameMain.ScreenWidth / zoomDistance) / -2))
			{
				cameraX = ((gameMain.ScreenWidth / zoomDistance) / -2);
			}
			if (cameraY > (height - (gameMain.ScreenHeight / zoomDistance) / 2))
			{
				cameraY = (height - (gameMain.ScreenHeight / zoomDistance) / 2);
			}
			if (cameraY < ((gameMain.ScreenHeight / zoomDistance) / -2))
			{
				cameraY = ((gameMain.ScreenHeight / zoomDistance) / -2);
			}
		}
		#endregion
	}
}
