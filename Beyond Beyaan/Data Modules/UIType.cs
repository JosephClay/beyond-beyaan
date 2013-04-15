using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;
using Font = GorgonLibrary.Graphics.Font;

namespace Beyond_Beyaan.Data_Modules
{
	enum UITypeEnum { IMAGE, LABEL, BUTTON }
	enum BaseUISprites
		{
			BACKGROUND,
			FOREGROUND,
			TOP_LEFT_BACKGROUND,
			TOP_MIDDLE_BACKGROUND, //Also used in VerticalStretchButton's top
			TOP_RIGHT_BACKGROUND,
			LEFT_MIDDLE_BACKGROUND, //Also used in HorizontalStretchButton's left
			MIDDLE_BACKGROUND,
			RIGHT_MIDDLE_BACKGROUND,
			BOTTOM_LEFT_BACKGROUND,
			BOTTOM_MIDDLE_BACKGROUND,
			BOTTOM_RIGHT_BACKGROUND,
			TOP_LEFT_FOREGROUND,
			TOP_MIDDLE_FOREGROUND, //Also used in VerticalStretchButton's top
			TOP_RIGHT_FOREGROUND,
			LEFT_MIDDLE_FOREGROUND, //Also used in HorizontalStretchButton's left
			MIDDLE_FOREGROUND,
			RIGHT_MIDDLE_FOREGROUND,
			BOTTOM_LEFT_FOREGROUND,
			BOTTOM_MIDDLE_FOREGROUND,
			BOTTOM_RIGHT_FOREGROUND,
		}

	public class BaseUIType
	{
		internal Dictionary<BaseUISprites, BaseSprite> Sprites { get; private set; }
		internal UITypeEnum Type { get; private set; }
		public string Name { get; private set; }

		public bool LoadType(XElement element, SpriteManager spriteManager, out string reason)
		{
			Sprites = new Dictionary<BaseUISprites, BaseSprite>();

			try
			{
				foreach (var attribute in element.Attributes())
				{
					switch (attribute.Name.LocalName.ToLower())
					{
						case "name":
							{
								Name = attribute.Value;
							} break;
						case "type":
							{
								switch (attribute.Value.ToLower())
								{
									case "button": //Generic button that only uses two sprites, button background/foreground
										Type = UITypeEnum.BUTTON;
										break;
									case "label": //A simple text label with no sprites associated
										Type = UITypeEnum.LABEL;
										break;
									case "image": //Generic image that just displays something, usually used in backgrounds
										Type = UITypeEnum.IMAGE;
										break;
								}
							} break;
						case "background":
							{
								Sprites.Add(BaseUISprites.BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "foreground":
							{
								Sprites.Add(BaseUISprites.FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
					}
				}
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			reason = null;
			return true;
		}
	}

	public class UIType
	{
		private Dictionary<BaseUISprites, BBSprite> _sprites;
		private TextSprite _textSprite;
		private UITypeEnum _type;
		private Color _color;
		private Color _disabledColor;

		private int _xPos;
		private int _yPos;

		private int _width;
		private int _height;

		private float _pulse1;
		private bool _direction1;
		private bool _pressed1;

		public bool Enabled { get; set; }
		public bool Selected { get; set; }

		#region Events
		public string OnClick { get; set; }
		#endregion

		public UIType(BaseUIType baseUIType, Random r)
		{
			_color = Color.White;
			_sprites = new Dictionary<BaseUISprites, BBSprite>();
			foreach (var sprite in baseUIType.Sprites)
			{
				_sprites.Add(sprite.Key, new BBSprite(sprite.Value, r));
			}
			_type = baseUIType.Type;
			Enabled = true;
		}

		public void SetRect(int x, int y, int width, int height)
		{
			_xPos = x;
			_yPos = y;
			_width = width;
			_height = height;
		}

		public void SetColor(byte a, byte r, byte g, byte b)
		{
			_color = Color.FromArgb(a, r, g, b);
			_disabledColor = Color.FromArgb(a, (byte) (r*0.7), (byte) (g*0.7), (byte) (b*0.7));
		}

		public void SetText(string content, Font font)
		{
			_textSprite = new TextSprite("Label", content, font);
		}

		public bool MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			switch (_type)
			{
				case UITypeEnum.BUTTON:
					{
						return Button_MouseHover(mouseX, mouseY, frameDeltaTime);
					}
				default: return false;
			}
		}

		public void MouseDown(int mouseX, int mouseY)
		{
			switch (_type)
			{
				case UITypeEnum.BUTTON:
					{
						Button_MouseDown(mouseX, mouseY);
					} break;
			}
		}

		public bool MouseUp(int mouseX, int mouseY)
		{
			switch (_type)
			{
				case UITypeEnum.BUTTON:
					{
						return Button_MouseUp(mouseX, mouseY);
					}
				default: return false;
			}
		}

		public void Draw()
		{
			switch (_type)
			{
				case UITypeEnum.BUTTON:
					{
						Button_Draw();
					} break;
				case UITypeEnum.IMAGE:
					{
						Image_Draw();
					} break;
				case UITypeEnum.LABEL:
					{
						Label_Draw();
					} break;
			}
		}

		public void Update(float frameDeltaTime, Random r)
		{
			foreach (var sprite in _sprites)
			{
				sprite.Value.Update(frameDeltaTime, r);
			}
		}
		#region Individual UI functions

		#region Label functions
		private void Label_Draw()
		{
			_textSprite.SetPosition(_xPos, _yPos);
			_textSprite.Color = _color;
			_textSprite.Draw();
		}
		#endregion

		#region Image functions
		private void Image_Draw()
		{
			var sprite = _sprites[BaseUISprites.BACKGROUND];
			if (_width == 0 || _height == 0)
			{
				//Possibly just want the normal scale
				sprite.Draw(_xPos, _yPos, 1, 1, _color);
			}
			else
			{
				sprite.Draw(_xPos, _yPos, _width / sprite.Width, _height / sprite.Height, _color);
			}
		}
		#endregion

		#region Button functions
		private void Button_Draw()
		{
			var backgroundSprite = _sprites[BaseUISprites.BACKGROUND];
			var foregroundSprite = _sprites[BaseUISprites.FOREGROUND];
			if (Enabled)
			{
				if (_pressed1 || Selected)
				{
					foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
				}
				else
				{
					backgroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
					if (_pulse1 > 0)
					{
						foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, Color.FromArgb((byte)(255 * _pulse1), _color));
					}
				}
			}
			else
			{
				backgroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				if (Selected)
				{
					foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				}
			}
			/*if (label.Text.Length > 0)
			{
				label.Draw();
			}*/
		}
		private void Button_MouseDown(int mouseX, int mouseY)
		{
			if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos && mouseY < _yPos + _height)
			{
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}*/
				if (Enabled)
				{
					_pressed1 = true;
				}
			}
		}
		private bool Button_MouseUp(int mouseX, int mouseY)
		{
			if (Enabled && _pressed1)
			{
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}*/
				_pressed1 = false;
				if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos && mouseY < _yPos + _height)
				{
					return true;
				}
			}
			return false;
		}
		private bool Button_MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos && mouseY < _yPos + _height)
			{
				if (_pulse1 < 0.6f)
				{
					_pulse1 = 0.9f;
				}
				if (Enabled)
				{
					if (_direction1)
					{
						_pulse1 += frameDeltaTime / 2;
						if (_pulse1 > 0.9f)
						{
							_direction1 = !_direction1;
							_pulse1 = 0.9f;
						}
					}
					else
					{
						_pulse1 -= frameDeltaTime / 2;
						if (_pulse1 < 0.6f)
						{
							_direction1 = !_direction1;
							_pulse1 = 0.6f;
						}
					}
				}
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
				}*/
				return true;
			}
			if (_pulse1 > 0)
			{
				_pulse1 -= frameDeltaTime * 2;
				if (_pulse1 < 0)
				{
					_pulse1 = 0;
				}
			}
			/*if (toolTipEnabled)
			{
				toolTip.SetShowing(false);
			}*/
			return false;
		}
		#endregion
		#endregion
	}
}
