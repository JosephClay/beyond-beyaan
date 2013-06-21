using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class DesignScreen : ScreenInterface
	{
		private const int NONE = 0;
		private const int ENGINE = 1;
		private const int COMPUTER = 2;
		private const int ARMOR = 3;
		private const int SHIELD = 4;
		private const int WEAPON = 5;

		#region Variables
		private List<SpriteName> spriteNames;
		private GameMain gameMain;
		private Ship shipDesign;
		private GorgonLibrary.Graphics.Sprite shipSprite;
		private int totalSpace;
		private int usedSpace;
		private int totalCost;
		private int x;
		private int y;
		Button[] removeButtons;
		Button clear;
		Button confirm;
		Button addWeapon;
		Button[] mountUpButton;
		Button[] mountDownButton;
		Button[] shotUpButton;
		Button[] shotDownButton;
		Button prevShip;
		Button nextShip;
		ComboBox sizeComboBox;
		Button engineButton;
		Button computerButton;
		Button armorButton;
		Button shieldButton;
		ProgressBar spaceUsage;
		List<Engine> availableEngines;
		List<Armor> availableArmors;
		List<Shield> availableShields;
		List<Computer> availableComputers;
		List<Weapon> availableWeapons;
		Button[] techButtons;
		int displayingTechOption;
		int shipWeaponIndex;
		SingleLineTextBox nameTextBox;
		Label[] techLabels;
		ScrollBar techScrollBar;
		#endregion

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			x = gameMain.ScreenWidth / 2 - 400;
			y = gameMain.ScreenHeight / 2 - 300;

			spriteNames = new List<SpriteName>();
			spriteNames.Add(SpriteName.MiniBackgroundButton);
			spriteNames.Add(SpriteName.MiniForegroundButton);
			spriteNames.Add(SpriteName.ScrollUpBackgroundButton);
			spriteNames.Add(SpriteName.ScrollUpForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBar);
			spriteNames.Add(SpriteName.ScrollDownBackgroundButton);
			spriteNames.Add(SpriteName.ScrollDownForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBackgroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalForegroundButton);

			List<string> shipSizes = new List<string>();
			for (int i = 1; i <= 10; i++)
			{
				shipSizes.Add(Utility.ShipSizeToString(i));
			}

			sizeComboBox = new ComboBox(spriteNames, shipSizes, x + 75, y + 40, 140, 25, 10);

			prevShip = new Button(SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton, string.Empty, x + 50, y + 135, 24, 24);
			nextShip = new Button(SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, string.Empty, x + 225, y + 135, 24, 24);
			addWeapon = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Add Weapon", x + 590, y + 10, 200, 25);
			confirm = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Confirm Design", x + 640, y + 570, 150, 25);
			clear = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Clear Design", x + 640, y + 530, 150, 25);

			engineButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 75, y + 230, 140, 25);
			computerButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 75, y + 300, 140, 25);
			armorButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 75, y + 400, 140, 25);
			shieldButton = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 75, y + 500, 140, 25);

			spaceUsage = new ProgressBar(x + 295, y + 575, 300, 16, 300, 0, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);


			displayingTechOption = NONE;
			shipWeaponIndex = 0;

			techButtons = new Button[15];
			for (int i = 0; i < techButtons.Length; i++)
			{
				techButtons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, x + 138, y + 40 + (i * 35), 500, 30);
			}

			int amount = 13;
			removeButtons = new Button[amount];
			mountUpButton = new Button[amount];
			mountDownButton = new Button[amount];
			shotUpButton = new Button[amount];
			shotDownButton = new Button[amount];
			for (int i = 0; i < removeButtons.Length; i++)
			{
				removeButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, x + 750, y + 84 + (i * 34), 16, 16);
				mountUpButton[i] = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, string.Empty, x + 600, y + 84 + (i * 34), 16, 16);
				mountDownButton[i] = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, string.Empty, x + 560, y + 84 + (i * 34), 16, 16);
				shotUpButton[i] = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, string.Empty, x + 660, y + 84 + (i * 34), 16, 16);
				shotDownButton[i] = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, string.Empty, x + 620, y + 84 + (i * 34), 16, 16);
			}
			nameTextBox = new SingleLineTextBox(x + 70, y + 5, 200, 25, SpriteName.MiniBackgroundButton);
			techLabels = new Label[5];
			techLabels[0] = new Label("Name", x + 138, y + 10);
			techLabels[1] = new Label(x + 350, y + 10);
			techLabels[2] = new Label(x + 425, y + 10);
			techLabels[3] = new Label("Cost", x + 500, y + 10);
			techLabels[4] = new Label("Space", x + 575, y + 10);

			techScrollBar = new ScrollBar(x + 640, y + 40, 16, 488, 15, 30, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
				SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton,
				SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			techScrollBar.SetEnabledState(false);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();

			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y - 1, 255, 285, 228, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y + 226, 255, 285, 67, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y + 293, 255, 285, 102, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y + 395, 255, 285, 102, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y + 497, 255, 285, 102, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x - 1, y + 599, 255, 285, 1, System.Drawing.Color.White);
			GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
			gameMain.ShipShader.Parameters["EmpireColor"].SetValue(gameMain.EmpireManager.CurrentEmpire.ConvertedColor);
			shipSprite.Draw();
			GorgonLibrary.Gorgon.CurrentShader = null;
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x + 285, y - 1, 255, 515, 75, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x + 285, y + 75, 255, 515, 450, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x + 285, y + 525, 255, 515, 75, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Name:", x + 25, y + 6, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Size:", x + 35, y + 41, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Engine:", x + 20, y + 231, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Computer:", x, y + 301, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Armor:", x + 23, y + 401, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Shield:", x + 20, y + 501, System.Drawing.Color.White);
			nameTextBox.Draw(drawingManagement);
			//DrawingManagement.DrawText("Arial", shipName, x + 75, y + 6, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Weapon Name", x + 290, y + 55, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Damage", x + 415, y + 55, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Accuracy", x + 480, y + 55, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Mounts", x + 560, y + 55, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Shots", x + 620, y + 55, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Space", x + 685, y + 55, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Galaxy Speed: " + shipDesign.engine.GetGalaxySpeed(), x + 2, y + 255, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Combat Speed: " + shipDesign.engine.GetCombatSpeed(), x + 2, y + 274, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Space Used: " + shipDesign.engine.GetSpace(totalSpace), x + 150, y + 255, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Cost: " + shipDesign.engine.GetCost(totalSpace), x + 150, y + 274, System.Drawing.Color.White);

			drawingManagement.DrawSprite(SpriteName.BeamIcon, x + 227, y + 295, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ParticleIcon, x + 227, y + 315, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.MissileIcon, x + 227, y + 335, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.TorpedoIcon, x + 227, y + 355, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.BombIcon, x + 227, y + 375, 255, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", shipDesign.computer.beamEfficiency.ToString() + "%", x + 242, y + 295, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.computer.particleEfficiency.ToString() + "%", x + 242, y + 315, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.computer.missileEfficiency.ToString() + "%", x + 242, y + 335, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.computer.torpedoEfficiency.ToString() + "%", x + 242, y + 355, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.computer.bombEfficiency.ToString() + "%", x + 242, y + 375, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Space Used: " + shipDesign.computer.GetSpace(totalSpace), x + 2, y + 340, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Cost: " + shipDesign.computer.GetCost(totalSpace), x + 2, y + 365, System.Drawing.Color.White);

			drawingManagement.DrawSprite(SpriteName.BeamIcon, x + 227, y + 397, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ParticleIcon, x + 227, y + 417, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.MissileIcon, x + 227, y + 437, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.TorpedoIcon, x + 227, y + 457, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.BombIcon, x + 227, y + 477, 255, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", shipDesign.armor.beamEfficiency.ToString() + "%", x + 242, y + 397, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.armor.particleEfficiency.ToString() + "%", x + 242, y + 417, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.armor.missileEfficiency.ToString() + "%", x + 242, y + 437, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.armor.torpedoEfficiency.ToString() + "%", x + 242, y + 457, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.armor.bombEfficiency.ToString() + "%", x + 242, y + 477, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Hit Points: " + shipDesign.armor.GetHP(totalSpace), x + 2, y + 428, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Space Used: " + shipDesign.armor.GetSpace(totalSpace), x + 2, y + 448, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Cost: " + shipDesign.armor.GetCost(totalSpace), x + 2, y + 468, System.Drawing.Color.White);

			drawingManagement.DrawSprite(SpriteName.BeamIcon, x + 227, y + 499, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.ParticleIcon, x + 227, y + 519, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.MissileIcon, x + 227, y + 539, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.TorpedoIcon, x + 227, y + 559, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.BombIcon, x + 227, y + 579, 255, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", shipDesign.shield.beamEfficiency.ToString() + "%", x + 242, y + 499, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.shield.particleEfficiency.ToString() + "%", x + 242, y + 519, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.shield.missileEfficiency.ToString() + "%", x + 242, y + 539, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.shield.torpedoEfficiency.ToString() + "%", x + 242, y + 559, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", shipDesign.shield.bombEfficiency.ToString() + "%", x + 242, y + 579, System.Drawing.Color.White);

			drawingManagement.DrawText("Arial", "Resistance: " + shipDesign.shield.GetResistance(), x + 2, y + 528, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Space Used: " + shipDesign.shield.GetSpace(totalSpace), x + 2, y + 548, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Cost: " + shipDesign.shield.GetCost(totalSpace), x + 2, y + 568, System.Drawing.Color.White);

			shieldButton.Draw(drawingManagement);
			armorButton.Draw(drawingManagement);
			computerButton.Draw(drawingManagement);
			engineButton.Draw(drawingManagement);
			prevShip.Draw(drawingManagement);
			nextShip.Draw(drawingManagement);
			sizeComboBox.Draw(drawingManagement);
			spaceUsage.Draw(drawingManagement);

			drawingManagement.DrawText("Arial", "Total Space: " + totalSpace, x + 300, y + 555, System.Drawing.Color.White);
			drawingManagement.DrawText("Arial", "Space Used: " + usedSpace, x + 470, y + 555, usedSpace <= totalSpace ? System.Drawing.Color.White : System.Drawing.Color.Red);

			drawingManagement.DrawText("Arial", "Total Cost: " + totalCost, x + 300, y + 535, System.Drawing.Color.White);

			int count = shipDesign.weapons.Count > 13 ? 13 : shipDesign.weapons.Count;

			for (int i = 0; i < count; i++)
			{
				switch (shipDesign.weapons[i + shipWeaponIndex].GetWeaponType())
				{
					case WeaponType.BEAM:
						{
							drawingManagement.DrawSprite(SpriteName.BeamIcon, x + 289, y + 80 + (i * 34), 255, System.Drawing.Color.White);
						} break;
					case WeaponType.PARTICLE:
						{
							drawingManagement.DrawSprite(SpriteName.ParticleIcon, x + 289, y + 80 + (i * 34), 255, System.Drawing.Color.White);
						} break;
					case WeaponType.MISSILE:
						{
							drawingManagement.DrawSprite(SpriteName.MissileIcon, x + 289, y + 80 + (i * 34), 255, System.Drawing.Color.White);
						} break;
					case WeaponType.TORPEDO:
						{
							drawingManagement.DrawSprite(SpriteName.TorpedoIcon, x + 289, y + 80 + (i * 34), 255, System.Drawing.Color.White);
						} break;
					case WeaponType.BOMB:
						{
							drawingManagement.DrawSprite(SpriteName.BombIcon, x + 289, y + 80 + (i * 34), 255, System.Drawing.Color.White);
						} break;
				}
				drawingManagement.DrawText("Arial", shipDesign.weapons[i + shipWeaponIndex].GetName(), x + 305, y + 82 + (i * 34), System.Drawing.Color.White);
				drawingManagement.DrawText("Arial", shipDesign.weapons[i + shipWeaponIndex].GetDamage(shipDesign.computer).ToString(), x + 440, y + 82 + (i * 34), System.Drawing.Color.White);
				if (shipDesign.weapons[i + shipWeaponIndex].GetAccuracy(shipDesign.computer) >= 0)
				{
					drawingManagement.DrawText("Arial", shipDesign.weapons[i + shipWeaponIndex].GetAccuracy(shipDesign.computer).ToString() + "%", x + 500, y + 82 + (i * 34), System.Drawing.Color.White);
				}
				int space = shipDesign.weapons[i + shipWeaponIndex].GetSpace();
				if (shipDesign.weapons[i + shipWeaponIndex].Mounts >= 1)
				{
					space *= shipDesign.weapons[i + shipWeaponIndex].Mounts;
					drawingManagement.DrawText("Arial", shipDesign.weapons[i + shipWeaponIndex].Mounts.ToString(), x + 578, y + 82 + (i * 34), System.Drawing.Color.White);
				}
				if (shipDesign.weapons[i + shipWeaponIndex].Ammo >= 1)
				{
					space *= shipDesign.weapons[i + shipWeaponIndex].Ammo;
					drawingManagement.DrawText("Arial", shipDesign.weapons[i + shipWeaponIndex].Ammo.ToString(), x + 637, y + 82 + (i * 34), System.Drawing.Color.White);
				}
				drawingManagement.DrawText("Arial", space.ToString(), x + 690, y + 82 + (i * 34), System.Drawing.Color.White);
				removeButtons[i].Draw(drawingManagement);
				if (shipDesign.weapons[i + shipWeaponIndex].Mounts > 0)
				{
					mountUpButton[i].Draw(drawingManagement);
					mountDownButton[i].Draw(drawingManagement);
				}
				if (shipDesign.weapons[i + shipWeaponIndex].Ammo > 0)
				{
					shotUpButton[i].Draw(drawingManagement);
					shotDownButton[i].Draw(drawingManagement);
				}
			}

			addWeapon.Draw(drawingManagement);
			confirm.Draw(drawingManagement);
			clear.Draw(drawingManagement);

			if (displayingTechOption != NONE)
			{
				drawingManagement.DrawSprite(SpriteName.ControlBackground, x + 130, y, 255, 540, 570, System.Drawing.Color.White);
				//DrawingManagement.DrawText("Arial", "Name", x + 160, y + 20, System.Drawing.Color.White);
				techScrollBar.DrawScrollBar(drawingManagement);
				foreach (Label label in techLabels)
				{
					label.Draw();
				}
				count = 0;
				switch (displayingTechOption)
				{
					case ENGINE:
						{
							count = availableEngines.Count > 15 ? 15 : availableEngines.Count;
						} break;
					case COMPUTER:
						{
							count = availableComputers.Count > 15 ? 15 : availableComputers.Count;
						} break;
					case ARMOR:
						{
							count = availableArmors.Count > 15 ? 15 : availableArmors.Count;
						} break;
					case SHIELD:
						{
							count = availableShields.Count > 15 ? 15 : availableShields.Count;
						} break;
					case WEAPON:
						{
							count = availableWeapons.Count > 15 ? 15 : availableWeapons.Count;;
							for (int i = 0; i < count; i++)
							{
								techButtons[i].Draw(drawingManagement);
								drawingManagement.DrawText("Arial", availableWeapons[i + techScrollBar.TopIndex].GetDamage(shipDesign.computer).ToString(), x + 400, y + 54 + (i * 35), System.Drawing.Color.White);
								if (availableWeapons[i + techScrollBar.TopIndex].GetAccuracy(shipDesign.computer) >= 0)
								{
									drawingManagement.DrawText("Arial", availableWeapons[i + techScrollBar.TopIndex].GetAccuracy(shipDesign.computer).ToString() + "%", x + 500, y + 54 + (i * 35), System.Drawing.Color.White);
								}
								drawingManagement.DrawText("Arial", availableWeapons[i + techScrollBar.TopIndex].GetSpace().ToString(), x + 600, y + 54 + (i * 35), System.Drawing.Color.White);
							}
						} break;
				}
			}
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			switch (displayingTechOption)
			{
				case NONE:
					{
						nameTextBox.Update(frameDeltaTime);
						sizeComboBox.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						engineButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						computerButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						armorButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						shieldButton.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						nextShip.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						prevShip.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						confirm.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						clear.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						addWeapon.UpdateHovering(mouseX, mouseY, frameDeltaTime);
						int count = shipDesign.weapons.Count > 13 ? 13 : shipDesign.weapons.Count;
						for (int i = 0; i < count; i++)
						{
							removeButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
							if (shipDesign.weapons[i + shipWeaponIndex].Mounts > 0)
							{
								mountUpButton[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
								mountDownButton[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
							}
							if (shipDesign.weapons[i + shipWeaponIndex].Ammo > 0)
							{
								shotUpButton[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
								shotDownButton[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
							}
						}
					} break;
				case ENGINE:
					{
						if (techScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							RefreshTechOptions();
							break;
						}
						int count = availableEngines.Count > 15 ? 15 : availableEngines.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
						}
					} break;
				case COMPUTER:
					{
						if (techScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							RefreshTechOptions();
							break;
						}
						int count = availableComputers.Count > 15 ? 15 : availableComputers.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
						}
					} break;
				case ARMOR:
					{
						if (techScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							RefreshTechOptions();
							break;
						}
						int count = availableArmors.Count > 15 ? 15 : availableArmors.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
						}
					} break;
				case SHIELD:
					{
						if (techScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							RefreshTechOptions();
							break;
						}
						int count = availableShields.Count > 15 ? 15 : availableShields.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
						}
					} break;
				case WEAPON:
					{
						if (techScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
						{
							RefreshTechOptions();
							break;
						}
						int count = availableWeapons.Count > 15 ? 15 : availableWeapons.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
						}
					} break;
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			switch (displayingTechOption)
			{
				case NONE:
					{
						if (nameTextBox.MouseDown(x, y))
						{
							return;
						}
						if (sizeComboBox.MouseDown(x, y))
						{
							return;
						}
						prevShip.MouseDown(x, y);
						nextShip.MouseDown(x, y);
						engineButton.MouseDown(x, y);
						computerButton.MouseDown(x, y);
						armorButton.MouseDown(x, y);
						shieldButton.MouseDown(x, y);
						confirm.MouseDown(x, y);
						clear.MouseDown(x, y);
						addWeapon.MouseDown(x, y);

						int count = shipDesign.weapons.Count > 13 ? 13 : shipDesign.weapons.Count;
						for (int i = 0; i < count; i++)
						{
							removeButtons[i].MouseDown(x, y);
							if (shipDesign.weapons[i + shipWeaponIndex].Mounts > 0)
							{
								mountUpButton[i].MouseDown(x, y);
								mountDownButton[i].MouseDown(x, y);
							}
							if (shipDesign.weapons[i + shipWeaponIndex].Ammo > 0)
							{
								shotUpButton[i].MouseDown(x, y);
								shotDownButton[i].MouseDown(x, y);
							}
						}
					} break;
				case ENGINE:
					{
						techScrollBar.MouseDown(x, y);
						int count = availableEngines.Count > 15 ? 15 : availableEngines.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].MouseDown(x, y);
						}
					} break;
				case COMPUTER:
					{
						techScrollBar.MouseDown(x, y);
						int count = availableComputers.Count > 15 ? 15 : availableComputers.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].MouseDown(x, y);
						}
					} break;
				case ARMOR:
					{
						techScrollBar.MouseDown(x, y);
						int count = availableArmors.Count > 15 ? 15 : availableArmors.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].MouseDown(x, y);
						}
					} break;
				case SHIELD:
					{
						techScrollBar.MouseDown(x, y);
						int count = availableShields.Count > 15 ? 15 : availableShields.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].MouseDown(x, y);
						}
					} break;
				case WEAPON:
					{
						techScrollBar.MouseDown(x, y);
						int count = availableWeapons.Count > 15 ? 15 : availableWeapons.Count;
						for (int i = 0; i < count; i++)
						{
							techButtons[i].MouseDown(x, y);
						}
					} break;
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			switch (displayingTechOption)
			{
				case NONE:
					{
						if (nameTextBox.MouseUp(x, y))
						{
							return;
						}
						if (sizeComboBox.MouseUp(x, y))
						{
							shipDesign.Size = sizeComboBox.SelectedIndex + 1;
							UpdateSpaceUsageAndCost();
							shipSprite = gameMain.EmpireManager.CurrentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
							shipSprite.SetPosition((gameMain.ScreenWidth / 2) - 305, (gameMain.ScreenHeight / 2) - 205);
							shipSprite.SetScale(100.0f / shipSprite.Width, 100.0f / shipSprite.Height);
							return;
						}
						if (prevShip.MouseUp(x, y))
						{
							shipDesign.WhichStyle--;
							if (shipDesign.WhichStyle < 0)
							{
								shipDesign.WhichStyle = 5;
							}
							shipSprite = gameMain.EmpireManager.CurrentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
							shipSprite.SetPosition((gameMain.ScreenWidth / 2) - 305, (gameMain.ScreenHeight / 2) - 205);
							shipSprite.SetScale(100.0f / shipSprite.Width, 100.0f / shipSprite.Height);
							return;
						}
						if (nextShip.MouseUp(x, y))
						{
							shipDesign.WhichStyle++;
							if (shipDesign.WhichStyle > 5)
							{
								shipDesign.WhichStyle = 0;
							}
							shipSprite = gameMain.EmpireManager.CurrentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
							shipSprite.SetPosition((gameMain.ScreenWidth / 2) - 305, (gameMain.ScreenHeight / 2) - 205);
							shipSprite.SetScale(100.0f / shipSprite.Width, 100.0f / shipSprite.Height);
							return;
						}
						if (engineButton.MouseUp(x, y))
						{
							displayingTechOption = ENGINE;
							LoadTechOptions();
							return;
						}
						if (computerButton.MouseUp(x, y))
						{
							displayingTechOption = COMPUTER;
							LoadTechOptions();
							return;
						}
						if (armorButton.MouseUp(x, y))
						{
							displayingTechOption = ARMOR;
							LoadTechOptions();
							return;
						}
						if (shieldButton.MouseUp(x, y))
						{
							displayingTechOption = SHIELD;
							LoadTechOptions();
							return;
						}
						if (addWeapon.MouseUp(x, y))
						{
							displayingTechOption = WEAPON;
							LoadTechOptions();
							return;
						}
						if (clear.MouseUp(x, y))
						{
							shipDesign.engine = availableEngines[0];
							engineButton.SetButtonText(shipDesign.engine.GetName());
							shipDesign.computer = availableComputers[0];
							computerButton.SetButtonText(shipDesign.computer.GetName());
							shipDesign.armor = availableArmors[0];
							armorButton.SetButtonText(shipDesign.armor.GetName());
							shipDesign.shield = availableShields[0];
							shieldButton.SetButtonText(shipDesign.shield.GetName());
							shipDesign.weapons.Clear();
							UpdateSpaceUsageAndCost();
						}
						if (confirm.MouseUp(x, y))
						{
							shipDesign.Name = nameTextBox.GetString();
							gameMain.EmpireManager.CurrentEmpire.FleetManager.AddShipDesign(shipDesign);
							NameGenerator generator = new NameGenerator();
							shipDesign.Name = generator.GetName();
							nameTextBox.SetString(shipDesign.Name);
						}
						int count = shipDesign.weapons.Count > 13 ? 13 : shipDesign.weapons.Count;
						for (int i = 0; i < count; i++)
						{
							if (removeButtons[i].MouseUp(x, y))
							{
								shipDesign.weapons.RemoveAt(i + shipWeaponIndex);
								UpdateSpaceUsageAndCost();
								break;
							}
							if (shipDesign.weapons[i + shipWeaponIndex].Mounts > 0)
							{
								if (mountUpButton[i].MouseUp(x, y))
								{
									shipDesign.weapons[i + shipWeaponIndex].Mounts++;
									UpdateSpaceUsageAndCost();
								}
								if (mountDownButton[i].MouseUp(x, y) && shipDesign.weapons[i + shipWeaponIndex].Mounts > 1)
								{
									shipDesign.weapons[i + shipWeaponIndex].Mounts--;
									UpdateSpaceUsageAndCost();
								}
							}
							if (shipDesign.weapons[i + shipWeaponIndex].Ammo > 0)
							{
								if (shotUpButton[i].MouseUp(x, y))
								{
									shipDesign.weapons[i + shipWeaponIndex].Ammo++;
									UpdateSpaceUsageAndCost();
								}
								if (shotDownButton[i].MouseUp(x, y) && shipDesign.weapons[i + shipWeaponIndex].Ammo > 1)
								{
									shipDesign.weapons[i + shipWeaponIndex].Ammo--;
									UpdateSpaceUsageAndCost();
								}
							}
						}
					} break;
				case ENGINE:
					{
						if (techScrollBar.MouseUp(x, y))
						{
							RefreshTechOptions();
							return;
						}
						int count = availableEngines.Count > 15 ? 15 : availableEngines.Count;
						for (int i = 0; i < count; i++)
						{
							if (techButtons[i].MouseUp(x, y))
							{
								shipDesign.engine = availableEngines[i + techScrollBar.TopIndex];
								engineButton.SetButtonText(shipDesign.engine.GetName());
								displayingTechOption = NONE;
								UpdateSpaceUsageAndCost();
								return;
							}
						}
					} break;
				case COMPUTER:
					{
						if (techScrollBar.MouseUp(x, y))
						{
							RefreshTechOptions();
							return;
						}
						int count = availableComputers.Count > 15 ? 15 : availableComputers.Count;
						for (int i = 0; i < count; i++)
						{
							if (techButtons[i].MouseUp(x, y))
							{
								shipDesign.computer = availableComputers[i + techScrollBar.TopIndex];
								computerButton.SetButtonText(shipDesign.computer.GetName());
								displayingTechOption = NONE;
								UpdateSpaceUsageAndCost();
								return;
							}
						}
					} break;
				case ARMOR:
					{
						if (techScrollBar.MouseUp(x, y))
						{
							RefreshTechOptions();
							return;
						}
						int count = availableArmors.Count > 15 ? 15 : availableArmors.Count;
						for (int i = 0; i < count; i++)
						{
							if (techButtons[i].MouseUp(x, y))
							{
								shipDesign.armor = availableArmors[i + techScrollBar.TopIndex];
								armorButton.SetButtonText(shipDesign.armor.GetName());
								displayingTechOption = NONE;
								UpdateSpaceUsageAndCost();
								return;
							}
						}
					} break;
				case SHIELD:
					{
						if (techScrollBar.MouseUp(x, y))
						{
							RefreshTechOptions();
							return;
						}
						int count = availableShields.Count > 15 ? 15 : availableShields.Count;
						for (int i = 0; i < count; i++)
						{
							if (techButtons[i].MouseUp(x, y))
							{
								shipDesign.shield = availableShields[i + techScrollBar.TopIndex];
								shieldButton.SetButtonText(shipDesign.shield.GetName());
								displayingTechOption = NONE;
								UpdateSpaceUsageAndCost();
								return;
							}
						}
					} break;
				case WEAPON:
					{
						if (techScrollBar.MouseUp(x, y))
						{
							RefreshTechOptions();
							return;
						}
						int count = availableWeapons.Count > 15 ? 15 : availableWeapons.Count;
						for (int i = 0; i < count; i++)
						{
							if (techButtons[i].MouseUp(x, y))
							{
								shipDesign.weapons.Add(new Weapon(availableWeapons[i + techScrollBar.TopIndex]));
								displayingTechOption = NONE;
								UpdateSpaceUsageAndCost();
								return;
							}
						}
					} break;
			}
			if (displayingTechOption != NONE)
			{
				if (x < this.x + 130 || x >= this.x + 670 || y < this.y || y >= this.y + 570)
				{
					displayingTechOption = NONE;
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void LoadScreen()
		{
			x = gameMain.ScreenWidth / 2 - 400;
			y = gameMain.ScreenHeight / 2 - 300;

			shipDesign = gameMain.EmpireManager.CurrentEmpire.FleetManager.LastShipDesign;
			shipSprite = gameMain.EmpireManager.CurrentEmpire.EmpireRace.GetShip(shipDesign.Size, shipDesign.WhichStyle);
			shipSprite.SetPosition(x + 95, y + 95);
			shipSprite.SetScale(100.0f / shipSprite.Width, 100.0f / shipSprite.Height);

			engineButton.SetButtonText(shipDesign.engine.GetName());
			computerButton.SetButtonText(shipDesign.computer.GetName());
			armorButton.SetButtonText(shipDesign.armor.GetName());
			shieldButton.SetButtonText(shipDesign.shield.GetName());
			sizeComboBox.SelectedIndex = shipDesign.Size - 1;

			UpdateSpaceUsageAndCost();

			NameGenerator generator = new NameGenerator();

			string shipName = generator.GetName();
			shipDesign.Name = shipName;
			nameTextBox.SetString(shipName);

			TechnologyManager techManager = gameMain.EmpireManager.CurrentEmpire.TechnologyManager;

			availableEngines = new List<Engine>();
			foreach (Engine engine in techManager.VisibleEngines)
			{
				if (engine.GetLevel() > 0)
				{
					availableEngines.Add(engine);
				}
			}
			availableComputers = new List<Computer>();
			foreach (Computer computer in techManager.VisibleComputers)
			{
				if (computer.GetLevel() > 0)
				{
					availableComputers.Add(computer);
				}
			}
			availableArmors = new List<Armor>();
			foreach (Armor armor in techManager.VisibleArmors)
			{
				if (armor.GetLevel() > 0)
				{
					availableArmors.Add(armor);
				}
			}
			availableShields = new List<Shield>();
			foreach (Shield shield in techManager.VisibleShields)
			{
				if (shield.GetLevel() > 0)
				{
					availableShields.Add(shield);
				}
			}

			availableWeapons = new List<Weapon>();
			foreach (Beam beam in techManager.VisibleBeams)
			{
				if (beam.GetLevel() > 0)
				{
					availableWeapons.Add(new Weapon(beam));
				}
			}
			foreach (Particle particle in techManager.VisibleParticles)
			{
				if (particle.GetLevel() > 0)
				{
					availableWeapons.Add(new Weapon(particle));
				}
			}
			foreach (Missile missile in techManager.VisibleMissiles)
			{
				if (missile.GetLevel() > 0)
				{
					availableWeapons.Add(new Weapon(missile));
				}
			}
			foreach (Torpedo torpedo in techManager.VisibleTorpedoes)
			{
				if (torpedo.GetLevel() > 0)
				{
					availableWeapons.Add(new Weapon(torpedo));
				}
			}
			foreach (Bomb bomb in techManager.VisibleBombs)
			{
				if (bomb.GetLevel() > 0)
				{
					availableWeapons.Add(new Weapon(bomb));
				}
			}
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
			if (nameTextBox.KeyDown(e))
			{
				return;
			}
			if (e.Key == KeyboardKeys.Space)
			{
				gameMain.ToggleSitRep();
			}
		}

		private void UpdateSpaceUsageAndCost()
		{
			totalSpace = shipDesign.TotalSpace;
			usedSpace = 0;
			usedSpace += shipDesign.engine.GetSpace(totalSpace);
			usedSpace += shipDesign.armor.GetSpace(totalSpace);
			usedSpace += shipDesign.computer.GetSpace(totalSpace);
			usedSpace += shipDesign.shield.GetSpace(totalSpace);

			totalCost = 0;
			totalCost += shipDesign.engine.GetCost(totalSpace);
			totalCost += shipDesign.armor.GetCost(totalSpace);
			totalCost += shipDesign.computer.GetCost(totalSpace);
			totalCost += shipDesign.shield.GetCost(totalSpace);

			foreach (Weapon weapon in shipDesign.weapons)
			{
				int weaponSpace = weapon.GetSpace();
				int weaponCost = weapon.GetCost();
				if (weapon.Mounts > 0)
				{
					weaponSpace *= weapon.Mounts;
					weaponCost *= weapon.Mounts;
				}
				if (weapon.Ammo > 0)
				{
					weaponSpace *= weapon.Ammo;
					weaponCost *= weapon.Ammo;
				}
				usedSpace += weaponSpace;
				totalCost += weaponCost;
			}

			spaceUsage.SetMaxProgress(totalSpace);
			spaceUsage.SetProgress(usedSpace);

			confirm.Active = totalSpace >= usedSpace;
			if (confirm.Active)
			{
				spaceUsage.SetColor(System.Drawing.Color.Green);
			}
			else
			{
				spaceUsage.SetColor(System.Drawing.Color.Red);
			}

			shipDesign.Cost = totalCost;
		}

		public void LoadTechOptions()
		{
			techScrollBar.TopIndex = 0;
			switch (displayingTechOption)
			{
				case ENGINE:
					{
						int count = availableEngines.Count > 15 ? 15 : availableEngines.Count;
						if (count < availableEngines.Count)
						{
							techScrollBar.SetEnabledState(true);
							techScrollBar.SetAmountOfItems(availableEngines.Count);
						}
						else
						{
							techScrollBar.SetEnabledState(false);
							techScrollBar.SetAmountOfItems(30);
						}
						
						for (int i = 0; i < count; i++)
						{
							techButtons[i].SetButtonText(availableEngines[i].GetName());
						}
						techLabels[1].SetText("Combat Speed");
						techLabels[2].SetText("Galaxy Speed");
					} break;
				case COMPUTER:
					{
						int count = availableComputers.Count > 15 ? 15 : availableComputers.Count;
						if (count < availableComputers.Count)
						{
							techScrollBar.SetEnabledState(true);
							techScrollBar.SetAmountOfItems(availableComputers.Count);
						}
						else
						{
							techScrollBar.SetEnabledState(false);
							techScrollBar.SetAmountOfItems(30);
						}
						for (int i = 0; i < count; i++)
						{
							techButtons[i].SetButtonText(availableComputers[i].GetName());
						}
						techLabels[1].SetText("Efficiency");
						techLabels[2].SetText(string.Empty);
					} break;
				case ARMOR:
					{
						int count = availableArmors.Count > 15 ? 15 : availableArmors.Count;
						if (count < availableArmors.Count)
						{
							techScrollBar.SetEnabledState(true);
							techScrollBar.SetAmountOfItems(availableArmors.Count);
						}
						else
						{
							techScrollBar.SetEnabledState(false);
							techScrollBar.SetAmountOfItems(30);
						}
						for (int i = 0; i < count; i++)
						{
							techButtons[i].SetButtonText(availableArmors[i].GetName());
						}
						techLabels[1].SetText("Efficiency");
						techLabels[2].SetText("HP");
					} break;
				case SHIELD:
					{
						int count = availableShields.Count > 15 ? 15 : availableShields.Count;
						if (count < availableShields.Count)
						{
							techScrollBar.SetEnabledState(true);
							techScrollBar.SetAmountOfItems(availableShields.Count);
						}
						else
						{
							techScrollBar.SetEnabledState(false);
							techScrollBar.SetAmountOfItems(30);
						}
						for (int i = 0; i < count; i++)
						{
							techButtons[i].SetButtonText(availableShields[i].GetName());
						}
						techLabels[1].SetText("Efficiency");
						techLabels[2].SetText("Resistance");
					} break;
				case WEAPON:
					{
						int count = availableWeapons.Count > 15 ? 15 : availableWeapons.Count;
						if (count < availableWeapons.Count)
						{
							techScrollBar.SetEnabledState(true);
							techScrollBar.SetAmountOfItems(availableWeapons.Count);
						}
						else
						{
							techScrollBar.SetEnabledState(false);
							techScrollBar.SetAmountOfItems(30);
						}
						for (int i = 0; i < count; i++)
						{
							techButtons[i].SetButtonText(availableWeapons[i].GetName());
						}
						techLabels[1].SetText("Damage");
						techLabels[2].SetText("Accuracy");
					} break;
			}
		}
		private void RefreshTechOptions()
		{
			//This is when the tech scrollbar is moved
			switch (displayingTechOption)
			{
				case ENGINE:
					{
						for (int i = 0; i < techButtons.Length; i++)
						{
							techButtons[i].SetButtonText(availableEngines[i + techScrollBar.TopIndex].GetName());
						}
					} break;
				case COMPUTER:
					{
						for (int i = 0; i < techButtons.Length; i++)
						{
							techButtons[i].SetButtonText(availableComputers[i + techScrollBar.TopIndex].GetName());
						}
					} break;
				case ARMOR:
					{
						for (int i = 0; i < techButtons.Length; i++)
						{
							techButtons[i].SetButtonText(availableArmors[i + techScrollBar.TopIndex].GetName());
						}
					} break;
				case SHIELD:
					{
						for (int i = 0; i < techButtons.Length; i++)
						{
							techButtons[i].SetButtonText(availableShields[i + techScrollBar.TopIndex].GetName());
						}
					} break;
				case WEAPON:
					{
						for (int i = 0; i < techButtons.Length; i++)
						{
							techButtons[i].SetButtonText(availableWeapons[i + techScrollBar.TopIndex].GetName());
						}
					} break;
			}
		}
	}
}
