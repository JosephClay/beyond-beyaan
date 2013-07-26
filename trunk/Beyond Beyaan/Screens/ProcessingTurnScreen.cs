using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class ProcessingTurnScreen : ScreenInterface
	{
		GameMain _gameMain;
		Camera camera;
		bool stillMoving;
		private bool resetted;
		int updateSection;
		Label updateText;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			camera = new Camera(gameMain.Galaxy.GalaxySize * 32, gameMain.Galaxy.GalaxySize * 32, gameMain.ScreenWidth, gameMain.ScreenHeight);
			camera.CenterCamera(camera.Width / 2, camera.Height / 2, camera.MaxZoom);

			stillMoving = true;
			updateSection = -1;
			updateText = new Label(string.Empty, (gameMain.ScreenWidth / 2) - 130, (gameMain.ScreenHeight / 2) - 17);
			resetted = false;

			reason = null;
			return true;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = _gameMain.Galaxy.GetStarsInArea(camera.CameraX, camera.CameraY, _gameMain.ScreenWidth / camera.ZoomDistance, _gameMain.ScreenHeight / camera.ZoomDistance);
			GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
			foreach (StarSystem system in systems)
			{
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				//drawingManagement.DrawSprite(SpriteName.Star, (int)(((system.X * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((system.Y * 32) - camera.CameraY) * camera.ZoomDistance), 255, system.Size * 32 * camera.ZoomDistance, system.Size * 32 * camera.ZoomDistance, System.Drawing.Color.White);
				system.Sprite.Draw((int)((system.X - camera.CameraX) * camera.ZoomDistance), (int)((system.Y - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance);
			}
			GorgonLibrary.Gorgon.CurrentShader = null;
			foreach (Fleet fleet in _gameMain.EmpireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, _gameMain.ScreenWidth / camera.ZoomDistance, _gameMain.ScreenHeight / camera.ZoomDistance))
			{
				fleet.Empire.EmpireRace.FleetIcon.Draw(((fleet.GalaxyX - camera.CameraX) * camera.ZoomDistance), ((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), 1, 1, fleet.Empire.EmpireColor);
			}
			if (updateSection != -1)
			{
				drawingManagement.DrawSprite(SpriteName.NormalBackgroundButton, (_gameMain.ScreenWidth / 2) - 150, (_gameMain.ScreenHeight / 2) - 20, 255, 300, 40, System.Drawing.Color.White);
				updateText.Draw();
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (!resetted)
			{
				_gameMain.EmpireManager.ResetFleetMovement();
				resetted = true;
			}
			if (stillMoving)
			{
				stillMoving = _gameMain.EmpireManager.UpdateFleetMovement(frameDeltaTime);
			}
			else
			{
				updateSection++;
				switch (updateSection)
				{
					case 0:
						updateText.SetText("Processing Empires");
						updateText.Move((int)((_gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((_gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 1:
						_gameMain.EmpireManager.UpdateEmpires(_gameMain.Galaxy);
						updateText.SetText("Processing Influences");
						updateText.Move((int)((_gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((_gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 2:
						//_gameMain.EmpireManager.UpdateInfluenceMaps(_gameMain.Galaxy);
						updateText.SetText("Processing Migration");
						updateText.Move((int)((_gameMain.ScreenWidth / 2) - (updateText.GetWidth() / 2)), (int)((_gameMain.ScreenHeight / 2) - (updateText.GetHeight() / 2)));
						break;
					case 3:
						//_gameMain.EmpireManager.UpdateMigration(_gameMain.Galaxy);
						//_gameMain.EmpireManager.LookForCombat();
						/*if (_gameMain.EmpireManager.HasCombat)
						{
							_gameMain.ChangeToScreen(Screen.Battle);
						}*/
						break;
					case 4:
						updateSection = -1;
						_gameMain.EmpireManager.SetInitialEmpireTurn();
						_gameMain.ChangeToScreen(Screen.Galaxy);
						stillMoving = true;
						resetted = false;
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
