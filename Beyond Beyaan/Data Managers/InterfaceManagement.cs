using System;
using System.Collections.Generic;
using System.Drawing;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	public enum ButtonTextAlignment { LEFT, CENTER, RIGHT }
	public class BBButton
	{
		#region Member Variables
		private BBToolTip toolTip;
		private BBSprite backgroundSprite;
		private BBSprite foregroundSprite;
		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		private BBLabel _label;
		private ButtonTextAlignment _alignment;
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
		public bool Initialize(string backgroundSprite, string foregroundSprite, string buttonText, ButtonTextAlignment alignment, int xPos, int yPos, int width, int height, Random r, out string reason)
		{
			this.backgroundSprite = SpriteManager.GetSprite(backgroundSprite, r);
			this.foregroundSprite = SpriteManager.GetSprite(foregroundSprite, r);
			if (backgroundSprite == null || foregroundSprite == null)
			{
				reason = string.Format("One of those sprites does not exist in sprites.xml: \"{0}\" or \"{1}\"", backgroundSprite, foregroundSprite);
				return false;
			}
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_alignment = alignment;

			_label = new BBLabel();
			if (!_label.Initialize(0, 0, buttonText, Color.White, out reason))
			{
				return false;
			}

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
			_label.SetText(text);
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}
		public bool SetToolTip(string name, string text, int screenWidth, int screenHeight, Random r, out string reason)
		{
			toolTip = new BBToolTip();
			if (!toolTip.Initialize(name, text, screenWidth, screenHeight, r, out reason))
			{
				return false;
			}
			return true;
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}

		public void Resize(int width, int height)
		{
			this._width = width;
			this._height = height;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
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
				if (toolTip != null)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
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
			if (toolTip != null)
			{
				toolTip.SetShowing(false);
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (toolTip != null)
				{
					toolTip.SetShowing(false);
				}
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
				if (toolTip != null)
				{
					toolTip.SetShowing(false);
				}
				pressed = false;
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
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
					foregroundSprite.Draw(_xPos, _yPos, foregroundSprite.Width / _width, foregroundSprite.Height / _height);
				}
				else if (!Selected)
				{
					backgroundSprite.Draw(_xPos, _yPos, foregroundSprite.Width / _width, foregroundSprite.Height / _height);
					if (pulse > 0)
					{
						foregroundSprite.Draw(_xPos, _yPos, foregroundSprite.Width / _width, foregroundSprite.Height / _height, (byte)(255 * pulse));
					}
				}
			}
			else
			{
				backgroundSprite.Draw(_xPos, _yPos, foregroundSprite.Width / _width, foregroundSprite.Height / _height, System.Drawing.Color.Tan);
				if (Selected)
				{
					foregroundSprite.Draw(_xPos, _yPos, foregroundSprite.Width / _width, foregroundSprite.Height / _height, System.Drawing.Color.Tan);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		public void DrawToolTip()
		{
			if (toolTip != null)
			{
				toolTip.Draw();
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
		private BBLabel _label;
		private ButtonTextAlignment _alignment;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;

		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(List<string> backgroundSections, List<string> foregroundSections, bool isHorizontal, string buttonText, ButtonTextAlignment alignment, int xPos, int yPos, int width, int height, Random r, out string reason)
		{
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_alignment = alignment;

			backgroundImage = new BBUniStretchableImage();
			foregroundImage = new BBUniStretchableImage();

			if (!backgroundImage.Initialize(xPos, yPos, width, height, 7, 2, isHorizontal, backgroundSections, r, out reason))
			{
				return false;
			}
			if (!foregroundImage.Initialize(xPos, yPos, width, height, 7, 2, isHorizontal, foregroundSections, r, out reason))
			{
				return false;
			}

			_label = new BBLabel();
			if (!_label.Initialize(0, 0, buttonText, Color.White, out reason))
			{
				return false;
			}

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
			_label.SetText(text);
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
			backgroundImage.MoveTo(x, y);
			foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this._width = width;
			this._height = height;
			backgroundImage.Resize(width, height);
			foregroundImage.Resize(width, height);
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
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
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
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
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
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
				backgroundImage.Draw(Color.Tan, 255);
				if (Selected)
				{
					foregroundImage.Draw(Color.Tan, 255);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		#endregion
	}

	public class BBStretchButton
	{
		#region Member Variables
		protected BBStretchableImage _backgroundImage;
		protected BBStretchableImage _foregroundImage;
		protected BBLabel _label;
		protected ButtonTextAlignment _alignment;

		//Button state information
		protected bool _pressed;
		protected float _pulse;
		protected bool _direction;

		protected int _xPos;
		protected int _yPos;
		protected int _width;
		protected int _height;
		#endregion

		#region Properties
		public bool Enabled { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public bool Initialize(string buttonText, ButtonTextAlignment alignment, StretchableImageType background, StretchableImageType foreground, int xPos, int yPos, int width, int height, Random r, out string reason)
		{
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;
			_alignment = alignment;

			_backgroundImage = new BBStretchableImage();
			_foregroundImage = new BBStretchableImage();

			if (!_backgroundImage.Initialize(xPos, yPos, width, height, background, r, out reason))
			{
				return false;
			}
			if (!_foregroundImage.Initialize(xPos, yPos, width, height, foreground, r, out reason))
			{
				return false;
			}

			_label = new BBLabel();
			if (!_label.Initialize(0, 0, buttonText, Color.White, out reason))
			{
				return false;
			}

			Reset();

			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public void Reset()
		{
			_pulse = 0;
			_direction = false;
			Enabled = true;
			_pressed = false;
			Selected = false;
		}

		public void SetText(string text)
		{
			_label.SetText(text);
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
		}
		public void SetTextColor(Color color)
		{
			_label.SetColor(color);
		}

		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;
			switch (_alignment)
			{
				case ButtonTextAlignment.LEFT:
					_label.MoveTo(_xPos + 5, (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.CENTER:
					_label.MoveTo((int)((_width / 2.0f) - (_label.GetWidth() / 2) + _xPos), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
				case ButtonTextAlignment.RIGHT:
					_label.MoveTo((int)(_xPos + _width - 5 - _label.GetWidth()), (int)((_height / 2.0f) - (_label.GetHeight() / 2) + _yPos));
					break;
			}
			_backgroundImage.MoveTo(x, y);
			_foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this._width = width;
			this._height = height;
			_backgroundImage.Resize(width, height);
			_foregroundImage.Resize(width, height);
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (_pulse < 0.6f)
				{
					_pulse = 0.9f;
				}
				if (Enabled)
				{
					if (_direction)
					{
						_pulse += frameDeltaTime / 2;
						if (_pulse > 0.9f)
						{
							_direction = !_direction;
							_pulse = 0.9f;
						}
					}
					else
					{
						_pulse -= frameDeltaTime / 2;
						if (_pulse < 0.6f)
						{
							_direction = !_direction;
							_pulse = 0.6f;
						}
					}
				}
				return true;
			}
			if (_pulse > 0)
			{
				_pulse -= frameDeltaTime * 2;
				if (_pulse < 0)
				{
					_pulse = 0;
				}
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
			{
				if (Enabled)
				{
					_pressed = true;
				}
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Enabled && _pressed)
			{
				_pressed = false;
				if (x >= _xPos && x < _xPos + _width && y >= _yPos && y < _yPos + _height)
				{
					return true;
				}
			}
			return false;
		}

		public void Draw()
		{
			if (Enabled)
			{
				if (_pressed || Selected)
				{
					_foregroundImage.Draw();
				}
				else if (!Selected)
				{
					_backgroundImage.Draw();
					if (_pulse > 0)
					{
						_foregroundImage.Draw((byte)(255 * _pulse));
					}
				}
			}
			else
			{
				_backgroundImage.Draw(Color.Tan, 255);
				if (Selected)
				{
					_foregroundImage.Draw(Color.Tan, 255);
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
		#endregion
	}

	public class BBInvisibleStretchButton : BBStretchButton
	{
		private bool _visible;

		new public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			_visible = base.MouseHover(x, y, frameDeltaTime);
			return _visible;
		}

		new public void Draw()
		{
			if (Enabled && (_visible || Selected))
			{
				if (_pressed || Selected)
				{
					_foregroundImage.Draw();
				}
				else if (!Selected)
				{
					_backgroundImage.Draw();
					if (_pulse > 0)
					{
						_foregroundImage.Draw((byte)(255 * _pulse));
					}
				}
			}
			if (_label.Text.Length > 0)
			{
				_label.Draw();
			}
		}
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

	public class BBComboBox
	{
		#region Member Variables
		//ComboBox drawing information
		private BBSprite _downArrowSprite;
		private int _xPos;
		private int _yPos;
		private int _width;
		private int _height;
		private List<string> _items;
		private List<BBInvisibleStretchButton> _buttons;
		private BBStretchableImage _dropBackground;
		private BBScrollBar _scrollBar;

		//ComboBox state information
		private bool _dropped;
		private bool _haveScroll;
		private int _selectedIndex;

		#endregion

		#region Properties
		public bool Enabled { get; set; }
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set { _selectedIndex = value; }
		}
		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="maxVisible"></param>
		/// <param name="r"></param>
		/// <param name="reason"></param>
		public bool Initialize(List<string> items, int xPos, int yPos, int width, int height, int maxVisible, Random r, out string reason)
		{
			_items = items;
			_xPos = xPos;
			_yPos = yPos;
			_width = width;
			_height = height;

			_selectedIndex = 0;
			_dropped = false;
			_downArrowSprite = SpriteManager.GetSprite("ScrollDownBGButton", r);

			if (items.Count < maxVisible)
			{
				_haveScroll = false;
				maxVisible = items.Count;
			}
			else if (items.Count > maxVisible)
			{
				_haveScroll = true;
			}

			Enabled = true;

			_buttons = new List<BBInvisibleStretchButton>();
			_dropBackground = new BBStretchableImage();
			_scrollBar = new BBScrollBar();
			
			for (int i = 0; i <= maxVisible; i++)
			{
				BBInvisibleStretchButton button = new BBInvisibleStretchButton();
				if (!button.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, _xPos, _yPos + (i * height), _width, _height, r, out reason))
				{
					return false;
				}
				_buttons.Add(button);
			}
			if (!_dropBackground.Initialize(_xPos, _yPos, width, height, StretchableImageType.ThinBorderBG, r, out reason))
			{
				return false;
			}
			if (!_scrollBar.Initialize(_xPos + _width, _yPos + _height, maxVisible * _height, maxVisible, items.Count, false, false, r, out reason))
			{
				return false;
			}
			RefreshSelection();
			RefreshLabels();
			return true;
		}
		#endregion

		#region Functions
		public void MoveTo(int x, int y)
		{
			_xPos = x;
			_yPos = y;

			for (int i = 0; i < _buttons.Count; i++)
			{
				_buttons[i].MoveTo(x, y + (i * _height));
			}

			_scrollBar.MoveTo(_xPos + _width, _yPos + _height);
			_dropBackground.MoveTo(_xPos, _yPos);
		}

		public void Draw()
		{
			if (_items.Count <= 0)
			{
				_buttons[0].Enabled = false;
				_buttons[0].SetText(string.Empty);
			}
			else
			{
				_buttons[0].Enabled = Enabled;
				_buttons[0].SetText(_items[_selectedIndex]);
			}
			if (!_dropped)
			{
				_dropBackground.Draw();
				_buttons[0].Draw();
				_downArrowSprite.Draw(_xPos + _width - 25, _yPos + (_height / 2) - (_downArrowSprite.Height / 2));
			}
			else
			{
				_dropBackground.Draw();
				for (int i = 0; i < _buttons.Count; i++)
				{
					_buttons[i].Draw();
				}
				if (_haveScroll)
				{
					_scrollBar.Draw();
				}
			}
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (Enabled)
			{
				if (!_dropped)
				{
					return _buttons[0].MouseHover(x, y, frameDeltaTime);
				}
				bool result = false;
				foreach (var button in _buttons)
				{
					result = button.MouseHover(x, y, frameDeltaTime) || result;
				}
				if (_scrollBar.MouseHover(x, y, frameDeltaTime))
				{
					//Need to refresh the button text
					RefreshLabels();
					result = true;
				}
				return result;
			}
			return false;
		}

		public bool MouseDown(int x, int y)
		{
			if (Enabled)
			{
				if (!_dropped)
				{
					return _buttons[0].MouseDown(x, y);
				}
				for (int i = 0; i < _buttons.Count; i++)
				{
					if (_buttons[i].MouseDown(x, y))
					{
						return true;
					}
				}
				return _scrollBar.MouseDown(x, y);
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			if (Enabled)
			{
				if (!_dropped)
				{
					if (_buttons[0].MouseUp(x, y))
					{
						_dropped = true;
						_dropBackground.Resize(_width + 30, _height * _items.Count + 20);
						_dropBackground.MoveTo(_xPos - 10, _yPos - 10);
						return true;
					}
				}
				else
				{
					for (int i = 0; i < _buttons.Count; i++)
					{
						if (_buttons[i].MouseUp(x, y))
						{
							if (i > 0)
							{
								_selectedIndex = i + _scrollBar.TopIndex - 1;
							}
							_dropped = false;
							_dropBackground.Resize(_width, _height);
							_dropBackground.MoveTo(_xPos, _yPos);
							return true;
						}
					}
					if (_scrollBar.MouseUp(x, y))
					{
						RefreshLabels();
						return true;
					}
					//At this point, even if the mouse is not over the UI, we want to capture the mouse up so the user don't click on something else
					_dropped = false;
					return true;
				}
			}
			return false;
		}

		private void RefreshLabels()
		{
			for (int i = 1; i < _buttons.Count; i++)
			{
				_buttons[i].SetText(_items[_scrollBar.TopIndex + (i - 1)]);
			}
		}
		private void RefreshSelection()
		{
			_buttons[0].SetText(_items[_selectedIndex]);
			_dropped = false;
		}
		#endregion
	}

	public class BBScrollBar
	{
		// TODO: Fix scrollbar so if I say 100 items, I don't need to specify 101 for amountOfItems
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
		public bool Initialize(int xPos, int yPos, int length, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, Random r, out string reason)
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
					if (!Up.Initialize("ScrollUpBGButton", "ScrollUpFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollDownBGButton", "ScrollDownFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos + length - 16, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollVerticalBGButton1", "ScrollVerticalBGButton2", "ScrollVerticalBGButton3" }, 
										   new List<string> { "ScrollVerticalFGButton1", "ScrollVerticalFGButton2", "ScrollVerticalFGButton3" },
										   false, "", ButtonTextAlignment.LEFT, xPos, yPos + 16, 16, scrollButtonLength, r, out reason))
					{
						return false;
					}
					scrollBar = SpriteManager.GetSprite("ScrollVerticalBar", r);
					if (scrollBar == null)
					{
						reason = "\"ScrollVerticalBar\" sprite does not exist";
						return false;
					}
				}
				else
				{
					if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", ButtonTextAlignment.CENTER, xPos + length - 16, yPos, 16, 16, r, out reason))
					{
						return false;
					}
					if (!Scroll.Initialize(new List<string> { "ScrollHorizontalBGButton1", "ScrollHorizontalBGButton2", "ScrollHorizontalBGButton3" },
										   new List<string> { "ScrollHorizontalFGButton1", "ScrollHorizontalFGButton2", "ScrollHorizontalFGButton3" },
										   false, "", ButtonTextAlignment.LEFT, xPos + 16, yPos, 16, scrollButtonLength, r, out reason))
					{
						return false;
					}
					scrollBar = SpriteManager.GetSprite("ScrollHorizontalBar", r);
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
				if (!Up.Initialize("ScrollLeftBGButton", "ScrollLeftFGButton", "", ButtonTextAlignment.CENTER, xPos, yPos, 16, 16, r, out reason))
				{
					return false;
				}
				if (!Down.Initialize("ScrollRightBGButton", "ScrollRightFGButton", "", ButtonTextAlignment.CENTER, xPos + length - 16, yPos, 16, 16, r, out reason))
				{
					return false;
				}
				if (!Scroll.Initialize(new List<string> { "SliderHorizontalBGButton1", "SliderHorizontalBGButton2", "SliderHorizontalBGButton3" },
									   new List<string> { "SliderHorizontalFGButton1", "SliderHorizontalFGButton2", "SliderHorizontalFGButton3" },
									   true, "", ButtonTextAlignment.LEFT, xPos + 16, yPos, 16, scrollButtonLength, r, out reason))
				{
					return false;
				}
				scrollBar = SpriteManager.GetSprite("SliderBGBar", r);
				if (scrollBar == null)
				{
					reason = "\"SliderBGBar\" sprite does not exist";
					return false;
				}
				highlightedScrollBar = SpriteManager.GetSprite("SliderFGBar", r);
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
				Up.MouseHover(x, y, frameDeltaTime);
				Down.MouseHover(x, y, frameDeltaTime);
				return false;
			}
			return false;
		}

		public void MoveTo(int x, int y)
		{
			Up.MoveTo(x, y);
			if (isHorizontal)
			{
				Down.MoveTo(x + scrollBarLength + 16, y);
				Scroll.MoveTo(x + 16 + scrollPos, y);
			}
			else
			{
				Down.MoveTo(x, y + scrollBarLength + 16);
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
		ThinBorderBG,
		ThinBorderFG,
		TinyButtonBG,
		TinyButtonFG,
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
		public bool Initialize(int x, int y, int width, int height, StretchableImageType type, Random r, out string reason)
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
						var tempSprite = SpriteManager.GetSprite("TextTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TextBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TextBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					}
					break;
				case StretchableImageType.ThinBorderBG:
				{
					sectionWidth = 30;
					sectionHeight = 13;
					sections = new List<BBSprite>();
					var tempSprite = SpriteManager.GetSprite("ThinBorderBGTL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGTC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGTR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGTR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGCR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGCR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBL", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBL\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBC", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBC\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
					tempSprite = SpriteManager.GetSprite("ThinBorderBGBR", r);
					if (tempSprite == null)
					{
						reason = "Failed to get \"ThinBorderBGBR\" from sprites.xml.";
						return false;
					}
					sections.Add(tempSprite);
				} break;
				case StretchableImageType.ThinBorderFG:
					{
						sectionWidth = 30;
						sectionHeight = 13;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("ThinBorderFGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThinBorderFGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThinBorderFGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.TinyButtonBG:
					{
						sectionWidth = 10;
						sectionHeight = 10;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("TinyButtonBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonBGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonBGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.TinyButtonFG:
					{
						sectionWidth = 10;
						sectionHeight = 10;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("TinyButtonFGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("TinyButtonFGBR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"TinyButtonFGBR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
					} break;
				case StretchableImageType.MediumBorder:
					{
						sectionWidth = 60;
						sectionHeight = 60;
						sections = new List<BBSprite>();
						var tempSprite = SpriteManager.GetSprite("MediumBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"MediumBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("MediumBorderBGBR", r);
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
						var tempSprite = SpriteManager.GetSprite("ThickBorderBGTL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGTC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGTR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGTR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGCR", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGCR\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBL", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBL\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBC", r);
						if (tempSprite == null)
						{
							reason = "Failed to get \"ThickBorderBGBC\" from sprites.xml.";
							return false;
						}
						sections.Add(tempSprite);
						tempSprite = SpriteManager.GetSprite("ThickBorderBGBR", r);
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

		public void Draw(Color color, byte alpha)
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
		public void Resize(int width, int height)
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
		public bool Initialize(int x, int y, int width, int height, int mainLength, int stretchLength, bool isHorizontal, List<string> sections, Random r, out string reason)
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
				BBSprite sprite = SpriteManager.GetSprite(spriteName, r);
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

	public class Label
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

	public class BBLabel
	{
		#region Member Variables
		private int x;
		private int y;
		private bool isRightAligned;
		private TextSprite textSprite;
		private Color color;
		#endregion

		#region Properties
		public string Text { get; private set; }
		#endregion

		#region Constructor
		public bool Initialize(int x, int y, string label, Color color, out string reason)
		{
			this.x = x;
			this.y = y;
			this.color = color;
			if (!SetText(label))
			{
				reason = "Default font not found";
				return false;
			}
			reason = null;
			return true;
		}
		public bool Initialize(int x, int y, string label, Color color, string fontName, out string reason)
		{
			this.x = x;
			this.y = y;
			this.color = color;
			if (!SetText(label, fontName))
			{
				reason = fontName + " font not found";
				return false;
			}
			reason = null;
			return true;
		}
		#endregion

		#region Functions
		public bool SetText(string text)
		{
			var font = FontManager.GetDefaultFont();
			if (font == null)
			{
				return false;
			}
			textSprite = new TextSprite("Arial", text, font);
			SetAlignment(isRightAligned);
			Text = text;
			return true;
		}
		public bool SetText(string text, string fontName)
		{
			var font = FontManager.GetFont(fontName);
			if (font == null)
			{
				return false;
			}
			textSprite = new TextSprite("Arial", text, font);
			SetAlignment(isRightAligned);
			Text = text;
			return true;
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
			return textSprite.Width > 0 ? textSprite.Width : 1;
		}
		public float GetHeight()
		{
			return textSprite.Height > 0 ? textSprite.Height : 1;
		}
		public void MoveTo(int x, int y)
		{
			this.x = x;
			this.y = y;
			textSprite.SetPosition(x, y);
		}
		public void SetColor(Color color)
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

	public class BBSingleLineTextBox
	{
		#region Member Variables
		private BBLabel text;
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

		#region Properties
		public string Text { get; private set; }
		#endregion

		#region Constructors
		public bool Initialize(string text, int x, int y, int width, int height, bool isReadOnly, Random r, out string reason)
		{
			xPos = x;
			yPos = y;
			this.width = width;
			this.height = height;

			background = new BBStretchableImage();
			if (!background.Initialize(x, y, width, height, StretchableImageType.TextBox, r, out reason))
			{
				return false;
			}

			Text = string.Empty;
			this.text = new BBLabel();
			if (!this.text.Initialize(x + 6, y + 7, text, Color.White, out reason))
			{
				return false;
			}
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
				text.SetText(Text);
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
					text.SetText(Text + (blink ? "|" : ""));
					timer -= 0.25f;
				}
			}
		}

		public void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			background.MoveTo(x, y);
			text.MoveTo(x + 6, y + 7);
		}

		public void Draw()
		{
			background.Draw();
			text.Draw();
		}

		public void SetString(string text)
		{
			Text = text;
			this.text.SetText(text);
		}

		public void SetReadOnly(bool readOnly)
		{
			isReadOnly = readOnly;
		}

		public void SetColor(Color color)
		{
			text.SetColor(color);
		}

		#region Keys
		public bool KeyDown(KeyboardInputEventArgs e)
		{
			if (isSelected)
			{
				string prevText = Text;
				switch (e.Key)
				{
					case KeyboardKeys.A:
						Text = Text + (e.Shift ? "A" : "a");
						break;
					case KeyboardKeys.B:
						Text = Text + (e.Shift ? "B" : "b");
						break;
					case KeyboardKeys.C:
						Text = Text + (e.Shift ? "C" : "c");
						break;
					case KeyboardKeys.D:
						Text = Text + (e.Shift ? "D" : "d");
						break;
					case KeyboardKeys.E:
						Text = Text + (e.Shift ? "E" : "e");
						break;
					case KeyboardKeys.F:
						Text = Text + (e.Shift ? "F" : "f");
						break;
					case KeyboardKeys.G:
						Text = Text + (e.Shift ? "G" : "g");
						break;
					case KeyboardKeys.H:
						Text = Text + (e.Shift ? "H" : "h");
						break;
					case KeyboardKeys.I:
						Text = Text + (e.Shift ? "I" : "i");
						break;
					case KeyboardKeys.J:
						Text = Text + (e.Shift ? "J" : "j");
						break;
					case KeyboardKeys.K:
						Text = Text + (e.Shift ? "K" : "k");
						break;
					case KeyboardKeys.L:
						Text = Text + (e.Shift ? "L" : "l");
						break;
					case KeyboardKeys.M:
						Text = Text + (e.Shift ? "M" : "m");
						break;
					case KeyboardKeys.N:
						Text = Text + (e.Shift ? "N" : "n");
						break;
					case KeyboardKeys.O:
						Text = Text + (e.Shift ? "O" : "o");
						break;
					case KeyboardKeys.P:
						Text = Text + (e.Shift ? "P" : "p");
						break;
					case KeyboardKeys.Q:
						Text = Text + (e.Shift ? "Q" : "q");
						break;
					case KeyboardKeys.R:
						Text = Text + (e.Shift ? "R" : "r");
						break;
					case KeyboardKeys.S:
						Text = Text + (e.Shift ? "S" : "s");
						break;
					case KeyboardKeys.T:
						Text = Text + (e.Shift ? "T" : "t");
						break;
					case KeyboardKeys.U:
						Text = Text + (e.Shift ? "U" : "u");
						break;
					case KeyboardKeys.V:
						Text = Text + (e.Shift ? "V" : "v");
						break;
					case KeyboardKeys.W:
						Text = Text + (e.Shift ? "W" : "w");
						break;
					case KeyboardKeys.X:
						Text = Text + (e.Shift ? "X" : "x");
						break;
					case KeyboardKeys.Y:
						Text = Text + (e.Shift ? "Y" : "y");
						break;
					case KeyboardKeys.Z:
						Text = Text + (e.Shift ? "Z" : "z");
						break;
					case KeyboardKeys.OemQuotes:
						Text = Text + (e.Shift ? "\"" : "'");
						break;
					case KeyboardKeys.OemSemicolon:
						Text = Text + (e.Shift ? ":" : ";");
						break;
					case KeyboardKeys.Oemtilde:
						Text = Text + (e.Shift ? "~" : "`");
						break;
					case KeyboardKeys.OemPeriod:
						Text = Text + (e.Shift ? ">" : ".");
						break;
					case KeyboardKeys.Oemcomma:
						Text = Text + (e.Shift ? "<" : ",");
						break;
					case KeyboardKeys.Space:
						Text = Text + " ";
						break;
					case KeyboardKeys.D0:
						Text = Text + (e.Shift ? ")" : "0");
						break;
					case KeyboardKeys.D1:
						Text = Text + (e.Shift ? "!" : "1");
						break;
					case KeyboardKeys.D2:
						Text = Text + (e.Shift ? "@" : "2");
						break;
					case KeyboardKeys.D3:
						Text = Text + (e.Shift ? "#" : "3");
						break;
					case KeyboardKeys.D4:
						Text = Text + (e.Shift ? "$" : "4");
						break;
					case KeyboardKeys.D5:
						Text = Text + (e.Shift ? "%" : "5");
						break;
					case KeyboardKeys.D6:
						Text = Text + (e.Shift ? "^" : "6");
						break;
					case KeyboardKeys.D7:
						Text = Text + (e.Shift ? "&" : "7");
						break;
					case KeyboardKeys.D8:
						Text = Text + (e.Shift ? "*" : "8");
						break;
					case KeyboardKeys.D9:
						Text = Text + (e.Shift ? "(" : "9");
						break;
					case KeyboardKeys.NumPad0:
						Text = Text + (e.Shift ? "!" : "0");
						break;
					case KeyboardKeys.NumPad1:
						Text = Text + "1";
						break;
					case KeyboardKeys.NumPad2:
						Text = Text + "2";
						break;
					case KeyboardKeys.NumPad3:
						Text = Text + "3";
						break;
					case KeyboardKeys.NumPad4:
						Text = Text + "4";
						break;
					case KeyboardKeys.NumPad5:
						Text = Text + "5";
						break;
					case KeyboardKeys.NumPad6:
						Text = Text + "6";
						break;
					case KeyboardKeys.NumPad7:
						Text = Text + "7";
						break;
					case KeyboardKeys.NumPad8:
						Text = Text + "8";
						break;
					case KeyboardKeys.NumPad9:
						Text = Text + "9";
						break;
					case KeyboardKeys.Subtract:
						Text = Text + "-";
						break;
					case KeyboardKeys.Multiply:
						Text = Text + "*";
						break;
					case KeyboardKeys.Divide:
						Text = Text + "/";
						break;
					case KeyboardKeys.Add:
						Text = Text + "+";
						break;
					case KeyboardKeys.OemMinus:
						Text = Text + (e.Shift ? "_" : "-");
						break;
					case KeyboardKeys.Enter:
						isSelected = false;
						break;
					case KeyboardKeys.Back:
						if (Text.Length > 0)
						{
							Text = Text.Substring(0, Text.Length - 1);
						}
						break;
				}
				text.SetText(Text);
				if (text.GetWidth() > width - 8)
				{
					text.SetText(prevText);
					Text = prevText;
				}
				return true;
			}
			return false;
		}
		#endregion
		#endregion
	}

	public class BBTextBox
	{
		#region Member Variables
		private Viewport _wrapView;
		private TextSprite _textSprite;
		private BBScrollBar _textScrollBar;
		private RenderImage _target;
		private bool _scrollbarVisible;
		private bool _usingScrollBar;
		private int _x;
		private int _y;
		#endregion

		public int Width { get; private set; }
		public int Height { get; private set; }

		public bool Initialize(int xPos, int yPos, int width, int height, bool useScrollBar, string name, Random r, out string reason)
		{
			//If using scrollbar, then shrink the actual width by 16 to allow for scrollbar, even if it's not visible
			_x = xPos;
			_y = yPos;
			Width = width;
			Height = height == 0 ? 1 : height;
			_usingScrollBar = useScrollBar;
			_scrollbarVisible = false;
			if (_usingScrollBar)
			{
				_textScrollBar = new BBScrollBar();
				if (!_textScrollBar.Initialize(xPos + Width - 16, yPos, Height, Height, 1, false, false, r, out reason))
				{
					return false;
				}
				_wrapView = new Viewport(0, 0, Width - 16, Height);

				_target = new RenderImage(name + "render", Width - 16, Height, ImageBufferFormats.BufferRGB888A8);
			}
			else
			{
				_wrapView = new Viewport(0, 0, Width, Height);

				_target = new RenderImage(name + "render", Width, Height, ImageBufferFormats.BufferRGB888A8);
			}
			_textSprite = new TextSprite(name, string.Empty, FontManager.GetDefaultFont());
			_textSprite.WordWrap = true;
			_textSprite.Bounds = _wrapView;
			_target.BlendingMode = BlendingModes.Modulated;
			reason = null;
			return true;
		}

		public void SetText(string text)
		{
			_textSprite.Text = text;
			if (_usingScrollBar)
			{
				_textScrollBar.TopIndex = 0;
				if (_textSprite.Height > Height)
				{
					_scrollbarVisible = true;
					_textScrollBar.SetAmountOfItems((int)_textSprite.Height);
				}
				else
				{
					_scrollbarVisible = false;
				}
			}
			else
			{
				//Expand the height to the actual text sprite's height
				Height = (int)_textSprite.Height;
				_target.Height = Height;
			}
			//Draw it once onto _target for performance reasons
			_target.Clear(Color.FromArgb(0, Color.Black));
			RenderTarget old = GorgonLibrary.Gorgon.CurrentRenderTarget;
			GorgonLibrary.Gorgon.CurrentRenderTarget = _target;
			_textSprite.SetPosition(0, _usingScrollBar ? -_textScrollBar.TopIndex : 0);
			_textSprite.Draw();
			GorgonLibrary.Gorgon.CurrentRenderTarget = old;
		}

		public void Draw()
		{
			if (_usingScrollBar && _scrollbarVisible)
			{
				_textScrollBar.Draw();
			}
			//Already rendered when text was set
			_target.Blit(_x, _y);
		}

		public bool MouseDown(int x, int y)
		{
			bool result = false;
			if (_usingScrollBar && _scrollbarVisible)
			{
				result = _textScrollBar.MouseDown(x, y);
			}
			if (!result && x >= _x && x < _x + Width && y >= _y && y < _y + Height)
			{
				return true;
			}
			return false;
		}

		public bool MouseUp(int x, int y)
		{
			bool result = false;
			if (_usingScrollBar && _scrollbarVisible)
			{
				result = _textScrollBar.MouseUp(x, y);
			}
			if (!result && x >= _x && x < _x + Width && y >= _y && y < _y + Height)
			{
				return true;
			}
			return false;
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (_usingScrollBar && _scrollbarVisible)
			{
				result = _textScrollBar.MouseHover(x, y, frameDeltaTime);
			}
			if (!result && x >= _x && x < _x + Width && y >= _y && y < _y + Height)
			{
				return true;
			}
			return false;
		}

		public void MoveTo(int x, int y)
		{
			_x = x;
			_y = y;
			if (_usingScrollBar)
			{
				_textScrollBar.MoveTo(_x + Width - 16, y);
			}
		}
	}

	public class BBToolTip
	{
		#region Member Variables
		private const int WIDTH = 250;

		private BBStretchableImage _background;
		private BBTextBox _text;

		private float _delayBeforeShowing;
		private bool _showing;

		private int _screenWidth;
		private int _screenHeight;

		private int _totalHeight;
		#endregion

		public bool Initialize(string name, string text, int screenWidth, int screenHeight, Random r, out string reason)
		{
			_text = new BBTextBox();
			if (!_text.Initialize(0, 0, WIDTH - 20, 0, false, name, r, out reason))
			{
				return false;
			}
			_text.SetText(text);

			_totalHeight = _text.Height + 10;

			_background = new BBStretchableImage();
			if (!_background.Initialize(0, 0, WIDTH, _totalHeight, StretchableImageType.ThinBorderBG, r, out reason))
			{
				return false;
			}

			_showing = false;
			_delayBeforeShowing = 0; //1 second will turn on showing

			_screenWidth = screenWidth;
			_screenHeight = screenHeight;

			reason = null;
			return true;
		}

		public void Draw()
		{
			if (_showing && _delayBeforeShowing >= 1)
			{
				_background.Draw();
				_text.Draw();
			}
		}

		public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (_showing)
			{
				int modifiedX = 0;
				int modifiedY = 0;
				if (x < _screenWidth - WIDTH)
				{
					modifiedX = x;
				}
				else
				{
					modifiedX = x - WIDTH;
				}
				if (y < _screenHeight - _totalHeight)
				{
					modifiedY = y;
				}
				else
				{
					modifiedY = y - _totalHeight;
				}
				_background.MoveTo(modifiedX, modifiedY);
				_text.MoveTo(modifiedX + 10, modifiedY + 5);

				if (_delayBeforeShowing < 1.0)
				{
					_delayBeforeShowing += frameDeltaTime;
				}
			}
			else
			{
				_delayBeforeShowing = 0;
			}
			return true;
		}

		public void SetShowing(bool showing)
		{
			_showing = showing;
			if (!_showing)
			{
				_delayBeforeShowing = 0;
			}
		}
	}
}
