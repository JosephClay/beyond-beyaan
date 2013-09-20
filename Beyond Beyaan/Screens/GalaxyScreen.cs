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

		private RenderTarget oldTarget;
		private RenderImage starName;
		private RenderImage backBuffer;
		private SystemView _systemView;
		private FleetView _fleetView;

		private TaskBar _taskBar;
		private InGameMenu _inGameMenu;
		private ResearchScreen _researchScreen;

		private WindowInterface _windowShowing;

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

			camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, _gameMain.ScreenWidth, _gameMain.ScreenHeight);

			/*
			fleetScrollBar = new ScrollBar(_gameMain.ScreenWidth - 24, 30, 16, 48, 4, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			shipSelectorScrollBar = new ScrollBar(_gameMain.ScreenWidth - 20, 130, 16, 286, 8, 10, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);*/

			starName = new RenderImage("starNameRendered", 1, 1, ImageBufferFormats.BufferRGB888A8);
			starName.BlendingMode = BlendingModes.Modulated;

			backBuffer = new RenderImage("galaxyBackBuffer", _gameMain.ScreenWidth, _gameMain.ScreenHeight, ImageBufferFormats.BufferRGB888A8);
			backBuffer.BlendingMode = BlendingModes.Modulated;

			_systemView = new SystemView();
			if (!_systemView.Initialize(_gameMain, "GalaxyScreen", out reason))
			{
				return false;
			}
			_fleetView = new FleetView();
			if (!_fleetView.Initialize(_gameMain, out reason))
			{
				return false;
			}

			_taskBar = new TaskBar();
			if (!_taskBar.Initialize(_gameMain, out reason))
			{
				return false;
			}
			_inGameMenu = new InGameMenu();
			_researchScreen = new ResearchScreen();
			if (!_inGameMenu.Initialize(_gameMain, out reason))
			{
				return false;
			}
			if (!_researchScreen.Initialize(_gameMain, out reason))
			{
				return false;
			}
			_inGameMenu.CloseWindow = CloseWindow;
			_researchScreen.CloseWindow = CloseWindow;

			_taskBar.ShowGameMenu = ShowInGameMenu;
			_taskBar.ShowResearchScreen = ShowResearchScreen;

			reason = null;
			return true;
		}

		public void CenterScreen()
		{
			if (_gameMain.EmpireManager.CurrentEmpire != null && _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem != null)
			{
				_gameMain.EmpireManager.CurrentEmpire.SelectedSystem = _gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem;
				camera.CenterCamera(_gameMain.EmpireManager.CurrentEmpire.SelectedSystem.X, _gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Y, camera.ZoomDistance);
				_systemView.LoadSystem();
			}
		}

		public void CenterScreenToPoint(Point point)
		{
			camera.CenterCamera(point.X * 60, point.Y * 60, camera.ZoomDistance);
		}

		//Used when other non-combat screens are open, to fill in the blank areas
		public void DrawGalaxy()
		{
			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;
			List<StarSystem> systems = _gameMain.Galaxy.GetAllStars();
			bool displayName = camera.ZoomDistance > 0.8f;

			if (showingFuelRange)
			{
				// TODO: Optimize this by going through an empire's owned systems, instead of all stars
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
				if (selectedSystem.Planets[0].RelocateToSystem != null)
				{
					pathSprite.Draw((selectedSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (selectedSystem.Planets[0].RelocateToSystem.Length / pathSprite.Width), camera.ZoomDistance, Color.Blue, selectedSystem.Planets[0].RelocateToSystem.Angle);
				}
				if (_systemView.IsTransferring && _systemView.TransferSystem != null)
				{
					pathSprite.Draw((selectedSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (_systemView.TransferSystem.Length / pathSprite.Width), camera.ZoomDistance, _systemView.TransferSystem.IsValid ? Color.LightGreen : Color.Red, _systemView.TransferSystem.Angle);
				}
				if (_systemView.IsRelocating && _systemView.RelocateSystem != null)
				{
					pathSprite.Draw((selectedSystem.X - camera.CameraX) * camera.ZoomDistance, (selectedSystem.Y - camera.CameraY) * camera.ZoomDistance, camera.ZoomDistance * (_systemView.RelocateSystem.Length / pathSprite.Width), camera.ZoomDistance, _systemView.RelocateSystem.IsValid ? Color.LightSkyBlue : Color.Red, _systemView.RelocateSystem.Angle);
				}
			}

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = _gameMain.StarShader;
				_gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
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
					system.StarName.MoveTo((int)x, (int)y);
					if (system.DominantEmpire != null)
					{
						// TODO: Optimize this by moving the text sprite and color shader to StarSystem, where it's updated when ownership changes
						float percentage = 1.0f;
						oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						starName.Width = (int)system.StarName.GetWidth();
						starName.Height = (int)system.StarName.GetHeight();
						GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
						system.StarName.MoveTo(0, 0);
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
					BBSprite fleetIcon = fleet.Ships.Count > 0 ? fleet.Empire.EmpireRace.FleetIcon : fleet.Empire.EmpireRace.TransportIcon;
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
							fleetIcon.Draw((int)(((fleet.GalaxyX - 32) - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
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
							fleetIcon.Draw((int)(((fleet.GalaxyX + 32) - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
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
						fleetIcon.Draw((int)((fleet.GalaxyX - camera.CameraX) * camera.ZoomDistance), (int)((fleet.GalaxyY - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance, fleet.Empire.EmpireColor);
					}
				}
			}
		}

		public void DrawScreen()
		{
			DrawGalaxy();

			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;

			_taskBar.Draw();

			if (selectedFleetGroup != null)
			{
				_fleetView.Draw();
				/* TODO: Add ETA display
				drawingManagement.DrawText("Arial", "ETA: " + selectedFleetGroup.FleetToSplit.TentativeETA + " Turns", (int)((((lastNode.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)(((((lastNode.Y + 1) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), System.Drawing.Color.White);*/
			}
			if (selectedSystem != null)
			{
				_systemView.Draw();
			}
			if (_windowShowing != null)
			{
				_windowShowing.Draw();
			}
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			pathSprite.Update(frameDeltaTime, _gameMain.Random);
			_gameMain.Galaxy.Update(frameDeltaTime, _gameMain.Random);

			if (_windowShowing != null)
			{
				_windowShowing.MouseHover(x, y, frameDeltaTime);
				return;
			}

			Empire currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			if (currentEmpire.SelectedSystem != null)
			{
				if (_systemView.MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
			}
			if (currentEmpire.SelectedFleetGroup != null)
			{
				if (_fleetView.MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
				if (currentEmpire.SelectedFleetGroup.SelectedFleet.Empire == currentEmpire)
				{
					Point hoveringPoint = new Point();

					hoveringPoint.X = (int)((x / camera.ZoomDistance) + camera.CameraX);
					hoveringPoint.Y = (int)((y / camera.ZoomDistance) + camera.CameraY);

					StarSystem selectedSystem = _gameMain.Galaxy.GetStarAtPoint(hoveringPoint);
					currentEmpire.SelectedFleetGroup.FleetToSplit.SetTentativePath(selectedSystem, currentEmpire.SelectedFleetGroup.FleetToSplit.HasReserveTanks, _gameMain.Galaxy);
				}
			}
			if (_taskBar.MouseHover(x, y, frameDeltaTime))
			{
				return;
			}
			
			camera.HandleUpdate(x, y, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				if (_windowShowing != null)
				{
					_windowShowing.MouseDown(x, y);
					return;
				}
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
				{
					if (_systemView.MouseDown(x, y))
					{
						return;
					}
				}
				else if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (_fleetView.MouseDown(x, y))
					{
						return;
					}
				}
				if (_taskBar.MouseDown(x, y, whichButton))
				{
					return;
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (whichButton == 1)
			{
				if (_windowShowing != null)
				{
					_windowShowing.MouseUp(x, y);
					return;
				}
				var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
				if (currentEmpire.SelectedSystem != null)
				{
					if (_systemView.MouseUp(x, y))
					{
						return;
					}
					if (_systemView.IsTransferring)
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
								if (!path[0].StarSystem.IsThisSystemExploredByEmpire(currentEmpire) || path[0].StarSystem.Planets[0].Owner == null)
								{
									path[0].IsValid = false;
								}
								_systemView.TransferSystem = path[0];
							}
						}
						else
						{
							//Clicked to clear the option
							_systemView.IsTransferring = false;
							_systemView.TransferSystem = null;
						}
						return;
					}
					if (_systemView.IsRelocating)
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
								if (path[0].StarSystem.Planets[0].Owner != currentEmpire)
								{
									path[0].IsValid = false;
								}
								_systemView.RelocateSystem = path[0];
							}
						}
						else
						{
							//Clicked to clear the option
							_systemView.IsRelocating = false;
							_systemView.RelocateSystem = null;
						}
						return;
					}
				}
				if (_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (_fleetView.MouseUp(x, y))
					{
						return;
					}
				}
				//If a window is open, but the player didn't click on another system or fleet, the action is to close the current window
				bool clearingUI = _gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null || _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null;
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
					_systemView.LoadSystem();
					return;
				}

				FleetGroup selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(pointClicked.X, pointClicked.Y);
				_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;

				if (selectedFleetGroup != null)
				{
					_fleetView.LoadFleetGroup(selectedFleetGroup);
					return;
				}
				if (_taskBar.MouseUp(x, y, whichButton))
				{
					return;
				}
				if (!clearingUI)
				{
					camera.CenterCamera(pointClicked.X, pointClicked.Y, camera.ZoomDistance);
				}
			}
			else if (whichButton == 2)
			{
				if (_windowShowing != null)
				{
					return;
				}
				var selectedFleetGroup = _gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup;
				if (selectedFleetGroup != null && selectedFleetGroup.FleetToSplit.Empire == _gameMain.EmpireManager.CurrentEmpire)
				{
					bool isIdling = selectedFleetGroup.SelectedFleet.AdjacentSystem != null;
					bool hasDestination = selectedFleetGroup.SelectedFleet.TravelNodes != null;
					if (selectedFleetGroup.FleetToSplit.ConfirmPath())
					{
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(_gameMain.EmpireManager.CurrentEmpire);
						_gameMain.EmpireManager.CurrentEmpire.FleetManager.MergeIdleFleets();
						if (isIdling && !hasDestination) //Select the remaining idling ships on right of star
						{
							Point point = new Point((int)selectedFleetGroup.SelectedFleet.GalaxyX + 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(point.X, point.Y);
							if (selectedFleetGroup == null) // No ships left, select the fleet on left of star
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint(point.X - 64, point.Y);
							}
						}
						else if (isIdling)
						{
							if (selectedFleetGroup.SelectedFleet.TravelNodes == null) //cleared out movement order
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX + 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							}
							else
							{
								selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX - 32, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
							}
						}
						else
						{
							selectedFleetGroup = _gameMain.EmpireManager.GetFleetsAtPoint((int)selectedFleetGroup.SelectedFleet.GalaxyX, (int)selectedFleetGroup.SelectedFleet.GalaxyY);
						}
						_gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;
						_fleetView.LoadFleetGroup(selectedFleetGroup);
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
			if (_windowShowing != null)
			{
				return;
			}
			camera.MouseWheel(direction, x, y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (_windowShowing != null)
			{
				_windowShowing.KeyDown(e);
				return;
			}
			if (_gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
			{
				if (_systemView.KeyDown(e))
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

		private void CloseWindow()
		{
			_windowShowing = null;
			_taskBar.Clear();
		}

		private void ShowInGameMenu()
		{
			_windowShowing = _inGameMenu;
			_inGameMenu.GetSaveList();
		}

		private void ShowResearchScreen()
		{
			_windowShowing = _researchScreen;
			_researchScreen.Load();
		}

		public void ResetCamera()
		{
			camera = new Camera(_gameMain.Galaxy.GalaxySize * 60, _gameMain.Galaxy.GalaxySize * 60, _gameMain.ScreenWidth, _gameMain.ScreenHeight);
		}
	}
}
