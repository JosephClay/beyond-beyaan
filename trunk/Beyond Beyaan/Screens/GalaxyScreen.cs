using System;
using System.Collections.Generic;
using System.Drawing;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class GalaxyScreen : ScreenInterface
	{
		private GameMain _gameMain;
		private Camera camera;

		private Button[] systemButtons;
		private Button[] fleetButtons;
		private ScrollBar systemScrollBar;
		private ScrollBar[] planetScrollBars;
		private Button[] planetFieldLocks;
		private Button prevShip;
		private Button nextShip;
		private ScrollBar fleetScrollBar;
		private ScrollBar shipSelectorScrollBar;
		private ScrollBar[] shipScrollBars;
		private RenderTarget oldTarget;
		private RenderImage starName;
		private RenderImage backBuffer;
		private SystemView systemView;

		private bool pressedInWindow;

		//private int maxVisible;
		private BBSprite pathSprite;
		private BBSprite fuelCircle;
		private BBSprite[] selectionSprites;
		private bool showingFuelRange;
	

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			pathSprite = SpriteManager.GetSprite("Path", _gameMain.Random);
			fuelCircle = SpriteManager.GetSprite("FuelCircle", _gameMain.Random);
			selectionSprites = new BBSprite[4];
			selectionSprites[0] = SpriteManager.GetSprite("SelectionTL", _gameMain.Random);
			selectionSprites[1] = SpriteManager.GetSprite("SelectionTR", _gameMain.Random);
			selectionSprites[2] = SpriteManager.GetSprite("SelectionBL", _gameMain.Random);
			selectionSprites[3] = SpriteManager.GetSprite("SelectionBR", _gameMain.Random);
			showingFuelRange = false;

			camera = new Camera(_gameMain.Galaxy.GalaxySize * 32, _gameMain.Galaxy.GalaxySize * 32, _gameMain.ScreenWidth, _gameMain.ScreenHeight);

			systemButtons = new Button[6];
			for (int i = 0; i < systemButtons.Length; i++)
			{
				systemButtons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, _gameMain.ScreenWidth - 490, 8 + (47 * i), 215, 47);
			}
			systemScrollBar = new ScrollBar(_gameMain.ScreenWidth - 274, 8, 20, 242, 6, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			fleetScrollBar = new ScrollBar(_gameMain.ScreenWidth - 24, 30, 16, 48, 4, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			shipSelectorScrollBar = new ScrollBar(_gameMain.ScreenWidth - 20, 130, 16, 286, 8, 10, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);

			planetScrollBars = new ScrollBar[5];
			planetFieldLocks = new Button[5];

			for (int i = 0; i < 5; i++)
			{
				planetScrollBars[i] = new ScrollBar(_gameMain.ScreenWidth - 245, 30 + (i * 40), 16, 188, 1, 101, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton,
					SpriteName.SliderHorizontalForegroundButton, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				planetFieldLocks[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, _gameMain.ScreenWidth - 21, 30 + (i * 40), 16, 16);
			}

			prevShip = new Button(SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton, string.Empty, _gameMain.ScreenWidth - 245, 275, 16, 16);
			nextShip = new Button(SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, string.Empty, _gameMain.ScreenWidth - 25, 275, 16, 16);
			pressedInWindow = false;

			starName = new RenderImage("starNameRendered", 1, 1, ImageBufferFormats.BufferRGB888A8);
			starName.BlendingMode = BlendingModes.Modulated;

			backBuffer = new RenderImage("galaxyBackBuffer", _gameMain.ScreenWidth, _gameMain.ScreenHeight, ImageBufferFormats.BufferRGB888A8);
			backBuffer.BlendingMode = BlendingModes.Modulated;

			systemView = new SystemView();
			if (!systemView.Initialize(_gameMain, _gameMain.Random, out reason))
			{
				return false;
			}
			reason = null;
			return true;
		}

		public void CenterScreen()
		{
			if (_gameMain.EmpireManager.CurrentEmpire != null && _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem != null)
			{
				_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem;
				camera.CenterCamera(_gameMain.EmpireManager.CurrentEmpire.SelectedSystem.X, _gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Y, camera.ZoomDistance);
				systemView.LoadSystem();
			}
		}

		public void CenterScreenToPoint(Point point)
		{
			camera.CenterCamera(point.X * 32, point.Y * 32, camera.ZoomDistance);
		}

		//Used when other non-combat screens are open, to fill in the blank areas
		public void DrawGalaxy(DrawingManagement drawingManagement)
		{
			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			//GorgonLibrary.Graphics.Sprite nebula = _gameMain.Galaxy.Nebula;
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
			//nebula.SetPosition(0 - (camera.CameraX * sizes[0] + (camera.XOffset * camera.Scale)), 0 - (camera.CameraY * sizes[0] + (camera.YOffset * camera.Scale)));
			//nebula.SetScale(sizes[0], sizes[0]);
			//nebula.Opacity = 200;
			//nebula.Draw();

			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();
			bool displayName = camera.ZoomDistance > 0.8f;

			if (showingFuelRange)
			{
				float scale = (currentEmpire.TechnologyManager.FuelRange / 3.0f) * camera.ZoomDistance;
				float extendedScale = ((currentEmpire.TechnologyManager.FuelRange + 3) / 3.0f) * camera.ZoomDistance;
				backBuffer.Clear(Color.Black);
				oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
				GorgonLibrary.Gorgon.CurrentRenderTarget = backBuffer;
				List<StarSystem> ownedSystems = new List<StarSystem>();
				foreach (StarSystem system in systems)
				{
					if (system.Planets[0].Owner == currentEmpire)
					{
						fuelCircle.Draw((system.X - camera.CameraX) * camera.ZoomDistance, (system.Y - camera.CameraY) * camera.ZoomDistance, extendedScale, extendedScale, currentEmpire.EmpireColor);
						ownedSystems.Add(system);
					}
				}
				GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
				backBuffer.Blit(0, 0, _gameMain.ScreenWidth, _gameMain.ScreenHeight, Color.FromArgb(75, Color.White), BlitterSizeMode.Crop);
				backBuffer.Clear(Color.Black);
				GorgonLibrary.Gorgon.CurrentRenderTarget = backBuffer;
				foreach (StarSystem system in ownedSystems)
				{
					fuelCircle.Draw((system.X - camera.CameraX) * camera.ZoomDistance, (system.Y - camera.CameraY) * camera.ZoomDistance, scale, scale, currentEmpire.EmpireColor);
				}
				GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
				backBuffer.Blit(0, 0, _gameMain.ScreenWidth, _gameMain.ScreenHeight, Color.FromArgb(75, Color.White), BlitterSizeMode.Crop);
			}

			if (selectedSystem != null)
			{
				if (selectedSystem.Planets[0].TransferSystem.Key.StarSystem != selectedSystem)
				{
					pathSprite.Draw((selectedSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedSystem.Planets[0].TransferSystem.Key.Length / pathSprite.Width), camera.ZoomDistance, Color.Green, selectedSystem.Planets[0].TransferSystem.Key.Angle);
				}
				if (systemView.IsTransferring && systemView.TransferSystem.StarSystem != selectedSystem)
				{
					pathSprite.Draw((selectedSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (systemView.TransferSystem.Length / pathSprite.Width), camera.ZoomDistance, systemView.TransferSystem.IsValid ? Color.LightGreen : Color.Red, systemView.TransferSystem.Angle);
				}
			}

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				//drawingManagement.DrawSprite(SpriteName.Star, (int)(((system.X * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((system.Y * 32) - camera.CameraY) * camera.ZoomDistance), 255, system.Size * 32 * camera.ZoomDistance, system.Size * 32 * camera.ZoomDistance, System.Drawing.Color.White);
				system.Sprite.Draw((int)((system.X - camera.CameraX) * camera.ZoomDistance), (int)((system.Y - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance);
				GorgonLibrary.Gorgon.CurrentShader = null;

				if (system == currentEmpire.SelectedSystem)
				{
					selectionSprites[0].Draw(((system.X - 16) - camera.CameraX) * camera.ZoomDistance, ((system.Y - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
					selectionSprites[1].Draw(((system.X) - camera.CameraX) * camera.ZoomDistance, ((system.Y - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
					selectionSprites[2].Draw(((system.X - 16) - camera.CameraX) * camera.ZoomDistance, ((system.Y) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
					selectionSprites[3].Draw(((system.X) - camera.CameraX) * camera.ZoomDistance, ((system.Y) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
				}

				if (displayName && (_gameMain.EmpireManager.CurrentEmpire.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(_gameMain.EmpireManager.CurrentEmpire)))
				{
					float x = (system.X - camera.CameraX) * camera.ZoomDistance;
					x -= (system.StarName.GetWidth() / 2);
					float y = ((system.Y + (system.Size * 16)) - camera.CameraY) * camera.ZoomDistance;
					system.StarName.Move((int)x, (int)y);
					if (system.DominantEmpire != null)
					{
						// TODO: Optimize this by moving the text sprite and color shader to StarSystem, where it's updated when ownership changes
						float percentage = 1.0f;
						oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						starName.Width = (int)system.StarName.GetWidth();
						starName.Height = (int)system.StarName.GetHeight();
						GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
						system.StarName.Move(0, 0);
						system.StarName.Draw();
						GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
						//GorgonLibrary.Gorgon.CurrentShader = _gameMain.NameShader;
						foreach (Empire empire in system.EmpiresWithPlanetsInThisSystem)
						{
							/*_gameMain.NameShader.Parameters["EmpireColor"].SetValue(empire.ConvertedColor);
							_gameMain.NameShader.Parameters["startPos"].SetValue(percentage);
							_gameMain.NameShader.Parameters["endPos"].SetValue(percentage + system.OwnerPercentage[empire]);*/
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

			if (selectedFleetGroup != null && selectedFleetGroup.SelectedFleet.TravelNodes != null)
			{
				if (selectedFleetGroup.SelectedFleet.Empire == currentEmpire)
				{
					for (int i = 0; i < selectedFleetGroup.SelectedFleet.TravelNodes.Count; i++)
					{
						if (i == 0)
						{
							var travelNode = selectedFleetGroup.FleetToSplit.TravelNodes[0];
							if (selectedFleetGroup.SelectedFleet.AdjacentSystem != null)
							{
								//Haven't left yet, so calculate custom path from left side of star
								float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX - 32);
								float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
								float length = (float)Math.Sqrt((x * x) + (y * y));
								float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
								pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - camera.CameraX - 32) * camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (length / pathSprite.Width), camera.ZoomDistance, Color.Green, angle);
							}
							else
							{
								pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - camera.CameraX) * camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (travelNode.Length / pathSprite.Width), camera.ZoomDistance, Color.Green, travelNode.Angle);
							}
						}
						else
						{
							pathSprite.Draw((selectedFleetGroup.SelectedFleet.TravelNodes[i - 1].StarSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.TravelNodes[i - 1].StarSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedFleetGroup.SelectedFleet.TravelNodes[i].Length / pathSprite.Width), camera.ZoomDistance, Color.Green, selectedFleetGroup.SelectedFleet.TravelNodes[i].Angle);
						}
					}
				}
				else
				{
					pathSprite.Draw((selectedFleetGroup.SelectedFleet.GalaxyX - camera.CameraX) * camera.ZoomDistance, (selectedFleetGroup.SelectedFleet.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedFleetGroup.SelectedFleet.TravelNodes[0].Length / pathSprite.Width), camera.ZoomDistance, Color.Red, selectedFleetGroup.SelectedFleet.TravelNodes[0].Angle);
				}
			}

			if (selectedFleetGroup != null && selectedFleetGroup.FleetToSplit.TentativeNodes != null)
			{
				for (int i = 0; i < selectedFleetGroup.FleetToSplit.TentativeNodes.Count; i++)
				{
					if (i == 0)
					{
						var travelNode = selectedFleetGroup.FleetToSplit.TentativeNodes[0];
						if (selectedFleetGroup.FleetToSplit.AdjacentSystem != null && selectedFleetGroup.FleetToSplit.TravelNodes != null)
						{
							//Haven't left yet, so calculate custom path from left side of star
							float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX - 32);
							float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
							float length = (float)Math.Sqrt((x * x) + (y * y));
							float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
							pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - camera.CameraX - 32) * camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (length / pathSprite.Width), camera.ZoomDistance, travelNode.IsValid ? Color.LightGreen : Color.Red, angle);
						}
						else if (selectedFleetGroup.FleetToSplit.AdjacentSystem != null)
						{
							//Haven't left, and not on enroute already, so calculate path from right side of star
							float x = travelNode.StarSystem.X - (selectedFleetGroup.FleetToSplit.GalaxyX + 32);
							float y = travelNode.StarSystem.Y - selectedFleetGroup.FleetToSplit.GalaxyY;
							float length = (float)Math.Sqrt((x * x) + (y * y));
							float angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
							pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - camera.CameraX + 32) * camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (length / pathSprite.Width), camera.ZoomDistance, travelNode.IsValid ? Color.LightGreen : Color.Red, angle);
						}
						else
						{
							pathSprite.Draw((selectedFleetGroup.FleetToSplit.GalaxyX - camera.CameraX) * camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.GalaxyY - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedFleetGroup.FleetToSplit.TentativeNodes[0].Length / pathSprite.Width), camera.ZoomDistance, Color.LightGreen, selectedFleetGroup.FleetToSplit.TentativeNodes[0].Angle);
						}
					}
					else
					{
						pathSprite.Draw((selectedFleetGroup.FleetToSplit.TentativeNodes[i - 1].StarSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedFleetGroup.FleetToSplit.TentativeNodes[i - 1].StarSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedFleetGroup.FleetToSplit.TentativeNodes[i].Length / pathSprite.Width), camera.ZoomDistance, selectedFleetGroup.FleetToSplit.TentativeNodes[i].IsValid ? Color.LightGreen : Color.Red, selectedFleetGroup.FleetToSplit.TentativeNodes[i].Angle);
					}
				}
			}

			foreach (Fleet fleet in _gameMain.EmpireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, _gameMain.ScreenWidth / camera.ZoomDistance, _gameMain.ScreenHeight / camera.ZoomDistance))
			{
				bool visible = fleet.Empire == _gameMain.EmpireManager.CurrentEmpire;
				if (visible)
				{
					if (fleet.AdjacentSystem != null)
					{
						if (fleet.TravelNodes != null && fleet.TravelNodes.Count > 0)
						{
							if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
							{
								selectionSprites[0].Draw(((fleet.GalaxyX - 48) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[1].Draw(((fleet.GalaxyX - 32) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[2].Draw(((fleet.GalaxyX - 48) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[3].Draw(((fleet.GalaxyX - 32) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
							}
							//Adjacent to a system, but is heading to another system
							fleet.Empire.EmpireRace.FleetIcon.Draw((int)(((fleet.GalaxyX - 32) - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
						}
						else
						{
							if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
							{
								selectionSprites[0].Draw(((fleet.GalaxyX + 16) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[1].Draw(((fleet.GalaxyX + 32) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[2].Draw(((fleet.GalaxyX + 16) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
								selectionSprites[3].Draw(((fleet.GalaxyX + 32) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
							}
							//Adjacent to a system, just chilling
							fleet.Empire.EmpireRace.FleetIcon.Draw((int)(((fleet.GalaxyX + 32) - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
						}
					}
					else
					{
						if (selectedFleetGroup != null && selectedFleetGroup.Fleets.Contains(fleet))
						{
							selectionSprites[0].Draw(((fleet.GalaxyX - 16) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
							selectionSprites[1].Draw(((fleet.GalaxyX) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY - 16) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
							selectionSprites[2].Draw(((fleet.GalaxyX - 16) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
							selectionSprites[3].Draw(((fleet.GalaxyX) - camera.CameraX) * camera.ZoomDistance, ((fleet.GalaxyY) - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance, camera.ZoomDistance, Color.Green);
						}
						fleet.Empire.EmpireRace.FleetIcon.Draw((int)((fleet.GalaxyX - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
					}
				}
			}
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			DrawGalaxy(drawingManagement);

			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;

			if (selectedFleetGroup != null)
			{
				/*drawingManagement.DrawSprite(SpriteName.SelectedFleet, (int)(((((selectedFleetGroup.Fleets[0].GalaxyX - 1) - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)(((((selectedFleetGroup.Fleets[0].GalaxyY - 1) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[2], sizes[2], System.Drawing.Color.White);
				if (selectedFleetGroup.FleetToSplit.TentativeNodes != null)
				{
					foreach (Point node in selectedFleetGroup.FleetToSplit.TentativeNodes)
					{
						if (_gameMain.Galaxy.GetGridCells()[node.X][node.Y].passable)
						{
							drawingManagement.DrawSprite(SpriteName.SelectCell, (int)((((node.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((node.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 150, sizes[0], sizes[0], System.Drawing.Color.LightGreen);
						}
					}
					Point lastNode = selectedFleetGroup.FleetToSplit.TentativeNodes[selectedFleetGroup.FleetToSplit.TentativeNodes.Count - 1];
					drawingManagement.DrawText("Arial", "ETA: " + selectedFleetGroup.FleetToSplit.TentativeETA + " Turns", (int)((((lastNode.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)(((((lastNode.Y + 1) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), System.Drawing.Color.White);
				}
				if (selectedFleetGroup.FleetToSplit.TravelNodes != null)
				{
					foreach (Point node in selectedFleetGroup.FleetToSplit.TravelNodes)
					{
						if (_gameMain.Galaxy.GetGridCells()[node.X][node.Y].passable)
						{
							drawingManagement.DrawSprite(SpriteName.SelectCell, (int)((((node.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((node.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 150, sizes[0], sizes[0], System.Drawing.Color.Green);
						}
					}
				}*/

				drawingManagement.DrawSprite(SpriteName.ControlBackground, _gameMain.ScreenWidth - 207, 0, 255, 207, 460, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.ControlBackground, _gameMain.ScreenWidth - 204, 6, 255, 200, 110, System.Drawing.Color.DarkGray);
				int max = selectedFleetGroup.Fleets.Count > 4 ? 4 : selectedFleetGroup.Fleets.Count;

				int x = _gameMain.ScreenWidth - 200;
				if (selectedFleetGroup.Fleets.Count <= 4)
				{
					x += 10;
				}
				for (int i = 0; i < max; i++)
				{
					string travel = selectedFleetGroup.Fleets[i + selectedFleetGroup.FleetIndex].ETA > 0 ? " - ETA " + selectedFleetGroup.Fleets[i + selectedFleetGroup.FleetIndex].ETA :
					" - Idling";
					fleetButtons[i].Draw(drawingManagement);
					drawingManagement.DrawText("Arial", selectedFleetGroup.Fleets[i + selectedFleetGroup.FleetIndex].Empire.EmpireName + " Fleet" + travel, x, 32 + (i * 20), selectedFleetGroup.Fleets[i + selectedFleetGroup.FleetIndex].Empire.EmpireColor);
				}
				if (selectedFleetGroup.Fleets.Count > 4)
				{
					fleetScrollBar.DrawScrollBar(drawingManagement);
				}

				drawingManagement.DrawText("Arial", "Fleets at this location", _gameMain.ScreenWidth - 200, 10, System.Drawing.Color.White);

				x = _gameMain.ScreenWidth - 203;
				if (selectedFleetGroup.Fleets[_gameMain.EmpireManager.CurrentEmpire.FleetSelected].Ships.Count <= 8)
				{
					x += 10;
				}
				else
				{
					shipSelectorScrollBar.DrawScrollBar(drawingManagement);
				}
				for (int i = 0; i < shipScrollBars.Length; i++)
				{
					shipScrollBars[i].DrawScrollBar(drawingManagement);
					drawingManagement.DrawText("Arial", selectedFleetGroup.GetShipsForDisplay()[i].Name + " x " +  selectedFleetGroup.FleetToSplit.Ships[selectedFleetGroup.GetShipsForDisplay()[i]], x, 130 + i * 40, System.Drawing.Color.White);
				}
			}
			if (selectedSystem != null)
			{
				systemView.Draw();
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			pathSprite.Update(frameDeltaTime, _gameMain.Random);
			_gameMain.Galaxy.Update(frameDeltaTime, _gameMain.Random);
			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			if (currentEmpire.SelectedSystem != null)
			{
				if (systemView.MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					return;
				}
			}
			if (currentEmpire.SelectedFleetGroup != null)
			{
				if (currentEmpire.SelectedFleetGroup.Fleets.Count > 4)
				{
					if (fleetScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
					{
						//When a fleet is selected, this may move the button up or down, need to update as needed
						currentEmpire.SelectedFleetGroup.FleetIndex = fleetScrollBar.TopIndex;
						foreach (Button button in fleetButtons)
						{
							button.Selected = false;
						}
						int adjustedIndex = currentEmpire.FleetSelected - fleetScrollBar.TopIndex;
						if (adjustedIndex >= 0 && adjustedIndex < fleetButtons.Length)
						{
							fleetButtons[adjustedIndex].Selected = true;
						}
					}
					foreach (Button button in fleetButtons)
					{
						button.UpdateHovering(mouseX, mouseY, frameDeltaTime);
					}
				}
				if (currentEmpire.SelectedFleetGroup.Fleets[currentEmpire.FleetSelected].Empire == currentEmpire)
				{
					for (int i = 0; i < shipScrollBars.Length; i++)
					{
						if (shipScrollBars[i].UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							//incremented/decremented the amount of ships for moving
							currentEmpire.SelectedFleetGroup.FleetToSplit.Ships[currentEmpire.SelectedFleetGroup.GetShipsForDisplay()[i]] = shipScrollBars[i].TopIndex;
						}
					}
					if (shipSelectorScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
					{
						//update the list of ships
						currentEmpire.SelectedFleetGroup.ShipIndex = shipSelectorScrollBar.TopIndex;
						LoadSelectedFleetInfoIntoUI(currentEmpire.SelectedFleetGroup);
						shipSelectorScrollBar.TopIndex = currentEmpire.SelectedFleetGroup.ShipIndex;
					}
					Point hoveringPoint = new Point();

					hoveringPoint.X = (int)((mouseX / camera.ZoomDistance) + camera.CameraX);
					hoveringPoint.Y = (int)((mouseY / camera.ZoomDistance) + camera.CameraY);

					StarSystem selectedSystem = _gameMain.Galaxy.GetStarAtPoint(hoveringPoint);
					currentEmpire.SelectedFleetGroup.FleetToSplit.SetTentativePath(selectedSystem, currentEmpire.SelectedFleetGroup.FleetToSplit.HasReserveTanks, _gameMain.Galaxy);
				}
				if ((mouseX >= _gameMain.ScreenWidth - 207 && mouseX < _gameMain.ScreenWidth - 1) && (mouseY < 460 && mouseY > 0))
				{
					return;
				}
			}
			camera.HandleUpdate(mouseX, mouseY, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
				{
					if (systemView.MouseDown(x, y))
					{
						return;
					}
				}
				else if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (x >= _gameMain.ScreenWidth - 250 && y < 720)
					{
						pressedInWindow = true;
					}
					if (fleetScrollBar.MouseDown(x, y))
					{
						return;
					}
					for (int i = 0; i < fleetButtons.Length; i++)
					{
						if (fleetButtons[i].MouseDown(x, y))
						{
							return;
						}
					}
					if (shipSelectorScrollBar.MouseDown(x, y))
					{
						return;
					}
					for (int i = 0; i < shipScrollBars.Length; i++)
					{
						if (shipScrollBars[i].MouseDown(x, y))
						{
							return;
						}
					}
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
				if (currentEmpire.SelectedSystem != null)
				{
					if (systemView.MouseUp(x, y))
					{
						return;
					}
					if (systemView.IsTransferring)
					{
						Point point = new Point();

						point.X = (int)((x / camera.ZoomDistance) + camera.CameraX);
						point.Y = (int)((y / camera.ZoomDistance) + camera.CameraY);

						StarSystem system = _gameMain.Galaxy.GetStarAtPoint(point);
						if (system != null)
						{
							var path = _gameMain.Galaxy.GetPath(currentEmpire.SelectedSystem.X, currentEmpire.SelectedSystem.Y, null, system, false, currentEmpire);
							if (path.Count > 0)
							{
								systemView.TransferSystem = path[0];
							}
						}
						else
						{
							//Clicked to clear the option
							systemView.IsTransferring = false;
						}
						return;
					}
				}
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (fleetScrollBar.MouseUp(x, y))
					{
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetIndex = fleetScrollBar.TopIndex;
						foreach (Button button in fleetButtons)
						{
							button.Selected = false;
						}
						int adjustedIndex = _gameMain.EmpireManager.CurrentEmpire.FleetSelected - fleetScrollBar.TopIndex;
						if (adjustedIndex >= 0 && adjustedIndex < fleetButtons.Length)
						{
							fleetButtons[adjustedIndex].Selected = true;
						}
						pressedInWindow = false;
					}
					for (int i = 0; i < fleetButtons.Length; i++)
					{
						if (fleetButtons[i].MouseUp(x, y))
						{
							_gameMain.EmpireManager.CurrentEmpire.FleetSelected = i + _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetIndex;
							_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SelectFleet(_gameMain.EmpireManager.CurrentEmpire.FleetSelected);
							LoadSelectedFleetInfoIntoUI(_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
							foreach (Button fleetButton in fleetButtons)
							{
								fleetButton.Selected = false;
							}
							fleetButtons[i].Selected = true;
							pressedInWindow = false;
						}
					}
					if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Ships.Count > 8 && shipSelectorScrollBar.MouseUp(x, y))
					{
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.ShipIndex = shipSelectorScrollBar.TopIndex;
						LoadSelectedFleetInfoIntoUI(_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
						shipSelectorScrollBar.TopIndex = _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.ShipIndex;
						pressedInWindow = false;
					}
					for (int i = 0; i < shipScrollBars.Length; i++)
					{
						if (shipScrollBars[i].MouseUp(x, y))
						{
							_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Ships[_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.GetShipsForDisplay()[i]] = shipScrollBars[i].TopIndex;
							pressedInWindow = false;
						}
					}
					if (x >= _gameMain.ScreenWidth - 207 && y < 460)
					{
						pressedInWindow = false;
						return;
					}
				}
				if (pressedInWindow)
				{
					pressedInWindow = false;
					return;
				}
				Point pointClicked = new Point();

				pointClicked.X = (int)((x / camera.ZoomDistance) + camera.CameraX);
				pointClicked.Y = (int)((y / camera.ZoomDistance) + camera.CameraY);

				StarSystem selectedSystem = _gameMain.Galaxy.GetStarAtPoint(pointClicked);
				if (selectedSystem != null && selectedSystem == _gameMain.EmpireManager.CurrentEmpire.SelectedSystem)
				{
					return;
				}
				_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = selectedSystem;

				if (selectedSystem != null)
				{
					_gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem = selectedSystem;
					_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = null;
					systemView.LoadSystem();
					return;
				}

				FleetGroup selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(pointClicked.X, pointClicked.Y);
				_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;

				if (selectedFleetGroup != null)
				{
					selectedFleetGroup.ShipIndex = 0;
					LoadFleetInfoIntoUI(selectedFleetGroup);
					return;
				}
				camera.CenterCamera(pointClicked.X, pointClicked.Y, camera.ZoomDistance);
			}
			else if (whichButton == 2)
			{
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null && _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Empire == _gameMain.EmpireManager.CurrentEmpire)
				{
					if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.ConfirmPath())
					{
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(_gameMain.EmpireManager.CurrentEmpire);
						LoadFleetInfoIntoUI(_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
			camera.MouseWheel(direction, x, y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
			{
				if (systemView.KeyDown(e))
				{
					return;
				}
			}
			if (e.Key == KeyboardKeys.F)
			{
				showingFuelRange = !showingFuelRange;
			}
			if (e.Key == KeyboardKeys.Escape)
			{
				_gameMain.ChangeToScreen(Screen.InGameMenu);
			}
			if (e.Key == KeyboardKeys.Space)
			{
				_gameMain.ToggleSitRep();
			}
		}

		private void LoadFleetInfoIntoUI(FleetGroup fleetGroup)
		{
			int maxVisible = fleetGroup.Fleets.Count > 4 ? 4 : fleetGroup.Fleets.Count;
			fleetButtons = new Button[maxVisible];
			int x = _gameMain.ScreenWidth - 200;
			if (fleetGroup.Fleets.Count <= 4)
			{
				x += 10;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, /*fleetGroup.Fleets[i + fleetGroup.FleetIndex].Empire.EmpireName + " Fleet" + travel*/"", x, 30 + (20 * i), 175, 20);
			}
			fleetScrollBar.SetAmountOfItems(fleetGroup.Fleets.Count);
			fleetScrollBar.TopIndex = 0;
			fleetButtons[0].Selected = true;
			_gameMain.EmpireManager.CurrentEmpire.FleetSelected = 0;
			_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SelectFleet(0);
			LoadSelectedFleetInfoIntoUI(fleetGroup);
		}

		private void LoadSelectedFleetInfoIntoUI(FleetGroup fleetGroup)
		{
			Fleet selectedFleet = fleetGroup.Fleets[_gameMain.EmpireManager.CurrentEmpire.FleetSelected];
			bool isEnabled = selectedFleet.Empire == _gameMain.EmpireManager.CurrentEmpire;
			int x = _gameMain.ScreenWidth - 203;
			if (selectedFleet.Ships.Count <= 8)
			{
				x += 10;
			}

			List<Ship> ships = fleetGroup.GetShipsForDisplay();
			shipScrollBars = new ScrollBar[ships.Count];

			for (int i = 0; i < ships.Count; i++)
			{
				int itemSize = (int)(168.0f / selectedFleet.Ships[ships[i]]);
				shipScrollBars[i] = new ScrollBar(x, 150 + i * 40, 16, 148, 1, selectedFleet.Ships[ships[i]] + 1, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton,
					SpriteName.SliderHorizontalForegroundButton, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				shipScrollBars[i].TopIndex = selectedFleet.Ships[ships[i]];
				shipScrollBars[i].SetEnabledState(isEnabled);
			}
			shipSelectorScrollBar.SetAmountOfItems(fleetGroup.Fleets[_gameMain.EmpireManager.CurrentEmpire.FleetSelected].Ships.Count);
		}
	}
}
