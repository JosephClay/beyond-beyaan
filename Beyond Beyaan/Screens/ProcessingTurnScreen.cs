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
			camera.InitCamera(gameMain.galaxy.GalaxySize, 32);
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
			List<StarSystem> systems = gameMain.galaxy.GetStarsInArea(camera.CameraX - 4, camera.CameraY - 4, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2);
			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
				if (system.Type.Shader != null)
				{
						system.Type.Shader.Parameters["StarColor"].SetValue(system.Type.ShaderValue);
				}
				system.Type.Sprite.SetPosition(((((system.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), ((((system.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale));
				system.Type.Sprite.SetScale(0.25f, 0.25f);
				system.Type.Sprite.Draw();
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
			foreach (Squadron fleet in gameMain.empireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2))
			{
				/*if (fleet.HasTransports)
				{
					drawingManagement.DrawSprite(SpriteName.Transport, (int)((((fleet.GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((fleet.GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, 32, 32, fleet.Empire.EmpireColor);
				}
				else
				{*/
				GorgonLibrary.Graphics.Sprite icon = fleet.Empire.EmpireRace.GetFleetIcon();
				icon.Color = fleet.Empire.EmpireColor;
				icon.SetScale(1, 1);
				icon.SetPosition(((fleet.FleetLocation.X - (camera.CameraX * 32) - camera.XOffset - 16) * camera.Scale), ((fleet.FleetLocation.Y - (camera.CameraY * 32) - camera.YOffset - 16) * camera.Scale));
				icon.Draw();
					//drawingManagement.DrawSprite(SpriteName.Fleet, (int)((((fleet.GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((fleet.GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, 32, 32, fleet.Empire.EmpireColor);
				//}
			}
			if (updateSection != -1)
			{
				drawingManagement.DrawSprite(SpriteName.NormalBackgroundButton, (gameMain.ScreenWidth / 2) - 150, (gameMain.ScreenHeight / 2) - 20, 255, 300, 40, System.Drawing.Color.White);
				updateText.Draw();
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (stillMoving)
			{
				tickCount += frameDeltaTime;
				if (tickCount > 0.50f)
				{
					stillMoving = gameMain.empireManager.UpdateFleetMovement();
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
						updateText.MoveTo((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 1:
						Random r = new Random();
						gameMain.empireManager.UpdateEmpires(gameMain.galaxy, gameMain.planetTypeManager, r);
						updateText.SetText("Processing Influences");
						updateText.MoveTo((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 2:
						//gameMain.empireManager.UpdateInfluenceMaps(gameMain.galaxy);
						updateText.SetText("Processing Migration");
						updateText.MoveTo((int)((gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 3:
						gameMain.empireManager.UpdateMigration(gameMain.galaxy);
						gameMain.empireManager.LookForCombat();
						/*if (gameMain.empireManager.HasCombat)
						{
							gameMain.ChangeToScreen(Screen.Battle);
						}*/
						break;
					case 4:
						gameMain.empireManager.CheckForColonizers(gameMain.galaxy);
						if (gameMain.empireManager.HasColonizers)
						{
							gameMain.ChangeToScreen(Screen.Colonize);
						}
						break;
					case 5:
						gameMain.empireManager.CheckForInvaders(gameMain.galaxy);
						if (gameMain.empireManager.HasInvaders)
						{
							gameMain.ChangeToScreen(Screen.Invade);
						}
						break;
					case 6:
						updateSection = -1;
						gameMain.empireManager.ClearEmptyFleets();
						gameMain.empireManager.CheckForDefeatedEmpires();
						gameMain.empireManager.SetInitialEmpireTurn();
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
