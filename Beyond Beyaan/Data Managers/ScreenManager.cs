using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ScreenManager
	{
		private GameMain _gameMain;
		private Dictionary<string, Screen> _screens;
		private Screen _currentScreen;
		private List<Screen> _windows;

		public bool Initialize(DirectoryInfo dataset, GameMain gameMain, out string reason)
		{
			_gameMain = gameMain;
			DirectoryInfo screenPath = new DirectoryInfo(Path.Combine(dataset.FullName, "Screens"));
			_screens = new Dictionary<string, Screen>();
			_windows = new List<Screen>();

			try
			{
				foreach (var file in screenPath.GetFiles("*.xml"))
				{
					XDocument doc = XDocument.Load(file.FullName);
					XElement root = doc.Element("Screen");
					Screen newScreen = new Screen();
					if (!newScreen.LoadScreen(root, gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain, out reason))
					{
						reason = string.Format(reason, file.Name);
						return false;
					}
					_screens.Add(file.Name.Substring(0, file.Name.IndexOf(file.Extension)), newScreen);
				}
				if (!_screens.ContainsKey(GameConfiguration.FirstScreen))
				{
					reason = "First screen '" + GameConfiguration.FirstScreen + "' does not exist.";
					return false;
				}
				if (!_screens.ContainsKey(GameConfiguration.ErrorDialog))
				{
					reason = "Error Window '" + GameConfiguration.ErrorDialog + "' does not exist";
					return false;
				}
				_currentScreen = _screens[GameConfiguration.FirstScreen];
				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = "Error in loading screens.  Reason: " + e.Message;
				return false;
			}
		}

		public void DrawScreen()
		{
			_currentScreen.Draw();
			foreach (var screen in _windows)
			{
				screen.Draw();
			}
		}

		public void ChangeScreen(string whichScreen)
		{
			if (_screens.ContainsKey(whichScreen))
			{
				_currentScreen = _screens[whichScreen];
				_currentScreen.RefreshData();
			}
			else
			{
				_gameMain.AddError("Screen '" + whichScreen + "' does not exist.");
				ShowScreen(GameConfiguration.ErrorDialog);
			}
		}

		public void ShowScreen(string whichScreen)
		{
			if (_screens.ContainsKey(whichScreen))
			{
				_screens[whichScreen].RefreshData();
				if (_windows.Contains(_screens[whichScreen]))
				{
					return;
				}
				_windows.Add(_screens[whichScreen]);
			}
			else
			{
				_gameMain.AddError("Screen '" + whichScreen + "' does not exist.");
				_windows.Add(_screens[GameConfiguration.ErrorDialog]);
				_screens[GameConfiguration.ErrorDialog].RefreshData();
			}
		}

		public void CloseScreen(Screen whichScreen)
		{
			_windows.Remove(whichScreen);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			for (int i = _windows.Count - 1; i >= 0; i--)
			{
				if (_windows[i].MouseDown(x, y, whichButton))
				{
					return;
				}
			}
			_currentScreen.MouseDown(x, y, whichButton);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			for (int i = _windows.Count - 1; i >= 0; i--)
			{
				if (_windows[i].MouseUp(x, y, whichButton))
				{
					return;
				}
			}
			_currentScreen.MouseUp(x, y, whichButton);
		}

		public void MouseHover(int x, int y, float frameDeltaTime)
		{
			for (int i = _windows.Count - 1; i >= 0; i--)
			{
				if (_windows[i].MouseHover(x, y, frameDeltaTime))
				{
					return;
				}
			}
			_currentScreen.MouseHover(x, y, frameDeltaTime);
		}
	}
}