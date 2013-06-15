using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class ProcessingTurnScreen : ScreenInterface
	{
		GameMain gameMain;
		Camera camera;
		bool stillMoving;
		float tickCount;
		int updateSection;
		Label updateText;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;
			camera = new Camera(gameMain.Galaxy.GalaxySize * 32, gameMain.Galaxy.GalaxySize * 32, gameMain);
			camera.CenterCamera(camera.Width / 2, camera.Height / 2, camera.MaxZoom);

			stillMoving = true;
			tickCount = 0;
			updateSection = -1;
			updateText = new Label(string.Empty, (gameMain.ScreenWidth / 2) - 130, (gameMain.ScreenHeight / 2) - 17);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = gameMain.Galaxy.GetStarsInArea(camera.CameraX, camera.CameraY, gameMain.ScreenWidth / camera.ZoomDistance, gameMain.ScreenHeight / camera.ZoomDistance);
			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
				gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				drawingManagement.DrawSprite(SpriteName.Star, (int)(((system.X * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((system.Y * 32) - camera.CameraY) * camera.ZoomDistance), 255, system.Size * 32 * camera.ZoomDistance, system.Size * 32 * camera.ZoomDistance, System.Drawing.Color.White);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
			foreach (Fleet fleet in gameMain.EmpireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, gameMain.ScreenWidth / camera.ZoomDistance, gameMain.ScreenHeight / camera.ZoomDistance))
			{
				drawingManagement.DrawSprite(SpriteName.Fleet, (int)(((fleet.GalaxyX * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((fleet.GalaxyY * 32) - camera.CameraY) * camera.ZoomDistance), 255, 32, 32, fleet.Empire.EmpireColor);
			}
			if (updateSection != -1)
			{
				drawingManagement.DrawSprite(SpriteName.NormalBackgroundButton, (gameMain.ScreenWidth / 2) - 150, (gameMain.ScreenHeight / 2) - 20, 255, 300, 40, System.Drawing.Color.White);
				updateText.Draw();
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (stillMoving)
			{
				tickCount += frameDeltaTime;
				if (tickCount > 0.50f)
				{
					stillMoving = gameMain.EmpireManager.UpdateFleetMovement(gameMain.Galaxy.GetGridCells());
					tickCount -= 0.50f;
				}
			}
			else
			{
				tickCount = 0;
				updateSection++;
				switch (updateSection)
				{
					case 0:
						updateText.SetText("Processing Empires");
						updateText.Move((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 1:
						gameMain.EmpireManager.UpdateEmpires(gameMain.Galaxy);
						updateText.SetText("Processing Influences");
						updateText.Move((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 2:
						gameMain.EmpireManager.UpdateInfluenceMaps(gameMain.Galaxy);
						updateText.SetText("Processing Migration");
						updateText.Move((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 3:
						gameMain.EmpireManager.UpdateMigration(gameMain.Galaxy);
						gameMain.EmpireManager.LookForCombat();
						if (gameMain.EmpireManager.HasCombat)
						{
							gameMain.ChangeToScreen(Screen.Battle);
						}
						break;
					case 4:
						updateSection = -1;
						gameMain.EmpireManager.SetInitialEmpireTurn();
						gameMain.ChangeToScreen(Screen.Galaxy);
						stillMoving = true;
						break;
				}
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
		}

		public void MouseUp(int x, int y, int whichButton)
		{
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}
	}
}
