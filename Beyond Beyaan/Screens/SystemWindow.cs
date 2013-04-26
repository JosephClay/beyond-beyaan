using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class SystemWindow : WindowInterface
	{
		//private GorgonLibrary.Graphics.Sprite shipSprite;
		/*private InvisibleStretchButton[] planetButtons;
		private InvisibleStretchButton systemButton;
		private Label[] planetNames;
		private Label[] planetPopulationAndRegions;
		
		/*private Button prevShip;
		private Button nextShip;*/
		
		//private Label planetOwner;
		
		/*private int maxVisible;
		private int offSet;
		private int centerY;
		private int y;

		
		Planet selectedPlanet;

		private Label systemNameLabel;
		//private TextBox systemDescription;

		private bool emptySystem;*/
		//private PlanetWindow planetWindow;

		//private GorgonLibrary.Graphics.FXShader angleShader;

		private StretchableImage systemBackground;
		private StretchableImage resourcesBackground;
		private StretchableImage projectsBackground;
		private StretchableImage regionsBackground;

		private StarSystem selectedSystem;
		private SingleLineTextBox systemName;
		private TextBox systemDescription;
		private TextBox resourcesDisplay;

		private GorgonLibrary.Graphics.RenderImage target;

		private List<SectorControl> sectorControls;
		private ScrollBar sectorScrollBar;

		public SystemWindow(int centerX, int centerY, GameMain gameMain) : 
			base (centerX - 420, centerY - 320, 655, 440, null, gameMain, true)
		{
			systemBackground = new StretchableImage(xPos + 15, yPos + 15, 300, 200, 30, 13, DrawingManagement.BoxBorder);
			resourcesBackground = new StretchableImage(xPos + 15, yPos + 215, 300, 100, 30, 13, DrawingManagement.BoxBorder);
			projectsBackground = new StretchableImage(xPos + 15, yPos + 315, 300, 100, 30, 13, DrawingManagement.BoxBorder);
			regionsBackground = new StretchableImage(xPos + 317, yPos + 15, 320, 400, 30, 13, DrawingManagement.BoxBorder);
			sectorScrollBar = new ScrollBar(xPos + 615, yPos + 22, 16, 348, 380, 380, false, false, DrawingManagement.VerticalScrollBar, gameMain.FontManager.GetDefaultFont());

			target = new GorgonLibrary.Graphics.RenderImage("systemWindowRender", 290, 380, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			target.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;

			//planetOwner = new Label(x + 10, y + 300);

			selectedSystem = null;
			systemName = new SingleLineTextBox(xPos + 98, yPos + 28, 210, 35, DrawingManagement.TextBox, gameMain.FontManager.GetDefaultFont());
			systemDescription = new TextBox(xPos + 28, yPos + 95, 280, 100, "systemDescriptionTextBox", string.Empty, gameMain.FontManager.GetDefaultFont(), DrawingManagement.VerticalScrollBar);
			resourcesDisplay = new TextBox(xPos + 20, yPos + 225, 290, 80, "systemResourcesDisplay", string.Empty, gameMain.FontManager.GetDefaultFont(), DrawingManagement.VerticalScrollBar);

			sectorControls = new List<SectorControl>();
			//selectedPlanet = null;
			//offSet = 0;
			//this.centerY = centerY;

			//maxVisible = 0;
			//systemNameLabel = new Label(0, 0);

			//systemDescription = new TextBox(xPos - 135, 30, 270, 90, "systemDescription", string.Empty, gameMain.DrawingManagement.GetDefaultFont(), DrawingManagement.VerticalScrollBar);

			//planetWindow = new PlanetWindow(centerX, centerY, gameMain);

			//angleShader = GorgonLibrary.Graphics.FXShader.FromFile(@"E:\Brent's Projects\Current Projects\Beyond Beyaan Trunk\Beyond Beyaan\bin\Debug\Data\Default\Shaders\CircleShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			backGroundImage.Draw(drawingManagement);
			systemBackground.Draw(drawingManagement);
			resourcesBackground.Draw(drawingManagement);
			regionsBackground.Draw(drawingManagement);
			projectsBackground.Draw(drawingManagement);

			GorgonLibrary.Gorgon.CurrentShader = selectedSystem.Type.Shader;
			if (selectedSystem.Type.Shader != null)
			{
				selectedSystem.Type.Shader.Parameters["ShaderValue"].SetValue(selectedSystem.Type.ShaderValue);
			}
			selectedSystem.Sprite.Draw(xPos + 58, yPos + 55, 60.0f / selectedSystem.Sprite.Width, 60.0f / selectedSystem.Sprite.Height);
			GorgonLibrary.Gorgon.CurrentShader = null;

			systemName.Draw(drawingManagement);
			systemDescription.Draw(drawingManagement);

			resourcesDisplay.Draw(drawingManagement);

			sectorScrollBar.Draw(drawingManagement);

			GorgonLibrary.Graphics.RenderTarget old = GorgonLibrary.Gorgon.CurrentRenderTarget;
			GorgonLibrary.Gorgon.CurrentRenderTarget = target;
			GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(System.Drawing.Color.FromArgb(0,255,255,255));
			int height = 0;
			for (int i = 0; i < sectorControls.Count; i++)
			{
				if (height + sectorControls[i].Height < sectorScrollBar.TopIndex || height > sectorScrollBar.TopIndex + 380)
				{
					height += sectorControls[i].Height;
					//control is not visible, skip
					continue;
				}
				sectorControls[i].Draw(drawingManagement, 0, height - sectorScrollBar.TopIndex);
				height += sectorControls[i].Height;
			}
			GorgonLibrary.Gorgon.CurrentRenderTarget = old;
			target.Blit(xPos + 323, yPos + 21);

			

			//systemDescription.Draw(drawingManagement);
			/*systemButton.Draw(drawingManagement);
			systemNameLabel.Draw();
			GorgonLibrary.Gorgon.CurrentShader = selectedSystem.Type.Shader;
			if (selectedSystem.Type.Shader != null)
			{
				selectedSystem.Type.Shader.Parameters["ShaderValue"].SetValue(selectedSystem.Type.ShaderValue);
			}
			selectedSystem.Type.Sprite.SetPosition(40, y + 55);
			selectedSystem.Type.Sprite.SetScale(60.0f / selectedSystem.Type.Sprite.Width, 60.0f / selectedSystem.Type.Sprite.Height);
			selectedSystem.Type.Sprite.Draw();
			GorgonLibrary.Gorgon.CurrentShader = null;
			if (emptySystem || !selectedSystem.Type.Inhabitable || !selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire))
			{
				return;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				planetButtons[i].Draw(drawingManagement);
				// TODO: Optimize this part of code
				if (selectedSystem.Planets[i].PlanetType.Inhabitable)
				{
					int count = selectedSystem.Planets[i].Regions.Count;
					int tempY = y + 95 + (i * 70);
					for (int j = 0; j < count; j++)
					{
						List<string> keys = new List<string>{ "Arc", "CircleColor" };
						List<float[]> values = new List<float[]> 
						{
							new[] { 0.5f, (float)(j * (float)(Math.PI * 2 / count) - Math.PI), (float)(j * (float)(Math.PI * 2 / count) + (float)(Math.PI * 2 / count) - Math.PI) },
							new[] { selectedSystem.Planets[i].Regions[j].RegionType.Color[0] * (j % 2 == 0 ? 1.0f : 0.8f), selectedSystem.Planets[i].Regions[j].RegionType.Color[1] * (j % 2 == 0 ? 1.0f : 0.8f), selectedSystem.Planets[i].Regions[j].RegionType.Color[2] * (j % 2 == 0 ? 1.0f : 0.8f), 1.0f }
						};
						GorgonLibrary.Graphics.RenderImage image = DrawingManagement.GetSpriteWithShader(drawingManagement.GetSprite(SpriteName.RegionRing), gameMain.RegionShader, keys, values);
						image.Blit(10, tempY);
					}
				}
				GorgonLibrary.Graphics.Sprite planet = selectedSystem.Planets[i].PlanetType.Sprite;
				planet.SetPosition(20, y + 105 + (i * 70));
				planet.Color = System.Drawing.Color.White;
				planet.Draw();

				/*GorgonLibrary.Graphics.RenderImage image = DrawingManagement.GetSpriteWithShader(planet, angleShader, new List<string> { "Arc", "CircleColor" }, new List<float[]> { new[] { 0.5f, 0.0f, 2f }, new[] { 1.0f, 0.0f, 0.0f, 1.0f } }); 

				image.Blit(xPos - offSet + (i * 70) + 15, yPos + 35);*/

				/*planetNames[i].Draw();
				planetPopulationAndRegions[i].Draw();
				/*if (selectedSystem.Planets[i].ConstructionBonus != PLANET_CONSTRUCTION_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetConstructionBonusToSprite(selectedSystem.Planets[i].ConstructionBonus), xPos - offSet + (i * 70) + 10, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EnvironmentBonus != PLANET_ENVIRONMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEnvironmentBonusToSprite(selectedSystem.Planets[i].EnvironmentBonus), xPos - offSet + (i * 70) + 27, yPos + 95, 255, System.Drawing.Color.White);
				}
				if (selectedSystem.Planets[i].EntertainmentBonus != PLANET_ENTERTAINMENT_BONUS.AVERAGE)
				{
					drawingManagement.DrawSprite(Utility.PlanetEntertainmentBonusToSprite(selectedSystem.Planets[i].EntertainmentBonus), xPos - offSet + (i * 70) + 44, yPos + 95, 255, System.Drawing.Color.White);
				}*/
			/*}
			if (selectedPlanet != null)
			{
				if (selectedPlanet.PlanetType.Inhabitable)
				{
					planetWindow.DrawWindow(drawingManagement);
				}
			}*/
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = resourcesDisplay.MouseUp(x, y);
			result = sectorScrollBar.MouseUp(x, y) || result;
			/*bool result = false;
			if (selectedPlanet != null)
			{
				if (!planetWindow.MouseUp(x, y))
				{
					//disable this window
					selectedPlanet = null;
					foreach (var button in planetButtons)
					{
						button.Selected = false;
					}
					result = true;
				}
			}
			if (!selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire) || !selectedSystem.Type.Inhabitable)
			{
				//result = systemDescription.MouseUp(x, y) || result;
			}
			if (systemButton.MouseUp(x, y))
			{
				result = true;
				foreach (InvisibleStretchButton button in planetButtons)
				{
					button.Selected = false;
				}
				systemButton.Selected = true;
				selectedSystem.SetPlanetSelected(gameMain.empireManager.CurrentEmpire, -1);
				selectedPlanet = null;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (planetButtons[i].MouseUp(x, y))
				{
					foreach (InvisibleStretchButton button in planetButtons)
					{
						button.Selected = false;
					}
					systemButton.Selected = false;
					planetButtons[i].Selected = true;
					selectedSystem.SetPlanetSelected(gameMain.empireManager.CurrentEmpire, i);
					selectedPlanet = selectedSystem.Planets[i];
					planetWindow.LoadPlanet(selectedPlanet);
					return true;
				}
			}
			if (x >= xPos - (windowHeight / 2) && x < xPos + (windowWidth / 2) && y >= yPos && y < yPos + windowHeight)
			{
				result = true;
				selectedPlanet = null;
				selectedSystem.SetPlanetSelected(gameMain.empireManager.CurrentEmpire, -1);
				foreach (InvisibleStretchButton button in planetButtons)
				{
					button.Selected = false;
				}
			}*/
			return result || base.MouseUp(x, y);
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = resourcesDisplay.MouseDown(x, y);
			result = sectorScrollBar.MouseDown(x, y) || result;
			/*bool result = false;
			if (selectedPlanet != null)
			{
				result = planetWindow.MouseDown(x, y);
			}
			if (!selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire) || !selectedSystem.Type.Inhabitable)
			{
				//result = systemDescription.MouseDown(x, y) || result;
			}
			result = systemButton.MouseDown(x, y) || result;
			foreach (InvisibleStretchButton button in planetButtons)
			{
				result = button.MouseDown(x, y) || result;
			}
			if (x >= xPos - (windowHeight / 2) && x < xPos + (windowWidth / 2) && y >= yPos && y < yPos + windowHeight)
			{
				result = true;
			}*/
			return result || base.MouseDown(x, y);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = resourcesDisplay.MouseHover(x, y, frameDeltaTime);
			result = sectorScrollBar.MouseHover(x, y, frameDeltaTime) || result;
			/*bool result = false;
			if (selectedPlanet != null)
			{
				result = planetWindow.MouseHover(x, y, frameDeltaTime);
			}
			if (!selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire) || !selectedSystem.Type.Inhabitable)
			{
				//result = systemDescription.MouseHover(x, y, frameDeltaTime) || result;
			}
			result = systemButton.MouseHover(x, y, frameDeltaTime) || result;
			foreach (InvisibleStretchButton button in planetButtons)
			{
				result = button.MouseHover(x, y, frameDeltaTime) || result;
			}
			if (x >= xPos - (windowHeight / 2) && x < xPos + (windowWidth / 2) && y >= yPos && y < yPos + windowHeight)
			{
				result = true;
			}*/
			return result || base.MouseHover(x, y, frameDeltaTime);
		}

		public override void MoveWindow()
		{
			base.MoveWindow();
			systemBackground.MoveTo(xPos + 15, yPos + 15);
			resourcesBackground.MoveTo(xPos + 15, yPos + 215);
			projectsBackground.MoveTo(xPos + 15, yPos + 315);
			regionsBackground.MoveTo(xPos + 317, yPos + 15);
			sectorScrollBar.MoveTo(xPos + 615, yPos + 22);
			systemName.MoveTo(xPos + 98, yPos + 28);
			systemDescription.MoveTo(xPos + 28, yPos + 95);
			resourcesDisplay.MoveTo(xPos + 20, yPos + 225);
		}

		/*private void RefreshPlanets()
		{
			for (int i = 0; i < planetButtons.Length; i++)
			{
				planetNumericNames[i] = new Label(selectedSystem.Planets[i].NumericName, 0, 0, selectedSystem.Planets[i].Owner == null ? System.Drawing.Color.White : selectedSystem.Planets[i].Owner.EmpireColor);
				planetNumericNames[i].MoveTo(xPos - offSet + (70 * i) - (int)(planetNumericNames[i].GetWidth() / 2) + 35, yPos + 73);
			}
			int index = selectedSystem.GetPlanetSelected(gameMain.empireManager.CurrentEmpire);
			foreach (InvisibleStretchButton button in planetButtons)
			{
				button.Selected = false;
			}
			if (index >= 0 && index < maxVisible)
			{
				planetButtons[index].Selected = true;
			}
		}*/

		public void LoadSystem()
		{
			var currentEmpire = gameMain.empireManager.CurrentEmpire;
			selectedSystem = gameMain.empireManager.CurrentEmpire.SelectedSystem;
			if (selectedSystem != null)
			{
				systemDescription.SetMessage(selectedSystem.Type.Description);
				sectorControls.Clear();

				if (selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire))
				{
					systemName.SetString(selectedSystem.Name);

					int total = 0;
					foreach (SectorObject sector in selectedSystem.SectorObjects)
					{
						var sectorControl = new SectorControl(sector, gameMain);
						sectorControls.Add(sectorControl);
						total += sectorControl.Height;
					}
					sectorScrollBar.SetAmountOfItems(total);
					sectorScrollBar.SetEnabledState(total > 370);
					sectorScrollBar.TopIndex = 0;
				}
				else
				{
					systemName.SetString("Unexplored");

					resourcesDisplay.SetMessage(string.Empty);

					sectorScrollBar.SetAmountOfItems(380);
					sectorScrollBar.TopIndex = 0;
					sectorScrollBar.SetEnabledState(false);
				}
			}

			/*if (selectedSystem.IsThisSystemExploredByEmpire(gameMain.empireManager.CurrentEmpire))
			{
				y = centerY - (selectedSystem.Planets.Count * 35) - 35;
				int height = selectedSystem.Planets.Count * 70 + 120;

				systemNameLabel.MoveTo(80, y + 25);

				backGroundImage = new StretchableImage(-30, y, 305, height, 30, 13, DrawingManagement.BoxBorderBG);

				systemNameLabel.SetText(selectedSystem.Name);
				if (selectedSystem.DominantEmpire != null)
				{
					systemNameLabel.SetColor(selectedSystem.DominantEmpire.EmpireColor);
				}
				else
				{
					systemNameLabel.SetColor(System.Drawing.Color.White);
				}

				if (selectedSystem.Planets.Count == 0)
				{
					emptySystem = true;
					//systemDescription.SetMessage("This system has no planets.");
					windowHeight = 140;
					windowWidth = 300;
					backGroundImage.MoveTo(xPos - 150, yPos);
					backGroundImage.SetDimensions(windowWidth, windowHeight);
					selectedPlanet = null;
					return;
				}
				emptySystem = false;
				//systemDescription.SetMessage(string.Empty);

				maxVisible = selectedSystem.Planets.Count;
				offSet = (int)((maxVisible / 2.0f) * 70);

				planetButtons = new InvisibleStretchButton[maxVisible];
				systemButton = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, 0, y + 20, 265, 70, 10, 10);
				planetNames = new Label[maxVisible];
				planetPopulationAndRegions = new Label[maxVisible];
				for (int i = 0; i < planetButtons.Length; i++)
				{
					planetButtons[i] = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, 0, y + 90 + (i * 70), 265, 70, 10, 10);
					planetNames[i] = new Label(selectedSystem.Planets[i].Name, 0, 0, selectedSystem.Planets[i].Owner == null ? System.Drawing.Color.White : selectedSystem.Planets[i].Owner.EmpireColor);
					planetNames[i].MoveTo(80, y + 92 + (i * 70));
					if (selectedSystem.Planets[i].Owner != null)
					{
						planetPopulationAndRegions[i] = new Label(string.Format("{0:0}%", (selectedSystem.Planets[i].SpaceUsage / (selectedSystem.Planets[i].Regions.Count * 10)) * 100.0f) + " (" + selectedSystem.Planets[i].Regions.Count + ")", 0, 0, System.Drawing.Color.White);
					}
					else
					{
						if (selectedSystem.Planets[i].PlanetType.Inhabitable)
						{
							planetPopulationAndRegions[i] = new Label("0% (" + selectedSystem.Planets[i].Regions.Count + ")", 0, 0, System.Drawing.Color.White);
						}
						else
						{
							planetPopulationAndRegions[i] = new Label("Uninhabitable", 0, 0, System.Drawing.Color.White);
						}
					}
					planetPopulationAndRegions[i].MoveTo(80, y + 112 + (i * 70));
				}
				int whichPlanetSelected = selectedSystem.GetPlanetSelected(gameMain.empireManager.CurrentEmpire);
				if (whichPlanetSelected < maxVisible && whichPlanetSelected >= 0)
				{
					planetButtons[selectedSystem.GetPlanetSelected(gameMain.empireManager.CurrentEmpire)].Selected = true;
					selectedPlanet = selectedSystem.Planets[whichPlanetSelected];
					//planetWindow.LoadPlanet(selectedPlanet, whichPlanetSelected * 70 - offSet + 35 + xPos, yPos + windowHeight);
				}
				else
				{
					selectedPlanet = null;
				}
			}
			else
			{
				y = centerY - 35;
				int height = 120;

				systemNameLabel.MoveTo(80, y + 25);
				systemNameLabel.SetText(selectedSystem.Type.Name);
				systemNameLabel.SetColor(System.Drawing.Color.White);
				//systemDescription.SetMessage(selectedSystem.Type.Description);
				selectedPlanet = null;
				maxVisible = 0;

				backGroundImage = new StretchableImage(-30, y, 305, height, 30, 13, DrawingManagement.BoxBorderBG);
			}*/
		}
	}
}
