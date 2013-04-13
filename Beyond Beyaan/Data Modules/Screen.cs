using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Data_Modules
{
	public class Screen
	{
		private List<UIType> UITypes;

		public bool LoadScreen(string filePath, GameMain gameMain, Random r, out string reason)
		{
			UITypes = new List<UIType>();

			try
			{
				XDocument file = XDocument.Load(filePath);
				XElement root = file.Element("Screen");

				foreach (var element in root.Elements())
				{
					if (element.Attribute("UIType") == null)
					{
						reason = "UIType not specified in " + filePath;
						return false;
					}
					string type = element.Attribute("UIType").Value;
					UIType newUI = gameMain.UITypeManager.GetUI(type, r);
					if (newUI == null)
					{
						reason = type + " UI Type in " + filePath + " is not defined in UITypes.xml.";
						return false;
					}
					int x = 0;
					int y = 0;
					int width = 0;
					int height = 0;

					foreach (var attribute in element.Attributes())
					{
						switch (attribute.Name.LocalName.ToLower())
						{
							case "xpos":
								{
									x = GetValue(attribute.Value, gameMain);
								} break;
							case "ypos":
								{
									y = GetValue(attribute.Value, gameMain);
								} break;
							case "width":
								{
									width = GetValue(attribute.Value, gameMain);
								} break;
							case "height":
								{
									height = GetValue(attribute.Value, gameMain);
								} break;
						}
					}
					newUI.SetRect(x, y, width, height);
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

		private int GetValue(string value, GameMain gameMain)
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
					tempValue = gameMain.ScreenWidth;
					value = value.Substring(5);
				}
				else if (value.StartsWith("HEIGHT", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = gameMain.ScreenHeight;
					value = value.Substring(6);
				}
				else if (value.StartsWith("XMIDDLE", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = gameMain.ScreenWidth / 2;
					value = value.Substring(7);
				}
				else if (value.StartsWith("YMIDDLE", StringComparison.CurrentCultureIgnoreCase))
				{
					tempValue = gameMain.ScreenHeight / 2;
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
				uiType.MouseUp(x, y);
			}
		}

		public void MouseHover(int x, int y, float frameDeltaTime)
		{
			foreach (var uiType in UITypes)
			{
				uiType.MouseHover(x, y, frameDeltaTime);
			}
		}
	}
}
