using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class UIElement
	{
		protected int xPos;
		protected int yPos;
		protected int width;
		protected int height;

		public UIElement(int xPos, int yPos, int width, int height)
		{
			this.xPos = xPos;
			this.yPos = yPos;
			this.width = width;
			this.height = height;
		}

		virtual public bool MouseUp(int x, int y)
		{
			return false;
		}

		virtual public bool MouseDown(int x, int y)
		{
			return false;
		}
		virtual public bool MouseHover(int x, int y, float frameDeltaTime)
		{
			return false;
		}

		virtual public void MoveTo(int x, int y)
		{
			this.xPos = x;
			this.yPos = y;
		}

		virtual public void Draw(DrawingManagement drawingManagement)
		{
		}
	}

	public class Button : UIElement
	{
		#region Member Variables
		//Button drawing information
		private SpriteName backgroundSprite;
		private SpriteName foregroundSprite;
		private Label label;
		private ToolTip toolTip;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;
		private bool toolTipEnabled;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public Button(SpriteName backgroundSprite, SpriteName foregroundSprite, string buttonText, int xPos, int yPos, int width, int height) : base(xPos, yPos, width, height)
		{
			this.backgroundSprite = backgroundSprite;
			this.foregroundSprite = foregroundSprite;

			label = new Label(buttonText, xPos + 15, yPos + 5, System.Drawing.Color.DarkGreen);

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

		public void SetToolTip(List<SpriteName> toolTipSections, GorgonLibrary.Graphics.Font font, string toolTip, string buttonName, int toolTipSectionWidth, int toolTipSectionHeight, int screenWidth, int screenHeight)
		{
			toolTipEnabled = true;
			this.toolTip = new ToolTip(buttonName, toolTip, font, toolTipSections, toolTipSectionWidth, toolTipSectionHeight, screenWidth, screenHeight);
		}
		public void SetToolTip(string toolTip)
		{
			this.toolTip.SetText(toolTip);
		}

		public void SetButtonText(string text)
		{
			label.SetText(text);
		}

		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.MoveTo(x + 15, y + 5);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
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
				if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
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
			if (toolTipEnabled)
			{
				toolTip.SetShowing(false);
			}
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (toolTipEnabled)
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

		public override bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public override void Draw(DrawingManagement drawingManagement)
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
		public void DrawToolTip(DrawingManagement drawingManagement)
		{
			if (toolTipEnabled)
			{
				toolTip.Draw(drawingManagement);
			}
		}
		#endregion
	}

	public class UniStretchButton : UIElement
	{
		#region Member Variables
		private UniStretchableImage backgroundImage;
		private UniStretchableImage foregroundImage;
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
		public UniStretchButton(List<SpriteName> backgroundSections, List<SpriteName> foregroundSections, bool isHorizontal, string buttonText, int xPos, int yPos, int width, int height)
			: base(xPos, yPos, width, height)
		{
			backgroundImage = new UniStretchableImage(xPos, yPos, width, height, 7, 2, isHorizontal, backgroundSections);
			foregroundImage = new UniStretchableImage(xPos, yPos, width, height, 7, 2, isHorizontal, foregroundSections);

			label = new Label(buttonText, 0, 0);
			SetButtonText(buttonText);
			label.SetColor(System.Drawing.Color.DarkBlue);

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
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
		}

		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
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

		public override bool MouseHover(int x, int y, float frameDeltaTime)
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

		public override bool MouseDown(int x, int y)
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

		public override bool MouseUp(int x, int y)
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

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (Active)
			{
				if (pressed)
				{
					foregroundImage.Draw(drawingManagement);
				}
				else if (!Selected)
				{
					backgroundImage.Draw(drawingManagement);
					if (pulse > 0)
					{
						foregroundImage.Draw((byte)(255 * pulse), drawingManagement);
					}
				}
				else
				{
					foregroundImage.Draw(drawingManagement);
				}
			}
			else
			{
				backgroundImage.Draw(System.Drawing.Color.Tan, 255, drawingManagement);
				if (Selected)
				{
					foregroundImage.Draw(System.Drawing.Color.Tan, 255, drawingManagement);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}
		#endregion
	}

	public class StretchButton : UIElement
	{
		#region Member Variables
		private StretchableImage backgroundImage;
		private StretchableImage foregroundImage;
		private Label label;
		private ToolTip toolTip;
		private InfoTip infoTip;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;
		private bool toolTipEnabled;
		private bool infoTipEnabled;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public StretchButton(List<SpriteName> backgroundSections, List<SpriteName> foregroundSections, string buttonText, int xPos, int yPos, int width, int height)
			: this(backgroundSections, foregroundSections, buttonText, xPos, yPos, width, height, 60, 13)
		{
		}
		public StretchButton(List<SpriteName> backgroundSections, List<SpriteName> foregroundSections, string buttonText, int xPos, int yPos, int width, int height, int sectionWidth, int sectionHeight)
			: base(xPos, yPos, width, height)
		{
			toolTipEnabled = false;
			infoTipEnabled = false;

			backgroundImage = new StretchableImage(xPos, yPos, width, height, sectionWidth, sectionHeight, backgroundSections);
			foregroundImage = new StretchableImage(xPos, yPos, width, height, sectionWidth, sectionHeight, foregroundSections);

			label = new Label(buttonText, 0, 0);
			SetButtonText(buttonText);

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

		public void SetToolTip(List<SpriteName> toolTipSections, GorgonLibrary.Graphics.Font font, string toolTip, string buttonName, int toolTipSectionWidth, int toolTipSectionHeight, int screenWidth, int screenHeight)
		{
			toolTipEnabled = true;
			this.toolTip = new ToolTip(buttonName, toolTip, font, toolTipSections, toolTipSectionWidth, toolTipSectionHeight, screenWidth, screenHeight);
		}

		public void SetInfoTip(string title, List<Icon> icons, Dictionary<string, object> values, List<SpriteName> backgroundImage, int sectionWidth, int sectionHeight, int screenWidth, int screenHeight)
		{
			infoTipEnabled = true;
			this.infoTip = new InfoTip(title, icons, values, backgroundImage, sectionWidth, sectionHeight, screenWidth, screenHeight);
		}

		public void SetButtonText(string text)
		{
			label.SetText(text);
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
		}

		public void SetButtonColor(System.Drawing.Color color)
		{
			label.SetColor(color);
		}

		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
			backgroundImage.MoveTo(x, y);
			foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
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
				if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(true);
					infoTip.MouseHover(x, y, frameDeltaTime);
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
			if (toolTipEnabled)
			{
				toolTip.SetShowing(false);
			}
			if (infoTipEnabled)
			{
				infoTip.SetShowing(false);
			}
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(false);
				}
				if (Active)
				{
					pressed = true;
				}
				return true;
			}
			return false;
		}

		public override bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(false);
				}
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (Active)
			{
				if (pressed)
				{
					foregroundImage.Draw(drawingManagement);
				}
				else if (!Selected)
				{
					backgroundImage.Draw(drawingManagement);
					if (pulse > 0)
					{
						foregroundImage.Draw((byte)(255 * pulse), drawingManagement);
					}
				}
				else
				{
					foregroundImage.Draw(drawingManagement);
				}
			}
			else
			{
				backgroundImage.Draw(System.Drawing.Color.Tan, 255, drawingManagement);
				if (Selected)
				{
					foregroundImage.Draw(System.Drawing.Color.Tan, 255, drawingManagement);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}
		public void DrawTips(DrawingManagement drawingManagement)
		{
			if (toolTipEnabled)
			{
				toolTip.Draw(drawingManagement);
			}
			if (infoTipEnabled)
			{
				infoTip.Draw(drawingManagement);
			}
		}
		#endregion
	}

	//This button is invisible until it's being hovered above or selected
	public class InvisibleStretchButton : UIElement
	{
		#region Member Variables
		private StretchableImage backgroundImage;
		private StretchableImage foregroundImage;
		private Label label;
		private ToolTip toolTip;
		private InfoTip infoTip;

		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;
		private bool visible;

		private bool toolTipEnabled;
		private bool infoTipEnabled;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool Selected { get; set; }
		#endregion

		#region Constructors
		public InvisibleStretchButton(List<SpriteName> backgroundSections, List<SpriteName> foregroundSections, string buttonText, int xPos, int yPos, int width, int height, int sectionWidth, int sectionHeight)
			: this(backgroundSections, foregroundSections, buttonText, xPos, yPos, width, height, sectionWidth, sectionHeight, System.Drawing.Color.Blue)
		{
		}

		public InvisibleStretchButton(List<SpriteName> backgroundSections, List<SpriteName> foregroundSections, string buttonText, int xPos, int yPos, int width, int height, int sectionWidth, int sectionHeight, System.Drawing.Color color)
			: base(xPos, yPos, width, height)
		{
			backgroundImage = new StretchableImage(xPos, yPos, width, height, sectionWidth, sectionHeight, backgroundSections);
			foregroundImage = new StretchableImage(xPos, yPos, width, height, sectionWidth, sectionHeight, foregroundSections);

			label = new Label(buttonText, 0, 0);
			SetButtonText(buttonText);
			label.SetColor(color);
			visible = false;

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
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
		}

		public void SetToolTip(List<SpriteName> toolTipSections, GorgonLibrary.Graphics.Font font, string toolTip, string buttonName, int toolTipSectionWidth, int toolTipSectionHeight, int screenWidth, int screenHeight)
		{
			toolTipEnabled = true;
			this.toolTip = new ToolTip(buttonName, toolTip, font, toolTipSections, toolTipSectionWidth, toolTipSectionHeight, screenWidth, screenHeight);
		}

		public void SetInfoTip(string title, List<Icon> icons, Dictionary<string, object> values, List<SpriteName> backgroundImage, int sectionWidth, int sectionHeight, int screenWidth, int screenHeight)
		{
			infoTipEnabled = true;
			this.infoTip = new InfoTip(title, icons, values, backgroundImage, sectionWidth, sectionHeight, screenWidth, screenHeight);
		}

		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			label.MoveTo((int)((width / 2) - (label.GetWidth() / 2) + xPos), (int)((height / 2) - (label.GetHeight() / 2) + yPos));
			backgroundImage.MoveTo(x, y);
			foregroundImage.MoveTo(x, y);
		}

		public void ResizeButton(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				visible = true;
				if (pulse < 0.3f)
				{
					pulse = 0.9f;
				}
				if (Active)
				{
					if (direction)
					{
						pulse += frameDeltaTime;
						if (pulse > 0.9f)
						{
							direction = !direction;
							pulse = 0.9f;
						}
					}
					else
					{
						pulse -= frameDeltaTime;
						if (pulse < 0.3f)
						{
							direction = !direction;
							pulse = 0.3f;
						}
					}
				}
				if (toolTipEnabled)
				{
					toolTip.SetShowing(true);
					toolTip.MouseHover(x, y, frameDeltaTime);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(true);
					infoTip.MouseHover(x, y, frameDeltaTime);
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
			if (toolTipEnabled)
			{
				toolTip.SetShowing(false);
			}
			if (infoTipEnabled)
			{
				infoTip.SetShowing(false);
			}
			visible = false;
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				if (Active)
				{
					pressed = true;
				}
				if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(false);
				}
				return true;
			}
			return false;
		}

		public override bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (toolTipEnabled)
				{
					toolTip.SetShowing(false);
				}
				if (infoTipEnabled)
				{
					infoTip.SetShowing(false);
				}
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					return true;
				}
			}
			return false;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (Active && (visible || Selected))
			{
				if (pressed)
				{
					foregroundImage.Draw(drawingManagement);
				}
				else if (!Selected)
				{
					backgroundImage.Draw(drawingManagement);
					if (pulse > 0)
					{
						foregroundImage.Draw((byte)(255 * pulse), drawingManagement);
					}
				}
				else
				{
					foregroundImage.Draw(drawingManagement);
				}
			}
			if (label.Text.Length > 0)
			{
				label.Draw();
			}
		}

		public void DrawTips(DrawingManagement drawingManagement)
		{
			if (toolTipEnabled)
			{
				toolTip.Draw(drawingManagement);
			}
			if (infoTipEnabled)
			{
				infoTip.Draw(drawingManagement);
			}
		}
		#endregion
	}

	public class CheckBox : UIElement
	{
		#region Member Variables
		private SpriteName backgroundNonChecked;
		private SpriteName foregroundNonChecked;
		private SpriteName backgroundChecked;
		private SpriteName foregroundChecked;
		private Label text;

		private bool isChecked;
		//Button state information
		private bool pressed;
		private float pulse;
		private bool direction;
		private bool isRadioButton;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public bool IsChecked
		{
			get { return isChecked; }
			set { isChecked = value; }
		}
		#endregion

		#region Constructors
		public CheckBox(List<SpriteName> sprites, string text, int xPos, int yPos, int width, int height, int buttonSize, bool isRadioButton)
			: base(xPos, yPos, width, height)
		{
			this.xPos = xPos;
			this.yPos = yPos;

			backgroundNonChecked = sprites[0];
			foregroundNonChecked = sprites[1];
			backgroundChecked = sprites[2];
			foregroundChecked = sprites[3];

			this.text = new Label(text, 0, 0);

			MoveTo(xPos, yPos);

			this.isRadioButton = isRadioButton;

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
			IsChecked = false;
		}

		public void SetButtonText(string text)
		{
			this.text.SetText(text);
			this.text.MoveTo(xPos + 35, (int)(yPos + (height / 2) - (this.text.GetHeight() / 2)));
		}

		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			text.MoveTo(xPos + 35, (int)(yPos + (height / 2) - (this.text.GetHeight() / 2)));
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
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

		public override bool MouseDown(int x, int y)
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

		public override bool MouseUp(int x, int y)
		{
			if (Active && pressed)
			{
				pressed = false;
				if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
				{
					if (isRadioButton)
					{
						isChecked = true;
					}
					else
					{
						isChecked = !isChecked;
					}
					return true;
				}
			}
			return false;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (Active)
			{
				if (pressed)
				{
					drawingManagement.DrawSprite(isChecked ? foregroundChecked : foregroundNonChecked, xPos + 10, yPos + height / 2 - 9);
				}
				else
				{
					drawingManagement.DrawSprite(isChecked ? backgroundChecked : backgroundNonChecked, xPos + 10, yPos + height / 2 - 9);
					if (pulse > 0)
					{
						drawingManagement.DrawSprite(isChecked ? foregroundChecked : foregroundNonChecked, xPos + 10, yPos + height / 2 - 9, (byte)(255 * pulse));
					}
				}
			}
			else
			{
				drawingManagement.DrawSprite(isChecked ? backgroundChecked : backgroundNonChecked, xPos + 10, yPos + height / 2 - 9, 255, System.Drawing.Color.Tan);
			}
			text.Draw();
		}
		#endregion
	}

	public class NumericUpDown : UIElement
	{
		#region Member Variables
		private Button upButton;
		private Button downButton;
		private int minimum;
		private int maximum;
		private Label valueLabel;
		private int value;
		private int incrementAmount;
		#endregion

		#region Properties
		public int Value
		{
			get { return value; }
		}
		#endregion

		#region Constructors
		public NumericUpDown(int xPos, int yPos, int width, int min, int max, int initialAmount) : base (xPos, yPos, width, 23)
		{
			minimum = min;
			maximum = max;
			value = initialAmount;
			valueLabel = new Label(xPos + 20, yPos, System.Drawing.Color.White);
			CheckAmount();

			upButton = new Button(SpriteName.PlusBackground, SpriteName.PlusForeground, string.Empty, xPos + width - 18, yPos + 2, 16, 16);
			downButton = new Button(SpriteName.MinusBackground, SpriteName.MinusForeground, string.Empty, xPos + 2, yPos + 2, 16, 16);
			incrementAmount = 1;
		}

		public NumericUpDown(int xPos, int yPos, int width, int min, int max, int initialAmount, int incrementAmount)
			: this(xPos, yPos, width, min, max, initialAmount)
		{
			this.incrementAmount = incrementAmount;
		}
		#endregion

		#region Functions
		public override bool MouseUp(int x, int y)
		{
			if (upButton.MouseUp(x, y))
			{
				value += incrementAmount;
				CheckAmount();
				return true;
			}
			if (downButton.MouseUp(x, y))
			{
				value -= incrementAmount;
				CheckAmount();
				return true;
			}
			return false;
		}

		public override bool MouseDown(int x, int y)
		{
			if (upButton.MouseDown(x, y))
			{
				return true;
			}
			if (downButton.MouseDown(x, y))
			{
				return true;
			}
			return false;
		}
		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (upButton.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
			}
			if (downButton.MouseHover(x, y, frameDeltaTime))
			{
				result = true;
			}
			return result;
		}

		public override void MoveTo(int x, int y)
		{
			base.MoveTo(x, y);
			upButton.MoveTo(xPos + width - 18, yPos + 2);
			downButton.MoveTo(xPos + 2, yPos + 2);
			valueLabel.MoveTo(xPos + 20, yPos);
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			upButton.Draw(drawingManagement);
			downButton.Draw(drawingManagement);
			valueLabel.Draw();
		}

		public void SetMin(int min)
		{
			minimum = min;
			CheckAmount();
		}

		public void SetMax(int max)
		{
			maximum = max;
			CheckAmount();
		}

		public void SetValue(int value)
		{
			this.value = value;
			CheckAmount();
		}

		private void CheckAmount()
		{
			if (minimum >= 0 && value < minimum)
			{
				value = minimum;
			}
			if (maximum >= 0 && value > maximum)
			{
				value = maximum;
			}
			valueLabel.SetText(value.ToString());
		}
		#endregion
	}

	public class ComboBoxNoStretch : UIElement
	{
		#region Member Variables
		//ComboBox drawing information
		private SpriteName downArrowSprite;
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
		public int Count
		{
			get { return items.Count; }
		}
		public string CurrentText
		{
			get { return items[selectedIndex]; }
		}
		public bool IsDroppedDown
		{
			get { return dropped; }
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
		public ComboBoxNoStretch(List<SpriteName> sprites, List<string> items, int xPos, int yPos, int width, int height, int maxVisible)
			: base(xPos, yPos, width, height)
		{
			this.items = items;

			dropped = false;
			downArrowSprite = sprites[2];

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
			List<SpriteName> scrollbarSections = new List<SpriteName>()
			{
				sprites[3],
				sprites[4],
				sprites[5],
				sprites[6],
				sprites[7],
				sprites[8],
				sprites[9],
				sprites[10],
				sprites[11],
				sprites[12],
				sprites[13],
				sprites[13]
			};
			RealScrollBar = new ScrollBar(xPos + width, yPos + height, 16, (height * maxVisible) - 32, maxVisible, items.Count, false, false, scrollbarSections);
		}
		#endregion

		#region Functions
		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;

			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].MoveTo(x, y + (i * height));
			}

			RealScrollBar.MoveTo(xPos + width, yPos + height);
		}

		public override void Draw(DrawingManagement drawingManagement)
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
			drawingManagement.DrawSprite(downArrowSprite, xPos + width - 33, yPos + 3, 255, System.Drawing.Color.White);
			if (dropped)
			{
				for (int i = 0; i < buttons.Count - 1; i++)
				{
					buttons[i + 1].SetButtonText(items[RealScrollBar.TopIndex + i]);
					buttons[i + 1].Draw(drawingManagement);
				}
				if (haveScroll)
				{
					RealScrollBar.Draw(drawingManagement);
				}
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (Active)
			{
				if (!dropped)
				{
					if (buttons[0].MouseHover(x, y, frameDeltaTime))
					{
						result = true;
					}
				}
				else
				{
					foreach (Button button in buttons)
					{
						if (button.MouseHover(x, y, frameDeltaTime))
						{
							result = true;
						}
					}
					if (RealScrollBar.MouseHover(x, y, frameDeltaTime))
					{
						result = true;
					}
				}
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
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

		public override bool MouseUp(int x, int y)
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

	public class ComboBox : UIElement
	{
		#region Member Variables
		//ComboBox drawing information
		private SpriteName downArrowSprite;
		private List<string> items;
		private List<StretchButton> buttons;
		private ScrollBar RealScrollBar;

		//ComboBox state information
		private bool dropped;
		private bool haveScroll;
		private int selectedIndex;
		private bool dropDirection;
		private int maxVisible;
		private int actualVisible;
		private List<SpriteName> backgroundSections;
		private List<SpriteName> foregroundSections;
		private int sectionWidth;
		private int sectionHeight;
		#endregion

		#region Properties
		public bool Active { get; set; }
		public int SelectedIndex 
		{ 
			get { return selectedIndex; }
			set { selectedIndex = value; }
		}
		public int Count
		{
			get { return items.Count; }
		}
		public string CurrentText
		{
			get { return items[selectedIndex]; }
		}
		public bool IsDroppedDown
		{
			get { return dropped; }
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
		public ComboBox(List<SpriteName> sprites, List<string> items, int xPos, int yPos, int width, int height, int maxVisible, bool dropDirection) 
			: this(sprites, items, xPos, yPos, width, height, maxVisible, dropDirection, 60, 13)
		{
		}

		public ComboBox(List<SpriteName> sprites, List<string> items, int xPos, int yPos, int width, int height, int maxVisible, bool dropDirection, int sectionWidth, int sectionHeight)
			: base(xPos, yPos, width, height)
		{
			dropped = false;
			this.dropDirection = dropDirection;
			downArrowSprite = sprites[20];

			Active = true;

			backgroundSections = new List<SpriteName>();
			foregroundSections = new List<SpriteName>();
			for (int i = 0; i < 9; i++)
			{
				backgroundSections.Add(sprites[i]);
				foregroundSections.Add(sprites[i + 9]);
			}
			this.sectionWidth = sectionWidth;
			this.sectionHeight = sectionHeight;

			this.maxVisible = maxVisible;
			actualVisible = items.Count < maxVisible ? items.Count : maxVisible;
			haveScroll = items.Count > maxVisible;

			buttons = new List<StretchButton>();
			for (int i = 0; i <= actualVisible; i++)
			{
				StretchButton button = new StretchButton(backgroundSections, foregroundSections, string.Empty, xPos, yPos + (dropDirection ? (i * height) : (i * height * -1)), width, height, sectionWidth, sectionHeight);
				buttons.Add(button);
			}
			List<SpriteName> scrollbarSections = new List<SpriteName>()
			{
				sprites[18],
				sprites[19],
				sprites[20],
				sprites[21],
				sprites[22],
				sprites[23],
				sprites[24],
				sprites[25],
				sprites[26],
				sprites[27],
				sprites[28],
				sprites[28]
			};
			RealScrollBar = new ScrollBar(xPos + width, yPos + height, 16, (height * maxVisible) - 32, maxVisible, items.Count, false, false, scrollbarSections);

			SetItems(items);
		}
		#endregion

		#region Functions
		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;

			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].MoveTo(x, y + (i * height));
			}

			RealScrollBar.MoveTo(xPos + width, yPos + height);
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			
			buttons[0].Draw(drawingManagement);
			drawingManagement.DrawSprite(downArrowSprite, xPos + width - 24, yPos + 8, 255, System.Drawing.Color.White);
			if (dropped)
			{
				for (int i = 0; i < actualVisible; i++)
				{
					buttons[i + 1].SetButtonText(items[RealScrollBar.TopIndex + i]);
					buttons[i + 1].Draw(drawingManagement);
				}
				if (haveScroll)
				{
					RealScrollBar.Draw(drawingManagement);
				}
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			if (Active)
			{
				if (!dropped)
				{
					if (buttons[0].MouseHover(x, y, frameDeltaTime))
					{
						result = true;
					}
				}
				else
				{
					for (int i = 0; i < actualVisible + 1; i++)
					{
						if (buttons[i].MouseHover(x, y, frameDeltaTime))
						{
							result = true;
						}
					}
					if (RealScrollBar.MouseHover(x, y, frameDeltaTime))
					{
						result = true;
					}
				}
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			if (Active)
			{
				if (!dropped)
				{
					return buttons[0].MouseDown(x, y);
				}
				else
				{
					for (int i = 0; i < actualVisible + 1; i++)
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

		public override bool MouseUp(int x, int y)
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
					for (int i = 0; i < actualVisible + 1; i++)
					{
						if (buttons[i].MouseUp(x, y))
						{
							if (i > 0)
							{
								selectedIndex = i + RealScrollBar.TopIndex - 1;
								buttons[0].SetButtonText(items[selectedIndex]);
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

		public void SetItems(List<string> items)
		{
			this.items = items;
			actualVisible = items.Count < maxVisible ? items.Count : maxVisible;
			haveScroll = items.Count > maxVisible;

			buttons = new List<StretchButton>();
			for (int i = 0; i <= actualVisible; i++)
			{
				StretchButton button = new StretchButton(backgroundSections, foregroundSections, string.Empty, xPos, yPos + (dropDirection ? (i * height) : (i * height * -1)), width, height, sectionWidth, sectionHeight);
				buttons.Add(button);
			}
			RealScrollBar.SetAmountOfItems(items.Count);
			if (items.Count <= 0)
			{
				selectedIndex = -1;
				buttons[0].Active = false;
				buttons[0].SetButtonText(string.Empty);
			}
			else
			{
				selectedIndex = 0;
				buttons[0].Active = Active;
				buttons[0].SetButtonText(items[selectedIndex]);
			}
		}
		#endregion
	}

	public class ScrollBar : UIElement
	{
		#region Member Variables
		//Variables that are defined in constructor
		private int scrollSize;
		private Button Up;
		private Button Down;
		private UniStretchButton Scroll;
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
		/// <summary>
		/// Fills out the scrollbar data
		/// </summary>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <param name="scrollSize"></param>
		/// <param name="scrollBarLength"></param>
		/// <param name="amountOfVisibleItems"></param>
		/// <param name="amountOfItems"></param>
		/// <param name="isHorizontal"></param>
		/// <param name="isSlider"></param>
		/// <param name="components">UpBackground, UpForeground, DownBackground, DownForeground, ScrollBackground, ScrollForeground, ScrollBarBackground, ScrollBarForeground</param>
		public ScrollBar(int xPos, int yPos, int scrollSize, int scrollBarLength, int amountOfVisibleItems, int amountOfItems, bool isHorizontal, bool isSlider, List<SpriteName> components) 
			: base(xPos, yPos, 0, 0)
		{
			this.scrollSize = scrollSize;
			this.scrollBarLength = scrollBarLength;
			this.amountOfItems = amountOfItems;
			this.amountVisible = amountOfVisibleItems;
			this.isSlider = isSlider;
			this.isHorizontal = isHorizontal;

			Up = new Button(components[0], components[1], "", xPos, yPos, scrollSize, scrollSize);

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
			this.scrollBar = components[10];
			this.highlightedScrollBar = components[11];

			if (isHorizontal)
			{
				Scroll = new UniStretchButton(new List<SpriteName>() { components[4], components[5], components[6] }, new List<SpriteName>() { components[7], components[8], components[9] }, isHorizontal, "", xPos + scrollSize, yPos, scrollButtonLength, scrollSize);
				Down = new Button(components[2], components[3], "", xPos + scrollBarLength + scrollSize, yPos, scrollSize, scrollSize);
			}
			else
			{
				Scroll = new UniStretchButton(new List<SpriteName>() { components[4], components[5], components[6] }, new List<SpriteName>() { components[7], components[8], components[9] }, isHorizontal, "", xPos, yPos + scrollSize, scrollSize, scrollButtonLength);
				Down = new Button(components[2], components[3], "", xPos, yPos + scrollBarLength + scrollSize, scrollSize, scrollSize);
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
				Scroll.MoveTo(xPos + scrollSize + scrollPos, yPos);
			}
			else
			{
				Scroll.MoveTo(xPos, yPos + scrollSize + scrollPos);
			}
		}
		#endregion

		#region Public Functions
		public override void Draw(DrawingManagement drawingManagement)
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

		public override bool MouseDown(int x, int y)
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

		public override bool MouseUp(int x, int y)
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

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (isEnabled)
			{
				Scroll.MouseHover(x, y, frameDeltaTime);
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
					Up.MouseHover(x, y, frameDeltaTime);
					Down.MouseHover(x, y, frameDeltaTime);
					return false;
				}
			}
			return false;
		}

		public override void MoveTo(int x, int y)
		{
			Up.MoveTo(x, y);
			if (isHorizontal)
			{
				Down.MoveTo(x + scrollBarLength + scrollSize, y);
				Scroll.MoveTo(x + scrollSize + scrollPos, y);
			}
			else
			{
				Down.MoveTo(x, y + scrollBarLength + scrollSize);
				Scroll.MoveTo(x, y + scrollSize + scrollPos);
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

		public void SetAmountVisible(int amount)
		{
			topIndex = 0;
			amountVisible = amount;
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

	public class ProgressBar : UIElement
	{
		#region Member Variables
		private float maxItems;
		private float currentItems;
		private SpriteName barBackground;
		private SpriteName barForeground;
		private int currentWidth;
		private int potentialWidth;
		private System.Drawing.Color color;
		private float potentinalIncrease;
		private System.Drawing.Color potentialIncreaseColor;
		#endregion

		#region Constructor
		public ProgressBar(int xPos, int yPos, int width, int height, int maxItems, int currentItems, SpriteName barBackground, SpriteName barForeground) : base(xPos, yPos, width, height)
		{
			this.maxItems = maxItems;
			this.currentItems = currentItems;

			this.barBackground = barBackground;
			this.barForeground = barForeground;
			color = System.Drawing.Color.White;
			potentinalIncrease = -1;
		}
		public ProgressBar(int xPos, int yPos, int width, int height, int maxItems, int currentItems, SpriteName barBackground, SpriteName barForeground, System.Drawing.Color potentialColor, System.Drawing.Color progressColor) : 
			this(xPos, yPos, width, height, maxItems, currentItems, barBackground, barForeground)
		{
			potentialIncreaseColor = potentialColor;
			color = progressColor;
		}
		#endregion

		#region Functions
		public override void MoveTo(int xPos, int yPos)
		{
			this.xPos = xPos;
			this.yPos = yPos;
		}

		private void UpdateWidth()
		{
			currentWidth = (int)(width * ((double)currentItems / (double)maxItems));
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
			potentialWidth = (int)(width * ((double)potentinalIncrease / (double)maxItems));
			if (currentWidth + potentialWidth > width)
			{
				potentialWidth = (width - currentWidth);
			}
		}

		public void IncrementProgress()
		{
			currentItems++;
			UpdateWidth();
		}

		public void SetProgress(long currentItems)
		{
			this.currentItems = currentItems;
			UpdateWidth();
		}

		public void SetPotentialProgress(long potentialAmount)
		{
			potentinalIncrease = potentialAmount;
			UpdateWidth();
		}

		public void SetMaxProgress(float maxItems)
		{
			this.maxItems = maxItems;
			UpdateWidth();
		}

		public override void Draw(DrawingManagement drawingManagement)
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
		public void SetPotentialColor(System.Drawing.Color color)
		{
			potentialIncreaseColor = color;
		}
		#endregion
	}

	public class AnimatedImage : UIElement
	{
		#region Member Variables
		private List<SpriteName> frames;
		private int frameIter;
		private bool reverse;
		private bool direction;
		private float frameTick;
		private float frameLength;
		private float rotation;
		private float scaleX;
		private float scaleY;
		#endregion

		#region Constructor
		#endregion

		public AnimatedImage(int x, int y, int width, int height, List<SpriteName> frames, bool reverse, float frameLength) :
			base (x, y, width, height)
		{
			this.frames = frames;
			this.frameLength = frameLength;
			this.reverse = reverse;
			rotation = 0;
			frameTick = 0;
			frameIter = 0;
			scaleX = 1;
			scaleY = 1;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, 255, drawingManagement);
		}

		public void Draw(byte alpha, DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, alpha, drawingManagement);
		}

		public void Draw(System.Drawing.Color color, byte alpha, DrawingManagement drawingManagement)
		{
			drawingManagement.GetSprite(frames[frameIter]).Rotation = rotation;
			drawingManagement.DrawSprite(frames[frameIter], xPos, yPos, alpha, scaleX, scaleY, color);
		}

		public void SetRotation(float rotation)
		{
			this.rotation = rotation;
		}

		public void SetScale(float scaleX, float scaleY)
		{
			this.scaleX = scaleX;
			this.scaleY = scaleY;
		}

		public void Update(float frameDeltaTime)
		{
			frameTick += frameDeltaTime;
			if (frameTick >= frameLength)
			{
				if (!direction)
				{
					frameIter++;
					if (frameIter >= frames.Count)
					{
						if (reverse)
						{
							direction = !direction;
							frameIter--;
						}
						else
						{
							frameIter = 0;
						}
					}
				}
				else
				{
					frameIter--;
					if (frameIter < 0)
					{
						direction = !direction;
						frameIter++;
					}
				}
				frameTick -= frameLength;
			}
		}
	}

	public class StretchableImage : UIElement
	{
		#region Member Variables
		private int sectionWidth;
		private int sectionHeight;
		private int horizontalStretchLength;
		private int verticalStretchLength;
		private List<SpriteName> sections;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public StretchableImage(int x, int y, int width, int height, int sectionWidth, int sectionHeight, List<SpriteName> sections) :
			base(x, y, width, height)
		{
			this.sectionWidth = sectionWidth;
			this.sectionHeight = sectionHeight;
			this.sections = new List<SpriteName>();
			foreach (SpriteName spriteName in sections)
			{
				this.sections.Add(spriteName);
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
		}
		#endregion

		#region Functions
		public override void Draw(DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, 255, drawingManagement);
		}

		public void Draw(byte alpha, DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, alpha, drawingManagement);
		}

		public void Draw(System.Drawing.Color color, byte alpha, DrawingManagement drawingManagement)
		{
			drawingManagement.DrawSprite(sections[0], xPos, yPos, alpha, color);
			drawingManagement.DrawSprite(sections[1], xPos + sectionWidth, yPos, alpha, horizontalStretchLength, sectionHeight, color);
			drawingManagement.DrawSprite(sections[2], xPos + sectionWidth + horizontalStretchLength, yPos, alpha, color);
			drawingManagement.DrawSprite(sections[3], xPos, yPos + sectionHeight, alpha, sectionWidth, verticalStretchLength, color);
			drawingManagement.DrawSprite(sections[4], xPos + sectionWidth, yPos + sectionHeight, alpha, horizontalStretchLength, verticalStretchLength, color);
			drawingManagement.DrawSprite(sections[5], xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight, alpha, sectionWidth, verticalStretchLength, color);
			drawingManagement.DrawSprite(sections[6], xPos, yPos + sectionHeight + verticalStretchLength, alpha, color);
			drawingManagement.DrawSprite(sections[7], xPos + sectionWidth, yPos + sectionHeight + verticalStretchLength, alpha, horizontalStretchLength, sectionHeight, color);
			drawingManagement.DrawSprite(sections[8], xPos + sectionWidth + horizontalStretchLength, yPos + sectionHeight + verticalStretchLength, alpha, color);
		}
		public void SetDimensions(int width, int height)
		{
			this.width = width;
			this.height = height;
			horizontalStretchLength = (width - (sectionWidth * 2));
			verticalStretchLength = (height - (sectionHeight * 2));
		}
		#endregion
	}

	public class UniStretchableImage : UIElement
	{
		#region Member Variables
		private int mainLength;
		private int stretchLength;
		private int stretchAmount;
		private List<SpriteName> sections;
		private bool isHorizontal;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public UniStretchableImage(int x, int y, int width, int height, int mainLength, int stretchLength, bool isHorizontal, List<SpriteName> sections) :
			base(x, y, width, height)
		{
			this.mainLength = mainLength;
			this.stretchLength = stretchLength;
			this.sections = new List<SpriteName>();
			foreach (SpriteName spriteName in sections)
			{
				this.sections.Add(spriteName);
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
		}
		#endregion

		#region Functions
		public override void Draw(DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, 255, drawingManagement);
		}

		public void Draw(byte alpha, DrawingManagement drawingManagement)
		{
			Draw(System.Drawing.Color.White, alpha, drawingManagement);
		}

		public void Draw(System.Drawing.Color color, byte alpha, DrawingManagement drawingManagement)
		{
			drawingManagement.DrawSprite(sections[0], xPos, yPos, alpha, color);
			if (isHorizontal)
			{
				drawingManagement.DrawSprite(sections[1], xPos + mainLength, yPos, alpha, stretchAmount, height, color);
				drawingManagement.DrawSprite(sections[2], xPos + mainLength + stretchAmount, yPos, alpha, color);
			}
			else
			{
				drawingManagement.DrawSprite(sections[1], xPos, yPos + mainLength, alpha, width, stretchAmount, color);
				drawingManagement.DrawSprite(sections[2], xPos, yPos + mainLength + stretchAmount, alpha, color);
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
		#endregion
	}

	public class Label : UIElement
	{
		#region Member Variables
		private string label;
		private bool isRightAligned;
		private GorgonLibrary.Graphics.TextSprite textSprite;
		private System.Drawing.Color color;
		#endregion

		#region Properties
		public string Text { get; private set; }
		#endregion

		#region Constructor
		public Label(int x, int y) : base(x, y, 0, 0)
		{
			color = System.Drawing.Color.White;
			SetText(string.Empty);
		}
		public Label(string label, int x, int y)
			: base(x, y, 0, 0)
		{
			color = System.Drawing.Color.White;
			SetText(label);
		}
		public Label(int x, int y, System.Drawing.Color color)
			: base(x, y, 0, 0)
		{
			SetText(string.Empty);
			this.color = color;
		}
		public Label(string label, int x, int y, System.Drawing.Color color)
			: base(x, y, 0, 0)
		{
			this.color = color;
			SetText(label);
		}
		#endregion

		#region Functions
		public void SetText(string label)
		{
			this.label = label;

			GorgonLibrary.Graphics.Font font;
			if (DrawingManagement.fonts.TryGetValue("Computer", out font))
			{
				textSprite = new GorgonLibrary.Graphics.TextSprite("Computer", label, font);
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
					textSprite.SetPosition(xPos - textSprite.Width, yPos);
				}
				else
				{
					textSprite.SetPosition(xPos, yPos);
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
		public override void MoveTo(int x, int y)
		{
			xPos = x;
			yPos = y;
			textSprite.SetPosition(x, y);
		}
		public void SetColor(System.Drawing.Color color)
		{
			this.color = color;
		}
		public override void Draw(DrawingManagement drawingManagement)
		{
			Draw();
		}
		public void Draw()
		{
			textSprite.Color = color;
			textSprite.Draw();
		}
		#endregion
	}

	public class TextBox : UIElement
	{
		#region Member Variables
		private GorgonLibrary.Graphics.Viewport wrapView;
		private GorgonLibrary.Graphics.TextSprite textSprite;
		//private StretchableImage background;
		private ScrollBar textScrollBar;
		//private int borderWidth;
		//private int borderHeight;
		private GorgonLibrary.Graphics.RenderImage target;
		private bool scrollbarVisible;
		private bool usingScrollBar;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		public TextBox(int xPos, int yPos, int width, int height, /*int sectionWidth, int sectionHeight, int borderWidth, int borderHeight,*/ string name, string message, GorgonLibrary.Graphics.Font font, /*List<SpriteName> sections,*/ List<SpriteName> scrollBarSprites)
			: base(xPos, yPos, width, height)
		{
			//this.borderWidth = borderWidth;
			//this.borderHeight = borderHeight;
			//background = new StretchableImage(xPos, yPos, width, height, sectionWidth, sectionHeight, sections);
			textScrollBar = new ScrollBar(xPos + width - 16, yPos, 16, height - 32, height, 1, false, false, scrollBarSprites);

			//Set the text stuff
			wrapView = new GorgonLibrary.Graphics.Viewport(0, 0, width - 16, height);
			textSprite = new GorgonLibrary.Graphics.TextSprite(name, string.Empty, font);
			textSprite.WordWrap = true;
			textSprite.Bounds = wrapView;

			target = new GorgonLibrary.Graphics.RenderImage(name + "render", width - 16, height, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			target.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;
			usingScrollBar = true;

			SetMessage(message);

			textScrollBar.SetAmountOfItems((int)textSprite.Height);
		}
		//No scrollbar, everything will be visible
		public TextBox(int xPos, int yPos, int width, int height, string name, string message, GorgonLibrary.Graphics.Font font)
			: base(xPos, yPos, width, height)
		{
			wrapView = new GorgonLibrary.Graphics.Viewport(0, 0, width, height);
			textSprite = new GorgonLibrary.Graphics.TextSprite(name, string.Empty, font);
			textSprite.WordWrap = true;
			textSprite.Bounds = wrapView;

			target = new GorgonLibrary.Graphics.RenderImage(name + "Render", width, height, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
			target.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;
			usingScrollBar = false;

			SetMessage(message);
			this.height = (int)textSprite.Height;
		}
		#endregion

		#region Functions
		public void SetMessage(string message)
		{
			textSprite.Text = message;
			if (usingScrollBar)
			{
				textScrollBar.TopIndex = 0;
				if (textSprite.Height > height)
				{
					scrollbarVisible = true;
					textScrollBar.SetAmountOfItems((int)textSprite.Height);
				}
				else
				{
					scrollbarVisible = false;
				}
			}
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			//background.Draw(drawingManagement);
			if (usingScrollBar && scrollbarVisible)
			{
				textScrollBar.Draw(drawingManagement);
			}

			target.Clear(System.Drawing.Color.FromArgb(0, 0, 0, 0));
			GorgonLibrary.Graphics.RenderTarget old = GorgonLibrary.Gorgon.CurrentRenderTarget;
			GorgonLibrary.Gorgon.CurrentRenderTarget = target;
			textSprite.SetPosition(0, usingScrollBar ? -textScrollBar.TopIndex : 0);
			textSprite.Draw();
			GorgonLibrary.Gorgon.CurrentRenderTarget = old;
			target.Blit(xPos, yPos);
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = base.MouseDown(x, y);

			if (usingScrollBar && scrollbarVisible)
			{
				result = textScrollBar.MouseDown(x, y) || result;
			}

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = base.MouseUp(x, y);

			if (usingScrollBar && scrollbarVisible)
			{
				result = textScrollBar.MouseUp(x, y) || result;
			}

			return result;
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = base.MouseHover(x, y, frameDeltaTime);

			if (usingScrollBar && scrollbarVisible)
			{
				result = textScrollBar.MouseHover(x, y, frameDeltaTime) || result;
			}

			return result;
		}

		public override void MoveTo(int x, int y)
		{
			base.MoveTo(x, y);
			if (usingScrollBar)
			{
				textScrollBar.MoveTo(xPos + width - 16, yPos);
			}
		}

		public int GetTextBoxHeight()
		{
			return (int)textSprite.Height;
		}
		public int GetTextBoxWidth()
		{
			return (int)textSprite.Width;
		}
		#endregion
	}

	public class SingleLineTextBox : UIElement
	{
		#region Member Variables
		private Label text;
		private string actualText;
		private StretchableImage background;

		private bool isSelected;
		private bool pressed;
		private bool blink;
		private float timer;
		#endregion

		#region Constructors
		public SingleLineTextBox(int x, int y, int width, int height, List<SpriteName> components)
			: base(x, y, width, height)
		{
			this.background = new StretchableImage(x, y, width, height, 30, 13, components);
			actualText = string.Empty;
			text = new Label(string.Empty, x + 6, y + 7);
			pressed = false;
			isSelected = false;
			blink = true;
		}

		public SingleLineTextBox(string text, int x, int y, int width, int height, List<SpriteName> components)
			: base(x, y, width, height)
		{
			this.background = new StretchableImage(x, y, width, height, 30, 13, components);
			actualText = text;
			this.text = new Label(text, x + 6, y + 7);
			pressed = false;
			isSelected = false;
			blink = false;
		}
		#endregion

		#region Functions
		public override bool MouseDown(int x, int y)
		{
			if (x >= xPos && x < xPos + width && y >= yPos && y < yPos + height)
			{
				pressed = true;
				return true;
			}
			return false;
		}

		public override bool MouseUp(int x, int y)
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

		public override void MoveTo(int x, int y)
		{
			base.MoveTo(x, y);
			background.MoveTo(x, y);
			text.MoveTo(x + 6, y + 7);
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			background.Draw(drawingManagement);
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

	public class ToolTip : UIElement
	{
		#region Member Variables
		private const int WIDTH = 250;

		private StretchableImage background;
		private TextBox textBox;

		private float delayBeforeShowing;
		private bool showing;

		private int screenWidth;
		private int screenHeight;

		private int textHeight;
		private int textWidth;
		private int sectionWidth;
		private int sectionHeight;
		#endregion

		public ToolTip(string name, string message, GorgonLibrary.Graphics.Font font, List<SpriteName> backgroundImage, int sectionWidth, int sectionHeight, int screenWidth, int screenHeight)
			: base(0, 0, WIDTH, 45)
		{
			textBox = new TextBox(sectionWidth, sectionHeight, WIDTH - sectionWidth, 400, name, message, font);
			textHeight = textBox.GetTextBoxHeight() + sectionHeight;
			textWidth = Math.Min(textBox.GetTextBoxWidth() + sectionWidth, WIDTH);
			background = new StretchableImage(0, 0, textWidth, textHeight, sectionWidth, sectionHeight, backgroundImage);

			showing = false;
			delayBeforeShowing = 0; //1 second will turn on showing

			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;

			this.sectionWidth = sectionWidth / 2;
			this.sectionHeight = sectionHeight / 2;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (showing && delayBeforeShowing >= 1)
			{
				background.Draw(drawingManagement);
				textBox.Draw(drawingManagement);
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (showing)
			{
				int modifiedX = 0;
				int modifiedY = 0;
				if (x < screenWidth - textWidth)
				{
					modifiedX = x;
				}
				else
				{
					modifiedX = x - textWidth;
				}
				if (y < screenHeight - textHeight)
				{
					modifiedY = y;
				}
				else
				{
					modifiedY = y - textHeight;
				}
				background.MoveTo(modifiedX, modifiedY);
				textBox.MoveTo(modifiedX + sectionWidth, modifiedY + sectionHeight);

				if (delayBeforeShowing < 1.0)
				{
					delayBeforeShowing += frameDeltaTime;
				}
			}
			else
			{
				delayBeforeShowing = 0;
			}
			return true;
		}

		public void SetShowing(bool showing)
		{
			this.showing = showing;
			if (!showing)
			{
				delayBeforeShowing = 0;
			}
		}
		public void SetText(string message)
		{
			textBox.SetMessage(message);
			textHeight = textBox.GetTextBoxHeight() + (sectionHeight * 2);
			textWidth = Math.Min(textBox.GetTextBoxWidth() + (sectionWidth * 2), WIDTH);
			background.SetDimensions(textWidth, textHeight);
		}
	}

	public class InfoTip : UIElement
	{
		#region Member Variables
		private const int WIDTH = 250;

		private StretchableImage background;
		private Label title;
		private List<Icon> icons;

		private bool showing;

		private int screenWidth;
		private int screenHeight;

		private int totalHeight;
		private int sectionWidth;
		private int sectionHeight;
		#endregion

		public InfoTip(string title, List<Icon> icons, Dictionary<string, object> values, List<SpriteName> backgroundImage, int sectionWidth, int sectionHeight, int screenWidth, int screenHeight)
			: base(0, 0, WIDTH, 45)
		{
			this.title = new Label(title, 0, 0);
			this.icons = new List<Icon>();

			foreach (Icon icon in icons)
			{
				Icon newIcon = new Icon(icon);
				newIcon.UpdateText(values);
				this.icons.Add(newIcon);
			}

			totalHeight = 25 + (icons.Count * 24);

			background = new StretchableImage(0, 0, WIDTH, totalHeight + sectionHeight, sectionWidth, sectionHeight, backgroundImage);

			showing = false;

			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;

			this.sectionWidth = sectionWidth / 2;
			this.sectionHeight = sectionHeight / 2;
		}

		public override void Draw(DrawingManagement drawingManagement)
		{
			if (showing)
			{
				background.Draw(drawingManagement);
				title.Draw();
				for (int i = 0; i < icons.Count; i++)
				{
					icons[i].Draw(xPos + 10, yPos + (i * 24) + 20 + sectionHeight, 250, 24, drawingManagement);
				}
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			int modifiedX = 0;
			int modifiedY = 0;
			if (x < screenWidth - WIDTH)
			{
				modifiedX = x;
			}
			else
			{
				modifiedX = x - WIDTH;
			}
			if (y < screenHeight - totalHeight)
			{
				modifiedY = y;
			}
			else
			{
				modifiedY = y - totalHeight;
			}
			background.MoveTo(modifiedX, modifiedY);
			title.MoveTo(modifiedX + sectionWidth, modifiedY + sectionHeight);
			xPos = modifiedX;
			yPos = modifiedY;
			return true;
		}

		public void SetShowing(bool showing)
		{
			this.showing = showing;
		}
	}
}
