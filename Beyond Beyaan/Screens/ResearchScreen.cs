using System;
using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class ResearchScreen : WindowInterface
	{
		/*private const int BEAM = 0;
		private const int PARTICLE = 1;
		private const int MISSILE = 2;
		private const int TORPEDO = 3;
		private const int BOMB = 4;
		private const int ENGINE = 5;
		private const int ARMOR = 6;
		private const int SHIELD = 7;
		private const int COMPUTER = 8;
		private const int INFRASTRUCTURE = 9;

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
		private int techIndex;*/

		//private BBStretchableImage _background;

		public Action CloseWindow;

		private BBStretchableImage _fieldsBackground;
		private BBStretchableImage _technologyListBackground;

		//The top UI part:
		private BBLabel[] _techFieldLabels;
		private BBLabel[] _techNamesBeingResearchedLabels;
		private BBLabel[] _techProgressLabels;
		private BBScrollBar[] _techSliders;
		private BBButton[] _techLockButtons;
		private BBLabel _totalResearchPointsLabel;

		private BBTextBox _researchedTechnologyDescriptions;
		private BBStretchButton[] _techFieldButtons;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 800, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_fieldsBackground = new BBStretchableImage();
			_technologyListBackground = new BBStretchableImage();
			if (!_fieldsBackground.Initialize(x + 20, y + 20, 760, 230, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_technologyListBackground.Initialize(x + 20, y + 300, 760, 280, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}

			_techFieldLabels = new BBLabel[6];
			_techNamesBeingResearchedLabels = new BBLabel[6];
			_techProgressLabels = new BBLabel[6];
			_techSliders = new BBScrollBar[6];
			_techLockButtons = new BBButton[6];
			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i] = new BBLabel();
				_techNamesBeingResearchedLabels[i] = new BBLabel();
				_techProgressLabels[i] = new BBLabel();
				_techSliders[i] = new BBScrollBar();
				_techLockButtons[i] = new BBButton();
			}
			_totalResearchPointsLabel = new BBLabel();

			if (!_techFieldLabels[0].Initialize(x + 135, y + 35, "Computers:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[1].Initialize(x + 135, y + 65, "Construction:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[2].Initialize(x + 135, y + 95, "Force Fields:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[3].Initialize(x + 135, y + 125, "Planetology:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[4].Initialize(x + 135, y + 155, "Propulsion:", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techFieldLabels[5].Initialize(x + 135, y + 185, "Weapons:", System.Drawing.Color.White, out reason))
			{
				return false;
			}

			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i].SetAlignment(true);
				if (!_techNamesBeingResearchedLabels[i].Initialize(x + 140, y + 35 + (i * 30), "None", System.Drawing.Color.White, out reason))
				{
					return false;
				}
				if (!_techProgressLabels[i].Initialize(x + 545, y + 35 + (i * 30), "N/A", System.Drawing.Color.White, out reason))
				{
					return false;
				}
				_techProgressLabels[i].SetAlignment(true);
				if (!_techSliders[i].Initialize(x + 550, y + 35 + (i * 30), 200, 1, 100, true, true, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_techLockButtons[i].Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.LEFT, x + 755, y + 35 + (i * 30), 16, 16, gameMain.Random, out reason))
				{
					return false;
				}
			}

			if (!_totalResearchPointsLabel.Initialize(x + 765, y + 215, "Total Research Points: 0", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			_totalResearchPointsLabel.SetAlignment(true);

			_researchedTechnologyDescriptions = new BBTextBox();
			if (!_researchedTechnologyDescriptions.Initialize(x + 30, y + 310, 740, 260, true, true, "TechnologyListDescriptions", gameMain.Random, out reason))
			{
				return false;
			}

			_techFieldButtons = new BBStretchButton[6];
			for (int i = 0; i < 6; i++)
			{
				_techFieldButtons[i] = new BBStretchButton();
			}

			if (!_techFieldButtons[0].Initialize("Computers", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 20, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[1].Initialize("Construction", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 147, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[2].Initialize("Force Fields", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 274, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[3].Initialize("Planetology", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 401, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[4].Initialize("Propulsion", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 528, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_techFieldButtons[5].Initialize("Weapons", ButtonTextAlignment.CENTER, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 655, y + 255, 125, 40, gameMain.Random, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}

		public override void Draw()
		{
			base.Draw();

			_fieldsBackground.Draw();
			_technologyListBackground.Draw();

			for (int i = 0; i < 6; i++)
			{
				_techFieldLabels[i].Draw();
				_techNamesBeingResearchedLabels[i].Draw();
				_techProgressLabels[i].Draw();
				_techSliders[i].Draw();
				_techLockButtons[i].Draw();
				_techFieldButtons[i].Draw();
			}
			_totalResearchPointsLabel.Draw();
			_researchedTechnologyDescriptions.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			foreach (var button in _techFieldButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			return result;
		}

		/*public void Update(int x, int y, float frameDeltaTime)
		{
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].MouseHover(x, y, frameDeltaTime))
				{
					switch (i)
					{
						case BEAM: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BEAM, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case PARTICLE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.PARTICLE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case MISSILE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.MISSILE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case TORPEDO: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.TORPEDO, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case BOMB: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BOMB, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ENGINE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ENGINE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ARMOR: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ARMOR, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case SHIELD: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.SHIELD, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case COMPUTER: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.COMPUTER, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case INFRASTRUCTURE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.INFRASTRUCTURE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
					}
				}
			}
			if (availableScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				techIndex = availableScrollBar.TopIndex;
				RefreshAvailableTechs();
				return;
			}
			for (int i = 0; i < lockedButtons.Length; i++)
			{
				lockedButtons[i].MouseHover(x, y, frameDeltaTime);
			}
			for (int i = 0; i < researchingTechNames.Length; i++)
			{
				researchingTechNames[i].MouseHover(x, y, frameDeltaTime);
			}
			for (int i = 0; i < maxVisible; i++)
			{
				availableTechs[i].MouseHover(x, y, frameDeltaTime);
			}
		}*/

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			foreach (var button in _techFieldButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			return base.MouseDown(x, y) || result;
		}

		/*public bool MouseDown(int x, int y, int whichButton)
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
		}*/

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < _techFieldButtons.Length; i++)
			{
				if (_techFieldButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 0: RefreshResearchedTechs(TechField.COMPUTER);
							break;
						case 1: RefreshResearchedTechs(TechField.CONSTRUCTION);
							break;
						case 2: RefreshResearchedTechs(TechField.FORCE_FIELD);
							break;
						case 3: RefreshResearchedTechs(TechField.PLANETOLOGY);
							break;
						case 4: RefreshResearchedTechs(TechField.PROPULSION);
							break;
						case 5: RefreshResearchedTechs(TechField.WEAPON);
							break;
					}
					result = true;
				}
			}
			if (!base.MouseUp(x, y))
			{
				//Clicked outside window, close the window
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return result;
		}

		/*public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = 0; i < techScrollBars.Length; i++)
			{
				if (techScrollBars[i].MouseUp(x, y))
				{
					switch (i)
					{
						case BEAM: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BEAM, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case PARTICLE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.PARTICLE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case MISSILE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.MISSILE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case TORPEDO: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.TORPEDO, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case BOMB: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.BOMB, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ENGINE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ENGINE, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case ARMOR: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.ARMOR, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case SHIELD: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.SHIELD, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case COMPUTER: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.COMPUTER, techScrollBars[i].TopIndex);
							SetPercentages(_gameMain.EmpireManager.CurrentEmpire.TechnologyManager);
							return;
						case INFRASTRUCTURE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.SetPercentage(TechField.INFRASTRUCTURE, techScrollBars[i].TopIndex);
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
						case BEAM: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BeamLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BeamLocked;
							break;
						case PARTICLE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ParticleLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ParticleLocked;
							break;
						case MISSILE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.MissileLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.MissileLocked;
							break;
						case TORPEDO: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.TorpedoLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.TorpedoLocked;
							break;
						case BOMB: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BombLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.BombLocked;
							break;
						case ENGINE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.EngineLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.EngineLocked;
							break;
						case ARMOR: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ArmorLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ArmorLocked;
							break;
						case SHIELD: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ShieldLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ShieldLocked;
							break;
						case COMPUTER: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ComputerLocked;
							break;
						case INFRASTRUCTURE: _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.InfrastructureLocked = !_gameMain.EmpireManager.CurrentEmpire.TechnologyManager.InfrastructureLocked;
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
					TechnologyManager techManager = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager;
					Technology whichTechToUpdate;
					switch (whichField)
					{
						case BEAM:
							{
								techManager.WhichBeamBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleBeams[i + techIndex];
								researchingTechNames[BEAM].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[BEAM].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[BEAM].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[BEAM].SetPotentialProgress((int)(researchPoints * (techManager.BeamPercentage * 0.01f)));
							} break;
						case PARTICLE:
							{
								techManager.WhichParticleBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleParticles[i + techIndex];
								researchingTechNames[PARTICLE].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[PARTICLE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[PARTICLE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[PARTICLE].SetPotentialProgress((int)(researchPoints * (techManager.ParticlePercentage * 0.01f)));
							} break;
						case MISSILE:
							{
								techManager.WhichMissileBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleMissiles[i + techIndex];
								researchingTechNames[MISSILE].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[MISSILE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[MISSILE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[MISSILE].SetPotentialProgress((int)(researchPoints * (techManager.MissilePercentage * 0.01f)));
							} break;
						case TORPEDO:
							{
								techManager.WhichTorpedoBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleTorpedoes[i + techIndex];
								researchingTechNames[TORPEDO].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[TORPEDO].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[TORPEDO].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[TORPEDO].SetPotentialProgress((int)(researchPoints * (techManager.TorpedoPercentage * 0.01f)));
							} break;
						case BOMB:
							{
								techManager.WhichBombBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleBombs[i + techIndex];
								researchingTechNames[BOMB].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[BOMB].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[BOMB].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[BOMB].SetPotentialProgress((int)(researchPoints * (techManager.BombPercentage * 0.01f)));
							} break;
						case ENGINE:
							{
								techManager.WhichEngineBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleEngines[i + techIndex];
								researchingTechNames[ENGINE].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[ENGINE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[ENGINE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[ENGINE].SetPotentialProgress((int)(researchPoints * (techManager.EnginePercentage * 0.01f)));
							} break;
						case ARMOR:
							{
								techManager.WhichArmorBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleArmors[i + techIndex];
								researchingTechNames[ARMOR].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[ARMOR].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[ARMOR].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[ARMOR].SetPotentialProgress((int)(researchPoints * (techManager.ArmorPercentage * 0.01f)));
							} break;
						case SHIELD:
							{
								techManager.WhichShieldBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleShields[i + techIndex];
								researchingTechNames[SHIELD].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[SHIELD].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[SHIELD].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[SHIELD].SetPotentialProgress((int)(researchPoints * (techManager.ShieldPercentage * 0.01f)));
							} break;
						case COMPUTER:
							{
								techManager.WhichComputerBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleComputers[i + techIndex];
								researchingTechNames[COMPUTER].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[COMPUTER].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[COMPUTER].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[COMPUTER].SetPotentialProgress((int)(researchPoints * (techManager.ComputerPercentage * 0.01f)));
							} break;
						case INFRASTRUCTURE:
							{
								techManager.WhichInfrastructureBeingResearched = i + techIndex;
								whichTechToUpdate = techManager.VisibleInfrastructures[i + techIndex];
								researchingTechNames[INFRASTRUCTURE].SetText(whichTechToUpdate.GetNameWithNextLevel());
								techFieldProgresses[INFRASTRUCTURE].SetMaxProgress(whichTechToUpdate.GetNextLevelCost());
								techFieldProgresses[INFRASTRUCTURE].SetProgress(whichTechToUpdate.GetTotalResearchPoints());
								techFieldProgresses[INFRASTRUCTURE].SetPotentialProgress((int)(researchPoints * (techManager.InfrastructurePercentage * 0.01f)));
							} break;
					}
				}
			}
		}*/

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public override bool KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape && CloseWindow != null)
			{
				CloseWindow();
				return true;
			}
			return false;
		}

		public void Load()
		{
			_gameMain.EmpireManager.CurrentEmpire.UpdateResearchPoints(); //To ensure that we have an accurate amount of points
			RefreshFields();
		}

		private void RefreshFields()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;

			_totalResearchPointsLabel.SetText(string.Format("Total Research Points: {0:0.00}", currentEmpire.ResearchPoints));

			if (currentEmpire.TechnologyManager.WhichComputerBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[0].SetText(currentEmpire.TechnologyManager.WhichComputerBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[0].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichConstructionBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[1].SetText(currentEmpire.TechnologyManager.WhichConstructionBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[1].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichForceFieldBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[2].SetText(currentEmpire.TechnologyManager.WhichForceFieldBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[2].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichPlanetologyBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[3].SetText(currentEmpire.TechnologyManager.WhichPlanetologyBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[3].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichPropulsionBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[4].SetText(currentEmpire.TechnologyManager.WhichPropulsionBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[4].SetText("None");
			}
			if (currentEmpire.TechnologyManager.WhichWeaponBeingResearched != null)
			{
				_techNamesBeingResearchedLabels[5].SetText(currentEmpire.TechnologyManager.WhichWeaponBeingResearched.TechName);
			}
			else
			{
				_techNamesBeingResearchedLabels[5].SetText("None");
			}

			RefreshSliders();

			RefreshProgressLabels();

			RefreshLockedStatus();

			RefreshResearchedTechs(TechField.COMPUTER);
		}

		private void RefreshLockedStatus()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			_techLockButtons[0].Selected = currentEmpire.TechnologyManager.ComputerLocked;
			_techLockButtons[1].Selected = currentEmpire.TechnologyManager.ConstructionLocked;
			_techLockButtons[2].Selected = currentEmpire.TechnologyManager.ForceFieldLocked;
			_techLockButtons[3].Selected = currentEmpire.TechnologyManager.PlanetologyLocked;
			_techLockButtons[4].Selected = currentEmpire.TechnologyManager.PropulsionLocked;
			_techLockButtons[5].Selected = currentEmpire.TechnologyManager.WeaponLocked;

			_techSliders[0].SetEnabledState(!currentEmpire.TechnologyManager.ComputerLocked);
			_techSliders[1].SetEnabledState(!currentEmpire.TechnologyManager.ConstructionLocked);
			_techSliders[2].SetEnabledState(!currentEmpire.TechnologyManager.ForceFieldLocked);
			_techSliders[3].SetEnabledState(!currentEmpire.TechnologyManager.PlanetologyLocked);
			_techSliders[4].SetEnabledState(!currentEmpire.TechnologyManager.PropulsionLocked);
			_techSliders[5].SetEnabledState(!currentEmpire.TechnologyManager.WeaponLocked);
		}

		private void RefreshProgressLabels()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			float amount = currentEmpire.ResearchPoints;

			_techProgressLabels[0].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.COMPUTER, amount));
			_techProgressLabels[1].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.CONSTRUCTION, amount));
			_techProgressLabels[2].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.FORCE_FIELD, amount));
			_techProgressLabels[3].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.PLANETOLOGY, amount));
			_techProgressLabels[4].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.PROPULSION, amount));
			_techProgressLabels[5].SetText(currentEmpire.TechnologyManager.GetFieldProgressString(TechField.WEAPON, amount));
		}

		private void RefreshSliders()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;

			_techSliders[0].TopIndex = currentEmpire.TechnologyManager.ComputerPercentage;
			_techSliders[1].TopIndex = currentEmpire.TechnologyManager.ConstructionPercentage;
			_techSliders[2].TopIndex = currentEmpire.TechnologyManager.ForceFieldPercentage;
			_techSliders[3].TopIndex = currentEmpire.TechnologyManager.PlanetologyPercentage;
			_techSliders[4].TopIndex = currentEmpire.TechnologyManager.PropulsionPercentage;
			_techSliders[5].TopIndex = currentEmpire.TechnologyManager.WeaponPercentage;
		}

		private void RefreshResearchedTechs(TechField whichField)
		{
			for (int i = 0; i < _techFieldButtons.Length; i++)
			{
				_techFieldButtons[i].Selected = false;
			}
			string techDescriptions = string.Empty;
			List<Technology> researchedTechs = new List<Technology>();
			switch (whichField)
			{
				case TechField.COMPUTER:
					{
						_techFieldButtons[0].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedComputerTechs;
					} break;
				case TechField.CONSTRUCTION:
					{
						_techFieldButtons[1].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedConstructionTechs;
					} break;
				case TechField.FORCE_FIELD:
					{
						_techFieldButtons[2].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedForceFieldTechs;
					} break;
				case TechField.PLANETOLOGY:
					{
						_techFieldButtons[3].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedPlanetologyTechs;
					} break;
				case TechField.PROPULSION:
					{
						_techFieldButtons[4].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedPropulsionTechs;
					} break;
				case TechField.WEAPON:
					{
						_techFieldButtons[5].Selected = true;
						researchedTechs = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager.ResearchedWeaponTechs;
					} break;
			}
			foreach (var researchedTech in researchedTechs)
			{
				techDescriptions += researchedTech.TechName + " -\r\n" + researchedTech.TechDescription + "\r\n\r\n\r\n";
			}
			_researchedTechnologyDescriptions.SetText(techDescriptions);
			_researchedTechnologyDescriptions.ScrollToBottom();
		}
	}
}
