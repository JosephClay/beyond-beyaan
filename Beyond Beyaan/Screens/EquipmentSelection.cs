using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class EquipmentSelection : WindowInterface
	{
		public delegate void OkClick(Equipment finalEquipment);
		public delegate void CancelClick();
		private OkClick OnOkClick;
		private CancelClick OnCancelClick;

		#region Constants
		private const int BEAM = 0;
		private const int PROJECTILE = 1;
		private const int SHOCKWAVE = 2;
		private const int MISSILE = 3;
		private const int TORPEDO = 4;
		private const int BOMB = 5;
		private const int SHIELD = 6;
		private const int ARMOR = 7;
		private const int COMPUTER = 8;
		private const int SYSTEM_ENGINE = 9;
		private const int STELLAR_DRIVE = 10;
		private const int REACTOR = 11;
		private const int SPECIAL = 12;
		#endregion

		#region Member Variables
		private List<TechnologyItem> mainItems;
		private List<TechnologyItem> secondaryItems;
		private List<TechnologyItem> modifierItems;

		//private EquipmentType whichType;

		private int size;
		private Dictionary<string, object> shipValues;
		#endregion

		#region UI Elements
		private StretchButton[] typeButtons;

		private CheckBox[] buttonList;

		//private int maxVisible;
		private List<int> selectedModifierItems;
		private ScrollBar scrollBar;

		private Label equipmentNameLabel;
		//private Equipment equipment;

		private ComboBox mainComboBox;
		private ComboBox mountComboBox;

		StretchableImage mainBackground;
		StretchableImage modifierBackground;
		StretchableImage iconsBackground;
		StretchableImage statsBackground;
		#endregion

		private Button okButton;
		private Button cancelButton;

		public EquipmentSelection(int x, int y, GameMain gameMain, OkClick okClick, CancelClick cancelClick)
			: base(x, y, 560, 380, string.Empty, gameMain, false)
		{
			backGroundImage = new StretchableImage(x, y, 560, 380, 60, 60, DrawingManagement.BorderBorder);

			buttonList = new CheckBox[8];

			selectedModifierItems = new List<int>();

			OnOkClick = okClick;
			OnCancelClick = cancelClick;

			okButton = new Button(SpriteName.EquipmentOK, SpriteName.EquipmentOKHL, string.Empty, x + 465, y + 325, 80, 40);
			cancelButton = new Button(SpriteName.EquipmentCancel, SpriteName.EquipmentCancelHL, string.Empty, x + 380, y + 325, 80, 40);

			GorgonLibrary.Graphics.Font font = DrawingManagement.GetFont("Computer");

			okButton.SetToolTip(DrawingManagement.BoxBorderBG, font, "Add this equipment", "addThisEquipmentToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			cancelButton.SetToolTip(DrawingManagement.BoxBorderBG, font, "Cancel", "cancelAddThisEquipmentToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			mainComboBox = new ComboBox(DrawingManagement.SmallComboBox, new List<string>(), xPos + 115, yPos + 27, 230, 35, 7, true, 10, 10);
			mountComboBox = new ComboBox(DrawingManagement.SmallComboBox, new List<string>(), xPos + 115, yPos + 67, 230, 35, 7, true, 10, 10);

			typeButtons = new StretchButton[13];

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i] = new StretchButton(DrawingManagement.IconButtonBG, DrawingManagement.IconButtonFG, string.Empty, x + 20 + (i % 2 * 40), y + 15 + (i / 2 * 40), 40, 40, 10, 10);
			}
			typeButtons[0].SetToolTip(DrawingManagement.BoxBorderBG, font, "Beam Weapons", "beamWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[1].SetToolTip(DrawingManagement.BoxBorderBG, font, "Projectile Weapons", "projectileWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[2].SetToolTip(DrawingManagement.BoxBorderBG, font, "Shockwave Weapons", "shockwaveWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[3].SetToolTip(DrawingManagement.BoxBorderBG, font, "Missiles", "missileWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[4].SetToolTip(DrawingManagement.BoxBorderBG, font, "Torpedoes", "torpedoWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[5].SetToolTip(DrawingManagement.BoxBorderBG, font, "Bombs", "bombWeaponToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[6].SetToolTip(DrawingManagement.BoxBorderBG, font, "Shields", "shieldsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[7].SetToolTip(DrawingManagement.BoxBorderBG, font, "Armors", "armorsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[8].SetToolTip(DrawingManagement.BoxBorderBG, font, "Computers", "computersToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[9].SetToolTip(DrawingManagement.BoxBorderBG, font, "Sublight Engines", "sublightEnginesToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[10].SetToolTip(DrawingManagement.BoxBorderBG, font, "FTL Engines", "FTLEnginesToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[11].SetToolTip(DrawingManagement.BoxBorderBG, font, "Reactors", "reactorsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[12].SetToolTip(DrawingManagement.BoxBorderBG, font, "Special Equipment", "specialEquipmentToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			typeButtons[0].Selected = true;
			//whichType = EquipmentType.BEAM;

			for (int i = 0; i < buttonList.Length; i++)
			{
				buttonList[i] = new CheckBox(DrawingManagement.RadioButton, string.Empty, xPos + 105, y + 125 + (i * 25), 205, 30, 19, true);
			}

			scrollBar = new ScrollBar(x + 333, y + 132, 16, 148, 8, 8, false, false, DrawingManagement.VerticalScrollBar);

			equipmentNameLabel = new Label(x + 20, y + 335);

			mainBackground = new StretchableImage(x + 105, y + 15, 250, 100, 30, 13, DrawingManagement.BoxBorder);
			modifierBackground = new StretchableImage(x + 105, y + 120, 250, 200, 30, 13, DrawingManagement.BoxBorder);
			statsBackground = new StretchableImage(x + 360, y + 120, 190, 200, 30, 13, DrawingManagement.BoxBorder);
			iconsBackground = new StretchableImage(x + 360, y + 15, 190, 100, 30, 13, DrawingManagement.BoxBorder);

			LoadEquipment();
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);

			okButton.Draw(drawingManagement);
			cancelButton.Draw(drawingManagement);

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i].Draw(drawingManagement);
			}
			
			drawingManagement.DrawSprite(SpriteName.BeamIcon, xPos + 30, yPos + 25);
			drawingManagement.DrawSprite(SpriteName.ProjectileIcon, xPos + 70, yPos + 25);
			drawingManagement.DrawSprite(SpriteName.ShockwaveIcon, xPos + 30, yPos + 65);
			drawingManagement.DrawSprite(SpriteName.MissileIcon, xPos + 70, yPos + 65);
			drawingManagement.DrawSprite(SpriteName.TorpedoIcon, xPos + 30, yPos + 105);
			drawingManagement.DrawSprite(SpriteName.BombIcon, xPos + 70, yPos + 105);
			drawingManagement.DrawSprite(SpriteName.ShieldIcon, xPos + 30, yPos + 145);
			drawingManagement.DrawSprite(SpriteName.ArmorIcon, xPos + 70, yPos + 145);
			drawingManagement.DrawSprite(SpriteName.ComputerIcon, xPos + 30, yPos + 185);
			drawingManagement.DrawSprite(SpriteName.SystemEngineIcon, xPos + 70, yPos + 185);
			drawingManagement.DrawSprite(SpriteName.StellarEngineIcon, xPos + 30, yPos + 225);
			drawingManagement.DrawSprite(SpriteName.PowerIcon, xPos + 70, yPos + 225);
			drawingManagement.DrawSprite(SpriteName.SpecialIcon, xPos + 30, yPos + 265);

			mainBackground.Draw(drawingManagement);
			modifierBackground.Draw(drawingManagement);
			statsBackground.Draw(drawingManagement);
			iconsBackground.Draw(drawingManagement);

			/*for (int i = 0; i < maxVisible; i++)
			{
				buttonList[i].Draw(drawingManagement);
			}*/
			scrollBar.Draw(drawingManagement);

			equipmentNameLabel.Draw();

			mountComboBox.Draw(drawingManagement);
			mainComboBox.Draw(drawingManagement);

			/*if (equipment != null)
			{
				for (int i = 0; i < equipment.DesignIcons.Count; i++)
				{
					equipment.DesignIcons[i].Draw(xPos + 370, yPos + 130 + (i * 25), 250, 25, drawingManagement);
				}
				for (int i = 0; i < equipment.DesignControls.Count; i++)
				{
					equipment.DesignControls[i].Draw(xPos + 370, yPos + 27 + (i * 25), 100, 25, drawingManagement);
				}
			}*/

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i].DrawTips(drawingManagement);
			}
			okButton.DrawToolTip(drawingManagement);
			cancelButton.DrawToolTip(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = base.MouseHover(x, y, frameDeltaTime);

			mainComboBox.MouseHover(x, y, frameDeltaTime);
			mountComboBox.MouseHover(x, y, frameDeltaTime);

			okButton.MouseHover(x, y, frameDeltaTime);
			cancelButton.MouseHover(x, y, frameDeltaTime);

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i].MouseHover(x, y, frameDeltaTime);
			}

			/*for (int i = 0; i < maxVisible; i++)
			{
				buttonList[i].MouseHover(x, y, frameDeltaTime);
			}

			if (scrollBar.MouseHover(x, y, frameDeltaTime))
			{
				for (int i = 0; i < maxVisible; i++)
				{
					buttonList[i].SetButtonText(modifierItems[i + scrollBar.TopIndex].Name);
					buttonList[i].IsChecked = false;
				}
				foreach (int i in selectedModifierItems)
				{
					if (i >= scrollBar.TopIndex && i < scrollBar.TopIndex + maxVisible)
					{
						buttonList[i].IsChecked = true;
					}
				}
			}

			if (equipment != null)
			{
				for (int i = 0; i < equipment.DesignControls.Count; i++)
				{
					equipment.DesignControls[i].Update(x, y, xPos + 370, yPos + 27 + (i * 25), frameDeltaTime);
				}
			}*/

			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = base.MouseDown(x, y);

			if (mainComboBox.MouseDown(x, y))
			{
				return true;
			}
			if (mountComboBox.MouseDown(x, y))
			{
				return true;
			}

			okButton.MouseDown(x, y);
			cancelButton.MouseDown(x, y);

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i].MouseDown(x, y);
			}

			/*for (int i = 0; i < maxVisible; i++)
			{
				buttonList[i].MouseDown(x, y);
			}

			if (equipment != null)
			{
				for (int i = 0; i < equipment.DesignControls.Count; i++)
				{
					equipment.DesignControls[i].MouseDown(x, y, xPos + 370, yPos + 27 + (i * 25));
				}
			}*/

			scrollBar.MouseDown(x, y);

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = base.MouseUp(x, y);

			if (mainComboBox.MouseUp(x, y))
			{
				CheckValidSelection();
				return true;
			}
			if (mountComboBox.MouseUp(x, y))
			{
				CheckValidSelection();
				return true;
			}

			if (okButton.MouseUp(x, y) && OnOkClick != null)
			{
				//OnOkClick(equipment);
			}
			if (cancelButton.MouseUp(x, y) && OnCancelClick != null)
			{
				OnCancelClick();
			}

			for (int i = 0; i < typeButtons.Length; i++)
			{
				if (typeButtons[i].MouseUp(x, y))
				{
					/*EquipmentType oldType = whichType;
					switch (i)
					{
						case BEAM: whichType = EquipmentType.BEAM;
							break;
						case PROJECTILE: whichType = EquipmentType.PROJECTILE;
							break;
						case SHOCKWAVE: whichType = EquipmentType.SHOCKWAVE;
							break;
						case MISSILE: whichType = EquipmentType.MISSILE;
							break;
						case TORPEDO: whichType = EquipmentType.TORPEDO;
							break;
						case BOMB: whichType = EquipmentType.BOMB;
							break;
						case SHIELD: whichType = EquipmentType.SHIELD;
							break;
						case ARMOR: whichType = EquipmentType.ARMOR;
							break;
						case COMPUTER: whichType = EquipmentType.COMPUTER;
							break;
						case SYSTEM_ENGINE: whichType = EquipmentType.SYSTEM_ENGINE;
							break;
						case STELLAR_DRIVE: whichType = EquipmentType.STELLAR_ENGINE;
							break;
						case REACTOR: whichType = EquipmentType.REACTOR;
							break;
						case SPECIAL: whichType = EquipmentType.SPECIAL;
							break;
					}
					foreach (StretchButton button in typeButtons)
					{
						button.Selected = false;
					}
					typeButtons[i].Selected = true;
					if (oldType != whichType)
					{
						selectedModifierItems = new List<int>();

						foreach (CheckBox button in buttonList)
						{
							button.IsChecked = false;
						}
						LoadEquipment();
					}*/
				}
			}

			/*for (int i = 0; i < maxVisible; i++)
			{
				if (buttonList[i].MouseUp(x, y))
				{
					if (selectedModifierItems.Contains(i + scrollBar.TopIndex) && buttonList[i].IsChecked)
					{
						selectedModifierItems.Remove(i + scrollBar.TopIndex);
						buttonList[i].IsChecked = false;
					}
					else if (!selectedModifierItems.Contains(i + scrollBar.TopIndex))
					{
						selectedModifierItems.Add(i + scrollBar.TopIndex);
						buttonList[i].IsChecked = true;
					}
					//RefreshEquipmentLabels(false);
				}
			}

			if (scrollBar.MouseUp(x, y))
			{
				for (int i = 0; i < maxVisible; i++)
				{
					buttonList[i].SetButtonText(mainItems[i + scrollBar.TopIndex].Name);
					buttonList[i].IsChecked = false;
				}
				foreach (int i in selectedModifierItems)
				{
					if (i >= scrollBar.TopIndex && i < scrollBar.TopIndex + maxVisible)
					{
						buttonList[i].IsChecked = true;
					}
				}
			}*/

			/*if (equipment != null)
			{
				for (int i = 0; i < equipment.DesignControls.Count; i++)
				{
					if (equipment.DesignControls[i].MouseUp(x, y, xPos + 370, yPos + 27 + (i * 25)))
					{
						if (equipment.ModifiableValues.ContainsKey(equipment.DesignControls[i].ValueNames[0]))
						{
							equipment.ModifiableValues[equipment.DesignControls[i].ValueNames[0]] = equipment.DesignControls[i].Value.ToString();
						}
						else
						{
							equipment.ModifiableValues.Add(equipment.DesignControls[i].ValueNames[0], equipment.DesignControls[i].Value.ToString());
						}
						RefreshEquipmentLabels(false);
					}
				}
			}*/

			return result;
		}

		private void LoadEquipment()
		{
			mainItems = new List<TechnologyItem>();
			secondaryItems = new List<TechnologyItem>();
			modifierItems = new List<TechnologyItem>();

			TechnologyManager techManager = gameMain.empireManager.CurrentEmpire.TechnologyManager;

			/*switch(whichType)
			{
				case EquipmentType.ARMOR:
					{
						mainItems = techManager.Armors;
						secondaryItems = techManager.ArmorPlatings;
						modifierItems = techManager.ArmorMods;
					} break;
				case EquipmentType.BEAM:
					{
						mainItems = techManager.Beams;
						secondaryItems = techManager.BeamMounts;
						modifierItems = techManager.BeamMods;
					} break;
				case EquipmentType.BOMB:
					{
						mainItems = techManager.Bombs;
						secondaryItems = techManager.BombBodies;
						modifierItems = techManager.BombMods;
					} break;
				case EquipmentType.COMPUTER:
					{
						mainItems = techManager.Computers;
						secondaryItems = techManager.ComputerMounts;
						modifierItems = techManager.ComputerMods;
					} break;
				case EquipmentType.MISSILE:
					{
						mainItems = techManager.MissileWarheads;
						secondaryItems = techManager.MissileBodies;
						modifierItems = techManager.MissileMods;
					} break;
				case EquipmentType.PROJECTILE:
					{
						mainItems = techManager.Projectiles;
						secondaryItems = techManager.ProjectileMounts;
						modifierItems = techManager.ProjectileMods;
					} break;
				case EquipmentType.REACTOR:
					{
						mainItems = techManager.Reactors;
						secondaryItems = techManager.ReactorMounts;
						modifierItems = techManager.ReactorMods;
					} break;
				case EquipmentType.SHIELD:
					{
						mainItems = techManager.Shields;
						secondaryItems = techManager.ShieldMounts;
						modifierItems = techManager.ShieldMods;
					} break;
				case EquipmentType.SHOCKWAVE:
					{
						mainItems = techManager.Shockwaves;
						secondaryItems = techManager.ShockwaveEmitters;
						modifierItems = techManager.ShockwaveMods;
					} break;
				case EquipmentType.SPECIAL:
					{
						mainItems = techManager.SpecialEquipment;
					} break;
				case EquipmentType.STELLAR_ENGINE:
					{
						mainItems = techManager.StellarEngines;
						secondaryItems = techManager.StellarEngineMounts;
						modifierItems = techManager.StellarEngineMods;
					} break;
				case EquipmentType.SYSTEM_ENGINE:
					{
						mainItems = techManager.SystemEngines;
						secondaryItems = techManager.SystemEngineMounts;
						modifierItems = techManager.SystemEngineMods;
					} break;
				case EquipmentType.TORPEDO:
					{
						mainItems = techManager.Torpedoes;
						secondaryItems = techManager.TorpedoLaunchers;
						modifierItems = techManager.TorpedoMods;
					} break;
			}
			List<string> itemNames = new List<string>();
			foreach (TechnologyItem item in mainItems)
			{
				itemNames.Add(item.Name);
			}
			mainComboBox.SetItems(itemNames);

			itemNames = new List<string>();
			foreach (TechnologyItem item in secondaryItems)
			{
				itemNames.Add(item.Name);
			}
			mountComboBox.SetItems(itemNames);

			if (modifierItems.Count > buttonList.Length)
			{
				maxVisible = buttonList.Length;
				scrollBar.SetEnabledState(true);
				scrollBar.SetAmountOfItems(modifierItems.Count);
			}
			else
			{
				maxVisible = modifierItems.Count;
				scrollBar.SetAmountOfItems(buttonList.Length);
				scrollBar.SetEnabledState(false);
			}

			for (int i = 0; i < maxVisible; i++)
			{
				buttonList[i].SetButtonText(modifierItems[i].Name);
			}

			selectedModifierItems = new List<int>();
			CheckValidSelection();*/
		}

		public void LoadWindow(int size, Dictionary<string, object> shipValues)
		{
			this.size = size;
			this.shipValues = shipValues;

			for (int i = 0; i < typeButtons.Length; i++)
			{
				typeButtons[i].Selected = false;
			}
			typeButtons[0].Selected = true;
			//whichType = EquipmentType.BEAM;
			LoadEquipment();

			foreach (CheckBox button in buttonList)
			{
				button.IsChecked = false;
			}
		}

		private void CheckValidSelection()
		{
			//if (mainComboBox.SelectedIndex < 0 || (mountComboBox.SelectedIndex < 0 && whichType != EquipmentType.SPECIAL))
			{
				okButton.Active = false;
			}
			//else
			{
				//okButton.Active = true;
			}
			//RefreshEquipmentLabels(true);
		}

		/*private void RefreshEquipmentLabels(bool wipe)
		{
			if (mainComboBox.SelectedIndex < 0 || (mountComboBox.SelectedIndex < 0 && whichType != EquipmentType.SPECIAL))
			{
				equipmentNameLabel.SetText("Invalid Selection");
				equipment = null;
			}
			else
			{
				if (wipe)
				{
					equipment = new Equipment(GetEquipment());
				}
				else
				{
					equipment.ItemType = mainItems[mainComboBox.SelectedIndex];
					if (whichType != EquipmentType.SPECIAL)
					{
						equipment.MountType = secondaryItems[mountComboBox.SelectedIndex];
						List<TechnologyItem> modifiers = new List<TechnologyItem>();
						foreach (int i in selectedModifierItems)
						{
							modifiers.Add(modifierItems[i]);
						}
						equipment.ModifierItems = modifiers;
					}
				}
				Dictionary<string, object> values = equipment.GetEquipmentInfo(shipValues);
				foreach (Icon icon in equipment.DesignControls)
				{
					icon.UpdateText(values);
					if (equipment.ModifiableValues.ContainsKey(icon.ValueNames[0]))
					{
						equipment.ModifiableValues[icon.ValueNames[0]] = icon.Value.ToString();
					}
					else
					{
						equipment.ModifiableValues.Add(icon.ValueNames[0], icon.Value.ToString());
					}
				}
				values = equipment.GetEquipmentInfo(shipValues);
				foreach (Icon icon in equipment.DesignIcons)
				{
					icon.UpdateText(values);
				}
				equipmentNameLabel.SetText(equipment.GetName());
			}
		}
		private Equipment GetEquipment()
		{
			List<TechnologyItem> modifiers = new List<TechnologyItem>();
			foreach (int i in selectedModifierItems)
			{
				modifiers.Add(modifierItems[i]);
			}
			return new Equipment(whichType, mainItems[mainComboBox.SelectedIndex], whichType == EquipmentType.SPECIAL ? null : secondaryItems[mountComboBox.SelectedIndex], modifiers, new Dictionary<string,object>(), gameMain.iconManager);
		}*/
	}
}
