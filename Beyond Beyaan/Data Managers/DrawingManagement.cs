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
		Main,
		Interface,
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
		Mod,
		Mod2,
		ModDown,
		ModMain,
		ModScrollUp,
		ModScrollUpHighlighted,
		ModScrollDown,
		ModScrollDownHighlighted,
		ModScrollBar,
		ModScrollButton1,
		ModScrollButton2,
		ModScrollButton3,
		ModScrollButton1Highlighted,
		ModScrollButton2Highlighted,
		ModScrollButton3Highlighted,

		NormalBackgroundButton,
		NormalForegroundButton,
		MiniBackgroundButton,
		MiniForegroundButton,
/*		GridCell00,
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
		GridCell15,*/
		BGStar1,
		BGStar2,
		BGStar3,
		BGStar4,
		BGStar5,
		BGStar6,
		BGStar7,
		Star,
		SelectedStar,
		Fleet,
		Transport,
		SystemShips,
		SelectedFleet,
		BlackHole,
		Screen,
		ScrollUpBackgroundButton,
		ScrollUpForegroundButton,
		ScrollVerticalBar,
		ScrollDownBackgroundButton,
		ScrollDownForegroundButton,
		ScrollVerticalBackgroundButton1,
		ScrollVerticalForegroundButton1,
		ScrollVerticalBackgroundButton2,
		ScrollVerticalForegroundButton2,
		ScrollVerticalBackgroundButton3,
		ScrollVerticalForegroundButton3,
		ScrollLeftBackgroundButton,
		ScrollLeftForegroundButton,
		ScrollHorizontalBar,
		ScrollRightBackgroundButton,
		ScrollRightForegroundButton,
		ScrollHorizontalBackgroundButton1,
		ScrollHorizontalForegroundButton1,
		ScrollHorizontalBackgroundButton2,
		ScrollHorizontalForegroundButton2,
		ScrollHorizontalBackgroundButton3,
		ScrollHorizontalForegroundButton3,
		SliderHorizontalBar,
		SliderHighlightedHorizontalBar,
		SliderHorizontalBackgroundButton1,
		SliderHorizontalForegroundButton1,
		SliderHorizontalBackgroundButton2,
		SliderHorizontalForegroundButton2,
		SliderHorizontalBackgroundButton3,
		SliderHorizontalForegroundButton3,
		//ControlBackground,
		SelectCell,
		GameMenu,
		HighlightedGameMenu,
		Diplomacy,
		HighlightedDiplomacy,
		FleetList,
		HighlightedFleetList,
		Design,
		HighlightedDesign,
		ProductionList,
		HighlightedDesignList,
		PlanetsList,
		HighlightedPlanetsList,
		Research,
		HighlightedResearch,
		EOT,
		HighlightedEOT,
		Galaxy,
		HighlightedGalaxy,
		Blank,
		HighlightedBlank,
		BeamIcon,
		ProjectileIcon,
		MissileIcon,
		TorpedoIcon,
		BombIcon,
		ShieldIcon,
		ArmorIcon,
		SystemEngineIcon,
		StellarEngineIcon,
		ComputerIcon,
		InfrastructureIcon,
		ShockwaveIcon,
		AccuracyIcon,
		DamageIcon,
		MoneyIcon,
		SpaceIcon,
		MountIcon,
		AmmoIcon,
		TimeIcon,
		PlanetResearchIcon,
		TradedResearchIcon,
		TotalResearchIcon,
		CapacityIcon,
		PowerIcon,
		SpecialIcon,
		GalaxyMovementIcon,
		CombatMovementIcon,
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
		RegionRing,
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
		/*EmpireTurnArrow,
		ShipSelection32,
		ShipSelection64,
		ShipSelection96,
		ShipSelection128,
		ShipSelection160,*/
		TextTL,
		TextTC,
		TextTR,
		TextCL,
		TextCC,
		TextCR,
		TextBL,
		TextBC,
		TextBR,
		ButtonBGTL,
		ButtonBGTC,
		ButtonBGTR,
		ButtonBGCL,
		ButtonBGCC,
		ButtonBGCR,
		ButtonBGBL,
		ButtonBGBC,
		ButtonBGBR,
		ButtonFGTL,
		ButtonFGTC,
		ButtonFGTR,
		ButtonFGCL,
		ButtonFGCC,
		ButtonFGCR,
		ButtonFGBL,
		ButtonFGBC,
		ButtonFGBR,
		TinyButtonBGTL,
		TinyButtonBGTC,
		TinyButtonBGTR,
		TinyButtonBGCL,
		TinyButtonBGCC,
		TinyButtonBGCR,
		TinyButtonBGBL,
		TinyButtonBGBC,
		TinyButtonBGBR,
		TinyButtonFGTL,
		TinyButtonFGTC,
		TinyButtonFGTR,
		TinyButtonFGCL,
		TinyButtonFGCC,
		TinyButtonFGCR,
		TinyButtonFGBL,
		TinyButtonFGBC,
		TinyButtonFGBR,
		CBUBG, //CheckBoxUncheckedBackGround
		CBUFG, //CheckBoxUncheckedForeGround
		CBCBG, //CheckBoxCheckedBackGround
		CBCFG, //CheckBoxCheckedForeGround
		RBUBG, //RadioBoxUncheckedBackGround
		RBUFG, //RadioBoxUncheckedForeGround
		RBCBG, //RadioBoxCheckedBackGround
		RBCFG, //RadioBoxCheckedForeGround
		BorderUL,
		BorderUC,
		BorderUR,
		BorderCL,
		BorderCC,
		BorderCR,
		BorderBL,
		BorderBC,
		BorderBR,
		BorderULT,
		BorderUCT,
		BorderURT,
		BorderCLT,
		BorderCCT,
		BorderCRT,
		BorderBLT,
		BorderBCT,
		BorderBRT,
		BoxUL,
		BoxUC,
		BoxUR,
		BoxCL,
		BoxCC,
		BoxCR,
		BoxBL,
		BoxBC,
		BoxBR,
		BoxULT,
		BoxUCT,
		BoxURT,
		BoxCLT,
		BoxCCT,
		BoxCRT,
		BoxBLT,
		BoxBCT,
		BoxBRT,
		BoxULTHL,
		BoxUCTHL,
		BoxURTHL,
		BoxCLTHL,
		BoxCCTHL,
		BoxCRTHL,
		BoxBLTHL,
		BoxBCTHL,
		BoxBRTHL,
		ScreenUL,
		ScreenUC,
		ScreenUR,
		ScreenCL,
		ScreenCC,
		ScreenCR,
		ScreenBL,
		ScreenBC,
		ScreenBR,
		FleetDest1,
		FleetDest2,
		FleetDest3,
		FleetDest4,
		TransferButtonBG,
		TransferButtonFG,
		LandTroopButtonBG,
		LandTroopButtonFG,
		OrbitButtonBG,
		OrbitButtonFG,
		NewSquadronButtonBG,
		NewSquadronButtonFG,
		CancelMovementBG,
		CancelMovementFG,
		Stargate1,
		Stargate2,
		Stargate3,
		Stargate4,
		IconButtonUL,
		IconButtonUC,
		IconButtonUR,
		IconButtonCL,
		IconButtonCC,
		IconButtonCR,
		IconButtonBL,
		IconButtonBC,
		IconButtonBR,
		IconButtonULHL,
		IconButtonUCHL,
		IconButtonURHL,
		IconButtonCLHL,
		IconButtonCCHL,
		IconButtonCRHL,
		IconButtonBLHL,
		IconButtonBCHL,
		IconButtonBRHL,
		EquipmentOK,
		EquipmentOKHL,
		EquipmentCancel,
		EquipmentCancelHL,
		AddEquipment,
		AddEquipmentHL,
		ColonizeButtonBG,
		ColonizeButtonFG,
		PlanetDoneButtonBG,
		PlanetDoneButtonFG,
		ApplyButtonBG,
		ApplyButtonFG,
		AddProjectBG,
		AddProjectFG,
		AddProjectsBG,
		AddProjectsFG,
		DoneProjectBG,
		DoneProjectFG,

		PrevPageBG,
		NextPageBG,
		FirstPageBG,
		LastPageBG,
		PrevPageFG,
		NextPageFG,
		FirstPageFG,
		LastPageFG,

		BattleBackground,
		BattleAutoFG,
		BattleRetreatFG,
		BattlePrevShipFG,
		BattlePrevShipDoneFG,
		BattleNextShipDoneFG,
		BattleNextShipFG,
		BattleNextTurnFG,
		BattleAutoBG,
		BattleRetreatBG,
		BattlePrevShipBG,
		BattlePrevShipDoneBG,
		BattleNextShipDoneBG,
		BattleNextShipBG,
		BattleNextTurnBG,

		ShipSelectionTL,
		ShipSelectionTR,
		ShipSelectionBL,
		ShipSelectionBR,
		ShipSelectionBG,
		ShipMove,
		ShipInvalidMove,
		TargetReticle,
	};

	public class DrawingManagement
	{
		#region Static Variables
		public static List<SpriteName> VerticalScrollBar = new List<SpriteName>()
		{
			SpriteName.ScrollUpBackgroundButton,
			SpriteName.ScrollUpForegroundButton,
			SpriteName.ScrollDownBackgroundButton,
			SpriteName.ScrollDownForegroundButton,
			SpriteName.ScrollVerticalBackgroundButton1,
			SpriteName.ScrollVerticalBackgroundButton2,
			SpriteName.ScrollVerticalBackgroundButton3,
			SpriteName.ScrollVerticalForegroundButton1,
			SpriteName.ScrollVerticalForegroundButton2,
			SpriteName.ScrollVerticalForegroundButton3,
			SpriteName.ScrollVerticalBar,
			SpriteName.ScrollVerticalBar
		};
		public static List<SpriteName> HorizontalScrollBar = new List<SpriteName>()
		{
			SpriteName.ScrollLeftBackgroundButton,
			SpriteName.ScrollLeftForegroundButton,
			SpriteName.ScrollRightBackgroundButton,
			SpriteName.ScrollRightForegroundButton,
			SpriteName.ScrollHorizontalBackgroundButton1,
			SpriteName.ScrollHorizontalBackgroundButton2,
			SpriteName.ScrollHorizontalBackgroundButton3,
			SpriteName.ScrollHorizontalForegroundButton1,
			SpriteName.ScrollHorizontalForegroundButton2,
			SpriteName.ScrollHorizontalForegroundButton3,
			SpriteName.ScrollHorizontalBar,
			SpriteName.ScrollHorizontalBar
		};
		public static List<SpriteName> HorizontalSliderBar = new List<SpriteName>()
		{
			SpriteName.ScrollLeftBackgroundButton,
			SpriteName.ScrollLeftForegroundButton,
			SpriteName.ScrollRightBackgroundButton,
			SpriteName.ScrollRightForegroundButton,
			SpriteName.SliderHorizontalBackgroundButton1,
			SpriteName.SliderHorizontalBackgroundButton2,
			SpriteName.SliderHorizontalBackgroundButton3,
			SpriteName.SliderHorizontalForegroundButton1,
			SpriteName.SliderHorizontalForegroundButton2,
			SpriteName.SliderHorizontalForegroundButton3,
			SpriteName.SliderHorizontalBar,
			SpriteName.SliderHighlightedHorizontalBar
		};
		public static List<SpriteName> ComboBox = new List<SpriteName>()
		{
			SpriteName.ButtonBGTL,
			SpriteName.ButtonBGTC,
			SpriteName.ButtonBGTR,
			SpriteName.ButtonBGCL,
			SpriteName.ButtonBGCC,
			SpriteName.ButtonBGCR,
			SpriteName.ButtonBGBL,
			SpriteName.ButtonBGBC,
			SpriteName.ButtonBGBR,
			SpriteName.ButtonFGTL,
			SpriteName.ButtonFGTC,
			SpriteName.ButtonFGTR,
			SpriteName.ButtonFGCL,
			SpriteName.ButtonFGCC,
			SpriteName.ButtonFGCR,
			SpriteName.ButtonFGBL,
			SpriteName.ButtonFGBC,
			SpriteName.ButtonFGBR,
			SpriteName.ScrollUpBackgroundButton,
			SpriteName.ScrollUpForegroundButton,
			SpriteName.ScrollDownBackgroundButton,
			SpriteName.ScrollDownForegroundButton,
			SpriteName.ScrollVerticalBackgroundButton1,
			SpriteName.ScrollVerticalBackgroundButton2,
			SpriteName.ScrollVerticalBackgroundButton3,
			SpriteName.ScrollVerticalForegroundButton1,
			SpriteName.ScrollVerticalForegroundButton2,
			SpriteName.ScrollVerticalForegroundButton3,
			SpriteName.ScrollVerticalBar,
		};
		public static List<SpriteName> SmallComboBox = new List<SpriteName>()
		{
			SpriteName.IconButtonUL,
			SpriteName.IconButtonUC,
			SpriteName.IconButtonUR,
			SpriteName.IconButtonCL,
			SpriteName.IconButtonCC,
			SpriteName.IconButtonCR,
			SpriteName.IconButtonBL,
			SpriteName.IconButtonBC,
			SpriteName.IconButtonBR,
			SpriteName.IconButtonULHL,
			SpriteName.IconButtonUCHL,
			SpriteName.IconButtonURHL,
			SpriteName.IconButtonCLHL,
			SpriteName.IconButtonCCHL,
			SpriteName.IconButtonCRHL,
			SpriteName.IconButtonBLHL,
			SpriteName.IconButtonBCHL,
			SpriteName.IconButtonBRHL,
			SpriteName.ScrollUpBackgroundButton,
			SpriteName.ScrollUpForegroundButton,
			SpriteName.ScrollDownBackgroundButton,
			SpriteName.ScrollDownForegroundButton,
			SpriteName.ScrollVerticalBackgroundButton1,
			SpriteName.ScrollVerticalBackgroundButton2,
			SpriteName.ScrollVerticalBackgroundButton3,
			SpriteName.ScrollVerticalForegroundButton1,
			SpriteName.ScrollVerticalForegroundButton2,
			SpriteName.ScrollVerticalForegroundButton3,
			SpriteName.ScrollVerticalBar,
		};
		public static List<SpriteName> ButtonBackground = new List<SpriteName>()
		{
			SpriteName.ButtonBGTL,
			SpriteName.ButtonBGTC,
			SpriteName.ButtonBGTR,
			SpriteName.ButtonBGCL,
			SpriteName.ButtonBGCC,
			SpriteName.ButtonBGCR,
			SpriteName.ButtonBGBL,
			SpriteName.ButtonBGBC,
			SpriteName.ButtonBGBR
		};
		public static List<SpriteName> ButtonForeground = new List<SpriteName>()
		{
			SpriteName.ButtonFGTL,
			SpriteName.ButtonFGTC,
			SpriteName.ButtonFGTR,
			SpriteName.ButtonFGCL,
			SpriteName.ButtonFGCC,
			SpriteName.ButtonFGCR,
			SpriteName.ButtonFGBL,
			SpriteName.ButtonFGBC,
			SpriteName.ButtonFGBR
		};
		public static List<SpriteName> TinyButtonBackground = new List<SpriteName>()
		{
			SpriteName.TinyButtonBGTL,
			SpriteName.TinyButtonBGTC,
			SpriteName.TinyButtonBGTR,
			SpriteName.TinyButtonBGCL,
			SpriteName.TinyButtonBGCC,
			SpriteName.TinyButtonBGCR,
			SpriteName.TinyButtonBGBL,
			SpriteName.TinyButtonBGBC,
			SpriteName.TinyButtonBGBR,
		};
		public static List<SpriteName> TinyButtonForeground = new List<SpriteName>()
		{
			SpriteName.TinyButtonFGTL,
			SpriteName.TinyButtonFGTC,
			SpriteName.TinyButtonFGTR,
			SpriteName.TinyButtonFGCL,
			SpriteName.TinyButtonFGCC,
			SpriteName.TinyButtonFGCR,
			SpriteName.TinyButtonFGBL,
			SpriteName.TinyButtonFGBC,
			SpriteName.TinyButtonFGBR,
		};
		public static List<SpriteName> BoxBorder = new List<SpriteName>
		{
			SpriteName.BoxUL,
			SpriteName.BoxUC,
			SpriteName.BoxUR,
			SpriteName.BoxCL,
			SpriteName.BoxCC,
			SpriteName.BoxCR,
			SpriteName.BoxBL,
			SpriteName.BoxBC,
			SpriteName.BoxBR,
		};
		public static List<SpriteName> BoxBorderBG = new List<SpriteName>
		{
			SpriteName.BoxULT,
			SpriteName.BoxUCT,
			SpriteName.BoxURT,
			SpriteName.BoxCLT,
			SpriteName.BoxCCT,
			SpriteName.BoxCRT,
			SpriteName.BoxBLT,
			SpriteName.BoxBCT,
			SpriteName.BoxBRT,
		};
		public static List<SpriteName> BoxBorderFG = new List<SpriteName>
		{
			SpriteName.BoxULTHL,
			SpriteName.BoxUCTHL,
			SpriteName.BoxURTHL,
			SpriteName.BoxCLTHL,
			SpriteName.BoxCCTHL,
			SpriteName.BoxCRTHL,
			SpriteName.BoxBLTHL,
			SpriteName.BoxBCTHL,
			SpriteName.BoxBRTHL,
		};
		public static List<SpriteName> BorderBorder = new List<SpriteName>
		{
			SpriteName.BorderUL,
			SpriteName.BorderUC,
			SpriteName.BorderUR,
			SpriteName.BorderCL,
			SpriteName.BorderCC,
			SpriteName.BorderCR,
			SpriteName.BorderBL,
			SpriteName.BorderBC,
			SpriteName.BorderBR,
		};
		public static List<SpriteName> BorderBorderBG = new List<SpriteName>
		{
			SpriteName.BorderULT,
			SpriteName.BorderUCT,
			SpriteName.BorderURT,
			SpriteName.BorderCLT,
			SpriteName.BorderCCT,
			SpriteName.BorderCRT,
			SpriteName.BorderBLT,
			SpriteName.BorderBCT,
			SpriteName.BorderBRT,
		};
		public static List<SpriteName> ScreenBorder = new List<SpriteName>
		{
			SpriteName.ScreenUL,
			SpriteName.ScreenUC,
			SpriteName.ScreenUR,
			SpriteName.ScreenCL,
			SpriteName.ScreenCC,
			SpriteName.ScreenCR,
			SpriteName.ScreenBL,
			SpriteName.ScreenBC,
			SpriteName.ScreenBR,
		};
		public static List<SpriteName> IconButtonBG = new List<SpriteName>
		{
			SpriteName.IconButtonUL,
			SpriteName.IconButtonUC,
			SpriteName.IconButtonUR,
			SpriteName.IconButtonCL,
			SpriteName.IconButtonCC,
			SpriteName.IconButtonCR,
			SpriteName.IconButtonBL,
			SpriteName.IconButtonBC,
			SpriteName.IconButtonBR,
		};
		public static List<SpriteName> IconButtonFG = new List<SpriteName>
		{
			SpriteName.IconButtonULHL,
			SpriteName.IconButtonUCHL,
			SpriteName.IconButtonURHL,
			SpriteName.IconButtonCLHL,
			SpriteName.IconButtonCCHL,
			SpriteName.IconButtonCRHL,
			SpriteName.IconButtonBLHL,
			SpriteName.IconButtonBCHL,
			SpriteName.IconButtonBRHL,
		};
		public static List<SpriteName> CheckBox = new List<SpriteName>
		{
			SpriteName.CBUBG,
			SpriteName.CBUFG,
			SpriteName.CBCBG,
			SpriteName.CBCFG,
		};
		public static List<SpriteName> RadioButton = new List<SpriteName>
		{
			SpriteName.RBUBG,
			SpriteName.RBUFG,
			SpriteName.RBCBG,
			SpriteName.RBCFG,
		};
		public static List<SpriteName> TextBox = new List<SpriteName>()
		{
			SpriteName.TextTL,
			SpriteName.TextTC,
			SpriteName.TextTR,
			SpriteName.TextCL,
			SpriteName.TextCC,
			SpriteName.TextCR,
			SpriteName.TextBL,
			SpriteName.TextBC,
			SpriteName.TextBR
		};
		#endregion

		#region Member Variables
		private Dictionary<SpriteName, Sprite> sprites;
		public static Dictionary<string, Font> fonts;
		private static RenderImage image;
		private static RenderImage backBuffer;
		#endregion

		#region Constructors
		public DrawingManagement()
		{
			GorgonLibrary.Graphics.ImageCache.DestroyAll();
			GorgonLibrary.Graphics.FontCache.DestroyAll();
			GorgonLibrary.Graphics.RenderTargetCache.DestroyAll();
			GorgonLibrary.Graphics.ShaderCache.DestroyAll();

			sprites = new Dictionary<SpriteName, Sprite>();
			fonts = new Dictionary<string, Font>();

			string reason;
			if (!LoadGlobalSprites(out reason))
			{
				MessageBox.Show(reason);
			}

			if (!AddFont("Arial", "Arial", 10.0f, false, out reason))
			{
				MessageBox.Show(reason);
			}
			if (!AddFontFromFile("Computer", 11, false, Environment.CurrentDirectory + "\\Data\\Default\\mainFont.ttf", out reason))
			{
				MessageBox.Show(reason);
			}
		}
		#endregion

		#region Functions
		public bool LoadGraphics(string directory, out string reason)
		{
			if (sprites.ContainsKey(SpriteName.Main))
			{
				if (!File.Exists(Path.Combine(directory, "Main.png")))
				{
					reason = "File " + Path.Combine(directory, "Main.png") + " not found";
					return false;
				}
				try
				{
					GorgonLibrary.Graphics.ImageCache.DestroyAll();
					sprites.Clear();
					if (!LoadGlobalSprites(out reason))
					{
						return false;
					}
				}
				catch (Exception exception)
				{
					reason = "Exception in loading " + Path.Combine(directory, "Main.png") + "\r\nReason: " + exception.Message;
					return false;
				}
			}

			if (!LoadSpriteFromFile("Main", SpriteName.Main, Path.Combine(directory, "main.png"), false, out reason))
			{
				return false;
			}

			#region Taskbar Buttons
			if (!LoadSpriteFromSprite("GameMenu", SpriteName.GameMenu, SpriteName.Main, 0, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Galaxy", SpriteName.Galaxy, SpriteName.Main, 40, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Diplomacy", SpriteName.Diplomacy, SpriteName.Main, 80, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetList", SpriteName.FleetList, SpriteName.Main, 120, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetsList", SpriteName.PlanetsList, SpriteName.Main, 160, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Design", SpriteName.Design, SpriteName.Main, 200, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Research", SpriteName.Research, SpriteName.Main, 240, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignList", SpriteName.ProductionList, SpriteName.Main, 280, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EOT", SpriteName.EOT, SpriteName.Main, 320, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GameMenuHighlighted", SpriteName.HighlightedGameMenu, SpriteName.Main, 0, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GalaxyHighlighted", SpriteName.HighlightedGalaxy, SpriteName.Main, 40, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DiplomacyHighlighted", SpriteName.HighlightedDiplomacy, SpriteName.Main, 80, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetListHighlighted", SpriteName.HighlightedFleetList, SpriteName.Main, 120, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetsListHighlighted", SpriteName.HighlightedPlanetsList, SpriteName.Main, 160, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignHighlighted", SpriteName.HighlightedDesign, SpriteName.Main, 200, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ResearchHighlighted", SpriteName.HighlightedResearch, SpriteName.Main, 240, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DesignListHighlighted", SpriteName.HighlightedDesignList, SpriteName.Main, 280, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EOTHighlighted", SpriteName.HighlightedEOT, SpriteName.Main, 320, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Blank", SpriteName.Blank, SpriteName.Main, 360, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("HighlightedBlank", SpriteName.HighlightedBlank, SpriteName.Main, 360, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Galaxy Components
			if (!LoadSpriteFromSprite("SelectedSystem", SpriteName.SelectedStar, SpriteName.Main, 256, 256, 64, 64, false, false, out reason))
			{
				return false;
			}
			GetSprite(SpriteName.SelectedStar).SetAxis(32f, 32f);
			if (!LoadSpriteFromSprite("FleetDest1", SpriteName.FleetDest1, SpriteName.Main, 256, 320, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetDest2", SpriteName.FleetDest2, SpriteName.Main, 288, 320, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetDest3", SpriteName.FleetDest3, SpriteName.Main, 320, 320, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FleetDest4", SpriteName.FleetDest4, SpriteName.Main, 352, 320, 32, 32, false, false, out reason))
			{
				return false;
			}
			GetSprite(SpriteName.FleetDest1).SetAxis(16f, 16f);
			GetSprite(SpriteName.FleetDest2).SetAxis(16f, 16f);
			GetSprite(SpriteName.FleetDest3).SetAxis(16f, 16f);
			GetSprite(SpriteName.FleetDest4).SetAxis(16f, 16f);
			if (!LoadSpriteFromSprite("BGStar1", SpriteName.BGStar1, SpriteName.Main, 208, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar2", SpriteName.BGStar2, SpriteName.Main, 224, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar3", SpriteName.BGStar3, SpriteName.Main, 240, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar4", SpriteName.BGStar4, SpriteName.Main, 256, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar5", SpriteName.BGStar5, SpriteName.Main, 272, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar6", SpriteName.BGStar6, SpriteName.Main, 288, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BGStar7", SpriteName.BGStar7, SpriteName.Main, 304, 128, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Star", SpriteName.Star, SpriteName.Main, 32, 192, 64, 64, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BlackHole", SpriteName.BlackHole, SpriteName.Main, 160, 288, 64, 64, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Fleet", SpriteName.Fleet, SpriteName.Main, 96, 160, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Transport", SpriteName.Transport, SpriteName.Main, 0, 160, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SystemShips", SpriteName.SystemShips, SpriteName.Main, 128, 192, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SelectedFleet", SpriteName.SelectedFleet, SpriteName.Main, 160, 160, 96, 96, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Interface Components
			if (!LoadSpriteFromSprite("ScrollUpBackgroundButton", SpriteName.ScrollUpBackgroundButton, SpriteName.Main, 528, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollUpForegroundButton", SpriteName.ScrollUpForegroundButton, SpriteName.Main, 544, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBar", SpriteName.ScrollVerticalBar, SpriteName.Main, 528, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollDownBackgroundButton", SpriteName.ScrollDownBackgroundButton, SpriteName.Main, 528, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollDownForegroundButton", SpriteName.ScrollDownForegroundButton, SpriteName.Main, 544, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBackgroundButton1", SpriteName.ScrollVerticalBackgroundButton1, SpriteName.Main, 528, 96, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalForegroundButton1", SpriteName.ScrollVerticalForegroundButton1, SpriteName.Main, 544, 96, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBackgroundButton2", SpriteName.ScrollVerticalBackgroundButton2, SpriteName.Main, 528, 103, 16, 2, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalForegroundButton2", SpriteName.ScrollVerticalForegroundButton2, SpriteName.Main, 544, 103, 16, 2, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalBackgroundButton3", SpriteName.ScrollVerticalBackgroundButton3, SpriteName.Main, 528, 105, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollVerticalForegroundButton3", SpriteName.ScrollVerticalForegroundButton3, SpriteName.Main, 544, 105, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollLeftBackgroundButton", SpriteName.ScrollLeftBackgroundButton, SpriteName.Main, 560, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollLeftForegroundButton", SpriteName.ScrollLeftForegroundButton, SpriteName.Main, 576, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBar", SpriteName.ScrollHorizontalBar, SpriteName.Main, 560, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollRightBackgroundButton", SpriteName.ScrollRightBackgroundButton, SpriteName.Main, 560, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollRightForegroundButton", SpriteName.ScrollRightForegroundButton, SpriteName.Main, 576, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBackgroundButton1", SpriteName.ScrollHorizontalBackgroundButton1, SpriteName.Main, 560, 96, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalForegroundButton1", SpriteName.ScrollHorizontalForegroundButton1, SpriteName.Main, 576, 96, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBackgroundButton2", SpriteName.ScrollHorizontalBackgroundButton2, SpriteName.Main, 567, 96, 2, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalForegroundButton2", SpriteName.ScrollHorizontalForegroundButton2, SpriteName.Main, 583, 96, 2, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalBackgroundButton3", SpriteName.ScrollHorizontalBackgroundButton3, SpriteName.Main, 569, 96, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScrollHorizontalForegroundButton3", SpriteName.ScrollHorizontalForegroundButton3, SpriteName.Main, 585, 96, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelBackground", SpriteName.CancelBackground, SpriteName.Main, 528, 112, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelForeground", SpriteName.CancelForeground, SpriteName.Main, 528, 128, 16, 16, true, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlusBackground", SpriteName.PlusBackground, SpriteName.Main, 592, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlusForeground", SpriteName.PlusForeground, SpriteName.Main, 592, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MinusBackground", SpriteName.MinusBackground, SpriteName.Main, 592, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MinusForeground", SpriteName.MinusForeground, SpriteName.Main, 592, 96, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LockDisabled", SpriteName.LockDisabled, SpriteName.Main, 608, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LockEnabled", SpriteName.LockEnabled, SpriteName.Main, 608, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBar", SpriteName.SliderHorizontalBar, SpriteName.Main, 544, 112, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHighlightedHorizontalBar", SpriteName.SliderHighlightedHorizontalBar, SpriteName.Main, 560, 112, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBackgroundButton1", SpriteName.SliderHorizontalBackgroundButton1, SpriteName.Main, 576, 112, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalForegroundButton1", SpriteName.SliderHorizontalForegroundButton1, SpriteName.Main, 592, 112, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBackgroundButton2", SpriteName.SliderHorizontalBackgroundButton2, SpriteName.Main, 583, 112, 2, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalForegroundButton2", SpriteName.SliderHorizontalForegroundButton2, SpriteName.Main, 599, 112, 2, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalBackgroundButton3", SpriteName.SliderHorizontalBackgroundButton3, SpriteName.Main, 585, 112, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SliderHorizontalForegroundButton3", SpriteName.SliderHorizontalForegroundButton3, SpriteName.Main, 601, 112, 7, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CBCBG", SpriteName.CBCBG, SpriteName.Main, 953, 80, 27, 27, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CBCFG", SpriteName.CBCFG, SpriteName.Main, 953, 50, 27, 27, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CBUBG", SpriteName.CBUBG, SpriteName.Main, 981, 80, 27, 27, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CBUFG", SpriteName.CBUFG, SpriteName.Main, 981, 50, 27, 27, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RBCBG", SpriteName.RBCBG, SpriteName.Main, 893, 129, 19, 19, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RBCFG", SpriteName.RBCFG, SpriteName.Main, 893, 109, 19, 19, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RBUBG", SpriteName.RBUBG, SpriteName.Main, 914, 129, 19, 19, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RBUFG", SpriteName.RBUFG, SpriteName.Main, 914, 109, 19, 19, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TransportButtonBG", SpriteName.TransferButtonBG, SpriteName.Main, 368, 196, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TransportButtonFG", SpriteName.TransferButtonFG, SpriteName.Main, 368, 161, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("OrbitButtonBG", SpriteName.OrbitButtonBG, SpriteName.Main, 292, 196, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("OrbitButtonFG", SpriteName.OrbitButtonFG, SpriteName.Main, 292, 161, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NewSquadronButtonBG", SpriteName.NewSquadronButtonBG, SpriteName.Main, 65, 405, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NewSquadronButtonFG", SpriteName.NewSquadronButtonFG, SpriteName.Main, 65, 370, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelMovementBG", SpriteName.CancelMovementBG, SpriteName.Main, 368, 267, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CancelMovementFG", SpriteName.CancelMovementFG, SpriteName.Main, 368, 232, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonUL", SpriteName.IconButtonUL, SpriteName.Main, 448, 568, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonUC", SpriteName.IconButtonUC, SpriteName.Main, 459, 568, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonUR", SpriteName.IconButtonUR, SpriteName.Main, 461, 568, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCL", SpriteName.IconButtonCL, SpriteName.Main, 448, 579, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCC", SpriteName.IconButtonCC, SpriteName.Main, 459, 579, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCR", SpriteName.IconButtonCR, SpriteName.Main, 461, 579, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBL", SpriteName.IconButtonBL, SpriteName.Main, 448, 581, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBC", SpriteName.IconButtonBC, SpriteName.Main, 459, 581, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBR", SpriteName.IconButtonBR, SpriteName.Main, 461, 581, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonULHL", SpriteName.IconButtonULHL, SpriteName.Main, 448, 544, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonUCHL", SpriteName.IconButtonUCHL, SpriteName.Main, 459, 544, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonURHL", SpriteName.IconButtonURHL, SpriteName.Main, 461, 544, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCLHL", SpriteName.IconButtonCLHL, SpriteName.Main, 448, 555, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCCHL", SpriteName.IconButtonCCHL, SpriteName.Main, 459, 555, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonCRHL", SpriteName.IconButtonCRHL, SpriteName.Main, 461, 555, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBLHL", SpriteName.IconButtonBLHL, SpriteName.Main, 448, 557, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBCHL", SpriteName.IconButtonBCHL, SpriteName.Main, 459, 557, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IconButtonBRHL", SpriteName.IconButtonBRHL, SpriteName.Main, 461, 557, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EquipmentOK", SpriteName.EquipmentOK, SpriteName.Main, 415, 463, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EquipmentOKHL", SpriteName.EquipmentOKHL, SpriteName.Main, 415, 383, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EquipmentCancel", SpriteName.EquipmentCancel, SpriteName.Main, 415, 503, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EquipmentCancelHL", SpriteName.EquipmentCancelHL, SpriteName.Main, 415, 423, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddEquipment", SpriteName.AddEquipment, SpriteName.Main, 253, 423, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddEquipmentHL", SpriteName.AddEquipmentHL, SpriteName.Main, 334, 423, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ColonizeButtonBG", SpriteName.ColonizeButtonBG, SpriteName.Main, 1, 239, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ColonizeButtonBG", SpriteName.ColonizeButtonFG, SpriteName.Main, 1, 204, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetDoneButtonBG", SpriteName.PlanetDoneButtonBG, SpriteName.Main, 77, 239, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetDoneButtonFG", SpriteName.PlanetDoneButtonFG, SpriteName.Main, 77, 204, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LandTroopButtonBG", SpriteName.LandTroopButtonBG, SpriteName.Main, 153, 239, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LandTroopButtonFG", SpriteName.LandTroopButtonFG, SpriteName.Main, 153, 204, 75, 35, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ApplyButtonBG", SpriteName.ApplyButtonBG, SpriteName.Main, 410, 343, 85, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ApplyButtonFG", SpriteName.ApplyButtonFG, SpriteName.Main, 410, 303, 85, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddProjectBG", SpriteName.AddProjectBG, SpriteName.Main, 0, 81, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddProjectFG", SpriteName.AddProjectFG, SpriteName.Main, 81, 81, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddProjectsBG", SpriteName.AddProjectsBG, SpriteName.Main, 0, 122, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AddProjectsFG", SpriteName.AddProjectsFG, SpriteName.Main, 81, 122, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DoneProjectBG", SpriteName.DoneProjectBG, SpriteName.Main, 0, 163, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DoneProjectFG", SpriteName.DoneProjectFG, SpriteName.Main, 81, 163, 80, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGTL", SpriteName.TinyButtonBGTL, SpriteName.Main, 65, 275, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGTC", SpriteName.TinyButtonBGTC, SpriteName.Main, 76, 275, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGTR", SpriteName.TinyButtonBGTR, SpriteName.Main, 78, 275, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGCL", SpriteName.TinyButtonBGCL, SpriteName.Main, 65, 286, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGCC", SpriteName.TinyButtonBGCC, SpriteName.Main, 76, 286, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGCR", SpriteName.TinyButtonBGCR, SpriteName.Main, 78, 286, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGBL", SpriteName.TinyButtonBGBL, SpriteName.Main, 65, 288, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGBC", SpriteName.TinyButtonBGBC, SpriteName.Main, 76, 288, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonBGBR", SpriteName.TinyButtonBGBR, SpriteName.Main, 78, 288, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGTL", SpriteName.TinyButtonFGTL, SpriteName.Main, 89, 275, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGTC", SpriteName.TinyButtonFGTC, SpriteName.Main, 100, 275, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGTR", SpriteName.TinyButtonFGTR, SpriteName.Main, 102, 275, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGCL", SpriteName.TinyButtonFGCL, SpriteName.Main, 89, 286, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGCC", SpriteName.TinyButtonFGCC, SpriteName.Main, 100, 286, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGCR", SpriteName.TinyButtonFGCR, SpriteName.Main, 102, 286, 10, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGBL", SpriteName.TinyButtonFGBL, SpriteName.Main, 89, 288, 10, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGBC", SpriteName.TinyButtonFGBC, SpriteName.Main, 100, 288, 1, 10, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TinyButtonFGBR", SpriteName.TinyButtonFGBR, SpriteName.Main, 102, 288, 10, 10, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Icons
			if (!LoadSpriteFromSprite("HumanPlayerIcon", SpriteName.HumanPlayerIcon, SpriteName.Main, 480, 0, 25, 25, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CPUPlayerIcon", SpriteName.CPUPlayerIcon, SpriteName.Main, 505, 0, 25, 25, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("BeamIcon", SpriteName.BeamIcon, SpriteName.Main, 560, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ParticleIcon", SpriteName.ProjectileIcon, SpriteName.Main, 580, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MissileIcon", SpriteName.MissileIcon, SpriteName.Main, 600, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TorpedoIcon", SpriteName.TorpedoIcon, SpriteName.Main, 620, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BombIcon", SpriteName.BombIcon, SpriteName.Main, 640, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShieldIcon", SpriteName.ShieldIcon, SpriteName.Main, 660, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ArmorIcon", SpriteName.ArmorIcon, SpriteName.Main, 680, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SystemEngineIcon", SpriteName.SystemEngineIcon, SpriteName.Main, 314, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("StellarEngineIcon", SpriteName.StellarEngineIcon, SpriteName.Main, 334, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PowerIcon", SpriteName.PowerIcon, SpriteName.Main, 354, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SpecialIcon", SpriteName.SpecialIcon, SpriteName.Main, 374, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TimeIcon", SpriteName.TimeIcon, SpriteName.Main, 394, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetResearchIcon", SpriteName.PlanetResearchIcon, SpriteName.Main, 254, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TradedResearchIcon", SpriteName.TradedResearchIcon, SpriteName.Main, 274, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TotalResearchIcon", SpriteName.TotalResearchIcon, SpriteName.Main, 294, 463, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CapacityIcon", SpriteName.CapacityIcon, SpriteName.Main, 448, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ComputerIcon", SpriteName.ComputerIcon, SpriteName.Main, 720, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("InfrastructureIcon", SpriteName.InfrastructureIcon, SpriteName.Main, 740, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShockwaveIcon", SpriteName.ShockwaveIcon, SpriteName.Main, 760, 28, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AccuracyIcon", SpriteName.AccuracyIcon, SpriteName.Main, 560, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("DamageIcon", SpriteName.DamageIcon, SpriteName.Main, 580, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MoneyIcon", SpriteName.MoneyIcon, SpriteName.Main, 600, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("SpaceIcon", SpriteName.SpaceIcon, SpriteName.Main, 620, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("MountIcon", SpriteName.MountIcon, SpriteName.Main, 640, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("GalaxyMovementIcon", SpriteName.GalaxyMovementIcon, SpriteName.Main, 660, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CombatMovementIcon", SpriteName.CombatMovementIcon, SpriteName.Main, 680, 0, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("AgricultureIcon", SpriteName.AgricultureIcon, SpriteName.Main, 480, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("EnvironmentIcon", SpriteName.EnvironmentIcon, SpriteName.Main, 496, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("CommerceIcon", SpriteName.CommerceIcon, SpriteName.Main, 512, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ResearchIcon", SpriteName.ResearchIcon, SpriteName.Main, 528, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ConstructionIcon", SpriteName.ConstructionIcon, SpriteName.Main, 544, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Spy", SpriteName.Spy, SpriteName.Main, 480, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Security", SpriteName.Security, SpriteName.Main, 496, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Relation", SpriteName.Relation, SpriteName.Main, 512, 48, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus1", SpriteName.PlanetConstructionBonus1, SpriteName.Main, 480, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus2", SpriteName.PlanetConstructionBonus2, SpriteName.Main, 480, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus3", SpriteName.PlanetConstructionBonus3, SpriteName.Main, 480, 96, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetConstructionBonus4", SpriteName.PlanetConstructionBonus4, SpriteName.Main, 480, 112, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus1", SpriteName.PlanetEnvironmentBonus1, SpriteName.Main, 496, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus2", SpriteName.PlanetEnvironmentBonus2, SpriteName.Main, 496, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus3", SpriteName.PlanetEnvironmentBonus3, SpriteName.Main, 496, 96, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEnvironmentBonus4", SpriteName.PlanetEnvironmentBonus4, SpriteName.Main, 496, 112, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus1", SpriteName.PlanetEntertainmentBonus1, SpriteName.Main, 512, 64, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus2", SpriteName.PlanetEntertainmentBonus2, SpriteName.Main, 512, 80, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus3", SpriteName.PlanetEntertainmentBonus3, SpriteName.Main, 512, 96, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PlanetEntertainmentBonus4", SpriteName.PlanetEntertainmentBonus4, SpriteName.Main, 512, 112, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Stargate1", SpriteName.Stargate1, SpriteName.Main, 256, 352, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Stargate2", SpriteName.Stargate2, SpriteName.Main, 288, 352, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Stargate3", SpriteName.Stargate3, SpriteName.Main, 320, 352, 32, 32, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("Stargate4", SpriteName.Stargate4, SpriteName.Main, 352, 352, 32, 32, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Planets
			if (!LoadSpriteFromSprite("RegionRing", SpriteName.RegionRing, SpriteName.Main, 0, 369, 60, 60, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Stretchable Images and Borders
			if (!LoadSpriteFromSprite("textUpLeft", SpriteName.TextTL, SpriteName.Main, 828, 109, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textUp", SpriteName.TextTC, SpriteName.Main, 859, 109, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textUpRight", SpriteName.TextTR, SpriteName.Main, 861, 109, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textLeft", SpriteName.TextCL, SpriteName.Main, 828, 123, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textCenter", SpriteName.TextCC, SpriteName.Main, 859, 123, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textRight", SpriteName.TextCR, SpriteName.Main, 861, 123, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textDownLeft", SpriteName.TextBL, SpriteName.Main, 828, 125, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textDown", SpriteName.TextBC, SpriteName.Main, 859, 125, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("textDownRight", SpriteName.TextBR, SpriteName.Main, 861, 125, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGUpLeft", SpriteName.ButtonBGTL, SpriteName.Main, 828, 79, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGUp", SpriteName.ButtonBGTC, SpriteName.Main, 889, 79, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGUpRight", SpriteName.ButtonBGTR, SpriteName.Main, 891, 79, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGCenterLeft", SpriteName.ButtonBGCL, SpriteName.Main, 828, 93, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGCenter", SpriteName.ButtonBGCC, SpriteName.Main, 889, 93, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGCenterRight", SpriteName.ButtonBGCR, SpriteName.Main, 891, 93, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGDownLeft", SpriteName.ButtonBGBL, SpriteName.Main, 828, 95, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGDown", SpriteName.ButtonBGBC, SpriteName.Main, 889, 95, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonBGDownRight", SpriteName.ButtonBGBR, SpriteName.Main, 891, 95, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGUpLeft", SpriteName.ButtonFGTL, SpriteName.Main, 828, 49, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGUp", SpriteName.ButtonFGTC, SpriteName.Main, 889, 49, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGUpRight", SpriteName.ButtonFGTR, SpriteName.Main, 891, 49, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGCenterLeft", SpriteName.ButtonFGCL, SpriteName.Main, 828, 63, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGCenter", SpriteName.ButtonFGCC, SpriteName.Main, 889, 63, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGCenterRight", SpriteName.ButtonFGCR, SpriteName.Main, 891, 63, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGDownLeft", SpriteName.ButtonFGBL, SpriteName.Main, 828, 65, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGDown", SpriteName.ButtonFGBC, SpriteName.Main, 889, 65, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("buttonFGDownRight", SpriteName.ButtonFGBR, SpriteName.Main, 891, 65, 60, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxUL", SpriteName.BoxUL, SpriteName.Main, 640, 49, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxUC", SpriteName.BoxUC, SpriteName.Main, 671, 49, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxUR", SpriteName.BoxUR, SpriteName.Main, 673, 49, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCL", SpriteName.BoxCL, SpriteName.Main, 640, 63, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCC", SpriteName.BoxCC, SpriteName.Main, 671, 63, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCR", SpriteName.BoxCR, SpriteName.Main, 673, 63, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBL", SpriteName.BoxBL, SpriteName.Main, 640, 65, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBC", SpriteName.BoxBC, SpriteName.Main, 671, 65, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBR", SpriteName.BoxBR, SpriteName.Main, 673, 65, 30, 13, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("BoxULT", SpriteName.BoxULT, SpriteName.Main, 640, 79, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxUCT", SpriteName.BoxUCT, SpriteName.Main, 671, 79, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxURT", SpriteName.BoxURT, SpriteName.Main, 673, 79, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCLT", SpriteName.BoxCLT, SpriteName.Main, 640, 93, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCCT", SpriteName.BoxCCT, SpriteName.Main, 671, 93, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCRT", SpriteName.BoxCRT, SpriteName.Main, 673, 93, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBLT", SpriteName.BoxBLT, SpriteName.Main, 640, 95, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBCT", SpriteName.BoxBCT, SpriteName.Main, 671, 95, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBRT", SpriteName.BoxBRT, SpriteName.Main, 673, 95, 30, 13, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("BoxULTHL", SpriteName.BoxULTHL, SpriteName.Main, 828, 139, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxUCTHL", SpriteName.BoxUCTHL, SpriteName.Main, 859, 139, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxURTHL", SpriteName.BoxURTHL, SpriteName.Main, 861, 139, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCLTHL", SpriteName.BoxCLTHL, SpriteName.Main, 828, 153, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCCTHL", SpriteName.BoxCCTHL, SpriteName.Main, 859, 153, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxCRTHL", SpriteName.BoxCRTHL, SpriteName.Main, 861, 153, 30, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBLTHL", SpriteName.BoxBLTHL, SpriteName.Main, 828, 155, 30, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBCTHL", SpriteName.BoxBCTHL, SpriteName.Main, 859, 155, 1, 13, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BoxBRTHL", SpriteName.BoxBRTHL, SpriteName.Main, 861, 155, 30, 13, false, false, out reason))
			{
				return false;
			}

			if (!LoadSpriteFromSprite("BorderUL", SpriteName.BorderUL, SpriteName.Main, 704, 49, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderUC", SpriteName.BorderUC, SpriteName.Main, 765, 49, 1, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderUR", SpriteName.BorderUR, SpriteName.Main, 767, 49, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCL", SpriteName.BorderCL, SpriteName.Main, 704, 110, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCC", SpriteName.BorderCC, SpriteName.Main, 765, 110, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCR", SpriteName.BorderCR, SpriteName.Main, 767, 110, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBL", SpriteName.BorderBL, SpriteName.Main, 704, 112, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBC", SpriteName.BorderBC, SpriteName.Main, 765, 112, 1, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBR", SpriteName.BorderBR, SpriteName.Main, 767, 112, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderULT", SpriteName.BorderULT, SpriteName.Main, 497, 176, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderUCT", SpriteName.BorderUCT, SpriteName.Main, 558, 176, 1, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderURT", SpriteName.BorderURT, SpriteName.Main, 560, 176, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCLT", SpriteName.BorderCLT, SpriteName.Main, 497, 237, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCCT", SpriteName.BorderCCT, SpriteName.Main, 558, 237, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderCRT", SpriteName.BorderCRT, SpriteName.Main, 560, 237, 60, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBLT", SpriteName.BorderBLT, SpriteName.Main, 497, 239, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBCT", SpriteName.BorderBCT, SpriteName.Main, 558, 239, 1, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BorderBRT", SpriteName.BorderBRT, SpriteName.Main, 560, 239, 60, 60, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenUL", SpriteName.ScreenUL, SpriteName.Main, 621, 176, 200, 200, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenUC", SpriteName.ScreenUC, SpriteName.Main, 822, 176, 1, 200, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenUR", SpriteName.ScreenUR, SpriteName.Main, 824, 176, 200, 200, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenCL", SpriteName.ScreenCL, SpriteName.Main, 621, 377, 200, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenCC", SpriteName.ScreenCC, SpriteName.Main, 822, 377, 1, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenCR", SpriteName.ScreenCR, SpriteName.Main, 824, 377, 200, 1, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenDL", SpriteName.ScreenBL, SpriteName.Main, 621, 379, 200, 200, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenDC", SpriteName.ScreenBC, SpriteName.Main, 822, 379, 1, 200, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ScreenDR", SpriteName.ScreenBR, SpriteName.Main, 824, 379, 200, 200, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Diplomacy Items
			if (!LoadSpriteFromSprite("RelationBar", SpriteName.RelationBar, SpriteName.Main, 368, 144, 200, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("RelationSlider", SpriteName.RelationSlider, SpriteName.Main, 644, 144, 20, 20, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("OutgoingMessageBackground", SpriteName.OutgoingMessageBackground, SpriteName.Main, 440, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("OutgoingMessageForeground", SpriteName.OutgoingMessageForeground, SpriteName.Main, 440, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IncomingMessageBackground", SpriteName.IncomingMessageBackground, SpriteName.Main, 400, 40, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("IncomingMessageForeground", SpriteName.IncomingMessageForeground, SpriteName.Main, 400, 0, 40, 40, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Tutorial Buttons
			if (!LoadSpriteFromSprite("PrevPageBG", SpriteName.PrevPageBG, SpriteName.Main, 17, 339, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FirstPageBG", SpriteName.FirstPageBG, SpriteName.Main, 48, 339, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LastPageBG", SpriteName.LastPageBG, SpriteName.Main, 79, 339, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NextPageBG", SpriteName.NextPageBG, SpriteName.Main, 110, 339, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("PrevPageFG", SpriteName.PrevPageFG, SpriteName.Main, 17, 308, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("FirstPageFG", SpriteName.FirstPageFG, SpriteName.Main, 48, 308, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("LastPageFG", SpriteName.LastPageFG, SpriteName.Main, 79, 308, 30, 30, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("NextPageFG", SpriteName.NextPageFG, SpriteName.Main, 110, 308, 30, 30, false, false, out reason))
			{
				return false;
			}
			#endregion

			#region Battle Items
			if (!LoadSpriteFromSprite("BattleBackground", SpriteName.BattleBackground, SpriteName.Main, 0, 594, 900, 175, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleAutoBG", SpriteName.BattleAutoBG, SpriteName.Main, 0, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleRetreatBG", SpriteName.BattleRetreatBG, SpriteName.Main, 40, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattlePrevShipBG", SpriteName.BattlePrevShipBG, SpriteName.Main, 80, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattlePrevShipDoneBG", SpriteName.BattlePrevShipDoneBG, SpriteName.Main, 120, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextShipDoneBG", SpriteName.BattleNextShipDoneBG, SpriteName.Main, 160, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextShipBG", SpriteName.BattleNextShipBG, SpriteName.Main, 200, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextTurnBG", SpriteName.BattleNextTurnBG, SpriteName.Main, 240, 809, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleAutoFG", SpriteName.BattleAutoFG, SpriteName.Main, 0, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleRetreatFG", SpriteName.BattleRetreatFG, SpriteName.Main, 40, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattlePrevShipFG", SpriteName.BattlePrevShipFG, SpriteName.Main, 80, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattlePrevShipDoneFG", SpriteName.BattlePrevShipDoneFG, SpriteName.Main, 120, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextShipDoneFG", SpriteName.BattleNextShipDoneFG, SpriteName.Main, 160, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextShipFG", SpriteName.BattleNextShipFG, SpriteName.Main, 200, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("BattleNextTurnFG", SpriteName.BattleNextTurnFG, SpriteName.Main, 240, 769, 40, 40, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelectionTL", SpriteName.ShipSelectionTL, SpriteName.Main, 0, 275, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelectionTR", SpriteName.ShipSelectionTR, SpriteName.Main, 16, 275, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelectionBL", SpriteName.ShipSelectionBL, SpriteName.Main, 0, 291, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelectionBR", SpriteName.ShipSelectionBR, SpriteName.Main, 16, 291, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipMove", SpriteName.ShipMove, SpriteName.Main, 32, 275, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipInvalidMove", SpriteName.ShipInvalidMove, SpriteName.Main, 48, 275, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ShipSelectionBG", SpriteName.ShipSelectionBG, SpriteName.Main, 0, 308, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("TargetReticle", SpriteName.TargetReticle, SpriteName.Main, 32, 291, 16, 16, false, false, out reason))
			{
				return false;
			}
			
			#endregion

			return true;
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
		
		private bool LoadGlobalSprites(out string reason)
		{
			//Load from file
			if (!LoadSpriteFromFile("InterfaceArt", SpriteName.Interface, Environment.CurrentDirectory + "\\Sprites\\InterfaceArt.png", true, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitlePlanet", SpriteName.TitlePlanet, Environment.CurrentDirectory + "\\Sprites\\Title planet.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitleName", SpriteName.TitleName, Environment.CurrentDirectory + "\\Sprites\\Title name.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("TitleNebula", SpriteName.TitleNebula, Environment.CurrentDirectory + "\\Sprites\\Title nebula.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CONTINUE", SpriteName.Continue, Environment.CurrentDirectory + "\\Sprites\\bot CONTINUE.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CUSTOMIZE", SpriteName.Customize, Environment.CurrentDirectory + "\\Sprites\\bot CUSTOMIZE.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("EXIT", SpriteName.Exit, Environment.CurrentDirectory + "\\Sprites\\bot EXIT.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("load game", SpriteName.LoadGame, Environment.CurrentDirectory + "\\Sprites\\bot load game.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("new game", SpriteName.NewGame, Environment.CurrentDirectory + "\\Sprites\\bot new game.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("OPTIONS", SpriteName.Options, Environment.CurrentDirectory + "\\Sprites\\bot OPTIONS.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CONTINUE2", SpriteName.Continue2, Environment.CurrentDirectory + "\\Sprites\\bot CONTINUE2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("CUSTOMIZE2", SpriteName.Customize2, Environment.CurrentDirectory + "\\Sprites\\bot CUSTOMIZE2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("EXIT2", SpriteName.Exit2, Environment.CurrentDirectory + "\\Sprites\\bot EXIT2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("load game2", SpriteName.LoadGame2, Environment.CurrentDirectory + "\\Sprites\\bot load game2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("new game2", SpriteName.NewGame2, Environment.CurrentDirectory + "\\Sprites\\bot new game2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("OPTIONS2", SpriteName.Options2, Environment.CurrentDirectory + "\\Sprites\\bot OPTIONS2.png", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("MOD", SpriteName.Mod, Environment.CurrentDirectory + "\\Sprites\\drop full 2.PNG", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("MOD2", SpriteName.Mod2, Environment.CurrentDirectory + "\\Sprites\\drop full.PNG", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("MODDown", SpriteName.ModDown, Environment.CurrentDirectory + "\\Sprites\\down.PNG", false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromFile("ModMain", SpriteName.ModMain, Environment.CurrentDirectory + "\\Sprites\\scroll.png", true, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollUpBackgroundButton", SpriteName.ModScrollUp, SpriteName.ModMain, 0, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollUpForegroundButton", SpriteName.ModScrollUpHighlighted, SpriteName.ModMain, 16, 0, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalBar", SpriteName.ModScrollBar, SpriteName.ModMain, 0, 16, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollDownBackgroundButton", SpriteName.ModScrollDown, SpriteName.ModMain, 0, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollDownForegroundButton", SpriteName.ModScrollDownHighlighted, SpriteName.ModMain, 16, 32, 16, 16, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalBackgroundButton1", SpriteName.ModScrollButton1, SpriteName.ModMain, 0, 48, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalForegroundButton1", SpriteName.ModScrollButton1Highlighted, SpriteName.ModMain, 16, 48, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalBackgroundButton2", SpriteName.ModScrollButton2, SpriteName.ModMain, 0, 55, 16, 2, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalForegroundButton2", SpriteName.ModScrollButton2Highlighted, SpriteName.ModMain, 16, 55, 16, 2, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalBackgroundButton3", SpriteName.ModScrollButton3, SpriteName.ModMain, 0, 57, 16, 7, false, false, out reason))
			{
				return false;
			}
			if (!LoadSpriteFromSprite("ModScrollVerticalForegroundButton3", SpriteName.ModScrollButton3Highlighted, SpriteName.ModMain, 16, 57, 16, 7, false, false, out reason))
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
			if (!LoadSpriteFromFile("Screen", SpriteName.Screen, Environment.CurrentDirectory + "\\Sprites\\Screen.png", false, out reason))
			{
				return false;
			}
			/*if (!LoadSpriteFromSprite("ControlBackground", SpriteName.ControlBackground, SpriteName.Interface, 0, 80, 175, 200, true, false, out reason))
			{
				return false;
			}*/
			
			/*if (!LoadSpriteFromSprite("RelationSlider", SpriteName.RelationSlider, SpriteName.BarSlider, 200, 0, 16, 16, false, false, out reason))
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
			}*/

			return true;
		}

		private static bool AddFont(string name, string fontFamily, float size, bool bold, out string reason)
		{
			if (fonts.ContainsKey(name))
			{
				reason = "Key " + name + " already exists";
				return false;
			}
			reason = null;
			try
			{
				Font font = new Font(name, fontFamily, size, true, bold);
				fonts.Add(name, font);
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			return true;
		}
		private static bool AddFontFromFile(string name, float size, bool bold, string filePath, out string reason)
		{
			if (fonts.ContainsKey(name))
			{
				reason = "Key " + name + " already exists";
				return false;
			}
			reason = null;
			try
			{
				Font font = GorgonLibrary.Graphics.Font.FromFile(filePath, size, false, bold, false, false);
				fonts.Add(name, font);
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			return true;
		}
		#endregion

		#region Public Functions
		public static RenderImage GetSpriteWithShader(Sprite spriteToDraw, Shader shader, List<string> keys, List<float[]> values)
		{
			if (image == null)
			{
				image = new GorgonLibrary.Graphics.RenderImage("imageWithShaders", (int)spriteToDraw.Width, (int)spriteToDraw.Height, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
				image.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;
			}
			if (backBuffer == null)
			{
				backBuffer = new GorgonLibrary.Graphics.RenderImage("backBufferWithShaders", (int)spriteToDraw.Width, (int)spriteToDraw.Height, GorgonLibrary.Graphics.ImageBufferFormats.BufferRGB888A8);
				backBuffer.BlendingMode = GorgonLibrary.Graphics.BlendingModes.Modulated;
			}
			image.Clear();
			backBuffer.Clear();
			image.SetDimensions((int)spriteToDraw.Width, (int)spriteToDraw.Height);
			backBuffer.SetDimensions((int)spriteToDraw.Width, (int)spriteToDraw.Height);
			RenderTarget oldTarget = Gorgon.CurrentRenderTarget;
			Gorgon.CurrentRenderTarget = image;
			spriteToDraw.SetPosition(0, 0);
			spriteToDraw.Draw();

			Gorgon.CurrentRenderTarget = backBuffer;
			Gorgon.CurrentShader = shader;
			for (int i = 0; i < keys.Count; i++)
			{
				shader.Parameters[keys[i]].SetValue(values[i]);
			}
			image.Blit(0, 0);
			Gorgon.CurrentShader = null;
			Gorgon.CurrentRenderTarget = oldTarget;
			return backBuffer;
		}

		public void DrawSprite(SpriteName spriteName, int x, int y)
		{
			DrawSprite(spriteName, x, y, 255, Drawing.Color.White);
		}
		public void DrawSprite(SpriteName spriteName, int x, int y, byte alpha)
		{
			DrawSprite(spriteName, x, y, alpha, Drawing.Color.White);
		}
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
		public void DrawSprite(SpriteName spriteName, int x, int y, float width, float height)
		{
			DrawSprite(spriteName, x, y, 255, width, height, System.Drawing.Color.White);
		}
		public void DrawSprite(SpriteName spriteName, int x, int y, float width, float height, byte alpha)
		{
			DrawSprite(spriteName, x, y, alpha, width, height, System.Drawing.Color.White);
		}
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
		public void SetSpriteAxis(SpriteName spriteName, float x, float y)
		{
			sprites[spriteName].SetAxis(x, y);
		}
		public Sprite GetSprite(SpriteName spriteName)
		{
			return sprites[spriteName];
		}

		public void DrawLine(int x1, int y1, int x2, int y2, System.Drawing.Color color)
		{
			Gorgon.Screen.Line(x1, y1, x2, y2, color);
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

		public static Font GetFont(string name)
		{
			Font font = null;
			fonts.TryGetValue(name, out font);
			return font;
		}
		#endregion
		#endregion
	}
}
