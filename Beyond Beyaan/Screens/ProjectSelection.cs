using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class ProjectSelection : WindowInterface
	{
		#region Delegated Functions
		public delegate void OkClick(Project project);
		public delegate void CancelClick();
		private OkClick OnOkClick;
		private CancelClick OnCancelClick;
		#endregion

		#region Member Variables
		private List<StarSystem> availableSystems;
		private List<Project> availableProjects;
		private int selectedSystem;
		private int selectedProject;
		#endregion

		#region UI Elements
		private GorgonLibrary.Graphics.RenderTarget oldTarget;
		private GorgonLibrary.Graphics.RenderImage starName;

		private StretchableImage galaxyBackground;
		private StretchableImage infoBackground;

		private InvisibleStretchButton[] systemButtons;
		private InvisibleStretchButton[] projectButtons;

		private ScrollBar systemScrollBar;
		private ScrollBar projectScrollBar;

		private Label[] projectNames;
		private Label[] projectProductionRequired;
		private Label projectCost;

		private Label[] systemAvailableProduction;

		private Button doneButton;
		private Button addButton;

		private int maxSystemsVisible;
		private int maxProjectsVisible;
		private float rotation;
		#endregion

		public ProjectSelection(int x, int y, GameMain gameMain, OkClick okClick, CancelClick cancelClick)
			: base(x, y, 700, 600, string.Empty, gameMain, false)
		{
			backGroundImage = new StretchableImage(x, y, 700, 600, 60, 60, DrawingManagement.BorderBorder);
			galaxyBackground = new StretchableImage(x + 15, y + 285, 300, 300, 30, 13, DrawingManagement.BoxBorder);
			infoBackground = new StretchableImage(x + 320, y + 532, 370, 56, 30, 13, DrawingManagement.BoxBorder);

			starName = new GorgonLibrary.Graphics.RenderImage("starNameRenderedProject", 1, 1, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			starName.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;

			OnOkClick = okClick;
			OnCancelClick = cancelClick;

			systemButtons = new InvisibleStretchButton[6];
			systemAvailableProduction = new Label[systemButtons.Length];

			projectButtons = new InvisibleStretchButton[11];
			projectProductionRequired = new Label[projectButtons.Length];
			projectNames = new Label[projectButtons.Length];

			for (int i = 0; i < systemButtons.Length; i++)
			{
				systemButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + 15, yPos + 15 + (i * 45), 280, 45, 30, 13);
				systemAvailableProduction[i] = new Label(xPos + 255, yPos + 25 + (i * 45));
			}
			for (int i = 0; i < projectButtons.Length; i++)
			{
				projectButtons[i] = new InvisibleStretchButton(DrawingManagement.BoxBorderBG, DrawingManagement.BoxBorderFG, string.Empty, xPos + 315, yPos + 15 + (i * 45), 350, 45, 30, 13);
				projectNames[i] = new Label(xPos + 325, yPos + 25 + (i * 45));
				projectProductionRequired[i] = new Label(xPos + 625, yPos + 25 + (i * 45));
			}
			systemScrollBar = new ScrollBar(xPos + 300, yPos + 15, 16, 238, 6, 6, false, false, DrawingManagement.VerticalScrollBar);
			projectScrollBar = new ScrollBar(xPos + 667, yPos + 15, 16, 463, 12, 12, false, false, DrawingManagement.VerticalScrollBar);

			systemScrollBar.SetEnabledState(false);
			projectScrollBar.SetEnabledState(false);

			addButton = new Button(SpriteName.AddProjectBG, SpriteName.AddProjectFG, string.Empty, xPos + 600, yPos + 540, 80, 40);
			doneButton = new Button(SpriteName.DoneProjectBG, SpriteName.DoneProjectFG, string.Empty, xPos + 510, yPos + 540, 80, 40);

			addButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Add this project", "addThisProjectToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			doneButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Exit", "exitProjectsToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			projectCost = new Label(xPos + 360, yPos + 550);

			maxProjectsVisible = 0;
			maxSystemsVisible = 0;
			rotation = 0;
			selectedProject = -1;
			addButton.Active = false;
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);

			for (int i = 0; i < maxSystemsVisible; i++)
			{
				systemButtons[i].Draw(drawingManagement);
				float percentage = 1.0f;
				oldTarget = GorgonLibrary.Gorgon.CurrentRenderTarget;
				starName.Width = (int)availableSystems[i + systemScrollBar.TopIndex].StarName.GetWidth();
				starName.Height = (int)availableSystems[i + systemScrollBar.TopIndex].StarName.GetHeight();
				starName.Clear(System.Drawing.Color.Transparent);
				GorgonLibrary.Gorgon.CurrentRenderTarget = starName;
				availableSystems[i + systemScrollBar.TopIndex].StarName.MoveTo(0, 0);
				availableSystems[i + systemScrollBar.TopIndex].StarName.Draw();
				GorgonLibrary.Gorgon.CurrentRenderTarget = oldTarget;
				//GorgonLibrary.Gorgon.CurrentShader = gameMain.NameShader;
				foreach (Empire empire in availableSystems[i + systemScrollBar.TopIndex].EmpiresWithSectorsInThisSystem)
				{
					/*gameMain.NameShader.Parameters["EmpireColor"].SetValue(empire.ConvertedColor);
					gameMain.NameShader.Parameters["startPos"].SetValue(percentage);
					gameMain.NameShader.Parameters["endPos"].SetValue(percentage + system.OwnerPercentage[empire]);*/
					starName.Blit(xPos + 25, yPos + 25 + (i * 45), starName.Width * percentage, starName.Height, empire.EmpireColor, GorgonLibrary.Graphics.BlitterSizeMode.Crop);
					percentage -= availableSystems[i + systemScrollBar.TopIndex].OwnerPercentage[empire];
				}
				systemAvailableProduction[i].Draw();
				drawingManagement.DrawSprite(SpriteName.CapacityIcon, xPos + 235, yPos + 28 + (i * 45));
			}
			for (int i = 0; i < maxProjectsVisible; i++)
			{
				projectButtons[i].Draw(drawingManagement);
				projectNames[i].Draw();
				projectProductionRequired[i].Draw();
				drawingManagement.DrawSprite(SpriteName.CapacityIcon, xPos + 605, yPos + 28 + (i * 45));
			}
			systemScrollBar.Draw(drawingManagement);
			projectScrollBar.Draw(drawingManagement);

			galaxyBackground.Draw(drawingManagement);
			DrawGalaxyPreview(drawingManagement);

			infoBackground.Draw(drawingManagement);

			doneButton.Draw(drawingManagement);
			addButton.Draw(drawingManagement);

			drawingManagement.DrawSprite(SpriteName.ConstructionIcon, xPos + 340, yPos + 553);
			projectCost.Draw();

			addButton.DrawToolTip(drawingManagement);
			doneButton.DrawToolTip(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			rotation -= frameDeltaTime * 100;

			bool result = false;
			for (int i = 0; i < maxSystemsVisible; i++)
			{
				result = systemButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			for (int i = 0; i < maxProjectsVisible; i++)
			{
				result = projectButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (systemScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshSystems();
			}
			if (projectScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshProjects();
			}
			result = addButton.MouseHover(x, y, frameDeltaTime) || result;
			result = doneButton.MouseHover(x, y, frameDeltaTime) || result;
			return base.MouseHover(x, y, frameDeltaTime) || result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < maxSystemsVisible; i++)
			{
				result = systemButtons[i].MouseDown(x, y) || result;
			}
			for (int i = 0; i < maxProjectsVisible; i++)
			{
				result = projectButtons[i].MouseDown(x, y) || result;
			}
			result = systemScrollBar.MouseDown(x, y) || result;
			result = projectScrollBar.MouseDown(x, y) || result;
			result = addButton.MouseDown(x, y) || result;
			result = doneButton.MouseDown(x, y) || result;
			return base.MouseDown(x, y) || result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < maxSystemsVisible; i++)
			{
				if (systemButtons[i].MouseUp(x, y))
				{
					result = true;
					availableProjects = availableSystems[i + systemScrollBar.TopIndex].GetAvailableProjects(gameMain.empireManager.CurrentEmpire);
					foreach (InvisibleStretchButton button in systemButtons)
					{
						button.Selected = false;
					}
					systemButtons[i].Selected = true;
					selectedSystem = i + systemScrollBar.TopIndex;
					LoadProjects();
				}
			}
			for (int i = 0; i < maxProjectsVisible; i++)
			{
				if (projectButtons[i].MouseUp(x, y))
				{
					result = true;
					selectedProject = i + projectScrollBar.TopIndex;
					addButton.Active = true;
					foreach (InvisibleStretchButton button in projectButtons)
					{
						button.Selected = false;
					}
					projectButtons[i].Selected = true;
					projectCost.SetText(availableProjects[i + projectScrollBar.TopIndex].Cost.ToString());
				}
			}
			if (systemScrollBar.MouseUp(x, y))
			{
				RefreshSystems();
			}
			if (projectScrollBar.MouseUp(x, y))
			{
				RefreshProjects();
			}
			if (addButton.MouseUp(x, y))
			{
				if (OnOkClick != null)
				{
					OnOkClick(availableProjects[selectedProject]);
					availableProjects = availableSystems[selectedSystem].GetAvailableProjects(gameMain.empireManager.CurrentEmpire);
					selectedProject = -1;
					addButton.Active = false;
					projectCost.SetText("-");
					LoadProjects();
					RefreshSystems();
				}
			}
			if (doneButton.MouseUp(x, y))
			{
				if (OnCancelClick != null)
				{
					OnCancelClick();
				}
			}
			return base.MouseUp(x, y) || result;
		}

		public void LoadData()
		{
			availableSystems = gameMain.empireManager.CurrentEmpire.GetProductiveSystems();
			if (availableSystems.Count > 0)
			{
				availableProjects = availableSystems[0].GetAvailableProjects(gameMain.empireManager.CurrentEmpire);
				systemButtons[0].Selected = true;
				selectedSystem = 0;
			}
			else
			{
				availableProjects = new List<Project>();
			}
			if (availableSystems.Count > 6)
			{
				maxSystemsVisible = 6;
				systemScrollBar.SetEnabledState(true);
				systemScrollBar.SetAmountOfItems(availableSystems.Count);
			}
			else
			{
				maxSystemsVisible = availableSystems.Count;
				systemScrollBar.SetEnabledState(false);
				systemScrollBar.SetAmountOfItems(6);
			}
			LoadProjects();
			RefreshSystems();
		}

		private void LoadProjects()
		{
			/*if (availableProjects.Count > 12)
			{
				maxProjectsVisible = 12;
				projectScrollBar.SetEnabledState(true);
				projectScrollBar.SetAmountOfItems(availableProjects.Count);
			}
			else
			{
				maxProjectsVisible = availableProjects.Count;
				projectScrollBar.SetEnabledState(false);
				projectScrollBar.SetAmountOfItems(12);
			}
			selectedProject = -1;
			addButton.Active = false;
			projectCost.SetText("-");
			RefreshProjects();*/
		}

		private void RefreshProjects()
		{
			/*for (int i = 0; i < maxProjectsVisible; i++)
			{
				projectButtons[i].Selected = false;
				if (i == selectedProject - projectScrollBar.TopIndex)
				{
					projectButtons[i].Selected = true;
				}
				projectNames[i].SetText(availableProjects[i + projectScrollBar.TopIndex].GetPotentialProjectName());
				projectProductionRequired[i].SetText(availableProjects[i + projectScrollBar.TopIndex].ProductionCapacityRequired.ToString());
			}*/
		}

		private void RefreshSystems()
		{
			for (int i = 0; i < maxSystemsVisible; i++)
			{
				systemButtons[i].Selected = false;
				if (i == (selectedSystem - systemScrollBar.TopIndex))
				{
					systemButtons[i].Selected = true;
				}
				systemAvailableProduction[i].SetText(availableSystems[i + systemScrollBar.TopIndex].GetProductionCapacity(gameMain.empireManager.CurrentEmpire).ToString());
			}
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			List<StarSystem> systems = gameMain.galaxy.GetAllStars();
			float x;
			float y;
			foreach (StarSystem system in systems)
			{
				x = (xPos + 20 + (276.0f * (system.X / (float)gameMain.galaxy.GalaxySize)));
				y = (yPos + 290 + (276.0f * (system.Y / (float)gameMain.galaxy.GalaxySize)));
				GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
				if (system.Type.Shader != null)
				{
					if (system.DominantEmpire != null && system.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire))
					{
						system.Type.Shader.Parameters["StarColor"].SetValue(system.DominantEmpire.ConvertedColor);
					}
					else
					{
						system.Type.Shader.Parameters["StarColor"].SetValue(new[] {0.5f, 0.5f, 0.5f, 1});
					}
				}
				system.Sprite.Draw(x, y, 0.1f, 0.1f);
				GorgonLibrary.Gorgon.CurrentShader = null;
			}
			StarSystem systemSelected = availableSystems[selectedSystem];
			x = xPos + 20 + (276.0f * ((systemSelected.X + (systemSelected.Type.Width / 64)) / (float)gameMain.galaxy.GalaxySize));
			y = yPos + 290 + (276.0f * ((systemSelected.Y + (systemSelected.Type.Height / 64)) / (float)gameMain.galaxy.GalaxySize));
			drawingManagement.GetSprite(SpriteName.SelectedStar).Rotation = rotation;
			drawingManagement.DrawSprite(SpriteName.SelectedStar, (int)x, (int)y, 255, systemSelected.Type.Width / 8, systemSelected.Type.Height / 8, System.Drawing.Color.White);
		}
	}
}
