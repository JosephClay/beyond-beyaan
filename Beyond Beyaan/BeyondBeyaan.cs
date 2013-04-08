using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Beyond_Beyaan.Properties;
using Drawing = System.Drawing;
using GorgonLibrary;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	public partial class BeyondBeyaan : Form
	{
		private Input input;
		private Keyboard keyboard;

		private GameMain gameMain;

		public BeyondBeyaan()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			List<DirectoryInfo> dataSets = new List<DirectoryInfo>();
			try
			{
				//Check to see if there's data in program directory that's not copied to general application data folder
				DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, "Data"));
				DirectoryInfo target = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Beyond Beyaan"));
				if (!target.Exists)
				{
					target.Create();
				}
				foreach (var directory in di.GetDirectories())
				{
					if (File.Exists(Path.Combine(directory.FullName, "config.xml")) &&
					    !Directory.Exists(Path.Combine(target.FullName, directory.Name)))
					{
						CopyDirectory(directory, new DirectoryInfo(Path.Combine(target.FullName, directory.Name)));
					}
				}
				//Get list of available datasets from general application data folder
				foreach (var directory in target.GetDirectories())
				{
					//Sanity check to ensure that it's a valid dataset
					if (File.Exists(Path.Combine(directory.FullName, "config.xml")))
					{
						dataSets.Add(directory);
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("Failed to copy directories.  Error: " + exception.Message);
				Close();
				return;
			}
			if (dataSets.Count == 0)
			{
				MessageBox.Show("There are no available datasets to choose from.  Ensure that the program is installed correctly.");
				Close();
				return;
			}

			dataSets.Sort((a, b) => a.Name.CompareTo(b.Name));

			try
			{
				Gorgon.Initialize(true, false);

				VideoMode videoMode;
				DirectoryInfo dataset;
				bool fullScreen;
				bool showTutorial;

				using (Configuration configuration = new Configuration())
				{
					configuration.FillResolutionList();
					configuration.FillDatasetList(dataSets);
					configuration.ShowDialog(this);
					if (configuration.DialogResult != DialogResult.OK)
					{
						Close();
						return;
					}
					videoMode = configuration.VideoMode;
					fullScreen = configuration.FullScreen;
					dataset = dataSets[configuration.DataSetIndex];
					showTutorial = configuration.ShowTutorial;
				}

				Gorgon.SetMode(this, videoMode.Width, videoMode.Height, BackBufferFormats.BufferRGB888, !fullScreen);
				
				Gorgon.Idle += Gorgon_Idle;
				Gorgon.DeviceReset += Gorgon_DeviceReset;
				Gorgon.FastResize = false;

				//Gorgon.FrameStatsVisible = true;

				input = Input.LoadInputPlugIn(Environment.CurrentDirectory + @"\GorgonInput.DLL", "Gorgon.RawInput");
				input.Bind(this);

				keyboard = input.Keyboard;
				keyboard.Enabled = true;
				keyboard.Exclusive = false;
				keyboard.KeyDown += keyboard_KeyDown;

				gameMain = new GameMain();

				gameMain.Input = input;

				string reason;
				if (!gameMain.Initalize(Gorgon.Screen.Width, Gorgon.Screen.Height, dataset, showTutorial, this, out reason))
				{
					MessageBox.Show(string.Format(Resources.ERROR_LOADING_GAME_RESOURCES__ERROR_MESSAGE_0, reason));
					Close();
					return;
				}

				Gorgon.Go();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
				Close();
			}
		}

		private void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
		{
			if (!target.Exists)
			{
				target.Create();
			}
			foreach (var file in source.GetFiles())
			{
				file.CopyTo(Path.Combine(target.FullName, file.Name));
			}
			foreach (var directory in source.GetDirectories())
			{
				CopyDirectory(directory, new DirectoryInfo(Path.Combine(target.FullName, directory.Name)));
			}
		}

		void keyboard_KeyDown(object sender, KeyboardInputEventArgs e)
		{
			if (e.Alt && e.Key == KeyboardKeys.Enter)
			{
				Gorgon.Screen.Windowed = !Gorgon.Screen.Windowed;
			}
			gameMain.KeyDown(e);
		}

		void Gorgon_DeviceReset(object sender, EventArgs e)
		{
			gameMain.Resize(Gorgon.Screen.Width, Gorgon.Screen.Height);
		}

		void Gorgon_Idle(object sender, FrameEventArgs e)
		{
			Gorgon.Screen.Clear(Drawing.Color.Black);
			Gorgon.Screen.BeginDrawing();

			gameMain.ProcessGame(e.FrameDeltaTime);

			Gorgon.Screen.EndDrawing();
		}

		private void BeyondBeyaan_MouseDown(object sender, MouseEventArgs e)
		{	
			gameMain.MouseDown(e);
		}

		private void BeyondBeyaan_MouseUp(object sender, MouseEventArgs e)
		{
			gameMain.MouseUp(e);
		}

		private void BeyondBeyaan_MouseMove(object sender, MouseEventArgs e)
		{
			gameMain.MousePos.X = e.X;
			gameMain.MousePos.Y = e.Y;
		}

		void BeyondBeyaan_MouseWheel(object sender, MouseEventArgs e)
		{
			gameMain.MouseScroll(e.Delta);
		}
	}
}
