using System.Collections.Generic;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class InvadeScreen : ScreenInterface
	{
		private GameMain gameMain;

		private TransferToPlanet transferWindow;
		//private List<SettlerToProcess> invadersToProcess;
		//private SettlerToProcess currentInvaderToProcess;

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;

			transferWindow = new TransferToPlanet((gameMain.ScreenWidth / 2), 75, gameMain, DoneFunction);
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			/*gameMain.DrawGalaxyBackground();

			if (currentInvaderToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				transferWindow.DrawWindow(drawingManagement);
			}
			else
			{
				//Process AI here
			}*/
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			gameMain.UpdateGalaxyBackground(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			/*UpdateBackground(frameDeltaTime);

			if (currentInvaderToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				transferWindow.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			else
			{
				invadersToProcess.Remove(currentInvaderToProcess);
				if (invadersToProcess.Count > 0)
				{
					currentInvaderToProcess = invadersToProcess[0];
				}
				else
				{
					//All done, change back to processing screen
					gameMain.ChangeToScreen(Screen.ProcessTurn);
				}
			}*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*if (currentInvaderToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				transferWindow.MouseDown(x, y);
			}*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*if (currentInvaderToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
			{
				transferWindow.MouseUp(x, y);
			}*/
		}

		private void DoneFunction()
		{
			/*invadersToProcess.RemoveAt(0);
			if (invadersToProcess.Count == 0)
			{
				gameMain.ChangeToScreen(Screen.ProcessTurn);
			}
			else
			{
				currentInvaderToProcess = invadersToProcess[0];
				transferWindow.LoadTransfer(currentInvaderToProcess.whichSystem, currentInvaderToProcess.whichFleet);
			}*/
		}

		public void LoadScreen(List<SettlerToProcess> invadersToProcess)
		{
			/*this.invadersToProcess = invadersToProcess;

			if (invadersToProcess.Count > 0)
			{
				currentInvaderToProcess = invadersToProcess[0];
				if (currentInvaderToProcess.whichFleet.Empire.Type == PlayerType.HUMAN)
				{
					transferWindow.LoadTransfer(currentInvaderToProcess.whichSystem, currentInvaderToProcess.whichFleet);
					gameMain.CenterGalaxyScreen(new Point(currentInvaderToProcess.whichSystem.X, currentInvaderToProcess.whichSystem.Y - 5));
				}
				else
				{
					//Let AI process this
				}
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}

		public void Resize()
		{
		}
	}
}
