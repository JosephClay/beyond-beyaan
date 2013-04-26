using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;
using Font = GorgonLibrary.Graphics.Font;

namespace Beyond_Beyaan.Data_Modules
{
	public enum UITypeEnum { IMAGE, STRETCHABLE_IMAGE, LABEL, BUTTON, STRETCHABLE_BUTTON, DROPDOWN, CAMERA_VIEW }
	public enum BaseUISprites
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
									case "cameraview":
										Type = UITypeEnum.CAMERA_VIEW;
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

		private string _spriteStr;
		private Dictionary<BaseUISprites, BBSprite> _sprites;
		private TextSprite _textSprite;
		private RenderImage _renderImage;
		private string _text; 
		private UITypeEnum _type;
		private Color _color;
		private Color _disabledColor;
		private Color _textColor;

		private int _xPos;
		private int _yPos;
		private string _xStr; //For data bindings
		private string _yStr;

		private int _width;
		private int _height;
		private string _widthStr;
		private string _heightStr;

		#region Drop-down properties
		private bool _dropped;
		private int _maxVisible;
		private int _arrowXOffset;
		private int _arrowYOffset;
		private int _selectedIndex;
		#endregion

		private List<float> _pulses;
		private List<bool> _directions;
		private List<bool> _presseds;
		private List<bool> _selecteds;

		public string Name { get; set; }
		public UITypeEnum Type { get { return _type; } }
		public bool Enabled { get; set; }
		public string DataSource { get; set; }
		public object Value { get { return _selectedIndex == -1 ? null : _objects[_selectedIndex]; } }

		#region Events
		public string OnClick { get; set; }
		#endregion

		#region Template
		private List<object> _objects; 
		private List<Screen> _screens;
		private XElement _template;
		private int _templateWidth;
		private int _templateHeight;
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

			switch (_type)
			{
				//TODO: Add scrollbars and sliders
				case UITypeEnum.DROPDOWN:
				case UITypeEnum.BUTTON:
				case UITypeEnum.STRETCHABLE_BUTTON:
					{
						ClearButtonData();
					} break;
			}
			_text = string.Empty;
			_xStr = string.Empty;
			_yStr = string.Empty;
			_widthStr = string.Empty;
			_heightStr = string.Empty;
			_spriteStr = string.Empty;
		}

		public void SetRect(int x, int y, int width, int height, string xstr, string ystr, string widthstr, string heightstr)
		{
			_xPos = x;
			_yPos = y;
			_width = width;
			_height = height;
			_xStr = xstr;
			_yStr = ystr;
			_widthStr = widthstr;
			_heightStr = heightstr;
			if (_type == UITypeEnum.CAMERA_VIEW)
			{
				_renderImage = new RenderImage(Name + "RenderImage", width, height, ImageBufferFormats.BufferRGB888A8);
				_renderImage.BlendingMode = BlendingModes.Modulated;
			}
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
			_textSprite = new TextSprite("Label", content, font);
			_text = content;
		}

		public void SetSprite(BBSprite sprite, string spriteStr)
		{
			_sprites[BaseUISprites.BACKGROUND] = sprite;
			_spriteStr = spriteStr;
		}

		public void SetTemplate(XElement template)
		{
			_screens = new List<Screen>();
			_template = template;
			if (_template.Attribute("width") != null)
			{
				_templateWidth = int.Parse(_template.Attribute("width").Value);
			}
			else
			{
				_templateWidth = _width;
			}
			if (_template.Attribute("height") != null)
			{
				_templateHeight = int.Parse(_template.Attribute("height").Value);
			}
			else
			{
				_templateHeight = _height;
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
				case UITypeEnum.CAMERA_VIEW:
					{
						return CameraView_MouseHover(mouseX, mouseY, frameDeltaTime);
					}
				default: return false;
			}
		}

		public bool MouseDown(int mouseX, int mouseY)
		{
			switch (_type)
			{
				case UITypeEnum.DROPDOWN:
					{
						return DropDown_MouseDown(mouseX, mouseY);
					}
				case UITypeEnum.STRETCHABLE_BUTTON:
				case UITypeEnum.BUTTON:
					{
						return Button_MouseDown(mouseX, mouseY);
					}
			}
			return false;
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
			Draw(0, 0, 1);
		}

		public void Draw(int xOffset, int yOffset)
		{
			Draw(xOffset, yOffset, 1);
		}
		public void Draw(int xOffset, int yOffset, float scale)
		{
			switch (_type)
			{
				case UITypeEnum.STRETCHABLE_BUTTON:
					{
						StretchableButton_Draw(xOffset, yOffset);
					} break;
				case UITypeEnum.BUTTON:
					{
						Button_Draw(xOffset, yOffset);
					} break;
				case UITypeEnum.STRETCHABLE_IMAGE:
					{
						StretchableImage_Draw(xOffset, yOffset);
					} break;
				case UITypeEnum.IMAGE:
					{
						Image_Draw(xOffset, yOffset, scale);
					} break;
				case UITypeEnum.DROPDOWN:
					{
						DropDown_Draw(xOffset, yOffset);
					} break;
				case UITypeEnum.LABEL:
					{
						Label_Draw(xOffset, yOffset, scale);
					} break;
				case UITypeEnum.CAMERA_VIEW:
					{
						CameraView_Draw(xOffset, yOffset);
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
		private void ClearButtonData()
		{
			//Main button
			_pulses.Add(0);
			_directions.Add(false);
			_presseds.Add(false);
			_selecteds.Add(false);
			if (_type == UITypeEnum.BUTTON || _type == UITypeEnum.STRETCHABLE_BUTTON)
			{
				return;
			}
			//Up/left, down/right, and scroll buttons
			for (int i = 0; i < 3; i++)
			{
				_pulses.Add(0);
				_directions.Add(false);
				_presseds.Add(false);
				_selecteds.Add(false);
			}
		}
		private void DrawStretchableWith9(List<BBSprite> sprites, Color color, int xOffset, int yOffset)
		{
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			sprites[0].Draw(xPos, yPos, 1, 1, color);
			sprites[1].Draw(xPos + sprites[0].Width, yPos, (_width - (sprites[0].Width + sprites[2].Width)) / sprites[1].Width, 1, color);
			sprites[2].Draw(xPos + (_width - sprites[2].Width), yPos, 1, 1, color);
			sprites[3].Draw(xPos, yPos + sprites[0].Height, 1, (_height - (sprites[0].Height + sprites[6].Height)) / sprites[3].Height, color);
			sprites[4].Draw(xPos + sprites[3].Width, yPos + sprites[1].Height, (_width - (sprites[3].Width + sprites[5].Width)) / sprites[4].Width, (_height - (sprites[1].Height + sprites[7].Height)) / sprites[4].Height, color);
			sprites[5].Draw(xPos + (_width - sprites[5].Width), yPos + sprites[2].Height, 1, (_height - (sprites[2].Height + sprites[8].Height)) / sprites[5].Height, color);
			sprites[6].Draw(xPos, yPos + (_height - sprites[6].Height), 1, 1, color);
			sprites[7].Draw(xPos + (sprites[6].Width), yPos + (_height - sprites[7].Height), (_width - (sprites[6].Width + sprites[8].Width)) / sprites[7].Width, 1, color);
			sprites[8].Draw(xPos + (_width - sprites[8].Width), yPos + (_height - sprites[8].Height), 1, 1, color);
		}
		private void DrawStretchableTop6(List<BBSprite> sprites, Color color, int xOffset, int yOffset, int height)
		{
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			sprites[0].Draw(xPos, yPos, 1, 1, color);
			sprites[1].Draw(xPos + sprites[0].Width, yPos, (_width - (sprites[0].Width + sprites[2].Width)) / sprites[1].Width, 1, color);
			sprites[2].Draw(xPos + (_width - sprites[2].Width), yPos, 1, 1, color);
			sprites[3].Draw(xPos, yPos + sprites[0].Height, 1, (height - sprites[0].Height) / sprites[3].Height, color);
			sprites[4].Draw(xPos + sprites[3].Width, yPos + sprites[1].Height, (_width - (sprites[3].Width + sprites[5].Width)) / sprites[4].Width, (height - sprites[1].Height) / sprites[4].Height, color);
			sprites[5].Draw(xPos + (_width - sprites[5].Width), yPos + sprites[2].Height, 1, (height - sprites[2].Height) / sprites[5].Height, color);
		}
		private void DrawStretchableMiddle3(List<BBSprite> sprites, Color color, int xOffset, int yOffset, int height)
		{
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			sprites[0].Draw(xPos, yPos, 1, height / sprites[0].Height, color);
			sprites[1].Draw(xPos + sprites[0].Width, yPos, (_width - (sprites[0].Width + sprites[2].Width)) / sprites[1].Width, height / sprites[1].Height, color);
			sprites[2].Draw(xPos + (_width - sprites[2].Width), yPos, 1, height / sprites[2].Height, color);
		}
		private void DrawStretchableBottom3(List<BBSprite> sprites, Color color, int xOffset, int yOffset)
		{
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			sprites[0].Draw(xPos, yPos, 1, 1, color);
			sprites[1].Draw(xPos + (sprites[0].Width), yPos, (_width - (sprites[0].Width + sprites[2].Width)) / sprites[1].Width, 1, color);
			sprites[2].Draw(xPos + (_width - sprites[2].Width), yPos, 1, 1, color);
		}
		
		public void FillData(List<object> values, GameMain gameMain)
		{
			_objects = new List<object>(values);
			_screens.Clear();
			_selectedIndex = -1;
			ClearButtonData();
			foreach (var value in _objects)
			{
				string reason;
				Screen screen = new Screen();
				screen.LoadScreen(_template, _templateWidth, _templateHeight, gameMain, out reason);
				screen.LoadValues(value, gameMain);
				_screens.Add(screen);
				_pulses.Add(0);
				_directions.Add(false);
				_presseds.Add(false);
				_selecteds.Add(false);
			}
		}
		public void FillContent(GameMain gameMain)
		{
			if (!string.IsNullOrEmpty(_text) && _text.StartsWith("[") && _text.EndsWith("]"))
			{
				_textSprite = new TextSprite("Label", gameMain.GetValue(_text.Substring(1, _text.Length - 2)), gameMain.FontManager.GetDefaultFont());
			}
		}
		public void LoadValues<T>(T value, GameMain gameMain) where T : class
		{
			//A value that needs to be replaced is denoted with []
			if (_text.StartsWith("[") && _text.EndsWith("]"))
			{
				string property = _text.Substring(1, _text.Length - 2);
				string newValue = value.GetType().GetProperty(property, typeof (string)).GetValue(value, null).ToString();
				_textSprite = new TextSprite("Label", newValue, gameMain.FontManager.GetDefaultFont());
			}
			if (_xStr.StartsWith("[") && _xStr.EndsWith("]"))
			{
				string property = _xStr.Substring(1, _xStr.Length - 2);
				_xPos = (int)value.GetType().GetProperty(property, typeof(int)).GetValue(value, null);
			}
			if (_yStr.StartsWith("[") && _yStr.EndsWith("]"))
			{
				string property = _yStr.Substring(1, _yStr.Length - 2);
				_yPos = (int)value.GetType().GetProperty(property, typeof(int)).GetValue(value, null);
			}
			if (_spriteStr.StartsWith("[") && _spriteStr.EndsWith("]"))
			{
				string property = _spriteStr.Substring(1, _spriteStr.Length - 2);
				_sprites[BaseUISprites.BACKGROUND] = (BBSprite)value.GetType().GetProperty(property, typeof(BBSprite)).GetValue(value, null);
			}
		}
		#endregion

		#region Individual UI functions

		#region Label functions
		private void Label_Draw(int xOffset, int yOffset, float scale)
		{
			_textSprite.SetPosition(_xPos + xOffset, _yPos + yOffset);
			_textSprite.SetScale(scale, scale);
			_textSprite.Color = _textColor;
			_textSprite.Draw();
		}
		#endregion

		#region CameraView functions
		private void CameraView_Draw(int xOffset, int yOffset)
		{
			_renderImage.Clear(Color.FromArgb(0, Color.Black));
			RenderTarget old = GorgonLibrary.Gorgon.CurrentRenderTarget;
			GorgonLibrary.Gorgon.CurrentRenderTarget = _renderImage;
			foreach (var item in _screens)
			{
				item.Draw(xOffset, yOffset, 0.5f);
			}
			GorgonLibrary.Gorgon.CurrentRenderTarget = old;
			_renderImage.Blit(_xPos, _yPos);
		}

		private bool CameraView_MouseHover(int mouseX, int mouseY, float frameDeltaTime)
		{
			foreach (var item in _screens)
			{
				item.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			return false;
		}

		#endregion

		#region DropDown Functions
		private void DropDown_Draw(int xOffset, int yOffset)
		{
			var bgSpritesTop = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_BACKGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_BACKGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_BACKGROUND]});
			var bgSpritesMiddle = new List<BBSprite>(new[] {_sprites[BaseUISprites.LEFT_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_BACKGROUND]});
			var bgSpritesBottom = new List<BBSprite>(new[] {_sprites[BaseUISprites.BOTTOM_LEFT_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_BACKGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_BACKGROUND]});

			var fgSpritesTop = new List<BBSprite>(new[] {_sprites[BaseUISprites.TOP_LEFT_FOREGROUND],
													_sprites[BaseUISprites.TOP_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.TOP_RIGHT_FOREGROUND],
													_sprites[BaseUISprites.LEFT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_FOREGROUND]});
			var fgSpritesMiddle = new List<BBSprite>(new[] {_sprites[BaseUISprites.LEFT_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.RIGHT_MIDDLE_FOREGROUND]});
			var fgSpritesBottom = new List<BBSprite>(new[] {_sprites[BaseUISprites.BOTTOM_LEFT_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_MIDDLE_FOREGROUND],
													_sprites[BaseUISprites.BOTTOM_RIGHT_FOREGROUND]});
			var bgArrow = _sprites[BaseUISprites.DOWN_BACKGROUND];
			var fgArrow = _sprites[BaseUISprites.DOWN_FOREGROUND];
			//If it's not dropped down, we can just imitate the StretchButton
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			int topY = (int)(bgSpritesTop[0].Height + (_height - (bgSpritesTop[0].Height + bgSpritesBottom[0].Height)));
			if (!_dropped)
			{
				if (Enabled)
				{
					if (_presseds[BUTTON] || _selecteds[BUTTON])
					{
						DrawStretchableTop6(fgSpritesTop, _color, xOffset, yOffset, topY);
						DrawStretchableBottom3(fgSpritesBottom, _color, xOffset, yOffset + topY);
						fgArrow.Draw(xPos + _width - (fgArrow.Width + _arrowXOffset), yPos + _arrowYOffset, 1, 1, _color);
					}
					else
					{
						DrawStretchableTop6(bgSpritesTop, _color, xOffset, yOffset, topY);
						DrawStretchableBottom3(bgSpritesBottom, _color, xOffset, yOffset + topY);
						bgArrow.Draw(xPos + _width - (bgArrow.Width + _arrowXOffset), yPos + _arrowYOffset, 1, 1, _color);
						if (_pulses[BUTTON] > 0)
						{
							var color = Color.FromArgb((byte) (255*_pulses[BUTTON]), _color);
							DrawStretchableTop6(fgSpritesTop, color, xOffset, yOffset, topY);
							DrawStretchableBottom3(fgSpritesBottom, color, xOffset, yOffset + topY);
							fgArrow.Draw(xPos + _width - (fgArrow.Width + _arrowXOffset), yPos + _arrowYOffset, 1, 1, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
						}
					}
				}
				else
				{
					if (_selecteds[BUTTON])
					{
						DrawStretchableTop6(fgSpritesTop, _disabledColor, xOffset, yOffset, topY);
						DrawStretchableBottom3(fgSpritesBottom, _disabledColor, xOffset, yOffset + topY);
						fgArrow.Draw(xPos + _width - (fgArrow.Width + _arrowXOffset), yPos + _arrowYOffset, 1, 1, _disabledColor);
					}
					else
					{
						DrawStretchableTop6(bgSpritesTop, _disabledColor, xOffset, yOffset, topY);
						DrawStretchableBottom3(bgSpritesBottom, _disabledColor, xOffset, yOffset + topY);
						bgArrow.Draw(xPos + _width - (bgArrow.Width + _arrowXOffset), yPos + _arrowYOffset, 1, 1, _disabledColor);
					}
				}
				if (_screens != null && _screens.Count > 0 && _selectedIndex >= 0)
				{
					_screens[_selectedIndex].Draw(xPos + (_width - _templateWidth), yPos + (_height - _templateHeight));
				}
			}
			else
			{
				topY = _height;
				if (_presseds[BUTTON] || _selecteds[BUTTON])
				{
					DrawStretchableTop6(fgSpritesTop, _color, xOffset, yOffset, topY);
				}
				else
				{
					DrawStretchableTop6(bgSpritesTop, _color, xOffset, yOffset, topY);
					if (_pulses[BUTTON] > 0)
					{
						var color = Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color);
						DrawStretchableTop6(fgSpritesTop, color, xOffset, yOffset, topY);
					}
				}
				for (int i = FIRST_DROPDOWN_BUTTON; i < _screens.Count + FIRST_DROPDOWN_BUTTON; i++)
				{
					if (_presseds[i] || _selecteds[i])
					{
						DrawStretchableMiddle3(fgSpritesMiddle, _color, xOffset, yOffset + topY + ((i - FIRST_DROPDOWN_BUTTON) * _templateHeight), _templateHeight);
					}
					else
					{
						DrawStretchableMiddle3(bgSpritesMiddle, _color, xOffset, yOffset + topY + ((i - FIRST_DROPDOWN_BUTTON) * _templateHeight), _templateHeight);
						if (_pulses[i] > 0)
						{
							var color = Color.FromArgb((byte)(255 * _pulses[i]), _color);
							DrawStretchableMiddle3(fgSpritesMiddle, color, xOffset, yOffset + topY + ((i - FIRST_DROPDOWN_BUTTON) * _templateHeight), _templateHeight);
						}
					}
				}
				DrawStretchableBottom3(bgSpritesBottom, _color, xOffset, yOffset + topY + (_screens.Count * _templateHeight));
				if (_screens != null && _screens.Count > 0)
				{
					if (_selectedIndex >= 0)
					{
						_screens[_selectedIndex].Draw(xPos + (_width - _templateWidth), yPos + (_height - _templateHeight));
					}
					for (int i = 0; i < _screens.Count; i++)
					{
						_screens[i].Draw(xPos + (_width - _templateWidth), yPos + (_height - _templateHeight) + ((i + 1) * _templateHeight));
					}
				}
			}
		}

		private bool DropDown_MouseDown(int mouseX, int mouseY)
		{
			if (Button_MouseDown(mouseX, mouseY))
			{
				return true;
			}
			if (_dropped)
			{
				for (int i = 0; i < _screens.Count; i++)
				{
					if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos + _height + (_templateHeight*i) && mouseY < _yPos + _height + (_templateHeight*(i + 1)))
					{
						_presseds[FIRST_DROPDOWN_BUTTON + i] = true;
						return true;
					}
				}
			}
			return false;
		}

		private bool DropDown_MouseUp(int mouseX, int mouseY)
		{
			if (!_dropped)
			{
				if (Button_MouseUp(mouseX, mouseY))
				{
					_dropped = true;
				}
			}
			else
			{
				_dropped = false;
				if (Button_MouseUp(mouseX, mouseY))
				{
					return false;
				}
				for (int i = 0; i < _screens.Count; i++)
				{
					if (_presseds[FIRST_DROPDOWN_BUTTON + i])
					{
						_presseds[FIRST_DROPDOWN_BUTTON + i] = false;
						if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos + _height + (_templateHeight*i) && mouseY < _yPos + _height + (_templateHeight*(i + 1)))
						{
							_selectedIndex = i;
							return true;
						}
					}
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
			bool result = Button_MouseHover(mouseX, mouseY, frameDeltaTime);
			for (int i = 0; i < _screens.Count; i++)
			{
				if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos + _height + (_templateHeight*i) && mouseY < _yPos + _height + (_templateHeight*(i + 1)))
				{
					UpdatePulse(FIRST_DROPDOWN_BUTTON + i, true, frameDeltaTime);
					result = true;
				}
				else
				{
					UpdatePulse(FIRST_DROPDOWN_BUTTON + i, false, frameDeltaTime);
				}
			}
			return result;
		}
		#endregion

		#region Stretchable Image functions
		private void StretchableImage_Draw(int xOffset, int yOffset)
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

			DrawStretchableWith9(sprites, _color, xOffset, yOffset);
		}
		#endregion

		#region Image functions
		private void Image_Draw(int xOffset, int yOffset, float scale)
		{
			var sprite = _sprites[BaseUISprites.BACKGROUND];
			if ((_width == 0 || _height == 0) && scale == 1)
			{
				//Possibly just want the normal scale
				sprite.Draw(_xPos + xOffset, _yPos + yOffset, 1, 1, _color);
			}
			else if (scale == 1)
			{
				sprite.Draw(_xPos + xOffset, _yPos + yOffset, _width / sprite.Width, _height / sprite.Height, _color);
			}
			else
			{
				sprite.Draw((_xPos + xOffset) * scale, (_yPos + yOffset) * scale, scale, scale, _color);
			}
		}
		#endregion

		#region Stretchable Button Functions
		private void StretchableButton_Draw(int xOffset, int yOffset)
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
					DrawStretchableWith9(fgSprites, _color, xOffset, yOffset);
				}
				else
				{
					DrawStretchableWith9(bgSprites, _color, xOffset, yOffset);
					if (_pulses[BUTTON] > 0)
					{
						DrawStretchableWith9(fgSprites, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color), xOffset, yOffset);
					}
				}
			}
			else
			{
				if (_selecteds[BUTTON])
				{
					DrawStretchableWith9(fgSprites, _disabledColor, xOffset, yOffset);
				}
				else
				{
					DrawStretchableWith9(bgSprites, _disabledColor, xOffset, yOffset);
				}
			}
			if (_textSprite != null)
			{
				_textSprite.SetPosition(_xPos + xOffset + _width / 2 - _textSprite.Width / 2, _yPos + yOffset + _height / 2 - _textSprite.Height / 2);
				_textSprite.Color = _textColor;
				_textSprite.Draw();
			}
		}
		#endregion

		#region Button functions
		private void Button_Draw(int xOffset, int yOffset)
		{
			var backgroundSprite = _sprites[BaseUISprites.BACKGROUND];
			var foregroundSprite = _sprites[BaseUISprites.FOREGROUND];
			int xPos = _xPos + xOffset;
			int yPos = _yPos + yOffset;
			if (Enabled)
			{
				if (_presseds[BUTTON] || _selecteds[BUTTON])
				{
					foregroundSprite.Draw(xPos, yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
				}
				else
				{
					backgroundSprite.Draw(xPos, yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _color);
					if (_pulses[BUTTON] > 0)
					{
						foregroundSprite.Draw(xPos, yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, Color.FromArgb((byte)(255 * _pulses[BUTTON]), _color));
					}
				}
			}
			else
			{
				if (_selecteds[BUTTON])
				{
					foregroundSprite.Draw(xPos, yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				}
				else
				{
					backgroundSprite.Draw(xPos, yPos, _width / foregroundSprite.Width, _height / foregroundSprite.Height, _disabledColor);
				}
			}
			if (_textSprite != null)
			{
				_textSprite.SetPosition(xPos + _width / 2 - _textSprite.Width / 2, yPos + _height / 2 - _textSprite.Height / 2);
				_textSprite.Color = _textColor;
				_textSprite.Draw();
			}
		}
		private bool Button_MouseDown(int mouseX, int mouseY)
		{
			if (!Enabled)
			{
				return false;
			}
			if (mouseX >= _xPos && mouseX < _xPos + _width && mouseY >= _yPos && mouseY < _yPos + _height)
			{
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}*/
				_presseds[BUTTON] = true;
				return true;
			}
			return false;
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
				UpdatePulse(BUTTON, true, frameDeltaTime);
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
				}*/
				return true;
			}
			UpdatePulse(BUTTON, false, frameDeltaTime);
			/*if (toolTipEnabled)
			{
				toolTip.SetShowing(false);
			}*/
			return false;
		}
		private void UpdatePulse(int whichButton, bool isHovering, float frameDeltaTime)
		{
			if (isHovering)
			{
				if (_pulses[whichButton] < 0.6f)
				{
					_pulses[whichButton] = 0.9f;
				}
				if (Enabled)
				{
					if (_directions[whichButton])
					{
						_pulses[whichButton] += frameDeltaTime / 2;
						if (_pulses[whichButton] > 0.9f)
						{
							_directions[whichButton] = !_directions[whichButton];
							_pulses[whichButton] = 0.9f;
						}
					}
					else
					{
						_pulses[whichButton] -= frameDeltaTime / 2;
						if (_pulses[whichButton] < 0.6f)
						{
							_directions[whichButton] = !_directions[whichButton];
							_pulses[whichButton] = 0.6f;
						}
					}
				}
				/*if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
				}*/
			}
			else if (_pulses[whichButton] > 0)
			{
				_pulses[whichButton] -= frameDeltaTime * 2;
				if (_pulses[whichButton] < 0)
				{
					_pulses[whichButton] = 0;
				}
			}
		}
		#endregion
		#endregion
	}
}
