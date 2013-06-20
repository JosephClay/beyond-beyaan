using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class GalaxyScreen : ScreenInterface
	{
		#region Constants
		private const int INFRASTRUCTURE = 0;
		private const int WASTE = 1;
		private const int DEFENSE = 2;
		private const int RESEARCH = 3;
		private const int CONSTRUCTION = 4;
		#endregion

		private GameMain gameMain;
		private Camera camera;
		//private GorgonLibrary.Graphics.Sprite shipSprite;
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
		private GorgonLibrary.Graphics.RenderTarget oldTarget;
		private GorgonLibrary.Graphics.RenderImage starName;
		private SystemView systemView;

		private bool pressedInWindow;
		private Label[] racePopLabels;
		private Label planetOwner;
		private Button transferButton;

		private Button transferOKButton;
		private Button transferCancelButton;
		private ScrollBar[] popTransferSliders;
		private Label[] amountPopTransferLabel;
		//private List<TransportShip> tempTransportShips;
		//private ScrollBar listOfPopTransferSlier;
		private bool showingTransferUI;
		//private int maxVisible;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			camera = new Camera(gameMain.Galaxy.GalaxySize * 32, gameMain.Galaxy.GalaxySize * 32, gameMain.ScreenWidth, gameMain.ScreenHeight);

			systemButtons = new Button[6];
			for (int i = 0; i < systemButtons.Length; i++)
			{
				systemButtons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, gameMain.ScreenWidth - 490, 8 + (47 * i), 215, 47);
			}
			systemScrollBar = new ScrollBar(gameMain.ScreenWidth - 274, 8, 20, 242, 6, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			fleetScrollBar = new ScrollBar(gameMain.ScreenWidth - 24, 30, 16, 48, 4, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			shipSelectorScrollBar = new ScrollBar(gameMain.ScreenWidth - 20, 130, 16, 286, 8, 10, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
							SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton,
							SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);

			planetOwner = new Label(gameMain.ScreenWidth - 490, 300);
			racePopLabels = new Label[0];

			planetScrollBars = new ScrollBar[5];
			planetFieldLocks = new Button[5];

			for (int i = 0; i < 5; i++)
			{
				planetScrollBars[i] = new ScrollBar(gameMain.ScreenWidth - 245, 30 + (i * 40), 16, 188, 1, 101, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton,
					SpriteName.SliderHorizontalForegroundButton, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				planetFieldLocks[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, gameMain.ScreenWidth - 21, 30 + (i * 40), 16, 16);
			}

			prevShip = new Button(SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton, string.Empty, gameMain.ScreenWidth - 245, 275, 16, 16);
			nextShip = new Button(SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, string.Empty, gameMain.ScreenWidth - 25, 275, 16, 16);
			pressedInWindow = false;

			starName = new GorgonLibrary.Graphics.RenderImage("starNameRendered", 1, 1, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			starName.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;

			transferButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Transfer Population", gameMain.ScreenWidth - 245, 370, 240, 25);

			transferOKButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Create Transport", gameMain.ScreenWidth / 2 + 10, gameMain.ScreenHeight / 2 + 50, 150, 25);
			transferCancelButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Cancel", gameMain.ScreenWidth / 2 - 160, gameMain.ScreenHeight / 2 + 50, 150, 25);
			popTransferSliders = new ScrollBar[4];
			amountPopTransferLabel = new Label[4];
			for (int i = 0; i < 4; i++)
			{
				popTransferSliders[i] = new ScrollBar(gameMain.ScreenWidth / 2 - 160, (gameMain.ScreenHeight / 2 - 120) + (i * 40) + 22, 16, 288, 1, 100, true, true, SpriteName.ScrollLeftBackgroundButton,
					SpriteName.ScrollLeftForegroundButton, SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton, SpriteName.SliderHorizontalForegroundButton,
					SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				amountPopTransferLabel[i] = new Label(gameMain.ScreenWidth / 2 - 160, (gameMain.ScreenHeight / 2 - 120) + (i * 40));
			}

			string reason;
			systemView = new SystemView();
			systemView.Initialize(gameMain, gameMain.SpriteManager, gameMain.Random, out reason);
		}

		public void CenterScreen()
		{
			if (gameMain.EmpireManager.CurrentEmpire != null && gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem != null)
			{
				gameMain.EmpireManager.CurrentEmpire.SelectedSystem = gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem;
				camera.CenterCamera(gameMain.EmpireManager.CurrentEmpire.SelectedSystem.X, gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Y, camera.ZoomDistance);
				LoadSystemInfoIntoUI(gameMain.EmpireManager.CurrentEmpire.SelectedSystem);
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
			Empire currentEmpire = gameMain.EmpireManager.CurrentEmpire;
			//GorgonLibrary.Graphics.Sprite nebula = gameMain.Galaxy.Nebula;
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
			List<StarSystem> systems = gameMain.Galaxy.GetStarsInArea(camera.CameraX, camera.CameraY, gameMain.ScreenWidth / camera.ZoomDistance, gameMain.ScreenHeight / camera.ZoomDistance);
			GridCell[][] gridCells = gameMain.Galaxy.GetGridCells();
			bool displayName = camera.ZoomDistance > 0.8f;

			foreach (StarSystem system in systems)
			{
				GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
				gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
				//drawingManagement.DrawSprite(SpriteName.Star, (int)(((system.X * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((system.Y * 32) - camera.CameraY) * camera.ZoomDistance), 255, system.Size * 32 * camera.ZoomDistance, system.Size * 32 * camera.ZoomDistance, System.Drawing.Color.White);
				system.Sprite.Draw((int)((system.X - camera.CameraX) * camera.ZoomDistance), (int)((system.Y - camera.CameraY) * camera.ZoomDistance), camera.ZoomDistance, camera.ZoomDistance);
				GorgonLibrary.Gorgon.CurrentShader = null;

				if (displayName && (gameMain.EmpireManager.CurrentEmpire.ContactManager.IsContacted(system.DominantEmpire) || system.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire)))
				{
					float x = (system.X - camera.CameraX) * camera.ZoomDistance;
					x -= (system.StarName.GetWidth() / 2);
					float y = ((system.Y + (system.Size * 16)) - camera.CameraY) * camera.ZoomDistance;
					system.StarName.Move((int)x, (int)y);
					if (system.DominantEmpire != null)
					{
						float percentage = 1.0f;
						oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
						starName.Width = (int)system.StarName.GetWidth();
						starName.Height = (int)system.StarName.GetHeight();
						GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
						system.StarName.Move(0, 0);
						system.StarName.Draw();
						GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
						//GorgonLibrary.Gorgon.CurrentShader = gameMain.NameShader;
						foreach (Empire empire in system.EmpiresWithPlanetsInThisSystem)
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

			/*foreach (Fleet fleet in gameMain.EmpireManager.GetFleetsWithinArea(camera.CameraX, camera.CameraY, gameMain.ScreenWidth / camera.ZoomDistance, gameMain.ScreenHeight / camera.ZoomDistance))
			{
				bool visible = false;
				if (fleet.Empire == gameMain.EmpireManager.CurrentEmpire)
				{
					visible = true;
				}
				if (gridCells[fleet.GalaxyX][fleet.GalaxyY].dominantEmpire == currentEmpire || gridCells[fleet.GalaxyX][fleet.GalaxyY].secondaryEmpire == currentEmpire)
				{
					visible = true;
				}
				if (visible)
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, (int)(((fleet.GalaxyX * 32) - camera.CameraX) * camera.ZoomDistance), (int)(((fleet.GalaxyY * 32) - camera.CameraY) * camera.ZoomDistance), 255, 32 * camera.ZoomDistance, 32 * camera.ZoomDistance, fleet.Empire.EmpireColor);
				}
			}*/
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			DrawGalaxy(drawingManagement);

			Empire currentEmpire = gameMain.EmpireManager.CurrentEmpire;
			StarSystem selectedSystem = currentEmpire.SelectedSystem;
			//FleetGroup selectedFleetGroup = currentEmpire.SelectedFleetGroup;

			/*if (selectedFleetGroup != null)
			{
				drawingManagement.DrawSprite(SpriteName.SelectedFleet, (int)(((((selectedFleetGroup.Fleets[0].GalaxyX - 1) - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)(((((selectedFleetGroup.Fleets[0].GalaxyY - 1) - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 255, sizes[2], sizes[2], System.Drawing.Color.White);
				if (selectedFleetGroup.FleetToSplit.TentativeNodes != null)
				{
					foreach (Point node in selectedFleetGroup.FleetToSplit.TentativeNodes)
					{
						if (gameMain.Galaxy.GetGridCells()[node.X][node.Y].passable)
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
						if (gameMain.Galaxy.GetGridCells()[node.X][node.Y].passable)
						{
							drawingManagement.DrawSprite(SpriteName.SelectCell, (int)((((node.X - camera.CameraX) * 32) - camera.XOffset) * camera.Scale), (int)((((node.Y - camera.CameraY) * 32) - camera.YOffset) * camera.Scale), 150, sizes[0], sizes[0], System.Drawing.Color.Green);
						}
					}
				}

				drawingManagement.DrawSprite(SpriteName.ControlBackground, gameMain.ScreenWidth - 207, 0, 255, 207, 460, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.ControlBackground, gameMain.ScreenWidth - 204, 6, 255, 200, 110, System.Drawing.Color.DarkGray);
				int max = selectedFleetGroup.Fleets.Count > 4 ? 4 : selectedFleetGroup.Fleets.Count;

				int x = gameMain.ScreenWidth - 200;
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

				drawingManagement.DrawText("Arial", "Fleets at this location", gameMain.ScreenWidth - 200, 10, System.Drawing.Color.White);

				x = gameMain.ScreenWidth - 203;
				if (selectedFleetGroup.Fleets[gameMain.EmpireManager.CurrentEmpire.FleetSelected].Ships.Count <= 8)
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
			}*/
			if (selectedSystem != null)
			{
				/*drawingManagement.DrawSprite(SpriteName.ControlBackground, gameMain.ScreenWidth - 500, 0, 255, 500, 400, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.ControlBackground, gameMain.ScreenWidth - 497, 4, 255, 244, 294, System.Drawing.Color.DarkGray);
				if (selectedSystem.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
				{
					int x = gameMain.ScreenWidth - 490;
					int max = selectedSystem.Planets.Count > 6 ? 6 : selectedSystem.Planets.Count;
					for (int i = 0; i < max; i++)
					{
						systemButtons[i].Draw(drawingManagement);
						drawingManagement.DrawSprite(Utility.PlanetTypeToSprite(selectedSystem.Planets[i + systemScrollBar.TopIndex].PlanetType), x + 2, 11 + (i * 47), 255, System.Drawing.Color.White);
						if (selectedSystem.Planets[i + systemScrollBar.TopIndex].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
						{
							drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(selectedSystem.Planets[i + systemScrollBar.TopIndex].ConstructionBonus), x + 155, 11 + (i * 47), 255, System.Drawing.Color.White);
						}
						if (selectedSystem.Planets[i + systemScrollBar.TopIndex].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
						{
							drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(selectedSystem.Planets[i + systemScrollBar.TopIndex].EnvironmentBonus), x + 175, 11 + (i * 47), 255, System.Drawing.Color.White);
						}
						if (selectedSystem.Planets[i + systemScrollBar.TopIndex].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
						{
							drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(selectedSystem.Planets[i + systemScrollBar.TopIndex].EntertainmentBonus), x + 195, 11 + (i * 47), 255, System.Drawing.Color.White);
						}
					}
					systemScrollBar.DrawScrollBar(drawingManagement);
					planetOwner.Draw();
					for (int k = 0; k < racePopLabels.Length; k++)
					{
						racePopLabels[k].Draw();
					}
					if (max > 0)
					{
						Planet selectedPlanet = selectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected];
						if (selectedPlanet.Owner == gameMain.EmpireManager.CurrentEmpire)
						{
							for (int i = 0; i < planetScrollBars.Length; i++)
							{
								planetScrollBars[i].DrawScrollBar(drawingManagement);
								planetFieldLocks[i].Draw(drawingManagement);
							}
							prevShip.Draw(drawingManagement);
							nextShip.Draw(drawingManagement);
						}
						x += 45;
						for (int i = 0; i < max; i++)
						{
							Planet currentPlanet = selectedSystem.Planets[i + systemScrollBar.TopIndex];
							drawingManagement.DrawText("Arial", currentPlanet.Name, x, 10 + (i * 47), currentPlanet.Owner != null ? currentPlanet.Owner.EmpireColor : System.Drawing.Color.White);
							drawingManagement.DrawText("Arial", currentPlanet.PlanetTypeString, x, 30 + (i * 47), System.Drawing.Color.White);
							if (currentPlanet.Owner != null)
							{
								drawingManagement.DrawText("Arial", String.Format("{0:0}", currentPlanet.TotalPopulation) + "/" + currentPlanet.PopulationMax, x + 90, 30 + (i * 47), System.Drawing.Color.White);
							}
							else if (currentPlanet.PlanetType != PLANET_TYPE.ASTEROIDS && currentPlanet.PlanetType != PLANET_TYPE.GAS_GIANT)
							{
								drawingManagement.DrawText("Arial", String.Format("{0:0}", currentPlanet.PopulationMax), x + 90, 30 + (i * 47), System.Drawing.Color.White);
							}
						}

						if (selectedPlanet.Owner == gameMain.EmpireManager.CurrentEmpire)
						{
							x = gameMain.ScreenWidth - 245;
							drawingManagement.DrawSprite(SpriteName.AgricultureIcon, x, 10, 255, System.Drawing.Color.White);
							drawingManagement.DrawSprite(SpriteName.EnvironmentIcon, x, 50, 255, System.Drawing.Color.White);
							drawingManagement.DrawSprite(SpriteName.CommerceIcon, x, 90, 255, System.Drawing.Color.White);
							drawingManagement.DrawSprite(SpriteName.ResearchIcon, x, 130, 255, System.Drawing.Color.White);
							drawingManagement.DrawSprite(SpriteName.ConstructionIcon, x, 170, 255, System.Drawing.Color.White);
							x += 20;
							drawingManagement.DrawText("Arial", selectedPlanet.AgricultureStringOutput, x, 10, System.Drawing.Color.White);
							drawingManagement.DrawText("Arial", selectedPlanet.EnvironmentStringOutput, x, 50, System.Drawing.Color.White);
							drawingManagement.DrawText("Arial", String.Format("{0:0.00}", selectedPlanet.CommerceOutput) + " BC", x, 90, System.Drawing.Color.White);
							drawingManagement.DrawText("Arial", String.Format("{0:0.00}", selectedPlanet.ResearchOutput) + " RP", x, 130, System.Drawing.Color.White);
							drawingManagement.DrawText("Arial", selectedPlanet.ConstructionStringOutput, x, 170, System.Drawing.Color.White);
							GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
							gameMain.ShipShader.Parameters["EmpireColor"].SetValue(currentEmpire.ConvertedColor);
							shipSprite.Draw();
							GorgonLibrary.Gorgon.CurrentShader = null;
							transferButton.Draw(drawingManagement);
						}
					}
				}
				else
				{
					for (int i = 0; i < 6; i++)
					{
						systemButtons[i].Draw(drawingManagement);
					}
					drawingManagement.DrawText("Arial", "Unexplored", gameMain.ScreenWidth - 450, 10, System.Drawing.Color.White);
				}*/
				systemView.Draw();
			}
			if (showingTransferUI)
			{
				drawingManagement.DrawSprite(SpriteName.ControlBackground, (gameMain.ScreenWidth / 2) - 170, (gameMain.ScreenHeight / 2) - 130, 255, 340, 250, System.Drawing.Color.White);
				transferOKButton.Draw(drawingManagement);
				transferCancelButton.Draw(drawingManagement);
				/*for (int i = 0; i < maxVisible; i++)
				{
					popTransferSliders[i].DrawScrollBar(drawingManagement);
				}*/
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (showingTransferUI)
			{
				transferCancelButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
				transferOKButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
				/*for (int i = 0; i < maxVisible; i++)
				{
					popTransferSliders[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
				}*/
				return;
			}
			Empire currentEmpire = gameMain.EmpireManager.CurrentEmpire;
			if (currentEmpire.SelectedSystem != null && !systemView.MouseHover(mouseX, mouseY, frameDeltaTime))
			{
				/*if (currentEmpire.SelectedSystem.Planets.Count > 6 && systemScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
				{
					for (int i = 0; i < (currentEmpire.SelectedSystem.Planets.Count <= 6 ? currentEmpire.SelectedSystem.Planets.Count : 6); i++)
					{
						//When a planet is selected, this may move the button up or down, need to update as needed
						if (currentEmpire.PlanetSelected - systemScrollBar.TopIndex == i)
						{
							systemButtons[i].Selected = true;
						}
						else
						{
							systemButtons[i].Selected = false;
						}
					}
					return;
				}
				foreach (Button button in systemButtons)
				{
					button.UpdateHovering(mouseX, mouseY, frameDeltaTime);
				}
				if (currentEmpire.SelectedSystem.Planets.Count > 0 && currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].Owner == currentEmpire)
				{
					for (int i = 0; i < planetScrollBars.Length; i++)
					{
						if (planetScrollBars[i].UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							switch (i)
							{
								case AGRICULTURE:
									currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, planetScrollBars[i].TopIndex);
									break;
								case WASTE:
									currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, planetScrollBars[i].TopIndex);
									break;
								case COMMERCE:
									currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.COMMERCE, planetScrollBars[i].TopIndex);
									break;
								case RESEARCH:
									currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.RESEARCH, planetScrollBars[i].TopIndex);
									break;
								case CONSTRUCTION:
									currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, planetScrollBars[i].TopIndex);
									break;
							}
							planetScrollBars[AGRICULTURE].TopIndex = currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].AgricultureAmount;
							planetScrollBars[WASTE].TopIndex = currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].EnvironmentAmount;
							planetScrollBars[COMMERCE].TopIndex = currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].CommerceAmount;
							planetScrollBars[RESEARCH].TopIndex = currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].ResearchAmount;
							planetScrollBars[CONSTRUCTION].TopIndex = currentEmpire.SelectedSystem.Planets[currentEmpire.PlanetSelected].ConstructionAmount;
							return;
						}
					}
					prevShip.UpdateHovering(mouseX, mouseY, frameDeltaTime);
					nextShip.UpdateHovering(mouseX, mouseY, frameDeltaTime);
					transferButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
				}
				if ((mouseX >= gameMain.ScreenWidth - 207 && mouseX < gameMain.ScreenWidth - 1) && (mouseY < 650 && mouseY > 0))
				{
					return;
				}*/
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
					int mouseOverX = (int)(((mouseX * camera.ZoomDistance) + camera.CameraX) / 32);
					int mouseOverY = (int)(((mouseY / camera.ZoomDistance) + camera.CameraY) / 32);

					currentEmpire.SelectedFleetGroup.FleetToSplit.SetTentativePath(mouseOverX, mouseOverY, gameMain.Galaxy);
				}
				if ((mouseX >= gameMain.ScreenWidth - 207 && mouseX < gameMain.ScreenWidth - 1) && (mouseY < 460 && mouseY > 0))
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
				if (showingTransferUI)
				{
					transferCancelButton.MouseDown(x, y);
					transferOKButton.MouseDown(x, y);
					/*for (int i = 0; i < maxVisible; i++)
					{
						popTransferSliders[i].MouseDown(x, y);
					}*/
					return;
				}
				if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null && !systemView.MouseDown(x, y))
				{
					/*if (x >= gameMain.ScreenWidth - 207 && y < 650)
					{
						pressedInWindow = true;
					}
					if (systemScrollBar.MouseDown(x, y))
					{
						return;
					}
					for (int i = 0; i < systemButtons.Length; i++)
					{
						if (systemButtons[i].MouseDown(x, y))
						{
							return;
						}
					}
					if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets.Count > 0 && gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].Owner == gameMain.EmpireManager.CurrentEmpire)
					{
						for (int i = 0; i < planetScrollBars.Length; i++)
						{
							if (planetScrollBars[i].MouseDown(x, y))
							{
								switch (i)
								{
									case AGRICULTURE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, planetScrollBars[i].TopIndex);
										break;
									case WASTE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, planetScrollBars[i].TopIndex);
										break;
									case COMMERCE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.COMMERCE, planetScrollBars[i].TopIndex);
										break;
									case RESEARCH:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.RESEARCH, planetScrollBars[i].TopIndex);
										break;
									case CONSTRUCTION:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, planetScrollBars[i].TopIndex);
										break;
								}
								planetScrollBars[AGRICULTURE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].AgricultureAmount;
								planetScrollBars[WASTE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].EnvironmentAmount;
								planetScrollBars[COMMERCE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].CommerceAmount;
								planetScrollBars[RESEARCH].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ResearchAmount;
								planetScrollBars[CONSTRUCTION].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ConstructionAmount;
								break;
							}
						}
						prevShip.MouseDown(x, y);
						nextShip.MouseDown(x, y);
						transferButton.MouseDown(x, y);
					}*/
				}
				else if (gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (x >= gameMain.ScreenWidth - 250 && y < 720)
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
				if (showingTransferUI)
				{
					if (transferCancelButton.MouseUp(x, y))
					{
						showingTransferUI = false;
					}
					if (transferOKButton.MouseUp(x, y))
					{
						//Create the transports
					}
					for (int i = 0; i < popTransferSliders.Length; i++)
					{
						if (popTransferSliders[i].MouseUp(x, y))
						{
							//Update the corresponding label
						}
					}
					return;
				}
				if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem != null)
				{
					if (systemView.MouseUp(x, y))
					{
						return;
					}
					else
					{
						pressedInWindow = false;
					}
					/*if (systemScrollBar.MouseUp(x, y))
					{
						for (int i = 0; i < (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets.Count <= 6 ? gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets.Count : 6); i++)
						{
							if (gameMain.EmpireManager.CurrentEmpire.PlanetSelected - systemScrollBar.TopIndex == i)
							{
								systemButtons[i].Selected = true;
							}
							else
							{
								systemButtons[i].Selected = false;
							}
						}
						pressedInWindow = false;
						return;
					}
					for (int i = 0; i < systemButtons.Length; i++)
					{
						if (systemButtons[i].MouseUp(x, y))
						{
							foreach (Button button in systemButtons)
							{
								button.Selected = false;
							}
							systemButtons[i].Selected = true;
							gameMain.EmpireManager.CurrentEmpire.PlanetSelected = i + systemScrollBar.TopIndex;
							LoadPlanetInfoIntoUI(gameMain.EmpireManager.CurrentEmpire.SelectedSystem);
							if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].Owner == gameMain.EmpireManager.CurrentEmpire)
							{
								planetScrollBars[AGRICULTURE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].AgricultureAmount;
								planetScrollBars[WASTE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].EnvironmentAmount;
								planetScrollBars[COMMERCE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].CommerceAmount;
								planetScrollBars[RESEARCH].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ResearchAmount;
								planetScrollBars[CONSTRUCTION].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ConstructionAmount;
								LoadShipSprite(gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected]);
							}
							pressedInWindow = false;
							return;
						}
					}
					if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets.Count > 0 && gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].Owner == gameMain.EmpireManager.CurrentEmpire)
					{
						for (int i = 0; i < planetScrollBars.Length; i++)
						{
							if (planetScrollBars[i].MouseUp(x, y))
							{
								switch (i)
								{
									case AGRICULTURE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.AGRICULTURE, planetScrollBars[i].TopIndex);
										break;
									case WASTE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.ENVIRONMENT, planetScrollBars[i].TopIndex);
										break;
									case COMMERCE:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.COMMERCE, planetScrollBars[i].TopIndex);
										break;
									case RESEARCH:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.RESEARCH, planetScrollBars[i].TopIndex);
										break;
									case CONSTRUCTION:
										gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].SetOutputAmount(OUTPUT_TYPE.CONSTRUCTION, planetScrollBars[i].TopIndex);
										break;
								}
								planetScrollBars[AGRICULTURE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].AgricultureAmount;
								planetScrollBars[WASTE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].EnvironmentAmount;
								planetScrollBars[COMMERCE].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].CommerceAmount;
								planetScrollBars[RESEARCH].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ResearchAmount;
								planetScrollBars[CONSTRUCTION].TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ConstructionAmount;
								pressedInWindow = false;
								break;
							}
						}
						if (prevShip.MouseUp(x, y))
						{
							Planet selectedPlanet = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected];
							selectedPlanet.ShipSelected--;
							if (selectedPlanet.ShipSelected < 0)
							{
								selectedPlanet.ShipSelected = gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns.Count - 1;
							}
							selectedPlanet.ShipBeingBuilt = gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns[selectedPlanet.ShipSelected];
							LoadShipSprite(selectedPlanet);
							pressedInWindow = false;
						}
						if (nextShip.MouseUp(x, y))
						{
							Planet selectedPlanet = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected];
							selectedPlanet.ShipSelected++;
							if (selectedPlanet.ShipSelected >= gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns.Count)
							{
								selectedPlanet.ShipSelected = 0;
							}
							selectedPlanet.ShipBeingBuilt = gameMain.EmpireManager.CurrentEmpire.FleetManager.CurrentDesigns[selectedPlanet.ShipSelected];
							LoadShipSprite(selectedPlanet);
							pressedInWindow = false;
						}
						if (transferButton.MouseUp(x, y))
						{
							Planet selectedPlanet = gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected];
							//Do something
							showingTransferUI = true;
							tempTransportShips = new List<TransportShip>();
							foreach (Race race in selectedPlanet.Races)
							{
								TransportShip tempShip = new TransportShip();
								tempShip.raceOnShip = race;
								tempTransportShips.Add(tempShip);
							}
							maxVisible = tempTransportShips.Count > 4 ? 4 : tempTransportShips.Count;
							for (int i = 0; i < maxVisible; i++)
							{
								popTransferSliders[i].SetAmountOfItems((int)selectedPlanet.GetRacePopulation(selectedPlanet.Races[i]));
							}
						}
					}
					if (x >= gameMain.ScreenWidth - 250 && y < 720)
					{
						pressedInWindow = false;
						return;
					}*/
				}
				if (gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null)
				{
					if (fleetScrollBar.MouseUp(x, y))
					{
						gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetIndex = fleetScrollBar.TopIndex;
						foreach (Button button in fleetButtons)
						{
							button.Selected = false;
						}
						int adjustedIndex = gameMain.EmpireManager.CurrentEmpire.FleetSelected - fleetScrollBar.TopIndex;
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
							gameMain.EmpireManager.CurrentEmpire.FleetSelected = i + gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetIndex;
							gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SelectFleet(gameMain.EmpireManager.CurrentEmpire.FleetSelected);
							LoadSelectedFleetInfoIntoUI(gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
							foreach (Button fleetButton in fleetButtons)
							{
								fleetButton.Selected = false;
							}
							fleetButtons[i].Selected = true;
							pressedInWindow = false;
						}
					}
					if (gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Ships.Count > 8 && shipSelectorScrollBar.MouseUp(x, y))
					{
						gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.ShipIndex = shipSelectorScrollBar.TopIndex;
						LoadSelectedFleetInfoIntoUI(gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
						shipSelectorScrollBar.TopIndex = gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.ShipIndex;
						pressedInWindow = false;
					}
					for (int i = 0; i < shipScrollBars.Length; i++)
					{
						if (shipScrollBars[i].MouseUp(x, y))
						{
							gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Ships[gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.GetShipsForDisplay()[i]] = shipScrollBars[i].TopIndex;
							pressedInWindow = false;
						}
					}
					if (x >= gameMain.ScreenWidth - 207 && y < 460)
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

				StarSystem selectedSystem = gameMain.Galaxy.GetStarAtPoint(pointClicked);
				if (selectedSystem != null && selectedSystem == gameMain.EmpireManager.CurrentEmpire.SelectedSystem)
				{
					return;
				}
				gameMain.EmpireManager.CurrentEmpire.SelectedSystem = selectedSystem;

				if (selectedSystem != null)
				{
					gameMain.EmpireManager.CurrentEmpire.LastSelectedSystem = selectedSystem;
					gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = null;
					LoadSystemInfoIntoUI(selectedSystem);
					systemView.LoadSystem();
					return;
				}

				/*FleetGroup selectedFleetGroup = gameMain.EmpireManager.GetFleetsAtPoint(whichGridCellClicked.X, whichGridCellClicked.Y);
				gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup = selectedFleetGroup;

				if (selectedFleetGroup != null)
				{
					selectedFleetGroup.ShipIndex = 0;
					LoadFleetInfoIntoUI(selectedFleetGroup);
					return;
				}*/
				camera.CenterCamera(pointClicked.X, pointClicked.Y, camera.ZoomDistance);
			}
			else if (whichButton == 2)
			{
				if (gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup != null && gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.Empire == gameMain.EmpireManager.CurrentEmpire)
				{
					gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.FleetToSplit.ConfirmPath();
					gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SplitFleet(gameMain.EmpireManager.CurrentEmpire, gameMain.Galaxy.GetGridCells());
					LoadFleetInfoIntoUI(gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup);
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
			camera.MouseWheel(direction, x, y);
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
		}

		private void LoadSystemInfoIntoUI(StarSystem system)
		{
			if (system.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
			{
				int maxVisible = (system.Planets.Count < 6 ? system.Planets.Count : 6);
				int x = gameMain.ScreenWidth - 240;
				systemScrollBar.TopIndex = 0;
				if (system.Planets.Count <= 6)
				{
					systemScrollBar.SetAmountOfItems(10);
					systemScrollBar.SetEnabledState(false);
				}
				else
				{
					systemScrollBar.SetAmountOfItems(system.Planets.Count);
					systemScrollBar.SetEnabledState(true);
				}

				if (system.Planets.Count > 0)
				{
					systemButtons[0].Selected = true;
					gameMain.EmpireManager.CurrentEmpire.PlanetSelected = 0;
					LoadPlanetInfoIntoUI(system);

					if (system.Planets[0].Owner == gameMain.EmpireManager.CurrentEmpire)
					{
						planetScrollBars[INFRASTRUCTURE].TopIndex = system.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].InfrastructureAmount;
						planetScrollBars[WASTE].TopIndex = system.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].EnvironmentAmount;
						planetScrollBars[DEFENSE].TopIndex = system.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].DefenseAmount;
						planetScrollBars[RESEARCH].TopIndex = system.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ResearchAmount;
						planetScrollBars[CONSTRUCTION].TopIndex = system.Planets[gameMain.EmpireManager.CurrentEmpire.PlanetSelected].ConstructionAmount;
						LoadShipSprite(system.Planets[0]);
					}
				}
				for (int i = 0; i < systemButtons.Length; i++)
				{
					systemButtons[i].Active = true;
				}
			}
			else
			{
				gameMain.EmpireManager.CurrentEmpire.PlanetSelected = -1;
				for (int i = 0; i < 6; i++)
				{
					systemButtons[i].Active = false;
				}
				systemScrollBar.SetAmountOfItems(10);
				systemScrollBar.SetEnabledState(false);
			}
		}

		private void LoadPlanetInfoIntoUI(StarSystem system)
		{
			int selectedPlanetIter = gameMain.EmpireManager.CurrentEmpire.PlanetSelected;
			if (selectedPlanetIter >= 0 && system.Planets.Count > selectedPlanetIter)
			{
				Planet selectedPlanet = system.Planets[selectedPlanetIter];
				planetOwner.SetText(selectedPlanet.Owner == null ? "Unowned" : "Owned by " + selectedPlanet.Owner.EmpireName);
				planetOwner.SetColor(selectedPlanet.Owner == null ? System.Drawing.Color.White : selectedPlanet.Owner.EmpireColor);

				int maxPopLabels = selectedPlanet.Races.Count > 4 ? 4 : selectedPlanet.Races.Count;
				racePopLabels = new Label[maxPopLabels];
				for (int i = 0; i < maxPopLabels; i++)
				{
					if (i < 3)
					{
						racePopLabels[i] = new Label(selectedPlanet.Races[i].RaceName + " - " + selectedPlanet.GetRacePopulation(selectedPlanet.Races[i]), 0, 0);
						racePopLabels[i].Move((int)((gameMain.ScreenWidth - 252) - racePopLabels[i].GetWidth()), 320 + (i * 20));
					}
					else
					{
						if (selectedPlanet.Races.Count == 4)
						{
							racePopLabels[i] = new Label(selectedPlanet.Races[i].RaceName + " - " + selectedPlanet.GetRacePopulation(selectedPlanet.Races[i]), 0, 0);
							racePopLabels[i].Move((int)((gameMain.ScreenWidth - 252) - racePopLabels[i].GetWidth()), 320 + (i * 20));
						}
						else
						{
							float totalRemainingPop = 0.0f;
							for (int j = 3; j < selectedPlanet.Races.Count; j++)
							{
								totalRemainingPop += (selectedPlanet.GetRacePopulation(selectedPlanet.Races[j]));
							}
							racePopLabels[i] = new Label("Others - " + totalRemainingPop, 0, 0);
							racePopLabels[i].Move((int)((gameMain.ScreenWidth - 252) - racePopLabels[i].GetWidth()), 320 + (i * 20));
						}
					}
				}
			}
			else
			{
				planetOwner.SetText("Unknown");
				planetOwner.SetColor(System.Drawing.Color.White);
				racePopLabels = new Label[0];
			}
		}

		private void LoadFleetInfoIntoUI(FleetGroup fleetGroup)
		{
			int maxVisible = fleetGroup.Fleets.Count > 4 ? 4 : fleetGroup.Fleets.Count;
			fleetButtons = new Button[maxVisible];
			int x = gameMain.ScreenWidth - 200;
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
			gameMain.EmpireManager.CurrentEmpire.FleetSelected = 0;
			gameMain.EmpireManager.CurrentEmpire.SelectedFleetGroup.SelectFleet(0);
			LoadSelectedFleetInfoIntoUI(fleetGroup);
		}

		private void LoadSelectedFleetInfoIntoUI(FleetGroup fleetGroup)
		{
			Fleet selectedFleet = fleetGroup.Fleets[gameMain.EmpireManager.CurrentEmpire.FleetSelected];
			bool isEnabled = selectedFleet.Empire == gameMain.EmpireManager.CurrentEmpire;
			int x = gameMain.ScreenWidth - 203;
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
			shipSelectorScrollBar.SetAmountOfItems(fleetGroup.Fleets[gameMain.EmpireManager.CurrentEmpire.FleetSelected].Ships.Count);
		}

		private void LoadShipSprite(Planet planet)
		{
			/*if (planet.Owner == null || planet.Owner != gameMain.EmpireManager.CurrentEmpire)
			{
				return;
			}
			if (planet.ShipBeingBuilt == null)
			{
				shipSprite = gameMain.DrawingManagement.GetSprite(SpriteName.CancelBackground);
			}
			shipSprite = planet.Owner.EmpireRace.GetShip(planet.ShipBeingBuilt.Size, planet.ShipBeingBuilt.WhichStyle);
			if (planet.ShipBeingBuilt.Size % 2 == 0)
			{
				shipSprite.SetScale(1.0f, 1.0f);
				shipSprite.SetPosition(gameMain.ScreenWidth - 125 - (shipSprite.Width / 2), 275 - (shipSprite.Height / 2));
			}
			else
			{
				shipSprite.SetScale((shipSprite.Width - 16) / shipSprite.Width, (shipSprite.Height - 16) / shipSprite.Height);
				shipSprite.SetPosition(gameMain.ScreenWidth - 117 - (shipSprite.Width / 2), 283 - (shipSprite.Height / 2));
			}*/
		}
	}
}
