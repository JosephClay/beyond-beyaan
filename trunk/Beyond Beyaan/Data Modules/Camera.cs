using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	class Camera
	{
		#region Member Variables
		private int gridSize;
		private int screenWidth;
		private int screenHeight;

		private float xVel; //for camera movement
		private float yVel; //for camera movement

		private float xOffset;
		private float yOffset;

		private int cameraX, cameraY; //The position where the player is looking at
		private int zoomDistance; //The distance from the galaxy "plane" the player is looking from
		private int gridCells; //size of area in terms of gridCells (ie 10x10 is 10)

		private List<Point> gridSizes;
		private int maxZoom;
		private List<float> scales;
		#endregion

		#region Auto Properties
		public int CameraX { get { return cameraX; } }
		public int CameraY { get { return cameraY; } }

		public int XOffset { get { return (int)xOffset; } }
		public int YOffset { get { return (int)yOffset; } }

		public int ZoomDistance { get { return zoomDistance; } }
		public float Scale { get { return scales[zoomDistance]; } }
		#endregion

		#region Constructors
		public Camera(int screenWidth, int screenHeight)
		{
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;
		}
		#endregion

		#region Functions
		public void InitCamera(int gridCells, int gridSize)
		{
			this.gridCells = gridCells;
			this.gridSize = gridSize;

			CalculateZoom();
		}

		public void ResizeScreen(int screenWidth, int screenHeight)
		{
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;

			CalculateZoom();
		}

		public Point GetViewSize()
		{
			return gridSizes[zoomDistance];
		}

		private void CalculateZoom()
		{
			if (gridCells == 0)
			{
				return;
			}
			//calculate the rows/columns for each zoom distance
			float scale = 1;
			int increment = 1;
			gridSizes = new List<Point>();
			scales = new List<float>();
			while (true)
			{
				float size = gridSize * scale;
				Point scaledGridSize = new Point((int)(screenWidth / size), (int)(screenHeight / size));
				gridSizes.Add(scaledGridSize);
				scales.Add(scale);
				if (scaledGridSize.Y >= gridCells)
				{
					maxZoom = increment;
					break;
				}
				scale *= 0.75f;
				increment++;
			}

			CheckPosition();
		}

		public void CenterCamera(int x, int y)
		{
			cameraX = x - (gridSizes[zoomDistance].X / 2);
			cameraY = y - (gridSizes[zoomDistance].Y / 2);

			CheckPosition();
		}

		public void HandleUpdate(int mouseX, int mouseY, float frameDeltaTime)
		{
			xVel = 0;
			yVel = 0;
			if (mouseY < 40 && cameraY > -5)
			{
				int y = mouseY - 40;
				y = y * (-y);
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseY >= screenHeight - 40 && cameraY < (gridCells - gridSizes[zoomDistance].Y) + 5)
			{
				int y = 40 - (screenHeight - mouseY);
				y = y * y;
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			if (mouseX < 40 && cameraX > -5)
			{
				int x = mouseX - 40;
				x = x * (-x);
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseX >= screenWidth - 40 && cameraX < (gridCells - gridSizes[zoomDistance].X) + 5)
			{
				int x = 40 - (screenWidth - mouseX);
				x = x * x;
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}

			if (xVel != 0 || yVel != 0)
			{
				MoveCamera();
			}
		}

		public void MouseWheel(int direction, int mouseX, int mouseY)
		{
			ChangeZoom(direction > 0, mouseX, mouseY, screenWidth, screenHeight);
		}

		public void ZoomOut()
		{
			zoomDistance = maxZoom - 1;

			cameraX = 0;
			cameraY = 0;

			//Redundant, but in case I change the above values, this is a safety catch.
			CheckPosition();
		}

		public void ZoomIn(int x, int y)
		{
			zoomDistance = 0;

			CenterCamera(x, y);
		}

		private void ChangeZoom(bool direction, int mouseX, int mouseY, int screenWidth, int screenHeight)
		{
			if (direction == false && zoomDistance < maxZoom - 1)
			{
				int xMove = gridSizes[zoomDistance + 1].X - gridSizes[zoomDistance].X;
				int yMove = gridSizes[zoomDistance + 1].Y - gridSizes[zoomDistance].Y;

				cameraX -= (xMove / 2);
				cameraY -= (yMove / 2);

				zoomDistance++;
			}
			else if (direction && zoomDistance > 0)
			{
				//find the grid cell the mouse is hovering above
				float size = gridSize * scales[zoomDistance];
				int gridX = (int)(mouseX / size);
				int gridY = (int)(mouseY / size);

				float xPer = (float)gridX / (float)gridSizes[zoomDistance].X;
				float yPer = (float)gridY / (float)gridSizes[zoomDistance].Y;

				int xMove = (int)((gridSizes[zoomDistance].X - gridSizes[zoomDistance - 1].X) * xPer);
				int yMove = (int)((gridSizes[zoomDistance].Y - gridSizes[zoomDistance - 1].Y) * yPer);

				cameraX += xMove;
				cameraY += yMove;

				zoomDistance--;
			}

			CheckPosition();
		}

		private void CheckPosition()
		{
			if (cameraX > (gridCells - gridSizes[zoomDistance].X) + 5)
			{
				cameraX = (gridCells - gridSizes[zoomDistance].X) + 5;
			}
			if (cameraX < -5)
			{
				cameraX = -5;
			}
			if (cameraY > (gridCells - gridSizes[zoomDistance].Y) + 5)
			{
				cameraY = (gridCells - gridSizes[zoomDistance].Y) + 5;
			}
			if (cameraY < -5)
			{
				cameraY = -5;
			}
		}

		private void MoveCamera()
		{
			xOffset += xVel;
			while (xOffset < 0)
			{
				xOffset += gridSize;
				cameraX--;
				if (cameraX < -5)
				{
					cameraX = -5;
					xVel = 0;
					xOffset = 0;
					break;
				}
			}
			while (xOffset >= gridSize)
			{
				xOffset -= gridSize;
				cameraX++;
				if (cameraX > (gridCells - gridSizes[zoomDistance].X) + 5)
				{
					cameraX = (gridCells - gridSizes[zoomDistance].X) + 5;
					xVel = 0;
					xOffset = 0;
					break;
				}
			}
			yOffset += yVel;
			while (yOffset < 0)
			{
				yOffset += gridSize;
				cameraY--;
				if (cameraY < -5)
				{
					cameraY = -5;
					yVel = 0;
					yOffset = 0;
					break;
				}
			}
			while (yOffset >= gridSize)
			{
				yOffset -= gridSize;
				cameraY++;
				if (cameraY > (gridCells - gridSizes[zoomDistance].Y) + 5)
				{
					cameraY = (gridCells - gridSizes[zoomDistance].Y) + 5;
					yVel = 0;
					yOffset = 0;
					break;
				}
			}
		}
		#endregion
	}
}
