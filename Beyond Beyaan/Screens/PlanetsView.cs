using System;

namespace Beyond_Beyaan.Screens
{
	public class PlanetsView : WindowInterface
	{
		public Action CloseWindow;

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 800, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}
			return true;
		}
		#endregion

		public void Load()
		{
			
		}

		public override void Draw()
		{
			base.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool MouseDown(int x, int y)
		{
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			if (!base.MouseUp(x, y))
			{
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return false;
		}
	}
}
