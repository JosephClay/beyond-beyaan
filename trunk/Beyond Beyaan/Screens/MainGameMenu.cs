using System;
using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Properties;
using GorgonLibrary.InputDevices;
using System.Windows.Forms;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	class MainGameMenu : ScreenInterface
	{
		GameMain gameMain;
		Button[] buttons;
		ComboBoxNoStretch modComboBox;
		Label version;
		int x;
		int y;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			buttons = new Button[5];

			x = (gameMain.ScreenWidth / 2) - 130;

			List<string> directories = new List<string>();
			try
			{
				DirectoryInfo di = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Data"));
				foreach (DirectoryInfo directory in di.GetDirectories())
				{
					if (directory.Name == ".svn")
					{
						continue;
					}
					directories.Add(directory.Name);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(Resources.A_FATAL_ERROR_OCCURED_IN_LOADING_DATA_DIRECTORIES + e.Message);
				Environment.Exit(-1);
			}
			if (directories.Count == 0)
			{
				MessageBox.Show(Resources.NO_DATA_DIRECTORIES_TO_LOAD);
				Environment.Exit(0);
			}

			List<SpriteName> modComboBoxSprites = new List<SpriteName>()
			{
				SpriteName.Mod,
				SpriteName.Mod2,
				SpriteName.ModDown,
				SpriteName.ModScrollUp,
				SpriteName.ModScrollUpHighlighted,
				SpriteName.ModScrollDown,
				SpriteName.ModScrollDownHighlighted,
				SpriteName.ModScrollButton1,
				SpriteName.ModScrollButton2,
				SpriteName.ModScrollButton3,
				SpriteName.ModScrollButton1Highlighted,
				SpriteName.ModScrollButton2Highlighted,
				SpriteName.ModScrollButton3Highlighted,
				SpriteName.ModScrollBar,
				SpriteName.ModScrollBar
			};

			modComboBox = new ComboBoxNoStretch(modComboBoxSprites, directories, 400, gameMain.ScreenHeight - 245, 260, 36, 6);

			buttons[0] = new Button(SpriteName.Continue2, SpriteName.Continue, string.Empty, 400, gameMain.ScreenHeight - 335, 260, 40);
			buttons[1] = new Button(SpriteName.NewGame2, SpriteName.NewGame, string.Empty, 400, gameMain.ScreenHeight - 285, 260, 40);
			buttons[2] = new Button(SpriteName.LoadGame2, SpriteName.LoadGame, string.Empty, 400, gameMain.ScreenHeight - 200, 260, 40);
			buttons[3] = new Button(SpriteName.Options2, SpriteName.Options, string.Empty, 400, gameMain.ScreenHeight - 150, 260, 40);
			buttons[4] = new Button(SpriteName.Exit2, SpriteName.Exit, string.Empty, 400, gameMain.ScreenHeight - 100, 260, 40);

			string versionString = string.Format(Resources.VERSION_0, "0.5.4");
			version = new Label(versionString, 5, gameMain.ScreenHeight - 25);

			x = (gameMain.ScreenWidth / 2) - 512;
			y = 25;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			for (int i = 0; i < gameMain.ScreenWidth; i += 1024)
			{
				for (int j = 0; j < gameMain.ScreenHeight; j += 600)
				{
					drawingManagement.DrawSprite(SpriteName.TitleNebula, i, j, 255, System.Drawing.Color.White);
				}
			}
			drawingManagement.DrawSprite(SpriteName.TitlePlanet, x, y, 255, System.Drawing.Color.White);
			drawingManagement.DrawSprite(SpriteName.TitleName, x, y, 255, System.Drawing.Color.White);
			foreach (Button button in buttons)
			{
				button.Draw(drawingManagement);
			}
			modComboBox.Draw(drawingManagement);
			version.Draw();
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (modComboBox.MouseHover(mouseX, mouseY, frameDeltaTime))
			{
				return;
			}
			foreach (Button button in buttons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (modComboBox.MouseDown(x, y))
			{
				return;
			}
			foreach (Button button in buttons)
			{
				button.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (modComboBox.MouseUp(x, y))
			{
				return;
			}
			for (int i = 0; i < buttons.Length; i++)
			{
				if (buttons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 1:
							string reason;
							if (LoadGameFiles(out reason))
							{
								gameMain.ChangeToScreen(Screen.GalaxySetup);
							}
							else
							{
								gameMain.ClearAll();
							}
							break;
						case 4:
							gameMain.ExitGame();
							break;
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
		}

		private bool LoadGameFiles(out string reason)
		{
			DirectoryInfo directory = new DirectoryInfo(Path.Combine(Path.Combine(Environment.CurrentDirectory, "Data"), modComboBox.CurrentText));
			DirectoryInfo graphicDirectory = new DirectoryInfo(Path.Combine(directory.FullName, "graphics"));

			try
			{
				if (!gameMain.SpriteManager.LoadSprites(directory, graphicDirectory, out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				gameMain.RegionShader = GorgonLibrary.Graphics.FXShader.FromFile(Path.Combine(Path.Combine(directory.FullName, "Shaders"), "CircleShader.fx"), GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
				gameMain.GameDataSet = modComboBox.CurrentText;
				if (!GameConfiguration.LoadConfiguration(Path.Combine(directory.FullName, "config.xml"), out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				if (!gameMain.LoadTutorial(Path.Combine(directory.FullName, "tutorial.xml"), out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				if (!gameMain.iconManager.Initialize(directory.FullName, graphicDirectory.FullName, out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				if (!gameMain.resourceManager.Initialize(directory.FullName, gameMain.iconManager, out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				if (!gameMain.masterItemManager.Initialize(directory, out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				gameMain.masterTechnologyList.ResetAll();
				gameMain.masterTechnologyList.LoadTechnologies(directory.FullName, gameMain.resourceManager, gameMain.masterItemManager);
				gameMain.shipScriptManager.LoadShipScripts(Path.Combine(Path.Combine(directory.FullName, "Scripts"), "Ship"));
				gameMain.raceManager = new RaceManager(directory.FullName, graphicDirectory.FullName, gameMain.masterTechnologyList, gameMain.shipScriptManager, Path.Combine(Path.Combine(directory.FullName, "Scripts"), "Technology"), gameMain.iconManager, gameMain.resourceManager);
				if (!gameMain.sectorTypeManager.LoadSectorTypes(Path.Combine(directory.FullName, "sectorObjects.xml"), gameMain, out reason))
				{
					MessageBox.Show(reason);
					return false;
				}
				gameMain.planetTypeManager.LoadPlanetTypes(Path.Combine(directory.FullName, "planets.xml"), Path.Combine(graphicDirectory.FullName, "planets.png"), graphicDirectory.FullName, gameMain);
				gameMain.regionTypeManager.LoadRegionTypes(Path.Combine(directory.FullName, "regions.xml"), gameMain);
				gameMain.starTypeManager.LoadStarTypes(Path.Combine(directory.FullName, "stars.xml"), Path.Combine(graphicDirectory.FullName, "stars.png"), Path.Combine(directory.FullName, "shaders"), gameMain.sectorTypeManager, gameMain);
				gameMain.particleManager.Initialize(directory.FullName, graphicDirectory.FullName);
				gameMain.effectManager.Initialize(directory.FullName);
				return gameMain.DrawingManagement.LoadGraphics(graphicDirectory.FullName, out reason);
			}
			catch (Exception e)
			{
				//handle error here
				MessageBox.Show(e.Message + "\n" + e.StackTrace);
				reason = e.Message;
				return false;
			}
		}
	}
}
