namespace Beyond_Beyaan.Data_Modules
{
	public abstract class BaseControl
	{
		public string Name { get; protected set; }
		public string Code { get; protected set; }
		public Label NameLabel { get; protected set; }
		public Label ValueLabel { get; protected set; }

		protected BaseControl(string name, string code)
		{
			Name = name;
			Code = code;
			NameLabel = new Label(Name + ": ", 0, 0);
			ValueLabel = new Label(0, 0);
			ValueLabel.SetAlignment(true);
		}

		public abstract object GetValue();

		//Draw the control
		public abstract void Draw(int x, int y, int width, int height, DrawingManagement drawingManagement);

		//Draw the name and value alone
		public void Draw(int x, int y, int width, int height)
		{
			NameLabel.MoveTo(x + 2, y + (height / 2) - (int)(NameLabel.GetHeight() / 2));
			NameLabel.Draw();

			ValueLabel.MoveTo(x + width - 2, y + (height / 2) - (int)(ValueLabel.GetHeight() / 2));
			ValueLabel.Draw();
		}

		//Handle Mouse Updates
		public abstract void MouseHover(int mouseX, int mouseY, int x, int y, int width, int height);
		public abstract void MouseDown(int mouseX, int mouseY, int x, int y, int width, int height);
		public abstract void MouseUp(int mouseX, int mouseY, int x, int y, int width, int height);
	}

	public class NumUpDown : BaseControl
	{
		public int Value { get; private set; }
		public int MinValue { get; private set; }
		public int MaxValue { get; private set; }
		public int IncrAmount { get; private set; }

		//private Button upButton;
		//private Button downButton;

		public NumUpDown(int minValue, int maxValue, int incrAmount, int startValue, string name, string code) 
			: base (name, code)
		{
			Value = startValue;
			MinValue = minValue;
			MaxValue = maxValue;
			IncrAmount = incrAmount;

			ValueLabel.SetText(Value.ToString());
		}

		public override object GetValue()
		{
			return Value;
		}

		public override void Draw(int x, int y, int width, int height, DrawingManagement drawingManagement)
		{
			
		}

		public override void MouseHover(int mouseX, int mouseY, int x, int y, int width, int height)
		{
			
		}

		public override void MouseDown(int mouseX, int mouseY, int x, int y, int width, int height)
		{
			
		}

		public override void MouseUp(int mouseX, int mouseY, int x, int y, int width, int height)
		{
			
		}
	}
}
