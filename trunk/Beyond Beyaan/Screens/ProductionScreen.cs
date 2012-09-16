using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class ProductionScreen : ScreenInterface
	{
		GameMain gameMain;

		private StretchableImage background;
		private StretchableImage infoBackground;

		private StretchableImage[] percentileBackground;
		private ScrollBar[] percentileSliders;
		private ProgressBar[] progressBars;
		private Button[] lockButtons;
		private Button[] cancelButtons;

		private Label[] productionPointsAllocated;
		private Label[] turnsRemaining;
		private Label[] projectName;
		private Label[] systemName;

		private Button addProjectButton;

		private ProjectManager projectManager;
		private float totalProduction;

		private ScrollBar projectScrollBar;

		private int xPos;
		private int yPos;
		private int maxVisible;

		private ProjectSelection projectSelection;
		private bool selectionVisible;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			xPos = (gameMain.ScreenWidth / 2) - 420;
			yPos = (gameMain.ScreenHeight / 2) - 320;

			background = new StretchableImage(xPos, yPos, 840, 640, 200, 200, DrawingManagement.ScreenBorder);
			infoBackground = new StretchableImage(xPos + 25, yPos + 567, 690, 50, 30, 13, DrawingManagement.BoxBorder);

			percentileSliders = new ScrollBar[9];
			percentileBackground = new StretchableImage[percentileSliders.Length];
			progressBars = new ProgressBar[percentileSliders.Length];
			lockButtons = new Button[percentileSliders.Length];
			cancelButtons = new Button[percentileSliders.Length];
			productionPointsAllocated = new Label[percentileSliders.Length];
			turnsRemaining = new Label[percentileSliders.Length];
			projectName = new Label[percentileSliders.Length];
			systemName = new Label[percentileSliders.Length];

			for (int i = 0; i < percentileSliders.Length; i++)
			{
				percentileBackground[i] = new StretchableImage(xPos + 25, yPos + 25 + (i * 60), 760, 60, 30, 13, DrawingManagement.BoxBorder);
				percentileSliders[i] = new ScrollBar(xPos + 555, yPos + 60 + (i * 60), 16, 168, 1, 101, true, true, DrawingManagement.HorizontalSliderBar);
				progressBars[i] = new ProgressBar(xPos + 345, yPos + 60 + (i * 60), 200, 16, 100, 0, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar, System.Drawing.Color.LightGreen, System.Drawing.Color.Green);
				lockButtons[i] = new Button(SpriteName.LockDisabled, SpriteName.LockEnabled, string.Empty, xPos + 760, yPos + 60 + (i * 60), 16, 16);
				cancelButtons[i] = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, xPos + 760, yPos + 40 + (i * 60), 16, 16);

				productionPointsAllocated[i] = new Label(xPos + 555, yPos + 33 + (i * 60));
				turnsRemaining[i] = new Label(xPos + 345, yPos + 33 + (i * 60));
				projectName[i] = new Label(xPos + 30, yPos + 33 + (i * 60));
				systemName[i] = new Label(xPos + 30, yPos + 57 + (i * 60));
			}
			projectScrollBar = new ScrollBar(xPos + 787, yPos + 25, 16, 508, 9, 1, false, false, DrawingManagement.VerticalScrollBar);
			projectScrollBar.SetEnabledState(false);

			addProjectButton = new Button(SpriteName.AddProjectsBG, SpriteName.AddProjectsFG, string.Empty, xPos + 720, yPos + 573, 80, 40);
			addProjectButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Add projects", "addProjectsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			projectSelection = new ProjectSelection(xPos + 60, yPos + 10, gameMain, OkClick, CancelClick);
			selectionVisible = false;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			gameMain.DrawGalaxyBackground();

			background.Draw(drawingManagement);
			infoBackground.Draw(drawingManagement);

			for (int i = 0; i < maxVisible; i++)
			{
				percentileBackground[i].Draw(drawingManagement);
				percentileSliders[i].Draw(drawingManagement);
				progressBars[i].Draw(drawingManagement);
				lockButtons[i].Draw(drawingManagement);
				cancelButtons[i].Draw(drawingManagement);
				projectName[i].Draw();
				systemName[i].Draw();
				productionPointsAllocated[i].Draw();
				turnsRemaining[i].Draw();
			}

			projectScrollBar.Draw(drawingManagement);
			addProjectButton.Draw(drawingManagement);

			addProjectButton.DrawToolTip(drawingManagement);

			if (selectionVisible)
			{
				projectSelection.DrawWindow(drawingManagement);
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			UpdateBackground(frameDeltaTime);

			if (selectionVisible)
			{
				projectSelection.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			else
			{
				addProjectButton.MouseHover(mouseX, mouseY, frameDeltaTime);

				for (int i = 0; i < projectManager.Projects.Count; i++)
				{
					if (percentileSliders[i].MouseHover(mouseX, mouseY, frameDeltaTime))
					{
						projectManager.SetPercentage(i + projectScrollBar.TopIndex, percentileSliders[i].TopIndex);
						UpdateProjects();
					}
					lockButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
					cancelButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
				}
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (selectionVisible)
			{
				projectSelection.MouseDown(x, y);
			}
			else
			{
				addProjectButton.MouseDown(x, y);
				for (int i = 0; i < projectManager.Projects.Count; i++)
				{
					percentileSliders[i].MouseDown(x, y);
					lockButtons[i].MouseDown(x, y);
					cancelButtons[i].MouseDown(x, y);
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (selectionVisible)
			{
				projectSelection.MouseUp(x, y);
			}
			else
			{
				if (addProjectButton.MouseUp(x, y))
				{
					projectSelection.LoadData();
					selectionVisible = true;
				}
				for (int i = 0; i < projectManager.Projects.Count; i++)
				{
					if (percentileSliders[i].MouseUp(x, y))
					{
						projectManager.SetPercentage(i + projectScrollBar.TopIndex, percentileSliders[i].TopIndex);
						UpdateProjects();
					}
					if (lockButtons[i].MouseUp(x, y))
					{
						lockButtons[i].Selected = !lockButtons[i].Selected;
						projectManager.SetLocked(i + projectScrollBar.TopIndex, lockButtons[i].Selected);
					}
					if (cancelButtons[i].MouseUp(x, y))
					{
						projectManager.CancelProject(i + projectScrollBar.TopIndex);
						RefreshProjects();
					}
				}
			}
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
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
		}

		public void Load()
		{
			projectManager = gameMain.empireManager.CurrentEmpire.ProjectManager;
			totalProduction = gameMain.empireManager.CurrentEmpire.EmpireProduction;
			selectionVisible = false;
			RefreshProjects();
			//planetsCapableOfTerraforming = gameMain.empireManager.CurrentEmpire.GetPlanetsCapableOfTerraforming();
		}

		private void RefreshProjects()
		{
			if (projectManager.Projects.Count > 9)
			{
				maxVisible = 9;
				projectScrollBar.SetEnabledState(true);
				projectScrollBar.SetAmountOfItems(projectManager.Projects.Count);
				projectScrollBar.TopIndex = 0;
			}
			else
			{
				maxVisible = projectManager.Projects.Count;
				projectScrollBar.SetEnabledState(false);
				projectScrollBar.SetAmountOfItems(9);
				projectScrollBar.TopIndex = 0;
			}
			UpdateProjects();
		}

		private void UpdateProjects()
		{
			for (int i = 0; i < maxVisible; i++)
			{
				progressBars[i].SetMaxProgress(projectManager.Projects[i + projectScrollBar.TopIndex].Cost);
				progressBars[i].SetProgress((long)projectManager.Projects[i + projectScrollBar.TopIndex].AmountSoFar);
				progressBars[i].SetPotentialProgress((long)(totalProduction * 0.01 * projectManager.PercentageAmounts[i + projectScrollBar.TopIndex]));
				percentileSliders[i].TopIndex = projectManager.PercentageAmounts[i + projectScrollBar.TopIndex];
				lockButtons[i].Selected = projectManager.Locked[i + projectScrollBar.TopIndex];
				projectName[i].SetText(projectManager.Projects[i + projectScrollBar.TopIndex].ToString());
				systemName[i].SetText("At " + projectManager.Projects[i + projectScrollBar.TopIndex].Location.Name);
				double denomiator = totalProduction * 0.01 * projectManager.PercentageAmounts[i + projectScrollBar.TopIndex];
				productionPointsAllocated[i].SetText(Utility.ConvertNumberToFourDigits(denomiator));
				string turnsRemainingString = string.Empty;
				if (denomiator > 0)
				{
					double turns = (projectManager.Projects[i + projectScrollBar.TopIndex].Cost - projectManager.Projects[i + projectScrollBar.TopIndex].AmountSoFar) / (totalProduction * 0.01 * projectManager.PercentageAmounts[i + projectScrollBar.TopIndex]);
					if (turns >= 1)
					{
						if (turns - (long)turns > 0)
						{
							turns += 1;
						}
						turnsRemainingString = ((long)turns).ToString() + " Turns";
					}
					else
					{
						double excess = (projectManager.Projects[i + projectScrollBar.TopIndex].AmountSoFar + (totalProduction * 0.01 * projectManager.PercentageAmounts[i + projectScrollBar.TopIndex])) - projectManager.Projects[i + projectScrollBar.TopIndex].Cost;
						double amount = excess / projectManager.Projects[i + projectScrollBar.TopIndex].Cost;
						if (amount >= 1)
						{
							turnsRemainingString = ((long)(1 + amount)).ToString() + " Per Turn";
						}
						else
						{
							turnsRemainingString = "1 Turn";
						}
					}
				}
				else
				{
					turnsRemainingString = "Paused";
				}
				turnsRemaining[i].SetText(turnsRemainingString);
			}
		}

		private void OkClick(Project project)
		{
			projectManager.AddProject(project);
			RefreshProjects();
		}

		private void CancelClick()
		{
			selectionVisible = false;
		}
	}
}
