using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;
using Font = GorgonLibrary.Graphics.Font;

namespace Beyond_Beyaan.Data_Modules
{
	enum UITypeEnum { IMAGE, STRETCHABLE_IMAGE, LABEL, BUTTON, STRETCHABLE_BUTTON, DROPDOWN }
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
			UP_BACKGROUND,
			UP_FOREGROUND,
			DOWN_BACKGROUND,
			DOWN_FOREGROUND,
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
									case "stretchablebutton":
										Type = UITypeEnum.STRETCHABLE_BUTTON;
										break;
									case "label": //A simple text label with no sprites associated
										Type = UITypeEnum.LABEL;
										break;
									case "image": //Generic image that just displays something, usually used in backgrounds
										Type = UITypeEnum.IMAGE;
										break;
									case "stretchableimage":
										Type = UITypeEnum.STRETCHABLE_IMAGE;
										break;
									case "dropdown":
										Type = UITypeEnum.DROPDOWN;
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
						case "top_left_background":
							{
								Sprites.Add(BaseUISprites.TOP_LEFT_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "top_middle_background":
							{
								Sprites.Add(BaseUISprites.TOP_MIDDLE_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "top_right_background":
							{
								Sprites.Add(BaseUISprites.TOP_RIGHT_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "left_middle_background":
							{
								Sprites.Add(BaseUISprites.LEFT_MIDDLE_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "middle_background":
							{
								Sprites.Add(BaseUISprites.MIDDLE_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "right_middle_background":
							{
								Sprites.Add(BaseUISprites.RIGHT_MIDDLE_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_left_background":
							{
								Sprites.Add(BaseUISprites.BOTTOM_LEFT_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_middle_background":
							{
								Sprites.Add(BaseUISprites.BOTTOM_MIDDLE_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_right_background":
							{
								Sprites.Add(BaseUISprites.BOTTOM_RIGHT_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "top_left_foreground":
							{
								Sprites.Add(BaseUISprites.TOP_LEFT_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "top_middle_foreground":
							{
								Sprites.Add(BaseUISprites.TOP_MIDDLE_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "top_right_foreground":
							{
								Sprites.Add(BaseUISprites.TOP_RIGHT_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "left_middle_foreground":
							{
								Sprites.Add(BaseUISprites.LEFT_MIDDLE_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "middle_foreground":
							{
								Sprites.Add(BaseUISprites.MIDDLE_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "right_middle_foreground":
							{
								Sprites.Add(BaseUISprites.RIGHT_MIDDLE_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_left_foreground":
							{
								Sprites.Add(BaseUISprites.BOTTOM_LEFT_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_middle_foreground":
							{
								Sprites.Add(BaseUISprites.BOTTOM_MIDDLE_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "bottom_right_foreground":
							{
								Sprites.Add(BaseUISprites.BOTTOM_RIGHT_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "up_background":
							{
								Sprites.Add(BaseUISprites.UP_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "up_foreground":
							{
								Sprites.Add(BaseUISprites.UP_FOREGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "down_background":
							{
								Sprites.Add(BaseUISprites.DOWN_BACKGROUND, spriteManager.Sprites[attribute.Value]);
							} break;
						case "down_foreground":
							{
								Sprites.Add(BaseUISprites.DOWN_FOREGROUND, spriteManager.Sprites[attribute.Value]);
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
		#region Constants
		private const int BUTTON = 0;
		private const int UP_OR_LEFT = 1;
		private const int DOWN_OR_RIGHT = 2;
		private const int SCROLL_BUTTON = 3;
		private const int FIRST_DROPDOWN_BUTTON = 4;
		#endregion

		private Dictionary<BaseUISprites, BBSprite> _sprites;
		private List<TextSprite> _textSprites; 
		private UITypeEnum _type;
		private Color _color;
		private Color _disabledColor;
		private Color _textColor;

		private int _xPos;
		private int _yPos;

		private int _width;
		private int _height;

		#region Drop-down properties
		private bool _dropped;
		private int _maxVisible;
		private int _arrowXOffset;
		private int _arrowYOffset;
		#endregion

		private List<float> _pulses;
		private List<bool> _directions;
		private List<bool> _presseds;
		private List<bool> _selecteds; 

		public bool Enabled { get; set; }
		public string DataSource { get; set; }

		#region Events
		public string OnClick { get; set; }
		#endregion

		public UIType(BaseUIType baseUIType, Random r)
		{
			_color = Color.White;
			_textColor = Color.White;
			_sprites = new Dictionary<BaseUISprites, BBSprite>();
			_arrowXOffset = 0;
			_arrowYOffset = 0;
			foreach (var sprite in baseUIType.Sprites)
			{
				_sprites.Add(sprite.Key, new BBSprite(sprite.Value, r));
			}
			_type = baseUIType.Type;
			Enabled = true;

			_pulses = new List<float>();
			_directions = new List<bool>();
			_presseds = new List<bool>();
			_selecteds = new List<bool>();
			_textSprites = new List<TextSprite>();

			switch (_type)
			{
				//TODO: Add scrollbars and sliders
				case UITypeEnum.DROPDOWN:
				case UITypeEnum.BUTTON:
				case UITypeEnum.STRETCHABLE_BUTTON:
					{
						//Main button
						_pulses.Add(0);
						_directions.Add(false);
						_presseds.Add(false);
						_selecteds.Add(false);
						if (_type == UITypeEnum.BUTTON || _type == UITypeEnum.STRETCHABLE_BUTTON)
						{
							break;
						}
						//Up/left, down/right, and scroll buttons
						for (int i = 0; i < 3; i++)
						{
							_pulses.Add(0);
							_directions.Add(false);
							_presseds.Add(false);
							_selecteds.Add(false);
						}
					} break;
			}
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

		public void SetTextColor(byte a, byte r, byte g, byte b)
		{
			_textColor = Color.FromArgb(a, r, g, b);
		}

		public void SetArrowOffset(int x, int y)
		{
			_arrowXOffset = x;
			_arrowYOffset = y;
		}

		public void SetText(string content, Font font)
		{
			if (_textSprites.Count == 0)
			{
				_textSprites.Add(new TextSprite("Label", content, font));
			}
			else
			{
				_textSprites[0] = new TextSprite("Label", content, font);
			}
		}

		public bool MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			switch (_type)
			{
				case UITypeEnum.DROPDOWN:
					{
						return DropDown_MouseHover(mouseX, mouseY, frameDeltaTime);
					}
				case UITypeEnum.STRETCHABLE_BUTTON:
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
				case UITypeEnum.DROPDOWN:
					{
						DropDown_MouseDown(mouseX, mouseY);
					} break;
				case UITypeEnum.STRETCHABLE_BUTTON:
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
				case UITypeEnum.DROPDOWN:
					{
						return DropDown_MouseUp(mouseX, mouseY);
					}
				case UITypeEnum.STRETCHABLE_BUTTON:
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
				case UITypeEnum.STRETCHABLE_BUTTON:
					{
						StretchableButton_Draw();
					} break;
				case UITypeEnum.BUTTON:
					{
						Button_Draw();
					} break;
				case UITypeEnum.STRETCHABLE_IMAGE:
					{
						StretchableImage_Draw();
					} break;
				case UITypeEnum.IMAGE:
					{
						Image_Draw();
					} break;
				case UITypeEnum.DROPDOWN:
					{
						DropDown_Draw();
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
		#region Handy Dandy Functions
		private void DrawStretchableWith9(List<BBSprite> sprites, Color color)
		{
			sprites[0].Draw(_xPos, _yPos, 1, 1, color);
			sprites[1].Draw(_xPos + sprites[0].Width, _yPos, (_width - (sprites[0].Width + sprites[2].Width)) / sprites[1].Width, 1, color);
			sprites[2].Draw(_xPos + (_width - sprites[2].Width), _yPos, 1, 1, color);
			sprites[3].Draw(_xPos, _yPos + sprites[0].Height, 1, (_height - (sprites[0].Height + sprites[6].Height)) / sprites[3].Height, color);
			sprites[4].Draw(_xPos + sprites[3].Width, _yPos + sprites[1].Height, (_width - (sprites[3].Width + sprites[5].Width)) / sprites[4].Width, (_height - (sprites[1].Height + sprites[7].Height)) / sprites[4].Height, color);
			sprites[5].Draw(_xPos + (_width - sprites[5].Width), _yPos + sprites[2].Height, 1, (_height - (sprites[2].Height + sprites[8].Height)) / sprites[5].Height, color);
			sprites[6].Draw(_xPos, _yPos + (_height - sprites[6].Height), 1, 1, color);
			sprites[7].Draw(_xPos + (sprites[6].Width), _yPos + (_height - sprites[7].Height), (_width - (sprites[6].Width + sprites[8].Width)) / sprites[7].Width, 1, color);
			sprites[8].Draw(_xPos + (_width - sprites[8].Width), _yPos + (_height - sprites[8].Height), 1, 1, color);
		}
		public void FillData<T>(List<T> values, GameMain gameMain) where T : class
		{
			foreach (T t in values)
			{
				string value = t.GetType().GetProperty("GalaxyScriptName", typeof (string)).GetValue(t, null).ToString();
			}
		}
		#endregion

		#region Individual UI functions

		#region Label functions
		private void Label_Draw()
		{
			_textSprites[0].SetPosition(_xPos, _yPos);
			_textSprites[0].Color = _textColor;
			_textSprites[0].Draw();
		}
		#endregion

		#region DropDown Functions
		private void DropDown_Draw()
		{
			var bgSprites = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_BACKGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_BACKGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_LEFT_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_BACKGROUND]});

			var fgSprites = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_FOREGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_FOREGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_LEFT_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_FOREGROUND]});
			var bgArrow = _sprites[BaseUISprites.DOWN_BACKGROUND];
			var fgArrow = _sprites[BaseUISprites.DOWN_FOREGROUND];
			//If it's not dropped down, we can just imitate the StretchButton
			if (!_dropped)
			{
				if (Enabled)
				{
					if (_presseds[BUTTON] || _selecteds[BUTTON])
					{
						DrawStretchableWith9(fgSprites, _color);
						fgArrow.Draw(_xPos + _width - (fgArrow.Width + _arrowXOffset), _yPos + _arrowYOffset, 1, 1, _color);
					}
					else
					{
						DrawStretchableWith9(bgSprites, _color);
						bgArrow.Draw(_xPos + _width - (bgArrow.Width + _arrowXOffset), _yPos + _arrowYOffset, 1, 1, _color);
						if (_pulses[BUTTON] > 0)
						{
							DrawStretchableWith9(fgSprites, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
							fgArrow.Draw(_xPos + _width - (fgArrow.Width + _arrowXOffset), _yPos + _arrowYOffset, 1, 1, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
						}
					}
				}
				else
				{
					if (_selecteds[BUTTON])
					{
						DrawStretchableWith9(fgSprites, _disabledColor);
						fgArrow.Draw(_xPos + _width - (fgArrow.Width + _arrowXOffset), _yPos + _arrowYOffset, 1, 1, _disabledColor);
					}
					else
					{
						DrawStretchableWith9(bgSprites, _disabledColor);
						bgArrow.Draw(_xPos + _width - (bgArrow.Width + _arrowXOffset), _yPos + _arrowYOffset, 1, 1, _disabledColor);
					}
				}
				if (_textSprites.Count > 0)
				{
					_textSprites[0].SetPosition(_xPos + _width / 2 - _textSprites[0].Width / 2, _yPos + _height / 2 - _textSprites[0].Height / 2);
					_textSprites[0].Color = _textColor;
					_textSprites[0].Draw();
				}
			}
		}

		private void DropDown_MouseDown(int mouseX, int mouseY)
		{
			if (!_dropped)
			{
				Button_MouseDown(mouseX, mouseY);
			}
		}

		private bool DropDown_MouseUp(int mouseX, int mouseY)
		{
			if (!_dropped)
			{
				if (Button_MouseUp(mouseX, mouseY))
				{
					//_dropped = true;
					return true;
				}
			}
			return false;
		}

		private bool DropDown_MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			if (!_dropped)
			{
				return Button_MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			return false;
		}
		#endregion

		#region Stretchable Image functions
		private void StretchableImage_Draw()
		{
			var sprites = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_BACKGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_BACKGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_LEFT_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_BACKGROUND]});

			DrawStretchableWith9(sprites, _color);
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

		#region Stretchable Button Functions
		private void StretchableButton_Draw()
		{
			var bgSprites = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_BACKGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_BACKGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_LEFT_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_BACKGROUND]});

			var fgSprites = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_FOREGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_FOREGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_LEFT_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_FOREGROUND]});

			if (Enabled)
			{
				if (_presseds[BUTTON] || _selecteds[BUTTON])
				{
					DrawStretchableWith9(fgSprites, _color);
				}
				else
				{
					DrawStretchableWith9(bgSprites, _color);
					if (_pulses[BUTTON] > 0)
					{
						DrawStretchableWith9(fgSprites, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
					}
				}
			}
			else
			{
				if (_selecteds[BUTTON])
				{
					DrawStretchableWith9(fgSprites, _disabledColor);
				}
				else
				{
					DrawStretchableWith9(bgSprites, _disabledColor);
				}
			}
			if (_textSprites.Count > 0)
			{
				_textSprites[0].SetPosition(_xPos + _width / 2 - _textSprites[0].Width / 2, _yPos + _height / 2 - _textSprites[0].Height / 2);
				_textSprites[0].Color = _textColor;
				_textSprites[0].Draw();
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
				if (_presseds[BUTTON] || _selecteds[BUTTON])
				{
					foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
				}
				else
				{
					backgroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
					if (_pulses[BUTTON] > 0)
					{
						foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
					}
				}
			}
			else
			{
				if (_selecteds[BUTTON])
				{
					foregroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				}
				else
				{
					backgroundSprite.Draw(_xPos, _yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				}
			}
			if (_textSprites.Count > 0)
			{
				_textSprites[0].SetPosition(_xPos + _width / 2 - _textSprites[0].Width / 2, _yPos + _height / 2 - _textSprites[0].Height / 2);
				_textSprites[0].Color = _textColor;
				_textSprites[0].Draw();
			}
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
					_presseds[BUTTON] = true;
				}
			}
		}
		private bool Button_MouseUp(int mouseX, int mouseY)
		{
			if (Enabled && _presseds[BUTTON])
			{
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}*/
				_presseds[BUTTON] = false;
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
				if (_pulses[BUTTON] < 0.6f)
				{
					_pulses[BUTTON] = 0.9f;
				}
				if (Enabled)
				{
					if (_directions[BUTTON])
					{
						_pulses[BUTTON] += frameDeltaTime / 2;
						if (_pulses[BUTTON] > 0.9f)
						{
							_directions[BUTTON] = !_directions[BUTTON];
							_pulses[BUTTON] = 0.9f;
						}
					}
					else
					{
						_pulses[BUTTON] -= frameDeltaTime / 2;
						if (_pulses[BUTTON] < 0.6f)
						{
							_directions[BUTTON] = !_directions[BUTTON];
							_pulses[BUTTON] = 0.6f;
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
			if (_pulses[BUTTON] > 0)
			{
				_pulses[BUTTON] -= frameDeltaTime * 2;
				if (_pulses[BUTTON] < 0)
				{
					_pulses[BUTTON] = 0;
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
