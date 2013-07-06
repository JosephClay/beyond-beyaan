﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class ResearchScreen : ScreenInterface
	{
		private const int BEAM = 0;
		private const int PARTICLE = 1;
		private const int MISSILE = 2;
		private const int TORPEDO = 3;
		private const int BOMB = 4;
		private const int ENGINE = 5;
		private const int ARMOR = 6;
		private const int SHIELD = 7;
		private const int COMPUTER = 8;
		private const int INFRASTRUCTURE = 9;

		GameMain gameMain;

		private float researchPoints;
		private Label researchPointsLabel;
		private ScrollBar[] techScrollBars;
		private Button[] researchingTechNames;
		private Button[] lockedButtons;
		private Button[] availableTechs;
		private ProgressBar[] availableTechProgresses;
		private ScrollBar availableScrollBar;
		private ProgressBar[] techFieldProgresses;

		private int whichField;
		private int maxVisible;
		private int techIndex;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			researchPointsLabel = new Label(x + 500, y + 575);

			researchingTechNames = new Button[10];
			techScrollBars = new ScrollBar[10];
			lockedButtons = new Button[10];
			techFieldProgresses = new ProgressBar[10];

			for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i] = new ScrollBar(x + 5, y + 32 + (i * 60), 16, 250, 1, 101, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton, SpriteName.SliderHorizontalForegroundButton,
					SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				researchingTechNames[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 5, y + 4 + (i * 60), 200, 24);
				lockedButtons[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, x + 295, y + 32 + (i * 60), 16, 16);
				techFieldProgresses[i] = new ProgressBar(x + 211, y + 7 + (i * 60), 100, 16, 100, 0, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar, System.Drawing.Color.Green);
			}

			availableTechs = new Button[12];
			availableTechProgresses = new ProgressBar[12];
			for (int i = 0; i < availableTechs.Length; i++)
			{
				availableTechs[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, x + 420, y + 4 + (i * 30), 350, 30);
				availableTechProgresses[i] = new ProgressBar(x + 665, y + 10 + (i * 30), 100, 16, 100, 0, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
			}

			availableScrollBar = new ScrollBar(x + 770, y + 4, 16, 342, 12, 30, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton,
				SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton,
				SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);
			availableScrollBar.SetEnabledState(false);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();

			drawingManagement.DrawSprite(SpriteName.ControlBackground, (gameMain.ScreenWidth / 2) - 400, (gameMain.ScreenHeight / 2) - 300, 255, 800, 600, System.Drawing.Color.White);

			for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i].DrawScrollBar(drawingManagement);
				researchingTechNames[i].Draw(drawingManagement);
				lockedButtons[i].Draw(drawingManagement);
				techFieldProgresses[i].Draw(drawingManagement);
			}

			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].Draw(drawingManagement);
				availableTechProgresses[i].Draw(drawingManagement);
			}

			availableScrollBar.DrawScrollBar(drawingManagement);
			researchPointsLabel.Draw();
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].UpdateHovering(mouseX, mouseY, frameDeltaTime))
				{
					switch (i)
					{
						case BEAM: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BEAM, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case PARTICLE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.PARTICLE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case MISSILE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.MISSILE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case TORPEDO: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.TORPEDO, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case BOMB: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BOMB, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ENGINE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ENGINE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ARMOR: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ARMOR, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case SHIELD: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.SHIELD, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case COMPUTER: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.COMPUTER, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case INFRASTRUCTURE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.INFRASTRUCTURE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
					}
				}
			}
			if (availableScrollBar.UpdateHovering(mouseX, mouseY, frameDeltaTime))
			{
				techIndex = availableScrollBar.TopIndex;
				RefreshAvailableTechs();
				return;
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				lockedButtons[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}
			for (int i = 0; i < researchingTechNames.Length; i++)
			{
				researchingTechNames[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].UpdateHovering(mouseX, mouseY, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				techScrollBars[i].MouseDown(x, y);
				lockedButtons[i].MouseDown(x, y);
			}
			availableScrollBar.MouseDown(x, y);
			for (int i = 0; i < researchingTechNames.Length; i++)
			{
				researchingTechNames[i].MouseDown(x, y);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].MouseUp(x, y))
				{
					switch (i)
					{
						case BEAM: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BEAM, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case PARTICLE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.PARTICLE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case MISSILE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.MISSILE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case TORPEDO: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.TORPEDO, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case BOMB: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BOMB, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ENGINE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ENGINE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ARMOR: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ARMOR, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case SHIELD: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.SHIELD, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case COMPUTER: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.COMPUTER, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case INFRASTRUCTURE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.INFRASTRUCTURE, techScrollBars[i].TopIndex);
							SetPercentages(gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
					}
				}
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				if (lockedButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case BEAM: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BeamLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BeamLocked;
							break;
						case PARTICLE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ParticleLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ParticleLocked;
							break;
						case MISSILE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.MissileLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.MissileLocked;
							break;
						case TORPEDO: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.TorpedoLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.TorpedoLocked;
							break;
						case BOMB: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BombLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BombLocked;
							break;
						case ENGINE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.EngineLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.EngineLocked;
							break;
						case ARMOR: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ArmorLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ArmorLocked;
							break;
						case SHIELD: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ShieldLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ShieldLocked;
							break;
						case COMPUTER: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked;
							break;
						case INFRASTRUCTURE: gameMain.EmpireManager.CurrentEmpire.TechnologyManager.InfrastructureLocked = !gameMain.EmpireManager.CurrentEmpire.TechnologyManager.InfrastructureLocked;
							break;
					}
					techScrollBars[i].SetEnabledState(lockedButtons[i].Selected);
					lockedButtons[i].Selected = !lockedButtons[i].Selected;
				}
			}
			for (int i = 0; i < researchingTechNames.Length; i++)
			{
				if (researchingTechNames[i].MouseUp(x, y))
				{
					whichField = i;
					RefreshAvailableTechs();
					foreach (Button button in researchingTechNames)
					{
						button.Selected = false;
					}
					researchingTechNames[i].Selected = true;
				}
			}
			if (availableScrollBar.MouseUp(x, y))
			{
				techIndex = availableScrollBar.TopIndex;
				RefreshAvailableTechs();
				return;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (availableTechs[i].MouseUp(x, y))
				{
					TechnologyManager techManager = gameMain.EmpireManager.CurrentEmpire.TechnologyManager;
					Technology whichTechToUpdate;
					switch (whichField)
					{
						case BEAM:
							{
								techManager.WhichBeamBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleBeams[i + techIndex];
								researchingTechNames[BEAM].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[BEAM].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[BEAM].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[BEAM].SetPotentialProgress((int)(researchPoints * (techManager.BeamPercentage * 0.01f)));
							} break;
						case PARTICLE:
							{
								techManager.WhichParticleBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleParticles[i + techIndex];
								researchingTechNames[PARTICLE].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[PARTICLE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[PARTICLE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[PARTICLE].SetPotentialProgress((int)(researchPoints * (techManager.ParticlePercentage * 0.01f)));
							} break;
						case MISSILE:
							{
								techManager.WhichMissileBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleMissiles[i + techIndex];
								researchingTechNames[MISSILE].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[MISSILE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[MISSILE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[MISSILE].SetPotentialProgress((int)(researchPoints * (techManager.MissilePercentage * 0.01f)));
							} break;
						case TORPEDO:
							{
								techManager.WhichTorpedoBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleTorpedoes[i + techIndex];
								researchingTechNames[TORPEDO].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[TORPEDO].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[TORPEDO].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[TORPEDO].SetPotentialProgress((int)(researchPoints * (techManager.TorpedoPercentage * 0.01f)));
							} break;
						case BOMB:
							{
								techManager.WhichBombBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleBombs[i + techIndex];
								researchingTechNames[BOMB].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[BOMB].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[BOMB].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[BOMB].SetPotentialProgress((int)(researchPoints * (techManager.BombPercentage * 0.01f)));
							} break;
						case ENGINE:
							{
								techManager.WhichEngineBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleEngines[i + techIndex];
								researchingTechNames[ENGINE].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[ENGINE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[ENGINE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[ENGINE].SetPotentialProgress((int)(researchPoints * (techManager.EnginePercentage * 0.01f)));
							} break;
						case ARMOR:
							{
								techManager.WhichArmorBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleArmors[i + techIndex];
								researchingTechNames[ARMOR].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[ARMOR].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[ARMOR].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[ARMOR].SetPotentialProgress((int)(researchPoints * (techManager.ArmorPercentage * 0.01f)));
							} break;
						case SHIELD:
							{
								techManager.WhichShieldBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleShields[i + techIndex];
								researchingTechNames[SHIELD].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[SHIELD].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[SHIELD].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[SHIELD].SetPotentialProgress((int)(researchPoints * (techManager.ShieldPercentage * 0.01f)));
							} break;
						case COMPUTER:
							{
								techManager.WhichComputerBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleComputers[i + techIndex];
								researchingTechNames[COMPUTER].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[COMPUTER].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[COMPUTER].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[COMPUTER].SetPotentialProgress((int)(researchPoints * (techManager.ComputerPercentage * 0.01f)));
							} break;
						case INFRASTRUCTURE:
							{
								techManager.WhichInfrastructureBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleInfrastructures[i + techIndex];
								researchingTechNames[INFRASTRUCTURE].SetButtonText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[INFRASTRUCTURE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[INFRASTRUCTURE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[INFRASTRUCTURE].SetPotentialProgress((int)(researchPoints * (techManager.InfrastructurePercentage * 0.01f)));
							} break;
					}
				}
			}
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
		}

		public void LoadPoints(float researchPoints)
		{
			this.researchPoints = researchPoints;
			researchPointsLabel.SetText("Research Points: " + researchPoints);

			TechnologyManager techManager = gameMain.EmpireManager.CurrentEmpire.TechnologyManager;

			researchingTechNames[BEAM].SetButtonText(techManager.VisibleBeams[techManager.WhichBeamBeingResearched].GetNameWithNextLevel());
			researchingTechNames[PARTICLE].SetButtonText(techManager.VisibleParticles[techManager.WhichParticleBeingResearched].GetNameWithNextLevel());
			researchingTechNames[MISSILE].SetButtonText(techManager.VisibleMissiles[techManager.WhichMissileBeingResearched].GetNameWithNextLevel());
			researchingTechNames[TORPEDO].SetButtonText(techManager.VisibleTorpedoes[techManager.WhichTorpedoBeingResearched].GetNameWithNextLevel());
			researchingTechNames[BOMB].SetButtonText(techManager.VisibleBombs[techManager.WhichBombBeingResearched].GetNameWithNextLevel());
			researchingTechNames[ENGINE].SetButtonText(techManager.VisibleEngines[techManager.WhichEngineBeingResearched].GetNameWithNextLevel());
			researchingTechNames[ARMOR].SetButtonText(techManager.VisibleArmors[techManager.WhichArmorBeingResearched].GetNameWithNextLevel());
			researchingTechNames[SHIELD].SetButtonText(techManager.VisibleShields[techManager.WhichShieldBeingResearched].GetNameWithNextLevel());
			researchingTechNames[COMPUTER].SetButtonText(techManager.VisibleComputers[techManager.WhichComputerBeingResearched].GetNameWithNextLevel());
			researchingTechNames[INFRASTRUCTURE].SetButtonText(techManager.VisibleInfrastructures[techManager.WhichInfrastructureBeingResearched].GetNameWithNextLevel());

			lockedButtons[BEAM].Selected = techManager.BeamLocked;
			lockedButtons[PARTICLE].Selected = techManager.ParticleLocked;
			lockedButtons[MISSILE].Selected = techManager.MissileLocked;
			lockedButtons[TORPEDO].Selected = techManager.TorpedoLocked;
			lockedButtons[BOMB].Selected = techManager.BombLocked;
			lockedButtons[ENGINE].Selected = techManager.EngineLocked;
			lockedButtons[ARMOR].Selected = techManager.ArmorLocked;
			lockedButtons[SHIELD].Selected = techManager.ShieldLocked;
			lockedButtons[COMPUTER].Selected = techManager.ComputerLocked;
			lockedButtons[INFRASTRUCTURE].Selected = techManager.InfrastructureLocked;

			techScrollBars[BEAM].SetEnabledState(!techManager.BeamLocked);
			techScrollBars[PARTICLE].SetEnabledState(!techManager.ParticleLocked);
			techScrollBars[MISSILE].SetEnabledState(!techManager.MissileLocked);
			techScrollBars[TORPEDO].SetEnabledState(!techManager.TorpedoLocked);
			techScrollBars[BOMB].SetEnabledState(!techManager.BombLocked);
			techScrollBars[ENGINE].SetEnabledState(!techManager.EngineLocked);
			techScrollBars[ARMOR].SetEnabledState(!techManager.ArmorLocked);
			techScrollBars[SHIELD].SetEnabledState(!techManager.ShieldLocked);
			techScrollBars[COMPUTER].SetEnabledState(!techManager.ComputerLocked);
			techScrollBars[INFRASTRUCTURE].SetEnabledState(!techManager.InfrastructureLocked);

			techFieldProgresses[BEAM].SetMaxProgress(techManager.VisibleBeams[techManager.WhichBeamBeingResearched].GetNextLevelCost());
			techFieldProgresses[BEAM].SetProgress(techManager.VisibleBeams[techManager.WhichBeamBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[PARTICLE].SetMaxProgress(techManager.VisibleParticles[techManager.WhichParticleBeingResearched].GetNextLevelCost());
			techFieldProgresses[PARTICLE].SetProgress(techManager.VisibleParticles[techManager.WhichParticleBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[MISSILE].SetMaxProgress(techManager.VisibleMissiles[techManager.WhichMissileBeingResearched].GetNextLevelCost());
			techFieldProgresses[MISSILE].SetProgress(techManager.VisibleMissiles[techManager.WhichMissileBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[TORPEDO].SetMaxProgress(techManager.VisibleTorpedoes[techManager.WhichTorpedoBeingResearched].GetNextLevelCost());
			techFieldProgresses[TORPEDO].SetProgress(techManager.VisibleTorpedoes[techManager.WhichTorpedoBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[BOMB].SetMaxProgress(techManager.VisibleBombs[techManager.WhichBombBeingResearched].GetNextLevelCost());
			techFieldProgresses[BOMB].SetProgress(techManager.VisibleBombs[techManager.WhichBombBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[ENGINE].SetMaxProgress(techManager.VisibleEngines[techManager.WhichEngineBeingResearched].GetNextLevelCost());
			techFieldProgresses[ENGINE].SetProgress(techManager.VisibleEngines[techManager.WhichEngineBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[ARMOR].SetMaxProgress(techManager.VisibleArmors[techManager.WhichArmorBeingResearched].GetNextLevelCost());
			techFieldProgresses[ARMOR].SetProgress(techManager.VisibleArmors[techManager.WhichArmorBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[SHIELD].SetMaxProgress(techManager.VisibleShields[techManager.WhichShieldBeingResearched].GetNextLevelCost());
			techFieldProgresses[SHIELD].SetProgress(techManager.VisibleShields[techManager.WhichShieldBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[COMPUTER].SetMaxProgress(techManager.VisibleComputers[techManager.WhichComputerBeingResearched].GetNextLevelCost());
			techFieldProgresses[COMPUTER].SetProgress(techManager.VisibleComputers[techManager.WhichComputerBeingResearched].GetTotalResearchPoints());

			techFieldProgresses[INFRASTRUCTURE].SetMaxProgress(techManager.VisibleInfrastructures[techManager.WhichInfrastructureBeingResearched].GetNextLevelCost());
			techFieldProgresses[INFRASTRUCTURE].SetProgress(techManager.VisibleInfrastructures[techManager.WhichInfrastructureBeingResearched].GetTotalResearchPoints());

			SetPercentages(techManager);

			whichField = BEAM;
			foreach (Button button in researchingTechNames)
			{
				button.Selected = false;
			}
			researchingTechNames[whichField].Selected = true;
			techIndex = 0;
			RefreshAvailableTechs();
		}

		private void SetPercentages(TechnologyManager techManager)
		{
			techScrollBars[BEAM].TopIndex = techManager.BeamPercentage;
			techScrollBars[PARTICLE].TopIndex = techManager.ParticlePercentage;
			techScrollBars[MISSILE].TopIndex = techManager.MissilePercentage;
			techScrollBars[TORPEDO].TopIndex = techManager.TorpedoPercentage;
			techScrollBars[BOMB].TopIndex = techManager.BombPercentage;
			techScrollBars[ENGINE].TopIndex = techManager.EnginePercentage;
			techScrollBars[ARMOR].TopIndex = techManager.ArmorPercentage;
			techScrollBars[SHIELD].TopIndex = techManager.ShieldPercentage;
			techScrollBars[COMPUTER].TopIndex = techManager.ComputerPercentage;
			techScrollBars[INFRASTRUCTURE].TopIndex = techManager.InfrastructurePercentage;

			techFieldProgresses[BEAM].SetPotentialProgress((int)((techManager.BeamPercentage * 0.01f) * researchPoints));
			techFieldProgresses[PARTICLE].SetPotentialProgress((int)((techManager.ParticlePercentage * 0.01f) * researchPoints));
			techFieldProgresses[MISSILE].SetPotentialProgress((int)((techManager.MissilePercentage * 0.01f) * researchPoints));
			techFieldProgresses[TORPEDO].SetPotentialProgress((int)((techManager.TorpedoPercentage * 0.01f) * researchPoints));
			techFieldProgresses[BOMB].SetPotentialProgress((int)((techManager.BombPercentage * 0.01f) * researchPoints));
			techFieldProgresses[ENGINE].SetPotentialProgress((int)((techManager.EnginePercentage * 0.01f) * researchPoints));
			techFieldProgresses[ARMOR].SetPotentialProgress((int)((techManager.ArmorPercentage * 0.01f) * researchPoints));
			techFieldProgresses[SHIELD].SetPotentialProgress((int)((techManager.ShieldPercentage * 0.01f) * researchPoints));
			techFieldProgresses[COMPUTER].SetPotentialProgress((int)((techManager.ComputerPercentage * 0.01f) * researchPoints));
			techFieldProgresses[INFRASTRUCTURE].SetPotentialProgress((int)((techManager.InfrastructurePercentage * 0.01f) * researchPoints));
		}

		private void RefreshAvailableTechs()
		{
			TechnologyManager techManager = gameMain.EmpireManager.CurrentEmpire.TechnologyManager;

			switch (whichField)
			{
				case BEAM:
					{
						maxVisible = techManager.VisibleBeams.Count > 12 ? 12 : techManager.VisibleBeams.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleBeams[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleBeams[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleBeams[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleBeams.Count > 12);
					} break;
				case PARTICLE:
					{
						maxVisible = techManager.VisibleParticles.Count > 12 ? 12 : techManager.VisibleParticles.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleParticles[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleParticles[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleParticles[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleParticles.Count > 12);
					} break;
				case MISSILE:
					{
						maxVisible = techManager.VisibleMissiles.Count > 12 ? 12 : techManager.VisibleMissiles.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleMissiles[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleMissiles[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleMissiles[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleMissiles.Count > 12);
					} break;
				case TORPEDO:
					{
						maxVisible = techManager.VisibleTorpedoes.Count > 12 ? 12 : techManager.VisibleTorpedoes.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleTorpedoes[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleTorpedoes[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleTorpedoes[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleTorpedoes.Count > 12);
					} break;
				case BOMB:
					{
						maxVisible = techManager.VisibleBombs.Count > 12 ? 12 : techManager.VisibleBombs.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleBombs[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleBombs[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleBombs[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleBombs.Count > 12);
					} break;
				case ENGINE:
					{
						maxVisible = techManager.VisibleEngines.Count > 12 ? 12 : techManager.VisibleEngines.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleEngines[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleEngines[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleEngines[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleEngines.Count > 12);
					} break;
				case ARMOR:
					{
						maxVisible = techManager.VisibleArmors.Count > 12 ? 12 : techManager.VisibleArmors.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleArmors[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleArmors[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleArmors[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleArmors.Count > 12);
					} break;
				case SHIELD:
					{
						maxVisible = techManager.VisibleShields.Count > 12 ? 12 : techManager.VisibleShields.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleShields[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleShields[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleShields[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleShields.Count > 12);
					} break;
				case COMPUTER:
					{
						maxVisible = techManager.VisibleComputers.Count > 12 ? 12 : techManager.VisibleComputers.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleComputers[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleComputers[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleComputers[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleComputers.Count > 12);
					} break;
				case INFRASTRUCTURE:
					{
						maxVisible = techManager.VisibleInfrastructures.Count > 12 ? 12 : techManager.VisibleInfrastructures.Count;
						for (int i = 0; i < maxVisible; i++)
						{
							availableTechs[i].SetButtonText(techManager.VisibleInfrastructures[i + techIndex].GetNameWithNextLevel());
							availableTechProgresses[i].SetMaxProgress(techManager.VisibleInfrastructures[i + techIndex].GetNextLevelCost());
							availableTechProgresses[i].SetProgress(techManager.VisibleInfrastructures[i + techIndex].GetTotalResearchPoints());
						}
						availableScrollBar.SetEnabledState(techManager.VisibleInfrastructures.Count > 12);
					} break;
			}
		}
	}
}