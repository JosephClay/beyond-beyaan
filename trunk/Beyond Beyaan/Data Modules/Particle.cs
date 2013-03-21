using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	public enum AnimationType { LOOP, MIRROR, ONCE, RANDOM }
	public enum WrappingMode { NONE, VERTICAL, HORIZONTAL, BOTH }
	public class Particle
	{
		private string name;
		private List<Frame> frames;
		public int NumOfFrames { get { return frames.Count; } }
		public float FrameDelay { get; private set; }
		//private AnimationType animationType;
		private int frameWidth;
		private int frameHeight;
		private int axisX;
		private int axisY;
		private WrappingMode wrappingMode;
		public ParticleScript Script { get; private set; }
		private Dictionary<string, string> values;

		public Particle(XElement particle, string graphicPath, string particleScriptPath)
		{
			frames = new List<Frame>();
			values = new Dictionary<string, string>();
			//Load the attributes
			foreach (XAttribute attribute in particle.Attributes())
			{
				values.Add(attribute.Name.LocalName, attribute.Value);
			}

			name = values["name"];
			frameWidth = int.Parse(values["frameWidth"]);
			frameHeight = int.Parse(values["frameHeight"]);
			axisX = int.Parse(values["axisX"]);
			axisY = int.Parse(values["axisY"]);
			switch (values["wrapping"].ToLower())
			{
				case "none": wrappingMode = WrappingMode.NONE;
					break;
				case "vertical": wrappingMode = WrappingMode.VERTICAL;
					break;
				case "horizontal": wrappingMode = WrappingMode.HORIZONTAL;
					break;
				case "both": wrappingMode = WrappingMode.BOTH;
					break;
				default: wrappingMode = WrappingMode.NONE;
					break;
			}
			/*switch (particle.Attribute("animation").Value.ToLower())
			{
				case "loop": animationType = AnimationType.LOOP;
					break;
				case "mirror": animationType = AnimationType.MIRROR;
					break;
				case "random": animationType = AnimationType.RANDOM;
					break;
				default: animationType = AnimationType.LOOP;
					break;
			}*/
			FrameDelay = int.Parse(values["frameDelay"]) / 1000f;

			if (values.ContainsKey("script"))
			{
				Script = new ParticleScript(new FileInfo(Path.Combine(particleScriptPath, values["script"] + ".cs")));
			}
			if (values.ContainsKey("file"))
			{
				//new GorgonLibrary.Graphics.Sprite("MainParticleGraphic", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicDirectory, "particles.png")));
				GorgonLibrary.Graphics.Sprite particleGraphic = new GorgonLibrary.Graphics.Sprite(values["file"] + ".png", GorgonLibrary.Graphics.Image.FromFile(Path.Combine(graphicPath, values["file"] + ".png")));
				int iter = 0;
				foreach (XElement element in particle.Elements())
				{
					frames.Add(new Frame(element, frameWidth, frameHeight, axisX, axisY, wrappingMode, particleGraphic, name, iter));
				}
			}
			/*values.Remove("name");
			values.Remove("frameWidth");
			values.Remove("frameHeight");
			values.Remove("axisX");
			values.Remove("axisY");
			values.Remove("wrapping");
			values.Remove("frameDelay");
			values.Remove("file");
			values.Remove("script");
			values.Remove("animation");*/
		}

		public void Draw(int x, int y, int width, int height, float angle, float scale, int frame)
		{
			frames[frame].Draw(x, y, width, height, angle, scale);
		}

		public Dictionary<string, object> AddValues(Dictionary<string, object> currentValues)
		{
			foreach (KeyValuePair<string, string> item in values)
			{
				if (!currentValues.ContainsKey(item.Key))
				{
					currentValues.Add(item.Key, item.Value);
				}
				//If it exists, don't replace, as it may be overriden by other scripts
			}
			return currentValues;
		}
	}
	public class Frame
	{
		private GorgonLibrary.Graphics.Sprite frameSprite;
		//private int width;
		//private int height;
		private WrappingMode wrappingMode;

		public Frame(XElement frame, int width, int height, int axisX, int axisY, WrappingMode wrappingMode, GorgonLibrary.Graphics.Sprite particleGraphic, string name, int iter)
		{
			int xPos = int.Parse(frame.Attribute("xPos").Value);
			int yPos = int.Parse(frame.Attribute("yPos").Value);

			//this.width = width;
			//this.height = height;
			this.wrappingMode = wrappingMode;

			frameSprite = new GorgonLibrary.Graphics.Sprite(name + iter, particleGraphic.Image, xPos, yPos, width, height);
			switch (wrappingMode)
			{
				case WrappingMode.HORIZONTAL: frameSprite.HorizontalWrapMode = GorgonLibrary.Graphics.ImageAddressing.Wrapping;
					break;
				case WrappingMode.VERTICAL: frameSprite.VerticalWrapMode = GorgonLibrary.Graphics.ImageAddressing.Wrapping;
					break;
				case WrappingMode.BOTH: frameSprite.WrapMode = GorgonLibrary.Graphics.ImageAddressing.Wrapping;
					break;
			}
			frameSprite.SetAxis(axisX, axisY);
		}

		public void Draw(int x, int y, float width, float height, float angle, float scale)
		{
			frameSprite.Position = new GorgonLibrary.Vector2D(x, y);
			//frameSprite.ScaledWidth = this.width * scale;
			//frameSprite.ScaledHeight = this.height * scale;
			switch(wrappingMode)
			{
				case WrappingMode.BOTH:
					{
						frameSprite.Width = width * scale;
						frameSprite.Height = height * scale;
					} break;
				case WrappingMode.HORIZONTAL:
					{
						frameSprite.Width = width * scale;
						frameSprite.ScaledHeight = height * scale;
					} break;
				case WrappingMode.VERTICAL:
					{
						frameSprite.ScaledWidth = width * scale;
						frameSprite.Height = height * scale;
					} break;
				default:
					{
						frameSprite.ScaledWidth = width * scale;
						frameSprite.ScaledHeight = height * scale;
					} break;
			}
			frameSprite.Rotation = angle;
			frameSprite.Draw();
		}
	}
	public class ParticleInstance
	{
		private Particle whichParticle;
		public Dictionary<string, object> Values { get; private set; }
		public ShipInstance ShipToIgnore { get; private set; } //If firing from a ship, don't impact that ship's equipment (shields, etc)
		private float frameDelay;
		private int frame;

		//public Empire friendlyIdentifier { get; private set; } //For any special handling

		public ParticleInstance(Particle whichParticle, ShipInstance ship, Dictionary<string, object> values)
		{
			this.whichParticle = whichParticle;
			this.Values = new Dictionary<string, object>(values);
			ShipToIgnore = ship;
			this.Values = whichParticle.AddValues(this.Values);
			this.Values = whichParticle.Script.Spawn(this.Values);
		}

		public Dictionary<string, object>[] Update(float frameDeltaTime)
		{
			Dictionary<string, object>[] result = whichParticle.Script.Update(Values, frameDeltaTime);
			Values = result[0];
			frameDelay += frameDeltaTime;
			while (frameDelay >= whichParticle.FrameDelay)
			{
				frameDelay -= whichParticle.FrameDelay;
				frame++;
				if (frame >= whichParticle.NumOfFrames)
				{
					frame = 0;
				}

			}
			return result;
		}

		public Dictionary<string, object>[] PostHit(Dictionary<string, object> newValues, int impactX, int impactY)
		{
			Dictionary<string, object>[] result = whichParticle.Script.PostHit(newValues, impactX, impactY);
			Values = result[0];
			return result;
		}

		public void Draw(int xOffset, int yOffset, float cameraScale)
		{
			int x = (int)(((float)Values["PosX"] * cameraScale) - xOffset);
			int y = (int)(((float)Values["PosY"] * cameraScale) - yOffset);
			float angle = (float)((float)Values["Angle"] * (180.0 / Math.PI));

			int width = (int)((float)Values["Width"] * cameraScale);
			int height = (int)((float)Values["Height"] * cameraScale);
			whichParticle.Draw(x, y, width, height, angle, cameraScale, frame);
		}
	}
}
