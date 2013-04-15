using GorgonLibrary.Graphics;

namespace Beyond_Beyaan.Screens
{
	class SquadronListWindow : WindowInterface
	{
		private InvisibleStretchButton[] squadronButtons;
		//private InvisibleStretchButton[] shipButtons;
		private ScrollBar squadronScrollBar;
		//private ScrollBar shipScrollBar;
		private Label[] squadronNames;
		private Button transportButton;
		//private Button orbitButton;
		//private Button clearButton;
		private Button newSquadronButton;
		private Sprite fleetIcon;

		private int maxVisible;
		//private int maxShipVisible;
		private bool squadronScrollBarVisible;
		//private bool shipScrollBarVisible;
		//private int offset;

		private SquadronGroup selectedSquadronGroup;

		//private StretchableImage fleetBorder;
		//private bool selectAll;

		//private bool showingTransferToShipWindow;
		//private TransferToShipWindow transferToShipWindow;

		private int centerY;

		public SquadronListWindow(int centerY, GameMain gameMain)
			: base(0, centerY, 10, 10, string.Empty, gameMain, false)
		{
			this.gameMain = gameMain;

			this.centerY = centerY;

			//backGroundImage = new StretchableImage(x, y, 275, 300, 30, 13, DrawingManagement.BoxBorderBG);

			squadronScrollBar = new ScrollBar(xPos - 195, 10, 16, 80, 4, 4, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont());
			//shipScrollBar = new ScrollBar(xPos - 160, 165, 16, 528, 7, 10, true, false, DrawingManagement.HorizontalScrollBar);
			//fleetBorder = new StretchableImage(xPos - 410, 0, 240, 140, 30, 13, DrawingManagement.BoxBorderBG);
			transportButton = new Button(SpriteName.TransferButtonBG, SpriteName.TransferButtonFG, string.Empty, 10, centerY, 75, 35, gameMain.FontManager.GetDefaultFont());
			newSquadronButton = new Button(SpriteName.NewSquadronButtonBG, SpriteName.NewSquadronButtonFG, string.Empty, 95, centerY, 75, 35, gameMain.FontManager.GetDefaultFont());
			//orbitButton = new Button(SpriteName.OrbitButtonBG, SpriteName.OrbitButtonFG, string.Empty, xPos - 325, 148, 75, 35);
			//clearButton = new Button(SpriteName.CancelMovementBG, SpriteName.CancelMovementFG, string.Empty, xPos - 245, 148, 75, 35);
			//transportButton.Active = false;
			//orbitButton.Active = false;
			//clearButton.Active = false;

			//transferToShipWindow = new TransferToShipWindow(centerX, centerY + 200, gameMain, DoneTransferToShip);

			//transportButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.DrawingManagement.GetDefaultFont(), "Transport people", "transportPeopleToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			//orbitButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.DrawingManagement.GetDefaultFont(), "Scrap selected ships", "scrapSelectedShipsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			//clearButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.DrawingManagement.GetDefaultFont(), "Clear movement orders", "clearMovementOrdersToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			backGroundImage.Draw(drawingManagement);
			transportButton.Draw(drawingManagement);
			newSquadronButton.Draw(drawingManagement);

			for (int i = 0; i < maxVisible; i++)
			{
				squadronButtons[i].Draw(drawingManagement);
				squadronNames[i].Draw(drawingManagement);
				//squadronButtons[i] = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, 0, centerY - ((maxVisible / 2) * 35) - 15 + (i * 35), 235, 35, 10, 10);
				fleetIcon = selectedSquadronGroup.Squadrons[i + squadronScrollBar.TopIndex].Empire.EmpireRace.GetFleetIcon();
				fleetIcon.SetPosition(20, (int)(centerY - ((maxVisible / 2.0) * 35) + (i * 35) + 2));
				fleetIcon.Color = selectedSquadronGroup.Squadrons[i + squadronScrollBar.TopIndex].Empire.EmpireColor;
				fleetIcon.SetScale(1, 1);
				fleetIcon.Draw();
			}

			if (squadronScrollBarVisible)
			{
				squadronScrollBar.Draw(drawingManagement);
			}
			/*fleetBorder.Draw(drawingManagement);
			for (int i = 0; i < maxFleetVisible; i++)
			{
				fleetButtons[i].Draw(drawingManagement);
				fleetLabels[i].Draw();
			}
			List<ShipInstance> ships = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships;
			foreach (InvisibleStretchButton button in shipButtons)
			{
				button.Draw(drawingManagement);
			}
			GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
			gameMain.ShipShader.Parameters["EmpireColor"].SetValue(selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Empire.ConvertedColor);
			for (int i = 0; i < maxShipVisible; i++)
			{
				ShipInstance ship = ships[i + shipScrollBar.TopIndex];
				GorgonLibrary.Graphics.Sprite sprite = ship.BaseShipDesign.ShipClass.Sprites[ship.BaseShipDesign.WhichStyle];
				if (ship.BaseShipDesign.ShipClass.Size % 2 == 0)
				{
					sprite.SetScale((64 / sprite.Width) * (sprite.Width / 160), (64 / sprite.Height) * (sprite.Width / 160));
				}
				else
				{
					sprite.SetScale((64 / (sprite.Width - 16.0f)) * ((sprite.Width - 16.0f) / 160), (64 / (sprite.Height - 16.0f)) * ((sprite.Width - 16.0f) / 160));
				}
				if (i % 2 == 1)
				{
					sprite.SetPosition(xPos + ((i / 2) * 80) - 120 - (sprite.ScaledWidth / 2), 121 - (sprite.ScaledHeight / 2));
				}
				else
				{
					sprite.SetPosition(xPos + ((i / 2) * 80) - 120 - (sprite.ScaledWidth / 2), 40 - (sprite.ScaledHeight / 2));
				}
				sprite.Draw();
			}
			GorgonLibrary.Gorgon.CurrentShader = null;
			/*if (shipScrollBarVisible)
			{
				shipScrollBar.Draw(drawingManagement);
			}*/
			/*if (fleetScrollBarVisible)
			{
				fleetScrollBar.Draw(drawingManagement);
			}
			//transportButton.Draw(drawingManagement);
			//orbitButton.Draw(drawingManagement);
			//clearButton.Draw(drawingManagement);
			if (showingTransferToShipWindow)
			{
				transferToShipWindow.DrawWindow(drawingManagement);
			}

			//transportButton.DrawToolTip(drawingManagement);
			//orbitButton.DrawToolTip(drawingManagement);
			//clearButton.DrawToolTip(drawingManagement);*/
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			for (int i = 0; i < maxVisible; i++)
			{
				result = squadronButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (squadronScrollBarVisible && squadronScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshDisplay();
				result = true;
			}
			/*if (showingTransferToShipWindow)
			{
				transferToShipWindow.MouseHover(x, y, frameDeltaTime);
				return true;
			}
			foreach (InvisibleStretchButton button in fleetButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			foreach (InvisibleStretchButton button in shipButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			if (fleetScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				for (int i = 0; i < maxFleetVisible; i++)
				{
					fleetLabels[i].SetText(selectedFleetGroup.Fleets[i + fleetScrollBar.TopIndex].Empire.EmpireName);
					fleetButtons[i].Selected = false;
				}
				if (selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex >= 0 && selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex < maxFleetVisible)
				{
					fleetButtons[selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex].Selected = true;
				}
				result = true;
			}
			/*if (shipScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshShips();
				result = true;
			}*/
			//result = transportButton.MouseHover(x, y, frameDeltaTime) || result;
			//result = orbitButton.MouseHover(x, y, frameDeltaTime) || result;
			//result = clearButton.MouseHover(x, y, frameDeltaTime) || result;
			/*if (x >= xPos - 420 && x < (xPos - 420) + windowWidth && y < windowHeight)
			{
				result = true;
			}*/
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			foreach (InvisibleStretchButton button in squadronButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			if (squadronScrollBarVisible && squadronScrollBar.MouseDown(x, y))
			{
				result = true;
			}
			/*if (showingTransferToShipWindow)
			{
				return transferToShipWindow.MouseDown(x, y);
			}
			foreach (InvisibleStretchButton button in fleetButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			if (gameMain.empireManager.CurrentEmpire == selectedFleetGroup.SelectedSquadron.Empire)
			{
				foreach (InvisibleStretchButton button in shipButtons)
				{
					result = button.MouseDown(x, y) || result;
				}
			}
			result = fleetScrollBar.MouseDown(x, y) || result;
			//result = shipScrollBar.MouseDown(x, y) || result;
			//result = transportButton.MouseDown(x, y) || result;
			//result = clearButton.MouseDown(x, y) || result;
			if (x >= xPos - 420 && x < (xPos - 420) + windowWidth && y < windowHeight)
			{
				result = true;
			}*/
			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			/*if (showingTransferToShipWindow)
			{
				return transferToShipWindow.MouseUp(x, y);
			}*/
			for (int i = 0; i < maxVisible; i++)
			{
				if (squadronButtons[i].MouseUp(x, y))
				{
					selectedSquadronGroup.SelectSquadron(i + squadronScrollBar.TopIndex, gameMain.empireManager.CurrentEmpire, gameMain.Input.Keyboard.KeyStates[GorgonLibrary.InputDevices.KeyboardKeys.ControlKey] == GorgonLibrary.InputDevices.KeyState.Down, gameMain.Input.Keyboard.KeyStates[GorgonLibrary.InputDevices.KeyboardKeys.ShiftKey] == GorgonLibrary.InputDevices.KeyState.Down);
					RefreshDisplay();
					result = true;
					/*if (!fleetButtons[i].Selected)
					{
						selectAll = true;
						selectedFleetGroup.FleetIndex = i + fleetScrollBar.TopIndex;
						selectedFleetGroup.SelectSquadron(selectedFleetGroup.FleetIndex, gameMain.empireManager.CurrentEmpire);
						foreach (InvisibleStretchButton button in fleetButtons)
						{
							button.Selected = false;
						}
						fleetButtons[i].Selected = true;
						RefreshDisplay();
						result = true;
					}
					else if (selectedFleetGroup.SelectedSquadron.Empire == gameMain.empireManager.CurrentEmpire)
					{
						selectedFleetGroup.SquadronToSplit.ClearShips();
						if (!selectAll)
						{
							selectedFleetGroup.SquadronToSplit.AddShipsThemselves(selectedFleetGroup.SelectedSquadron.Ships);
						}
						selectAll = !selectAll;
						RefreshShips();
					}*/
				}
			}
			if (squadronScrollBarVisible && squadronScrollBar.MouseUp(x, y))
			{
				RefreshDisplay();
				result = true;
			}
			/*if (fleetScrollBar.MouseUp(x, y))
			{
				for (int i = 0; i < maxFleetVisible; i++)
				{
					fleetLabels[i].SetText(selectedFleetGroup.Fleets[i + fleetScrollBar.TopIndex].Empire.EmpireName);
					fleetButtons[i].Selected = false;
				}
				if (selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex >= 0 && selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex < maxFleetVisible)
				{
					fleetButtons[selectedFleetGroup.FleetIndex - fleetScrollBar.TopIndex].Selected = true;
				}
				result = true;
			}
			if (gameMain.empireManager.CurrentEmpire == selectedFleetGroup.SelectedSquadron.Empire)
			{
				for (int i = 0; i < maxShipVisible; i++)
				{
					if (shipButtons[i].MouseUp(x, y))
					{
						if (shipButtons[i].Selected)
						{
							selectedFleetGroup.SquadronToSplit.SubtractShip(selectedFleetGroup.SelectedSquadron.Ships[i + (shipScrollBar.TopIndex * 2)]);
						}
						else
						{
							selectedFleetGroup.SquadronToSplit.AddShipItself(selectedFleetGroup.SelectedSquadron.Ships[i + (shipScrollBar.TopIndex * 2)]);
						}
						RefreshShips();
					}
				}
			}
			/*if (transportButton.MouseUp(x, y))
			{
				transferToShipWindow.LoadTransfer(selectedFleetGroup.AdjacentSystem, selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex]);
				showingTransferToShipWindow = true;
			}*/
			/*if (clearButton.MouseUp(x, y))
			{
				gameMain.empireManager.CurrentEmpire.ClearFleetMovement(selectedFleetGroup.SelectedSquadron);
				gameMain.empireManager.CurrentEmpire.SelectedFleetGroup = gameMain.empireManager.GetSquadronsAtPoint(selectedFleetGroup.AdjacentSystem, selectedFleetGroup.SelectedSquadron.FleetLocation.X, selectedFleetGroup.SelectedSquadron.FleetLocation.Y);
				LoadFleetGroup();
			}*/
			/*if (shipScrollBar.MouseUp(x, y))
			{
				RefreshShips();
				result = true;
			}*/
			/*if (x >= xPos - 420 && x < (xPos - 420) + windowWidth && y < windowHeight)
			{
				result = true;
			}*/
			return result;
		}

		public void LoadFleetGroup()
		{
			selectedSquadronGroup = gameMain.empireManager.CurrentEmpire.SelectedFleetGroup;

			maxVisible = (gameMain.ScreenHeight - 300) / 30;
			if (selectedSquadronGroup.Squadrons.Count < maxVisible)
			{
				maxVisible = selectedSquadronGroup.Squadrons.Count;
				squadronScrollBarVisible = false;
			}
			else
			{
				squadronScrollBarVisible = true;
			}

			backGroundImage = new StretchableImage(-35, (int)(centerY - ((maxVisible / 2.0) * 35) - 25), squadronScrollBarVisible ? 300 : 275, maxVisible * 35 + 70, 30, 13, DrawingManagement.BoxBorderBG);
			transportButton.MoveTo(squadronScrollBarVisible ? 95 : 70, (int)(centerY + ((maxVisible / 2.0) * 35)));
			newSquadronButton.MoveTo(squadronScrollBarVisible ? 180 : 155, (int)(centerY + ((maxVisible / 2.0) * 35)));
			squadronScrollBar = new ScrollBar(238, (int)(centerY - ((maxVisible / 2.0) * 35) - 15), 16, (maxVisible * 35) - 32, maxVisible, selectedSquadronGroup.Squadrons.Count, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont());

			squadronNames = new Label[maxVisible];
			squadronButtons = new InvisibleStretchButton[maxVisible];

			for (int i = 0; i < maxVisible; i++)
			{
				squadronButtons[i] = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, 0, (int)(centerY - ((maxVisible / 2.0) * 35) - 15 + (i * 35)), 235, 35, 10, 10, gameMain.FontManager.GetDefaultFont());
			}

			/*maxFleetVisible = selectedFleetGroup.Fleets.Count > 4 ? 4 : selectedFleetGroup.Fleets.Count;
			if (selectedFleetGroup.Fleets.Count > 4)
			{
				fleetScrollBarVisible = true;
				fleetScrollBar.SetAmountOfItems(selectedFleetGroup.Fleets.Count);
			}
			else
			{
				fleetScrollBarVisible = false;
			}
			fleetScrollBar.SetEnabledState(fleetScrollBarVisible);
			fleetButtons = new InvisibleStretchButton[maxFleetVisible];
			fleetLabels = new Label[maxFleetVisible];
			for (int i = 0; i < maxFleetVisible; i++)
			{
				fleetButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos - 400, 10 + (i * 30), 200, 30, 30, 13);
				fleetLabels[i] = new Label(selectedFleetGroup.Fleets[i].Empire.EmpireName, xPos - 390, 12 + (i * 30));
			}
			selectedFleetGroup.FleetIndex = 0;
			selectedFleetGroup.SelectSquadron(selectedFleetGroup.FleetIndex, gameMain.empireManager.CurrentEmpire);
			fleetButtons[selectedFleetGroup.FleetIndex].Selected = true;
			selectAll = true;*/
			RefreshDisplay();
		}

		private void RefreshDisplay()
		{
			for (int i = 0; i < maxVisible; i++)
			{
				squadronNames[i] = new Label(selectedSquadronGroup.Squadrons[i + squadronScrollBar.TopIndex].Name, 40, (int)(centerY - ((maxVisible / 2.0) * 35) - 8 + (i * 35)), gameMain.FontManager.GetDefaultFont());
				if (selectedSquadronGroup.SelectedSquadron.Contains(selectedSquadronGroup.Squadrons[i + squadronScrollBar.TopIndex]))
				{
					squadronButtons[i].Selected = true;
				}
				else
				{
					squadronButtons[i].Selected = false;
				}
			}
			/*List<ShipInstance> ships = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships;
			maxShipVisible = ships.Count > 14 ? 14 : ships.Count;
			if (selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships.Count > 14)
			{
				shipScrollBarVisible = true;
				shipScrollBar.SetAmountOfItems(selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships.Count / 2 + selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships.Count % 2);
			}
			else
			{
				shipScrollBarVisible = false;
			}
			shipScrollBar.TopIndex = 0;
			shipScrollBar.SetEnabledState(shipScrollBarVisible);

			RefreshShips();

			windowWidth = ((maxShipVisible / 2) + maxShipVisible % 2) * 80 + 290;
			if (windowWidth < 370)
			{
				windowWidth = 370;
			}
			windowHeight = 220;
			backGroundImage.MoveTo(xPos - 420, -20);
			backGroundImage.SetDimensions(windowWidth, windowHeight);
			/*clearButton.Active = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TravelNodes != null
				&& selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Empire == gameMain.empireManager.CurrentEmpire;
			//orbitButton.Active = selectedFleetGroup.AdjacentSystem != null && selectedFleetGroup.AdjacentSystem.SystemHaveInhabitablePlanets();
			transportButton.Active = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TransportCapacity > 0
				&& selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TransportCapacity > selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TotalPeopleInTransit 
				&& selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Empire == gameMain.empireManager.CurrentEmpire 
				&& (selectedFleetGroup.AdjacentSystem == null ? false : selectedFleetGroup.AdjacentSystem.EmpiresWithPlanetsInThisSystem.Contains(gameMain.empireManager.CurrentEmpire));*/
		}

		/*private void RefreshShips()
		{
			List<ShipInstance> selectedShips = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships;
			List<ShipInstance> splitShips = selectedFleetGroup.SquadronToSplit.Ships;

			int count = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Ships.Count;
			if (count > 14)
			{
				if (count % 2 == 1 && count - (shipScrollBar.TopIndex * 2) < 14)
				{
					maxShipVisible = 13;
				}
				else
				{
					maxShipVisible = 14;
				}
			}
			else
			{
				maxShipVisible = count;
			}

			shipButtons = new InvisibleStretchButton[maxShipVisible];
			for (int i = 0; i < maxShipVisible; i++)
			{
				if (i % 2 == 1)
				{
					shipButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + ((i / 2) * 80) - 160, 81, 80, 80, 30, 13);
				}
				else
				{
					shipButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + ((i / 2) * 80) - 160, 0, 80, 80, 30, 13);
				}
			}

			for (int i = 0; i < maxShipVisible; i++)
			{
				if (splitShips.Contains(selectedShips[i + (shipScrollBar.TopIndex * 2)]))
				{
					shipButtons[i].Selected = true;
				}
				else
				{
					shipButtons[i].Selected = false;
				}
			}
		}*/

		private void DoneTransferToShip()
		{
			//showingTransferToShipWindow = false;
			/*transportButton.Active = selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TransportCapacity > 0
				&& selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TransportCapacity > selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].TotalPeopleInTransit
				&& selectedFleetGroup.Fleets[selectedFleetGroup.FleetIndex].Empire == gameMain.empireManager.CurrentEmpire
				&& (selectedFleetGroup.AdjacentSystem == null ? false : selectedFleetGroup.AdjacentSystem.EmpiresWithPlanetsInThisSystem.Contains(gameMain.empireManager.CurrentEmpire));*/
		}
	}
}
