using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	public class Screen
	{
		private int _width;
		private int _height;
		private int _xPos;
		private int _yPos;
		private int _origX;
		private int _origY;
		private int _mouseX;
		private int _mouseY;
		private bool _moving;
		private bool _movable;
		private GameMain _gameMain;
		private List<UIType> UITypes;
		public Dictionary<string, string> Values { get; private set; }

		public bool LoadScreen(XElement rootElement, int width, int height, GameMain gameMain, out string reason)
		{
			Values = new Dictionary<string, string>();
			//Set defaults
			_movable = false;
			_width = width;
			_height = height;
			_xPos = 0;
			_yPos = 0;

			_gameMain = gameMain;
			UITypes = new List<UIType>();

			try
			{
				int newWidth = -1;
				int newHeight = -1;
				foreach (var attribute in rootElement.Attributes())
				{
					switch (attribute.Name.LocalName.ToLower())
					{
						case "movable":
							{
								_movable = bool.Parse(attribute.Value);
							} break;
						case "width":
							{
								newWidth = GetValue(attribute.Value);
							} break;
						case "height":
							{
								newHeight = GetValue(attribute.Value);
							} break;
						case "xpos":
							{
								_xPos = GetValue(attribute.Value);
							} break;
						case "ypos":
							{
								_yPos = GetValue(attribute.Value);
							} break;
					}
				}
				if (newWidth != -1)
				{
					_width = newWidth;
				}
				if (newHeight != -1)
				{
					_height = newHeight;
				}

				foreach (var element in rootElement.Elements())
				{
					if (element.Attribute("UIType") == null)
					{
						reason = "UIType not specified in {0}";
						return false;
					}
					string type = element.Attribute("UIType").Value;
					UIType newUI = gameMain.UITypeManager.GetUI(type, this, _gameMain.Random);
					if (newUI == null)
					{
						reason = type + " UI Type in {0} is not defined in UITypes.xml.";
						return false;
					}
					int x = 0;
					int y = 0;
					int uiWidth = 0;
					int uiHeight = 0;
					string xStr = null;
					string yStr = null;
					string widthStr = null;
					string heightStr = null;
					string font = null;
					string content = null;
					int arrowXOffset = 0;
					int arrowYOffset = 0;

					foreach (var attribute in element.Attributes())
					{
						switch (attribute.Name.LocalName.ToLower())
						{
							case "name":
								{
									newUI.Name = attribute.Value;
								} break;
							case "color":
								{
									string[] values = attribute.Value.Split(new[] {','});
									if (values.Length == 3)
									{
										newUI.SetColor(255, byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]));
									}
									else
									{
										newUI.SetColor(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3]));
									}
								} break;
							case "textcolor":
							{
								string[] values = attribute.Value.Split(new[] { ',' });
								if (values.Length == 3)
								{
									newUI.SetTextColor(255, byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]));
								}
								else
								{
									newUI.SetTextColor(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3]));
								}
							} break;
							case "font":
								{
									font = attribute.Value;
								} break;
							case "content":
								{
									content = attribute.Value;
								} break;
							case "xpos":
								{
									x = GetValue(attribute.Value);
									xStr = attribute.Value;
								} break;
							case "ypos":
								{
									y = GetValue(attribute.Value);
									yStr = attribute.Value;
								} break;
							case "width":
								{
									uiWidth = GetValue(attribute.Value);
									widthStr = attribute.Value;
								} break;
							case "height":
								{
									uiHeight = GetValue(attribute.Value);
									heightStr = attribute.Value;
								} break;
							case "onclick":
								{
									newUI.OnClick = attribute.Value;
								} break;
							case "arrowxoffset":
								{
									arrowXOffset = int.Parse(attribute.Value);
								} break;
							case "arrowyoffset":
								{
									arrowYOffset = int.Parse(attribute.Value);
								} break;
							case "cameraborder":
								{
									newUI.SetCameraBorder(int.Parse(attribute.Value));
								} break;
							case "datasource":
								{
									newUI.DataSource = attribute.Value;
								} break;
							case "sprite":
								{
									newUI.SetSprite(_gameMain.SpriteManager.GetSprite(attribute.Value, gameMain.Random), attribute.Value);
								} break;
							case "shader":
								{
									newUI.SetShader(_gameMain.ShaderManager.GetShader(attribute.Value), attribute.Value);
								} break;
							case "shadervalue":
								{
									newUI.SetShaderValue(attribute.Value);
								} break;
						}
					}
					if (!string.IsNullOrEmpty(content))
					{
						if (!string.IsNullOrEmpty(font))
						{
							newUI.SetText(content, _gameMain.FontManager.GetFont(font));
						}
						else
						{
							newUI.SetText(content, _gameMain.FontManager.GetDefaultFont());
						}
					}
					newUI.SetRect(x, y, uiWidth, uiHeight, xStr, yStr, widthStr, heightStr);
					newUI.SetArrowOffset(arrowXOffset, arrowYOffset);
					if (element.HasElements)
					{
						newUI.SetTemplate(element.Element("Template"));
					}
					UITypes.Add(newUI);
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

		private int GetValue(string value)
		{
			if (value.StartsWith("[") && value.EndsWith("]"))
			{
				return -1;
			}
			int result = 0;
			while (value.Length > 0)
			{
				int tempValue = 0;
				bool positive = true;
				if (value.StartsWith("-"))
				{
					positive = false;
					value = value.Substring(1);
				}
				else if (value.StartsWith("+"))
				{
					//Just remove this
					value = value.Substring(1);
				}
				if (value.StartsWith("WIDTH", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = _width;
					value = value.Substring(5);
				}
				else if (value.StartsWith("HEIGHT", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = _height;
					value = value.Substring(6);
				}
				else if (value.StartsWith("XMIDDLE", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = _width / 2;
					value = value.Substring(7);
				}
				else if (value.StartsWith("YMIDDLE", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = _height / 2;
					value = value.Substring(7);
				}
				else
				{
					if (int.TryParse(value, out tempValue))
					{
						value = string.Empty;
					}
					else
					{
						throw new Exception("Value \"" + value + "\" cannot be parsed.");
					}
				}
				if (positive)
				{
					result += tempValue;
				}
				else
				{
					result -= tempValue;
				}
			}
			return result;
		}

		public void Draw()
		{
			foreach (var uiType in UITypes)
			{
				uiType.Draw(_xPos, _yPos);
			}
		}
		public void Draw(int x, int y)
		{
			foreach (var uiType in UITypes)
			{
				uiType.Draw(x, y);
			}
		}
		public void Draw(int x, int y, float scale)
		{
			foreach (var uiType in UITypes)
			{
				uiType.Draw(x, y, scale);
			}
		}

		public bool MouseDown(int x, int y, int whichButton)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				bool result = false;
				foreach (var uiType in UITypes)
				{
					result |= uiType.MouseDown(x - _xPos, y - _yPos);
				}
				if (_movable && !result)
				{
					_moving = true;
					_mouseX = x;
					_mouseY = y;
					_origX = _xPos;
					_origY = _yPos;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y, int whichButton)
		{
			bool result = false;
			foreach (var uiType in UITypes)
			{
				if (uiType.MouseUp(x - _xPos, y - _yPos))
				{
					result = true;
					_gameMain.OnClick(uiType.OnClick, this);
				}
			}
			if (_moving)
			{
				result = true;
				_moving = false;
			}
			return result;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (_moving)
			{
				_xPos = (x - _mouseX) + _origX;
				_yPos = (y - _mouseY) + _origY;

				if (_xPos + _width > _gameMain.ScreenWidth)
				{
					_xPos = _gameMain.ScreenWidth - _width;
				}
				if (_yPos + _height > _gameMain.ScreenHeight)
				{
					_yPos = _gameMain.ScreenHeight - _height;
				}
				if (_xPos < 0)
				{
					_xPos = 0;
				}
				if (_yPos < 0)
				{
					_yPos = 0;
				}
				return true;
			}
			foreach (var uiType in UITypes)
			{
				result |= uiType.MouseHover(x - _xPos, y - _yPos, frameDeltaTime);
				uiType.Update(frameDeltaTime, _gameMain.Random);
			}
			return result;
		}

		public bool MouseScroll(int mouseX, int mouseY, int delta)
		{
			bool result = false;
			foreach (var uiType in UITypes)
			{
				result |= uiType.MouseScroll(mouseX, mouseY, delta);
			}
			return result;
		}

		public void RefreshData()
		{
			foreach (var uiType in UITypes)
			{
				if (!string.IsNullOrEmpty(uiType.DataSource))
				{
					uiType.FillData(_gameMain.GetData(uiType.DataSource), _gameMain);
				}
				uiType.FillContent(_gameMain);
			}
		}

		public void LoadValues(object value, GameMain gameMain)
		{
			foreach (var uiType in UITypes)
			{
				uiType.LoadValues(value, gameMain);
			}
		}

		public object GetUIValue(string uiName)
		{
			foreach (var uiType in UITypes)
			{
				if (uiType.Name == uiName)
				{
					return uiType.Value;
				}
			}
			return null;
		}
	}
}
