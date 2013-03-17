using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using GorgonLibrary.Graphics;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class GalaxyScreen : ScreenInterface
	{
		private BackgroundStars backgroundStars;
		private GameMain gameMain;
		private Camera camera;

		private GorgonLibrary.Graphics.RenderTarget oldTarget;
		private GorgonLibrary.Graphics.RenderImage starName;

		private float rotation;
		private bool pressedInWindow;

		private Button transferOKButton;
		private Button transferCancelButton;
		private ScrollBar[] popTransferSliders;
		private Label[] amountPopTransferLabel;
		private ScrollBar listOfPopTransferScrollBar;
		//private bool showingSplitWindow;
		private Label transferUpkeepCost;

		private SystemWindow systemWindow;
		private SquadronListWindow squadronListWindow;
		//private SplitPopulation splitPopulation;

		//private AnimatedImage fleetDest;
		//private AnimatedImage stargate;

		private Point LastClickedPosition; //used for refreshing fleet list
		private Point whichGridCellClicked;

		private GorgonLibrary.Graphics.Sprite movementPath;
		private GorgonLibrary.Graphics.Sprite starlaneSprite;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			camera = new Camera(gameMain.ScreenWidth, gameMain.ScreenHeight);
			camera.InitCamera(gameMain.galaxy.GalaxySize, 32);

			pressedInWindow = false;

			starName = new GorgonLibrary.Graphics.RenderImage("starNameRendered", 1, 1, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			starName.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;

			//transferButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Transfer Population", gameMain.ScreenWidth - 245, 370, 240, 25);

			transferOKButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Create Transport", gameMain.ScreenWidth / 2 + 10, gameMain.ScreenHeight / 2 + 70, 150, 25);
			transferCancelButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Cancel", gameMain.ScreenWidth / 2 - 160, gameMain.ScreenHeight / 2 + 70, 150, 25);
			transferUpkeepCost = new Label(gameMain.ScreenWidth / 2 - 160, gameMain.ScreenHeight / 2 + 50, System.Drawing.Color.White);
			popTransferSliders = new ScrollBar[4];
			amountPopTransferLabel = new Label[4];
			for (int i = 0; i < 4; i++)
			{
				popTransferSliders[i] = new ScrollBar(gameMain.ScreenWidth / 2 - 160, (gameMain.ScreenHeight / 2 - 120) + (i * 40) + 22, 16, 268, 1, 100, true, true, DrawingManagement.HorizontalSliderBar);
				amountPopTransferLabel[i] = new Label(gameMain.ScreenWidth / 2 - 160, (gameMain.ScreenHeight / 2 - 120) + (i * 40));
			}
			listOfPopTransferScrollBar = new ScrollBar(gameMain.ScreenWidth / 2 + 142, gameMain.ScreenHeight / 2 - 120, 16, 128, 4, 10, false, false, DrawingManagement.VerticalScrollBar);

			systemWindow = new SystemWindow(gameMain.ScreenWidth / 2, gameMain.ScreenHeight / 2, gameMain);
			squadronListWindow = new SquadronListWindow(gameMain.ScreenHeight / 2, gameMain);
			//splitPopulation = new SplitPopulation(gameMain.ScreenWidth / 2, 220, gameMain, SplitFleet);

			GorgonLibrary.Graphics.Image starlaneImage = new GorgonLibrary.Graphics.Image("starlaneImage", 1, 3, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			GorgonLibrary.Graphics.Image.ImageLockBox newImage0 = starlaneImage.GetImageData();
			newImage0.Lock(false);

			newImage0[0, 0] = System.Drawing.Color.FromArgb(200, 150, 150, 150).ToArgb();
			//newImage0[0, 1] = System.Drawing.Color.FromArgb(255, 200, 200, 200).ToArgb();
			newImage0[0, 1] = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
			//newImage0[0, 3] = System.Drawing.Color.FromArgb(255, 200, 200, 200).ToArgb();
			newImage0[0, 2] = System.Drawing.Color.FromArgb(200, 150, 150, 150).ToArgb();

			newImage0.Unlock();

			starlaneSprite = new GorgonLibrary.Graphics.Sprite("starlaneSprite", starlaneImage);
			starlaneSprite.HorizontalWrapMode = GorgonLibrary.Graphics.ImageAddressing.Wrapping;
			starlaneSprite.Axis = new GorgonLibrary.Vector2D(0.5f, 1.0f);

			GorgonLibrary.Graphics.Image movementImage = new GorgonLibrary.Graphics.Image("movementImage", 20, 5, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			GorgonLibrary.Graphics.Image.ImageLockBox newImage = movementImage.GetImageData();
			newImage.Lock(false);

			for (int i = 0; i < 20; i++)
			{
				int value = (int)(255 - (i / 20.0f) * 130);
				int value2 = value - 80;
				int value3 = value2 - 40;
				newImage[i, 0] = System.Drawing.Color.FromArgb(255, value3, value3, value3).ToArgb();
				newImage[i, 1] = System.Drawing.Color.FromArgb(255, value2, value2, value2).ToArgb();
				newImage[i, 2] = System.Drawing.Color.FromArgb(255, value, value, value).ToArgb();
				newImage[i, 3] = System.Drawing.Color.FromArgb(255, value2, value2, value2).ToArgb();
				newImage[i, 4] = System.Drawing.Color.FromArgb(255, value3, value3, value3).ToArgb();
			}

			newImage.Unlock();

			movementPath = new GorgonLibrary.Graphics.Sprite("movementPath", movementImage);
			movementPath.HorizontalWrapMode = GorgonLibrary.Graphics.ImageAddressing.Wrapping;
			movementPath.Axis = new GorgonLibrary.Vector2D(0.5f, 2.5f);

			backgroundStars = new BackgroundStars(gameMain.galaxy.GalaxySize, gameMain.r, 40);
			rotation = 0;

			List<SpriteName> frames = new List<SpriteName>
			{
				SpriteName.FleetDest1, 
				SpriteName.FleetDest2, 
				SpriteName.FleetDest3, 
				SpriteName.FleetDest4
			};
			//fleetDest = new AnimatedImage(0, 0, 32, 32, frames, true, 0.25f);
			/*List<SpriteName> stargateFrames = new List<SpriteName>
			{
				SpriteName.Stargate1,
				SpriteName.Stargate2,
				SpriteName.Stargate3,
				SpriteName.Stargate4
			};
			stargate = new AnimatedImage(0, 0, 32, 32, stargateFrames, true, 0.10f);*/
			LastClickedPosition = new Point(int.MinValue, int.MinValue);
			whichGridCellClicked = new Point(int.MinValue, int.MinValue);
		}

		public void LoadScreen()
		{
			gameMain.taskBar.UpdateDisplays();
		}

		public void CenterScreen()
		{
			if (gameMain.empireManager.CurrentEmpire != null && gameMain.empireManager.CurrentEmpire.LastSelectedSystem != null)
			{
				gameMain.empireManager.CurrentEmpire.SelectedSystem = gameMain.empireManager.CurrentEmpire.LastSelectedSystem;
				camera.CenterCamera(gameMain.empireManager.CurrentEmpire.SelectedSystem.X + 2, gameMain.empireManager.CurrentEmpire.SelectedSystem.Y + 2);
				systemWindow.LoadSystem();
				//LoadSystemInfoIntoUI(gameMain.empireManager.CurrentEmpire.SelectedSystem);
			}
		}

		public void CenterScreenToPoint(Point point)
		{
			camera.CenterCamera(point.X, point.Y);
		}

		//Used when other non-combat screens are open, to fill in the blank areas
		public void DrawGalaxyBackground(DrawingManagement drawingManagement)
		{
			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			/*if (!currentEmpire.ShowBorders)
			{
				GorgonLibrary.Graphics.Sprite nebula = gameMain.galaxy.Nebula;
				nebula.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
				nebula.SetScale(sizes[0], sizes[0]);
				nebula.Opacity = 200;
				nebula.Draw();
			}*/

			/*GorgonLibrary.Graphics.Sprite influenceMap = currentEmpire.InfluenceMap;
			influenceMap.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
			influenceMap.SetScale(sizes[0], sizes[0]);
			influenceMap.Draw();
			foreach (Contact contact in currentEmpire.ContactManager.Contacts)
			{
				if (contact.Contacted)
				{
					influenceMap = contact.EmpireInContact.InfluenceMap;
					influenceMap.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
					influenceMap.SetScale(sizes[0], sizes[0]);
					influenceMap.Draw();
				}
			}*/

			backgroundStars.Draw(camera.CameraX, camera.CameraY, camera.GetViewSize().X, camera.GetViewSize().Y, camera.XOffset, camera.YOffset, camera.Scale, drawingManagement);

			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			SquadronGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
			List<StarSystem> systems = gameMain.galaxy.GetStarsInArea(camera.CameraX - 4, camera.CameraY - 4, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2);
			//GridCell[][] gridCells = gameMain.galaxy.GetGridCells();
			bool displayName = camera.ZoomDistance < 4;

			Point size = camera.GetViewSize();
			int left = camera.CameraX - 3;
			int right = camera.CameraX + size.X + 3;
			int top = camera.CameraY - 3;
			int bottom = camera.CameraY + size.Y + 3;

			/*foreach (Starlane starlane in gameMain.galaxy.Starlanes)
			{
				if (Utility.LineRectangleIntersected(starlane.SystemA.X, starlane.SystemA.Y, starlane.SystemB.X, starlane.SystemB.Y, left, top, right, bottom))
				/*if ((starlane.SystemA.X >= camera.CameraX - 10 && starlane.SystemA.X < camera.CameraX + 30 &&
					starlane.SystemA.Y >= camera.CameraY - 10 && starlane.SystemA.Y < camera.CameraY + 30) ||
					(starlane.SystemB.X >= camera.CameraX - 10 && starlane.SystemB.X < camera.CameraX + 30 &&
					starlane.SystemB.Y >= camera.CameraY - 10 && starlane.SystemB.Y < camera.CameraY + 30))*/
				/*{
					//Draw Starlanes first so Star will overlap them
					int X1 = (int)((((starlane.SystemA.X - camera.CameraX) * 32 + (starlane.SystemA.Type.Width / 2)) - camera.XOffset) * camera.Scale);
					int Y1 = (int)((((starlane.SystemA.Y - camera.CameraY) * 32 + (starlane.SystemA.Type.Height / 2)) - camera.YOffset) * camera.Scale);

					if (starlane.Visible)
					{
						starlaneSprite.SetPosition(X1, Y1);
						starlaneSprite.Width = (float)starlane.Length * camera.Scale;
						//starlaneSprite.Height = 5 * camera.Scale;
						starlaneSprite.Rotation = starlane.Angle;
						starlaneSprite.Draw();
						//Draw the line
						//drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.White);
						//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.DimGray);
					}
					else
					{
						//drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.DimGray);
					}

					/*if (starlane.SystemA.IsThisSystemExploredByEmpire(currentEmpire) && starlane.SystemB.IsThisSystemExploredByEmpire(currentEmpire))
					{
						//Both systems are explored, check to see if owner are same, if so, then color the starlane their color
						if (starlane.SystemA.DominantEmpire == starlane.SystemB.DominantEmpire && starlane.SystemA.DominantEmpire != null)
						{
							drawingManagement.DrawLine(X1, Y1, X2, Y2, starlane.SystemA.DominantEmpire.EmpireColor);
							//target.Line( + incrementsX / 2,  + incrementsY / 2,  empires[starlanes[i].System1.GetSystemOwner()].EmpireColor);
						}
						else //bright line to show explored
						{
							drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.Snow);
							//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.Snow);
						}
					}
					else if (starlane.SystemA.IsThisSystemExploredByEmpire(currentEmpire) || starlane.SystemB.IsThisSystemExploredByEmpire(currentEmpire) == true)
					{
						//Draw the line
						drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.DimGray);
						//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.DimGray);
					}
					else
					{
						drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.LightGray);
					}*/
				//}
			//}

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
				if (system.Type.Shader != null)
				{
					if (currentEmpire.ShowBorders)
					{
						if (system.IsThisSystemExploredByEmpire(currentEmpire))
						{
							system.Type.Shader.Parameters["StarColor"].SetValue(system.DominantEmpire.ConvertedColor);
						}
						else
						{
							system.Type.Shader.Parameters["StarColor"].SetValue(new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
						}
					}
					else
					{
						system.Type.Shader.Parameters["StarColor"].SetValue(system.Type.ShaderValue);
					}
				}
				system.Type.Sprite.SetPosition(((((system.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), ((((system.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale));
				system.Type.Sprite.SetScale(camera.Scale < 0.25f ? 0.25f : camera.Scale, camera.Scale < 0.25f ? 0.25f : camera.Scale);
				system.Type.Sprite.Draw();
				GorgonLibrary.Gorgon.CurrentShader = null;

				if (displayName && (gameMain.empireManager.CurrentEmpire.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire)))
				{
					int x = (int)((((system.X - camera.CameraX) * 32) + (system.Type.Width / 2) - camera.XOffset) * camera.Scale);
					x -= (int)(system.StarName.GetWidth() / 2);
					int y = (int)((((system.Y - camera.CameraY) * 32) + system.Type.Height - camera.YOffset) * camera.Scale);
					system.StarName.MoveTo(x, y);
					if (system.DominantEmpire != null)
					{
						float percentage = 1.0f;
						oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						starName.Width = (int)system.StarName.GetWidth();
						starName.Height = (int)system.StarName.GetHeight();
						GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
						system.StarName.MoveTo(0, 0);
						system.StarName.Draw();
						GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
						//GorgonLibrary.Gorgon.CurrentShader = gameMain.NameShader;
						foreach (Empire empire in system.EmpiresWithSectorsInThisSystem)
						{
							/*gameMain.NameShader.Parameters["EmpireColor"].SetValue(empire.ConvertedColor);
							gameMain.NameShader.Parameters["startPos"].SetValue(percentage);
							gameMain.NameShader.Parameters["endPos"].SetValue(percentage + system.OwnerPercentage[empire]);*/
							starName.Blit(x, y, starName.Width * percentage, starName.Height, empire.EmpireColor, GorgonLibrary.Graphics.BlitterSizeMode.Crop);
							percentage -= system.OwnerPercentage[empire];
						}
						//GorgonLibrary.Gorgon.CurrentShader = null;
					}
					else
					{
						system.StarName.Draw();
					}
				}
			}

			foreach (Squadron fleet in gameMain.empireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2))
			{
				bool visible = false;
				if (fleet.Empire == gameMain.empireManager.CurrentEmpire)
				{
					visible = true;
				}
				/*if (gridCells[fleet.GalaxyX][fleet.GalaxyY].dominantEmpire == currentEmpire || gridCells[fleet.GalaxyX][fleet.GalaxyY].secondaryEmpire == currentEmpire)
				{
					visible = true;
				}*/
				if (visible)
				{
					/*if (fleet.HasTransports)
					{
						drawingManagement.DrawSprite(SpriteName.Transport, (int)((((fleet.GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((fleet.GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[0], sizes[0], fleet.Empire.EmpireColor);
					}
					else
					{*/
					Sprite icon = fleet.Empire.EmpireRace.GetFleetIcon();
					icon.Color = fleet.Empire.EmpireColor;
					icon.SetPosition(((fleet.FleetLocation.X - camera.XOffset - (camera.CameraX * 32)) * camera.Scale), ((fleet.FleetLocation.Y - camera.YOffset - (camera.CameraY * 32)) * camera.Scale));
					float scale = camera.Scale;
					icon.SetScale(scale, scale);
					icon.Draw();
						//drawingManagement.DrawSprite(SpriteName.Fleet, (int)((((fleet.GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((fleet.GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[0], sizes[0], fleet.Empire.EmpireColor);
					//}
				}
			}
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			/*if (!currentEmpire.ShowBorders)
			{
				GorgonLibrary.Graphics.Sprite nebula = gameMain.galaxy.Nebula;
				nebula.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
				nebula.SetScale(sizes[0], sizes[0]);
				nebula.Opacity = 200;
				nebula.Draw();
			}*/
			/*GorgonLibrary.Graphics.Sprite influenceMap = currentEmpire.InfluenceMap;
			influenceMap.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
			influenceMap.SetScale(sizes[0], sizes[0]);
			influenceMap.Draw();
			foreach (Contact contact in currentEmpire.ContactManager.Contacts)
			{
				if (contact.Contacted)
				{
					influenceMap = contact.EmpireInContact.InfluenceMap;
					influenceMap.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
					influenceMap.SetScale(sizes[0], sizes[0]);
					influenceMap.Draw();
				}
			}*/

			backgroundStars.Draw(camera.CameraX, camera.CameraY, camera.GetViewSize().X, camera.GetViewSize().Y, camera.XOffset, camera.YOffset, camera.Scale, drawingManagement);

			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			SquadronGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
			List<StarSystem> systems = gameMain.galaxy.GetStarsInArea(camera.CameraX - 4, camera.CameraY - 4, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2);
			//GridCell[][] gridCells = gameMain.galaxy.GetGridCells();
			bool displayName = camera.ZoomDistance < 4;

			Point size = camera.GetViewSize();
			int left = camera.CameraX - 3;
			int right = camera.CameraX + size.X + 3;
			int top = camera.CameraY - 3;
			int bottom = camera.CameraY + size.Y + 3;

			/*foreach (Starlane starlane in gameMain.galaxy.Starlanes)
			{
				if (Utility.LineRectangleIntersected(starlane.SystemA.X, starlane.SystemA.Y, starlane.SystemB.X, starlane.SystemB.Y, left, top, right, bottom))
				{
					//Draw Starlanes first so Star will overlap them
					int X1 = (int)((((starlane.SystemA.X - camera.CameraX) * 32 + (starlane.SystemA.Type.Width / 2)) - camera.XOffset) * camera.Scale);
					int Y1 = (int)((((starlane.SystemA.Y - camera.CameraY) * 32 + (starlane.SystemA.Type.Height / 2)) - camera.YOffset) * camera.Scale);
					/*int X2 = (int)(((starlane.SystemB.X - starlane.SystemA.X) * 32) * camera.Scale);
					int Y2 = (int)(((starlane.SystemB.Y - starlane.SystemA.Y) * 32) * camera.Scale);*/

					/*if (starlane.Visible && (starlane.SystemA.IsThisSystemExploredByEmpire(currentEmpire) || starlane.SystemB.IsThisSystemExploredByEmpire(currentEmpire)))
					{
						starlaneSprite.SetPosition(X1, Y1);
						starlaneSprite.Width = (float)starlane.Length * camera.Scale;
						//starlaneSprite.Height = 5 * camera.Scale;
						starlaneSprite.Rotation = starlane.Angle;
						starlaneSprite.Draw();
						//Draw the line
						//drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.White);
						//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.DimGray);
					}
					else
					{
						//drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.DimGray);
					}

					/*if (starlane.SystemA.IsThisSystemExploredByEmpire(currentEmpire) && starlane.SystemB.IsThisSystemExploredByEmpire(currentEmpire))
					{
						//Both systems are explored, check to see if owner are same, if so, then color the starlane their color
						if (starlane.SystemA.DominantEmpire == starlane.SystemB.DominantEmpire && starlane.SystemA.DominantEmpire != null)
						{
							drawingManagement.DrawLine(X1, Y1, X2, Y2, starlane.SystemA.DominantEmpire.EmpireColor);
							//target.Line( + incrementsX / 2,  + incrementsY / 2,  empires[starlanes[i].System1.GetSystemOwner()].EmpireColor);
						}
						else //bright line to show explored
						{
							drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.Snow);
							//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.Snow);
						}
					}
					else if (starlane.SystemA.IsThisSystemExploredByEmpire(currentEmpire) || starlane.SystemB.IsThisSystemExploredByEmpire(currentEmpire) == true)
					{
						//Draw the line
						drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.DimGray);
						//target.Line(X1 + incrementsX / 2, Y1 + incrementsY / 2, X2, Y2, Color.DimGray);
					}
					else
					{
						drawingManagement.DrawLine(X1, Y1, X2, Y2, System.Drawing.Color.LightGray);
					}*/
				//}
			//}

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
				if (system.Type.Shader != null)
				{
					if (currentEmpire.ShowBorders)
					{
						if (system.IsThisSystemExploredByEmpire(currentEmpire))
						{
							system.Type.Shader.Parameters["StarColor"].SetValue(system.DominantEmpire == null ? new float[] { 0.5f, 0.5f, 0.5f, 0.5f } : system.DominantEmpire.ConvertedColor);
						}
						else
						{
							system.Type.Shader.Parameters["StarColor"].SetValue(new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
						}
					}
					else
					{
						system.Type.Shader.Parameters["StarColor"].SetValue(system.Type.ShaderValue);
					}
				}
				system.Type.Sprite.SetPosition((((((system.X - camera.CameraX) * 32) - camera.XOffset) + system.Type.Width / 2.0f) * camera.Scale), (((((system.Y - camera.CameraY) * 32) - camera.YOffset) + system.Type.Height / 2.0f) * camera.Scale));
				float scale = camera.Scale;
				if (scale < 0.25f)
				{
					scale = 0.25f;
				}
				system.Type.Sprite.SetScale(scale, scale);
				//system.Type.Sprite.SetScale(sizes[system.Size - 1] / 64.0f, sizes[system.Size - 1] / 64.0f);
				system.Type.Sprite.Draw();
				GorgonLibrary.Gorgon.CurrentShader = null;

				/*if (system.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire) && system.GetStargates().Count > 0)
				{
					GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
					if (system.GetStargates().ContainsKey(gameMain.empireManager.CurrentEmpire))
					{
						gameMain.ShipShader.Parameters["EmpireColor"].SetValue(gameMain.empireManager.CurrentEmpire.ConvertedColor);
					}
					else
					{
						foreach (KeyValuePair<Empire, Stargate> keyValuePair in system.GetStargates())
						{
							gameMain.ShipShader.Parameters["EmpireColor"].SetValue(keyValuePair.Key.ConvertedColor);
							break;
						}
					}
					stargate.MoveTo((int)((((system.X + system.Size - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((system.Y + system.Size - 1 - camera.CameraY) * 32) - camera.YOffset) * camera.Scale));
					stargate.SetScale(sizes[0], sizes[0]);
					stargate.Draw(drawingManagement);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}*/
				if (currentEmpire != null && system == currentEmpire.SelectedSystem)
				{
					drawingManagement.GetSprite(SpriteName.SelectedStar).Rotation = rotation;
					int max = Math.Max(system.Type.Width, system.Type.Height) + 32;
					drawingManagement.DrawSprite(SpriteName.SelectedStar, (int)((((system.X - camera.CameraX) * 32) + (system.Type.Width / 2) - camera.XOffset) * camera.Scale), (int)((((system.Y - camera.CameraY) * 32) + (system.Type.Height / 2) - camera.YOffset) * camera.Scale), 255, max * scale, max * scale, System.Drawing.Color.White);
				}
				if (displayName && (gameMain.empireManager.CurrentEmpire.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire)))
				{
					int x = (int)((((system.X - camera.CameraX) * 32) + (system.Type.Width / 2) - camera.XOffset) * camera.Scale);
					x -= (int)(system.StarName.GetWidth() / 2);
					int y = (int)((((system.Y - camera.CameraY) * 32) + system.Type.Height - camera.YOffset) * camera.Scale);
					system.StarName.MoveTo(x, y);
					if (system.DominantEmpire != null)
					{
						float percentage = 1.0f;
						oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						starName.Width = (int)system.StarName.GetWidth();
						starName.Height = (int)system.StarName.GetHeight();
                        starName.Clear(System.Drawing.Color.Transparent);
						GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
						system.StarName.MoveTo(0, 0);
						system.StarName.Draw();
						GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
						//GorgonLibrary.Gorgon.CurrentShader = gameMain.NameShader;
						foreach (Empire empire in system.EmpiresWithSectorsInThisSystem)
						{
							/*gameMain.NameShader.Parameters["EmpireColor"].SetValue(empire.ConvertedColor);
							gameMain.NameShader.Parameters["startPos"].SetValue(percentage);
							gameMain.NameShader.Parameters["endPos"].SetValue(percentage + system.OwnerPercentage[empire]);*/
							starName.Blit(x, y, starName.Width * percentage, starName.Height, empire.EmpireColor, GorgonLibrary.Graphics.BlitterSizeMode.Crop);
							percentage -= system.OwnerPercentage[empire];
						}
						//GorgonLibrary.Gorgon.CurrentShader = null;
					}
					else
					{
						system.StarName.Draw();
					}
					/*if (system.SystemFleets.Count > 0)
					{
						drawingManagement.DrawSprite(SpriteName.SystemShips, (int)((((system.SystemFleets[0].GalaxyX - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((system.SystemFleets[0].GalaxyY - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[0], sizes[0], system.SystemFleets[0].Empire.EmpireColor);
					}*/
				}
			}

			foreach (Squadron fleet in gameMain.empireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, camera.GetViewSize().X + 2, camera.GetViewSize().Y + 2))
			{
				bool visible = false;
				if (fleet.Empire == gameMain.empireManager.CurrentEmpire)
				{
					visible = true;
				}
				/*if (gridCells[fleet.GalaxyX][fleet.GalaxyY].dominantEmpire == currentEmpire || gridCells[fleet.GalaxyX][fleet.GalaxyY].secondaryEmpire == currentEmpire)
				{
					visible = true;
				}*/
				if (visible)
				{
					Sprite icon = fleet.Empire.EmpireRace.GetFleetIcon();
					icon.Color = fleet.Empire.EmpireColor;
					icon.SetPosition(((fleet.FleetLocation.X - camera.XOffset - (camera.CameraX * 32)) * camera.Scale), ((fleet.FleetLocation.Y - camera.YOffset - (camera.CameraY * 32)) * camera.Scale));
					float scale = camera.Scale;
					icon.SetScale(scale, scale);
					icon.Draw();
				}
			}

			if (selectedFleetGroup != null)
			{
				drawingManagement.GetSprite(SpriteName.SelectedStar).Rotation = rotation;
				drawingManagement.DrawSprite(SpriteName.SelectedStar, (int)((selectedFleetGroup.Squadrons[0].FleetLocation.X - camera.XOffset - (camera.CameraX * 32)) * camera.Scale), (int)((selectedFleetGroup.Squadrons[0].FleetLocation.Y - camera.YOffset - (camera.CameraY * 32)) * camera.Scale), 255, 48 * camera.Scale, 48 * camera.Scale, System.Drawing.Color.White);
				movementPath.SetScale(camera.Scale, camera.Scale);
				if (selectedFleetGroup.TravelNodes != null)
				{
					movementPath.Color = System.Drawing.Color.Green;

					for (int i = 1; i < selectedFleetGroup.TravelNodes.Count; i++)
					{
						KeyValuePair<StarSystem, Starlane> starlane = selectedFleetGroup.TravelNodes[i];

						int X1;
						int Y1;

						if (i == 1)
						{
							//X1 = (int)(((selectedFleetGroup.SquadronToSplit.FleetLocation.X - camera.CameraX * 32) - camera.XOffset) * camera.Scale);
							//Y1 = (int)(((selectedFleetGroup.SquadronToSplit.FleetLocation.Y - camera.CameraY * 32) - camera.YOffset) * camera.Scale);

							X1 = (int)((((starlane.Key.X - camera.CameraX) * 32 + (starlane.Key.Type.Width / 2)) - camera.XOffset) * camera.Scale);
							Y1 = (int)((((starlane.Key.Y - camera.CameraY) * 32 + (starlane.Key.Type.Height / 2)) - camera.YOffset) * camera.Scale);

							movementPath.SetPosition(X1, Y1);
							movementPath.Width = selectedFleetGroup.Length;
							movementPath.Rotation = selectedFleetGroup.Angle;
							movementPath.Draw();
						}
						else
						{
							X1 = (int)((((starlane.Key.X - camera.CameraX) * 32 + (starlane.Key.Type.Width / 2)) - camera.XOffset) * camera.Scale);
							Y1 = (int)((((starlane.Key.Y - camera.CameraY) * 32 + (starlane.Key.Type.Height / 2)) - camera.YOffset) * camera.Scale);

							movementPath.SetPosition(X1, Y1);
							movementPath.Width = (float)starlane.Value.Length;
							if (starlane.Value.SystemA == starlane.Key)
							{
								movementPath.Rotation = starlane.Value.Angle;
							}
							else
							{
								movementPath.Rotation = starlane.Value.Angle + 180;
							}
							movementPath.Draw();
						}
					}
				}
				if (selectedFleetGroup.TentativeNodes != null)
				{
					movementPath.Color = System.Drawing.Color.LightGreen;

					for (int i = 1; i < selectedFleetGroup.TentativeNodes.Count; i++)
					{
						KeyValuePair<StarSystem, Starlane> starlane = selectedFleetGroup.TentativeNodes[i];

						int X1;
						int Y1;

						if (i == 1)
						{
							//X1 = (int)(((selectedFleetGroup.SquadronToSplit.FleetLocation.X - camera.CameraX * 32) - camera.XOffset) * camera.Scale);
							//Y1 = (int)(((selectedFleetGroup.SquadronToSplit.FleetLocation.Y - camera.CameraY * 32) - camera.YOffset) * camera.Scale);

							X1 = (int)((((starlane.Key.X - camera.CameraX) * 32 + (starlane.Key.Type.Width / 2)) - camera.XOffset) * camera.Scale);
							Y1 = (int)((((starlane.Key.Y - camera.CameraY) * 32 + (starlane.Key.Type.Height / 2)) - camera.YOffset) * camera.Scale);

							movementPath.SetPosition(X1, Y1);
							movementPath.Width = selectedFleetGroup.TentativeLength;
							movementPath.Rotation = selectedFleetGroup.TentativeAngle;
							movementPath.Draw();
						}
						else
						{
							X1 = (int)((((starlane.Key.X - camera.CameraX) * 32 + (starlane.Key.Type.Width / 2)) - camera.XOffset) * camera.Scale);
							Y1 = (int)((((starlane.Key.Y - camera.CameraY) * 32 + (starlane.Key.Type.Height / 2)) - camera.YOffset) * camera.Scale);

							movementPath.SetPosition(X1, Y1);
							movementPath.Width = (float)starlane.Value.Length;
							if (starlane.Value.SystemA == starlane.Key)
							{
								movementPath.Rotation = starlane.Value.Angle;
							}
							else
							{
								movementPath.Rotation = starlane.Value.Angle + 180;
							}
							movementPath.Draw();
						}
					}

					/*Point lastNode = selectedFleetGroup.SquadronToSplit.TentativeNodes[selectedFleetGroup.SquadronToSplit.TentativeNodes.Count - 1];
					//fleetDest.SetRotation(rotation);
					fleetDest.MoveTo((int)((((lastNode.X - camera.CameraX + 0.5f) * 32) - camera.XOffset) * camera.Scale), (int)(((((lastNode.Y + 0.5f) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale));
					//fleetDest.MoveTo(0, 0);
					fleetDest.SetScale(sizes[0], sizes[0]);
					fleetDest.Draw(System.Drawing.Color.LightGreen, 255, drawingManagement);
					//drawingManagement.DrawSprite(SpriteName.FleetDest1, 16, 16);*/
					//drawingManagement.DrawText("Arial", "ETA: " + (selectedFleetGroup.SquadronToSplit.TentativeETA >= 0 ? (selectedFleetGroup.SquadronToSplit.TentativeETA + " Turns") : "Infinite"), (int)((((lastNode.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)(((((lastNode.Y + 1) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), System.Drawing.Color.White);
				}
				squadronListWindow.DrawWindow(drawingManagement);
				/*if (showingSplitWindow)
				{
					splitPopulation.DrawWindow(drawingManagement);
				}*/
			}
			if (selectedSystem != null)
			{
				systemWindow.DrawWindow(drawingManagement);
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			rotation -= frameDeltaTime * 100;
			backgroundStars.Update(frameDeltaTime);
			//fleetDest.Update(frameDeltaTime);

			movementPath.ImageOffset -= new GorgonLibrary.Vector2D(frameDeltaTime * -50.0f, 0);
			//stargate.Update(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			UpdateBackground(frameDeltaTime);

			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			if (currentEmpire.SelectedSystem != null)
			{
				if (systemWindow.MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					return;
				}
			}
			if (currentEmpire.SelectedFleetGroup != null)
			{
				/*if (showingSplitWindow)
				{
					if (splitPopulation.MouseHover(mouseX, mouseY, frameDeltaTime))
					{
						return;
					}
				}*/
				if (squadronListWindow.MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					return;
				}
				if (currentEmpire.SelectedFleetGroup.Squadrons[currentEmpire.FleetSelected].Empire == currentEmpire)
				{
					int mouseOverX = (int)(((mouseX / camera.Scale) + camera.XOffset) / 32) + camera.CameraX;
					int mouseOverY = (int)(((mouseY / camera.Scale) + camera.YOffset) / 32) + camera.CameraY;

					StarSystem destination = gameMain.galaxy.GetStarAtPoint(new Point(mouseOverX, mouseOverY));

					if (destination != null)
					{
						currentEmpire.SelectedFleetGroup.SetTentativePath(destination, gameMain.galaxy, gameMain.Input.Keyboard.KeyStates[GorgonLibrary.InputDevices.KeyboardKeys.ControlKey] == GorgonLibrary.InputDevices.KeyState.Down, currentEmpire);
					}
					else
					{
						currentEmpire.SelectedFleetGroup.SetTentativePath(null, null, false, currentEmpire);
					}
				}
			}
			camera.HandleUpdate(mouseX, mouseY, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
				if (currentEmpire.SelectedSystem != null)
				{
					if (systemWindow.MouseDown(x, y))
					{
						return;
					}
				}
				else if (gameMain.empireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					/*if (showingSplitWindow)
					{
						if (splitPopulation.MouseDown(x, y))
						{
							return;
						}
					}*/
					if (squadronListWindow.MouseDown(x, y))
					{
						return;
					}
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			if (whichButton == 1)
			{
				if (currentEmpire.SelectedSystem != null)
				{
					if (systemWindow.MouseUp(x, y))
					{
						return;
					}
				}
				if (gameMain.empireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					/*if (showingSplitWindow)
					{
						if (splitPopulation.MouseUp(x, y))
						{
							return;
						}
					}*/
					if (squadronListWindow.MouseUp(x, y))
					{
						return;
					}
				}
				if (pressedInWindow)
				{
					pressedInWindow = false;
					return;
				}

				whichGridCellClicked.X = (int)(((x / camera.Scale) + camera.XOffset) / 32) + camera.CameraX;
				whichGridCellClicked.Y = (int)(((y / camera.Scale) + camera.YOffset) / 32) + camera.CameraY;

				StarSystem selectedSystem = gameMain.galaxy.GetStarAtPoint(whichGridCellClicked);
				if (selectedSystem != null && selectedSystem == gameMain.empireManager.CurrentEmpire.SelectedSystem)
				{
					return;
				}
				gameMain.empireManager.CurrentEmpire.SelectedSystem = selectedSystem;

				if (selectedSystem != null)
				{
					gameMain.empireManager.CurrentEmpire.LastSelectedSystem = selectedSystem;
					gameMain.empireManager.CurrentEmpire.SelectedFleetGroup = null;
					systemWindow.LoadSystem();
					return;
				}

				//See if there is an adjacent system
				selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X - 1, whichGridCellClicked.Y));
				if (selectedSystem == null)
				{
					selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X + 1, whichGridCellClicked.Y));
					if (selectedSystem == null)
					{
						selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X, whichGridCellClicked.Y - 1));
						if (selectedSystem == null)
						{
							selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X, whichGridCellClicked.Y + 1));
						}
					}
				}
				LastClickedPosition.X = (int)(((x / camera.Scale) + camera.XOffset) + (camera.CameraX * 32));
				LastClickedPosition.Y = (int)(((y / camera.Scale) + camera.YOffset) + (camera.CameraY * 32));
				SquadronGroup selectedFleetGroup = gameMain.empireManager.GetSquadronsAtPoint(selectedSystem, LastClickedPosition.X, LastClickedPosition.Y);
				gameMain.empireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;

				if (selectedFleetGroup != null)
				{
					squadronListWindow.LoadFleetGroup();
					return;
				}
				camera.CenterCamera(whichGridCellClicked.X, whichGridCellClicked.Y);
			}
			else if (whichButton == 2)
			{
				if (gameMain.empireManager.CurrentEmpire.SelectedFleetGroup != null && gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.Empire == gameMain.empireManager.CurrentEmpire)
				{
					gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.ConfirmPath();
					//gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(gameMain.empireManager.CurrentEmpire, new Dictionary<Race, int>());
					//See if there is an adjacent system
					StarSystem selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X - 1, whichGridCellClicked.Y));
					if (selectedSystem == null)
					{
						selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X + 1, whichGridCellClicked.Y));
						if (selectedSystem == null)
						{
							selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X, whichGridCellClicked.Y - 1));
							if (selectedSystem == null)
							{
								selectedSystem = gameMain.galaxy.GetStarAtPoint(new Point(whichGridCellClicked.X, whichGridCellClicked.Y + 1));
							}
						}
					}
					SquadronGroup selectedFleetGroup = gameMain.empireManager.GetSquadronsAtPoint(selectedSystem, LastClickedPosition.X, LastClickedPosition.Y);
					gameMain.empireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;
					if (selectedFleetGroup != null)
					{
						squadronListWindow.LoadFleetGroup();
					}
					//showingSplitWindow = true;
					//splitPopulation.LoadSplitPopulation(gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.SquadronToSplit, gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.Fleets[gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.FleetIndex]);
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
			camera.MouseWheel(direction, x, y);
		}

		public void Resize()
		{
			camera.ResizeScreen(gameMain.ScreenWidth, gameMain.ScreenHeight);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.InGameMenu);
			}
			if (e.Key == KeyboardKeys.Space)
			{
				gameMain.ToggleSitRep();
			}
			if (e.Key == KeyboardKeys.B)
			{
				gameMain.empireManager.CurrentEmpire.ToggleBorder();
			}
		}

		/*private void SplitFleet(Dictionary<Race, int> races)
		{
			gameMain.empireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(gameMain.empireManager.CurrentEmpire, races);
			fleetWindow.LoadFleetGroup();
			showingSplitWindow = false;
		}*/
	}
}
