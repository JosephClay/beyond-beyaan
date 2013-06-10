using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	public class BBButton
	{
		#region Member Variables
		private BBSprite backgroundSprite;
		private BBSprite foregroundSprite;
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private Label label;
		#endregion

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(string backgroundSprite, string foregroundSprite, string buttonText, int xPos, int yPos, int width, int height, SpriteManager spriteManager, Random r, out string reason)
		{
			this.backgroundSprite = spriteManager.GetSprite(backgroundSprite, r);
			this.foregroundSprite = spriteManager.GetSprite(foregroundSprite, r);
			if (backgroundSprite == null || foregroundSprite == null)
			{
				reason = string.Format("One of those sprites does not exist in sprites.xml: \"{0}\" or \"{1}\"", backgroundSprite, foregroundSprite);
				return false;
			}
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;

			label = new Label(buttonText, xPos + 2, yPos + 2);

			Reset();
			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Reset()
		{
			pulse = 0;
			direction = false;
			Active = true;
			pressed = false;
			Selected = false;
		}

		public void SetButtonText(string text)
		{
			label.SetText(text);
		}

		public void MoveButton(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.Move(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public bool UpdateHovering(int x, int y, float frameDeltaTime)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (pulse < 0.6f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime / 2;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime / 2;
						if (pulse < 0.6f)
						{
							direction = !direction;
							pulse = 0.6f;
						}
					}
				}
				return true;
			}
			else if (pulse > 0)
			{
				pulse -= frameDeltaTime * 2;
				if (pulse < 0)
				{
					pulse = 0;
				}
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Active)
			{
				if (pressed || Selected)
				{
					foregroundSprite.Draw(xPos, yPos, foregroundSprite.Width / width, foregroundSprite.Height / height);
				}
				else if (!Selected)
				{
					backgroundSprite.Draw(xPos, yPos, foregroundSprite.Width / width, foregroundSprite.Height / height);
					if (pulse > 0)
					{
						foregroundSprite.Draw(xPos, yPos, foregroundSprite.Width / width, foregroundSprite.Height / height, (byte)(255 * pulse));
					}
				}
			}
			else
			{
				backgroundSprite.Draw(xPos, yPos, foregroundSprite.Width / width, foregroundSprite.Height / height, System.Drawing.Color.Tan);
				if (Selected)
				{
					foregroundSprite.Draw(xPos, yPos, foregroundSprite.Width / width, foregroundSprite.Height / height, System.Drawing.Color.Tan);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}
		#endregion
	}

	class Button
	{
		#region Member Variables
		//Button drawing information
		private SpriteName backgroundSprite;
		private SpriteName foregroundSprite;
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private Label label;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public Button(SpriteName backgroundSprite, SpriteName foregroundSprite, string buttonText, int xPos, int yPos, int width, int height)
		{
			this.backgroundSprite = backgroundSprite;
			this.foregroundSprite = foregroundSprite;
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;

			label = new Label(buttonText, xPos + 2, yPos + 2);

			Reset();
		}
		#endregion

		#region Functions
		public void Reset()
		{
			pulse = 0;
			direction = false;
			Active = true;
			pressed = false;
			Selected = false;
		}

		public void SetButtonText(string text)
		{
			label.SetText(text);
		}

		public void MoveButton(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.Move(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public bool UpdateHovering(int x, int y, float frameDeltaTime)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (pulse < 0.6f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime / 2;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime / 2;
						if (pulse < 0.6f)
						{
							direction = !direction;
							pulse = 0.6f;
						}
					}
				}
				return true;
			}
			else if (pulse > 0)
			{
				pulse -= frameDeltaTime * 2;
				if (pulse < 0)
				{
					pulse = 0;
				}
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw(DrawingManagement drawingManagement)
		{
			if (Active)
			{
				if (pressed)
				{
					drawingManagement.DrawSprite(foregroundSprite, xPos, yPos, 255, width, height, System.Drawing.Color.White);
				}
				else if (!Selected)
				{
					drawingManagement.DrawSprite(backgroundSprite, xPos, yPos, 255, width, height, System.Drawing.Color.White);
					if (pulse > 0)
					{
						drawingManagement.DrawSprite(foregroundSprite, xPos, yPos, (byte)(255 * pulse), width, height, System.Drawing.Color.White);
					}
				}
				else
				{
					drawingManagement.DrawSprite(foregroundSprite, xPos, yPos, 255, width, height, System.Drawing.Color.White);
				}
			}
			else
			{
				drawingManagement.DrawSprite(backgroundSprite, xPos, yPos, 255, width, height, System.Drawing.Color.Tan);
				if (Selected)
				{
					drawingManagement.DrawSprite(foregroundSprite, xPos, yPos, 255, width, height, System.Drawing.Color.Tan);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}
		#endregion
	}

	public class BBUniStretchButton
	{
		#region Member Variables
		private BBUniStretchableImage backgroundImage;
		private BBUniStretchableImage foregroundImage;
		private Label label;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;

		private int xPos;
		private int yPos;
		private int width;
		private int height;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(List<string> backgroundSections, List<string> foregroundSections, bool isHorizontal, string buttonText, int xPos, int yPos, int width, int height, SpriteManager spriteManager, Random r, out string reason)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;

			backgroundImage = new BBUniStretchableImage();
			foregroundImage = new BBUniStretchableImage();

			if (!backgroundImage.Initialize(xPos, yPos, width, height, 7, 2, isHorizontal, backgroundSections, spriteManager, r, out reason))
			{
				return false;
			}
			if (!foregroundImage.Initialize(xPos, yPos, width, height, 7, 2, isHorizontal, foregroundSections, spriteManager, r, out reason))
			{
				return false;
			}

			label = new Label(0, 0);
			SetButtonText(buttonText);
			label.SetColor(System.Drawing.Color.DarkBlue);

			Reset();

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Reset()
		{
			pulse = 0;
			direction = false;
			Active = true;
			pressed = false;
			Selected = false;
		}

		public void SetButtonText(string text)
		{
			label.SetText(text);
			label.Move((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
		}

		public void MoveTo(int x, int y)
		{
			this.xPos = x;
			this.yPos = y;
			label.Move((int)((width / 2) - (label.GetWidth() / 2) + x), (int)((height / 2) - (label.GetHeight() / 2) + y));
			backgroundImage.MoveTo(x, y);
			foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
			backgroundImage.Resize(width, height);
			foregroundImage.Resize(width, height);
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (pulse < 0.6f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime / 2;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime / 2;
						if (pulse < 0.6f)
						{
							direction = !direction;
							pulse = 0.6f;
						}
					}
				}
				return true;
			}
			if (pulse > 0)
			{
				pulse -= frameDeltaTime * 2;
				if (pulse < 0)
				{
					pulse = 0;
				}
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Active)
			{
				if (pressed || Selected)
				{
					foregroundImage.Draw();
				}
				else if (!Selected)
				{
					backgroundImage.Draw();
					if (pulse > 0)
					{
						foregroundImage.Draw((byte)(255 * pulse));
					}
				}
			}
			else
			{
				backgroundImage.Draw(System.Drawing.Color.Tan, 255);
				if (Selected)
				{
					foregroundImage.Draw(System.Drawing.Color.Tan, 255);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}
		#endregion
	}

	class ComboBox
	{
		#region Member Variables
		//ComboBox drawing information
		private SpriteName downArrowSprite;
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private List<string> items;
		private List<Button> buttons;
		private ScrollBar RealScrollBar;

		//ComboBox state information
		private bool dropped;
		private bool haveScroll;
		private int selectedIndex;
		
		#endregion

		#region Properties
		public bool Active { get; set; }
		public int SelectedIndex 
		{ 
			get { return selectedIndex; }
			set { selectedIndex = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprites">0 - bkgnd Button, 1 - Foregnd Button, 2 - bkgnd up, 3 - foregnd up, 4 - scrollbar, 5 - bkgnd down, 6 - foregnd down, 7 - bkgnd scroll, 8 - foregnd scroll</param>
		/// <param name="items"></param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="maxVisible"></param>
		public ComboBox(List<SpriteName> sprites, List<string> items, int xPos, int yPos, int width, int height, int maxVisible)
		{
			this.items = items;
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;

			dropped = false;
			downArrowSprite = sprites[5];

			if (items.Count < maxVisible)
			{
				maxVisible = items.Count;
			}
			else if (items.Count > maxVisible)
			{
				haveScroll = true;
			}

			Active = true;

			buttons = new List<Button>();
			for (int i = 0; i <= maxVisible; i++)
			{
				Button button = new Button(sprites[0], sprites[1], string.Empty, xPos, yPos + (i * height), width, height);
				buttons.Add(button);
			}
			RealScrollBar = new ScrollBar(xPos + width, yPos + height, 16, (height * maxVisible) - 32, maxVisible, items.Count, false, false, sprites[2], sprites[3], sprites[5], sprites[6], sprites[7], sprites[8], sprites[4], sprites[4]);
		}
		#endregion

		#region Functions
		public void MoveComboBox(int x, int y)
		{
			xPos = x;
			yPos = y;

			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].MoveButton(x, y + (i * height));
			}

			RealScrollBar.MoveScrollBar(xPos + width, yPos + height);
		}

		public void Draw(DrawingManagement drawingManagement)
		{
			if (items.Count <= 0)
			{
				buttons[0].Active = false;
				buttons[0].SetButtonText(string.Empty);
			}
			else
			{
				buttons[0].Active = Active;
				buttons[0].SetButtonText(items[selectedIndex]);
			}
			buttons[0].Draw(drawingManagement);
			drawingManagement.DrawSprite(downArrowSprite, xPos + width - 20, yPos + 4, 255, System.Drawing.Color.White);
			if (dropped)
			{
				for (int i = 0; i < buttons.Count - 1; i++)
				{
					buttons[i + 1].SetButtonText(items[RealScrollBar.TopIndex + i]);
					buttons[i + 1].Draw(drawingManagement);
				}
				if (haveScroll)
				{
					RealScrollBar.DrawScrollBar(drawingManagement);
				}
			}
		}

		public void UpdateHovering(int x, int y, float frameDeltaTime)
		{
			if (Active)
			{
				if (!dropped)
				{
					buttons[0].UpdateHovering(x, y, frameDeltaTime);
				}
				else
				{
					foreach (Button button in buttons)
					{
						button.UpdateHovering(x, y, frameDeltaTime);
					}
					RealScrollBar.UpdateHovering(x, y, frameDeltaTime);
				}
			}
		}

		public bool MouseDown(int x, int y)
		{
			if (Active)
			{
				if (!dropped)
				{
					return buttons[0].MouseDown(x, y);
				}
				else
				{
					for (int i = 0; i < buttons.Count; i++)
					{
						if (buttons[i].MouseDown(x, y))
						{
							return true;
						}
					}
					return RealScrollBar.MouseDown(x, y);
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Active)
			{
				if (!dropped)
				{
					if (buttons[0].MouseUp(x, y))
					{
						dropped = true;
						return true;
					}
				}
				else
				{
					for (int i = 0; i < buttons.Count; i++)
					{
						if (buttons[i].MouseUp(x, y))
						{
							if (i > 0)
							{
								selectedIndex = i + RealScrollBar.TopIndex - 1;
							}
							dropped = false;
							return true;
						}
					}
					if (RealScrollBar.MouseUp(x, y))
					{
						return true;
					}
				}
			}
			dropped = false;
			return false;
		}
		#endregion
	}

	class BBScrollBar
	{
		#region Member Variables
		//Variables that are defined in constructor
		private int xPos;
		private int yPos;
		private BBButton Up;
		private BBButton Down;
		private BBUniStretchButton Scroll;
		private BBSprite scrollBar;
		private BBSprite highlightedScrollBar;
		private int amountOfItems;
		private int amountVisible;
		private int scrollBarLength;

		private int topIndex; //Which topmost item is visible
		private int scrollPos;
		private bool scrollSelected; //is the scroll button selected? if so, drag it
		private int initialMousePos;
		private int initialScrollPos;
		private bool isHorizontal;
		private bool isSlider; //Is the scroll bar's behavior like a scroll bar or a slider?
		private bool isEnabled;

		//Variables that are calculated from the values passed into the constructor
		private int scrollButtonLength;

		#endregion

		#region Properties
		public int TopIndex
		{
			get { return topIndex; }
			set
			{
				topIndex = value;
				SetScrollButtonPosition();
			}
		}
		#endregion

		#region Constructors
		public bool Initialize(int xPos, int yPos, int length, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, SpriteManager spriteManager, Random r, out string reason)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			Up = new BBButton();
			Down = new BBButton();
			Scroll = new BBUniStretchButton();
			
			this.amountOfItems = amountOfItems;
			this.amountVisible = amountOfVisibleItems;
			this.isSlider = isSlider;
			this.isHorizontal = isHorizontal;

			scrollBarLength = length - 32;

			if (!isSlider)
			{
				scrollButtonLength = (int)(((float)amountOfVisibleItems / amountOfItems) * scrollBarLength);
				if (!isHorizontal)
				{
					if (!Up.Initialize("ScrollUpBGButton", "ScrollUpFGButton", "", xPos, yPos, 16, 16, spriteManager, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollDownBGButton", "ScrollDownFGButton", "", xPos, yPos + length - 32, 16, 16, spriteManager, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollVerticalBGButton1", "ScrollVerticalBGButton2", "ScrollVerticalBGButton3" }, 
										   new List<string> { "ScrollVerticalFGButton1", "ScrollVerticalFGButton2", "ScrollVerticalFGButton3" },
										   false, "", xPos, yPos + 16, 16, scrollButtonLength, spriteManager, r, out reason))
					{
						return false;
					}
					scrollBar = spriteManager.GetSprite("ScrollVerticalBar", r);
					if (scrollBar == null)
					{
						reason = "\"ScrollVerticalBar\" sprite does not exist";
						return false;
					}
				}
				else
				{
					if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", xPos, yPos, 16, 16, spriteManager, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", xPos + length - 32, yPos, 16, 16, spriteManager, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollHorizontalBGButton1", "ScrollHorizontalBGButton2", "ScrollHorizontalBGButton3" },
										   new List<string> { "ScrollHorizontalFGButton1", "ScrollHorizontalFGButton2", "ScrollHorizontalFGButton3" },
										   false, "", xPos + 16, yPos, 16, scrollButtonLength, spriteManager, r, out reason))
					{
						return false;
					}
					scrollBar = spriteManager.GetSprite("ScrollHorizontalBar", r);
					if (scrollBar == null)
					{
						reason = "\"ScrollHorizontalBar\" sprite does not exist";
						return false;
					}
				}
			}
			else
			{
				scrollButtonLength = 16;
				if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", xPos, yPos, 16, 16, spriteManager, r, out reason))
				{
					return false;
				}
				if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", xPos + length - 32, yPos, 16, 16, spriteManager, r, out reason))
				{
					return false;
				}
				if (!Scroll.Initialize(new List<string> { "SliderHorizontalBGButton1", "SliderHorizontalBGButton2", "SliderHorizontalBGButton3" },
									   new List<string> { "SliderHorizontalFGButton1", "SliderHorizontalFGButton2", "SliderHorizontalFGButton3" },
									   false, "", xPos + 16, yPos, 16, scrollButtonLength, spriteManager, r, out reason))
				{
					return false;
				}
				scrollBar = spriteManager.GetSprite("SliderBGBar", r);
				if (scrollBar == null)
				{
					reason = "\"SliderBGBar\" sprite does not exist";
					return false;
				}
				highlightedScrollBar = spriteManager.GetSprite("SliderFGBar", r);
				if (highlightedScrollBar == null)
				{
					reason = "\"SliderFGBar\" sprite does not exist";
					return false;
				}
			}

			topIndex = 0;
			scrollPos = 0; //relative to the scrollbar itself
			scrollSelected = false;
			isEnabled = true;
			reason = null;
			return true;
		}
		#endregion

		#region Private Functions
		private void SetScrollButtonPosition()
		{
			scrollPos = (int)(((float)topIndex / (amountOfItems - amountVisible)) * (scrollBarLength - scrollButtonLength));
			if (scrollPos < 0)
			{
				scrollPos = 0;
			}
			else if (scrollPos > (scrollBarLength - scrollButtonLength))
			{
				scrollPos = scrollBarLength - scrollButtonLength;
			}
			if (isHorizontal)
			{
				Scroll.MoveTo(xPos + 16 + scrollPos, yPos);
			}
			else
			{
				Scroll.MoveTo(xPos, yPos + 16 + scrollPos);
			}
		}
		#endregion

		#region Public Functions
		public void Draw()
		{
			System.Drawing.Color enabledColor = isEnabled ? System.Drawing.Color.White : System.Drawing.Color.Tan;
			if (!isSlider)
			{
				if (!isHorizontal)
				{
					scrollBar.Draw(xPos, yPos + 16, 1, scrollBarLength / scrollBar.Height, enabledColor);
				}
				else
				{
					scrollBar.Draw(xPos + 16, yPos, scrollBarLength / scrollBar.Width, 1, enabledColor);
				}
			}
			else
			{
				scrollBar.Draw(xPos + 16, yPos, scrollBarLength / scrollBar.Width, 1, enabledColor);
				highlightedScrollBar.Draw(xPos + 16, yPos, scrollPos / highlightedScrollBar.Width, 1, enabledColor);
			}
			Up.Draw();
			Down.Draw();
			Scroll.Draw();
		}

		public bool MouseDown(int x, int y)
		{
			if (isEnabled)
			{
				if (Up.MouseDown(x, y))
				{
					return true;
				}
				if (Down.MouseDown(x, y))
				{
					return true;
				}
				if (!isSlider && Scroll.MouseDown(x, y))
				{
					scrollSelected = true;
					if (isHorizontal)
					{
						initialMousePos = x;
					}
					else
					{
						initialMousePos = y;
					}
					initialScrollPos = scrollPos;
					return true;
				}
				//at this point, only the scroll bar itself is left
				if ((isHorizontal && (x >= xPos + 16 && x < xPos + 16 + scrollBarLength && yPos <= y && y < yPos + 16))
					|| (!isHorizontal && (x >= xPos && x < xPos + 16 && yPos + 16 <= y && y < yPos + 16 + scrollBarLength)))
				{
					if (!isSlider)
					{
						//clicked on the bar itself, jump up one page
						if ((!isHorizontal && y < yPos + 16 + scrollPos) || (isHorizontal && x < xPos + 16 + scrollPos))
						{
							topIndex -= amountVisible;
							if (topIndex < 0)
							{
								topIndex = 0;
							}
						}
						else
						{
							//since up is checked already, jump down one page
							topIndex += amountVisible;
							if (topIndex > (amountOfItems - amountVisible))
							{
								topIndex = (amountOfItems - amountVisible);
							}
						}
						SetScrollButtonPosition();
					}
					else
					{
						scrollSelected = true;
					}
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (isEnabled)
			{
				bool changed = false;
				if (Up.MouseUp(x, y))
				{
					if (TopIndex > 0)
					{
						TopIndex--;
					}
					changed = true;
				}
				else if (Down.MouseUp(x, y))
				{
					if (TopIndex < amountOfItems - amountVisible)
					{
						TopIndex++;
					}
					changed = true;
				}
				if (changed || scrollSelected)
				{
					scrollSelected = false;
					if (!isSlider)
					{
						SetScrollButtonPosition();
						Scroll.MouseUp(x, y);
					}
					return true;
				}
				if (x >= xPos && x < xPos + 16 + (isHorizontal ? scrollBarLength : 0) && yPos + 16 <= y && y < yPos + 16 + (isHorizontal ? 0 : scrollBarLength))
				{
					return true;
				}
			}
			return false;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (isEnabled)
			{
				Scroll.MouseHover(x, y, frameDeltaTime);
				if (scrollSelected)
				{
					int newPosition = 0;
					if (isHorizontal)
					{
						newPosition = initialScrollPos + (x - (isSlider ? (xPos + 16 + (scrollButtonLength / 2)) : initialMousePos));
					}
					else
					{
						newPosition = initialScrollPos + (y - (isSlider ? (yPos + 16 + (scrollButtonLength / 2)) : initialMousePos));
					}
					if (newPosition < 0)
					{
						newPosition = 0;
					}
					else if (newPosition > (scrollBarLength - scrollButtonLength))
					{
						newPosition = scrollBarLength - scrollButtonLength;
					}
					float itemsPerIncrement = ((float)(amountOfItems - amountVisible) / (float)(scrollBarLength - scrollButtonLength));
					int oldIndex = topIndex;
					topIndex = (int)((itemsPerIncrement * newPosition) + 0.5f);
					SetScrollButtonPosition();
					return !(oldIndex == topIndex);
				}
				else
				{
					Up.UpdateHovering(x, y, frameDeltaTime);
					Down.UpdateHovering(x, y, frameDeltaTime);
					return false;
				}
			}
			return false;
		}

		public void MoveScrollBar(int x, int y)
		{
			Up.MoveButton(x, y);
			if (isHorizontal)
			{
				Down.MoveButton(x + scrollBarLength + 16, y);
				Scroll.MoveTo(x + 16 + scrollPos, y);
			}
			else
			{
				Down.MoveButton(x, y + scrollBarLength + 16);
				Scroll.MoveTo(x, y + 16 + scrollPos);
			}
			xPos = x;
			yPos = y;
		}

		public void SetAmountOfItems(int amount)
		{
			topIndex = 0;
			amountOfItems = amount;
			if (!isSlider)
			{
				scrollButtonLength = (int)(((float)amountVisible / amountOfItems) * scrollBarLength);
				if (scrollButtonLength < 8)
				{
					scrollButtonLength = 8;
				}
				if (isHorizontal)
				{
					Scroll.ResizeButton(scrollButtonLength, 16);
				}
				else
				{
					Scroll.ResizeButton(16, scrollButtonLength);
				}
			}
			SetScrollButtonPosition();
		}

		public void SetEnabledState(bool enabled)
		{
			Up.Active = enabled;
			Down.Active = enabled;
			Scroll.Active = enabled;
			isEnabled = enabled;
		}
		#endregion
	}

	class ScrollBar
	{
		#region Member Variables
		//Variables that are defined in constructor
		private int xPos;
		private int yPos;
		private int scrollSize;
		private Button Up;
		private Button Down;
		private Button Scroll;
		private SpriteName scrollBar;
		private SpriteName highlightedScrollBar;
		private int amountOfItems;
		private int amountVisible;
		private int scrollBarLength;

		private int topIndex; //Which topmost item is visible
		private int scrollPos;
		private bool scrollSelected; //is the scroll button selected? if so, drag it
		private int initialMousePos;
		private int initialScrollPos;
		private bool isHorizontal;
		private bool isSlider; //Is the scroll bar's behavior like a scroll bar or a slider?
		private bool isEnabled;

		//Variables that are calculated from the values passed into the constructor
		private int scrollButtonLength;
		
		#endregion

		#region Properties
		public int TopIndex
		{
			get { return topIndex; }
			set
			{
				topIndex = value;
				SetScrollButtonPosition();
			}
		}
		#endregion

		#region Constructors
		public ScrollBar(int xPos, int yPos, int scrollSize, int scrollBarLength, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, SpriteName UpBackground, SpriteName UpForeGround, SpriteName DownBackGround, SpriteName DownForeground, SpriteName ScrollBackground, SpriteName ScrollForeground, SpriteName ScrollBar, SpriteName HighlightedScrollBar)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			this.scrollSize = scrollSize;
			this.scrollBarLength = scrollBarLength;
			this.amountOfItems = amountOfItems;
			this.amountVisible = amountOfVisibleItems;
			this.isSlider = isSlider;
			this.isHorizontal = isHorizontal;

			Up = new Button(UpBackground, UpForeGround, "", xPos, yPos, scrollSize, scrollSize);

			if (isSlider)
			{
				scrollButtonLength = scrollSize;
			}
			else
			{
				scrollButtonLength = (int)(((float)amountOfVisibleItems / amountOfItems) * scrollBarLength);
				if (scrollButtonLength < 8)
				{
					scrollButtonLength = 8;
				}
			}
			this.scrollBar = ScrollBar;
			this.highlightedScrollBar = HighlightedScrollBar;

			if (isHorizontal)
			{
				Scroll = new Button(ScrollBackground, ScrollForeground, "", xPos + scrollSize, yPos, scrollButtonLength, scrollSize);
				Down = new Button(DownBackGround, DownForeground, "", xPos + scrollBarLength + scrollSize, yPos, scrollSize, scrollSize);
			}
			else
			{
				Scroll = new Button(ScrollBackground, ScrollForeground, "", xPos, yPos + scrollSize, scrollSize, scrollButtonLength);
				Down = new Button(DownBackGround, DownForeground, "", xPos, yPos + scrollBarLength + scrollSize, scrollSize, scrollSize);
			}

			topIndex = 0;
			scrollPos = 0; //relative to the scrollbar itself
			scrollSelected = false;
			isEnabled = true;
		}
		#endregion

		#region Private Functions
		private void SetScrollButtonPosition()
		{
			scrollPos = (int)(((float)topIndex / (amountOfItems - amountVisible)) * (scrollBarLength - scrollButtonLength));
			if (scrollPos < 0)
			{
				scrollPos = 0;
			}
			else if (scrollPos > (scrollBarLength - scrollButtonLength))
			{
				scrollPos = scrollBarLength - scrollButtonLength;
			}
			if (isHorizontal)
			{
				Scroll.MoveButton(xPos + scrollSize + scrollPos, yPos);
			}
			else
			{
				Scroll.MoveButton(xPos, yPos + scrollSize + scrollPos);
			}
		}
		#endregion

		#region Public Functions
		public void DrawScrollBar(DrawingManagement drawingManagement)
		{
			System.Drawing.Color enabledColor = isEnabled ? System.Drawing.Color.White : System.Drawing.Color.Tan;
			if (isHorizontal)
			{
				if (!isSlider)
				{
					drawingManagement.DrawSprite(scrollBar, xPos + scrollSize, yPos, 255, scrollBarLength, scrollSize, enabledColor);
				}
				else
				{
					drawingManagement.DrawSprite(highlightedScrollBar, xPos + scrollSize, yPos, 255, scrollPos + (scrollButtonLength / 2), scrollSize, enabledColor);
					drawingManagement.DrawSprite(scrollBar, xPos + scrollSize + scrollPos + (scrollButtonLength / 2), yPos, 255, scrollBarLength - (scrollPos + (scrollButtonLength / 2)), scrollSize, enabledColor);
				}
			}
			else
			{
				if (!isSlider)
				{
					drawingManagement.DrawSprite(scrollBar, xPos, yPos + scrollSize, 255, scrollSize, scrollBarLength, enabledColor);
				}
				else
				{
					drawingManagement.DrawSprite(scrollBar, xPos, yPos + scrollSize, 255, scrollSize, scrollPos + (scrollButtonLength / 2), enabledColor);
					drawingManagement.DrawSprite(highlightedScrollBar, xPos, yPos + scrollSize + scrollPos + (scrollButtonLength / 2), 255, scrollSize, scrollBarLength - (scrollPos + (scrollButtonLength / 2)), enabledColor);
				}
			}
			Up.Draw(drawingManagement);
			Down.Draw(drawingManagement);
			Scroll.Draw(drawingManagement);
		}

		public bool MouseDown(int x, int y)
		{
			if (isEnabled)
			{
				if (Up.MouseDown(x, y))
				{
					return true;
				}
				if (Down.MouseDown(x, y))
				{
					return true;
				}
				if (!isSlider && Scroll.MouseDown(x, y))
				{
					scrollSelected = true;
					if (isHorizontal)
					{
						initialMousePos = x;
					}
					else
					{
						initialMousePos = y;
					}
					initialScrollPos = scrollPos;
					return true;
				}
				//at this point, only the scroll bar itself is left
				if ((isHorizontal && (x >= xPos + scrollSize && x < xPos + scrollSize + scrollBarLength && yPos <= y && y < yPos + scrollSize))
					|| (!isHorizontal && (x >= xPos && x < xPos + scrollSize && yPos + scrollSize <= y && y < yPos + scrollSize + scrollBarLength)))
				{
					if (!isSlider)
					{
						//clicked on the bar itself, jump up one page
						if ((!isHorizontal && y < yPos + scrollSize + scrollPos) || (isHorizontal && x < xPos + scrollSize + scrollPos))
						{
							topIndex -= amountVisible;
							if (topIndex < 0)
							{
								topIndex = 0;
							}
						}
						else
						{
							//since up is checked already, jump down one page
							topIndex += amountVisible;
							if (topIndex > (amountOfItems - amountVisible))
							{
								topIndex = (amountOfItems - amountVisible);
							}
						}
						SetScrollButtonPosition();
					}
					else
					{
						scrollSelected = true;
					}
					return true;
				}
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (isEnabled)
			{
				bool changed = false;
				if (Up.MouseUp(x, y))
				{
					if (TopIndex > 0)
					{
						TopIndex--;
					}
					changed = true;
				}
				else if (Down.MouseUp(x, y))
				{
					if (TopIndex < amountOfItems - amountVisible)
					{
						TopIndex++;
					}
					changed = true;
				}
				if (changed || scrollSelected)
				{
					scrollSelected = false;
					if (!isSlider)
					{
						SetScrollButtonPosition();
						Scroll.MouseUp(x, y);
					}
					return true;
				}
				if (x >= xPos && x < xPos + scrollSize + (isHorizontal ? scrollBarLength : 0) && yPos + scrollSize <= y && y < yPos + scrollSize + (isHorizontal ? 0 : scrollBarLength))
				{
					return true;
				}
			}
			return false;
		}

		public bool UpdateHovering(int x, int y, float frameDeltaTime)
		{
			if (isEnabled)
			{
				Scroll.UpdateHovering(x, y, frameDeltaTime);
				if (scrollSelected)
				{
					int newPosition = 0;
					if (isHorizontal)
					{
						newPosition = initialScrollPos + (x - (isSlider ? (xPos + scrollSize + (scrollButtonLength / 2)) : initialMousePos));
					}
					else
					{
						newPosition = initialScrollPos + (y - (isSlider ? (yPos + scrollSize + (scrollButtonLength / 2)) : initialMousePos));
					}
					if (newPosition < 0)
					{
						newPosition = 0;
					}
					else if (newPosition > (scrollBarLength - scrollButtonLength))
					{
						newPosition = scrollBarLength - scrollButtonLength;
					}
					float itemsPerIncrement = ((float)(amountOfItems - amountVisible) / (float)(scrollBarLength - scrollButtonLength));
					int oldIndex = topIndex;
					topIndex = (int)((itemsPerIncrement * newPosition) + 0.5f);
					SetScrollButtonPosition();
					return !(oldIndex == topIndex);
				}
				else
				{
					Up.UpdateHovering(x, y, frameDeltaTime);
					Down.UpdateHovering(x, y, frameDeltaTime);
					return false;
				}
			}
			return false;
		}

		public void MoveScrollBar(int x, int y)
		{
			Up.MoveButton(x, y);
			if (isHorizontal)
			{
				Down.MoveButton(x + scrollBarLength + scrollSize, y);
				Scroll.MoveButton(x + scrollSize + scrollPos, y);
			}
			else
			{
				Down.MoveButton(x, y + scrollBarLength + scrollSize);
				Scroll.MoveButton(x, y + scrollSize + scrollPos);
			}
			xPos = x;
			yPos = y;
		}

		public void SetAmountOfItems(int amount)
		{
			topIndex = 0;
			amountOfItems = amount;
			if (!isSlider)
			{
				scrollButtonLength = (int)(((float)amountVisible / amountOfItems) * scrollBarLength);
				if (scrollButtonLength < 8)
				{
					scrollButtonLength = 8;
				}
				if (isHorizontal)
				{
					Scroll.ResizeButton(scrollButtonLength, scrollSize);
				}
				else
				{
					Scroll.ResizeButton(scrollSize, scrollButtonLength);
				}
			}
			SetScrollButtonPosition();
		}

		public void SetEnabledState(bool enabled)
		{
			Up.Active = enabled;
			Down.Active = enabled;
			Scroll.Active = enabled;
			isEnabled = enabled;
		}
		#endregion
	}

	class ProgressBar
	{
		#region Member Variables
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private int maxItems;
		private int currentItems;
		private SpriteName barBackground;
		private SpriteName barForeground;
		private int currentWidth;
		private int potentialWidth;
		private System.Drawing.Color color;
		private int potentinalIncrease;
		private System.Drawing.Color potentialIncreaseColor;
		#endregion

		#region Constructor
		public ProgressBar(int xPos, int yPos, int width, int height, int maxItems, int currentItems, SpriteName barBackground, SpriteName barForeground)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;
			this.maxItems = maxItems;
			this.currentItems = currentItems;

			this.barBackground = barBackground;
			this.barForeground = barForeground;
			color = System.Drawing.Color.White;
			potentinalIncrease = -1;
		}
		public ProgressBar(int xPos, int yPos, int width, int height, int maxItems, int currentItems, SpriteName barBackground, SpriteName barForeground, System.Drawing.Color potentialColor) : 
			this(xPos, yPos, width, height, maxItems, currentItems, barBackground, barForeground)
		{
			potentialIncreaseColor = potentialColor;
		}
		#endregion

		#region Functions
		public void Move(int xPos, int yPos)
		{
			this.xPos = xPos;
			this.yPos = yPos;
		}

		private void UpdateWidth()
		{
			currentWidth = (int)(width * ((float)currentItems / (float)maxItems));
			if (currentWidth > width)
			{
				//in case we went over
				currentWidth = width;
			}
			if (currentWidth < 0)
			{
				//Don't want negative progress extending to left
				currentWidth = 0;
			}
			if (potentinalIncrease > 0)
			{
				potentialWidth = (int)(width * ((float)potentinalIncrease / (float)maxItems));
				if (currentWidth + potentialWidth > width)
				{
					potentialWidth = (width - currentWidth);
				}
			}
		}

		public void IncrementProgress()
		{
			currentItems++;
			UpdateWidth();
		}

		public void SetProgress(int currentItems)
		{
			this.currentItems = currentItems;
			UpdateWidth();
		}

		public void SetPotentialProgress(int potentialAmount)
		{
			potentinalIncrease = potentialAmount;
			UpdateWidth();
		}

		public void SetMaxProgress(int maxItems)
		{
			this.maxItems = maxItems;
			UpdateWidth();
		}

		public void Draw(DrawingManagement drawingManagement)
		{
			drawingManagement.DrawSprite(barBackground, xPos, yPos, 255, width, height, System.Drawing.Color.White);
			drawingManagement.DrawSprite(barForeground, xPos, yPos, 255, currentWidth, height, color);
			if (potentialWidth > 0)
			{
				drawingManagement.DrawSprite(barForeground, xPos + currentWidth, yPos, 255, potentialWidth, height, potentialIncreaseColor);
			}
		}

		public void SetColor(System.Drawing.Color color)
		{
			this.color = color;
		}
		#endregion
	}

	public enum StretchableImageType
	{
		ThickBorder,
		MediumBorder,
		ThinBorder,
		TextBox
	}
	public class BBStretchableImage
	{
		#region Member Variables
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private int sectionWidth;
		private int sectionHeight;
		private int horizontalStretchLength;
		private int verticalStretchLength;
		private List<BBSprite> sections;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, int width, int height, StretchableImageType type, SpriteManager spriteManager, Random r, out string reason)
		{
			xPos = x;
			yPos = y;

			switch (type)
			{
				case StretchableImageType.TextBox:
					{
						sectionWidth = 30;
						sectionHeight = 13;
						sections = new List<BBSprite>();
						var tempSprite = spriteManager.GetSprite("TextTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("TextBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					}
					break;
				case StretchableImageType.ThinBorder:
				{
					sectionWidth = 30;
					sectionHeight = 13;
					sections = new List<BBSprite>();
					var tempSprite = spriteManager.GetSprite("ThinBorderBGTL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGTC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGTR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGCL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGCC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGCR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGBL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGBC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = spriteManager.GetSprite("ThinBorderBGBR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
				} break;
				case StretchableImageType.MediumBorder:
					{
						sectionWidth = 60;
						sectionHeight = 60;
						sections = new List<BBSprite>();
						var tempSprite = spriteManager.GetSprite("MediumBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("MediumBorderBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.ThickBorder:
					{
						sectionWidth = 200;
						sectionHeight = 200;
						sections = new List<BBSprite>();
						var tempSprite = spriteManager.GetSprite("ThickBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = spriteManager.GetSprite("ThickBorderBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					}
					break;
			}
			
			horizontalStretchLength = (width - (sectionWidth * 2));
			verticalStretchLength = (height - (sectionHeight * 2));

			if (horizontalStretchLength < 0)
			{
				horizontalStretchLength = 0;
			}
			if (verticalStretchLength < 0)
			{
				verticalStretchLength = 0;
			}

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Draw()
		{
			Draw(System.Drawing.Color.White, 255);
		}

		public void Draw(byte alpha)
		{
			Draw(System.Drawing.Color.White, alpha);
		}

		public void Draw(System.Drawing.Color color, byte alpha)
		{
			sections[0].Draw(xPos, yPos, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[1].Draw(xPos + sectionWidth, yPos, horizontalStretchLength / sections[1].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[2].Draw(xPos + sectionWidth + horizontalStretchLength, yPos, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[3].Draw(xPos, yPos + sectionHeight, 1, verticalStretchLength / sections[3].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[4].Draw(xPos + sectionWidth, yPos + sectionHeight, horizontalStretchLength / sections[4].Width, verticalStretchLength / sections[4].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[5].Draw(xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight, 1, verticalStretchLength / sections[5].Height, System.Drawing.Color.FromArgb(alpha, color));
			sections[6].Draw(xPos, yPos + sectionHeight + verticalStretchLength, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[7].Draw(xPos + sectionWidth, yPos + sectionHeight + verticalStretchLength, horizontalStretchLength / sections[7].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
			sections[8].Draw(xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight + verticalStretchLength, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
		}
		public void SetDimensions(int width, int height)
		{
			this.width = width;
			this.height = height;
			horizontalStretchLength = (width - (sectionWidth * 2));
			verticalStretchLength = (height - (sectionHeight * 2));
		}
		public void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
		}
		#endregion
	}

	public class BBUniStretchableImage
	{
		#region Member Variables
		private int x;
		private int y;
		private int width;
		private int height;
		private int mainLength;
		private int stretchLength;
		private int stretchAmount;
		private List<BBSprite> sections;
		private bool isHorizontal;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, int width, int height, int mainLength, int stretchLength, bool isHorizontal, List<string> sections, SpriteManager spriteManager, Random r, out string reason)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.mainLength = mainLength;
			this.stretchLength = stretchLength;
			this.sections = new List<BBSprite>();
			foreach (string spriteName in sections)
			{
				BBSprite sprite = spriteManager.GetSprite(spriteName, r);
				if (sprite == null)
				{
					reason = string.Format("Can't find sprite \"{0}\".", spriteName);
					return false;
				}
				this.sections.Add(sprite);
			}
			this.isHorizontal = isHorizontal;
			if (isHorizontal)
			{
				stretchAmount = (width - (mainLength * 2));
			}
			else
			{
				stretchAmount = (height - (mainLength * 2));
			}

			if (stretchAmount < 0)
			{
				stretchAmount = 0;
			}

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Draw()
		{
			Draw(System.Drawing.Color.White, 255);
		}

		public void Draw(byte alpha)
		{
			Draw(System.Drawing.Color.White, alpha);
		}

		public void Draw(System.Drawing.Color color, byte alpha)
		{
			sections[0].Draw(x, y, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			if (isHorizontal)
			{
				sections[1].Draw(x + mainLength, y, stretchAmount / sections[1].Width, 1, System.Drawing.Color.FromArgb(alpha, color));
				sections[2].Draw(x + mainLength + stretchAmount, y, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			}
			else
			{
				sections[1].Draw(x, y + mainLength, 1, stretchAmount / sections[1].Height, System.Drawing.Color.FromArgb(alpha, color));
				sections[2].Draw(x, y + mainLength + stretchAmount, 1, 1, System.Drawing.Color.FromArgb(alpha, color));
			}
		}

		public void Resize(int width, int height)
		{
			this.width = width;
			this.height = height;
			if (isHorizontal)
			{
				stretchAmount = (width - (mainLength * 2));
			}
			else
			{
				stretchAmount = (height - (mainLength * 2));
			}

			if (stretchAmount < 0)
			{
				stretchAmount = 0;
			}
		}

		public void MoveTo(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion
	}

	class Label
	{
		#region Member Variables
		private string label;
		private int x;
		private int y;
		private bool isRightAligned;
		private GorgonLibrary.Graphics.TextSprite textSprite;
		private System.Drawing.Color color;
		#endregion

		#region Properties
		public string Text { get; private set; }
		#endregion

		#region Constructor
		public Label(int x, int y)
		{
			this.x = x;
			this.y = y;
			color = System.Drawing.Color.White;
		}
		public Label(string label, int x, int y)
		{
			this.x = x;
			this.y = y;
			color = System.Drawing.Color.White;
			SetText(label);
		}
		public Label(int x, int y, System.Drawing.Color color)
		{
			this.x = x;
			this.y = y;
			this.color = color;
		}
		public Label(string label, int x, int y, System.Drawing.Color color)
		{
			this.x = x;
			this.y = y;
			this.color = color;
			SetText(label);
		}
		#endregion

		#region Functions
		public void SetText(string label)
		{
			this.label = label;

			GorgonLibrary.Graphics.Font font;
			if (DrawingManagement.fonts.TryGetValue("Arial", out font))
			{
				textSprite = new GorgonLibrary.Graphics.TextSprite("Arial", label, font);
				SetAlignment(isRightAligned);
				Text = label;
			}
		}
		public void SetAlignment(bool isRightAligned)
		{
			this.isRightAligned = isRightAligned;
			if (textSprite != null)
			{
				if (isRightAligned)
				{
					textSprite.SetPosition(x - textSprite.Width, y);
				}
				else
				{
					textSprite.SetPosition(x, y);
				}
			}
		}

		public float GetWidth()
		{
			return textSprite.Width;
		}
		public float GetHeight()
		{
			return textSprite.Height;
		}
		public void Move(int x, int y)
		{
			this.x = x;
			this.y = y;
			textSprite.SetPosition(x, y);
		}
		public void SetColor(System.Drawing.Color color)
		{
			this.color = color;
		}
		public void Draw()
		{
			textSprite.Color = color;
			textSprite.Draw();
		}
		#endregion
	}

	class SingleLineTextBox
	{
		#region Member Variables
		private Label text;
		private string actualText;
		private int xPos;
		private int yPos;
		private int width;
		private int height;
		private SpriteName background;

		private bool isSelected;
		private bool pressed;
		private bool blink;
		private float timer;
		#endregion

		#region Constructors
		public SingleLineTextBox(int x, int y, int width, int height, SpriteName background)
		{
			xPos = x;
			yPos = y;
			this.width = width;
			this.height = height;
			this.background = background;
			actualText = string.Empty;
			text = new Label(string.Empty, x + 2, y + 2);
			pressed = false;
			isSelected = false;
			blink = true;
		}

		public SingleLineTextBox(string text, int x, int y, int width, int height, SpriteName background)
		{
			xPos = x;
			yPos = y;
			this.width = width;
			this.height = height;
			this.background = background;
			actualText = text;
			this.text = new Label(text, x + 2, y + 2);
			pressed = false;
			isSelected = false;
			blink = false;
		}
		#endregion

		#region Functions
		public bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				pressed = true;
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					blink = true;
					isSelected = true;
				}
				else
				{
					isSelected = false;
				}
			}
			else
			{
				isSelected = false;
			}
			if (!isSelected)
			{
				blink = false;
				text.SetText(actualText);
			}
			return isSelected;
		}

		public void Update(float frameDeltaTime)
		{
			if (isSelected)
			{
				timer += frameDeltaTime;
				if (timer >= 0.25f)
				{
					blink = !blink;
					text.SetText(actualText + (blink ? "|" : ""));
					timer -= 0.25f;
				}
			}
		}

		public void Draw(DrawingManagement drawingManagement)
		{
			drawingManagement.DrawSprite(background, xPos, yPos, 255, width, height, System.Drawing.Color.White);
			text.Draw();
		}

		public string GetString()
		{
			return actualText;
		}

		public void SetString(string text)
		{
			actualText = text;
			this.text.SetText(text);
		}

		#region Keys
		public bool KeyDown(KeyboardInputEventArgs e)
		{
			if (isSelected)
			{
				string prevText = actualText;
				switch (e.Key)
				{
					case KeyboardKeys.A: 
						actualText = actualText + (e.Shift ? "A" : "a");
						break;
					case KeyboardKeys.B:
						actualText = actualText + (e.Shift ? "B" : "b");
						break;
					case KeyboardKeys.C:
						actualText = actualText + (e.Shift ? "C" : "c");
						break;
					case KeyboardKeys.D:
						actualText = actualText + (e.Shift ? "D" : "d");
						break;
					case KeyboardKeys.E:
						actualText = actualText + (e.Shift ? "E" : "e");
						break;
					case KeyboardKeys.F:
						actualText = actualText + (e.Shift ? "F" : "f");
						break;
					case KeyboardKeys.G:
						actualText = actualText + (e.Shift ? "G" : "g");
						break;
					case KeyboardKeys.H:
						actualText = actualText + (e.Shift ? "H" : "h");
						break;
					case KeyboardKeys.I:
						actualText = actualText + (e.Shift ? "I" : "i");
						break;
					case KeyboardKeys.J:
						actualText = actualText + (e.Shift ? "J" : "j");
						break;
					case KeyboardKeys.K:
						actualText = actualText + (e.Shift ? "K" : "k");
						break;
					case KeyboardKeys.L:
						actualText = actualText + (e.Shift ? "L" : "l");
						break;
					case KeyboardKeys.M:
						actualText = actualText + (e.Shift ? "M" : "m");
						break;
					case KeyboardKeys.N:
						actualText = actualText + (e.Shift ? "N" : "n");
						break;
					case KeyboardKeys.O:
						actualText = actualText + (e.Shift ? "O" : "o");
						break;
					case KeyboardKeys.P:
						actualText = actualText + (e.Shift ? "P" : "p");
						break;
					case KeyboardKeys.Q:
						actualText = actualText + (e.Shift ? "Q" : "q");
						break;
					case KeyboardKeys.R:
						actualText = actualText + (e.Shift ? "R" : "r");
						break;
					case KeyboardKeys.S:
						actualText = actualText + (e.Shift ? "S" : "s");
						break;
					case KeyboardKeys.T:
						actualText = actualText + (e.Shift ? "T" : "t");
						break;
					case KeyboardKeys.U:
						actualText = actualText + (e.Shift ? "U" : "u");
						break;
					case KeyboardKeys.V:
						actualText = actualText + (e.Shift ? "V" : "v");
						break;
					case KeyboardKeys.W:
						actualText = actualText + (e.Shift ? "W" : "w");
						break;
					case KeyboardKeys.X:
						actualText = actualText + (e.Shift ? "X" : "x");
						break;
					case KeyboardKeys.Y:
						actualText = actualText + (e.Shift ? "Y" : "y");
						break;
					case KeyboardKeys.Z:
						actualText = actualText + (e.Shift ? "Z" : "z");
						break;
					case KeyboardKeys.OemQuotes:
						actualText = actualText + (e.Shift ? "\"" : "'");
						break;
					case KeyboardKeys.OemSemicolon:
						actualText = actualText + (e.Shift ? ":" : ";");
						break;
					case KeyboardKeys.Oemtilde:
						actualText = actualText + (e.Shift ? "~" : "`");
						break;
					case KeyboardKeys.OemPeriod:
						actualText = actualText + (e.Shift ? ">" : ".");
						break;
					case KeyboardKeys.Oemcomma:
						actualText = actualText + (e.Shift ? "<" : ",");
						break;
					case KeyboardKeys.Space:
						actualText = actualText + " ";
						break;
					case KeyboardKeys.D0:
						actualText = actualText + (e.Shift ? ")" : "0");
						break;
					case KeyboardKeys.D1:
						actualText = actualText + (e.Shift ? "!" : "1");
						break;
					case KeyboardKeys.D2:
						actualText = actualText + (e.Shift ? "@" : "2");
						break;
					case KeyboardKeys.D3:
						actualText = actualText + (e.Shift ? "#" : "3");
						break;
					case KeyboardKeys.D4:
						actualText = actualText + (e.Shift ? "$" : "4");
						break;
					case KeyboardKeys.D5:
						actualText = actualText + (e.Shift ? "%" : "5");
						break;
					case KeyboardKeys.D6:
						actualText = actualText + (e.Shift ? "^" : "6");
						break;
					case KeyboardKeys.D7:
						actualText = actualText + (e.Shift ? "&" : "7");
						break;
					case KeyboardKeys.D8:
						actualText = actualText + (e.Shift ? "*" : "8");
						break;
					case KeyboardKeys.D9:
						actualText = actualText + (e.Shift ? "(" : "9");
						break;
					case KeyboardKeys.NumPad0:
						actualText = actualText + (e.Shift ? "!" : "0");
						break;
					case KeyboardKeys.NumPad1:
						actualText = actualText + "1";
						break;
					case KeyboardKeys.NumPad2:
						actualText = actualText + "2";
						break;
					case KeyboardKeys.NumPad3:
						actualText = actualText + "3";
						break;
					case KeyboardKeys.NumPad4:
						actualText = actualText + "4";
						break;
					case KeyboardKeys.NumPad5:
						actualText = actualText + "5";
						break;
					case KeyboardKeys.NumPad6:
						actualText = actualText + "6";
						break;
					case KeyboardKeys.NumPad7:
						actualText = actualText + "7";
						break;
					case KeyboardKeys.NumPad8:
						actualText = actualText + "8";
						break;
					case KeyboardKeys.NumPad9:
						actualText = actualText + "9";
						break;
					case KeyboardKeys.Subtract:
						actualText = actualText + "-";
						break;
					case KeyboardKeys.Multiply:
						actualText = actualText + "*";
						break;
					case KeyboardKeys.Divide:
						actualText = actualText + "/";
						break;
					case KeyboardKeys.Add:
						actualText = actualText + "+";
						break;
					case KeyboardKeys.OemMinus:
						actualText = actualText + (e.Shift ? "_" : "-");
						break;
					case KeyboardKeys.Enter:
						isSelected = false;
						break;
					case KeyboardKeys.Back:
						if (actualText.Length > 0)
						{
							actualText = actualText.Substring(0, actualText.Length - 1);
						}
						break;
				}
				text.SetText(actualText);
				if (text.GetWidth() > width)
				{
					text.SetText(prevText);
					actualText = prevText;
				}
				return true;
			}
			return false;
		}
		#endregion
		#endregion
	}

	public class BBSingleLineTextBox
	{
		#region Member Variables
		private Label text;
		private string actualText;
		private BBStretchableImage background;
		private int xPos;
		private int yPos;
		private int width;
		private int height;

		private bool isReadOnly;
		private bool isSelected;
		private bool pressed;
		private bool blink;
		private float timer;
		#endregion

		#region Constructors
		public bool Initialize(string text, int x, int y, int width, int height, bool isReadOnly, SpriteManager spriteManager, Random r, out string reason)
		{
			xPos = x;
			yPos = y;
			this.width = width;
			this.height = height;

			background = new BBStretchableImage();
			if (!background.Initialize(x, y, width, height, StretchableImageType.TextBox, spriteManager, r, out reason))
			{
				return false;
			}

			actualText = string.Empty;
			this.text = new Label(text, x + 6, y + 7);
			pressed = false;
			isSelected = false;
			blink = true;
			this.isReadOnly = isReadOnly;

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public bool MouseDown(int x, int y)
		{
			if (isReadOnly)
			{
				return false;
			}
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				pressed = true;
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (isReadOnly)
			{
				return false;
			}
			if (pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					blink = true;
					isSelected = true;
				}
				else
				{
					isSelected = false;
				}
			}
			else
			{
				isSelected = false;
			}
			if (!isSelected)
			{
				blink = false;
				text.SetText(actualText);
			}
			return isSelected;
		}

		public void Update(float frameDeltaTime)
		{
			if (isSelected)
			{
				timer += frameDeltaTime;
				if (timer >= 0.25f)
				{
					blink = !blink;
					text.SetText(actualText + (blink ? "|" : ""));
					timer -= 0.25f;
				}
			}
		}

		public void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			background.MoveTo(x, y);
			text.Move(x + 6, y + 7);
		}

		public void Draw()
		{
			background.Draw();
			text.Draw();
		}

		public string GetString()
		{
			return actualText;
		}

		public void SetString(string text)
		{
			actualText = text;
			this.text.SetText(text);
		}

		#region Keys
		public bool KeyDown(KeyboardInputEventArgs e)
		{
			if (isSelected)
			{
				string prevText = actualText;
				switch (e.Key)
				{
					case KeyboardKeys.A:
						actualText = actualText + (e.Shift ? "A" : "a");
						break;
					case KeyboardKeys.B:
						actualText = actualText + (e.Shift ? "B" : "b");
						break;
					case KeyboardKeys.C:
						actualText = actualText + (e.Shift ? "C" : "c");
						break;
					case KeyboardKeys.D:
						actualText = actualText + (e.Shift ? "D" : "d");
						break;
					case KeyboardKeys.E:
						actualText = actualText + (e.Shift ? "E" : "e");
						break;
					case KeyboardKeys.F:
						actualText = actualText + (e.Shift ? "F" : "f");
						break;
					case KeyboardKeys.G:
						actualText = actualText + (e.Shift ? "G" : "g");
						break;
					case KeyboardKeys.H:
						actualText = actualText + (e.Shift ? "H" : "h");
						break;
					case KeyboardKeys.I:
						actualText = actualText + (e.Shift ? "I" : "i");
						break;
					case KeyboardKeys.J:
						actualText = actualText + (e.Shift ? "J" : "j");
						break;
					case KeyboardKeys.K:
						actualText = actualText + (e.Shift ? "K" : "k");
						break;
					case KeyboardKeys.L:
						actualText = actualText + (e.Shift ? "L" : "l");
						break;
					case KeyboardKeys.M:
						actualText = actualText + (e.Shift ? "M" : "m");
						break;
					case KeyboardKeys.N:
						actualText = actualText + (e.Shift ? "N" : "n");
						break;
					case KeyboardKeys.O:
						actualText = actualText + (e.Shift ? "O" : "o");
						break;
					case KeyboardKeys.P:
						actualText = actualText + (e.Shift ? "P" : "p");
						break;
					case KeyboardKeys.Q:
						actualText = actualText + (e.Shift ? "Q" : "q");
						break;
					case KeyboardKeys.R:
						actualText = actualText + (e.Shift ? "R" : "r");
						break;
					case KeyboardKeys.S:
						actualText = actualText + (e.Shift ? "S" : "s");
						break;
					case KeyboardKeys.T:
						actualText = actualText + (e.Shift ? "T" : "t");
						break;
					case KeyboardKeys.U:
						actualText = actualText + (e.Shift ? "U" : "u");
						break;
					case KeyboardKeys.V:
						actualText = actualText + (e.Shift ? "V" : "v");
						break;
					case KeyboardKeys.W:
						actualText = actualText + (e.Shift ? "W" : "w");
						break;
					case KeyboardKeys.X:
						actualText = actualText + (e.Shift ? "X" : "x");
						break;
					case KeyboardKeys.Y:
						actualText = actualText + (e.Shift ? "Y" : "y");
						break;
					case KeyboardKeys.Z:
						actualText = actualText + (e.Shift ? "Z" : "z");
						break;
					case KeyboardKeys.OemQuotes:
						actualText = actualText + (e.Shift ? "\"" : "'");
						break;
					case KeyboardKeys.OemSemicolon:
						actualText = actualText + (e.Shift ? ":" : ";");
						break;
					case KeyboardKeys.Oemtilde:
						actualText = actualText + (e.Shift ? "~" : "`");
						break;
					case KeyboardKeys.OemPeriod:
						actualText = actualText + (e.Shift ? ">" : ".");
						break;
					case KeyboardKeys.Oemcomma:
						actualText = actualText + (e.Shift ? "<" : ",");
						break;
					case KeyboardKeys.Space:
						actualText = actualText + " ";
						break;
					case KeyboardKeys.D0:
						actualText = actualText + (e.Shift ? ")" : "0");
						break;
					case KeyboardKeys.D1:
						actualText = actualText + (e.Shift ? "!" : "1");
						break;
					case KeyboardKeys.D2:
						actualText = actualText + (e.Shift ? "@" : "2");
						break;
					case KeyboardKeys.D3:
						actualText = actualText + (e.Shift ? "#" : "3");
						break;
					case KeyboardKeys.D4:
						actualText = actualText + (e.Shift ? "$" : "4");
						break;
					case KeyboardKeys.D5:
						actualText = actualText + (e.Shift ? "%" : "5");
						break;
					case KeyboardKeys.D6:
						actualText = actualText + (e.Shift ? "^" : "6");
						break;
					case KeyboardKeys.D7:
						actualText = actualText + (e.Shift ? "&" : "7");
						break;
					case KeyboardKeys.D8:
						actualText = actualText + (e.Shift ? "*" : "8");
						break;
					case KeyboardKeys.D9:
						actualText = actualText + (e.Shift ? "(" : "9");
						break;
					case KeyboardKeys.NumPad0:
						actualText = actualText + (e.Shift ? "!" : "0");
						break;
					case KeyboardKeys.NumPad1:
						actualText = actualText + "1";
						break;
					case KeyboardKeys.NumPad2:
						actualText = actualText + "2";
						break;
					case KeyboardKeys.NumPad3:
						actualText = actualText + "3";
						break;
					case KeyboardKeys.NumPad4:
						actualText = actualText + "4";
						break;
					case KeyboardKeys.NumPad5:
						actualText = actualText + "5";
						break;
					case KeyboardKeys.NumPad6:
						actualText = actualText + "6";
						break;
					case KeyboardKeys.NumPad7:
						actualText = actualText + "7";
						break;
					case KeyboardKeys.NumPad8:
						actualText = actualText + "8";
						break;
					case KeyboardKeys.NumPad9:
						actualText = actualText + "9";
						break;
					case KeyboardKeys.Subtract:
						actualText = actualText + "-";
						break;
					case KeyboardKeys.Multiply:
						actualText = actualText + "*";
						break;
					case KeyboardKeys.Divide:
						actualText = actualText + "/";
						break;
					case KeyboardKeys.Add:
						actualText = actualText + "+";
						break;
					case KeyboardKeys.OemMinus:
						actualText = actualText + (e.Shift ? "_" : "-");
						break;
					case KeyboardKeys.Enter:
						isSelected = false;
						break;
					case KeyboardKeys.Back:
						if (actualText.Length > 0)
						{
							actualText = actualText.Substring(0, actualText.Length - 1);
						}
						break;
				}
				text.SetText(actualText);
				if (text.GetWidth() > width - 8)
				{
					text.SetText(prevText);
					actualText = prevText;
				}
				return true;
			}
			return false;
		}
		#endregion
		#endregion
	}
}
