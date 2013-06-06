using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
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
		private Mouse mouse;

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
				Gorgon.Initialize(true, false);
				Gorgon.SetMode(this, 1024, 768, BackBufferFormats.BufferRGB888, true);
				
				Gorgon.Idle += new FrameEventHandler(Gorgon_Idle);
				Gorgon.DeviceReset += new EventHandler(Gorgon_DeviceReset);
				Gorgon.FastResize = false;

				//Gorgon.FrameStatsVisible = true;

				input = Input.LoadInputPlugIn(Environment.CurrentDirectory + @"\GorgonInput.DLL", "Gorgon.RawInput");
				input.Bind(this);

				keyboard = input.Keyboard;
				keyboard.Enabled = true;
				keyboard.Exclusive = false;
				keyboard.KeyDown += new KeyboardInputEvent(keyboard_KeyDown);

				mouse = input.Mouse;
				mouse.Enabled = true;
				mouse.Exclusive = false;

				gameMain = new GameMain();

				string reason;
				if (!gameMain.Initalize(Gorgon.Screen.Width, Gorgon.Screen.Height, this, out reason))
				{
					MessageBox.Show("Error loading game resources, error message: \r\n" + reason);
					return;
				}

				Gorgon.Go();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
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

			if (mouse.Wheel != 0)
			{
				gameMain.MouseScroll(mouse.Wheel, (int)mouse.Position.X, (int)mouse.Position.Y);
				mouse.Wheel = 0;
			}

			gameMain.ProcessGame(mouse, e.FrameDeltaTime);

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
	}
}
