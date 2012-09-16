using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Screens
{
	public class BackgroundStars
	{
		private BackgroundStar[] stars;
		private float tick;
		private int twinkleFrame;

		public BackgroundStars(int galaxySize, Random r, int maxSize)
		{
			galaxySize += galaxySize;
			stars = new BackgroundStar[galaxySize * 20];
			for (int i = 0; i < stars.Length; i++)
			{
				System.Drawing.Color color = System.Drawing.Color.White;
				switch (r.Next(4))
				{
					case 0: color = System.Drawing.Color.LightGray;
						break;
					case 1: color = System.Drawing.Color.Gray;
						break;
					case 2: color = System.Drawing.Color.DarkGray;
						break;
				}
				stars[i] = new BackgroundStar(r.Next(maxSize), r.Next(galaxySize * 32), r.Next(galaxySize * 32), r.Next(5, 15), r.Next(300), color);
			}
			tick = 0;
			twinkleFrame = 0;
		}

		public void Update(float frameDeltaTime)
		{
			tick += frameDeltaTime;
			if (tick >= 0.25f)
			{
				tick -= 0.25f;
				twinkleFrame++;
				if (twinkleFrame >= 300)
				{
					twinkleFrame = 0;
				}
			}
		}

		public void Draw(int cameraX, int cameraY, int cameraWidth, int cameraHeight, float xOffset, float yOffset, float scale, DrawingManagement drawingManagement)
		{
			float realX = ((cameraX * 32) * scale);
			float realY = ((cameraY * 32) * scale);

			float realWidth = ((cameraWidth * 32 + 32) * scale);
			float realHeight = ((cameraHeight * 32 + 32) * scale);

			float scaledSize = scale * 16;
			int threshold = (int)scaledSize;

			foreach (BackgroundStar star in stars)
			{
				//float convertedX = star.X + ((cameraX * 32) * star.Layer);
				//float convertedY = star.Y + ((cameraY * 32) * star.Layer);
				float convertedX = (star.X - ((realX + (xOffset * scale)) * (1.0f / star.Layer))) * scale;
				float convertedY = (star.Y - ((realY + (yOffset * scale)) * (1.0f / star.Layer))) * scale;
				if (convertedX >= 0 && convertedX < realWidth && convertedY >= 0 && convertedY < realHeight)
				{

					int size = star.Size;//star.TwinkleMoment == twinkleFrame ? (star.Size + 2) : star.TwinkleMoment == twinkleFrame - 1 ? star.Size + 1 : star.Size;
					System.Drawing.Color starColor = star.TwinkleMoment == twinkleFrame ? System.Drawing.Color.White : star.color;
					switch (size)
					{
						case 0:
							if (threshold >= 16)
								drawingManagement.DrawSprite(SpriteName.BGStar1, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						case 1:
							if (threshold >= 13)
								drawingManagement.DrawSprite(SpriteName.BGStar2, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						case 2:
							if (threshold >= 10)
								drawingManagement.DrawSprite(SpriteName.BGStar3, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						case 3:
							if (threshold >= 8)
								drawingManagement.DrawSprite(SpriteName.BGStar4, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						case 4:
							if (threshold >= 6)
								drawingManagement.DrawSprite(SpriteName.BGStar5, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						case 5:
							if (threshold >= 3)
								drawingManagement.DrawSprite(SpriteName.BGStar6, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
						default:
							if (threshold >= 1)
								drawingManagement.DrawSprite(SpriteName.BGStar7, (int)(convertedX), (int)(convertedY), 255, scaledSize, scaledSize, starColor);
							break;
					}
				}
			}
		}
	}

	public class BackgroundStar
	{
		public int X;
		public int Y;
		public int Layer;
		public int Size;
		public System.Drawing.Color color;

		public int TwinkleMoment;

		public BackgroundStar(int size, int x, int y, int layer, int twinkleMoment, System.Drawing.Color color)
		{
			X = x;
			Y = y;
			Size = size / 10;
			Layer = layer;
			TwinkleMoment = twinkleMoment;
			this.color = color;
		}
	}
}
