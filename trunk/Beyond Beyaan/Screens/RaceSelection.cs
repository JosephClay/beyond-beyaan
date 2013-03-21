using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class RaceSelection : WindowInterface
	{
		#region Delegated Functions
		public delegate void OkClick(Race selectedRace);
		public delegate void CancelClick();
		private OkClick OnOkClick;
		private CancelClick OnCancelClick;
		#endregion

		#region Member Variables
		private int selectedRace;

		private List<Race> racesAvailable;
		//private List<string> raceNames;
		int maxVisible;
		#endregion

		#region UI Elements
		private StretchButton okButton;
		private StretchButton cancelButton;

		private StretchableImage raceListBackground;
		private StretchableImage racePortraitBackground;
		private StretchableImage raceDescriptionBackground;
		private CheckBox[] raceRadioButtons;

		private ScrollBar raceListScrollbar;
		private TextBox raceDescription;

		private GorgonLibrary.Graphics.Sprite miniAvatar;
		#endregion

		public RaceSelection(int x, int y, GameMain gameMain, OkClick okClick, CancelClick cancelClick)
			: base(x, y, 600, 500, string.Empty, gameMain, false)
		{
			backGroundImage = new StretchableImage(x, y, 600, 500, 60, 60, DrawingManagement.BorderBorder);

			OnOkClick = okClick;
			OnCancelClick = cancelClick;

			okButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Select Race", x + 380, y + 445, 200, 35);
			cancelButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Cancel", x + 20, y + 445, 200, 35);

			raceListScrollbar = new ScrollBar(x + 247, y + 40, 16, 343, 15, 15, false, false, DrawingManagement.VerticalScrollBar);

			raceListBackground = new StretchableImage(x + 20, y + 30, 250, 400, 30, 13, DrawingManagement.BoxBorder);
			racePortraitBackground = new StretchableImage(x + 270, y + 30, 310, 140, 30, 13, DrawingManagement.BoxBorder);
			raceDescriptionBackground = new StretchableImage(x + 270, y + 170, 310, 260, 30, 13, DrawingManagement.BoxBorder);

			racesAvailable = gameMain.raceManager.Races;
			maxVisible = (racesAvailable.Count + 1) > 15 ? 15 : (racesAvailable.Count + 1);

			if (racesAvailable.Count + 1 <= 15)
			{
				raceListScrollbar.SetEnabledState(false);
				raceListScrollbar.SetAmountOfItems(15);
			}
			else
			{
				raceListScrollbar.SetEnabledState(true);
				raceListScrollbar.SetAmountOfItems(racesAvailable.Count + 1);
			}

			selectedRace = 0;

			raceRadioButtons = new CheckBox[15];
			for (int i = 0; i < raceRadioButtons.Length; i++)
			{
				raceRadioButtons[i] = new CheckBox(DrawingManagement.RadioButton, string.Empty, x + 25, y + 40 + (i * 25), 220, 25, 19, true);
			}

			raceDescription = new TextBox(x + 280, y + 180, 290, 240, "raceSelection", string.Empty, DrawingManagement.GetFont("Computer"), DrawingManagement.VerticalScrollBar);

			RefreshList();
			RefreshLabels();
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);
			raceListBackground.Draw(drawingManagement);
			racePortraitBackground.Draw(drawingManagement);
			raceDescriptionBackground.Draw(drawingManagement);

			for (int i = 0; i < maxVisible; i++)
			{
				raceRadioButtons[i].Draw(drawingManagement);
			}

			okButton.Draw(drawingManagement);
			cancelButton.Draw(drawingManagement);
			raceListScrollbar.Draw(drawingManagement);
			raceDescription.Draw(drawingManagement);

			if (selectedRace > 0)
			{
				miniAvatar.Draw();
			}
			else
			{
				drawingManagement.DrawSprite(SpriteName.CancelBackground, xPos + 361, yPos + 36, 255, 128, 128, System.Drawing.Color.White);
			}
		}

		private void RefreshList()
		{
			for (int i = 0; i < maxVisible; i++)
			{
				raceRadioButtons[i].IsChecked = false;
				if (selectedRace == i + raceListScrollbar.TopIndex)
				{
					raceRadioButtons[i].IsChecked = true;
					if (selectedRace > 0)
					{
						miniAvatar = gameMain.raceManager.Races[selectedRace - 1].GetMiniAvatar();
						miniAvatar.SetPosition(xPos + 361, yPos + 36);
						raceDescription.SetMessage(gameMain.raceManager.Races[selectedRace - 1].RaceDescription);
					}
					else
					{
						raceDescription.SetMessage("A race will be randomly picked when the game starts.");
					}
				}
			}
		}

		private void RefreshLabels()
		{
			for (int i = 0; i < maxVisible; i++)
			{
				if (i + raceListScrollbar.TopIndex == 0)
				{
					raceRadioButtons[0].SetButtonText("Random Race");
					continue;
				}
				raceRadioButtons[i].SetButtonText(racesAvailable[i + raceListScrollbar.TopIndex - 1].RaceName);
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = base.MouseHover(x, y, frameDeltaTime);

			result = okButton.MouseHover(x, y, frameDeltaTime) || result;
			result = cancelButton.MouseHover(x, y, frameDeltaTime) || result;

			for (int i = 0; i < maxVisible; i++)
			{
				result = raceRadioButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}

			if (raceListScrollbar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshList();
				RefreshLabels();
				result = true;
			}

			result = raceDescription.MouseHover(x, y, frameDeltaTime) || result;
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = base.MouseDown(x, y);

			result = raceListScrollbar.MouseDown(x, y) || result;

			for (int i = 0; i < maxVisible; i++)
			{
				result = raceRadioButtons[i].MouseDown(x, y) || result;
			}

			result = okButton.MouseDown(x, y) || result;
			result = cancelButton.MouseDown(x, y) || result;

			result = raceDescription.MouseDown(x, y) || result;

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = base.MouseUp(x, y);

			if (raceListScrollbar.MouseUp(x, y))
			{
				result = true;
				RefreshList();
				RefreshLabels();
			}
			result = raceDescription.MouseUp(x, y) || result;
			for (int i = 0; i < maxVisible; i++)
			{
				if (raceRadioButtons[i].MouseUp(x, y))
				{
					result = true;
					selectedRace = i + raceListScrollbar.TopIndex;
					RefreshList();
					break;
				}
			}
			if (okButton.MouseUp(x, y))
			{
				result = true;
				if (OnOkClick != null)
				{
					if (selectedRace == 0)
					{
						OnOkClick(null);
					}
					else
					{
						OnOkClick(racesAvailable[selectedRace - 1]);
					}
				}
			}
			if (cancelButton.MouseUp(x, y))
			{
				result = true;
				if (OnCancelClick != null)
				{
					OnCancelClick();
				}
			}
			return result;
		}
	}
}
