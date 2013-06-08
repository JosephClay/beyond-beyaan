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
		float[] sizes;
		int updateSection;
		Label updateText;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;
			camera = new Camera(gameMain.ScreenWidth, gameMain.ScreenHeight);
			camera.InitCamera(gameMain.Galaxy.GalaxySize, 32);
			camera.ZoomOut();

			sizes = new float[4];
			sizes[0] = 32 * camera.Scale;
			sizes[1] = 2 * 32 * camera.Scale;
			sizes[2] = 3 * 32 * camera.Scale;
			sizes[3] = 4 * 32 * camera.Scale;

			stillMoving = true;
			tickCount = 0;
			updateSection = -1;
			updateText = new Label(string.Empty, (gameMain.ScreenWidth / 2) - 130, (gameMain.ScreenHeight / 2) - 17);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = gameMain.Galaxy.GetStarsInArea(camera.CameraX - 4, camera.CameraY - 4, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2);
			foreach (StarSystem system in systems)
			{
				if (system.Type == StarType.BLACK_HOLE)
				{
					drawingManagement.DrawSprite(SpriteName.BlackHole, (int)((((system.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((system.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[system.Size - 1], sizes[system.Size - 1], System.Drawing.Color.White);
				}
				else
				{
					GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
					gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
					drawingManagement.DrawSprite(SpriteName.Star, (int)((((system.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((system.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[system.Size - 1], sizes[system.Size - 1], System.Drawing.Color.White);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}
			}
			foreach (Fleet fleet in gameMain.EmpireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2))
			{
				drawingManagement.DrawSprite(SpriteName.Fleet, (int)((((fleet.GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((fleet.GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, 32, 32, fleet.Empire.EmpireColor);
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

		public void Resize()
		{
			camera.ResizeScreen(gameMain.ScreenWidth, gameMain.ScreenHeight);
			sizes = new float[4];
			sizes[0] = 32 * camera.Scale;
			sizes[1] = 2 * 32 * camera.Scale;
			sizes[2] = 3 * 32 * camera.Scale;
			sizes[3] = 4 * 32 * camera.Scale;
		}
	}
}
