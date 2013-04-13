using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GorgonLibrary.InputDevices;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Beyond_Beyaan.Screens
{
	class GalaxySetup : ScreenInterface
	{
		private GameMain gameMain;

		private Dictionary<string, UIElement> uiElements;
		private List<string> uiKeys;
		private List<Label> uiLabels;
		private ScrollBar uiScrollBar;
		private int maxVisible;

		private ComboBox whichGalaxyScript;

		private Label galaxyScriptLabel;
		private Label numOfStars;

		private System.Reflection.MethodInfo genGalaxyFunc;
		private Object scriptInstance;
		private Dictionary<string, string> vars;

		private StretchButton nextScreen;
		private StretchButton prevScreen;
		private StretchButton battleSimulator;
		private StretchButton generateGalaxyButton;

		private StretchableImage background;
		private StretchableImage galaxyBackground;
		private StretchableImage galaxyConfigurationBackground;
		private StretchableImage gameConfigurationBackground;
		private StretchableImage galaxyScriptBackground;

		#region Game Options
		private CheckBox permanentAlliance;
		#endregion

		private int xScreenPos;
		private int yScreenPos;

		private SelectBattle selectBattle;
		private bool showingBattle;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;
			xScreenPos = (gameMain.ScreenWidth / 2) - 400;
			yScreenPos = (gameMain.ScreenHeight / 2) - 300;

			uiElements = new Dictionary<string, UIElement>();
			uiLabels = new List<Label>();

			System.IO.DirectoryInfo scriptsDir = new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Demo\\Scripts\\Galaxy");
			System.IO.FileInfo[] scripts = scriptsDir.GetFiles("*.cs");
			List<string> scriptNames = new List<string>();
			for (int i = 0; i < scripts.Length; i++)
			{
				scriptNames.Add(scripts[i].Name.Substring(0, scripts[i].Name.Length - 3));
			}

			whichGalaxyScript = new ComboBox(DrawingManagement.ComboBox, scriptNames, xScreenPos + 145, yScreenPos + 25, 290, 35, 6, true);

			galaxyScriptLabel = new Label("Galaxy Script: ", xScreenPos + 140, yScreenPos + 32);
			galaxyScriptLabel.SetAlignment(true);

			prevScreen = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Main Menu", xScreenPos + 15, yScreenPos + 560, 200, 35);
			nextScreen = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Players Setup", xScreenPos + 575, yScreenPos + 560, 200, 35);
			battleSimulator = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Battle Simulator", xScreenPos + 300, yScreenPos + 560, 200, 35);
			generateGalaxyButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Generate Galaxy", xScreenPos + 165, yScreenPos + 505, 175, 35);
			generateGalaxyButton.Active = false;

			background = new StretchableImage(xScreenPos - 30, yScreenPos - 30, 860, 660, 200, 200, DrawingManagement.ScreenBorder);
			galaxyBackground = new StretchableImage(xScreenPos + 475, yScreenPos + 10, 320, 320, 60, 60, DrawingManagement.BorderBorder);
			galaxyConfigurationBackground = new StretchableImage(xScreenPos, yScreenPos + 10, 470, 545, 60, 60, DrawingManagement.BorderBorder);
			gameConfigurationBackground = new StretchableImage(xScreenPos + 475, yScreenPos + 370, 320, 185, 60, 60, DrawingManagement.BorderBorder);
			galaxyScriptBackground = new StretchableImage(xScreenPos + 10, yScreenPos + 70, 450, 430, 30, 13, DrawingManagement.BoxBorder);

			uiScrollBar = new ScrollBar(xScreenPos + 437, yScreenPos + 80, 16, 360, 14, 14, false, false, DrawingManagement.VerticalScrollBar);
			uiScrollBar.SetEnabledState(false);

			numOfStars = new Label(0, 0);

			permanentAlliance = new CheckBox(DrawingManagement.CheckBox, "Permanent Alliances", xScreenPos + 490, yScreenPos + 385, 200, 35, 27, false);

			LoadGalaxyScript();

			showingBattle = false;
			selectBattle = new SelectBattle(gameMain.ScreenWidth / 2, gameMain.ScreenHeight / 2, gameMain, CancelBattle, CommenceBattle);
			nextScreen.Active = false;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			/*for (int i = 0; i < gameMain.ScreenWidth; i += 1024)
			{
				for (int j = 0; j < gameMain.ScreenHeight; j += 600)
				{
					drawingManagement.DrawSprite(SpriteName.TitleNebula, i, j, 255, System.Drawing.Color.White);
				}
			}*/
			background.Draw(drawingManagement);
			DrawGalaxyPreview(drawingManagement);
			galaxyConfigurationBackground.Draw(drawingManagement);
			gameConfigurationBackground.Draw(drawingManagement);
			galaxyScriptBackground.Draw(drawingManagement);
			
			galaxyScriptLabel.Draw();

			foreach (string key in uiElements.Keys)
			{
				uiElements[key].Draw(drawingManagement);
			}

			for (int i = 0; i < uiElements.Count; i++)
			{
				uiLabels[i].Draw();
			}
			uiScrollBar.Draw(drawingManagement);
			generateGalaxyButton.Draw(drawingManagement);

			prevScreen.Draw(drawingManagement);
			nextScreen.Draw(drawingManagement);
			battleSimulator.Draw(drawingManagement);

			whichGalaxyScript.Draw(drawingManagement);

			permanentAlliance.Draw(drawingManagement);

			numOfStars.Draw();

			if (showingBattle)
			{
				selectBattle.DrawWindow(drawingManagement);
			}
		}

		public void UpdateBackground(float frameDeltaTime)
		{
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			if (showingBattle)
			{
				selectBattle.MouseHover(x, y, frameDeltaTime);
				return;
			}
			if (whichGalaxyScript.MouseHover(x, y, frameDeltaTime))
			{
				return;
			}
			foreach (string key in uiElements.Keys)
			{
				uiElements[key].MouseHover(x, y, frameDeltaTime);
			}
			prevScreen.MouseHover(x, y, frameDeltaTime);
			nextScreen.MouseHover(x, y, frameDeltaTime);
			battleSimulator.MouseHover(x, y, frameDeltaTime);

			permanentAlliance.MouseHover(x, y, frameDeltaTime);

			generateGalaxyButton.MouseHover(x, y, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (showingBattle)
			{
				selectBattle.MouseDown(x, y);
				return;
			}
			if (whichGalaxyScript.MouseDown(x, y))
			{
				return;
			}

			foreach (string key in uiElements.Keys)
			{
				uiElements[key].MouseDown(x, y);
			}
			prevScreen.MouseDown(x, y);
			nextScreen.MouseDown(x, y);
			battleSimulator.MouseDown(x, y);

			permanentAlliance.MouseDown(x, y);

			generateGalaxyButton.MouseDown(x, y);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (showingBattle)
			{
				selectBattle.MouseUp(x, y);
				return;
			}
			if (whichGalaxyScript.MouseUp(x, y) && !whichGalaxyScript.IsDroppedDown)
			{
				LoadGalaxyScript();
				return;
			}

			foreach (string key in uiElements.Keys)
			{
				uiElements[key].MouseUp(x, y);
			}

			if (prevScreen.MouseUp(x, y))
			{
				gameMain.ChangeToScreen(ScreenEnum.MainMenu);
			}
			if (nextScreen.MouseUp(x, y) && gameMain.galaxy.GalaxySize > 0)
			{
				gameMain.ChangeToScreen(ScreenEnum.PlayerSetup);
			}

			permanentAlliance.MouseUp(x, y);

			if (generateGalaxyButton.MouseUp(x, y))
			{
				List<string> keys = new List<string>();
				foreach (string key in vars.Keys)
				{
					keys.Add(key);
				}

				foreach (string key in keys)
				{
					vars[key] = ((NumericUpDown)uiElements[key]).Value.ToString();
				}
				gameMain.galaxy.GenerateGalaxy(genGalaxyFunc, scriptInstance, vars, gameMain.StarTypeManager, gameMain.SectorTypeManager, gameMain.SpriteManager);
				if (gameMain.galaxy.GetAllStars().Count > 0)
				{
					nextScreen.Active = true;
				}
				numOfStars.SetText(gameMain.galaxy.GetAllStars().Count.ToString() + " Stars");
				numOfStars.MoveTo(xScreenPos + 650 - (int)(numOfStars.GetWidth() / 2), yScreenPos + 350 - (int)(numOfStars.GetHeight() / 2));
			}

			if (battleSimulator.MouseUp(x, y))
			{
				if (gameMain.ShipShader == null)
				{
					gameMain.ShipShader = GorgonLibrary.Graphics.FXShader.FromFile("ColorShader.fx", GorgonLibrary.Graphics.ShaderCompileOptions.OptimizationLevel3);
				}
				selectBattle.LoadBattles();
				showingBattle = true;
			}
		}

		public void MouseScroll(int direction, int xScreenPos, int yScreenPos)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}

		public void Resize()
		{
		}

		public void LoadGalaxyScript()
		{
			CompilerParameters cp = new CompilerParameters();
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = true;
			cp.ReferencedAssemblies.Add("system.dll");
			//cp.ReferencedAssemblies.Add("system.collections.generic.dll");
			//cp.ReferencedAssemblies.Add("system.linq.dll");
			//cp.ReferencedAssemblies.Add("system.text.dll");

			CSharpCodeProvider provider = new CSharpCodeProvider();

			string fname = Environment.CurrentDirectory + "\\Data\\" + gameMain.GameDataSet + "\\Scripts\\Galaxy\\" + whichGalaxyScript.CurrentText + ".cs";
			CompilerResults result = provider.CompileAssemblyFromFile(cp, fname);

			if (result.Errors.HasErrors)
			{
				string errors = string.Empty;
				foreach (CompilerError err in result.Errors)
				{
					errors += err.ToString() + "\n";
				}
				MessageBox.Show(errors);
				return;
			}

			System.Reflection.Assembly a = result.CompiledAssembly;
			scriptInstance = a.CreateInstance("Beyond_Beyaan.GalaxyGenerator", false, System.Reflection.BindingFlags.ExactBinding, null, null, null, null);
			System.Reflection.MethodInfo getVars = a.GetType("Beyond_Beyaan.GalaxyGenerator").GetMethod("GetVariables");
			genGalaxyFunc = a.GetType("Beyond_Beyaan.GalaxyGenerator").GetMethod("Generate");

			Object ret = getVars.Invoke(scriptInstance, null);

			vars = (Dictionary<string, string>)ret;

			uiElements = new Dictionary<string, UIElement>();
			uiLabels = new List<Label>();
			uiKeys = new List<string>();

			try
			{
				foreach (string key in vars.Keys)
				{
					Label label = new Label(key, 0, 0);
					uiLabels.Add(label);

					string[] parts = vars[key].Split(new[] { '|' });

					switch (parts[0])
					{
						case "upDown":
							{
								int min = int.Parse(parts[1]);
								int max = int.Parse(parts[2]);
								int increment = int.Parse(parts[3]);
								int initialAmount = int.Parse(parts[4]);
								NumericUpDown upDown = new NumericUpDown(0, 0, 100, min, max, initialAmount, increment);
								uiElements[key] = upDown;
								uiKeys.Add(key);
							} break;
					}
				}
				if (uiElements.Count > 14)
				{
					maxVisible = 14;
					uiScrollBar.SetEnabledState(true);
					uiScrollBar.SetAmountOfItems(uiElements.Count);
				}
				else
				{
					maxVisible = uiElements.Count;
					uiScrollBar.SetEnabledState(false);
					uiScrollBar.SetAmountOfItems(14);
				}
				for (int i = 0; i < maxVisible; i++)
				{
					uiElements[uiKeys[i + uiScrollBar.TopIndex]].MoveTo(xScreenPos + 200, yScreenPos + 80 + i * 28);
					uiLabels[i + uiScrollBar.TopIndex].MoveTo(xScreenPos + 200, yScreenPos + 80 + (i * 28));
					uiLabels[i + uiScrollBar.TopIndex].SetAlignment(true);
				}
				generateGalaxyButton.Active = true;
			}
			catch (Exception e)
			{
				uiElements = new Dictionary<string, UIElement>();
				uiLabels = new List<Label>(new[] { new Label("There was an error parsing the variables returned from this script: " + e.Message, xScreenPos + 5, yScreenPos + 150) });
				generateGalaxyButton.Active = false;
			}
		}

		private void DrawGalaxyPreview(DrawingManagement drawingManagement)
		{
			galaxyBackground.Draw(drawingManagement);

			/*GorgonLibrary.Graphics.Sprite nebula = gameMain.galaxy.Nebula;
			if (nebula != null)
			{
				nebula.SetPosition(xScreenPos + 488, yScreenPos + 23);
				float scale = (295.0f / (gameMain.galaxy.GalaxySize + 3));
				nebula.SetScale(scale, scale);
				gameMain.galaxy.Nebula.Draw();
			}*/

			List<StarSystem> systems = gameMain.galaxy.GetAllStars();

			if (systems.Count > 0)
			{
				galaxyBackground = new StretchableImage(xScreenPos + 475, yScreenPos + 10, 320, 320, 60, 60, DrawingManagement.BorderBorder);
				foreach (StarSystem system in systems)
				{
					int xPos = xScreenPos + 488 + (int)(295.0f * (system.X / (float)(gameMain.galaxy.GalaxySize + 3)));
					int yPos = yScreenPos + 23 + (int)(295.0f * (system.Y / (float)(gameMain.galaxy.GalaxySize + 3)));
					GorgonLibrary.Gorgon.CurrentShader = system.Type.Shader; //if it's null, no worries
					if (system.Type.Shader != null)
					{
						system.Type.Shader.Parameters["StarColor"].SetValue(system.Type.ShaderValue);
					}
					system.Sprite.Draw(xPos, yPos, 0.1f, 0.1f);
					GorgonLibrary.Gorgon.CurrentShader = null;
				}
			}
		}

		private void CancelBattle()
		{
			showingBattle = false;
		}

		private void CommenceBattle(string filename)
		{
			showingBattle = false;
		}
	}
}
