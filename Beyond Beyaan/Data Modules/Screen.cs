using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	public class Screen
	{
		private int _width;
		private int _height;
		private GameMain _gameMain;
		private List<UIType> UITypes;

		public bool LoadScreen(XElement rootElement, int width, int height, GameMain gameMain, out string reason)
		{
			_width = width;
			_height = height;
			_gameMain = gameMain;
			UITypes = new List<UIType>();

			try
			{
				foreach (var element in rootElement.Elements())
				{
					if (element.Attribute("UIType") == null)
					{
						reason = "UIType not specified in {0}";
						return false;
					}
					string type = element.Attribute("UIType").Value;
					UIType newUI = gameMain.UITypeManager.GetUI(type, _gameMain.Random);
					if (newUI == null)
					{
						reason = type + " UI Type in {0} is not defined in UITypes.xml.";
						return false;
					}
					int x = 0;
					int y = 0;
					int uiWidth = 0;
					int uiHeight = 0;
					string font = null;
					string content = null;
					int arrowXOffset = 0;
					int arrowYOffset = 0;

					foreach (var attribute in element.Attributes())
					{
						switch (attribute.Name.LocalName.ToLower())
						{
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
								} break;
							case "ypos":
								{
									y = GetValue(attribute.Value);
								} break;
							case "width":
								{
									uiWidth = GetValue(attribute.Value);
								} break;
							case "height":
								{
									uiHeight = GetValue(attribute.Value);
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
							case "datasource":
								{
									newUI.DataSource = attribute.Value;
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
					newUI.SetRect(x, y, uiWidth, uiHeight);
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
				uiType.Draw();
			}
		}
		public void Draw(int x, int y)
		{
			foreach (var uiType in UITypes)
			{
				uiType.Draw(x, y);
			}
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			foreach (var uiType in UITypes)
			{
				uiType.MouseDown(x, y);
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			foreach (var uiType in UITypes)
			{
				if (uiType.MouseUp(x, y))
				{
					_gameMain.OnClick(uiType.OnClick);
				}
			}
		}

		public void MouseHover(int x, int y, float frameDeltaTime)
		{
			foreach (var uiType in UITypes)
			{
				uiType.MouseHover(x, y, frameDeltaTime);
				uiType.Update(frameDeltaTime, _gameMain.Random);
			}
		}

		public void RefreshData()
		{
			foreach (var uiType in UITypes)
			{
				if (!string.IsNullOrEmpty(uiType.DataSource))
				{
					uiType.FillData(_gameMain.GetData(uiType.DataSource), _gameMain);
				}
			}
		}

		public void LoadValues(object value, GameMain gameMain)
		{
			foreach (var uiType in UITypes)
			{
				uiType.LoadValues(value, gameMain);
			}
		}
	}
}
