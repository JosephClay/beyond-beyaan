using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class FleetListScreen : ScreenInterface
	{
		private GameMain gameMain;

		private int x;
		private int y;

		Label fleetLabel;
		Label shipLabel;
		Label shipNameLabel;
		Label sizeLabel;
		/*Label engineLabel;
		Label computerLabel;
		Label armorLabel;
		Label shieldLabel;
		Label weaponLabel;
		Label mountsLabel;
		Label shotsLabel;
		Label specs;*/

		Button[] fleetButtons;
		Button[] shipButtons;
		/*Button scrapFleet;
		Button scrapShip;*/
		Button showOurFleets;
		Button showOtherFleets;

		/*List<Fleet> ownedFleets;
		List<Fleet> otherFleets;
		List<Fleet> allFleets;
		List<Fleet> whichFleets;*/

		SingleLineTextBox nameText;
		SingleLineTextBox sizeText;
		/*SingleLineTextBox engineText;
		SingleLineTextBox computerText;
		SingleLineTextBox armorText;
		SingleLineTextBox shieldText;
		SingleLineTextBox[] weaponTexts;
		SingleLineTextBox[] mountsTexts;
		SingleLineTextBox[] shotsTexts;

		int weaponIndex;*/
		/*int fleetIndex;
		int shipIndex;
		int selectedFleet;
		int selectedShip;*/

		//Fleet fleetSelected;
		//Fleet hoveringFleet;

		//Ship shipSelected;
		//GorgonLibrary.Graphics.Sprite shipSprite;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			x = (gameMain.ScreenWidth / 2) - 400;
			y = (gameMain.ScreenHeight / 2) - 300;

			fleetLabel = new Label("Fleets", x + 2, y + 2);
			shipLabel = new Label("Ships in this fleet", x + 202, y + 2);
			//specs = new Label("Specifications", x + 200, y + 400);
			shipNameLabel = new Label("Name:", x + 200, y + 425);
			sizeLabel = new Label("Size", x + 200, y + 460);
			/*engineLabel = new Label("Engine:", x + 200, y + 475);
			computerLabel = new Label("Computer:", x + 200, y + 500);
			armorLabel = new Label("Armor:", x + 200, y + 525);
			shieldLabel = new Label("Shield:", x + 200, y + 550);
			weaponLabel = new Label("Weapons", x + 452, y + 402);
			mountsLabel = new Label("Mounts", x + 675, y + 402);
			shotsLabel = new Label("Shots", x + 730, y + 402);*/

			/*fleetButtons = new Button[11];
			shipButtons = new Button[11];

			for (int i = 0; i < fleetButtons.Length; i++)
			{
				fleetButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 2, y + 25 + (i * 25), 182, 25);
				shipButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 202, y + 25 + (i * 25), 182, 25);
			}

			//scrapFleet = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Scrap this fleet", x + 2, y + 304, 182, 25);
			showOurFleets = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Show our fleets", x + 2, y + 332, 182, 25);
			//scrapShip = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Scrap this ship", x + 202, y + 304, 182, 25);
			showOtherFleets = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Show other fleets", x + 202, y + 332, 182, 25);*/
			
			List<SpriteName> textBox = new List<SpriteName>()
			{
				SpriteName.TextTL,
				SpriteName.TextTC,
				SpriteName.TextTR,
				SpriteName.TextCL,
				SpriteName.TextCC,
				SpriteName.TextCR,
				SpriteName.TextBL,
				SpriteName.TextBC,
				SpriteName.TextBR
			};
			nameText = new SingleLineTextBox(x + 275, y + 425, 150, 35, textBox);
			sizeText = new SingleLineTextBox(x + 275, y + 460, 150, 35, textBox);
			/*engineText = new SingleLineTextBox(x + 275, y + 475, 150, 23, SpriteName.MiniBackgroundButton);
			computerText = new SingleLineTextBox(x + 275, y + 500, 150, 23, SpriteName.MiniBackgroundButton);
			armorText = new SingleLineTextBox(x + 275, y + 525, 150, 23, SpriteName.MiniBackgroundButton);
			shieldText = new SingleLineTextBox(x + 275, y + 550, 150, 23, SpriteName.MiniBackgroundButton);

			weaponTexts = new SingleLineTextBox[6];
			mountsTexts = new SingleLineTextBox[6];
			shotsTexts = new SingleLineTextBox[6];

			for (int i = 0; i < weaponTexts.Length; i++)
			{
				weaponTexts[i] = new SingleLineTextBox(x + 450, y + 425, 200, 23, SpriteName.MiniBackgroundButton);
				mountsTexts[i] = new SingleLineTextBox(x + 675, y + 425, 40, 23, SpriteName.MiniBackgroundButton);
				shotsTexts[i] = new SingleLineTextBox(x + 730, y + 425, 40, 23, SpriteName.MiniBackgroundButton);
			}*/
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			/*gameMain.DrawGalaxyBackground();

			drawingManagement.DrawSprite(SpriteName.ControlBackground, (gameMain.ScreenWidth / 2) - 400, (gameMain.ScreenHeight / 2) - 300, 255, 800, 600, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.Screen, (gameMain.ScreenWidth / 2), (gameMain.ScreenHeight / 2) - 300, 255, 399, 399, System.Drawing.Color.White);

			DrawGalaxyPreview(drawingManagement);

			fleetLabel.Draw();
			shipLabel.Draw();

			int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].Draw(drawingManagement);
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].Draw(drawingManagement);
				}
			}

			//scrapFleet.Draw(drawingManagement);
			showOurFleets.Draw(drawingManagement);
			//scrapShip.Draw(drawingManagement);
			showOtherFleets.Draw(drawingManagement);

			if (selectedFleet != -1 && selectedShip != -1)
			{
				engineLabel.Draw();
				computerLabel.Draw();
				armorLabel.Draw();
				shieldLabel.Draw();
				shipNameLabel.Draw();
				sizeLabel.Draw();
				weaponLabel.Draw();
				mountsLabel.Draw();
				shotsLabel.Draw();
				specs.Draw();

				GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
				gameMain.ShipShader.Parameters["EmpireColor"].SetValue(whichFleets[selectedFleet].Empire.ConvertedColor);
				shipSprite.Draw();
				GorgonLibrary.Gorgon.CurrentShader = null;
				//drawingManagement.DrawSprite(SpriteName.Corvette, (gameMain.ScreenWidth / 2) - 398, (gameMain.ScreenHeight / 2) + 98, 255, 180, 180, System.Drawing.Color.White);

				nameText.Draw(drawingManagement);
				sizeText.Draw(drawingManagement);
				/*engineText.Draw(drawingManagement);
				computerText.Draw(drawingManagement);
				armorText.Draw(drawingManagement);
				shieldText.Draw(drawingManagement);*/

				/*maxVisible = shipSelected.weapons.Count > weaponTexts.Length ? weaponTexts.Length : shipSelected.weapons.Count;

				for (int i = 0; i < maxVisible; i++)
				{
					weaponTexts[i].Draw(drawingManagement);
					mountsTexts[i].Draw(drawingManagement);
					shotsTexts[i].Draw(drawingManagement);
				}
			}*/
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			hoveringFleet = null;
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime))
				{
					hoveringFleet = whichFleets[i + fleetIndex];
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
				}
			}
			//scrapFleet.MouseHover(mouseX, mouseY, frameDeltaTime);
			//scrapShip.MouseHover(mouseX, mouseY, frameDeltaTime);
			showOtherFleets.MouseHover(mouseX, mouseY, frameDeltaTime);
			showOurFleets.MouseHover(mouseX, mouseY, frameDeltaTime);*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].MouseDown(x, y);
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					shipButtons[i].MouseDown(x, y);
				}
			}
			//scrapFleet.MouseDown(x, y);
			//scrapShip.MouseDown(x, y);
			showOtherFleets.MouseDown(x, y);
			showOurFleets.MouseDown(x, y);*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				if (fleetButtons[i].MouseUp(x, y))
				{
					foreach (Button button in fleetButtons)
					{
						button.Selected = false;
					}
					selectedFleet = i + fleetIndex;
					fleetSelected = whichFleets[i + fleetIndex];
					selectedShip = -1;
					UpdateLabels();
					fleetButtons[i].Selected = true;
					return;
				}
			}
			if (selectedFleet >= 0)
			{
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				for (int i = 0; i < maxVisible; i++)
				{
					if (shipButtons[i].MouseUp(x, y))
					{
						foreach (Button button in shipButtons)
						{
							button.Selected = false;
						}
						selectedShip = i + shipIndex;
						UpdateShipSpecs();
						shipButtons[i].Selected = true;
						return;
					}
				}
			}
			if (scrapFleet.MouseUp(x, y))
			{
			}
			if (scrapShip.MouseUp(x, y))
			{
			}
			if (showOtherFleets.MouseUp(x, y))
			{
				showOtherFleets.Selected = !showOtherFleets.Selected;
				UpdateList();
				UpdateLabels();
			}
			if (showOurFleets.MouseUp(x, y))
			{
				showOurFleets.Selected = !showOurFleets.Selected;
				UpdateList();
				UpdateLabels();
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void Resize()
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(ScreenEnum.Galaxy);
			}
			/*if (e.Key == KeyboardKeys.Space)
			{
				gameMain.ToggleSitRep();
			}*/
		}

		public void LoadScreen()
		{
			/*Empire currentEmpire = gameMain.empireManager.CurrentEmpire;
			ownedFleets = currentEmpire.FleetManager.GetFleets();
			otherFleets = currentEmpire.VisibleFleets;
			allFleets = new List<Fleet>();
			foreach (Fleet fleet in ownedFleets)
			{
				allFleets.Add(fleet);
			}
			foreach (Fleet fleet in otherFleets)
			{
				allFleets.Add(fleet);
			}
			showOtherFleets.Selected = true;
			showOurFleets.Selected = true;

			fleetIndex = 0;
			shipIndex = 0;
			selectedFleet = -1;
			selectedShip = -1;
			fleetSelected = null;

			UpdateList();
			UpdateLabels();*/
		}

		/*public void UpdateLabels()
		{
			int maxVisible = (whichFleets.Count > fleetButtons.Length ? fleetButtons.Length : whichFleets.Count);
			for (int i = 0; i < maxVisible; i++)
			{
				fleetButtons[i].SetButtonText(whichFleets[i + fleetIndex].Empire.EmpireName);
			}
			if (selectedFleet >= 0)
			{
				foreach (Button button in fleetButtons)
				{
					button.Selected = false;
				}
				fleetButtons[selectedFleet - fleetIndex].Selected = true;
				maxVisible = (whichFleets[selectedFleet + fleetIndex].Ships.Count > shipButtons.Length ? shipButtons.Length : whichFleets[selectedFleet + fleetIndex].Ships.Count);
				int i = 0;
				int j = 0;
				foreach (KeyValuePair<Ship, int> ship in whichFleets[selectedFleet + fleetIndex].Ships)
				{
					if (i >= shipIndex)
					{
						if (i >= (shipIndex + maxVisible))
						{
							break;
						}
						shipButtons[j].SetButtonText(ship.Key.Name + " x " + ship.Value);
						j++;
					}
					i++;
				}
			}
		}

		public void UpdateList()
		{
			fleetSelected = null;
			selectedFleet = -1;
			selectedShip = -1;
			fleetIndex = 0;
			shipIndex = 0;
			foreach (Button button in shipButtons)
			{
				button.Selected = false;
			}
			foreach (Button button in fleetButtons)
			{
				button.Selected = false;
			}

			if (showOurFleets.Selected && showOtherFleets.Selected)
			{
				whichFleets = allFleets;
			}
			else if (!showOtherFleets.Selected && showOurFleets.Selected)
			{
				whichFleets = ownedFleets;
			}
			else if (showOtherFleets.Selected && !showOurFleets.Selected)
			{
				whichFleets = otherFleets;
			}
			else
			{
				whichFleets = new List<Fleet>();
			}
		}*/

		public void UpdateShipSpecs()
		{
			/*if (selectedFleet != -1)
			{
				shipSelected = whichFleets[selectedFleet - fleetIndex].OrderedShips[selectedShip + shipIndex];
				nameText.SetString(shipSelected.Name);
				sizeText.SetString(Utility.ShipSizeToString(shipSelected.Size));
				engineText.SetString(shipSelected.engine.GetName());
				computerText.SetString(shipSelected.computer.GetName());
				armorText.SetString(shipSelected.armor.GetName());
				shieldText.SetString(shipSelected.shield.GetName());
				weaponIndex = 0;
				UpdateWeaponSpecs();
				LoadShipSprite(whichFleets[selectedFleet - fleetIndex].Empire, shipSelected);
			}*/
		}

		public void UpdateWeaponSpecs()
		{
			/*int maxVisible = shipSelected.weapons.Count > weaponTexts.Length ? weaponTexts.Length : shipSelected.weapons.Count;

			for (int i = 0; i < maxVisible; i++)
			{
				weaponTexts[i].SetString(shipSelected.weapons[i + weaponIndex].GetName());
				mountsTexts[i].SetString(shipSelected.weapons[i + weaponIndex].Mounts.ToString());
				shotsTexts[i].SetString(shipSelected.weapons[i + weaponIndex].Ammo.ToString());
			}*/
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			/*List<StarSystem> systems = gameMain.galaxy.GetAllStars();

			foreach (StarSystem system in systems)
			{
				int x = (gameMain.ScreenWidth / 2) + (int)(386.0f * (system.X / (float)gameMain.galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (system.Y / (float)gameMain.galaxy.GalaxySize));

				if (system.Type == StarType.BLACK_HOLE)
				{
					drawingManagement.DrawSprite(SpriteName.BlackHole, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
				}
				else
				{
					GorgonLibrary.Gorgon.CurrentShader = gameMain.StarShader;
					gameMain.StarShader.Parameters["StarColor"].SetValue(system.StarColor);
					drawingManagement.DrawSprite(SpriteName.Star, x, y, 255, 6 * system.Size, 6 * system.Size, System.Drawing.Color.White);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}
			}

			foreach (Fleet fleet in whichFleets)
			{
				int x = (gameMain.ScreenWidth / 2) + (int)(386.0f * (fleet.GalaxyX / (float)gameMain.galaxy.GalaxySize));
				int y = ((gameMain.ScreenHeight / 2) - 300) + (int)(386.0f * (fleet.GalaxyY / (float)gameMain.galaxy.GalaxySize));

				if (fleet == fleetSelected || fleet == hoveringFleet)
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 32, 32, fleet.Empire.EmpireColor);
				}
				else
				{
					drawingManagement.DrawSprite(SpriteName.Fleet, x, y, 255, 16, 16, fleet.Empire.EmpireColor);
				}
			}*/
		}

		/*private void LoadShipSprite(Empire empire, ShipDesign ship)
		{
			shipSprite = empire.EmpireRace.ShipSprites[ship.Size][ship.WhichStyle];

			if (ship.Size % 2 == 0)
			{
				shipSprite.SetScale(1.0f, 1.0f);
				shipSprite.SetPosition(x + 85 - (shipSprite.Width / 2), y + 485 - (shipSprite.Height / 2));
			}
			else
			{
				shipSprite.SetScale((shipSprite.Width - 16) / shipSprite.Width, (shipSprite.Height - 16) / shipSprite.Height);
				shipSprite.SetPosition(x + 93 - (shipSprite.Width / 2), y + 493 - (shipSprite.Height / 2));
			}
		}*/
	}
}
