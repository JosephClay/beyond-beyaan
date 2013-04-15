using System.Collections.Generic;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	class ColorSelection : WindowInterface
	{
		#region Delegated Functions
		public delegate void OkClick(Color selectedColor);
		public delegate void CancelClick();
		private OkClick OnOkClick;
		private CancelClick OnCancelClick;
		#endregion

		#region UI Components
		private GorgonLibrary.Graphics.Sprite colorBlock;
		private ScrollBar redSlider;
		private ScrollBar greenSlider;
		private ScrollBar blueSlider;
		private Label[] colorLabels;
		private List<Color> presetColors;
		private StretchButton cancelButton;
		private StretchButton okButton;
		private StretchableImage box;
		private bool pressed;
		private int colorPressed;
		#endregion

		#region Variables
		private Color selectedColor;
		#endregion

		public ColorSelection(int x, int y, GameMain gameMain, OkClick okClick, CancelClick cancelClick)
			: base(x, y, 400, 500, string.Empty, gameMain, false)
		{
			backGroundImage = new StretchableImage(x, y, 400, 500, 60, 60, DrawingManagement.BorderBorder);
			this.OnCancelClick = cancelClick;
			this.OnOkClick = okClick;

			presetColors = new List<Color>
			{
				Color.Red,
				Color.Blue,
				Color.Green,
				Color.Yellow,
				Color.Teal,
				Color.Purple,
				Color.Orange,
				Color.Brown,
				Color.Gray,
				Color.HotPink,
				Color.Khaki,
				Color.Aqua,
				Color.Beige,
				Color.LightBlue,
				Color.LightCyan,
				Color.LightGray,
			};

			redSlider = new ScrollBar(xPos + 20, yPos + 280, 16, 255, 1, 256, true, true, DrawingManagement.HorizontalSliderBar, gameMain.FontManager.GetDefaultFont());
			greenSlider = new ScrollBar(xPos + 20, yPos + 330, 16, 255, 1, 256, true, true, DrawingManagement.HorizontalSliderBar, gameMain.FontManager.GetDefaultFont());
			blueSlider = new ScrollBar(xPos + 20, yPos + 380, 16, 255, 1, 256, true, true, DrawingManagement.HorizontalSliderBar, gameMain.FontManager.GetDefaultFont());

			colorLabels = new Label[3];
			colorLabels[0] = new Label("Red: ", xPos + 20, yPos + 260, Color.Red, gameMain.FontManager.GetDefaultFont());
			colorLabels[1] = new Label("Green: ", xPos + 20, yPos + 310, Color.Green, gameMain.FontManager.GetDefaultFont());
			colorLabels[2] = new Label("Blue: ", xPos + 20, yPos + 360, Color.Blue, gameMain.FontManager.GetDefaultFont());

			cancelButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Cancel", xPos + 20, yPos + 440, 150, 35, gameMain.FontManager.GetDefaultFont());
			okButton = new StretchButton(DrawingManagement.ButtonBackground, DrawingManagement.ButtonForeground, "Confirm", xPos + 220, yPos + 440, 150, 35, gameMain.FontManager.GetDefaultFont());

			GorgonLibrary.Graphics.Image block = new GorgonLibrary.Graphics.Image("colorBlock", 40, 40, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			block.Clear(Color.White);
			colorBlock = new GorgonLibrary.Graphics.Sprite("color", block);

			box = new StretchableImage(0, 0, 60, 60, 30, 13, DrawingManagement.BoxBorder);
		}

		public void SetColor(Color color)
		{
			//Make a copy
			selectedColor = Color.FromArgb(color.ToArgb());
			redSlider.TopIndex = selectedColor.R;
			greenSlider.TopIndex = selectedColor.G;
			blueSlider.TopIndex = selectedColor.B;

			colorLabels[0].SetText("Red: " + redSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
			colorLabels[1].SetText("Green: " + greenSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
			colorLabels[2].SetText("Blue: " + blueSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
		}

		public Color GetPresetColor(int index)
		{
			if (index < 0 || index > 15)
			{
				return Color.Black;
			}
			return presetColors[index];
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);
			if (colorPressed >= 0)
			{
				box.MoveTo(xPos + ((colorPressed % 4)) * 60 + 70, ((colorPressed / 4)) * 60 + 20 + yPos);
				box.Draw(drawingManagement);
			}

			box.MoveTo(xPos + 320, 305 + yPos);
			box.Draw(drawingManagement);

			for (int i = 0; i < presetColors.Count; i++)
			{
				colorBlock.SetPosition(((i % 4) * 60) + 80 + xPos, ((i / 4) * 60) + 30 + yPos);
				colorBlock.Color = presetColors[i];
				colorBlock.Draw();
			}

			
			colorBlock.SetPosition(xPos + 330, yPos + 315);
			colorBlock.Color = selectedColor;
			colorBlock.Draw();

			redSlider.Draw(drawingManagement);
			greenSlider.Draw(drawingManagement);
			blueSlider.Draw(drawingManagement);

			foreach (Label label in colorLabels)
			{
				label.Draw();
			}

			cancelButton.Draw(drawingManagement);
			okButton.Draw(drawingManagement);
		}
		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (greenSlider.MouseHover(x, y, frameDeltaTime) || redSlider.MouseHover(x, y, frameDeltaTime) || blueSlider.MouseHover(x, y, frameDeltaTime))
			{
				UpdateColor();
				result = true;
			}
			result |= cancelButton.MouseHover(x, y, frameDeltaTime);
			result |= okButton.MouseHover(x, y, frameDeltaTime);
			
			if (!pressed)
			{
				colorPressed = -1;
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						int xOffset = (i * 60) + 80 + xPos;
						int yOffset = (j * 60) + 30 + yPos;
						if (x >= xOffset && x < xOffset + 40 && y >= yOffset && y < yOffset + 40)
						{
							colorPressed = i + (j * 4);
						}
					}
				}
			}
			result |= base.MouseHover(x, y, frameDeltaTime);
			return result;
		}
		public override bool MouseDown(int x, int y)
		{
			bool result = redSlider.MouseDown(x, y);
			result |= greenSlider.MouseDown(x, y);
			result |= blueSlider.MouseDown(x, y);
			result |= cancelButton.MouseDown(x, y);
			result |= okButton.MouseDown(x, y);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int xOffset = (i * 60) + 80 + xPos;
					int yOffset = (j * 60) + 30 + yPos;
					if (x >= xOffset && x < xOffset + 40 && y >= yOffset && y < yOffset + 40)
					{
						pressed = true;
						break;
					}
				}
			}
			result |= base.MouseDown(x, y);
			return result;
		}
		public override bool MouseUp(int x, int y)
		{
			bool result = false;
			if (greenSlider.MouseUp(x, y) || redSlider.MouseUp(x, y) || blueSlider.MouseUp(x, y))
			{
				UpdateColor();
				result = true;
			}
			if (cancelButton.MouseUp(x, y) && OnCancelClick != null)
			{
				result = true;
				OnCancelClick();
			}
			if (okButton.MouseUp(x, y) && OnOkClick != null)
			{
				result = true;
				OnOkClick(selectedColor);
			}
			if (pressed && (colorPressed >= 0 && colorPressed < 16))
			{
				int xOffset = (colorPressed % 4 * 60) + 80 + xPos;
				int yOffset = (colorPressed / 4 * 60) + 30 + yPos;
				if (x >= xOffset && x < xOffset + 40 && y >= yOffset && y < yOffset + 40)
				{
					selectedColor = presetColors[colorPressed];
					redSlider.TopIndex = selectedColor.R;
					greenSlider.TopIndex = selectedColor.G;
					blueSlider.TopIndex = selectedColor.B;
					UpdateColor();
					result = true;
				}
				pressed = false;
			}
			result |= base.MouseUp(x, y);
			return result;
		}
		private void UpdateColor()
		{
			selectedColor = Color.FromArgb(redSlider.TopIndex, greenSlider.TopIndex, blueSlider.TopIndex);

			colorLabels[0].SetText("Red: " + redSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
			colorLabels[1].SetText("Green: " + greenSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
			colorLabels[2].SetText("Blue: " + blueSlider.TopIndex, gameMain.FontManager.GetDefaultFont());
		}
	}
}
