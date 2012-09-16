using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;

namespace Beyond_Beyaan.Data_Modules
{
	public class Icon
	{
		private Sprite iconSprite;
		private Label text;
		private bool hasSlider;
		private bool hasNumericUpDown;

		private NumericUpDown numericUpDown;
		private ScrollBar slider;

		public int Value
		{
			get
			{
				if (hasNumericUpDown)
				{
					return numericUpDown.Value;
				}
				else if (hasSlider)
				{
					return slider.TopIndex;
				}
				return -1;
			}
		}
		public int Size { get; private set; }
		public string Format;
		public string[] ValueNames;

		public Icon(string iconName, XElement element, Sprite sprite)
		{
			Format = element.Attribute("format").Value;
			string preParseValues = element.Attribute("valueNames").Value;
			if (!string.IsNullOrEmpty(preParseValues))
			{
				ValueNames = preParseValues.Split(new[] { '|' });
			}
			else
			{
				ValueNames = new string[0];
			}
			int x = int.Parse(element.Attribute("xPos").Value);
			int y = int.Parse(element.Attribute("yPos").Value);
			Size = int.Parse(element.Attribute("size").Value);
			iconSprite = new Sprite(iconName + "_Icon", sprite.Image, x, y, Size, Size);

			text = new Label(0, 0);

			if (element.Attribute("control") != null)
			{
				string[] values = element.Attribute("control").Value.Split(new[] { ',' });
				if (values.Length != 3) //Should have type of control, max value, initial value
				{
					throw new Exception(iconName + " icon has invalid control values.  Values should be (upDown or slider),(max Value),(initial value) without ()'s");
				}
				if (values[0].ToLower() == "updown")
				{
					numericUpDown = new NumericUpDown(0, 0, 150, 0, int.Parse(values[1]), int.Parse(values[2]));
					hasNumericUpDown = true;
				}
				else if (values[0].ToLower() == "slider")
				{
					slider = new ScrollBar(0, 0, 16, 118, 1, int.Parse(values[1]), true, true, DrawingManagement.HorizontalSliderBar);
					hasSlider = true;
				}
				else
				{
					throw new Exception(iconName + " icon specified an illegal control value (" + values[0] + "), valid values are upDown or slider.");
				}
			}
		}

		public Icon(Image iconImage, int imageX, int imageY, string iconName, int size, string format, string[] valueNames)
		{
			iconSprite = new Sprite(iconName + "_Icon", iconImage, imageX, imageY, size, size);
			Size = (int) iconSprite.Width;
			Format = format;
			ValueNames = valueNames;
			
			//text.SetAlignment(true);
		}

		public Icon(Image iconImage, int imageX, int imageY, string iconName, int size, string format, string[] valueNames, bool hasSlider, bool hasNumericUpDown, List<SpriteName> sprites)
		{
			iconSprite = new Sprite(iconName + "_Icon", iconImage, imageX, imageY, size, size);
			Size = (int)iconSprite.Width;
			Format = format;
			ValueNames = valueNames;
			text = new Label(0, 0);
			//text.SetAlignment(true);
			this.hasSlider = hasSlider;
			this.hasNumericUpDown = hasNumericUpDown;

			if (hasSlider)
			{
				slider = new ScrollBar(0, 0, 16, 75, 1, 100, true, true, sprites);
			}
			else if (hasNumericUpDown)
			{
				numericUpDown = new NumericUpDown(0, 0, 75, 0, 100, 0);
			}
		}

		public Icon(Icon iconToCopy)
		{
			iconSprite = iconToCopy.iconSprite;
			Size = iconToCopy.Size;
			Format = iconToCopy.Format;
			ValueNames = iconToCopy.ValueNames;
			text = new Label(0, 0);
		}

		public void UpdateText(Dictionary<string, object> values)
		{
			if (!hasNumericUpDown && !hasSlider)
			{
				string[] newValues = new string[ValueNames.Length];
				for (int i = 0; i < ValueNames.Length; i++)
				{
					newValues[i] = (string)values[ValueNames[i]];
				}
				text.SetText(string.Format(Format, newValues));
			}
		}

		public void Draw(int x, int y)
		{
			iconSprite.SetPosition(x, y);
			iconSprite.Draw();
		}

		public void Draw(int x, int y, int width, int height, DrawingManagement drawingManagement)
		{
			iconSprite.SetPosition(x, y + (height / 2) - (Size / 2));
			iconSprite.Draw();
			if (!hasNumericUpDown && !hasSlider)
			{
				text.MoveTo((int)(x + iconSprite.Width + 2), y + (height / 2) - (int)(text.GetHeight() / 2));
				text.Draw();
			}
			else
			{
				if (hasNumericUpDown)
				{
					numericUpDown.MoveTo((int)(x + iconSprite.Width + 2), y + 2);
					numericUpDown.Draw(drawingManagement);
				}
			}
		}

		public bool Update(int mouseX, int mouseY, int x, int y, float frameDeltaTime)
		{
			if (hasNumericUpDown)
			{
				numericUpDown.MoveTo((int)(x + iconSprite.Width + 2), y + 2);
				return numericUpDown.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			return false;
		}

		public bool MouseDown(int mouseX, int mouseY, int x, int y)
		{
			if (hasNumericUpDown)
			{
				numericUpDown.MoveTo((int)(x + iconSprite.Width + 2), y + 2);
				return numericUpDown.MouseDown(mouseX, mouseY);
			}
			return false;
		}

		public bool MouseUp(int mouseX, int mouseY, int x, int y)
		{
			if (hasNumericUpDown)
			{
				numericUpDown.MoveTo((int)(x + iconSprite.Width + 2), y + 2);
				return numericUpDown.MouseUp(mouseX, mouseY);
			}
			return false;
		}
	}
}
