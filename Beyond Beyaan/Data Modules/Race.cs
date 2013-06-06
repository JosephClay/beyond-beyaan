using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GorgonLibrary.Graphics;

namespace Beyond_Beyaan.Data_Modules
{
	public enum Expression { PLEASED, NEUTRAL, ANNOYED }
	class Race
	{
		public float AgricultureMultipler { get; private set; }
		public float WasteMultipler { get; private set; }
		public float CommerceMultipler { get; private set; }
		public float ResearchMultipler { get; private set; }
		public float ConstructionMultipler { get; private set; }

		public float EngineMultipler { get; private set; }
		public float ArmorMultipler { get; private set; }
		public float ShieldMultipler { get; private set; }
		public float BeamMultipler { get; private set; }
		public float ParticleMultipler { get; private set; }
		public float MissileMultipler { get; private set; }
		public float TorpedoMultipler { get; private set; }
		public float BombMultipler { get; private set; }

		public float GrowthMultipler { get; private set; }
		public float SpyMultipler { get; private set; }
		public float SpyDefenseMultipler { get; private set; }
		public float AccuracyMultipler { get; private set; }
		public float CharismaMultipler { get; private set; }

		public string RaceName { get; private set; }

		private List<string> shipNames;
		private Sprite raceGraphic;
		private Sprite neutralAvatar;
		private Sprite annoyedAvatar;
		private Sprite pleasedAvatar;
		private Sprite miniAvatar;
		private Sprite[][] ships;

		public bool Initialize(FileInfo file)
		{
			List<string> errors = new List<string>();
			shipNames = new List<string>();
			string[] lines;

			try
			{
				lines = File.ReadAllLines(file.FullName);
			}
			catch (Exception e)
			{
				errors.Add("Exception occured: " + e.Message);
				return false;
			}

			SetBaseDefault();

			//A simple checksum
			int iter = 0;
			float total = 0;

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
				{
					continue;
				}
				string[] parts = line.Split(new[] { '|' });
				if (parts.Length == 2)
				{
					if (string.Compare(parts[0], "name", true) == 0)
					{
						RaceName = parts[1];
						continue;
					}
					if (string.Compare(parts[0], "shipnames") == 0)
					{
						string[] names = parts[1].Split(new[] { ',' });
						foreach (string name in names)
						{
							shipNames.Add(name);
						}
						continue;
					}
					if (string.Compare(parts[0], "graphic") == 0)
					{
						string graphic = Path.Combine(file.DirectoryName, parts[1]);
						if (!File.Exists(graphic))
						{
							errors.Add("Graphic file " + parts[1] + " does not exist");
							break;
						}
						if (string.IsNullOrEmpty(RaceName))
						{
							errors.Add("Race name must be listed before graphic file");
							break;
						}
						if (ImageCache.Images.Contains(RaceName))
						{
							errors.Add("Duplicate race name: " + RaceName);
							break;
						}
						raceGraphic = new Sprite(RaceName, GorgonLibrary.Graphics.Image.FromFile(graphic));
						if (raceGraphic.Width != 1024 || raceGraphic.Height != 1024)
						{
							errors.Add("Graphic file " + parts[1] + " is not 1024x1024 pixels");
							break;
						}
						continue;
					}

					float value = 1.0f;
					if (!float.TryParse(parts[1], out value))
					{
						errors.Add("Trying to parse " + parts[0] + " failed because " + parts[1] + " is not a valid float value");
						break;
					}
					
					iter++;
					total += value;
					
					switch (parts[0].ToLower())
					{
						case "agriculture":
							AgricultureMultipler = value;
							break;
						case "waste":
							WasteMultipler = value;
							break;
						case "commerce":
							CommerceMultipler = value;
							break;
						case "research":
							ResearchMultipler = value;
							break;
						case "construction":
							ConstructionMultipler = value;
							break;
						case "engine":
							EngineMultipler = value;
							break;
						case "armor":
							ArmorMultipler = value;
							break;
						case "shield":
							ShieldMultipler = value;
							break;
						case "beam":
							BeamMultipler = value;
							break;
						case "particle":
							ParticleMultipler = value;
							break;
						case "missile":
							MissileMultipler = value;
							break;
						case "torpedo":
							TorpedoMultipler = value;
							break;
						case "bomb":
							BombMultipler = value;
							break;
						case "growth":
							GrowthMultipler = value;
							break;
						case "spy":
							SpyMultipler = value;
							break;
						case "spydefense":
							SpyDefenseMultipler = value;
							break;
						case "accuracy":
							AccuracyMultipler = value;
							break;
						case "charisma":
							CharismaMultipler = value;
							break;
					}
				}
				else
				{
					errors.Add("\"" + line + "\" is not a valid line.  Must have two values, separated by |");
					break;
				}
			}
			//We need to remove the floating rounding errors
			total *= 10;
			total += 0.5f; //round up
			iter *= 10;
			if ((int)total - iter != 0)
			{
				errors.Add("Race is not balanced, ensure that all values add up to the same amount of values.  If 5 values are modified, they must add up to 5.0");
			}
			if (string.IsNullOrEmpty(RaceName))
			{
				errors.Add("Race name cannot be blank");
			}
			string reason;
			if (!LoadSpritesFromGraphic(out reason))
			{
				errors.Add(reason);
			}

			return errors.Count == 0;
		}

		private void SetBaseDefault()
		{
			AgricultureMultipler = 1.0f;
			WasteMultipler = 1.0f;
			CommerceMultipler = 1.0f;
			ResearchMultipler = 1.0f;
			ConstructionMultipler = 1.0f;
			EngineMultipler = 1.0f;
			ArmorMultipler = 1.0f;
			ShieldMultipler = 1.0f;
			BeamMultipler = 1.0f;
			ParticleMultipler = 1.0f;
			MissileMultipler = 1.0f;
			TorpedoMultipler = 1.0f;
			BombMultipler = 1.0f;
			GrowthMultipler = 1.0f;
			SpyMultipler = 1.0f;
			SpyDefenseMultipler = 1.0f;
			AccuracyMultipler = 1.0f;
			CharismaMultipler = 1.0f;
		}

		public string GetRandomShipName()
		{
			//This attempts to get a random name from list of ship names.  If no such list exists, it uses a random generator
			if (shipNames.Count > 0)
			{
				Random r = new Random();
				return shipNames[r.Next(shipNames.Count)];
			}

			NameGenerator nameGenerator = new NameGenerator();
			return nameGenerator.GetName();
		}

		private bool LoadSpritesFromGraphic(out string reason)
		{
			try
			{
				miniAvatar = new Sprite(RaceName + "miniAvatar", raceGraphic.Image, 0, 781, 128, 128);
				neutralAvatar = new Sprite(RaceName + "neutralAvatar", raceGraphic.Image, 0, 481, 300, 300);
				pleasedAvatar = new Sprite(RaceName + "pleasedAvatar", raceGraphic.Image, 300, 481, 300, 300);
				annoyedAvatar = new Sprite(RaceName + "annoyedAvatar", raceGraphic.Image, 600, 481, 300, 300);

				ships = new Sprite[5][];
				for (int i = 0; i < ships.Length; i++)
				{
					ships[i] = new Sprite[6];
					int y = 0;
					int iter = 0;
					int size = (i + 1) * 32;
					while (iter <= i)
					{
						y += (32 * iter);
						iter++;
					}
					for (int j = 0; j < ships[i].Length; j++)
					{
						int x = j * size;
						ships[i][j] = new Sprite(RaceName + "ship" + i.ToString() + j.ToString(), raceGraphic.Image, x, y, size, size);
					}
				}
			}
			catch (Exception exception)
			{
				reason = "Exception in loading sprite from another sprite. Reason: " + exception.Message;
				return false;
			}
			reason = null;
			return true;
		}

		public Sprite GetShip(int size, int whichStyle)
		{
			return ships[(size - 1) / 2][whichStyle];
		}

		public Sprite GetMiniAvatar()
		{
			return miniAvatar;
		}

		public Sprite GetAvatar(Expression whichExpression)
		{
			switch (whichExpression)
			{
				case Expression.ANNOYED:
					return annoyedAvatar;
				case Expression.PLEASED:
					return pleasedAvatar;
			}
			return neutralAvatar;
		}

		public string GetRandomEmperorName()
		{
			//There will be a list of emperor names, or unique name generator, but for now, just use random name generator
			NameGenerator nameGenerator = new NameGenerator();
			return nameGenerator.GetName();
		}
	}
}
