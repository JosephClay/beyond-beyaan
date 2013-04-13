using System;
using System.Collections.Generic;
using System.IO;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ScreenManager
	{
		private Dictionary<string, Screen> _screens;
		private Screen _currentScreen;

		public bool Initialize(DirectoryInfo dataset, GameMain gameMain, string firstScreen, Random r, out string reason)
		{
			DirectoryInfo screenPath = new DirectoryInfo(Path.Combine(dataset.FullName, "Screens"));
			_screens = new Dictionary<string, Screen>();

			try
			{
				foreach (var file in screenPath.GetFiles("*.xml"))
				{
					Screen newScreen = new Screen();
					if (!newScreen.LoadScreen(file.FullName, gameMain, r, out reason))
					{
						return false;
					}
					_screens.Add(file.Name.Substring(0, file.Name.IndexOf(file.Extension)), newScreen);
				}
				if (!_screens.ContainsKey(firstScreen))
				{
					reason = "First screen " + firstScreen + " does not exist.";
					return false;
				}
				_currentScreen = _screens[firstScreen];
				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = "Error in loading screens.  Reason: " + e.Message;
				return false;
			}
		}

		public void DrawCurrentScreen()
		{
			_currentScreen.Draw();
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			_currentScreen.MouseDown(x, y, whichButton);
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			_currentScreen.MouseUp(x, y, whichButton);
		}

		public void MouseHover(int x, int y, float frameDeltaTime)
		{
			_currentScreen.MouseHover(x, y, frameDeltaTime);
		}
	}
}