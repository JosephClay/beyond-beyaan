using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class SectorControl
	{
		private Sector sector;

		public SectorControl(Sector sector)
		{
			this.sector = sector;
		}

		public void Draw(DrawingManagement drawingManagement, int x, int y)
		{
			switch (sector.SectorType)
			{
				case SECTORTYPE.PLANET:
					DrawPlanetControl(drawingManagement, x, y);
					break;
				case SECTORTYPE.EMPTY:
					DrawEmptyControl(drawingManagement, x, y);
					break;
			}
		}

		private void DrawPlanetControl(DrawingManagement drawingManagement, int x, int y)
		{

		}

		private void DrawEmptyControl(DrawingManagement drawingManagement, int x, int y)
		{

		}
	}
}
