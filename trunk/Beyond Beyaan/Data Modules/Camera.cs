using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	class Camera
	{
		#region Member Variables
		private int width;
		private int height;

		private int windowWidth;
		private int windowHeight;

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
		public Camera(int width, int height, int windowWidth, int windowHeight)
		{
			this.width = width;
			this.height = height;

			this.windowWidth = windowWidth;
			this.windowHeight = windowHeight;

			maxZoom = 1.0f;
			while (true)
			{
				maxZoom -= 0.05f;
				if (width * maxZoom < windowWidth && height * maxZoom < windowHeight)
				{
					//Get a maxZoom that have the items fill the screen
					maxZoom = windowWidth / (float)width;
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

			cameraX = x - (windowWidth / zoomDistance) / 2;
			cameraY = y - (windowHeight / zoomDistance) / 2;

			CheckPosition();
		}

		public void HandleUpdate(int mouseX, int mouseY, float frameDeltaTime)
		{
			xVel = 0;
			yVel = 0;
			if (mouseY < 40 && cameraY > ((windowHeight / zoomDistance) / -2))
			{
				int y = mouseY - 40;
				y = y * (-y);
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseY >= windowHeight - 40 && cameraY < (height - (windowHeight / zoomDistance) / 2))
			{
				int y = 40 - (windowHeight - mouseY);
				y = y * y;
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			if (mouseX < 40 && cameraX > ((windowWidth / zoomDistance) / -2))
			{
				int x = mouseX - 40;
				x = x * (-x);
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseX >= windowWidth - 40 && cameraX < (width - (windowWidth / zoomDistance) / 2))
			{
				int x = 40 - (windowWidth - mouseX);
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

		public void MouseWheel(int direction, float mouseX, float mouseY)
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

					float xScale = mouseX / windowWidth;
					float yScale = mouseY / windowHeight;

					cameraX += ((windowWidth / oldScale) - (windowWidth / zoomDistance)) * xScale;
					cameraY += ((windowHeight / oldScale) - (windowHeight / zoomDistance)) * yScale;
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

					cameraX -= ((windowWidth / zoomDistance) - (windowWidth / oldScale)) / 2;
					cameraY -= ((windowHeight / zoomDistance) - (windowHeight / oldScale)) / 2;
				}
			}
			CheckPosition();
		}

		private void CheckPosition()
		{
			if (cameraX > (width - (windowWidth / zoomDistance) / 2))
			{
				cameraX = (width - (windowWidth / zoomDistance) / 2);
			}
			if (cameraX < ((windowWidth / zoomDistance) / -2))
			{
				cameraX = ((windowWidth / zoomDistance) / -2);
			}
			if (cameraY > (height - (windowHeight / zoomDistance) / 2))
			{
				cameraY = (height - (windowHeight / zoomDistance) / 2);
			}
			if (cameraY < ((windowHeight / zoomDistance) / -2))
			{
				cameraY = ((windowHeight / zoomDistance) / -2);
			}
		}
		#endregion
	}
}
