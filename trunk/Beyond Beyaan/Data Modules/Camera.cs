using System.Collections.Generic;

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

		private int gridCells; //size of area in terms of gridCells (ie 10x10 is 10)

		private List<Point> gridSizes;
		private int maxZoom;
		private List<float> scales;
		#endregion

		#region Auto Properties

		public int CameraX { get; private set; }
		public int CameraY { get; private set; }

		public int XOffset { get { return (int)xOffset; } }
		public int YOffset { get { return (int)yOffset; } }

		public int ZoomDistance { get; private set; }
		public float Scale { get { return scales[ZoomDistance]; } }
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
			return gridSizes[ZoomDistance];
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
			CameraX = x - (gridSizes[ZoomDistance].X / 2);
			CameraY = y - (gridSizes[ZoomDistance].Y / 2);

			CheckPosition();
		}

		public void HandleUpdate(int mouseX, int mouseY, float frameDeltaTime)
		{
			xVel = 0;
			yVel = 0;
			if (mouseY < 40 && CameraY > -5)
			{
				int y = mouseY - 40;
				y = y * (-y);
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseY >= screenHeight - 40 && CameraY < (gridCells - gridSizes[ZoomDistance].Y) + 5)
			{
				int y = 40 - (screenHeight - mouseY);
				y = y * y;
				yVel = (y / 10000.0f) * (5000 * frameDeltaTime);
			}
			if (mouseX < 40 && CameraX > -5)
			{
				int x = mouseX - 40;
				x = x * (-x);
				xVel = (x / 10000.0f) * (5000 * frameDeltaTime);
			}
			else if (mouseX >= screenWidth - 40 && CameraX < (gridCells - gridSizes[ZoomDistance].X) + 5)
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
			ZoomDistance = maxZoom - 1;

			CameraX = 0;
			CameraY = 0;

			//Redundant, but in case I change the above values, this is a safety catch.
			CheckPosition();
		}

		public void ZoomIn(int x, int y)
		{
			ZoomDistance = 0;

			CenterCamera(x, y);
		}

		private void ChangeZoom(bool direction, int mouseX, int mouseY, int screenWidth, int screenHeight)
		{
			if (direction == false && ZoomDistance < maxZoom - 1)
			{
				int xMove = gridSizes[ZoomDistance + 1].X - gridSizes[ZoomDistance].X;
				int yMove = gridSizes[ZoomDistance + 1].Y - gridSizes[ZoomDistance].Y;

				CameraX -= (xMove / 2);
				CameraY -= (yMove / 2);

				ZoomDistance++;
			}
			else if (direction && ZoomDistance > 0)
			{
				//find the grid cell the mouse is hovering above
				float size = gridSize * scales[ZoomDistance];
				int gridX = (int)(mouseX / size);
				int gridY = (int)(mouseY / size);

				float xPer = gridX / (float)gridSizes[ZoomDistance].X;
				float yPer = gridY / (float)gridSizes[ZoomDistance].Y;

				int xMove = (int)((gridSizes[ZoomDistance].X - gridSizes[ZoomDistance - 1].X) * xPer);
				int yMove = (int)((gridSizes[ZoomDistance].Y - gridSizes[ZoomDistance - 1].Y) * yPer);

				CameraX += xMove;
				CameraY += yMove;

				ZoomDistance--;
			}

			CheckPosition();
		}

		private void CheckPosition()
		{
			if (CameraX > (gridCells - gridSizes[ZoomDistance].X) + 5)
			{
				CameraX = (gridCells - gridSizes[ZoomDistance].X) + 5;
			}
			if (CameraX < -5)
			{
				CameraX = -5;
			}
			if (CameraY > (gridCells - gridSizes[ZoomDistance].Y) + 5)
			{
				CameraY = (gridCells - gridSizes[ZoomDistance].Y) + 5;
			}
			if (CameraY < -5)
			{
				CameraY = -5;
			}
		}

		private void MoveCamera()
		{
			xOffset += xVel;
			while (xOffset < 0)
			{
				xOffset += gridSize;
				CameraX--;
				if (CameraX < -5)
				{
					CameraX = -5;
					xVel = 0;
					xOffset = 0;
					break;
				}
			}
			while (xOffset >= gridSize)
			{
				xOffset -= gridSize;
				CameraX++;
				if (CameraX > (gridCells - gridSizes[ZoomDistance].X) + 5)
				{
					CameraX = (gridCells - gridSizes[ZoomDistance].X) + 5;
					xVel = 0;
					xOffset = 0;
					break;
				}
			}
			yOffset += yVel;
			while (yOffset < 0)
			{
				yOffset += gridSize;
				CameraY--;
				if (CameraY < -5)
				{
					CameraY = -5;
					yVel = 0;
					yOffset = 0;
					break;
				}
			}
			while (yOffset >= gridSize)
			{
				yOffset -= gridSize;
				CameraY++;
				if (CameraY > (gridCells - gridSizes[ZoomDistance].Y) + 5)
				{
					CameraY = (gridCells - gridSizes[ZoomDistance].Y) + 5;
					yVel = 0;
					yOffset = 0;
					break;
				}
			}
		}
		#endregion
	}
}
