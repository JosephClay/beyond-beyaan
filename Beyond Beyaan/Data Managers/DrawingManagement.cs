using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Drawing = System.Drawing;
using GorgonLibrary;
using GorgonLibrary.Graphics;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan
{
	public enum SpriteName 
	{ 
		Interface,
		Buttons,
		HumanCpu,
		ScrollNSlider,
		Stars,
		Planets,
		Message,
		TitlePlanet,
		TitleName,
		TitleNebula,
		Continue,
		Customize,
		Exit,
		LoadGame,
		NewGame,
		Options,
		Continue2,
		Customize2,
		Exit2,
		LoadGame2,
		NewGame2,
		Options2,
		BattleItems,

		NormalBackgroundButton,
		NormalForegroundButton,
		MiniBackgroundButton,
		MiniForegroundButton,
		UpBackgroundButton,
		UpForegroundButton,
		DownBackgroundButton,
		DownForegroundButton,
		GridCell00,
		GridCell01,
		GridCell02,
		GridCell03,
		GridCell04,
		GridCell05,
		GridCell06,
		GridCell07,
		GridCell08,
		GridCell09,
		GridCell10,
		GridCell11,
		GridCell12,
		GridCell13,
		GridCell14,
		GridCell15,
		MapStar,
		Star,
		SelectedStar,
		Fleet,
		SelectedFleet,
		BlackHole,
		Screen,
		ScrollUpBackgroundButton,
		ScrollUpForegroundButton,
		ScrollVerticalBar,
		ScrollDownBackgroundButton,
		ScrollDownForegroundButton,
		ScrollVerticalBackgroundButton,
		ScrollVerticalForegroundButton,
		ScrollLeftBackgroundButton,
		ScrollLeftForegroundButton,
		ScrollHorizontalBar,
		ScrollRightBackgroundButton,
		ScrollRightForegroundButton,
		ScrollHorizontalBackgroundButton,
		ScrollHorizontalForegroundButton,
		SliderHorizontalBar,
		SliderHighlightedHorizontalBar,
		SliderHorizontalBackgroundButton,
		SliderHorizontalForegroundButton,
		ControlBackground,
		SelectCell,
		Nebula,
		GameMenu,
		HighlightedGameMenu,
		Diplomacy,
		HighlightedDiplomacy,
		FleetList,
		HighlightedFleetList,
		Design,
		HighlightedDesign,
		DesignList,
		HighlightedDesignList,
		PlanetsList,
		HighlightedPlanetsList,
		Research,
		HighlightedResearch,
		EOT,
		HighlightedEOT,
		Galaxy,
		HighlightedGalaxy,
		BeamIcon,
		ParticleIcon,
		MissileIcon,
		TorpedoIcon,
		BombIcon,
		CancelBackground,
		CancelForeground,
		PlusBackground,
		PlusForeground,
		MinusBackground,
		MinusForeground,
		HumanPlayerIcon,
		CPUPlayerIcon,
		LockDisabled,
		LockEnabled,
		Terran,
		Jungle,
		Ocean,
		Badlands,
		Steppe,
		Desert,
		Arctic,
		Barren,
		Tundra,
		Dead,
		Volcanic,
		Toxic,
		Radiated,
		Asteroids,
		GasGiant,
		PlanetIcons,
		AgricultureIcon,
		EnvironmentIcon,
		CommerceIcon,
		ResearchIcon,
		ConstructionIcon,
		PlanetConstructionBonus1,
		PlanetConstructionBonus2,
		PlanetConstructionBonus3,
		PlanetConstructionBonus4,
		PlanetEnvironmentBonus1,
		PlanetEnvironmentBonus2,
		PlanetEnvironmentBonus3,
		PlanetEnvironmentBonus4,
		PlanetEntertainmentBonus1,
		PlanetEntertainmentBonus2,
		PlanetEntertainmentBonus3,
		PlanetEntertainmentBonus4,
		Spy,
		Security,
		Relation,
		RelationBar,
		RelationSlider,
		BarSlider,
		IncomingMessageBackground,
		IncomingMessageForeground,
		OutgoingMessageBackground,
		OutgoingMessageForeground,
		EmpireTurnArrow,
		ShipSelection32,
		ShipSelection64,
		ShipSelection96,
		ShipSelection128,
		ShipSelection160,
		AutoBackground,
		AutoForeground,
		RetreatBackground,
		RetreatForeground,
		PrevShipBackground,
		PrevShipForeground,
		DonePrevShipBackground,
		DonePrevShipForeground,
		DoneNextShipBackground,
		DoneNextShipForeground,
		NextShipBackground,
		NextShipForeground,
	};

	public class DrawingManagement
	{
		#region Member Variables
		private Dictionary<SpriteName, Sprite> sprites;
		public static Dictionary<string, Font> fonts;
		#endregion

		#region Constructors
		public DrawingManagement()
		{
			sprites = new Dictionary<SpriteName, Sprite>();
			fonts = new Dictionary<string, Font>();
		}
		#endregion

		#region Private Functions
		private bool LoadSpriteFromFile(string name, SpriteName spriteName, string filepath, bool colorkey, out string reason)
		{
			if (!File.Exists(filepath))
			{
				reason = "File " + filepath + " not found";
				return false;
			}
			try
			{
				Sprite newSprite;
				if (colorkey)
				{
					newSprite = new Sprite(name, GorgonLibrary.Graphics.Image.FromFile(filepath, ImageBufferFormats.BufferUnknown, Drawing.Color.Black));
				}
				else
				{
					newSprite = new Sprite(name, GorgonLibrary.Graphics.Image.FromFile(filepath));
				}
				sprites.Add(spriteName, newSprite);
				reason = null;
				return true;
			}
			catch (Exception exception)
			{
				reason = "Exception in loading " + filepath + "\r\nReason: " + exception.Message;
				return false;
			}
		}

		private bool LoadSpriteFromSprite(string name, SpriteName spriteName, SpriteName sourceSprite, int x, int y, int width, int height, bool colorkey, bool smoothing, out string reason)
		{
			try
			{
				Sprite newSprite = new Sprite(name, sprites[sourceSprite].Image, x, y, width, height);
				if (smoothing)
				{
					newSprite.Smoothing = Smoothing.Smooth;
				}
				sprites.Add(spriteName, newSprite);
				reason = null;
				return true;
			}
			catch (Exception exception)
			{
				reason = "Exception in loading sprite from another sprite. Reason: " + exception.Message;
				return false;
			}
		}

		public bool LoadGraphics(string directory, out string reason)
		{
			if (!AddFont("Arial", "Arial", 10.0f, false, out reason))
			{
				MessageBox.Show(reason);
			}
			//Load from file
			if (!LoadSpriteFromFile("InterfaceArt", SpriteName.Interface, directory + "\\InterfaceArt.png", true, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("Buttons", SpriteName.Buttons, directory + "\\buttons.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("HumanCpu", SpriteName.HumanCpu, directory + "\\human cpu.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("ScrollNSlider", SpriteName.ScrollNSlider, directory + "\\Scroll and slider.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("Stars", SpriteName.Stars, directory + "\\Stars old.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("Planets", SpriteName.Planets, directory + "\\planets.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("PlanetIcons", SpriteName.PlanetIcons, directory + "\\icon 2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("BarSlider", SpriteName.BarSlider, directory + "\\bar slider.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("RelationSlider", SpriteName.RelationSlider, directory + "\\bar mark 25.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("Message", SpriteName.Message, directory + "\\button message.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitlePlanet", SpriteName.TitlePlanet, directory + "\\Title planet.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitleName", SpriteName.TitleName, directory + "\\Title name.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitleNebula", SpriteName.TitleNebula, directory + "\\Title nebula.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CONTINUE", SpriteName.Continue, directory + "\\bot CONTINUE.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CUSTOMIZE", SpriteName.Customize, directory + "\\bot CUSTOMIZE.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("EXIT", SpriteName.Exit, directory + "\\bot EXIT.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("load game", SpriteName.LoadGame, directory + "\\bot load game.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("new game", SpriteName.NewGame, directory + "\\bot new game.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("OPTIONS", SpriteName.Options, directory + "\\bot OPTIONS.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CONTINUE2", SpriteName.Continue2, directory + "\\bot CONTINUE2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CUSTOMIZE2", SpriteName.Customize2, directory + "\\bot CUSTOMIZE2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("EXIT2", SpriteName.Exit2, directory + "\\bot EXIT2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("load game2", SpriteName.LoadGame2, directory + "\\bot load game2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("new game2", SpriteName.NewGame2, directory + "\\bot new game2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("OPTIONS2", SpriteName.Options2, directory + "\\bot OPTIONS2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("BattleItems", SpriteName.BattleItems, directory + "\\battleItems.png", true, out reason))
			{
				return false;
			}

			//Load from loaded sprites
			if (!LoadSpriteFromSprite("SelectCell", SpriteName.SelectCell, SpriteName.Interface, 175, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NormalBackgroundButton", SpriteName.NormalBackgroundButton, SpriteName.Interface, 0, 0, 175, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NormalForegroundButton", SpriteName.NormalForegroundButton, SpriteName.Interface, 0, 40, 175, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MiniBackgroundButton", SpriteName.MiniBackgroundButton, SpriteName.Interface, 0, 344, 175, 25, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MiniForegroundButton", SpriteName.MiniForegroundButton, SpriteName.Interface, 0, 369, 175, 25, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("UpBackgroundButton", SpriteName.UpBackgroundButton, SpriteName.Interface, 0, 280, 175, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("UpForegroundButton", SpriteName.UpForegroundButton, SpriteName.Interface, 0, 296, 175, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DownBackgroundButton", SpriteName.DownBackgroundButton, SpriteName.Interface, 0, 312, 175, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DownForegroundButton", SpriteName.DownForegroundButton, SpriteName.Interface, 0, 328, 175, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MapStar", SpriteName.MapStar, SpriteName.Interface, 239, 0, 9, 9, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GridCell00", SpriteName.GridCell00, SpriteName.Interface, 175, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("Screen", SpriteName.Screen, directory + "\\Screen.png", false, out reason))
			{
				return false;
			}


			if (!LoadSpriteFromSprite("ScrollUpBackgroundButton", SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollNSlider, 0, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollUpForegroundButton", SpriteName.ScrollUpForegroundButton, SpriteName.ScrollNSlider, 16, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBar", SpriteName.ScrollVerticalBar, SpriteName.ScrollNSlider, 0, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollDownBackgroundButton", SpriteName.ScrollDownBackgroundButton, SpriteName.ScrollNSlider, 0, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollDownForegroundButton", SpriteName.ScrollDownForegroundButton, SpriteName.ScrollNSlider, 16, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBackgroundButton", SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollNSlider, 0, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalForegroundButton", SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollNSlider, 16, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollLeftBackgroundButton", SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollNSlider, 32, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollLeftForegroundButton", SpriteName.ScrollLeftForegroundButton, SpriteName.ScrollNSlider, 48, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBar", SpriteName.ScrollHorizontalBar, SpriteName.ScrollNSlider, 32, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollRightBackgroundButton", SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollNSlider, 32, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollRightForegroundButton", SpriteName.ScrollRightForegroundButton, SpriteName.ScrollNSlider, 48, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBackgroundButton", SpriteName.ScrollHorizontalBackgroundButton, SpriteName.ScrollNSlider, 32, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalForegroundButton", SpriteName.ScrollHorizontalForegroundButton, SpriteName.ScrollNSlider, 48, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelBackground", SpriteName.CancelBackground, SpriteName.ScrollNSlider, 0, 64, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelForeground", SpriteName.CancelForeground, SpriteName.ScrollNSlider, 0, 80, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlusBackground", SpriteName.PlusBackground, SpriteName.ScrollNSlider, 64, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlusForeground", SpriteName.PlusForeground, SpriteName.ScrollNSlider, 64, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MinusBackground", SpriteName.MinusBackground, SpriteName.ScrollNSlider, 64, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MinusForeground", SpriteName.MinusForeground, SpriteName.ScrollNSlider, 64, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LockDisabled", SpriteName.LockDisabled, SpriteName.ScrollNSlider, 80, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LockEnabled", SpriteName.LockEnabled, SpriteName.ScrollNSlider, 80, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBar", SpriteName.SliderHorizontalBar, SpriteName.ScrollNSlider, 16, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHighlightedHorizontalBar", SpriteName.SliderHighlightedHorizontalBar, SpriteName.ScrollNSlider, 32, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBackgroundButton", SpriteName.SliderHorizontalBackgroundButton, SpriteName.ScrollNSlider, 48, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalForegroundButton", SpriteName.SliderHorizontalForegroundButton, SpriteName.ScrollNSlider, 64, 64, 16, 16, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("SelectedSystem", SpriteName.SelectedStar, SpriteName.Stars, 0, 96, 128, 128, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Star", SpriteName.Star, SpriteName.Stars, 32, 32, 64, 64, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BlackHole", SpriteName.BlackHole, SpriteName.Stars, 160, 128, 64, 64, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Fleet", SpriteName.Fleet, SpriteName.Stars, 128, 32, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SelectedFleet", SpriteName.SelectedFleet, SpriteName.Stars, 160, 0, 96, 96, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("ControlBackground", SpriteName.ControlBackground, SpriteName.Interface, 0, 80, 175, 200, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Nebula", SpriteName.Nebula, SpriteName.SelectCell, 223, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GameMenu", SpriteName.GameMenu, SpriteName.Buttons, 0, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Galaxy", SpriteName.Galaxy, SpriteName.Buttons, 40, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Diplomacy", SpriteName.Diplomacy, SpriteName.Buttons, 80, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetList", SpriteName.FleetList, SpriteName.Buttons, 120, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetsList", SpriteName.PlanetsList, SpriteName.Buttons, 160, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Design", SpriteName.Design, SpriteName.Buttons, 200, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Research", SpriteName.Research, SpriteName.Buttons, 240, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignList", SpriteName.DesignList, SpriteName.Buttons, 280, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EOT", SpriteName.EOT, SpriteName.Buttons, 320, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GameMenuHighlighted", SpriteName.HighlightedGameMenu, SpriteName.Buttons, 0, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GalaxyHighlighted", SpriteName.HighlightedGalaxy, SpriteName.Buttons, 40, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DiplomacyHighlighted", SpriteName.HighlightedDiplomacy, SpriteName.Buttons, 80, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetListHighlighted", SpriteName.HighlightedFleetList, SpriteName.Buttons, 120, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetsListHighlighted", SpriteName.HighlightedPlanetsList, SpriteName.Buttons, 160, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignHighlighted", SpriteName.HighlightedDesign, SpriteName.Buttons, 200, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ResearchHighlighted", SpriteName.HighlightedResearch, SpriteName.Buttons, 240, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignListHighlighted", SpriteName.HighlightedDesignList, SpriteName.Buttons, 280, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EOTHighlighted", SpriteName.HighlightedEOT, SpriteName.Buttons, 320, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BeamIcon", SpriteName.BeamIcon, SpriteName.Interface, 40, 474, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ParticleIcon", SpriteName.ParticleIcon, SpriteName.Interface, 56, 474, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MissileIcon", SpriteName.MissileIcon, SpriteName.Interface, 72, 474, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TorpedoIcon", SpriteName.TorpedoIcon, SpriteName.Interface, 88, 474, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BombIcon", SpriteName.BombIcon, SpriteName.Interface, 104, 474, 16, 16, true, false, out reason))
			{
				return false;
			}
			
			if (!LoadSpriteFromSprite("HumanPlayerIcon", SpriteName.HumanPlayerIcon, SpriteName.HumanCpu, 0, 0, 25, 25, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CPUPlayerIcon", SpriteName.CPUPlayerIcon, SpriteName.HumanCpu, 25, 0, 25, 25, true, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("Terran", SpriteName.Terran, SpriteName.Planets, 0, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Jungle", SpriteName.Jungle, SpriteName.Planets, 40, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Ocean", SpriteName.Ocean, SpriteName.Planets, 80, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Badlands", SpriteName.Badlands, SpriteName.Planets, 120, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Steppe", SpriteName.Steppe, SpriteName.Planets, 160, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Desert", SpriteName.Desert, SpriteName.Planets, 200, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Arctic", SpriteName.Arctic, SpriteName.Planets, 240, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Barren", SpriteName.Barren, SpriteName.Planets, 280, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Tundra", SpriteName.Tundra, SpriteName.Planets, 320, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Dead", SpriteName.Dead, SpriteName.Planets, 360, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Volcanic", SpriteName.Volcanic, SpriteName.Planets, 0, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Toxic", SpriteName.Toxic, SpriteName.Planets, 40, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Radiated", SpriteName.Radiated, SpriteName.Planets, 80, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Asteroids", SpriteName.Asteroids, SpriteName.Planets, 160, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GasGiant", SpriteName.GasGiant, SpriteName.Planets, 120, 40, 40, 40, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("AgricultureIcon", SpriteName.AgricultureIcon, SpriteName.PlanetIcons, 0, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EnvironmentIcon", SpriteName.EnvironmentIcon, SpriteName.PlanetIcons, 16, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CommerceIcon", SpriteName.CommerceIcon, SpriteName.PlanetIcons, 32, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ResearchIcon", SpriteName.ResearchIcon, SpriteName.PlanetIcons, 48, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ConstructionIcon", SpriteName.ConstructionIcon, SpriteName.PlanetIcons, 64, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus1", SpriteName.PlanetConstructionBonus1, SpriteName.PlanetIcons, 80, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus2", SpriteName.PlanetConstructionBonus2, SpriteName.PlanetIcons, 80, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus3", SpriteName.PlanetConstructionBonus3, SpriteName.PlanetIcons, 80, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus4", SpriteName.PlanetConstructionBonus4, SpriteName.PlanetIcons, 80, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus1", SpriteName.PlanetEnvironmentBonus1, SpriteName.PlanetIcons, 112, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus2", SpriteName.PlanetEnvironmentBonus2, SpriteName.PlanetIcons, 112, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus3", SpriteName.PlanetEnvironmentBonus3, SpriteName.PlanetIcons, 112, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus4", SpriteName.PlanetEnvironmentBonus4, SpriteName.PlanetIcons, 112, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus1", SpriteName.PlanetEntertainmentBonus1, SpriteName.PlanetIcons, 144, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus2", SpriteName.PlanetEntertainmentBonus2, SpriteName.PlanetIcons, 144, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus3", SpriteName.PlanetEntertainmentBonus3, SpriteName.PlanetIcons, 144, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus4", SpriteName.PlanetEntertainmentBonus4, SpriteName.PlanetIcons, 144, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Spy", SpriteName.Spy, SpriteName.PlanetIcons, 0, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Security", SpriteName.Security, SpriteName.PlanetIcons, 16, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Relation", SpriteName.Relation, SpriteName.PlanetIcons, 32, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RelationBar", SpriteName.RelationBar, SpriteName.BarSlider, 0, 0, 200, 16, false, false, out reason))
			{
				return false;
			}
			/*if (!LoadSpriteFromSprite("RelationSlider", SpriteName.RelationSlider, SpriteName.BarSlider, 200, 0, 16, 16, false, false, out reason))
			{
				return false;
			}*/
			if (!LoadSpriteFromSprite("OutgoingMessageBackground", SpriteName.OutgoingMessageBackground, SpriteName.Message, 80, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("OutgoingMessageForeground", SpriteName.OutgoingMessageForeground, SpriteName.Message, 80, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IncomingMessageBackground", SpriteName.IncomingMessageBackground, SpriteName.Message, 40, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IncomingMessageForeground", SpriteName.IncomingMessageForeground, SpriteName.Message, 40, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EmpireTurnArrow", SpriteName.EmpireTurnArrow, SpriteName.BattleItems, 32, 0, 25, 25, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelection32", SpriteName.ShipSelection32, SpriteName.BattleItems, 0, 0, 32, 32, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelection64", SpriteName.ShipSelection64, SpriteName.BattleItems, 0, 32, 64, 64, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelection96", SpriteName.ShipSelection96, SpriteName.BattleItems, 0, 96, 96, 96, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelection128", SpriteName.ShipSelection128, SpriteName.BattleItems, 0, 192, 128, 128, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelection160", SpriteName.ShipSelection160, SpriteName.BattleItems, 0, 320, 160, 160, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AutoForeground", SpriteName.AutoForeground, SpriteName.BattleItems, 455, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AutoBackground", SpriteName.AutoBackground, SpriteName.BattleItems, 455, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RetreatForeground", SpriteName.RetreatForeground, SpriteName.BattleItems, 495, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RetreatBackground", SpriteName.RetreatBackground, SpriteName.BattleItems, 495, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PrevShipForeground", SpriteName.PrevShipForeground, SpriteName.BattleItems, 535, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PrevShipBackground", SpriteName.PrevShipBackground, SpriteName.BattleItems, 535, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DonePrevShipForeground", SpriteName.DonePrevShipForeground, SpriteName.BattleItems, 575, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DonePrevShipBackground", SpriteName.DonePrevShipBackground, SpriteName.BattleItems, 575, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DoneNextShipForeground", SpriteName.DoneNextShipForeground, SpriteName.BattleItems, 615, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DoneNextShipBackground", SpriteName.DoneNextShipBackground, SpriteName.BattleItems, 615, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NextShipForeground", SpriteName.NextShipForeground, SpriteName.BattleItems, 655, 0, 40, 40, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NextShipBackground", SpriteName.NextShipBackground, SpriteName.BattleItems, 655, 40, 40, 40, true, false, out reason))
			{
				return false;
			}
			return true;
		}

		private bool AddFont(string name, string fontFamily, float size, bool bold, out string reason)
		{
			if (fonts.ContainsKey(name))
			{
				reason = "Key " + name + " already exists";
				return false;
			}
			reason = null;
			Font font = new Font(name, fontFamily, size, true, bold);
			fonts.Add(name, font);
			return true;
		}
		#endregion

		#region Public Functions
		public void DrawSprite(SpriteName spriteName, int x, int y, byte alpha, Drawing.Color color)
		{
			sprites[spriteName].Color = Drawing.Color.FromArgb(alpha, color);
			sprites[spriteName].SetPosition(x, y);
			sprites[spriteName].Draw();
		}
		/*public void DrawSprite(SpriteName spriteName, int x, int y, byte alpha, float scale, Drawing.Color color)
		{
			sprites[spriteName].SetScale(scale, scale);
			sprites[spriteName].Color = Drawing.Color.FromArgb(alpha, color);
			sprites[spriteName].SetPosition(x, y);
			sprites[spriteName].Draw();
			sprites[spriteName].SetScale(1, 1);
		}*/
		public void DrawSprite(SpriteName spriteName, int x, int y, byte alpha, float width, float height, Drawing.Color color)
		{
			sprites[spriteName].SetScale(width / sprites[spriteName].Width, height / sprites[spriteName].Height);
			sprites[spriteName].Color = Drawing.Color.FromArgb(alpha, color);
			sprites[spriteName].SetPosition(x, y);
			sprites[spriteName].Draw();
			sprites[spriteName].SetScale(1, 1);
		}
		public void DrawSprite(SpriteName spriteName, int x, int y, byte alpha, float width, float height, bool flipped, Drawing.Color color)
		{
			sprites[spriteName].HorizontalFlip = flipped;
			DrawSprite(spriteName, x, y, alpha, width, height, color);
			sprites[spriteName].HorizontalFlip = false;
		}
		public void SetSpriteScale(SpriteName spriteName, float width, float height)
		{
			sprites[spriteName].SetScale(width / sprites[spriteName].Width, height / sprites[spriteName].Height);
		}
		public Sprite GetSprite(SpriteName spriteName)
		{
			return sprites[spriteName];
		}

		#region Texts/Fonts
		public bool DrawText(string name, string text, float x, float y, Drawing.Color color)
		{
			Font font;
			if (fonts.TryGetValue(name, out font))
			{
				TextSprite textSprite = new TextSprite(name, text, font, x, y);
				textSprite.Color = color;
				textSprite.Draw();
				return true;
			}
			return false;
		}
		#endregion
		#endregion
	}
}
