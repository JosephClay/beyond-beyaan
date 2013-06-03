using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class ColonizeWindow : WindowInterface
	{
		public delegate void ColonizeFunction(Planet planet);
		public delegate void DoneFunction();

		private ColonizeFunction colonizeFunction;
		private DoneFunction doneFunction;

		//private InvisibleStretchButton[] planetButtons;
		//private InvisibleStretchButton[] shipButtons;
		//private Label[] planetNumericNames;
		private Label systemNameLabel;
		private ScrollBar shipScrollBar;

		private Button colonizeButton;
		private Button doneButton;

		private StarSystem selectedSystem;
		private List<ShipInstance> colonyShips;
		private Squadron selectedFleet;

		//private int selectedPlanet;
		//private int selectedShip;

		//private int planetOffset;
		//private int shipOffset;

		//private int maxPlanetVisible;
		//private int maxShipVisible;

		//private bool[] colonizable;

		public ColonizeWindow(int centerX, int centerY, GameMain gameMain, ColonizeFunction colonizeFunction, DoneFunction doneFunction)
			: base(centerX, centerY, 10, 10, string.Empty, gameMain, false)
		{
			systemNameLabel = new Label(0, 0, gameMain.FontManager.GetDefaultFont());

			windowHeight = 290;

			shipScrollBar = new ScrollBar(0, 0, 16, 100, 10, 10, true, false, DrawingManagement.HorizontalScrollBar, gameMain.FontManager.GetDefaultFont());

			colonizeButton = new Button(SpriteName.ColonizeButtonBG, SpriteName.ColonizeButtonFG, string.Empty, centerX + 10, yPos + 235, 75, 35, gameMain.FontManager.GetDefaultFont());
			doneButton = new Button(SpriteName.PlanetDoneButtonBG, SpriteName.PlanetDoneButtonFG, string.Empty, centerX - 85, yPos + 235, 75, 35, gameMain.FontManager.GetDefaultFont());

			colonizeButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "Colonize planet with selected ship", "colonizePlanetWithSelectedShipToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			doneButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "Exit", "exitColonizePlanetWithSelectedShipToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			backGroundImage = new StretchableImage(0, 0, 40, 40, 60, 60, DrawingManagement.BorderBorderBG);

			this.colonizeFunction = colonizeFunction;
			this.doneFunction = doneFunction;
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			backGroundImage.Draw(drawingManagement);
			systemNameLabel.Draw();

			/*for (int i = 0; i < maxPlanetVisible; i++)
			{
				planetButtons[i].Draw(drawingManagement);
				GorgonLibrary.Graphics.Sprite planet = selectedSystem.Planets[i].PlanetType.Sprite;
				planet.SetPosition(xPos - planetOffset + (i * 70) + 15, yPos + 35);
				if (colonizable[i])
				{
					planet.Color = System.Drawing.Color.White;
				}
				else
				{
					planet.Color = System.Drawing.Color.FromArgb(255, 75, 75, 75);
				}
				planet.Draw();
				planetNumericNames[i].Draw();
				if (selectedSystem.Planets[i].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(selectedSystem.Planets[i].ConstructionBonus), xPos - planetOffset + (i * 70) + 10, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(selectedSystem.Planets[i].EnvironmentBonus), xPos - planetOffset + (i * 70) + 27, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(selectedSystem.Planets[i].EntertainmentBonus), xPos - planetOffset + (i * 70) + 44, yPos + 95, 255, System.Drawing.Color.White);
				}
			}
			for (int i = 0; i < maxShipVisible; i++)
			{
				shipButtons[i].Draw(drawingManagement);

				ShipInstance ship = colonyShips[i + shipScrollBar.TopIndex];
				GorgonLibrary.Graphics.Sprite sprite = ship.BaseShipDesign.ShipClass.Sprites[ship.BaseShipDesign.WhichStyle];
				if (ship.BaseShipDesign.ShipClass.Size % 2 == 0)
				{
					sprite.SetScale((64 / sprite.Width) * (sprite.Width / 160), (64 / sprite.Height) * (sprite.Width / 160));
				}
				else
				{
					sprite.SetScale((64 / (sprite.Width - 16.0f)) * ((sprite.Width - 16.0f) / 160), (64 / (sprite.Height - 16.0f)) * ((sprite.Width - 16.0f) / 160));
				}
				//xPos - planetOffset + (i * 70) + 15
				sprite.SetPosition(xPos - shipOffset + ((i / 2) * 70) + 35 - (sprite.ScaledWidth / 2) + (i * 70), yPos + 170 - (sprite.ScaledHeight / 2));
				sprite.Draw();
			}*/

			colonizeButton.Draw(drawingManagement);
			doneButton.Draw(drawingManagement);

			colonizeButton.DrawToolTip(drawingManagement);
			doneButton.Draw(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			/*foreach (InvisibleStretchButton button in planetButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			foreach (InvisibleStretchButton button in shipButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			result = colonizeButton.MouseHover(x, y, frameDeltaTime) || result;
			result = doneButton.MouseHover(x, y, frameDeltaTime) || result;

			if (shipScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
			}*/

			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			/*foreach (InvisibleStretchButton button in planetButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			foreach (InvisibleStretchButton button in shipButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			result = colonizeButton.MouseDown(x, y) || result;
			result = doneButton.MouseDown(x, y) || result;
			result = shipScrollBar.MouseDown(x, y) || result;*/

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			/*for (int i = 0; i < maxPlanetVisible; i++)
			{
				if (planetButtons[i].MouseUp(x, y))
				{
					selectedPlanet = i;
					foreach (InvisibleStretchButton button in planetButtons)
					{
						button.Selected = false;
					}
					planetButtons[i].Selected = true;
					return true;
				}
			}
			for (int i = 0; i < maxShipVisible; i++)
			{
				if (shipButtons[i].MouseUp(x, y))
				{
					selectedShip = i + shipScrollBar.TopIndex;
					foreach (InvisibleStretchButton button in shipButtons)
					{
						button.Selected = false;
					}
					shipButtons[i].Selected = true;
					//RefreshPlanets();
					return true;
				}
			}
			if (colonizeButton.MouseUp(x, y))
			{
				selectedSystem.Planets[selectedPlanet].Owner = selectedFleet.Empire;
				selectedSystem.UpdateOwners();
				selectedFleet.SubtractShip(colonyShips[selectedShip]);
				colonyShips.Remove(colonyShips[selectedShip]);
				if (colonyShips.Count == 0 && doneFunction != null)
				{
					//All done
					doneFunction();
					return true;
				}
				LoadScreen(selectedSystem, selectedFleet);
				if (colonizeFunction != null)
				{
					colonizeFunction(selectedSystem.Planets[selectedPlanet]);
				}
			}
			if (doneButton.MouseUp(x, y))
			{
				doneFunction();
			}*/
			return false;
		}

		public void LoadScreen(StarSystem system, Squadron fleet)
		{
			selectedSystem = system;
			selectedFleet = fleet;
			colonyShips = new List<ShipInstance>();

			/*foreach (ShipInstance ship in selectedFleet.Ships)
			{
				bool breakOut = false;
				foreach (EquipmentInstance equipment in ship.Equipments)
				{
					if (equipment.EquipmentType == EquipmentType.SPECIAL && equipment.ItemType.AttributeValues.ContainsKey("colonizes"))
					{
						string value = (string)equipment.ItemType.AttributeValues["colonizes"];
						string[] planets = value.Split(new[] { ',' });
						foreach (string planet in planets)
						{
							foreach (Planet actualPlanet in selectedSystem.Planets)
							{
								if (actualPlanet.Owner == null && string.Compare(actualPlanet.PlanetType.InternalName, planet) == 0)
								{
									colonyShips.Add(ship);
									breakOut = true;
									break;
								}
							}
							if (breakOut)
							{
								break;
							}
						}
						if (breakOut)
						{
							break;
						}
					}
				}
			}*/

			if (colonyShips.Count == 0)
			{
				//For some reason there's no available colony ships, remove this item to process
				if (doneFunction != null)
				{
					doneFunction();
					return;
				}
			}
			/*selectedShip = 0;
			//find the first colonizable planet, and select that by default
			foreach (EquipmentInstance equipment in colonyShips[0].Equipments)
			{
				bool breakOut = false;
				if (equipment.EquipmentType == EquipmentType.SPECIAL && equipment.ItemType.AttributeValues.ContainsKey("colonizes"))
				{
					string value = (string)equipment.ItemType.AttributeValues["colonizes"];
					string[] planets = value.Split(new[] { ',' });
					foreach (string planet in planets)
					{
						for (int i = 0; i < selectedSystem.Planets.Count; i++)
						{
							if (string.Compare(selectedSystem.Planets[i].PlanetType.InternalName, planet) == 0)
							{
								selectedPlanet = i;
								breakOut = true;
								break;
							}
						}
						if (breakOut)
						{
							break;
						}
					}
					if (breakOut)
					{
						break;
					}
				}
			}

			maxPlanetVisible = selectedSystem.Planets.Count;
			planetOffset = (int)((maxPlanetVisible / 2.0f) * 70);

			if (colonyShips.Count > 10)
			{
				maxShipVisible = 10;
				shipScrollBar.SetEnabledState(true);
				shipScrollBar.TopIndex = 0;
				shipScrollBar.SetAmountOfItems(colonyShips.Count);
			}
			else
			{
				maxShipVisible = colonyShips.Count;
				shipScrollBar.SetEnabledState(false);
				shipScrollBar.TopIndex = 0;
				shipScrollBar.SetAmountOfItems(10);
			}
			shipOffset = (int)((maxShipVisible / 2.0f) * 70);

			planetButtons = new InvisibleStretchButton[maxPlanetVisible];
			planetNumericNames = new Label[maxPlanetVisible];

			shipButtons = new InvisibleStretchButton[maxShipVisible];

			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos - planetOffset + (70 * i), yPos + 28, 70, 100, 30, 13);
			}
			for (int i = 0; i < shipButtons.Length; i++)
			{
				shipButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos - shipOffset + (70 * i), yPos + 138, 70, 70, 30, 13);
			}
			//planetButtons[selectedPlanet].Selected = true;
			shipButtons[selectedShip].Selected = true;

			int maxVisible = Math.Max(maxPlanetVisible, maxShipVisible);
			windowWidth = maxVisible * 70 + 40;

			if (windowWidth < 200)
			{
				windowWidth = 200;
			}

			backGroundImage.MoveTo(xPos - windowWidth / 2, yPos);
			backGroundImage.SetDimensions(windowWidth, windowHeight);

			RefreshPlanets();*/
		}

		/*private void RefreshPlanets()
		{
			List<string> colonizablePlanets = new List<string>();
			foreach (EquipmentInstance equipment in colonyShips[selectedShip].Equipments)
			{
				if (equipment.EquipmentType == EquipmentType.SPECIAL && equipment.ItemType.AttributeValues.ContainsKey("colonizes"))
				{
					string value = (string)equipment.ItemType.AttributeValues["colonizes"];
					string[] planets = value.Split(new[] { ',' });
					foreach (string planet in planets)
					{
						if (!colonizablePlanets.Contains(planet))
						{
							colonizablePlanets.Add(planet);
						}
					}
				}
			}

			bool selected = false;
			colonizable = new bool[planetButtons.Length];
			for (int i = 0; i < planetButtons.Length; i++)
			{
				if (selectedSystem.Planets[i].Owner != null)
				{
					planetNumericNames[i] = new Label(selectedSystem.Planets[i].NumericName, 0, 0, selectedSystem.Planets[i].Owner.EmpireColor);
					planetNumericNames[i].MoveTo(xPos - planetOffset + (70 * i) - (int)(planetNumericNames[i].GetWidth() / 2) + 35, yPos + 73);
					planetButtons[i].Active = false;
					colonizable[i] = false;
				}
				else
				{
					foreach (string colonizablePlanet in colonizablePlanets)
					{
						if (string.Compare(colonizablePlanet, selectedSystem.Planets[i].PlanetType.InternalName) == 0)
						{
							colonizable[i] = true;
							if (!selected)
							{
								planetButtons[i].Selected = true;
								selected = true;
							}
							break;
						}
					}
					planetNumericNames[i] = new Label(selectedSystem.Planets[i].NumericName, 0, 0, colonizable[i] ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(255, 75, 75, 75));
					planetNumericNames[i].MoveTo(xPos - planetOffset + (70 * i) - (int)(planetNumericNames[i].GetWidth() / 2) + 35, yPos + 73);
					planetButtons[i].Active = colonizable[i];
				}
			}
		}*/
	}
}
