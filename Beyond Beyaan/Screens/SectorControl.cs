using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class SectorControl
	{
		private SectorObject sector;
		public int Height { get; private set; }

		private StretchableImage background;
		//private List<StretchButton> stretchButtons;
		private SingleLineTextBox sectorName;
		private StretchButton actionButton;

		public SectorControl(SectorObject sector, GameMain gameMain)
		{
			this.sector = sector;
			background = new StretchableImage(0, 0, 290, 200, 30, 13, DrawingManagement.BoxBorder);
			sectorName = new SingleLineTextBox(0, 0, 220, 35, DrawingManagement.TextBox);
			actionButton = new StretchButton(DrawingManagement.IconButtonBG, DrawingManagement.IconButtonFG, "No Current Project", 0, 0, 270, 35, 10, 10);

			/*switch (sector.Type)
			{
				case SECTORTYPE.PLANET:
					{
						Height = 150 + sector.Planet.Regions.Count * 35;
						background.SetDimensions(290, Height);
						stretchButtons = new List<StretchButton>();
						for (int i = 0; i < sector.Planet.Regions.Count; i++)
						{
							stretchButtons.Add(new StretchButton(DrawingManagement.IconButtonBG, DrawingManagement.IconButtonFG, sector.Planet.Regions[i].RegionType.RegionTypeName, 10, 50 + i * 35, 270, 35, 10, 10));
						}
						sectorName.SetString(sector.Name);
						if (sector.Owner != gameMain.empireManager.CurrentEmpire)
						{
							actionButton.Active = false;
						}
					} break;
			}*/
		}

		public void Draw(DrawingManagement drawingManagement, int x, int y)
		{
			background.MoveTo(x, y);
			background.Draw(drawingManagement);

			/*switch (sector.Type)
			{
				case SECTORTYPE.PLANET:
					DrawPlanetControl(drawingManagement, x, y);
					break;
				case SECTORTYPE.EMPTY:
					DrawEmptyControl(drawingManagement, x, y);
					break;
			}*/
			DrawEmptyControl(drawingManagement, x, y);
		}

		private void DrawPlanetControl(DrawingManagement drawingManagement, int x, int y)
		{
			//sector.Planet.PlanetType.Sprite.SetPosition(x + 10, y + 10);
			//sector.Planet.PlanetType.Sprite.Draw();

			sectorName.MoveTo(x + 60, y + 13);
			sectorName.Draw(drawingManagement);

			/*for (int i = 0; i < stretchButtons.Count; i++)
			{
				stretchButtons[i].MoveTo(x + 10, y + 90 + i * 35);
				stretchButtons[i].Draw(drawingManagement);
			}*/

			actionButton.MoveTo(x + 10, y + Height - 50);
			actionButton.Draw(drawingManagement);
		}

		private void DrawEmptyControl(DrawingManagement drawingManagement, int x, int y)
		{

		}
	}
}
