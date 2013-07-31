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

			try
			{
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
						//To-do - Devise a mechanism where it detects modified files and update to the newer files
						if (Directory.Exists(Path.Combine(target.FullName, directory.Name)))
						{
							Directory.Delete(Path.Combine(target.FullName, directory.Name), true);
						}
						CopyDirectory(directory, new DirectoryInfo(Path.Combine(target.FullName, directory.Name)));
					}
					//Get list of available datasets from general application data folder
					foreach (var directory in target.GetDirectories())
					{
						//Sanity check to ensure that it's a valid dataset
						dataSets.Add(directory);
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(string.Format("Failed to copy directories.  Error: {0}", exception.Message));
					Close();
					return;
				}
				if (dataSets.Count == 0)
				{
					MessageBox.Show(Resources.BeyondBeyaan_OnLoad_There_are_no_available_datasets_to_choose_from___Ensure_that_the_program_is_installed_correctly_);
					Close();
					return;
				}

				dataSets.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase));

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
				
				Gorgon.Idle += new FrameEventHandler(Gorgon_Idle);
				Gorgon.FastResize = false;

				//Gorgon.FrameStatsVisible = true;

				input = Input.LoadInputPlugIn(Environment.CurrentDirectory + @"\GorgonInput.DLL", "Gorgon.RawInput");
				input.Bind(this);

				keyboard = input.Keyboard;
				keyboard.Enabled = true;
				keyboard.Exclusive = false;
				keyboard.KeyDown += keyboard_KeyDown;

				string reason;
				FileInfo fileInfo = new FileInfo(Path.Combine(dataset.FullName, "configuration.xml"));
				if (!GameConfiguration.Initialize(fileInfo, out reason))
				{
					MessageBox.Show(string.Format("Error loading configuration, reason: {0}", reason));
					Close();
					return;
				}

				gameMain = new GameMain();

				if (!gameMain.Initalize(Gorgon.Screen.Width, Gorgon.Screen.Height, dataset, showTutorial, this, out reason))
				{
					MessageBox.Show(string.Format("Error loading game resources, error message: {0}", reason));
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

		private static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
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

		private void BeyondBeyaan_MouseLeave(object sender, EventArgs e)
		{
			Cursor.Show();
		}

		private void BeyondBeyaan_MouseEnter(object sender, EventArgs e)
		{
			Cursor.Hide();
		}
	}
}
