using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class DesignScreen : ScreenInterface
	{
		private GameMain gameMain;

		#region Variables
		private StretchableImage background;
		private StretchableImage shipBackground;
		private StretchableImage statsBackground;
		private StretchableImage componentsBackground;
		private StretchableImage buttonsBackground;
		private StretchableImage descriptionBackground;
		private StretchableImage titleBackground;

		private Label designNameLabel;
		private SingleLineTextBox nameTextBox;

		private InvisibleStretchButton[] equipmentButtons;
		private ScrollBar equipmentScrollbar;
		private Button[] removeButtons;
		private Label[] equipmentLabels;
		private int maxEquipmentVisible;

		private Button addButton;
		private StretchButton clearButton;
		private StretchButton confirmButton;
		private ComboBox shipClassComboBox;
		private Button prevShip;
		private Button nextShip;

		//private ShipDesign shipDesign;
		//private Dictionary<string, object> shipValues;
		//private GorgonLibrary.Graphics.Sprite shipSprite;

		private int xPos;
		private int yPos;

		private EquipmentSelection equipmentSelection;
		private bool selectionShowing;
		#endregion

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			xPos = (gameMain.ScreenWidth / 2) - 420;
			yPos = (gameMain.ScreenHeight / 2) - 320;

			background = new StretchableImage(xPos, yPos, 840, 640, 200, 200, DrawingManagement.ScreenBorder);
			componentsBackground = new StretchableImage(xPos + 25, yPos + 25, 565, 450, 60, 60, DrawingManagement.BorderBorder);
			shipBackground = new StretchableImage(xPos + 590, yPos + 25, 225, 225, 30, 13, DrawingManagement.BoxBorder);
			statsBackground = new StretchableImage(xPos + 590, yPos + 250, 225, 275, 30, 13, DrawingManagement.BoxBorder);
			buttonsBackground = new StretchableImage(xPos + 590, yPos + 525, 225, 90, 30, 13, DrawingManagement.BoxBorder);
			descriptionBackground = new StretchableImage(xPos + 25, yPos + 475, 565, 140, 30, 13, DrawingManagement.BoxBorder);
			titleBackground = new StretchableImage(xPos + 35, yPos + 35, 545, 60, 30, 13, DrawingManagement.BoxBorder);

			prevShip = new Button(SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton, string.Empty, xPos + 600, yPos + 112, 16, 16);
			nextShip = new Button(SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, string.Empty, xPos + 790, yPos + 112, 16, 16);

			clearButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Clear All", xPos + 600, yPos + 533, 205, 35);
			confirmButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Confirm Design", xPos + 600, yPos + 572, 205, 35);

			addButton = new Button(SpriteName.AddEquipment, SpriteName.AddEquipmentHL, string.Empty, xPos + 494, yPos + 46, 80, 40);
			addButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Add equipment", "addEquipmentToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			designNameLabel = new Label("Name:", xPos + 45, yPos + 55);
			nameTextBox = new SingleLineTextBox(xPos + 105, yPos + 49, 200, 35, DrawingManagement.TextBox);

			List<string> shipSizes = new List<string>();

			shipClassComboBox = new ComboBox(DrawingManagement.ComboBox, shipSizes, xPos + 600, yPos + 200, 205, 35, 10, true);

			equipmentScrollbar = new ScrollBar(xPos + 560, yPos + 107, 16, 318, 10, 10, false, false, DrawingManagement.VerticalScrollBar);
			equipmentButtons = new InvisibleStretchButton[10];
			removeButtons = new Button[equipmentButtons.Length];
			equipmentLabels = new Label[equipmentButtons.Length];
			for (int i = 0; i < equipmentButtons.Length; i++)
			{
				equipmentButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + 40, yPos + 107 + (35 * i), 520, 35, 30, 13);
				removeButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, xPos + 536, yPos + 118 + (35 * i), 16, 16);
				equipmentLabels[i] = new Label(xPos + 64, yPos + 117 + (35 * i));
			}
			maxEquipmentVisible = 10;

			equipmentSelection = new EquipmentSelection(xPos + 140, yPos + 150, gameMain, OnOkClick, OnCancelClick);
			selectionShowing = false;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			//Draw the backgrounds
			gameMain.DrawGalaxyBackground();
			background.Draw(drawingManagement);
			componentsBackground.Draw(drawingManagement);
			shipBackground.Draw(drawingManagement);
			statsBackground.Draw(drawingManagement);
			buttonsBackground.Draw(drawingManagement);
			descriptionBackground.Draw(drawingManagement);
			titleBackground.Draw(drawingManagement);

			//Draw the UI and information
			addButton.Draw(drawingManagement);
			prevShip.Draw(drawingManagement);
			nextShip.Draw(drawingManagement);
			clearButton.Draw(drawingManagement);
			confirmButton.Draw(drawingManagement);

			/*for (int i = 0; i < shipDesign.ShipClass.DesignIcons.Count; i++)
			{
				shipDesign.ShipClass.DesignIcons[i].Draw(xPos + 600, yPos + 265 + (i * 25), 150, 25, drawingManagement);
			}*/

			designNameLabel.Draw();
			nameTextBox.Draw(drawingManagement);

			equipmentScrollbar.Draw(drawingManagement);
			for (int i = 0; i < maxEquipmentVisible; i++)
			{
				equipmentButtons[i].Draw(drawingManagement);
				removeButtons[i].Draw(drawingManagement);
				equipmentLabels[i].Draw(drawingManagement);
				SpriteName sprite = SpriteName.BeamIcon;
				/*switch (shipDesign.Equipments[i + equipmentScrollbar.TopIndex].EquipmentType)
				{
					case EquipmentType.BEAM: sprite = SpriteName.BeamIcon;
						break;
					case EquipmentType.PROJECTILE: sprite = SpriteName.ProjectileIcon;
						break;
					case EquipmentType.SHOCKWAVE: sprite = SpriteName.ShockwaveIcon;
						break;
					case EquipmentType.MISSILE: sprite = SpriteName.MissileIcon;
						break;
					case EquipmentType.TORPEDO: sprite = SpriteName.TorpedoIcon;
						break;
					case EquipmentType.BOMB: sprite = SpriteName.BombIcon;
						break;
					case EquipmentType.COMPUTER: sprite = SpriteName.ComputerIcon;
						break;
					case EquipmentType.SHIELD: sprite = SpriteName.ShieldIcon;
						break;
					case EquipmentType.ARMOR: sprite = SpriteName.ArmorIcon;
						break;
					case EquipmentType.SYSTEM_ENGINE: sprite = SpriteName.SystemEngineIcon;
						break;
					case EquipmentType.STELLAR_ENGINE: sprite = SpriteName.StellarEngineIcon;
						break;
					case EquipmentType.REACTOR: sprite = SpriteName.PowerIcon;
						break;
					case EquipmentType.SPECIAL: sprite = SpriteName.SpecialIcon;
						break;
				}*/
				drawingManagement.DrawSprite(sprite, xPos + 40, yPos + 117 + (i * 35));
			}

			GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
			gameMain.ShipShader.Parameters["EmpireColor"].SetValue(gameMain.empireManager.CurrentEmpire.ConvertedColor);
			//shipSprite.Draw();
			GorgonLibrary.Gorgon.CurrentShader = null;

			shipClassComboBox.Draw(drawingManagement);

			addButton.DrawToolTip(drawingManagement);

			if (selectionShowing)
			{
				equipmentSelection.DrawWindow(drawingManagement);
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			UpdateBackground(frameDeltaTime);

			if (!selectionShowing)
			{
				nameTextBox.Update(frameDeltaTime);
				if (shipClassComboBox.MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
				nextShip.MouseHover(x, y, frameDeltaTime);
				prevShip.MouseHover(x, y, frameDeltaTime);
				addButton.MouseHover(x, y, frameDeltaTime);
				clearButton.MouseHover(x, y, frameDeltaTime);
				confirmButton.MouseHover(x, y, frameDeltaTime);
				for (int i = 0; i < maxEquipmentVisible; i++)
				{
					removeButtons[i].MouseHover(x, y, frameDeltaTime);
				}
			}
			else
			{
				equipmentSelection.MouseHover(x, y, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (!selectionShowing)
			{
				if (nameTextBox.MouseDown(x, y))
				{
					return;
				}
				if (shipClassComboBox.MouseDown(x, y))
				{
					return;
				}
				prevShip.MouseDown(x, y);
				nextShip.MouseDown(x, y);
				addButton.MouseDown(x, y);
				clearButton.MouseDown(x, y);
				confirmButton.MouseDown(x, y);
				for (int i = 0; i < maxEquipmentVisible; i++)
				{
					removeButtons[i].MouseDown(x, y);
				}
			}
			else
			{
				equipmentSelection.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*if (!selectionShowing)
			{
				if (nameTextBox.MouseUp(x, y))
				{
					return;
				}
				if (shipClassComboBox.MouseUp(x, y))
				{
					shipDesign.ShipClass = shipDesign.Race.ShipClasses[shipClassComboBox.SelectedIndex];
					shipDesign.WhichStyle = 0;
					UpdateInformation();
					shipSprite = shipDesign.ShipClass.Sprites[shipDesign.WhichStyle];
					nameTextBox.SetString(shipDesign.Race.GetRandomShipName(shipDesign.ShipClass.Size));
					LoadShipSprite();
					return;
				}
				if (prevShip.MouseUp(x, y))
				{
					shipDesign.WhichStyle--;
					if (shipDesign.WhichStyle < 0)
					{
						shipDesign.WhichStyle = shipDesign.ShipClass.NumberOfStyles - 1;
					}
					shipSprite = shipDesign.ShipClass.Sprites[shipDesign.WhichStyle];
					LoadShipSprite();
					return;
				}
				if (nextShip.MouseUp(x, y))
				{
					shipDesign.WhichStyle++;
					if (shipDesign.WhichStyle >= shipDesign.ShipClass.NumberOfStyles)
					{
						shipDesign.WhichStyle = 0;
					}
					shipSprite = shipDesign.ShipClass.Sprites[shipDesign.WhichStyle];
					LoadShipSprite();
					return;
				}
				if (addButton.MouseUp(x, y))
				{
					selectionShowing = true;
					equipmentSelection.LoadWindow(shipDesign.ShipClass.Size, shipDesign.ShipClass.Values);
					return;
				}
				if (clearButton.MouseUp(x, y))
				{
					shipDesign.Equipments.Clear();
					UpdateInformation();
					return;
				}
				if (confirmButton.MouseUp(x, y))
				{
					shipDesign.Name = nameTextBox.GetString();
					gameMain.empireManager.CurrentEmpire.FleetManager.AddShipDesign(shipDesign);
					shipDesign.Equipments.Clear();
					nameTextBox.SetString(shipDesign.Race.GetRandomShipName(shipDesign.ShipClass.Size));
					UpdateInformation();
				}
				for (int i = 0; i < maxEquipmentVisible; i++)
				{
					if (removeButtons[i].MouseUp(x, y))
					{
						shipDesign.Equipments.RemoveAt(i + equipmentScrollbar.TopIndex);
						UpdateInformation();
						return;
					}
				}
			}
			else
			{
				equipmentSelection.MouseUp(x, y);
			}*/
		}

		private void OnOkClick(Equipment finalEquipment)
		{
			//shipDesign.Equipments.Add(finalEquipment);
			UpdateInformation();
		}

		private void OnCancelClick()
		{
			selectionShowing = false;
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void Resize()
		{
			/*x = gameMain.ScreenWidth / 2 - 400;
			y = gameMain.ScreenHeight / 2 - 300;

			sizeComboBox.MoveTo(x + 75, y + 40);
			engineButton.MoveTo(x + 75, y + 230);
			computerButton.MoveTo(x + 75, y + 300);
			armorButton.MoveTo(x + 75, y + 400);
			shieldButton.MoveTo(x + 75, y + 500);

			prevShip.MoveTo(x + 75, y + 135);
			nextShip.MoveTo(x + 251, y + 135);

			for (int i = 0; i < techButtons.Length; i++)
			{
				techButtons[i].MoveTo(x + 150, y + 50 + (i * 35));
			}*/
		}

		private void LoadShipSprite()
		{
			/*float scale = 1.0f;
			if (shipDesign.ShipClass.Size <= 10)
			{
				scale = (shipDesign.ShipClass.Size * 16) / shipSprite.Width;
			}
			else
			{
				scale = 160.0f / shipSprite.Width;
			}
			shipSprite.SetScale(scale, scale);
			shipSprite.SetPosition(xPos + 702 - (shipSprite.ScaledWidth / 2), yPos + 115 - (shipSprite.ScaledHeight / 2));*/
		}

		public void LoadScreen()
		{
			/*xPos = gameMain.ScreenWidth / 2 - 420;
			yPos = gameMain.ScreenHeight / 2 - 320;

			Empire currentEmpire = gameMain.empireManager.CurrentEmpire;

			shipDesign = currentEmpire.FleetManager.LastShipDesign;
			shipSprite = shipDesign.ShipClass.Sprites[shipDesign.WhichStyle];

			List<string> shipSizeLabels = new List<string>();
			int selectedIndex = 0;
			for (int i = 0; i < currentEmpire.EmpireRace.ShipClasses.Count; i++)
			{
				shipSizeLabels.Add(currentEmpire.EmpireRace.ShipClasses[i].ClassName);
				if (shipDesign.ShipClass == currentEmpire.EmpireRace.ShipClasses[i])
				{
					selectedIndex = i;
				}
			}

			shipClassComboBox.SetItems(shipSizeLabels);
			shipClassComboBox.SelectedIndex = selectedIndex;
			LoadShipSprite();
			nameTextBox.SetString(shipDesign.Race.GetRandomShipName(shipDesign.ShipClass.Size));

			UpdateInformation();*/
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(ScreenEnum.Galaxy);
			}
			if (nameTextBox.KeyDown(e))
			{
				UpdateInformation();
				return;
			}
			/*if (e.Key == KeyboardKeys.Space)
			{
				gameMain.ToggleSitRep();
			}*/
		}

		private void UpdateInformation()
		{
			/*shipValues = shipDesign.GetBasicValues();

			foreach (Icon icon in shipDesign.ShipClass.DesignIcons)
			{
				icon.UpdateText(shipValues);
			}

			string isValid = shipDesign.ShipClass.ShipScript.IsShipDesignValid(shipValues);
			confirmButton.Active = string.IsNullOrEmpty(isValid);

			maxEquipmentVisible = shipDesign.Equipments.Count > equipmentButtons.Length ? equipmentButtons.Length : shipDesign.Equipments.Count;

			if (shipDesign.Equipments.Count > equipmentButtons.Length)
			{
				maxEquipmentVisible = equipmentButtons.Length;
				equipmentScrollbar.SetEnabledState(true);
				equipmentScrollbar.SetAmountOfItems(shipDesign.Equipments.Count);
			}
			else
			{
				maxEquipmentVisible = shipDesign.Equipments.Count;
				equipmentScrollbar.SetEnabledState(false);
				equipmentScrollbar.SetAmountOfItems(10);
			}
			equipmentScrollbar.TopIndex = 0;

			for (int i = 0; i < maxEquipmentVisible; i++)
			{
				equipmentLabels[i].SetText(shipDesign.Equipments[i].GetName());
			}*/
		}
	}
}
